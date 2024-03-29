﻿namespace VmMachineHwVersionUpdater.Core.BasicApplication;

/// <inheritdoc />
/// <summary>
///     Constructor
/// </summary>
/// <param name="guestOsStringMapping"></param>
/// <param name="load"></param>
public class GuestOsesInUse(
    [NotNull] IGuestOsStringMapping guestOsStringMapping,
    [NotNull] ILoad load) : IGuestOsesInUse
{
    private readonly IGuestOsStringMapping _guestOsStringMapping = guestOsStringMapping ?? throw new ArgumentNullException(nameof(guestOsStringMapping));
    private readonly ILoad _load = load ?? throw new ArgumentNullException(nameof(load));

    /// <inheritdoc />
    public List<string> Value
    {
        get
        {
            var list = new List<string>();
            var configuration = _guestOsStringMapping.Value;
            var searchOsItems = _load.Value?.SearchOsItems ?? [];

            foreach (var configurationSection in configuration.GetChildren())
            {
                var configurationSectionValue = configurationSection.Value;
                if (configurationSectionValue == null || !searchOsItems.Contains(configurationSectionValue, StringComparer.OrdinalIgnoreCase))
                {
                    continue;
                }

                var os = configurationSectionValue.Contains(' ') ? configurationSectionValue.Split(' ')[0] : configurationSectionValue;

                if (!list.Contains(os, StringComparer.OrdinalIgnoreCase))
                {
                    list.Add(os);
                }
            }

            return list.Distinct().OrderBy(x => x).ToList();
        }
    }
}