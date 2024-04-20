using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        // add viewmodels and services below
        AppHost = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                foreach (var type in GetTypes("LogisticsUX.ViewModels"))
                {
                    services.AddSingleton(type);
                }
                foreach (var type in GetTypes("LogisticsUX.Views"))
                {
                    services.AddSingleton(type);
                }
            })
            .Build();
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.ShutdownRequested += Shutdown;
            var mainWindow = AppHost.Services.GetRequiredService<MainWindow>();
            mainWindow.DataContext = AppHost.Services.GetRequiredService<MainWindowViewModel>();
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

    public IEnumerable<Type> GetTypes(string namespaceName)
    {
        Assembly assembly = Assembly.GetExecutingAssembly();

        var types = assembly.GetTypes()
            .Where(t => t.Namespace == namespaceName);

        return types;
    }
}
