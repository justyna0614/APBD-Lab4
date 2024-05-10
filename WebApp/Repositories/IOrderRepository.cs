using WebApp.Models;

namespace WebApp.Repositories;

public interface IOrderRepository
{
    public Task <Order?> FetchOrderByProductAmountDateAsync(int idProduct, int amount, DateTime createdAt);
    
    public bool UpdateOrderFulfilledAt(int orderId, DateTime fulfilledAt);
}