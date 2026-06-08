using System;
using System.Collections.Generic;
using System.Text;
using MauiProject.Models;

namespace MauiProject.Services
{
    public interface IDBStore
    {
        public Task<User> GetUserAsync(string username);
        public Task<bool> AddUserAsync(User user);
        public Task<List<Product>> GetProductsAsync();

    }
}
