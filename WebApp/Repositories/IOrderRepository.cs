using WebApp.Models;

namespace WebApp.Repositories;

public interface IOrderRepository
{
    public Task <Order?> FetchOrderByProductAmountDateAsync(int idProduct, int amount, DateTime createdAt);
    
    public Task<bool> UpdateOrderFulfilledAtAsync(int orderId, DateTime fulfilledAt);
}