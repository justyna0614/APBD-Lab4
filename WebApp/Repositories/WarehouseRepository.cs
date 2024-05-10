using System.Data.SqlClient;

namespace WebApp.Repositories;

public class WarehouseRepository : IWarehouseRepository
{
    private readonly IConfiguration _configuration;

    public WarehouseRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public bool ExistById(int id)
    {
        using var connection = new SqlConnection(
            _configuration.GetConnectionString("DefaultConnection")
        );
        connection.Open();

        using var command = new SqlCommand($"SELECT * FROM Warehouse WHERE IdWarehouse = {id}", connection);
        var warehouse = command.ExecuteReader();

        return warehouse.HasRows;
    }
}