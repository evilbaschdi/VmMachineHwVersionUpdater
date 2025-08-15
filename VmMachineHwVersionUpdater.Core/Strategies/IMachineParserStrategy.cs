namespace VmMachineHwVersionUpdater.Core.Strategies;

/// <summary>
///     Strategy to parse a machine file
/// </summary>
public interface IMachineParserStrategy
{
    /// <summary>
    ///     Parses a machine file and returns a RawMachine
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    RawMachine Parse(string filePath);
}