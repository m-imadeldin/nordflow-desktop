// ========================================
// CUSTOMER MODEL - Simple POCO för UI
// ========================================
namespace NordFlow.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        
        public override string ToString() => Name;  // För ListBox display
    }
}