using MauiProject.Models;
using MauiProject.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;


namespace MauiProject.ViewModels
    {
    [QueryProperty(nameof(ProductID), "id")]
    public class ProductPageViewModel : ObservableObject, IQueryAttributable
    {
        private string _productName = "AeroComfort Pro 2024";
        private string _description = "The AeroComfort Pro 2024 combines high-grade breathable mesh with an adaptive lumbar support system. Designed for 8+ hours of continuous use, it features 4D adjustable armrests and a silent multi-position tilt mechanism.";
        private string _selectedCategory = "Office Furniture";
        private string _price = "299.99";
        private string _productImageUrl = "ergonomic_chair_studio.png";
        private IDBStore dbStore;
        private int _productId;

        public ObservableCollection<string> Categories { get; set; }

        public int ProductID 
        {  
            get => _productId;
            set{ _productId = value; }
        }

        public string ProductName
        {
            get => _productName;
            set { _productName = value; OnPropertyChanged(); }
        }

        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(); }
        }

        public string SelectedCategory
        {
            get => _selectedCategory;
            set { _selectedCategory = value; OnPropertyChanged(); }
        }

        public string Price
        {
            get => _price;
            set { _price = value; OnPropertyChanged(); }
        }

        public string ProductImageUrl
        {
            get => _productImageUrl;
            set { _productImageUrl = value; OnPropertyChanged(); }
        }

        // Action Command Pointers
        public ICommand SaveProductCommand { get; }
        public ICommand DiscardChangesCommand { get; }
        public ICommand DeleteListingCommand { get; }
        public ICommand ChangeImageCommand { get; }

        public ProductPageViewModel(IDBStore dbStore)
        {
            // Populate list items collection context 
            Categories = new ObservableCollection<string>
            {
                "Office Furniture",
                "Electronics",
                "Home Decor"
            };

            // Implement interactive operational event routing handlers
            SaveProductCommand = new Command(ExecuteSaveProduct);
            DiscardChangesCommand = new Command(ExecuteDiscardChanges);
            DeleteListingCommand = new Command(ExecuteDeleteListing);
            ChangeImageCommand = new Command(ExecuteChangeImage);
        }

        private async void ExecuteSaveProduct()
        {
            // Add persistence service tracking infrastructure routines here
            await Application.Current.MainPage.DisplayAlert("Admin Portal", $"Product '{ProductName}' has been successfully updated.", "OK");
        }

        private async void ExecuteDiscardChanges()
        {
            bool reset = await Application.Current.MainPage.DisplayAlert("Confirmation", "Are you sure you want to revert all current pending field modifications?", "Yes", "No");
            if (reset)
            {
                ProductName = "AeroComfort Pro 2024";
                Price = "299.99";
                SelectedCategory = "Office Furniture";
            }
        }

        private async void ExecuteDeleteListing()
        {
            bool deleteConfirmed = await Application.Current.MainPage.DisplayAlert("Destructive Warning", "This operation is permanent. Delete item from database repository catalog immediately?", "Delete", "Cancel");
            if (deleteConfirmed)
            {
                // Execute execution delete pipeline parameters
            }
        }

        private async void ExecuteChangeImage()
        {
            try
            {
                var customMediaResult = await MediaPicker.Default.PickPhotoAsync();
                if (customMediaResult != null)
                {
                    ProductImageUrl = customMediaResult.FullPath;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Local device media file extraction failure: {ex.Message}");
            }
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("id", out var id))
            {
                ProductID = Convert.ToInt32(id);
            }
        }
    }
}
