using ConsoleApp27.DbContext;
using ConsoleApp27.Entity;
using Microsoft.EntityFrameworkCore;

using var context = new TicketDbContext();

var events = await context.Events.ToListAsync();