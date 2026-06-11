using MauiProject.Models;
using MauiProject.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;


namespace MauiProject.ViewModels
    {
    public class ProductPageViewModel : ObservableObject, IQueryAttributable
    {
        private string _productName = "";
        private string _description = " ";
        private string _selectedCategory = "";
        private string _price = "";
        private string _productImageUrl = "";
        private IDBStore dbStore;
        private int _productId = 1;
        private Task loadCats;

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
            // Implement interactive operational event routing handlers
            SaveProductCommand = new Command(ExecuteSaveProduct);
            DiscardChangesCommand = new Command(ExecuteDiscardChanges);
            DeleteListingCommand = new Command(ExecuteDeleteListing);
            ChangeImageCommand = new Command(ExecuteChangeImage);
            this.dbStore = dbStore;
            Categories = new ObservableCollection<string>(((App)Application.Current).Categories.Select(c => c.Name)); 
        }

        private async void ExecuteSaveProduct()
        {
            if (ProductID == -1)
            {

                await dbStore.AddProductAsync(new Product
                {
                    Id = 0,
                    Name = ProductName,
                    Description = Description,
                    Price = double.TryParse(Price, out var price) ? price : 0,
                    ImageUrl = ProductImageUrl,
                    CategoryId = ((App)Application.Current).Categories.FirstOrDefault(c => c.Name == SelectedCategory)?.Id ?? -1,
                    Category = ((App)Application.Current).Categories.FirstOrDefault(c => c.Name == SelectedCategory)
                });
            }
            else
            {
                await dbStore.UpdateProductAsync(new Product
                {
                    Id = ProductID,
                    Name = ProductName,
                    Description = Description,
                    Price = double.TryParse(Price, out var price) ? price : 0,
                    ImageUrl = ProductImageUrl,
                    CategoryId = ((App)Application.Current).Categories.FirstOrDefault(c => c.Name == SelectedCategory)?.Id ?? -1,
                    Category = ((App)Application.Current).Categories.FirstOrDefault(c => c.Name == SelectedCategory)
                });
            }
            // Add persistence service tracking infrastructure routines here
            await Application.Current.MainPage.DisplayAlert("Admin Portal", $"Product '{ProductName}' has been successfully updated.", "OK");
            await Shell.Current.GoToAsync("..");
        }

        private async void ExecuteDiscardChanges()
        {
            bool reset = await Application.Current.MainPage.DisplayAlert("Confirmation", "Are you sure you want to revert all current pending field modifications?", "Yes", "No");
            if (reset)
            {
                ProductName = "";
                Price = "";
                SelectedCategory = "";
                ProductImageUrl = "";
                await Shell.Current.GoToAsync("..");

            }
        }

        private async void ExecuteDeleteListing()
        {
            bool deleteConfirmed = await Application.Current.MainPage.DisplayAlert("Destructive Warning", "This operation is permanent. Delete item from database repository catalog immediately?", "Delete", "Cancel");
            if (deleteConfirmed)
            {
                await dbStore.DeleteProductAsync(ProductID);
                await Shell.Current.GoToAsync("..");
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

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("id", out var id))
            {
                ProductID = Convert.ToInt32(id);
                Product product = await dbStore.GetProductAsync(ProductID);
                if (product != null)
                {
                    ProductName = product.Name;
                    Description = product.Description;
                    SelectedCategory = product.Category.Name;
                    Price = product.Price.ToString();
                    ProductImageUrl = product.ImageUrl;
                }
            }
        }
    }
}
