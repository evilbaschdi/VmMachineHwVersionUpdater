namespace VmMachineHwVersionUpdater.Core.PerMachine;

/// <inheritdoc />
public class LineStartActions : ILineStartActions
{
    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="returnValueFromVmxLine"></param>
    /// <param name="convertAnnotationLineBreaks"></param>
    public LineStartActions([NotNull] IReturnValueFromVmxLine returnValueFromVmxLine,
                            [NotNull] IConvertAnnotationLineBreaks convertAnnotationLineBreaks)
    {
        ArgumentNullException.ThrowIfNull(returnValueFromVmxLine);
        ArgumentNullException.ThrowIfNull(convertAnnotationLineBreaks);

        Value = new(StringComparer.OrdinalIgnoreCase)
                {
                    //Note: Add other actions here...
                    ["virtualhw.version"]
                        = (machine, line) => machine.HwVersion = int.Parse(returnValueFromVmxLine.ValueFor(line, "virtualhw.version")),
                    ["displayname"]
                        = (machine, line) => machine.DisplayName = returnValueFromVmxLine.ValueFor(line, "displayname"),
                    ["tools.syncTime"]
                        = (machine, line) => machine.SyncTimeWithHost = returnValueFromVmxLine.ValueFor(line, "tools.syncTime"),
                    ["tools.upgrade.policy"]
                        = (machine, line) => machine.ToolsUpgradePolicy = returnValueFromVmxLine.ValueFor(line, "tools.upgrade.policy"),
                    ["guestos"]
                        = (machine, line) => machine.GuestOs = returnValueFromVmxLine.ValueFor(line, "guestos"),
                    ["guestOS.detailed.data"]
                        = (machine, line) => machine.DetailedData = returnValueFromVmxLine.ValueFor(line, "guestOS.detailed.data"),
                    ["guestInfo.detailed.data"]
                        = (machine, line) => machine.DetailedData = returnValueFromVmxLine.ValueFor(line, "guestInfo.detailed.data"),
                    ["annotation"]
                        = (machine, line) => machine.Annotation = convertAnnotationLineBreaks.ValueFor(returnValueFromVmxLine.ValueFor(line, "annotation")),
                    ["encryption.encryptedKey"]
                        = (machine, line) => machine.EncryptionEncryptedKey = returnValueFromVmxLine.ValueFor(line, "encryption.encryptedKey"),
                    ["encryption.keySafe"]
                        = (machine, line) => machine.EncryptionKeySafe = returnValueFromVmxLine.ValueFor(line, "encryption.keySafe"),
                    ["encryption.data"]
                        = (machine, line) => machine.EncryptionData = returnValueFromVmxLine.ValueFor(line, "encryption.data"),
                    ["managedvm.autoAddVTPM"]
                        = (machine, line) => machine.ManagedVmAutoAddVTpm = returnValueFromVmxLine.ValueFor(line, "managedvm.autoAddVTPM"),
                    ["memsize"]
                        = (machine, line) => machine.MemSize = int.Parse(returnValueFromVmxLine.ValueFor(line, "memsize")),
                    ["mks.enable3d"]
                        = (machine, line) => machine.MksEnable3D = returnValueFromVmxLine.ValueFor(line, "mks.enable3d")
                };
    }

    /// <inheritdoc />
    public Dictionary<string, Action<RawMachine, string>> Value { get; }
}