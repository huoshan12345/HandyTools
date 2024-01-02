namespace HandyTools;

internal class Program
{
    private const string LogPath = "error.txt";

    private static void LogError(Exception ex)
    {
        try
        {
            Console.WriteLine(ex.ToString());
            File.AppendAllText(LogPath, ex.ToString());
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    private static async Task Main(string[] args)
    {
        try
        {
            await CommandProcessor.Process(args);
        }
        catch (Exception ex)
        {
            LogError(ex);
        }
    }
}