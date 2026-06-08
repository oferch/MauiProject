using CommunityToolkit.Maui.Core.Extensions;
using MauiProject.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MauiProject.Views.Helpers
{
    public class StorePageHelper : ObservableObject
    {
        // A collection to hold our dynamic database/API data
        public ObservableCollection<Product> ProductsList { get; set; } = new();
        private bool _isAscending = true;
        private Grid ProductGrid = null;
        private Page page = null;
        private bool isClean = false;

        public StorePageHelper(ObservableCollection<Product> ProductsList, Grid ProductGrid, Page page, bool IsClean = false)
        {
            this.ProductsList = ProductsList;
            this.ProductsList.CollectionChanged += ProductsList_CollectionChanged;

            this.ProductGrid = ProductGrid;
            this.page = page;
            this.isClean = IsClean;
        }

        private void ProductsList_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
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
                text = "מיון: מהנמוך לגבוה";
            }
            else
            {
                ProductsList = ProductsList.OrderByDescending(p => p.Price).ToObservableCollection();
                text = "מיון: מהגבוה לנמוך";
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
            if (Application.Current.Resources.TryGetValue("BorderLight", out var borderLightColor) && borderLightColor is Color color)
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
                IsVisible = !isClean
            };
            favButton.Clicked += (s, e) => {
                product.IsFavorite = !product.IsFavorite;
                favButton.Source = product.IsFavorite ? "favorite_filled.png" : "favorite_outline.png";
                OnPropertyChanged(nameof(ProductsList));
            };
            imageGrid.Children.Add(favButton);

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
            if (Application.Current.Resources.TryGetValue("TextPrimary", out var primaryColor))
                titleLabel.TextColor = (Color)primaryColor;

            var priceLabel = new Label
            {
                Text = $"₪{product.Price:F2}",
                FontSize = 16,
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Start
            };
            if (Application.Current.Resources.TryGetValue("PrimaryBlue", out var brandColor))
                priceLabel.TextColor = (Color)brandColor;

            var addToCartButton = new Button
            {
                Text = "הוסף לסל",
                HeightRequest = 36,
                CornerRadius = 8,
                FontSize = 12,
                TextColor = Colors.White,
                IsVisible = !isClean
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
