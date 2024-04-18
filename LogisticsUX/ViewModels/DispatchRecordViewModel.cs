/*
 This one is gonna need some serious reworking. I went in hack and slash and
 am now suffering the consequences. This class will need to be refactored such
 that it has wrapper properties for each relevant property, along with logic
 in the setter that does necessary business/service layer stuff. May also need
 to rework how the homeview handles swapping views on command but that will 
 have to come later. I need solid, readable, and easy to maintain and use
 viewmodels.
*/


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Runtime.CompilerServices;
using DispatchRecordSystem;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

namespace LogisticsUX.ViewModels;

public class DispatchRecordViewModel : ViewModelBase
{

    private DispatchRecord _dispatchRecord;

    public DispatchRecord DispatchRecord
    {
        get => _dispatchRecord;
        set => this.RaiseAndSetIfChanged(ref _dispatchRecord, value);
    }

    private int _truckId;

    public int TruckId
    {
        get => TruckId;
        set => this.RaiseAndSetIfChanged(ref _truckId, value);
    }

    public ReactiveCommand<int, Unit> TruckChanged;

    private DispatchRecord.DispatchStatus _status;

    public DispatchRecord.DispatchStatus Status
    {
        get => _status;
        set => this.RaiseAndSetIfChanged(ref _status, value);
    }

    private ObservableCollection<DispatchRecord.DispatchStatus> _statusList;

    public ObservableCollection<DispatchRecord.DispatchStatus> StatusList
    {
        get => _statusList;
        set => this.RaiseAndSetIfChanged(ref _statusList, value);
    }

    public ReactiveCommand<DispatchRecord.DispatchStatus, Unit> StatusChanged;
    

    public DispatchRecordViewModel()
    {
        DispatchRecord = App.AppHost.Services
            .GetRequiredService<SessionContainer>()
            .SelectedEntity as DispatchRecord;
            StatusList = new ObservableCollection<DispatchRecord.DispatchStatus>(
                Enum.GetValues(typeof(DispatchRecord.DispatchStatus)) as IEnumerable<DispatchRecord.DispatchStatus>);
            Status = DispatchRecord.Status;
            StatusChanged = ReactiveCommand.Create<DispatchRecord.DispatchStatus>(OnStatusChanged);
            this.WhenAnyValue(x => x.Status)
                .InvokeCommand(StatusChanged);
    }

    public void OnStatusChanged(DispatchRecord.DispatchStatus status)
    {
        DispatchRecord.Status = status;
        App.AppHost!.Services
            .GetRequiredService<Repository<DispatchRecord>>()
            .Update(DispatchRecord);
    }

    public void OnTruckChanged(int truckId)
    {
        //try
        //{
            //App.AppHost.Services
                //.GetRequiredService<DispatchRecordService>()
        //}
    }

    public void OnViewEntityInfoClick(IEntity entity)
    {
        App.AppHost.Services
            .GetRequiredService<SessionContainer>()
            .SelectedEntity = entity;
        App.AppHost.Services
            .GetRequiredService<SessionContainer>()
            .InfoView = GetViewModelForEntity(entity);
    }

    
}