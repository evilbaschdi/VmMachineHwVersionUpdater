namespace VmMachineHwVersionUpdater.Internal
{
    public interface IMachine
    {
        string Id { get; set; }

        string DisplayName { get; set; }

        string ShortPath { get; set; }

        string Path { get; set; }

        string DirectorySize { get; set; }

        double DirectorySizeGb { get; set; }

        int HwVersion { get; set; }

        string GuestOs { get; set; }
    }
}