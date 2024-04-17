namespace DispatchRecordSystem;

public class PasswordInvalidException : ApplicationException
{
    public PasswordInvalidException()
    {
        Console.WriteLine("Invalid password.");
    }
}