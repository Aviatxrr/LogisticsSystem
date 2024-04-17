using System;
using System.Linq;
using System.Reactive;
using System.Runtime.InteropServices;
using Avalonia.Media;
using DispatchRecordSystem;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

namespace LogisticsUX.ViewModels;

public class LoginViewModel : ViewModelBase
{
    private UserService userService;
    private Repository<User> users;
    private INavigationService navigationService;
    private string username;
    public string Username
    {
        get => username;
        set => this.RaiseAndSetIfChanged(ref username, value);
        
    }

    private string password;
    public string Password
    {
        get => password;
        set => this.RaiseAndSetIfChanged(ref password, value);
    }

    private Brush userBorder;

    public Brush UserBorder
    {
        get => userBorder;
        set => this.RaiseAndSetIfChanged(ref userBorder, value);
    }

    private Brush passwordBorder;
    public Brush PasswordBorder
    {
        get => passwordBorder;
        set => this.RaiseAndSetIfChanged(ref passwordBorder, value);
    }
    public ReactiveCommand<Unit, Unit> Login { get; }
    
    public LoginViewModel()
    {
        Login = ReactiveCommand.Create(OnLoginClick);
        userService = App.AppHost!.Services.GetRequiredService<UserService>();
        users = App.AppHost.Services.GetRequiredService<Repository<User>>();
        navigationService = App.AppHost.Services.GetRequiredService<INavigationService>();
        PasswordBorder = new SolidColorBrush(Colors.White);
        UserBorder = new SolidColorBrush(Colors.White);
    }

    public void OnLoginClick()
    {
        PasswordBorder = new SolidColorBrush(Colors.White);
        UserBorder = new SolidColorBrush(Colors.White);
        try
        {
            bool confirmLogin;
            confirmLogin = userService.ConfirmLogin(username, password);
            if (!confirmLogin)
            {
                PasswordBorder = new SolidColorBrush(Colors.Red);
                Password = "";
                throw new PasswordInvalidException();
            }

            App.AppHost.Services.GetRequiredService<SessionContainer>().currentUser =
                users.Find(new ByUserName(Username)).First();
            navigationService.NavigateTo<HomeViewModel>();
        }
        catch (UserNotFoundException)
        {
            UserBorder = new SolidColorBrush(Colors.Red);
        }
    }
}