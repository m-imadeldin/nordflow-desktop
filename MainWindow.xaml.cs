using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Microsoft.VisualBasic;
using NordFlow.Services;
using NordFlow.Models;

namespace NordFlow;

/// <summary>
/// MainWindow - NordFlow ERP system main window
/// Coordinates UI, services, and business logic
/// </summary>
public partial class MainWindow : Window
{
    // ========================================
    // FIELDS - Store data and services
    // ========================================
    private readonly CustomerService _customerService = new CustomerService();
    private readonly ObservableCollection<Customer> _customers = new();
    private Customer? _selectedCustomer;

    private readonly InvoiceService _invoiceService = new InvoiceService();
    private Invoice? _selectedInvoice;
    private Customer? _selectedCustomerForInvoice;
    private decimal _newInvoiceAmount;

    private readonly ProductService _productService = new ProductService();
    private readonly ObservableCollection<Product> _products = new();
    private Product? _selectedProduct;

    // ========================================
    // PUBLIC PROPERTIES - For XAML data binding
    // ========================================
    public CustomerService CustomerService { get; }
    public InvoiceService InvoiceService { get; }
    public ProductService ProductService { get; }

    // ========================================
    // CONSTRUCTOR - Initialize window and bind data
    // ========================================
    public MainWindow()
    {
        InitializeComponent();

        // Connect services to UI
        CustomerService = _customerService;
        InvoiceService = _invoiceService;
        ProductService = _productService;

        // Bind lists to ListBoxes
        CustomerListBox.ItemsSource = _customers;
        InvoiceListBox.ItemsSource = _invoiceService.Invoices;
        ProductListBox.ItemsSource = _products;

        // Load initial data
        LoadCustomers();
        LoadInvoices();
        LoadProducts();
    }

    // ========================================
    // LOAD CUSTOMERS - Refresh customer list from service
    // ========================================
    private void LoadCustomers()
    {
        _customers.Clear();
        foreach (var customer in _customerService.Customers)
        {
            _customers.Add(customer);
        }
    }

    // ========================================
    // LOAD INVOICES - Refresh invoice list from service
    // ========================================
    private void LoadInvoices()
    {
        InvoiceListBox.ItemsSource = null;
        InvoiceListBox.ItemsSource = _invoiceService.Invoices;
    }

    // ========================================
    // LOAD PRODUCTS - Refresh product list from service
    // ========================================
    private void LoadProducts()
    {
        _products.Clear();
        foreach (var product in _productService.Products)
        {
            _products.Add(product);
        }
    }

    // ========================================
    // ADD CUSTOMER - Create a new customer
    // ========================================
    private void MyButton_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NameInput.Text))
        {
            MessageBox.Show("Enter a customer name first!");
            return;
        }

        var customer = new Customer
        {
            Name = NameInput.Text
        };

        _customerService.AddCustomer(customer);
        LoadCustomers();
        NameInput.Clear();
    }

    // ========================================
    // EDIT CUSTOMER - Load selected customer into input
    // ========================================
    private void EditButton_Click(object sender, RoutedEventArgs e)
    {
        _selectedCustomer = CustomerListBox.SelectedItem as Customer;
        if (_selectedCustomer != null)
        {
            NameInput.Text = _selectedCustomer.Name;
        }
    }

    // ========================================
    // DELETE CUSTOMER - Remove selected customer
    // ========================================
    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        _selectedCustomer = CustomerListBox.SelectedItem as Customer;
        if (_selectedCustomer == null)
        {
            MessageBox.Show("Select a customer first!");
            return;
        }

        _customerService.RemoveCustomer(_selectedCustomer);
        LoadCustomers();
    }

    // ========================================
    // CREATE INVOICE - Add a new invoice for selected customer
    // ========================================
    private void NewInvoice_Click(object sender, RoutedEventArgs e)
    {
        _selectedCustomerForInvoice = CustomerListBox.SelectedItem as Customer;
        if (_selectedCustomerForInvoice == null)
        {
            MessageBox.Show("Select a customer first!");
            return;
        }

        string amountText = Interaction.InputBox(
            "Enter invoice amount:",
            "New Invoice",
            "1000");

        if (!decimal.TryParse(amountText, out _newInvoiceAmount) || _newInvoiceAmount <= 0)
        {
            MessageBox.Show("Enter a valid amount!");
            return;
        }

        var invoice = new Invoice
        {
            Customer = _selectedCustomerForInvoice,
            CustomerId = _selectedCustomerForInvoice.Id,
            Amount = _newInvoiceAmount,
            Date = DateTime.Now,
            Status = "Unpaid"
        };

        _invoiceService.AddInvoice(invoice);
        LoadInvoices();
    }

    // ========================================
    // PAY INVOICE - Mark selected invoice as paid
    // ========================================
    private void PayInvoice_Click(object sender, RoutedEventArgs e)
    {
        _selectedInvoice = InvoiceListBox.SelectedItem as Invoice;
        if (_selectedInvoice == null)
        {
            MessageBox.Show("Select an invoice first!");
            return;
        }

        if (_selectedInvoice.Status == "Paid")
        {
            MessageBox.Show("Invoice already paid!");
            return;
        }

        _invoiceService.PayInvoice(_selectedInvoice);
        LoadInvoices();
        MessageBox.Show($"Invoice paid! Total: {_selectedInvoice.Amount:C}");
    }

    // ========================================
    // DELETE INVOICE - Remove selected invoice
    // ========================================
    private void DeleteInvoice_Click(object sender, RoutedEventArgs e)
    {
        _selectedInvoice = InvoiceListBox.SelectedItem as Invoice;
        if (_selectedInvoice == null)
        {
            MessageBox.Show("Select an invoice first!");
            return;
        }

        _invoiceService.RemoveInvoice(_selectedInvoice);
        LoadInvoices();
    }
    
    // ========================================
    // ADD PRODUCT - Create a new product and add it to the list
    // ========================================

    private void AddProduct_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(ProductNameInput.Text))
        {
            MessageBox.Show("Enter a product name first");
            return;
        }

        if (!decimal.TryParse(ProductPriceInput.Text, out decimal price) || price < 0)
        {
            MessageBox.Show("Enter a valid price!");
            return;
        }


        if (!int.TryParse(ProductStockInput.Text, out int stock) || stock < 0)
        {
            MessageBox.Show("Enter a valid stock quantity!");
            return;
        }

        var product = new Product
        {
            Name = ProductNameInput.Text,
            Price = price,
            StockQuantity = stock
        };

        _productService.AddProduct(product);
        LoadProducts();

        ProductNameInput.Clear();
        ProductPriceInput.Clear();
        ProductStockInput.Clear();
    }
}
