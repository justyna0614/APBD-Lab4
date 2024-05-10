using WebApp.Models;

namespace WebApp.Repositories;

public interface IWarehouseRepository
{
    bool ExistById(int id);

}