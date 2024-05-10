using WebApp.Models;

namespace WebApp.Repositories;

public interface IProductRepository
{
    public Task <Product?> FetchProductByIdAsync(int id);
    
    public Task <decimal> FetchPriceByIdAsync(int id);
}