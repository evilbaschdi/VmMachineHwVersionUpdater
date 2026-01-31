namespace VmMachineHwVersionUpdater.Core.Strategies;

/// <inheritdoc />
public class MachineParserStrategy : IMachineParserStrategy
{
    private readonly IParseVmxFile _parseVmxFile;
    private readonly IParseVboxFile _parseVboxFile;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="parseVmxFile"></param>
    /// <param name="parseVboxFile"></param>
    public MachineParserStrategy([NotNull] IParseVmxFile parseVmxFile, [NotNull] IParseVboxFile parseVboxFile)
    {
        _parseVmxFile = parseVmxFile ?? throw new ArgumentNullException(nameof(parseVmxFile));
        _parseVboxFile = parseVboxFile ?? throw new ArgumentNullException(nameof(parseVboxFile));
    }

    /// <inheritdoc />
    public RawMachine Parse([NotNull] string filePath)
    {
        ArgumentNullException.ThrowIfNull(filePath);

        var extension = Path.GetExtension(filePath);

        return extension.ToLowerInvariant() switch
        {
            ".vmx" => _parseVmxFile.ValueFor(filePath),
            ".vbox" => _parseVboxFile.ValueFor(filePath),
            _ => throw new NotSupportedException($"File extension '{extension}' is not supported.")
        };
    }
}