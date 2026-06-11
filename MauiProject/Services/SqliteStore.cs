using MauiProject.Models;
using SQLite;
using SQLiteNetExtensionsAsync.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

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
        SQLiteAsyncConnection? connection;

        private async Task Init()
        {
            if (connection != null)
                return;
            try
            {
                connection = new SQLiteAsyncConnection(DatabasePath, flags);
#if REFRESH_DATA
                await connection.DropTableAsync<Favorite>();
                await connection.DropTableAsync<User>();
                await connection.DropTableAsync<Product>();
                await connection.DropTableAsync<Category>();

                await connection.CreateTableAsync<User>();
                await connection.CreateTableAsync<Product>();
                await connection.CreateTableAsync<Category>();
                await connection.CreateTableAsync<Favorite>();
#endif
            }
            catch (Exception ex) { 
                Console.WriteLine(ex.Message);
            }
        }

        public async Task<User?> GetUserAsync(string username)
        {
            try
            {
                await Init();
                if (connection is null)
                    return null;

                // Get user by ID first (we need the ID to use GetWithChildrenAsync)
                var user = await connection.Table<User>()
                                               .FirstOrDefaultAsync(u => u.UserName == username);

                // Now hydrate children using the proper overload
                if (user is not null)
                {
                    // Use the ID-based overload for GetWithChildrenAsync
                    user = await connection.GetWithChildrenAsync<User>(user.Id, recursive: true);
                }

                return user;
            }
            catch (Exception ex) { 
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<Product> GetProductAsync(int ProductID)
        {
            await Init();
            if (connection is null)
                return new Product();
            var prod = await connection.Table<Product>().Where(p => p.Id == ProductID).FirstOrDefaultAsync();
            if (prod is not null)
            {
                prod = await connection.GetWithChildrenAsync<Product>(prod.Id, recursive: true);
            }
            return prod;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            await Init();
            if (connection is null)
                return new List<User>();
            return await connection.Table<User>().ToListAsync();
        }

        public async Task<bool> AddUserAsync(User user)
        {
            try
            {
                await Init();
                if (connection is null)
                    return false;
                await connection.InsertAsync(user);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }


        public async Task AddProductAsync(Product product)
        {
            await Init();
            if (connection is not null)
                await connection.InsertAsync(product);
        }

        public async Task UpdateProductAsync(Product product)
        {
            await Init();

            if (product.CategoryId == -1)
                product.CategoryId = product.Category?.Id ?? -1;

            if (connection is not null)
                await connection.UpdateWithChildrenAsync(product);
        }

        public async Task DeleteProductAsync(int productID)
        {
            await Init();
            if (connection is not null)
                await connection.DeleteAsync(await connection.Table<Product>().Where(p => p.Id == productID).FirstOrDefaultAsync());
        }


        public async Task UpdateUserAsync(User u)
        {
            await Init();
            if (connection is not null)
                await connection.UpdateWithChildrenAsync(u);
        }

        public async Task AddFavoriteAsync(Favorite favorite)
        {
            await Init();
            if (connection is not null)
                await connection.InsertAsync(favorite);
        }

        public async Task<List<User>> GetUsersWithFavoritesAsync()
        {
            await Init();
            if (connection is null)
                return new List<User>();
            return await connection.GetAllWithChildrenAsync<User>();
        }

        public async Task  LoadAsyncMockData()
        {
            await Init();

            var category1 = new Category { Name = "Sports" };
            await connection.InsertWithChildrenAsync(category1);
            var category2 = new Category { Name = "Elegant" };
            await connection.InsertWithChildrenAsync(category2);
            var category3 = new Category { Name = "Snickers" };
            await connection.InsertWithChildrenAsync(category3);


            var user = new User {FirstName = "John", LastName = "Doe", UserName = "johndoe", Password = "password", PhoneNumber = "123-456-7890", DateOfBirth=DateTime.Parse("12/12/2000"), IsAdmin = false };
            var user2 = new User { FirstName = "master", LastName = "Admin", UserName = "admin", Password = "admin", PhoneNumber = "098-765-4321", DateOfBirth=DateTime.Parse("10/10/2000"), IsAdmin = true };
            var product1 = new Product {  Name = "Product 1", Description = "Description for Product 1", Price = 9.99, ImageUrl= "product10.png", Category = category1, CategoryId = category1.Id };
            var product2 = new Product { Name = "Product 2", Description = "Description for Product 2", Price = 19.99, ImageUrl= "product12.png", Category = category1, CategoryId = category1.Id };
            for (int i = 0; i < 10; i++)
            {
                var product = new Product { Name = $"Product {i + 3}", Description = $"Description for Product {i + 3}", Price = 9.99 + i, ImageUrl= $"product{i}.png", Category = category1, CategoryId = category1.Id };
                await connection.InsertWithChildrenAsync(product);
            }

            await connection.InsertWithChildrenAsync(product1);
            await connection.InsertWithChildrenAsync(product2);
            await connection.InsertWithChildrenAsync(user);
            await connection.InsertWithChildrenAsync(user2);

            user.Favorites.Add(product1);        
            user.Favorites.Add(product2);
            await connection.UpdateWithChildrenAsync(user);//
           // await connection.InsertWithChildrenAsync(user);

//            var favorite1 = new Favorite {  UserID = user.Id, ProductID = product1.Id };
//            var favorite2 = new Favorite {  UserID = user.Id, ProductID = product2.Id };

//            await connection.InsertAsync(favorite1);
//            await connection.InsertAsync(favorite2);
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            await Init();
            if (connection is null)
                return new List<Product>();
            return await connection.Table<Product>().ToListAsync();
        }



        public async Task<List<Favorite>> GetFavoritesAsync()
        {
            await Init();
            if (connection is null)
                return new List<Favorite>();
            return await connection.Table<Favorite>().ToListAsync();
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            await Init();
            if (connection is null)
                return new List<Category>();
            return await connection.Table<Category>().ToListAsync();
        }

    }
}
