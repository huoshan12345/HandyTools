using System.Text;
using CommandLine.Text;
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
        throw new InvalidOperationException(helpText);
    }

    private static async Task AnsibleVault(string command, AnsibleVaultOptions options)
    {
        var path = await ProcessHelper.Wsl.ConvertWinPathToWsl(options.Path);
        var actualCommand = command.Replace("ansible-", "ansible-vault ");
        var builder = new StringBuilder(actualCommand);
        foreach (var id in options.VaultId)
        {
            builder.Append(" --vault-id ").Append(id);
        }
        builder.Append(' ').Append(path);

        var fullCommand = builder.ToString();
        await ProcessHelper.Wsl.ExecuteCommandAsync(fullCommand);
    }

    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(AnsibleVaultOptions))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(AnsibleDecryptOptions))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(AnsibleEncryptOptions))]
    public static async Task Process(string[] args)
    {
        if (args.Length == 0)
        {
            throw new InvalidOperationException("Empty command!");
        }

        var command = args[0];
        var actualArgs = args[1..];
        var task = command.ToLower() switch
        {
            AnsibleEncrypt => Process<AnsibleEncryptOptions>(actualArgs, o => AnsibleVault(command, o)),
            AnsibleDecrypt => Process<AnsibleDecryptOptions>(actualArgs, o => AnsibleVault(command, o)),
            _ => UnhandledCommand(command),
        };
        await task;
        return;

        static Task UnhandledCommand(string command)
        {
            throw new InvalidOperationException("Unsupported command: " + command);
        }
    }
}