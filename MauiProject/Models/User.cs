using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiProject.Models
{
    [Table("User")]
    public class User : ObservableObject
    {
        private int id;
        private string userName="";
        private string password="";
        private string firstName="";
        private string lastName="";
        private string email = "";
        private string phoneNumber = "";
        private bool isAdmin = false;

        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        
        public bool IsAdmin { get; set; }

        [Indexed]
        public string UserName
        {
            get => userName;
            set
            {
                if (userName != value)
                {
                    userName = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Password
        {
            get => password;
            set
            {
                if (password != value)
                {
                    password = value;
                    OnPropertyChanged();
                }
            }
        }
        public string FirstName
        {
            get => firstName;
            set
            {
                if (firstName != value)
                {
                    firstName = value;
                    OnPropertyChanged();
                }
            }
        }
        public string LastName
        {
            get => lastName;
            set
            {
                if (lastName != value)
                {
                    lastName = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Email
        {
            get => email;
            set
            {
                if (email != value)
                {
                    email = value;
                    OnPropertyChanged();
                }
            }
        }

        public string PhoneNumber   
        {
            get => phoneNumber;
            set
            {
                if (phoneNumber != value)
                {
                    phoneNumber = value;
                    OnPropertyChanged();
                }
            }
        }

    }
}
