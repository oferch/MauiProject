using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensionsAsync.Extensions;

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
                await connection.CreateTableAsync<Models.User>();
                await connection.CreateTableAsync<Models.Product>();
                await connection.CreateTableAsync<Models.Favorite>();

            }
            catch (Exception ex) { }
        }

        public async Task<List<Models.User>> GetUsersAsync()
        {
            await Init();
            return await connection.Table<Models.User>().ToListAsync();
        }

        public async Task AddUserAsync(Models.User user)
        {
            await Init();
            await connection.InsertAsync(user);
        }

        public async Task AddProductAsync(Models.Product product)
        {
            await Init();
            await connection.InsertAsync(product);
        }

        public async Task AddFavoriteAsync(Models.Favorite favorite)
        {
            await Init();
            await connection.InsertAsync(favorite);
        }

        public async Task<List<Models.User>> GetUsersWithFavoritesAsync()
        {
            await Init();
            return await connection.GetAllWithChildrenAsync<Models.User>();
        }

        public async Task  LoadAsyncMockData()
        {
            await Init();
            await connection.DeleteAllAsync<Models.User>();
            await connection.DeleteAllAsync<Models.Product>();
            await connection.DeleteAllAsync<Models.Favorite>();

            var user = new Models.User {FirstName = "John", LastName = "Doe", UserName = "johndoe", Password = "password" };
            var product1 = new Models.Product {  Name = "Product 1", Description = "Description for Product 1", Price = 9.99 };
            var product2 = new Models.Product { Name = "Product 2", Description = "Description for Product 2", Price = 19.99 };

            await connection.InsertAsync(user);
            await connection.InsertAsync(product1);
            await connection.InsertAsync(product2);

            var favorite1 = new Models.Favorite { Id = 1, UserID = user.Id, ProductID = product1.Id };
            var favorite2 = new Models.Favorite { Id = 2, UserID = user.Id, ProductID = product2.Id };

            await connection.InsertAsync(favorite1);
            await connection.InsertAsync(favorite2);
        }

        public async Task<List<Models.Product>> GetProductsAsync()
        {
            await Init();
            return await connection.Table<Models.Product>().ToListAsync();
        }

        public async Task<List<Models.Favorite>> GetFavoritesAsync()
        {
            await Init();
            return await connection.Table<Models.Favorite>().ToListAsync();
        }
    }
}
