using MauiProject.ViewModels;
using System.Text.RegularExpressions;

namespace MauiProject.Views;

public partial class RegisterPage : ContentPage
{
    public RegisterPage(RegisterPageViewModel rVM)
    {
        InitializeComponent();
        BindingContext = rVM;

    }

    private void OnTogglePasswordClicked(object sender, EventArgs e)
    {
        RegisterPageViewModel props = (RegisterPageViewModel)BindingContext;
        props.IsPasswordNotVisible = !props.IsPasswordNotVisible;
        if (props.IsPasswordNotVisible)
        {
            TogglePasswordButton.Source = "visibility_off.png";

        }
        else
        {
            TogglePasswordButton.Source = "visibility.png";
        }
    }

    private void OnDateOfBirthSelected(object sender, DateChangedEventArgs e)
    {

    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        RegisterPageViewModel props = (RegisterPageViewModel)BindingContext;

        if (props != null)
        {

             await DisplayAlertAsync("All Good", "All Good", "All Good");
        }
    }

    private void OnSignInClicked(object sender, EventArgs e)
    {
    }
}