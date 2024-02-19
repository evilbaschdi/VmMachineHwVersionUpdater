namespace VmMachineHwVersionUpdater.Core.Settings;

/// <inheritdoc cref="IGuestOsOutputStringMapping" />
/// <summary>
///     Constructor
/// </summary>
/// <param name="guestOsStringMapping"></param>
public class GuestOsOutputStringMapping(IGuestOsStringMapping guestOsStringMapping) : CachedValueFor<string, string>, IGuestOsOutputStringMapping
{
    private readonly IGuestOsStringMapping _guestOsStringMapping = guestOsStringMapping ?? throw new ArgumentNullException(nameof(guestOsStringMapping));

    /// <inheritdoc />
    /// <summary>
    ///     Reads guestOs name string from app.config.
    /// </summary>
    protected override string NonCachedValueFor(string guestOs)
    {
        ArgumentNullException.ThrowIfNull(guestOs);

        var configuration = _guestOsStringMapping.Value;
        var fullName = configuration[guestOs];
        var value = !string.IsNullOrWhiteSpace(fullName) ? fullName : guestOs;
        return value;
    }
}