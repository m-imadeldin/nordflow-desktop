using System.ComponentModel;

namespace NordFlow.Models
{
    /// <summary>
    /// Product represents a single item in the inventory
    /// Like a blueprint for every product in the warehouse
    /// </summary>
    public class Product : INotifyPropertyChanged
    {
        private int _id;
        private string _name = string.Empty;
        private decimal _price;
        private int _stockQuantity;

        // ========================================
        // PROPERTIES - Data fields with validation
        // ========================================
        public int Id 
        { 
            get => _id; 
            set 
            { 
                _id = value; 
                OnPropertyChanged(nameof(Id));
            } 
        }

        public string Name 
        { 
            get => _name; 
            set 
            { 
                _name = value; 
                OnPropertyChanged(nameof(Name));
            } 
        }

        public decimal Price 
        { 
            get => _price; 
            set 
            { 
                _price = value >= 0 ? value : 0;
                OnPropertyChanged(nameof(Price));
            } 
        }

        public int StockQuantity 
        { 
            get => _stockQuantity; 
            set 
            { 
                _stockQuantity = value >= 0 ? value : 0;
                OnPropertyChanged(nameof(StockQuantity));
            } 
        }

        // ========================================
        // INotifyPropertyChanged - UI updates automatically
        // ========================================
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // For ListBox display
        public override string ToString() => $"{Name} - {Price:C} (Stock: {StockQuantity})";
    }
}