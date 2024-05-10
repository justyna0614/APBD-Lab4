using System.Data.SqlClient;
using WebApp.DTO;
using WebApp.Models;

namespace WebApp.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly IConfiguration _configuration;

    public OrderRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    

    public async Task <Order?> FetchOrderByProductAmountDateAsync(int idProduct, int amount, DateTime createdAt)
    {
        await using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        await connection.OpenAsync();
        
        await using var transaction = await connection.BeginTransactionAsync();



        var query =
            "SELECT * FROM [Order] WHERE IdProduct = @productId AND Amount = @amount AND CreatedAt < @createdAt AND FulfilledAt IS NULL";
        await using var command = new SqlCommand(query, connection);

        command.Transaction = (SqlTransaction)transaction;
        command.Parameters.AddWithValue("@productId", idProduct); // Czy tak jest poprawnie?
        command.Parameters.AddWithValue("@amount", amount); // Czy tak jest poprawnie?
        command.Parameters.AddWithValue("@createdAt", createdAt); // Czy tak jest poprawnie?

        var reader = await command.ExecuteReaderAsync();
        var order = new Order();


        if (reader.Read())
        {
            order.IdOrder = (int)reader["IdOrder"];
            order.IdProduct = (int)reader["IdProduct"];
            order.Amount = (int)reader["Amount"];
            order.CreatedAt = (DateTime)reader["CreatedAt"];

            return order;
        }

        return null;
    }
    

    public bool UpdateOrderFulfilledAt(int orderId, DateTime fulfilledAt)
    {
        using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        connection.Open();

        using var command = new SqlCommand($"UPDATE [Order] SET FulfilledAt = @FulfilledAt WHERE IdOrder = {orderId}", connection);
        command.Parameters.AddWithValue("@FulfilledAt", fulfilledAt);

        var updated = command.ExecuteNonQuery();
        return updated > 0;
    }
}