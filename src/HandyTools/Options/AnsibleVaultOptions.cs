namespace HandyTools.Options;

public abstract class AnsibleVaultOptions
{
    [Value(0, Required = true)]
    public string Path { get; set; } = default!;

    [Option("vault-id", Required = false, Default = new[] { Constants.VaultIds.Huoshan })]
    public virtual IEnumerable<string> VaultId { get; set; } = default!;
}