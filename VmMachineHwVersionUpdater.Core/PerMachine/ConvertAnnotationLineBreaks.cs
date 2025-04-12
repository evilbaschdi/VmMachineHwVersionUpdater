namespace VmMachineHwVersionUpdater.Core.PerMachine;

/// <inheritdoc />
public class ConvertAnnotationLineBreaks : IConvertAnnotationLineBreaks
{
    /// <inheritdoc />
    public string ValueFor([NotNull] string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        var r = value.Replace("|0D", "\r");
        var n = r.Replace("|0A", "\n");

        return n;
    }
}