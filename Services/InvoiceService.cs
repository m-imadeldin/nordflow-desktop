using NordFlow.Models;
using System.Collections.ObjectModel;

namespace NordFlow.Services
{
    public class InvoiceService
    {
        public ObservableCollection<Invoice> Invoices { get; } = new();

        public void AddInvoice(Invoice invoice)
        {
            invoice.Id = Invoices.Count + 1;
            invoice.Date = DateTime.Now;
            invoice.Status = "Unpaid";
            Invoices.Add(invoice);
        }

        public void RemoveInvoice(Invoice invoice)
        {
            Invoices.Remove(invoice);
        }

        public void PayInvoice(Invoice invoice)
        {
            invoice.Status = "Paid";
        }
    }
}