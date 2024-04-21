using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

namespace LogisticsUX.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public ViewModelBase DashboardVM { get; }

    public MainWindowViewModel()
    {
        DashboardVM = App.AppHost.Services.GetRequiredService<DashboardViewModel>();
    }
}