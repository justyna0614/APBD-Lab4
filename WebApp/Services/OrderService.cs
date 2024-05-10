using WebApp.Repositories;

namespace WebApp.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductWarehouseRepository _productWarehouseRepository;


    public OrderService(IOrderRepository orderRepository, IProductWarehouseRepository productWarehouseRepository)
    {
        _orderRepository = orderRepository;
        _productWarehouseRepository = productWarehouseRepository;
    }

    public void UpdateFulfilledDate(int idOrder)
    {
        if (!_productWarehouseRepository.OrderHasBennCompleted(idOrder))
        {
            _orderRepository.UpdateOrderFulfilledAt(idOrder, DateTime.Now);
        }
    }
}