using System;
using System.Collections.ObjectModel;
using System.Linq;
using DispatchRecordSystem;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

namespace LogisticsUX.ViewModels;

public class DispatchViewModel : ViewModelBase
{
    
    public User currentUser;
    public Station currentStation;
    
    public ObservableCollection<DispatchRecord> InboundDispatch { get; }
    public ObservableCollection<DispatchRecord> OutboundDispatch { get; }

    

    private IEntity _selectedEntity;
    public IEntity SelectedEntity
    {
        get => _selectedEntity;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedEntity, value);
            App.AppHost!.Services
                .GetRequiredService<SessionContainer>()
                .EntityViewEntity = value;
        }
    }
    

    public DispatchViewModel()
    {
        currentUser = App.AppHost
            .Services
            .GetRequiredService<SessionContainer>()
            .currentUser;
        
        currentStation = App.AppHost
            .Services
            .GetRequiredService<Repository<Station>>()
            .First(s => s.Id == currentUser.StationId);
        
        InboundDispatch = new ObservableCollection<DispatchRecord>(currentStation.Inbound);
        OutboundDispatch = new ObservableCollection<DispatchRecord>(currentStation.Outbound);
        
    }

    
}