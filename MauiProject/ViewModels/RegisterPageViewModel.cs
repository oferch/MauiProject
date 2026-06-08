using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using MauiProject.Models;
using MauiProject.Services;

namespace MauiProject.ViewModels
{
    public class RegisterPageViewModel : ObservableObject
    {
        public User user = new User() { Id = 0, FirstName = "", LastName = "", Password = "", UserName = "", Email = "", PhoneNumber = "" };
        private int age = 0;
        private string fullName = "";
        private bool isPasswordNotVisible = true;
        private DateTime dateOfBirth = DateTime.Now;
        IDBStore dbStore;

        private string password = "", passErr = "", ageErr = "",  userErr = "",email = "", phone = "", userName = "";

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

        public string PassError
        {
            get => passErr;
        }

        public string Password
        {
            get { return password; }
            set
            {
                if (password != value)
                {
                    password = value;
                    string pattern = @"^(?=.*[A-Z])(?=.*\d).+$";
                    if (!Regex.IsMatch(password, pattern))
                    {
                        passErr = "Password Must contin an upper case letter and a digit";
                    }
                    else
                    {
                        user.Password = password;
                        passErr = "";
                    }

                    OnPropertyChanged();
                    OnPropertyChanged(nameof(PassError));
                    OnPropertyChanged(nameof(IsReady));
                }
            }
        }

        public string UsernameError
        {
            get => userErr;
        }

        public string UserName
        {
            get { return userName; }
            set
            {
                if (userName != value)
                {
                    userName = value;
                    if (char.IsDigit(userName[0]) || userName.Contains(" "))
                    {
                        userErr = "Username must not start with a digit and must not contain spaces.";
                    }
                    else
                    {
                        user.UserName = userName;
                        userErr = "";
                    }

                    OnPropertyChanged();
                    OnPropertyChanged(nameof(UsernameError));
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
                    user.Email = value;
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
                    user.PhoneNumber = value;
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
        public string AgeError
        {
            get => ageErr;
        }

        public int Age { get => age; private set => age = value; }
        public DateTime DateOfBirth
        {
            get => dateOfBirth;
            set
            {
                if (value != dateOfBirth)
                {
                    dateOfBirth = value;
                    user.DateOfBirth = value;
                    TimeSpan ts = DateTime.Now.Subtract(dateOfBirth);
                    age = DateTime.Now.Year - dateOfBirth.Year;

                    // Adjust if the birthday hasn't occurred this year yet
                    if (DateTime.Now.Month < dateOfBirth.Month || (DateTime.Now.Month == dateOfBirth.Month && DateTime.Now.Day < dateOfBirth.Day))
                    {
                        age--;
                    }
                    if (age < 18)
                    {
                        ageErr = "You must be at least 18 years old to register.";
                    }
                    else
                    {
                        ageErr = "";
                    }
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(Age));
                    OnPropertyChanged(nameof(AgeError));
                    OnPropertyChanged(nameof(IsReady));
                }
            }
        }

        private bool IsRegisterReady()
        {
            return age > 0 && fullName.Trim().Length > 0 && fullName.Trim().Contains(' ') && password.Trim().Length > 0 &&
                phone.Trim().Length > 0 && email.Trim().Length > 0 && passErr.Trim().Length == 0 && ageErr.Trim().Length == 0
                && userErr.Trim().Length == 0;
        }

        public bool Save()
        {
            if (IsRegisterReady())
            {
                dbStore.AddUserAsync(user);
                return true;
            }
            return false;
        }

        public RegisterPageViewModel(IDBStore dbStore)
        {
            this.dbStore = dbStore;
        }
    }
}
