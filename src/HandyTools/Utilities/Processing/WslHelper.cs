using System.Collections.Concurrent;
using System.Diagnostics;

namespace HandyTools.Utilities.Processing;

public class WslHelper
{
    // We use instance method here to make extenstion methods possible
    // ReSharper disable once MemberCanBeMadeStatic.Global
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
    public async Task<string> ExecuteCommandAsync(WslCommand command)
    {
        var text = command.StripCarriageReturn
            ? command.CommandText.Replace("\r", "")
            : command.CommandText;

        // ReSharper disable once UsingStatementResourceInitialization
        using var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "bash.exe",
                Arguments = $"-c \"{text}\"",
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                UseShellExecute = false,
                WorkingDirectory = command.WorkingDirectory,
            },
            EnableRaisingEvents = true,

        };
        var queue = new ConcurrentQueue<string?>();
        process.OutputDataReceived += (sender, e) => queue.Enqueue(e.Data);
        process.ErrorDataReceived += (sender, e) => queue.Enqueue(e.Data);
        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

#if NETSTANDARD2_0
        await Task.Yield();
        process.WaitForExit();
#else
        await process.WaitForExitAsync();
#endif

        var output = queue.Where(m => m is not null).JoinWith(Environment.NewLine);

        if (process.ExitCode != 0)
            throw new ProcessException(process.ExitCode, output);

        return output;
    }
}