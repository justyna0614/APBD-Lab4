using System.Data.SqlClient;
using WebApp.DTO;
using WebApp.Models;

namespace WebApp.Repositories;

public class ProductWarehouseRepository : IProductWarehouseRepository
{
    private readonly IConfiguration _configuration;


    public ProductWarehouseRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    public async Task <bool> OrderHasBennCompletedAsync(int idOrder)
    {
        await using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        await connection.OpenAsync();

        await using var command = new SqlCommand($"SELECT * FROM Product_Warehouse WHERE IdOrder = {idOrder}", connection);

        await using var reader = await command.ExecuteReaderAsync();
        return reader.HasRows;
    }


    public async Task <int?> AddProductToWarehouseAsync(ProductWarehouse productWarehouse)
    {
        await using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        await connection.OpenAsync();

        await using var command =
            new SqlCommand("INSERT INTO Product_Warehouse (IdWarehouse, IdProduct, IdOrder, Amount, Price, CreatedAt) " +
                           "VALUES (@IdWarehouse, @IdProduct, @IdOrder, @Amount, @Price, @CreatedAt); SELECT SCOPE_IDENTITY()", connection);
        command.Parameters.AddWithValue("@Amount", productWarehouse.Amount);
        command.Parameters.AddWithValue("@Price", productWarehouse.Price);
        command.Parameters.AddWithValue("@IdProduct", productWarehouse.IdProduct);
        command.Parameters.AddWithValue("@IdWarehouse", productWarehouse.IdWarehouse);
        command.Parameters.AddWithValue("@IdOrder", productWarehouse.IdOrder);
        command.Parameters.AddWithValue("@CreatedAt", productWarehouse.CreatedAt);

        var rows = await command.ExecuteNonQueryAsync();

        if (rows > 0)
        {
            return Convert.ToInt32(command.ExecuteScalar());
        }


        return null;
    }
}