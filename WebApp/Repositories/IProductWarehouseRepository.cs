using WebApp.DTO;
using WebApp.Models;

namespace WebApp.Repositories;

public interface IProductWarehouseRepository
{
    bool OrderHasBennCompleted(int idOrder);
    
    int? AddProductToWarehouse(ProductWarehouse productWarehouse);
}