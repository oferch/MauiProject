using MauiProject.Models;

namespace MauiProject.Views;

public partial class StorePage : ContentPage
{
    // A collection to hold our dynamic database/API data
    private List<Product> ProductsList { get; set; } = new();
    private bool _isAscending = true;

    public StorePage()
    {
        InitializeComponent();

        // 1. Load mock data matching your catalog requirements
        LoadMockProducts();

        // 2. Programmatically generate and inject cards into the XAML Grid
        PopulateProductGrid();
    }

    private void LoadMockProducts()
    {
        ProductsList = new List<Product>
        {
            new Product { Name = "שעון חכם דגם Pro", Price = 850, ImageUrl = "watch_pro.png", IsFavorite = true },
            new Product { Name = "אוזניות בלוטות' פרימיום", Price = 499, ImageUrl = "headphones.png", IsFavorite = false },
            new Product { Name = "נעלי ריצה מקצועיות", Price = 320, ImageUrl = "running_shoes.png", IsFavorite = false },
            new Product { Name = "מצלמת פולארויד רטרו", Price = 280, ImageUrl = "retro_camera.png", IsFavorite = true }
        };
    }

    private void OnSortClicked(object sender, EventArgs e)
    {
        // היפוך מצב המיון
        _isAscending = !_isAscending;

        // הפעלת לוגיקת המיון והרענון
        SortAndRefreshGrid();
    }

    private void SortAndRefreshGrid()
    {
        // 1. מיון הרשימה דינמית באמצעות LINQ לפי מצב המשתנה
        if (_isAscending)
        {
            ProductsList = ProductsList.OrderBy(p => p.Price).ToList();
            LblSortText.Text = "מיון: מהנמוך לגבוה";
        }
        else
        {
            ProductsList = ProductsList.OrderByDescending(p => p.Price).ToList();
            LblSortText.Text = "מיון: מהגבוה לנמוך";
        }

        // 2. בנייה מחדש של ה-Grid הויזואלי עם הרשימה הממוינת החדשה
        PopulateProductGrid();
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
            var productCard = CreateProductCard(product);

            // Assign spatial coordinate values inside the parental Grid layout matrix
            Grid.SetRow(productCard, row);
            Grid.SetColumn(productCard, col);

            // Inject the created card hierarchy into the view tree
            ProductGrid.Children.Add(productCard);
        }
    }

    // Dynamic Visual Card Factory
    [Obsolete]
    private View CreateProductCard(Product product)
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
            CornerRadius = 16
        };
        favButton.Clicked += (s, e) => {
            product.IsFavorite = !product.IsFavorite;
            favButton.Source = product.IsFavorite ? "favorite_filled.png" : "favorite_outline.png";
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
            Text = $"₪{product.Price}",
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
            TextColor = Colors.White
        };
        if (brandColor != null) addToCartButton.BackgroundColor = (Color)brandColor;

        addToCartButton.Clicked += async (s, e) => {
            await DisplayAlert("עגלת קניות", $"המוצר {product.Name} התווסף בהצלחה לסל!", "אישור");
        };

        infoStack.Children.Add(titleLabel);
        infoStack.Children.Add(priceLabel);
        infoStack.Children.Add(addToCartButton);

        mainLayout.Children.Add(infoStack);
        cardBorder.Content = mainLayout;

        return cardBorder;
    }
}