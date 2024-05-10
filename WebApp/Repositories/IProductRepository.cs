using WebApp.Models;

namespace WebApp.Repositories;

public interface IProductRepository
{
    Product? FetchProductById(int id);
    
    decimal FetchPriceById(int id);
}