using ConsoleApp27.Entity;

namespace ConsoleApp27.Interfaces;

public interface IEventService
{
    Task<Event> GetByIdAsync(int id);
    Task<List<Event>> GetAllAsync();
    Task CreateAsync (Event @event);
    Task<bool> UpdateAsync(int id, Event updatedEvent);
    Task<bool> DeleteAsync(int id);
}
