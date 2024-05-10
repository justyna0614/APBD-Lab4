namespace WebApp.Exceptions;

public class OrderCompletedException : Exception
{
    public OrderCompletedException(string message) : base(message)
    {
    }
}