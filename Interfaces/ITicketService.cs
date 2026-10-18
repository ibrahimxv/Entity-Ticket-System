using ConsoleApp27.Entity;

namespace ConsoleApp27.Interfaces;

public interface ITicketService
{
    Task<Ticket> BuyTicketAsync(int eventId);
    Task<Ticket> BuyTicketFromInputAsync();
    Task UseTicketAsync(int ticketid);
    Task<List<Ticket>> GetAllTicketsAsync();
}
