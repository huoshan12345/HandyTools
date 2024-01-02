using CommandLine;

namespace HandyTools.Options;

public class AnsibleVaultOptions
{
    [Value(0, Required = true)]
    public string Path { get; set; } = default!;

    [Option(Required = false, Default = "dev@~/.ansible_vault_password_dev")]
    public string VaultId { get; set; } = default!;
}