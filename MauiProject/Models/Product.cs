using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MauiProject.Models
{

    public class Product : ObservableObject
    {
        private int id = 0;
        private double price=0;
        private string name="";
        private string description = "";    
        private string imageUrl="";
        private bool isFavorite=false;

        [PrimaryKey, AutoIncrement]
        public int Id { get; set;  }

        public string Name { get => name; 
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Description { get => description; 
            set
            {
                if (description != value)
                {
                    description = value;
                    OnPropertyChanged();
                }
            }
        }

        public double Price { get => price; 
            set
            {
                if (price != value)
                {
                    price = value;
                    OnPropertyChanged();
                }
            }
        }
        public string ImageUrl { get => imageUrl;
            set
            {
                if (imageUrl != value)
                {
                    imageUrl = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool IsFavorite { get => isFavorite;
            set
            {
                if (isFavorite != value)
                {
                    isFavorite = value;
                    OnPropertyChanged();
                }       
            }
        }
    }
}
