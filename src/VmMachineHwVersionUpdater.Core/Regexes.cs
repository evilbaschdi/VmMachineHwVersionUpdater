using System.Text.RegularExpressions;

namespace VmMachineHwVersionUpdater.Core;

/// <summary>
///     Class to contain <see cref="Regex" /> definitions
/// </summary>
// ReSharper disable once PartialTypeWithSinglePart
public static partial class Regexes
{
    /// <summary>
    ///     <see cref="Regex" /> to parsed <see cref="RawMachine" />.DetailedData
    /// </summary>
    [GeneratedRegex(@"(\w+)='([^']*)'")]
    public static partial Regex GuestInfoDetailedDataParser { get; }
}