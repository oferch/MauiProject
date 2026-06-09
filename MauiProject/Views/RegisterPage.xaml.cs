using MauiProject.ViewModels;
using System.Text.RegularExpressions;

namespace MauiProject.Views;

public partial class RegisterPage : ContentPage
{
    IServiceProvider serviceProvider;
    public RegisterPage(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        BindingContext = serviceProvider.GetService<RegisterPageViewModel>();
        this.serviceProvider = serviceProvider;

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
            props.Save();
            await DisplayAlert("Success", "Your account has been created successfully!", "OK");
            if (Application.Current is not null)
                Application.Current.Windows[0].Page = serviceProvider.GetService<LoginPage>();
        }
    }

    private void OnSignInClicked(object sender, EventArgs e)
    {
        if (Application.Current is not null)
             Application.Current.Windows[0].Page = serviceProvider.GetService<LoginPage>();
    }
}