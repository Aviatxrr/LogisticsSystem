using System;
using ReactiveUI;

namespace LogisticsUX;

public class UserMessage : ReactiveObject
{
    public event EventHandler UserMessageChanged;
    private string _contents;
    public string Contents
    {
        get => _contents;
        set
        { 
            this.RaiseAndSetIfChanged(ref _contents, value);
            if (UserMessageChanged != null) UserMessageChanged.Invoke(this, EventArgs.Empty);
        }
    }

    public UserMessage()
    {
        Contents = "HI";
    }
}