using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
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

    public ReactiveCommand<DispatchRecord.DispatchStatus, Unit> SelectionChnaged;

    public DispatchRecordViewModel()
    {
            DispatchRecord = App.AppHost.Services
                .GetRequiredService<SessionContainer>()
                .EntityViewEntity as DispatchRecord;
            StatusList = new ObservableCollection<DispatchRecord.DispatchStatus>(
                Enum.GetValues(typeof(DispatchRecord.DispatchStatus)) as IEnumerable<DispatchRecord.DispatchStatus>);
            Status = DispatchRecord.Status;
            SelectionChnaged = ReactiveCommand.Create<DispatchRecord.DispatchStatus>(OnSelectionChanged);
            this.WhenAnyValue(x => x.Status)
                .InvokeCommand(SelectionChnaged);
    }

    public void OnSelectionChanged(DispatchRecord.DispatchStatus status)
    {
        Console.WriteLine("Updating status");
        DispatchRecord.Status = status;
        App.AppHost!.Services
            .GetRequiredService<Repository<DispatchRecord>>()
            .Update(DispatchRecord);
    }
    
}