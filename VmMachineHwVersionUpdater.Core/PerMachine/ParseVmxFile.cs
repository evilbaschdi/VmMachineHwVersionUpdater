namespace VmMachineHwVersionUpdater.Core.PerMachine;

/// <inheritdoc />
public class ParseVmxFile(
    [NotNull] IVmxLineStartsWith vmxLineStartsWith,
    [NotNull] ILineStartActions lineStartActions)
    : IParseVmxFile
{
    private readonly IVmxLineStartsWith _vmxLineStartsWith = vmxLineStartsWith ?? throw new ArgumentNullException(nameof(vmxLineStartsWith));
    private readonly ILineStartActions _lineStartActions = lineStartActions ?? throw new ArgumentNullException(nameof(lineStartActions));

    /// <inheritdoc />
    public RawMachine ValueFor(string file)
    {
        ArgumentNullException.ThrowIfNull(file);

        var lineStartActions = _lineStartActions.Value;
        var rawMachine = new RawMachine();
        var readAllLines = File.ReadAllLines(file);

        Parallel.ForEach(readAllLines, line =>
                                       {
                                           var lineActions = lineStartActions.Where(action => _vmxLineStartsWith.ValueFor(line, action.Key));
                                           foreach (var action in lineActions)
                                           {
                                               if (action.Key == "guestOS.detailed.data" && !string.IsNullOrWhiteSpace(rawMachine.DetailedData))
                                               {
                                                   continue;
                                               }

                                               action.Value(rawMachine, line);
                                               break;
                                           }
                                       });

        return rawMachine;
    }
}