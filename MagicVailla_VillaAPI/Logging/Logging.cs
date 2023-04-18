namespace MagicVilla_VillaAPI.Logging;

public class Logging : ILogging
{
    public void LogInformation(string message)
    {
        Console.WriteLine(message);
    }

    public void LogError(string message)
    {
        Console.WriteLine("Error-" + message);
    }
}