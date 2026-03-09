using NordFlow.Models;
using System.Collections.ObjectModel;

namespace NordFlow.Services
{
    public class InvoiceService
    {
        public ObservableCollection<Invoice> Invoices { get; set; } = new();

        public void AddInvoice(Invoice invoice)
        {
            invoice.Id = Invoices.Count + 1;
            Invoices.Add(invoice);
        }

        public void RemoveInvoice(Invoice invoice)
        {
            Invoices.Remove(invoice);
        }
    }
}