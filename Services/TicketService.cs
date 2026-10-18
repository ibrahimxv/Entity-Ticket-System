using ConsoleApp27.DbContext;
using ConsoleApp27.Entity;
using ConsoleApp27.Exceptions;
using ConsoleApp27.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ConsoleApp27.Services;
public class TicketService : ITicketService
{
    private readonly TicketDbContext _context;

    public TicketService(TicketDbContext context)
    {
        _context = context;
    }

    public async Task<Ticket> BuyTicketAsync(int eventId)
    {
        var selectedEvent = await _context.Events
            .FirstOrDefaultAsync(e => e.Id == eventId);

        if (selectedEvent == null)
            throw new ArgumentException("Event not found.");

        if (selectedEvent.Date <= DateTime.Now)
            throw new EventExpiredException();

        var ticket = new Ticket
        {
            Price = selectedEvent.Price,
            EventId = selectedEvent.Id,
            IsUsed = false
        };

        _context.Tickets.Add(ticket);
        await _context.SaveChangesAsync();

        return ticket;
    }

    public async Task<Ticket> BuyTicketFromInputAsync()
    {
        while (true)
        {
            var events = await _context.Events
                .Where(e => e.Date <= DateTime.Now)
                .OrderBy(e => e.Date)
                .ToListAsync();

            if (events.Count == 0)
                throw new InvalidOperationException("There is no past event.");

            foreach(var e in events)
                Console.WriteLine($"ID: {e.Id} | Name: {e.Name} | Price: {e.Price} AZN");

            Console.Write("ID of the event for which you want to purchase a ticket: ");

            if (!int.TryParse(Console.ReadLine(), out int eventId))
            {
                Console.WriteLine("ID must be a number.");
                continue;
            }

            if (!events.Any(e => e.Id == eventId))
            {
                Console.WriteLine("This ID does not exist. Enter it again.");
                continue;
            }

            return await BuyTicketAsync(eventId);
        }
    }

    public async Task<List<Ticket>> GetAllTicketsAsync()
    {
        return await _context.Tickets
            .Include(t => t.Event)
            .ToListAsync();
    }

    public async Task UseTicketAsync(int ticketid)
    {
        var ticket = await _context.Tickets
            .FirstOrDefaultAsync(t => t.Id == ticketid);

        if (ticket == null)
            throw new ArgumentException("Ticket not found");

        if (ticket.IsUsed)
        {
            throw new TicketAlreadyUsedException();
        }

        ticket.IsUsed = true;

        await _context.SaveChangesAsync();
    }
}
