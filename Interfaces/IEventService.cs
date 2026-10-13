using ConsoleApp27.Entity;

namespace ConsoleApp27.Interfaces;

public interface IEventService
{
    Task<Event> GetByIdAsync(int id);
    Task<List<Event>> GetAllAsync();
    Task CreateAsync (Event @event);
    Task UpdateAsync(Event @event);
    Task DeleteAsync(int id);
}
