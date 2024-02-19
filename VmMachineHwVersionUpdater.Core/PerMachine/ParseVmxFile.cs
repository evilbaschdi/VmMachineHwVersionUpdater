namespace VmMachineHwVersionUpdater.Core.PerMachine;

/// <inheritdoc />
public class ParseVmxFile : IParseVmxFile
{
    private readonly IVmxLineStartsWith _vmxLineStartsWith;
    private readonly ILineStartActions _lineStartActions;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="vmxLineStartsWith"></param>
    /// <param name="lineStartActions"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public ParseVmxFile(
        [NotNull] IVmxLineStartsWith vmxLineStartsWith,
        [NotNull] ILineStartActions lineStartActions)
    {
        _vmxLineStartsWith = vmxLineStartsWith ?? throw new ArgumentNullException(nameof(vmxLineStartsWith));
        _lineStartActions = lineStartActions ?? throw new ArgumentNullException(nameof(lineStartActions));
    }

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