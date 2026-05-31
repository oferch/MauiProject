using System;
using System.Collections.Generic;
using System.Text;

namespace MauiProject.Models
{

    internal class Product : ObservableObject
    {
        private double price=0;
        private string name="";
        private string imageUrl="";
        private bool isFaovrite=false;

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
        public bool IsFavorite { get => isFaovrite;
            set
            {
                if (isFaovrite != value)
                {
                    isFaovrite = value;
                    OnPropertyChanged();
                }       
            }
        }
    }
}
