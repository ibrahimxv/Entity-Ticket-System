namespace ConsoleApp27.DbContext;

using ConsoleApp27.Entity;
using Microsoft.EntityFrameworkCore;

public class TicketDbContext : DbContext
{
    override protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlServer("Server=TITAN006\\SQLEXPRESS;Database=TicketDb;trusted_connection=true;integrated security=true;TrustServerCertificate=true;");
    }
    public DbSet<Ticket> Tickets { get; set; } = null!;
    public DbSet<Event> Events { get; set; } = null!;
}
