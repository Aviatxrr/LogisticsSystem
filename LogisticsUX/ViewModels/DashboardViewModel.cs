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

    //DI Container
    private IServiceProvider _serviceProvider = App.AppHost.Services;
    
    private static ViewModelFactory _viewModelFactory;
    
    //UserMessage is a wrapper for the contents of UserMessage.cs
    private string _userMessage;
    public string UserMessage
    {
        get => _userMessage;
        set => this.RaiseAndSetIfChanged(ref _userMessage, value);
    }

    //This is a placeholder for whatever viewmodel should be on the right.
    //If a viewmodel for a particular entity is needed, set SelectedEntity instead.
    private ViewModelBase _infoView;
    public ViewModelBase InfoView
    {
        get => _infoView;
        set => this.RaiseAndSetIfChanged(ref _infoView, value);
    }

    
    //set this to get an info view of the selection on the right. can be set from other VMs
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
    
    //wrapper for inbound dispatch
    private ObservableCollection<DispatchRecord> _inbound;

    public ObservableCollection<DispatchRecord> Inbound
    {
        get => _inbound;
        set => this.RaiseAndSetIfChanged(ref _inbound, value);
    }

    //and for outbound
    private ObservableCollection<DispatchRecord> _outbound;

    public ObservableCollection<DispatchRecord> Outbound
    {
        get => _outbound;
        set => this.RaiseAndSetIfChanged(ref _outbound, value);
    }

    //Combobox selection of available stations. Should default to user's "StationId"
    public ObservableCollection<int> StationIds { get;  }
    private int _selectedStation;

    public int SelectedStation
    {
        get => _selectedStation;
        set
        {
            UpdateTripList(value);
            this.RaiseAndSetIfChanged(ref _selectedStation, value);
        }
    }

    
    public DashboardViewModel()
    {
        //init available stations
        StationIds = new ObservableCollection<int>(_serviceProvider
            .GetRequiredService<Repository<Station>>()
            .Select(s => s.Id));
        
        //get a viewmodel factory, should only be needed in the dashboard but may move
        //into the DI container later
        _viewModelFactory = new ViewModelFactory();
        
        //subscribe UserMessage to contents of the version in DI container
        //this allows the UserMessage to be automatically updated.
        _serviceProvider.GetRequiredService<UserMessage>()
            .UserMessageChanged += updateUserMessage;
    }

    //used to refresh the datagrids
    public void UpdateTripList(int stationId)
    {
        Outbound = new ObservableCollection<DispatchRecord>(_serviceProvider
            .GetRequiredService<Repository<DispatchRecord>>()
            .GetAll()
            .Where(d => d.OriginId == stationId));
        Inbound = new ObservableCollection<DispatchRecord>(_serviceProvider
            .GetRequiredService<Repository<DispatchRecord>>()
            .GetAll()
            .Where(d => d.DestinationId == stationId));
    }

    //update this VM's usermessage to the global usermessage
    private void updateUserMessage(object? sender, EventArgs e)
    {
        UserMessage = _serviceProvider.GetRequiredService<UserMessage>()
            .Contents;
    }
}