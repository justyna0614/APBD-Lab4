using WebApp.DTO;
using WebApp.Models;

namespace WebApp.Services;

public interface IWarehouseService
{
    ProductWarehouse? AddStock(RegisterProductInWarehouseRequestDTO request);
}