using System.Data.SqlClient;

namespace WebApp.Repositories;

public class WarehouseRepository : IWarehouseRepository
{
    private readonly IConfiguration _configuration;

    public WarehouseRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<bool> ExistById(int id)
    {
        await using var connection = new SqlConnection(
            _configuration.GetConnectionString("DefaultConnection")
        );
        await connection.OpenAsync();

        await using var command = new SqlCommand($"SELECT * FROM Warehouse WHERE IdWarehouse = {id}", connection);
        await using var warehouse = await command.ExecuteReaderAsync();

        return warehouse.HasRows;
    }
}