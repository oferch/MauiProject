using MauiProject.Views.Helpers;
using static MauiProject.Views.Helpers.StorePageHelper;

namespace MauiProject.Views;

public partial class FavoritesPage : ContentPage
{
    private StorePageHelper? helper;

    public FavoritesPage()
	{
		InitializeComponent();

        if (Application.Current != null && ((App)Application.Current).CurrentUser != null)
        {
            helper = new StorePageHelper(((App)Application.Current).CurrentUser.Favorites, ((App)Application.Current).Categories, ProductGrid, CategoryStackLayout, this, StorePageHelper.StorePageMode.Favorites);
        }

        LblSortText.Text = helper?.DoSort() ?? string.Empty;
        ProductCountLabel.Text = $"{helper?.ProductsList?.Count ?? 0} תוצאות";
    }

}