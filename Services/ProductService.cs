using NordFlow.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace NordFlow.Services;

/// <summary>
/// ProductService manages the product collection
/// </summary>
public class ProductService
{
    // ========================================
    // PRODUCT LIST - Stores all products in memory
    // ========================================
    public ObservableCollection<Product> Products { get; } = new();

    // ========================================
    // ADD PRODUCT - Adds a new product to the list
    // ========================================
    public void AddProduct(Product product)
    {
        product.Id = Products.Count + 1;
        Products.Add(product);
    }
}    
        