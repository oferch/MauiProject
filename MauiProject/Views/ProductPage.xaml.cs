namespace MauiProject.Views;
using MauiProject.ViewModels;

public partial class ProductPage : ContentPage
{
	public ProductPage(ProductPageViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        // Back navigation handling sequence
        await Shell.Current.GoToAsync("..");
    }
}