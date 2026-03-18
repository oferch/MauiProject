using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiProject.Models;

namespace MauiProject.ViewModels
{
    internal class RegisterPageViewModel : ObservableObject
    {
        public User user = new User() { Id = 0, FirstName = "", LastName = "", Password = "", UserName = "" };
        private int age = 0;
        private string fullName = "";
        private DateTime dateOfBirth = DateTime.Now;

        private string password, email, phone, userName;

        public string Password
        {
            get { return password; }
            set
            {
                if (password != value)
                {
                    password = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsReady));
                }
            }
        }

        public string UserName
        {
            get { return userName; }
            set
            {
                if (userName != value)
                {
                    userName = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsReady));
                }
            }
        }

        public string Email
        {
            get { return email; }
            set
            {
                if (email != value)
                {
                    email = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsReady));
                }
            }
        }

        public string Phone
        {
            get { return phone; }
            set
            {
                if (phone != value)
                {
                    phone = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsReady));
                }
            }
        }

        public string FullName
        {
            get => fullName;
            set
            {
                if (fullName != value)
                {
                    string[] parts = value.Split(' ');
                    fullName = value;
                    if (parts.Length > 1)
                    {
                        user.LastName = parts[parts.Length - 1];
                        user.FirstName = "";
                        for (int i = 0; i < parts.Length - 1; i++)
                        {
                            if (i > 0)
                                user.FirstName += " ";
                            user.FirstName += parts[i];
                        }
                    }
                    else
                    {
                        user.LastName = "";
                        user.FirstName = "";
                    }
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsReady));
                }
            }
        }
        public bool IsReady { get => IsRegisterReady(); }
        public int Age { get => age; private set => age = value; }
        public DateTime DateOfBirth
        {
            get => dateOfBirth;
            set
            {
                if (value != dateOfBirth)
                {
                    dateOfBirth = value;
                    TimeSpan ts = DateTime.Now.Subtract(dateOfBirth);
                    age = DateTime.Now.Year - dateOfBirth.Year;

                    // Adjust if the birthday hasn't occurred this year yet
                    if (DateTime.Now.Month < dateOfBirth.Month || (DateTime.Now.Month == dateOfBirth.Month && DateTime.Now.Day < dateOfBirth.Day))
                    {
                        age--;
                    }
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(Age));
                    OnPropertyChanged(nameof(IsReady));
                }
            }
        }

        private bool IsRegisterReady()
        {
            return age > 0 && fullName.Trim().Length > 0 && fullName.Trim().Contains(' ') && password.Trim().Length > 0 &&
                phone.Trim().Length > 0 && email.Trim().Length > 0;
        }
    }
}
