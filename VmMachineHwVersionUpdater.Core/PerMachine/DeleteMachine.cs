namespace VmMachineHwVersionUpdater.Core.PerMachine;

/// <inheritdoc />
public class DeleteMachine : IDeleteMachine
{
    /// <inheritdoc />
    public void RunFor([NotNull] string path)
    {
        ArgumentNullException.ThrowIfNull(path);

        if (!File.Exists(path))
        {
            return;
        }

        var directoryName = Path.GetDirectoryName(path);
        if (!string.IsNullOrWhiteSpace(directoryName))
        {
            Directory.Delete(directoryName, true);
        }
    }
}