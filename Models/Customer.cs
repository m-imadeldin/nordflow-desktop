namespace NordFlow.Models;

public class Customer
{
    public int Id { get; set; }           // Primary Key (auto-generated)
    public string Name { get; set; } = ""; // Customer name
    public string Email { get; set; } = ""; // Customer email (optional)
}