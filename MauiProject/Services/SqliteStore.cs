using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensionsAsync.Extensions;
using MauiProject.Models;

namespace MauiProject.Services
{
    public class SqliteStore : IDBStore
    {
        #region DB Consts   
        const string DB_NAME = "Store.db3";

        public const SQLite.SQLiteOpenFlags flags = SQLite.SQLiteOpenFlags.ReadWrite |
            SQLite.SQLiteOpenFlags.Create |
            SQLite.SQLiteOpenFlags.SharedCache;
        /*...
        string filename = ...
        SQLiteConnection conn = new SQLiteConnection(filename);*/
        #endregion

        string DatabasePath => System.IO.Path.Combine(FileSystem.AppDataDirectory, DB_NAME);
        SQLiteAsyncConnection connection;

        private async Task Init()
        {
            if (connection != null)
                return;
            try
            {
                connection = new SQLiteAsyncConnection(DatabasePath, flags);
                await connection.CreateTableAsync<User>();
                await connection.CreateTableAsync<Product>();
                await connection.CreateTableAsync<Favorite>();

            }
            catch (Exception ex) { }
        }

        public async Task<User> GetUserAsync(string username)
        {
            try
            {
                await Init();
                var user = await connection.GetAsync<User>(u => u.UserName == username);
                return user;
            }
            catch (Exception ex) { 
            Console.WriteLine(ex.Message);
            }

            return null;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            await Init();
            return await connection.Table<User>().ToListAsync();
        }

        public async Task AddUserAsync(User user)
        {
            await Init();
            await connection.InsertAsync(user);
        }

        public async Task AddProductAsync(Product product)
        {
            await Init();
            await connection.InsertAsync(product);
        }

        public async Task AddFavoriteAsync(Favorite favorite)
        {
            await Init();
            await connection.InsertAsync(favorite);
        }

        public async Task<List<User>> GetUsersWithFavoritesAsync()
        {
            await Init();
            return await connection.GetAllWithChildrenAsync<User>();
        }

        public async Task  LoadAsyncMockData()
        {
            await Init();
            await connection.DeleteAllAsync<User>();
            await connection.DeleteAllAsync<Product>();
            await connection.DeleteAllAsync<Favorite>();

            var user = new User {FirstName = "John", LastName = "Doe", UserName = "johndoe", Password = "password", IsAdmin = false };
            var user2 = new User { FirstName = "master", LastName = "Admin", UserName = "admin", Password = "admin", IsAdmin = true };
            var product1 = new Product {  Name = "Product 1", Description = "Description for Product 1", Price = 9.99 };
            var product2 = new Product { Name = "Product 2", Description = "Description for Product 2", Price = 19.99 };

            await connection.InsertAsync(user);
            await connection.InsertAsync(user2);
            await connection.InsertAsync(product1);
            await connection.InsertAsync(product2);

            var favorite1 = new Favorite { Id = 1, UserID = user.Id, ProductID = product1.Id };
            var favorite2 = new Favorite { Id = 2, UserID = user.Id, ProductID = product2.Id };

            await connection.InsertAsync(favorite1);
            await connection.InsertAsync(favorite2);
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            await Init();
            return await connection.Table<Product>().ToListAsync();
        }

        public async Task<List<Favorite>> GetFavoritesAsync()
        {
            await Init();
            return await connection.Table<Favorite>().ToListAsync();
        }
    }
}
