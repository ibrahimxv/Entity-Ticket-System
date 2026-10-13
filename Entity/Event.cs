namespace ConsoleApp27.Entity;

public class Event
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime Date { get; set; }
    public decimal Price { get; set; }
    public Ticket Ticket { get; set; } = null!;
    public override string ToString()
    {
        return $"Id: {Id}, Name: {Name}, Date: {Date}, Price: {Price}";
    }
}
