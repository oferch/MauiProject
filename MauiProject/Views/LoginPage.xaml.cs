using MauiProject.ViewModels;
using System.Windows.Input;
using MauiProject.Models;
namespace MauiProject.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;

    }






    private void OnTogglePasswordClicked(object sender, EventArgs e)
    {

    }
}