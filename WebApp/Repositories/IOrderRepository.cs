using WebApp.DTO;
using WebApp.Models;

namespace WebApp.Repositories;

public interface IOrderRepository
{
    bool ExsistOrderOfProduct(RegisterProductInWarehouseRequestDTO request);
    Order? FetchOrderByProductAmountDate(int idProduct, int amount, DateTime createdAt);
    Order? FetchOrderById(int idOrder);
    
    bool UpdateOrderFulfilledAt(int orderId, DateTime fulfilledAt);
}