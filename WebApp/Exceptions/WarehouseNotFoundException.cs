namespace WebApp.Exceptions;

public class WarehouseNotFoundException : Exception
{
    public WarehouseNotFoundException(string message) : base(message)
    {
    }
}