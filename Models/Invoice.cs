namespace NordFlow.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; } = "Unpaid";
        
        public override string ToString() => $"{Customer?.Name ?? "Unknown"} - {Amount:C} ({Status})";
    }
}
