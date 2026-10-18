namespace ConsoleApp27.Exceptions;

public class TicketAlreadyUsedException : Exception
{
    public TicketAlreadyUsedException()
    {
        
    }
    public TicketAlreadyUsedException (string message) 
        : base ("This ticket already used.")
    {
    }
}
