using MauiProject.Models;
using MauiProject.Services;
using MauiProject.Views;

namespace MauiProject
{
    public partial class App : Application
    {
        Page startPage;
        public User? CurrentUser { get; set; }
        public List<Category>? Categories { get; set; } = new List<Category>();
        private SqliteStore dbStore;
        private Task loadMockData;
        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            startPage = serviceProvider.GetRequiredService<LoginPage>();
            dbStore = (SqliteStore)serviceProvider.GetRequiredService<IDBStore>();
#if REFRESH_DATA
           loadMockData = LoadAsyncMockData();
#endif
        }

        private async Task LoadAsyncMockData()
        {
            await dbStore.LoadAsyncMockData();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            // return new Window(new AppShell());
            return new Window(startPage);
        }
    }
}