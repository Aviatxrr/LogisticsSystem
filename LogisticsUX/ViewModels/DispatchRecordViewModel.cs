using DispatchRecordSystem.Models;
using ReactiveUI;

namespace LogisticsUX.ViewModels;

public class DispatchRecordViewModel : ViewModelBase
{
    private IEntity _entity;

    public IEntity Entity
    {
        get => _entity;
        set => this.RaiseAndSetIfChanged(ref _entity, value);
    }
}