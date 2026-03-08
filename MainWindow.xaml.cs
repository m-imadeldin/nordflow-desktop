using System.Collections.ObjectModel;
using NordFlow.Services;
using System.Text;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NordFlow.Models;

namespace NordFlow;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private CustomerService _customerService = new CustomerService();
    private ObservableCollection<Customer> _customers;
    public MainWindow()
    {
        InitializeComponent();
        _customers = new ObservableCollection<Customer>(_customerService.GetCustomers());
        CustomerListBox.ItemsSource = _customers;
    }   
    
    /// Button
    public void MyButton_Click(object sender, RoutedEventArgs e)
    {
        var newCustomer = new Customer { Name = NameInput.Text };
        _customers.Add(newCustomer);
        
    }
    
    
    
}