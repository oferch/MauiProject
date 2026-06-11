using CommunityToolkit.Maui.Core.Extensions;
using MauiProject.Models;
using MauiProject.Services;
using MauiProject.ViewModels;
using MauiProject.Views.Helpers;

namespace MauiProject.Views;

public partial class StorePage : ContentPage
{
    private Task? loadData;
    private StorePageHelper? helper;


    public StorePage(StorePageViewModel vm, IDBStore dbStore)
    {
        InitializeComponent();
        BindingContext = vm;
        loadData = LoadProductsDataAsync(vm, dbStore);
      //  ProductsList = vm.Products;

        // 1. Load mock data matching your catalog requirements
//        LoadMockProducts();

        // 2. Programmatically generate and inject cards into the XAML Grid
      //  PopulateProductGrid();
    }

    private async Task? LoadProductsDataAsync(StorePageViewModel vm, IDBStore dbStore)
    {
        if (Application.Current != null && ((App)Application.Current).CurrentUser != null)
        {
            await vm.LoadAsyncProductsData(((App)Application.Current).CurrentUser);
            var categories = await dbStore.GetCategoriesAsync();

            if (!((App)Application.Current).CurrentUser.IsAdmin)
                helper = new StorePageHelper(vm.Products.ToObservableCollection(), categories, ProductGrid, CategoryStackLayout,  this);
            else
                helper = new StorePageHelper(vm.Products.ToObservableCollection(), categories, ProductGrid, CategoryStackLayout, this, StorePageHelper.StorePageMode.Admins);
            ProductCountLabel.Text = $"נמצאו {vm.Products.Count} מוצרים";

            LblSortText.Text = helper.DoSort();
        }
    }


    //private void LoadMockProducts()
    //{
    //    ProductsList = new List<Product>
    //    {
    //        new Product { Name = "שעון חכם דגם Pro", Price = 850, ImageUrl = "watch_pro.png", IsFavorite = true },
    //        new Product { Name = "אוזניות בלוטות' פרימיום", Price = 499, ImageUrl = "headphones.png", IsFavorite = false },
    //        new Product { Name = "נעלי ריצה מקצועיות", Price = 320, ImageUrl = "running_shoes.png", IsFavorite = false },
    //        new Product { Name = "מצלמת פולארויד רטרו", Price = 280, ImageUrl = "retro_camera.png", IsFavorite = true }
    //    };
    //}

    public void OnSortClicked(object sender, EventArgs e)
    {
        if (helper != null)
            LblSortText.Text = helper.DoSort();        
    }

}