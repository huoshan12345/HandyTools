namespace HandyTools.Utilities.Processing;

public class ProcessException : Exception
{
    public ProcessException(int exitCode, string message) : base(message)
    {
        ExitCode = exitCode;
    }

    public int ExitCode { get; }
}