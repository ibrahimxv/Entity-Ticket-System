using ConsoleApp27.DbContext;
using ConsoleApp27.Entity;
using ConsoleApp27.Exceptions;
using ConsoleApp27.Services;

await using var context = new TicketDbContext();

var eventService = new EventService(context);
var ticketService = new TicketService(context);

while (true)
{
    Console.WriteLine("\n===== EVENT MANAGEMENT =====");
    Console.WriteLine("1. Create Event");
    Console.WriteLine("2. Get All Events");
    Console.WriteLine("3. Get Event By ID");
    Console.WriteLine("4. Update Event");
    Console.WriteLine("5. Delete Event");
    Console.WriteLine("6. Buy Ticket");
    Console.WriteLine("7. Get All Tickets");
    Console.WriteLine("8. Use Ticket");
    Console.WriteLine("0. Exit");
    Console.Write("Choose: ");

    if (!int.TryParse(Console.ReadLine(), out int choice))
    {
        Console.WriteLine("Enter a valid number.");
        continue;
    }

    try
    {
        switch (choice)
        {
            case 1:
                {
                    Console.Write("Event name: ");
                    string name = Console.ReadLine() ?? "";

                    Console.Write("Event date (yyyy-MM-dd HH:mm): ");
                    if (!DateTime.TryParse(Console.ReadLine(), out DateTime date))
                    {
                        Console.WriteLine("Invalid date.");
                        break;
                    }

                    Console.Write("Ticket price: ");
                    if (!decimal.TryParse(Console.ReadLine(), out decimal price)
                        || price < 0)
                    {
                        Console.WriteLine("Invalid price.");
                        break;
                    }

                    var newEvent = new Event
                    {
                        Name = name,
                        Date = date,
                        Price = price
                    };

                    await eventService.CreateAsync(newEvent);
                    Console.WriteLine("Event created successfully.");
                    break;
                }

            case 2:
                {
                    var events = await eventService.GetAllAsync();

                    foreach (var e in events)
                        Console.WriteLine(e);

                    if (events.Count == 0)
                        Console.WriteLine("No events found.");

                    break;
                }

            case 3:
                {
                    Console.Write("Event ID: ");
                    if (!int.TryParse(Console.ReadLine(), out int id))
                        break;

                    var e = await eventService.GetByIdAsync(id);

                    if (e == null)
                        Console.WriteLine("Event not found.");
                    else
                        Console.WriteLine(e);

                    break;
                }

            case 4:
                {
                    Console.Write("Event ID: ");
                    if (!int.TryParse(Console.ReadLine(), out int id))
                        break;

                    Console.Write("New name: ");
                    string name = Console.ReadLine() ?? "";

                    Console.Write("New date (yyyy-MM-dd HH:mm): ");
                    if (!DateTime.TryParse(Console.ReadLine(), out DateTime date))
                        break;

                    Console.Write("New price: ");
                    if (!decimal.TryParse(Console.ReadLine(), out decimal price))
                        break;

                    var updatedEvent = new Event
                    {
                        Name = name,
                        Date = date,
                        Price = price
                    };

                    bool updated = await eventService.UpdateAsync(id, updatedEvent);

                    Console.WriteLine(updated
                        ? "Event updated successfully."
                        : "Event not found.");

                    break;
                }

            case 5:
                {
                    Console.Write("Event ID: ");
                    if (!int.TryParse(Console.ReadLine(), out int id))
                        break;

                    bool deleted = await eventService.DeleteAsync(id);

                    Console.WriteLine(deleted
                        ? "Event deleted successfully."
                        : "Event not found.");

                    break;
                }

            case 6:
                {
                    var events = await eventService.GetAllAsync();

                    var upcomingEvents = events
                        .Where(e => e.Date > DateTime.Now)
                        .OrderBy(e => e.Date)
                        .ToList();

                    if (upcomingEvents.Count == 0)
                    {
                        Console.WriteLine("No upcoming events.");
                        break;
                    }

                    foreach (var e in upcomingEvents)
                        Console.WriteLine(e);

                    Console.Write("Event ID to buy a ticket: ");
                    if (!int.TryParse(Console.ReadLine(), out int eventId))
                        break;

                    var ticket = await ticketService.BuyTicketAsync(eventId);
                    Console.WriteLine($"Ticket purchased: {ticket}");

                    break;
                }

            case 7:
                {
                    var tickets = await ticketService.GetAllTicketsAsync();

                    foreach (var ticket in tickets)
                        Console.WriteLine(
                            $"{ticket} | Event: {ticket.Event.Name} | Used: {ticket.IsUsed}");

                    if (tickets.Count == 0)
                        Console.WriteLine("No tickets found.");

                    break;
                }

            case 8:
                {
                    Console.Write("Ticket ID: ");
                    if (!int.TryParse(Console.ReadLine(), out int ticketId))
                        break;

                    await ticketService.UseTicketAsync(ticketId);
                    Console.WriteLine("Ticket used successfully.");

                    break;
                }

            case 0:
                return;

            default:
                Console.WriteLine("Invalid choice.");
                break;
        }
    }
    catch (EventExpiredException ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
    catch (TicketAlreadyUsedException ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}