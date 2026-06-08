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
                    if (u.Favorites != null)
                    {
                        item.PropertyChanged += Item_PropertyChanged;
                    }
                }
            }

        }

        private void Item_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Product p = sender as Product;
            User u = ((App)Application.Current).CurrentUser;

            if (p != null)
            {
                if (!p.IsFavorite)
                {
                    if (u.Favorites != null && u.Favorites.Any(t => t.Id == p.Id))
                    {
                        var toRemove = u.Favorites.FirstOrDefault(t => t.Id == p.Id);
                        u.Favorites.Remove(toRemove);
                        dbStore.UpdateUserAsync(u);
                    }
                }
                else
                {
                    if (u.Favorites != null && !u.Favorites.Any(t => t.Id == p.Id))
                    {
                        u.Favorites.Add(p);
                        dbStore.UpdateUserAsync(u);
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
