using MauiProject.ViewModels;
using System.Text.RegularExpressions;

namespace MauiProject.Views;

public partial class RegisterPage : ContentPage
{
    public RegisterPage()
    {
        InitializeComponent();
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

    private void OnRegisterClicked(object sender, EventArgs e)
    {
        RegisterPageViewModel props = (RegisterPageViewModel)BindingContext;
        bool allValid = true;
        string pattern = @"^(?=.*[A-Z])(?=.*\d).+$";
        if (props != null)
        {
            PasswordErrorLabel.Text = "";
            UsernameErrorLabel.Text = "";
            AgeErrorLabel.Text = "";
            if (char.IsDigit(props.UserName[0]) || props.UserName.Contains(" "))
            {
                allValid = false;
                UsernameErrorLabel.Text = "Username must not start with a digit and must not contain spaces.";
            }
            if (props.Age < 18)
            {
                allValid = false;
                AgeErrorLabel.Text = "You must be at least 18 years old to register.";
            }

            if (!Regex.IsMatch(props.Password, pattern))
            {
                allValid = false;
                PasswordErrorLabel.Text = "Password Must contin an upper case letter and a digit";
            }

            if (allValid)
            {
                DisplayAlert("All Good", "All Good", "All Good");
            }
        }
    }

    private void OnSignInClicked(object sender, EventArgs e)
    {
    }
}