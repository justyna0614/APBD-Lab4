using WebApp.DTO;
using WebApp.Models;

namespace WebApp.Services;

public interface IWarehouseService
{
    public Task <ProductWarehouse?> AddStock(RegisterProductInWarehouseRequestDTO request);
}