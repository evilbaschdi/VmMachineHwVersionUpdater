namespace VmMachineHwVersionUpdater.Core.PerMachine;

/// <inheritdoc />
public class ConvertAnnotationLineBreaks : IConvertAnnotationLineBreaks
{
    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public string ValueFor([NotNull] string value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        var r = value.Replace("|0D", "\r");
        var n = r.Replace("|0A", "\n");

        return n;
    }
}