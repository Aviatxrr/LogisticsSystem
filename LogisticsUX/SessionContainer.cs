using System;
using DispatchRecordSystem;
using LogisticsUX.ViewModels;

namespace LogisticsUX;

public class SessionContainer
{
    public delegate void ViewModelSelectionEventHandler(ViewModelBase entity);
    
    public event ViewModelSelectionEventHandler SelectionChanged;
    public User currentUser { get; set; }

    private ViewModelBase _infoView;
    public ViewModelBase InfoView
    {
        get => _infoView;
        set
        {
            _infoView = value;
            SelectionChanged?.Invoke(_infoView); 
        }
    }

    private IEntity _selectedEntity;

    public IEntity SelectedEntity
    {
        get => _selectedEntity;
        set => _selectedEntity = value;
    }


}