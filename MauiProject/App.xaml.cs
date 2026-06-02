using MauiProject.Views;

namespace MauiProject
{
    public partial class App : Application
    {
        Page startPage;
        public App(LoginPage page)
        {
            InitializeComponent();
            startPage = page;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            // return new Window(new AppShell());
            return new Window(startPage);
        }
    }
}