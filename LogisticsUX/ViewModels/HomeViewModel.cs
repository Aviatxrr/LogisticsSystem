using System;
using System.Reflection;
using Castle.DynamicProxy;
using DispatchRecordSystem;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using Type = System.Type;

namespace LogisticsUX.ViewModels;

public class HomeViewModel : ViewModelBase
{
    private ViewModelBase dispatchView;
    public ViewModelBase DispatchView
    {
        get => dispatchView;
        set => this.RaiseAndSetIfChanged(ref dispatchView, value);
    }

    private ViewModelBase entityView;

    public ViewModelBase EntityView
    {
        get => entityView;
        set => this.RaiseAndSetIfChanged(ref entityView, value);
    }

    public HomeViewModel()
    {
        DispatchView = App.AppHost.Services.GetRequiredService<DispatchViewModel>();
        App.AppHost.Services
            .GetRequiredService<SessionContainer>()
            .SelectionChanged += SetEntityView;
    }

    public void SetEntityView(ViewModelBase viewModel)
    {
        EntityView = viewModel;
    }
    
}