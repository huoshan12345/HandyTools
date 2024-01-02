namespace HandyTools.Utilities.Processing;

public static class WslHelperExtensions
{
    public static Task<string> ConvertWinPathToWsl(this WslHelper wsl, string path)
    {
        return wsl.ExecuteCommandAsync($"wslpath '{path}'");
    }
}