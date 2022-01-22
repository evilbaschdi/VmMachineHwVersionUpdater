using System;
using EvilBaschdi.Core;

namespace VmMachineHwVersionUpdater.Core.Settings;

/// <inheritdoc cref="IGuestOsOutputStringMapping" />
public class GuestOsOutputStringMapping : CachedValueFor<string, string>, IGuestOsOutputStringMapping
{
    private readonly IGuestOsStringMapping _guestOsStringMapping;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="guestOsStringMapping"></param>
    public GuestOsOutputStringMapping(IGuestOsStringMapping guestOsStringMapping)
    {
        _guestOsStringMapping = guestOsStringMapping ?? throw new ArgumentNullException(nameof(guestOsStringMapping));
    }

    /// <inheritdoc />
    /// <summary>
    ///     Reads guestOs name string from app.config.
    /// </summary>
    protected override string NonCachedValueFor(string guestOs)
    {
        if (guestOs == null)
        {
            throw new ArgumentNullException(nameof(guestOs));
        }

        var configuration = _guestOsStringMapping.Value;
        var fullName = configuration[guestOs];
        var value = !string.IsNullOrWhiteSpace(fullName) ? fullName : guestOs;
        return value;
    }
}