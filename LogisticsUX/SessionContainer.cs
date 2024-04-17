using System;
using DispatchRecordSystem;

namespace LogisticsUX;

public class SessionContainer
{
    public delegate void EntitySelectionEventHandler(IEntity entity);
    
    public event EntitySelectionEventHandler SelectionChanged;
    public User currentUser { get; set; }

    private IEntity _entityViewEntity;
    public IEntity EntityViewEntity
    {
        get => _entityViewEntity;
        set
        {
            _entityViewEntity = value;
            SelectionChanged?.Invoke(_entityViewEntity); 
        }
    }
    
    
}