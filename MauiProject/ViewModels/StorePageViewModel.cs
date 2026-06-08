using MauiProject.Models;
using MauiProject.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace MauiProject.ViewModels
{
    public class StorePageViewModel : ObservableObject
    {
        private List<Product> products;
        IDBStore dbStore;
        public List<Product> Products
        {
            get => products;
            set
            {
                if (products != value)
                {
                    products = value;
                    OnPropertyChanged();
                }
            }
        }

        public async Task LoadAsyncProductsData(User u)
        {
            var result = await dbStore.GetProductsAsync();
            Products = result;
            if (u is not null)
            {
                foreach (var item in result)
                {
                    if (u.Favorites != null && u.Favorites.Any(p => p.Id == item.Id))
                    {
                        item.IsFavorite = true;
                    }
                }
            }

        }

        public StorePageViewModel(IDBStore dbStore)
        {
            // Sample data for testing
            this.dbStore = dbStore;
        }
    }
}
