using System.Data.SqlClient;
using WebApp.Models;

namespace WebApp.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly IConfiguration _configuration;

    public ProductRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task <Product?> FetchProductByIdAsync(int id)
    {
         await using var connection = new SqlConnection(
            _configuration.GetConnectionString("DefaultConnection")
        );
        await connection.OpenAsync();

        await using var command = new SqlCommand($"SELECT * FROM Product WHERE IdProduct = {id}", connection);
        await using var reader = await command.ExecuteReaderAsync();
        var product = new Product();

        if (!await reader.ReadAsync()) return null;
        product.IdProduct = (int)reader["IdProduct"];
        product.Name = reader["Name"].ToString()!;
        product.Description = reader["Description"].ToString()!;
        product.Price = (decimal)reader["Price"];

        return product;

    }
    
    public async Task <decimal> FetchPriceByIdAsync(int id)
    {
        await using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        await connection.OpenAsync();

        await using var command = new SqlCommand($"SELECT Price FROM Product WHERE IdProduct = {id}", connection);
        await using var reader = await command.ExecuteReaderAsync();
        if (!await reader.ReadAsync()) return 0;
        return (decimal)reader["Price"];
    }
}