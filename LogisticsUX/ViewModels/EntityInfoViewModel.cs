using System;
using DispatchRecordSystem.Models;
using DynamicData.Binding;
using Microsoft.Extensions.DependencyInjection;

namespace LogisticsUX.ViewModels;

public abstract class EntityInfoViewModel<T> : ViewModelBase where T : IEntity
{
    public T Entity { get; set; }
    protected IServiceProvider _serviceProvider = App.AppHost.Services;

    public EntityInfoViewModel()
    {
        
        //configured such that the datagrids will update anytime the entity's info is changed.
        this.WhenAnyPropertyChanged()
            .Subscribe(property =>
            {
                _serviceProvider.GetRequiredService<DashboardViewModel>()
                    .UpdateTripList(_serviceProvider.GetRequiredService<DashboardViewModel>()
                        .SelectedStation);

                _serviceProvider.GetRequiredService<DashboardViewModel>()
                    .InfoView = this;
            });
    }
}