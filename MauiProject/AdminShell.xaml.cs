namespace MauiProject;

public partial class AdminShell : Shell
{
	public AdminShell()
	{
		InitializeComponent();

        Routing.RegisterRoute("UpdateProductPage", typeof(Views.ProductPage));
    }
}