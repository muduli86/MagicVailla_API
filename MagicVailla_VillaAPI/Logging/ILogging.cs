namespace MagicVilla_VillaAPI.Logging
{
    public interface ILogging
    {
        public void LogInformation(string message);
        public void LogError(string message);
    }
}
