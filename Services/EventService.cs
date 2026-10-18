using ConsoleApp27.DbContext;
using ConsoleApp27.Entity;
using ConsoleApp27.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp27.Services;

public class EventService : IEventService
{
    private readonly TicketDbContext _context;

    public EventService(TicketDbContext context)
    {
        _context = context;
    }
    public async Task CreateAsync(Event @event)
    {
        _context.Events.Add(@event);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _context.Events.FindAsync(id);

        if (existing == null)
            return false;

        _context.Events.Remove(existing);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<List<Event>> GetAllAsync()
    {
        return await _context.Events
            .Include(e => e.Ticket)
            .ToListAsync();
    }

    public async Task<Event> GetByIdAsync(int id)
    {
        return await _context.Events
            .Include(e => e.Ticket)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<bool> UpdateAsync(int id, Event updatedEvent)
    {
        var existing = await _context.Events.FindAsync(id);

        if (existing == null)
            return false;

        if (string.IsNullOrWhiteSpace(updatedEvent.Name))
            throw new ArgumentException("Event name can't be null.");

        if (updatedEvent.Price < 0)
            throw new ArgumentException("Price is must be positive.");

        existing.Name = updatedEvent.Name;
        existing.Date = updatedEvent.Date;
        existing.Price = updatedEvent.Price;

        await _context.SaveChangesAsync();
        return true;
    }
}
