using MauiProject.ViewModels;

namespace MauiProject.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

    private void OnLoginClicked(object sender, EventArgs e)
    {

    }

    private void OnTogglePasswordClicked(object sender, EventArgs e)
    {

    }
}