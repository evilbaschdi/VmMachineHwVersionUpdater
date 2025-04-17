namespace VmMachineHwVersionUpdater.Core.Settings;

/// <inheritdoc cref="SettingsFromJsonFile" />
public class GuestOsStringMapping() : SettingsFromJsonFile("Settings/GuestOsStringMapping.json"), IGuestOsStringMapping
{
    //https://github.com/vmware/open-vm-tools/blob/master/open-vm-tools/lib/include/guest_os_tables.h
    //https://gist.github.com/dcode/1003a83388d770b978f723e37ce09e42
}