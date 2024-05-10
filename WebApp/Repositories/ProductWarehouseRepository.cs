using System.Data.SqlClient;
using WebApp.DTO;
using WebApp.Models;

namespace WebApp.Repositories;

public class ProductWarehouseRepository : IProductWarehouseRepository
{
    IConfiguration _configuration;
    private IProductRepository _productRepository;
    private IOrderRepository _orderRepository;

    public ProductWarehouseRepository(IConfiguration configuration, IProductRepository productRepository, IOrderRepository orderRepository)
    {
        _configuration = configuration;
        _productRepository = productRepository;
        _orderRepository = orderRepository;
    }


    public bool OrderHasBennCompleted(int idOrder)
    {
        using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        connection.Open();

        var command = new SqlCommand($"SELECT * FROM Product_Warehouse WHERE IdOrder = {idOrder}", connection);

        using var reader = command.ExecuteReader();
        return reader.HasRows;
    }


    public int? AddProductToWarehouse(ProductWarehouse productWarehouse)
    {
        using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        connection.Open();

        using var command =
            new SqlCommand("INSERT INTO Product_Warehouse (IdWarehouse, IdProduct, IdOrder, Amount, Price, CreatedAt) " +
                           "VALUES (@IdWarehouse, @IdProduct, @IdOrder, @Amount, @Price, @CreatedAt); SELECT SCOPE_IDENTITY()", connection);
        command.Parameters.AddWithValue("@Amount", productWarehouse.Amount);
        command.Parameters.AddWithValue("@Price", productWarehouse.Price);
        command.Parameters.AddWithValue("@IdProduct", productWarehouse.IdProduct);
        command.Parameters.AddWithValue("@IdWarehouse", productWarehouse.IdWarehouse);
        command.Parameters.AddWithValue("@IdOrder", productWarehouse.IdOrder);
        command.Parameters.AddWithValue("@CreatedAt", productWarehouse.CreatedAt);

        var rows = command.ExecuteNonQuery();

        if (rows > 0)
        {
            return Convert.ToInt32(command.ExecuteScalar());
        }


        return null;
    }
}