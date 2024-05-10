using System.Data.SqlClient;
using WebApp.Models;

namespace WebApp.Repositories;

public class ProductRepository : IProductRepository
{
    private IConfiguration _configuration;

    public ProductRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Product? FetchProductById(int id)
    {
        using var connection = new SqlConnection(
            _configuration.GetConnectionString("DefaultConnection")
        );
        connection.Open();

        var command = new SqlCommand($"SELECT * FROM Product WHERE IdProduct = {id}", connection);
        //TO DO: Co jeśli nie znajdzie produktu o podanym Id w bazie? Tutaj wywalamy błąd?
        using var reader = command.ExecuteReader();
        var product = new Product();

        if (!reader.Read()) return null;
        product.IdProduct = (int)reader["IdProduct"];
        product.Name = reader["Name"].ToString()!;
        product.Description = reader["Description"].ToString()!;
        product.Price = (decimal)reader["Price"];

        return product;

    }
    
    public decimal FetchPriceById(int id)
    {
        using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        connection.Open();

        using var command = new SqlCommand($"SELECT Price FROM Product WHERE IdProduct = {id}", connection);
        using var reader = command.ExecuteReader();
        if (!reader.Read()) return 0;
        return (decimal)reader["Price"];
    }
}