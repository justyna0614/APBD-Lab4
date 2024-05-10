using System.Data.SqlClient;
using WebApp.DTO;
using WebApp.Models;

namespace WebApp.Repositories;

public class OrderRepository : IOrderRepository
{
    private IConfiguration _configuration;

    public OrderRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public bool ExsistOrderOfProduct(RegisterProductInWarehouseRequestDTO request)
    {
        using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText =
            "SELECT * FROM [Order] WHERE IdProduct = @productId AND Amount = @amount AND CreatedAt < @createdAt";
        command.Parameters.AddWithValue("@productId", request.IdProduct); // Czy tak jest poprawnie?
        command.Parameters.AddWithValue("@amount", request.Amount); // Czy tak jest poprawnie?
        command.Parameters.AddWithValue("@createdAt", request.CreatedAt); // Czy tak jest poprawnie?

        using var reader = command.ExecuteReader();
        return reader.HasRows;
    }

    public Order? FetchOrderByProductAmountDate(int idProduct, int amount, DateTime createdAt)
    {
        using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        connection.Open();


        using var command = new SqlCommand(
            "SELECT * FROM [Order] WHERE IdProduct = @productId AND Amount = @amount AND CreatedAt < @createdAt AND FulfilledAt IS NULL",
            connection);
        command.Parameters.AddWithValue("@productId", idProduct); // Czy tak jest poprawnie?
        command.Parameters.AddWithValue("@amount", amount); // Czy tak jest poprawnie?
        command.Parameters.AddWithValue("@createdAt", createdAt); // Czy tak jest poprawnie?

        using var reader = command.ExecuteReader();
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

    public Order? FetchOrderById(int idOrder)
    {
        using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        connection.Open();
    
        var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM [Order] WHERE IdOrder = @idOrder";
        command.Parameters.AddWithValue("@idOrder", idOrder);
    
        using var reader = command.ExecuteReader();
        var order = new Order();
    
        if (!reader.Read()) return null;
    
        while (reader.Read())
        {
            order.IdOrder = (int)reader["IdOrder"];
            order.IdProduct = (int)reader["IdProduct"];
            order.Amount = (int)reader["Amount"];
            order.CreatedAt = (DateTime)reader["CreatedAt"];
            order.FulfilledAt = (DateTime)reader["FulfilledAt"];
        }
    
        return order;
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