using System.Text.RegularExpressions;

namespace VmMachineHwVersionUpdater.Core.PerMachine;

/// <inheritdoc />
public class ReturnValueFromVmxLine : IReturnValueFromVmxLine
{
    /// <inheritdoc />
    public string ValueFor([NotNull] string line, [NotNull] string key)
    {
        ArgumentNullException.ThrowIfNull(line);

        ArgumentNullException.ThrowIfNull(key);

        var value = line.Replace('"', ' ').Trim();
        value = Regex.Replace(value, $"{key} =", "", RegexOptions.IgnoreCase).Trim();

        return value;
    }
}