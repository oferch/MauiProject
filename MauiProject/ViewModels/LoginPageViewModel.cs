using MauiProject.Models;
using MauiProject.Services;
using MauiProject.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace MauiProject.ViewModels
{
    public class LoginPageViewModel : ObservableObject
    {
        private IDBStore dbStore;
        private IServiceProvider serviceProvider;
        private  User user = new User() { Id = 0, FirstName = "", LastName = "", Password = "", UserName = "" };
        private bool isPasswordNotVisible=true;

        public ICommand LoginCommand { get; }
        public ICommand SignUpCommand { get; }

        private async Task DoLogin()
        {

            User u = await dbStore.GetUserAsync(user.UserName);
            if (u != null && u.Password == user.Password)
            {
                user = u;
                if (Application.Current is not null)
                {
                    ((App)Application.Current).CurrentUser = u;
                    var categories = await dbStore.GetCategoriesAsync();
                    ((App)Application.Current).Categories = categories;

                    if (u.IsAdmin)
                        Application.Current.Windows[0].Page = new AdminShell();
                    else
                        Application.Current.Windows[0].Page = new AppShell();
                }
            }
        }

        public LoginPageViewModel(IServiceProvider serviceProvider)
        {   
            this.serviceProvider = serviceProvider;
            this.dbStore = serviceProvider.GetRequiredService<IDBStore>();
            LoginCommand = new Command(async () => await DoLogin());
            SignUpCommand = new Command(async () => await DoSignUp());
        }

        private async Task DoSignUp()
        {
            if (Application.Current is not null)
                Application.Current.Windows[0].Page = serviceProvider.GetRequiredService<RegisterPage>();
        }

        public bool IsPasswordNotVisible
        {
            get => isPasswordNotVisible;
            set
            {
                if (isPasswordNotVisible != value)
                {
                    isPasswordNotVisible = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Username
        {
            get => user.UserName;
            set
            {
                if (user.UserName != value)
                {
                    user.UserName = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Password
        {
            get => user.Password;
            set
            {
                if (user.Password != value)
                {
                    user.Password = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
