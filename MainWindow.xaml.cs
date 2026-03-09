using System.Collections.ObjectModel;
using NordFlow.Services;
using System.Windows;
using System.Windows.Controls;
using NordFlow.Models;
using Microsoft.VisualBasic;

namespace NordFlow;

/// <summary>
/// MainWindow - Entry point for NordFlow ERP system
/// Handles all UI interactions and business logic coordination
/// </summary>
public partial class MainWindow : Window
{
    // ========================================
    // FIELD SECTION - Private services and state
    // ========================================
    
    // Customer management service - handles all customer data operations
    private readonly CustomerService _customerService = new CustomerService();
    
    // UI-bound customer list - automatically updates ListBox when modified
    private readonly ObservableCollection<Customer> _customers;
    
    // Currently selected customer for editing operations (nullable = can be null)
    private Customer? _selectedCustomer;
    
    // Invoice management service - handles all invoice data operations  
    private readonly InvoiceService _invoiceService;
    
    // Selected invoice for Pay/Delete operations (nullable = can be null)
    private Invoice? _selectedInvoice;
    
    // Temporary state for creating new invoices
    private Customer? _selectedCustomerForInvoice;
    private decimal _newInvoiceAmount;

    // ========================================
    // UI BINDING PROPERTIES - Public properties for XAML data binding
    // ========================================
    
    // Public property for XAML binding to CustomerService
    public CustomerService CustomerService { get; }
    
    // Public property for XAML binding to InvoiceService  
    public InvoiceService InvoiceService { get; }

    // ========================================
    // MAIN WINDOW CONSTRUCTOR - Initialize services and UI bindings
    // ========================================
    
    public MainWindow()
    {
        InitializeComponent();

        // Step 1: Bind customer service to public property
        CustomerService = _customerService;

        // Step 2: Create empty customer collection
        _customers = new ObservableCollection<Customer>();
        CustomerListBox.ItemsSource = _customers;

        // Step 3: Initialize invoice service
        _invoiceService = new InvoiceService();
        InvoiceService = _invoiceService;
        InvoiceListBox.ItemsSource = _invoiceService.Invoices;
    }

    // ========================================
    // CUSTOMER BUTTONS - CRUD operations for customers
    // ========================================
    
    /// <summary>
    /// ADD/EDIT Customer button - Dual purpose: Add new or update selected customer
    /// </summary>
    public void MyButton_Click(object sender, RoutedEventArgs e)
    {
        // Validation: Check if input is empty
        if (string.IsNullOrWhiteSpace(NameInput.Text)) return; 
        
        // EDIT MODE: If customer is selected, update existing customer
        if (_selectedCustomer != null)
        {
            _selectedCustomer.Name = NameInput.Text;  // Update customer name
            CustomerListBox.Items.Refresh();          // Refresh UI display
            NameInput.Text = "";                      // Clear input field
            _selectedCustomer = null;                 // Exit edit mode
            return;
        }
        
        // ADD MODE: Create new customer and add to collection
        var newCustomer = new Customer 
        { 
            Name = NameInput.Text,
            Id = _customers.Count + 1  // Simple auto-increment ID
        };
        _customers.Add(newCustomer);  // UI auto-updates via ObservableCollection
    }
    
    /// <summary>
    /// DELETE Customer button - Removes selected customer from list
    /// </summary>
    public void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        // Get selected customer from ListBox (cast to Customer)
        var selected = CustomerListBox.SelectedItem as Customer;
        if (selected == null) return;  // Nothing selected - exit silently

        _customers.Remove(selected);  // Remove from collection (UI auto-updates)
    }
    
    /// <summary>
    /// EDIT Customer button - Loads selected customer into input field for editing
    /// </summary>
    public void EditButton_Click(object sender, RoutedEventArgs e)
    {
        // Get selected customer from ListBox
        var selected = CustomerListBox.SelectedItem as Customer;
        if (selected == null) return;  // Nothing selected - exit silently
        
        // Load customer data into input field and enter edit mode
        NameInput.Text = selected.Name;
        _selectedCustomer = selected;  // Set edit mode flag
    }

    // ========================================
    // INVOICE BUTTONS - Create, pay, and delete invoices
    // ========================================
    
    /// <summary>
    /// NEW INVOICE button - Creates invoice for selected customer
    /// </summary>
    private void NewInvoice_Click(object sender, RoutedEventArgs e)
    {
        // Step 1: Validate - customer must be selected first
        var selectedCustomer = CustomerListBox.SelectedItem as Customer;
        if (selectedCustomer == null)
        {
            MessageBox.Show("Please select a customer first!");  // User feedback
            return;
        }

        // Step 2: Store selected customer for invoice creation
        _selectedCustomerForInvoice = selectedCustomer;
        
        // Step 3: Get invoice amount using simple input dialog
        var result = Microsoft.VisualBasic.Interaction.InputBox(
            "Enter invoice amount (e.g. 1500):", 
            "Create New Invoice", 
            "0");

        // Step 4: Validate input and create invoice
        if (decimal.TryParse(result, out _newInvoiceAmount) && _newInvoiceAmount > 0)
        {
            var invoice = new Invoice
            {
                Id = _invoiceService.Invoices.Count + 1,      // Simple auto-increment ID
                CustomerId = selectedCustomer.Id,             // Foreign key relationship
                Customer = selectedCustomer,                  // Navigation property for UI
                Amount = _newInvoiceAmount,                   // Invoice amount
                Date = DateTime.Now,                          // Invoice date
                Status = "Unpaid"                             // Default status
            };
            _invoiceService.AddInvoice(invoice);  // Add to service (UI auto-updates)
        }
    }

    /// <summary>
    /// PAY INVOICE button - Marks selected invoice as paid
    /// </summary>
    private void PayInvoice_Click(object sender, RoutedEventArgs e)
    {
        // Get selected invoice from ListBox
        _selectedInvoice = InvoiceListBox.SelectedItem as Invoice;
        if (_selectedInvoice == null) return;  // Nothing selected - exit silently

        // Update invoice status and refresh UI display
        _selectedInvoice.Status = "Paid";
        InvoiceListBox.Items.Refresh();  // Force UI update
    }

    /// <summary>
    /// DELETE INVOICE button - Removes selected invoice
    /// </summary>
    private void DeleteInvoice_Click(object sender, RoutedEventArgs e)
    {
        // Get selected invoice from ListBox
        _selectedInvoice = InvoiceListBox.SelectedItem as Invoice;
        if (_selectedInvoice == null) return;  // Nothing selected - exit silently

        // Remove invoice through service layer (UI auto-updates)
        _invoiceService.RemoveInvoice(_selectedInvoice);
    }
}
