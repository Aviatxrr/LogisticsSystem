using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using DispatchRecordSystem;
using LogisticsUX.ViewModels;
using LogisticsUX.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LogisticsUX;

public partial class App : Application
{
    public static IHost? AppHost { get; private set; }
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        AppHost = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<MainWindow>();
                    services.AddSingleton<MainWindowViewModel>();
                    services.AddSingleton<INavigationService, NavigationService>();
                    services.AddTransient<LoginViewModel>();
                    services.AddTransient<HomeViewModel>();
                    services.AddTransient<DispatchViewModel>();
                    services.AddTransient<DispatchRecordViewModel>();
                    services.AddSingleton<LogisticsDbContext>();
                    services.AddSingleton<Repository<User>>();
                    services.AddSingleton<Repository<Station>>();
                    services.AddSingleton<Repository<Truck>>();
                    services.AddSingleton<Repository<Trailer>>();
                    services.AddSingleton<Repository<Driver>>();
                    services.AddSingleton<Repository<DispatchRecord>>();
                    services.AddSingleton<Repository<Company>>();
                    services.AddSingleton<UserService>();
                    services.AddSingleton<StationService>();
                    services.AddSingleton<TruckService>();
                    services.AddSingleton<Trailer>();
                    services.AddSingleton<DriverService>();
                    services.AddSingleton<DispatchRecordService>();
                    services.AddSingleton<SessionContainer>();
                    
                })
                .Build();
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.ShutdownRequested += Shutdown;
            var mainWindow = AppHost!.Services.GetRequiredService<MainWindow>();
            mainWindow.DataContext = AppHost.Services.GetRequiredService<MainWindowViewModel>();
            AppHost.Services.GetRequiredService<INavigationService>().NavigateTo<LoginViewModel>();
            desktop.MainWindow = mainWindow;
            desktop.MainWindow.Show();
        }

        base.OnFrameworkInitializationCompleted();
    }

    public void Shutdown(object? sender, ShutdownRequestedEventArgs e)
    {

        AppHost.StopAsync();
        AppHost.Dispose();

    }
    
}