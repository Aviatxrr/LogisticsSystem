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
using DispatchRecordSystem.Models;
using DispatchRecordSystem.Services;
using LogisticsUX.ViewModels;
using LogisticsUX.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Runtime.CompilerServices;
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
                //add the DBContext
                services.AddSingleton<LogisticsDbContext>();
                services.AddSingleton<UserMessage>();
                foreach (var type in GetTypes("LogisticsUX.ViewModels",Assembly.GetExecutingAssembly()))
                {
                    //add each of the viewmodels as a singleton
                    Console.WriteLine(type);
                    services.AddSingleton(type);
                }
                foreach (var type in GetTypes("LogisticsUX.Views",Assembly.GetExecutingAssembly()))
                {
                    //add each of the views as well
                    Console.WriteLine(type);
                    services.AddSingleton(type);
                }
                foreach (var model in GetTypes("DispatchRecordSystem.Models", Assembly.Load("DispatchRecordSystem")))
                {
                    //add a DB repository for each type of model in the above
                    Console.WriteLine(model);
                    services.AddSingleton(typeof(Repository<>).MakeGenericType(model));
                }
                foreach (var service in GetTypes("DispatchRecordSystem.Services", Assembly.Load("DispatchRecordSystem")))
                {
                    Console.WriteLine(service);
                    services.AddSingleton(service);
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
            TruckService trucks = AppHost.Services.GetRequiredService<TruckService>();
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

    public IEnumerable<Type> GetTypes(string namespaceName, Assembly assembly)
    {

        var types = assembly.GetTypes()
            .Where(t => t.Namespace == namespaceName
                        && t.IsClass
                        && !t.IsNested
                        && !t.IsAbstract
            );

        return types;
    }
}
