using CommandLine.Text;
using CommandLine;
using HandyTools.Options;
using HandyTools.Utilities.Processing;

namespace HandyTools.Utilities;

public static class CommandProcessor
{
    private const string AnsibleEncrypt = "ansible-encrypt";
    private const string AnsibleDecrypt = "ansible-decrypt";

    private static async Task Process<TOptions>(IEnumerable<string> args, Func<TOptions, Task> task)
    {
        var result = Parser.Default.ParseArguments<TOptions>(args);

        if (result is Parsed<TOptions> parsed)
        {
            await task(parsed.Value);
            return;
        }

        var helpText = HelpText.AutoBuild(result, h => h, e => e);
        Console.WriteLine(helpText);
    }

    private static async Task AnsibleVault(string command, AnsibleVaultOptions options)
    {
        var path = await ProcessHelper.Wsl.ConvertWinPathToWsl(options.Path);
        var actualCommand = command.Replace("ansible-", "ansible-vault ");
        var fullCommand = $"{actualCommand} --vault-id {options.VaultId} {path}";
        await ProcessHelper.Wsl.ExecuteCommandAsync(fullCommand);
    }

    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(AnsibleVaultOptions))]
    public static async Task Process(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Empty command!");
            return;
        }

        var command = args[0];
        var actualArgs = args[1..];
        var task = command.ToLower() switch
        {
            AnsibleEncrypt or AnsibleDecrypt => Process<AnsibleVaultOptions>(actualArgs, o => AnsibleVault(command, o)),
            _ => UnhandledCommand(command),
        };
        await task;
        return;

        static Task UnhandledCommand(string command)
        {
            Console.WriteLine("Unsupported command: " + command);
            return Task.CompletedTask;
        }
    }
}