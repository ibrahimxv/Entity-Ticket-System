namespace ConsoleApp27.Exceptions;

public class EventExpiredException : Exception
{
    public EventExpiredException()
    {
    }

    public EventExpiredException(string message)
        : base("Time for the event has already passed.")
    {
    }
}
