using System;
using System.Collections.ObjectModel;
using System.Linq;
using DispatchRecordSystem;
using DispatchRecordSystem.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

namespace LogisticsUX.ViewModels;


public class DashboardViewModel : ViewModelBase
{

    private IServiceProvider _serviceProvider = App.AppHost.Services;
    private ViewModelBase _infoView;
    private static ViewModelFactory _viewModelFactory;

    public ViewModelBase InfoView
    {
        get => _infoView;
        set => this.RaiseAndSetIfChanged(ref _infoView, value);
    }

    private IEntity _selectedEntity;

    public IEntity SelectedEntity
    {
        get => _selectedEntity;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedEntity, value);
            InfoView = _viewModelFactory.GetViewModel(_selectedEntity);
        }
    }

    private ObservableCollection<DispatchRecord> _inbound;

    public ObservableCollection<DispatchRecord> Inbound
    {
        get => _inbound;
        set => this.RaiseAndSetIfChanged(ref _inbound, value);
    }

    private ObservableCollection<DispatchRecord> _outbound;

    public ObservableCollection<DispatchRecord> Outbound
    {
        get => _outbound;
        set => this.RaiseAndSetIfChanged(ref _outbound, value);
    }

    public ObservableCollection<int> StationIds { get;  }
    private int _selectedStation;

    public int SelectedStation
    {
        get => _selectedStation;
        set
        {
            updateTripList(value);
            this.RaiseAndSetIfChanged(ref _selectedStation, value);
        }
    }

    public DashboardViewModel()
    {
        StationIds = new ObservableCollection<int>(_serviceProvider
            .GetRequiredService<Repository<Station>>()
            .Select(s => s.Id));
        _viewModelFactory = new ViewModelFactory();
    }

    private void updateTripList(int stationId)
    {
        Outbound = new ObservableCollection<DispatchRecord>(_serviceProvider
            .GetRequiredService<Repository<DispatchRecord>>()
            .GetAll()
            .Where(d => d.OriginId == stationId));
        Inbound = new ObservableCollection<DispatchRecord>(_serviceProvider
            .GetRequiredService<Repository<DispatchRecord>>()
            .GetAll()
            .Where(d => d.DestinationId == stationId));
        Console.WriteLine(Outbound.Count);
        Console.WriteLine(Inbound.Count);
    }
}