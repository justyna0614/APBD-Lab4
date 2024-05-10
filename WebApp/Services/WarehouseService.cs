using WebApp.DTO;
using WebApp.Exceptions;
using WebApp.Models;
using WebApp.Repositories;

namespace WebApp.Services;

public class WarehouseService : IWarehouseService
{
    private readonly IWarehouseRepository _warehouseRepository;
    private readonly IProductRepository _productRepository;
    private readonly IProductWarehouseRepository _productWarehouseRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderService _orderService;

    public WarehouseService(IWarehouseRepository warehouseRepository, IProductRepository productRepository,
        IProductWarehouseRepository productWarehouseRepository, IOrderRepository orderRepository, IOrderService orderService)
    {
        _warehouseRepository = warehouseRepository;
        _productRepository = productRepository;
        _productWarehouseRepository = productWarehouseRepository;
        _orderRepository = orderRepository;
        _orderService = orderService;
    }

    public async Task <ProductWarehouse?> AddStock(RegisterProductInWarehouseRequestDTO request)
    {
        if (_warehouseRepository.ExistById(request.IdWarehouse) == false)
            throw new WarehouseNotFoundException($"Warehouse with id: {request.IdWarehouse} could not be found");

        if (await _productRepository.FetchProductByIdAsync(request.IdProduct) == null)
            throw new ProductNotFoundException($"Product with id: {request.IdProduct} could not be found");

        if (request.Amount <= 0)
            throw new IncorectAmountException("Amount must be greater than 0");

        var order = await _orderRepository.FetchOrderByProductAmountDateAsync(request.IdProduct, request.Amount, request.CreatedAt);
        if (order == null)
            throw new OrderNotFoundException("Order could not be found");

        if (await _productWarehouseRepository.OrderHasBennCompletedAsync(order.IdOrder))
            throw new OrderCompletedException($"Order with id: {order.IdOrder} has been completed");

        _orderService.UpdateFulfilledDate(order.IdOrder);

        var price = await _productRepository.FetchPriceByIdAsync(request.IdProduct) * request.Amount;

        var warehouseProduct = new ProductWarehouse();
        warehouseProduct.IdProduct = request.IdProduct;
        warehouseProduct.IdWarehouse = request.IdWarehouse;
        warehouseProduct.IdOrder = order.IdOrder;
        warehouseProduct.Amount = request.Amount;
        warehouseProduct.Price = price;
        warehouseProduct.CreatedAt = request.CreatedAt;

        var id = await _productWarehouseRepository.AddProductToWarehouseAsync(warehouseProduct);
        if (id == null)
        {
            return null;
        }

        warehouseProduct.IdProductWarehouse = id.Value;
        return warehouseProduct;
    }

    // public ProductWarehouse? AddStockWithProcedure(RegisterProductInWarehouseRequestDTO request)
    // {
    //     
    // }

}