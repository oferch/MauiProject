using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using MauiProject.Services;
using MauiProject.ViewModels;
using MauiProject.Views;
using Microsoft.Extensions.Logging;

namespace MauiProject
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>().UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Inter-Medium.ttf", "InterMedium");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.AddServices().AddViewModels().AddPages();
            return builder.Build();
        }

        public static MauiAppBuilder AddPages(this MauiAppBuilder builder)
        {
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<ProfilePage>();
            builder.Services.AddTransient<RegisterPage>();
            builder.Services.AddTransient<StorePage>();
            return builder;
        }

        public static MauiAppBuilder AddServices(this MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<IDBStore, SqliteStore>();
            return builder;
        }

        public static MauiAppBuilder AddViewModels(this MauiAppBuilder builder)
        {
            builder.Services.AddTransient<LoginPageViewModel>();
            builder.Services.AddTransient<ProfilePageViewModel>();
            builder.Services.AddTransient<RegisterPageViewModel>();
            return builder;
        }

    }
}
