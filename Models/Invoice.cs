namespace NordFlow.Models;

public class Invoice
{
    public int Id { get; set; }           // Primary Key
    
    public int CustomerId { get; set; }   // Foreign Key to Customer
    public Customer? Customer { get; set; } // Navigation property (nullable)
    
    public decimal Amount { get; set; }   // Invoice amount
    public DateTime Date { get; set; } = DateTime.Now; // Creation date
    public string Status { get; set; } = "Unpaid";     // Unpaid or Paid
}