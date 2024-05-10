namespace WebApp.Exceptions;

public class IncorectAmountException : Exception
{
    public IncorectAmountException(string message) : base(message)
    {
    }
}