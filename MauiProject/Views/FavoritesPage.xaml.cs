using MauiProject.Views.Helpers;

namespace MauiProject.Views;

public partial class FavoritesPage : ContentPage
{
    private StorePageHelper helper;

    public FavoritesPage()
	{
		InitializeComponent();

        helper = new StorePageHelper(((App)Application.Current).CurrentUser.Favorites, ProductGrid, this, true);

        LblSortText.Text = helper.DoSort();
        ProductCountLabel.Text = $"{helper.ProductsList.Count} תוצאות";
    }

}