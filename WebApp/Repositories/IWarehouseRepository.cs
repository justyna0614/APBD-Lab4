namespace WebApp.Repositories;

public interface IWarehouseRepository
{
    public Task<bool> ExistById(int id);
}