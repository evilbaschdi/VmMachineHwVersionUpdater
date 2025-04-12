using System.Collections.Concurrent;

namespace VmMachineHwVersionUpdater.Core.BasicApplication;

/// <inheritdoc />
public class GuestOsesInUse(
    [NotNull] IGuestOsStringMapping guestOsStringMapping,
    [NotNull] ILoad load)
    : IGuestOsesInUse
{
    private readonly IGuestOsStringMapping _guestOsStringMapping = guestOsStringMapping ?? throw new ArgumentNullException(nameof(guestOsStringMapping));
    private readonly ILoad _load = load ?? throw new ArgumentNullException(nameof(load));

    /// <inheritdoc />
    public ConcurrentDictionary<string, bool> Value
    {
        get
        {
            var concurrentDictionary = new ConcurrentDictionary<string, bool>();
            var configuration = _guestOsStringMapping.Value;
            var searchOsItems = _load.Value?.SearchOsItems ?? [];

            Parallel.ForEach(configuration.GetChildren(),
                configurationSection =>
                {
                    var configurationSectionValue = configurationSection.Value;
                    if (string.IsNullOrWhiteSpace(configurationSectionValue) || !searchOsItems.ContainsKey(configurationSectionValue))
                    {
                        return;
                    }

                    var os = configurationSectionValue.Contains(' ') ? configurationSectionValue.Split(' ')[0] : configurationSectionValue;

                    if (!concurrentDictionary.ContainsKey(os))
                    {
                        concurrentDictionary.TryAdd(os, true);
                    }
                });

            return concurrentDictionary;
        }
    }
}