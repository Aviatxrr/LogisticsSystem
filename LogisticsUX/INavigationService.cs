using System;
using LogisticsUX.ViewModels;

namespace LogisticsUX;

public interface INavigationService
{
    void NavigateTo<TViewModel>() where TViewModel : ViewModelBase, new();
}

public class NavigationService : INavigationService
{
    private MainWindowViewModel _mainWindowViewModel;

    public NavigationService(MainWindowViewModel mainWindowViewModel)
    {
        _mainWindowViewModel = mainWindowViewModel;
    }

    public void NavigateTo<TViewModel>() where TViewModel : ViewModelBase, new()
    {
        _mainWindowViewModel.CurrentView = new TViewModel();
        Console.WriteLine($"Navigating to {typeof(TViewModel)}");
    }
}