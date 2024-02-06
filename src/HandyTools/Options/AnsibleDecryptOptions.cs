namespace HandyTools.Options;

public class AnsibleDecryptOptions : AnsibleVaultOptions
{
    [Option("vault-id", Required = false, Default = new[] { Constants.VaultIds.Huoshan, Constants.VaultIds.Dev })]
    public override IEnumerable<string> VaultId { get; set; } = default!;
}