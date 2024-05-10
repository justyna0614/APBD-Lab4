using WebApp.Models;

namespace WebApp.Repositories;

public interface IProductWarehouseRepository
{
    public Task <bool> OrderHasBennCompletedAsync(int idOrder);
    
    public Task <int?> AddProductToWarehouseAsync(ProductWarehouse productWarehouse);
}