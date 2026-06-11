using CommunityToolkit.Maui.Core.Extensions;
using MauiProject.Models;
using Microsoft.Maui.Graphics.Text;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MauiProject.Views.Helpers
{
    public class StorePageHelper : ObservableObject
    {
        public enum StorePageMode
        {
            Store,
            Favorites,
            Admins
        }
        // A collection to hold our dynamic database/API data
        public ObservableCollection<Product>? ProductsList { get; set; } = new();
        private bool _isAscending = true;
        private Grid? ProductGrid = null;
        private Page? page = null;
        private StorePageMode mode = StorePageMode.Store;

        private HorizontalStackLayout categoryStackLayout;

        public StorePageHelper(ObservableCollection<Product> ProductsList, List<Category> Categories, Grid ProductGrid, HorizontalStackLayout CategoryStackLayout, Page page, StorePageMode mode=StorePageMode.Store)
        {
            this.ProductsList = ProductsList;
            this.ProductsList.CollectionChanged += ProductsList_CollectionChanged;

            this.ProductGrid = ProductGrid;
            this.page = page;
            this.mode = mode;
            this.categoryStackLayout = CategoryStackLayout;

            Application.Current.Resources.TryGetValue("PrimaryBlue", out var colorValue);

            foreach (var category in Categories)
            {
                var categoryButton = new Button
                {
                    Text = category.Name,
                    HeightRequest = 38,
                    CornerRadius = 20,
                    FontSize = 12,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Colors.White,
                    Padding = new Thickness(20, 0),
                    BackgroundColor = (Color)colorValue
                };

                categoryStackLayout.Children.Add(categoryButton);

            }

        }

        private void ProductsList_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (sender == null) return;

            ProductsList = sender as ObservableCollection<Product>;
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    break;

                default:
                    break;
            }
                PopulateProductGrid();
        }

        public string DoSort()
        {
            _isAscending = !_isAscending;

            // הפעלת לוגיקת המיון והרענון
            return SortAndRefreshGrid();
        }
        private string SortAndRefreshGrid()
        {

            string text = string.Empty;
            // 1. מיון הרשימה דינמית באמצעות LINQ לפי מצב המשתנה
            if (_isAscending)
            {
                ProductsList = ProductsList.OrderBy(p => p.Price).ToObservableCollection();
                text = "Sort: Low to High";
            }
            else
            {
                ProductsList = ProductsList.OrderByDescending(p => p.Price).ToObservableCollection();
                text = "Sort: High to Low";
            }

            // 2. בנייה מחדש של ה-Grid הויזואלי עם הרשימה הממוינת החדשה
            PopulateProductGrid();
            return text;
        }

        private void PopulateProductGrid()
        {
            // Clear out any placeholder visual rows
            ProductGrid.Children.Clear();
            ProductGrid.RowDefinitions.Clear();

            int columnsCount = 2;

            for (int i = 0; i < ProductsList.Count; i++)
            {
                var product = ProductsList[i];

                // Calculate row and column positions dynamically
                int row = i / columnsCount;
                int col = i % columnsCount;

                // Dynamically add a new row definition to the layout grid if we started a new pair
                if (col == 0)
                {
                    ProductGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                }

                // Create the individual visual item card container
                var productCard = CreateProductCard(product, page);

                // Assign spatial coordinate values inside the parental Grid layout matrix
                Grid.SetRow(productCard, row);
                Grid.SetColumn(productCard, col);

                // Inject the created card hierarchy into the view tree
                ProductGrid.Children.Add(productCard);
            }
        }

        // Dynamic Visual Card Factory
        [Obsolete]
        private View CreateProductCard(Product product, Page page)
        {
            // 1. Outer Rounded Card Box
            var cardBorder = new Border
            {
                StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = 12 },
                BackgroundColor = Colors.White,
                Padding = 0,
                Margin = 0
            };

            // If you defined your BorderLight resource globally, you can map it:
            if ((Application.Current is not null &&  Application.Current.Resources.TryGetValue("BorderLight", out var borderLightColor)) && borderLightColor is Color color)
            {
                // יצירת מברשת חלקה מהצבע שנמצא ומניעת ההתרסקות
                cardBorder.Stroke = new SolidColorBrush(color);
            }
            else
            {
                // גיבוי במקרה שהמשאב לא נמצא (צבע אפור ברירת מחדל)
                cardBorder.Stroke = new SolidColorBrush(Color.FromArgb("#CBD5E1"));
            }

            var mainLayout = new VerticalStackLayout { Spacing = 0 };

            // 2. Top Image Stack Wrapper
            var imageGrid = new Grid();

            var productImage = new Image
            {
                Source = product.ImageUrl,
                Aspect = Aspect.AspectFill,
                HeightRequest = 160
            };
            imageGrid.Children.Add(productImage);

            // Heart Icon Layer Button
            var favButton = new ImageButton
            {
                Source = product.IsFavorite ? "favorite_filled.png" : "favorite_outline.png",
                WidthRequest = 32,
                HeightRequest = 32,
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Start,
                Margin = new Thickness(8),
                BackgroundColor = Color.FromRgba(255, 255, 255, 204), // CCFFFFFF semi-transparent
                CornerRadius = 16,
            };
            favButton.Clicked += (s, e) => {
                product.IsFavorite = !product.IsFavorite;
                favButton.Source = product.IsFavorite ? "favorite_filled.png" : "favorite_outline.png";
                OnPropertyChanged(nameof(ProductsList));
            };

            var editButton = new ImageButton
            {
                Source = "edit_icon.png",
                WidthRequest = 32,
                HeightRequest = 32,
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Start,
                Margin = new Thickness(8),
                BackgroundColor = Color.FromRgba(0, 0, 0, 204), // CCFFFFFF semi-transparent
                CornerRadius = 16,
            };
            editButton.Clicked += async (s, e) => {
                // Handle edit button click
                await Shell.Current.GoToAsync("//StorePage/UpdateProductPage?id=" + product.Id);
            };

            if (mode == StorePageMode.Store)
                imageGrid.Children.Add(favButton);
            if (mode == StorePageMode.Admins)
                imageGrid.Children.Add(editButton);

            mainLayout.Children.Add(imageGrid);

            // 3. Information Details Content Body Block
            var infoStack = new VerticalStackLayout { Padding = new Thickness(12), Spacing = 6 };

            var titleLabel = new Label
            {
                Text = product.Name,
                FontSize = 14,
                LineBreakMode = LineBreakMode.TailTruncation,
                HorizontalTextAlignment = TextAlignment.Start
            };
            if (Application.Current != null && Application.Current.Resources.TryGetValue("TextPrimary", out var primaryColor))
                titleLabel.TextColor = (Color)primaryColor;

            var priceLabel = new Label
            {
                Text = $"₪{product.Price:F2}",
                FontSize = 16,
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Start
            };
            Object? brandColor = null;
            if (Application.Current != null && Application.Current.Resources.TryGetValue("PrimaryBlue", out brandColor))
                priceLabel.TextColor = (Color)brandColor;

            var addToCartButton = new Button
            {
                Text = "הוסף לסל",
                HeightRequest = 36,
                CornerRadius = 8,
                FontSize = 12,
                TextColor = Colors.White,
                IsVisible = (mode == StorePageMode.Store)
            };
            if (brandColor != null) addToCartButton.BackgroundColor = (Color)brandColor;

            addToCartButton.Clicked += async (s, e) => {
                await page.DisplayAlert("עגלת קניות", $"המוצר {product.Name} התווסף בהצלחה לסל!", "אישור");
            };

            infoStack.Children.Add(titleLabel);
            infoStack.Children.Add(priceLabel);
            infoStack.Children.Add(addToCartButton);

            mainLayout.Children.Add(infoStack);
            cardBorder.Content = mainLayout;

            return cardBorder;
        }

    }
}
