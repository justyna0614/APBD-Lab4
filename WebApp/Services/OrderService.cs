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

    public async void UpdateFulfilledDate(int idOrder)
    {
        if (!await _productWarehouseRepository.OrderHasBennCompletedAsync(idOrder))
        {
            await _orderRepository.UpdateOrderFulfilledAtAsync(idOrder, DateTime.Now);
        }
    }
}