namespace VmMachineHwVersionUpdater.Core.PerMachine;

/// <inheritdoc />
public class ParseGuestInfoDetailedData : IParseGuestInfoDetailedData
{
    /// <inheritdoc />
    public Dictionary<string, string> ValueFor([NotNull] string guestInfoDetailedData)
    {
        ArgumentNullException.ThrowIfNull(guestInfoDetailedData);

        //"architecture='X86' bitness='64' buildNumber='26220' distroName='Windows' distroVersion='10.0' familyName='Windows' kernelVersion='26220.9202' prettyName='Windows 11 Home, 64-bit (Build 26220.9202)'"
        //"architecture='X86' bitness='64' distroAddlVersion='26.04 LTS (Resolute Raccoon)' distroName='Ubuntu' distroVersion='26.04' familyName='Linux' kernelVersion='7.0.0-29-generic' prettyName='Ubuntu 26.04 LTS'"
        //"architecture='X86' bitness='64' distroAddlVersion='13 (trixie)' distroName='Debian GNU/Linux' distroVersion='13' familyName='Linux' kernelVersion='6.12.95+deb13-amd64' prettyName='Debian GNU/Linux 13 (trixie)'"
        //"architecture='X86' bitness='64' distroAddlVersion='2026.3' distroName='Kali GNU/Linux' distroVersion='2026.3' familyName='Linux' kernelVersion='6.19.14+kali-amd64' prettyName='Kali GNU/Linux Rolling'"
        //"architecture='X86' bitness='64' cpeString='cpe:/o:fedoraproject:fedora:44' distroAddlVersion='44 (Workstation Edition)' distroName='Fedora Linux' distroVersion='44' familyName='Linux' kernelVersion='7.1.6-201.fc44.x86_64' prettyName='Fedora Linux 44 (Workstation Edition)'"
        //"architecture='X86' bitness='64' buildNumber='rolling' distroName='CachyOS Linux' familyName='Linux' kernelVersion='6.18.20-1-cachyos-lts' prettyName='CachyOS'"

        var matches = Regexes.GuestInfoDetailedDataParser.Matches(guestInfoDetailedData);

        return matches.ToDictionary(m => m.Groups[1].Value, m => m.Groups[2].Value);
    }
}