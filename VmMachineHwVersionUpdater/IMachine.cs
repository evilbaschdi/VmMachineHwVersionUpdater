namespace VmMachineHwVersionUpdater
{
    public interface IMachine
    {
        string Id { get; set; }

        string DisplayName { get; set; }

        string Path { get; set; }

        int HwVersion { get; set; }

        string GuestOs { get; set; }
    }
}