using System;

namespace DispatchRecordSystem;

public class UserNotFoundException : ApplicationException
{
    public UserNotFoundException(string userName)
    {
        Console.WriteLine($"User {userName} not found.");
    }
}