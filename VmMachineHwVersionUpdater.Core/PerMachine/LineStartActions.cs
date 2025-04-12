namespace VmMachineHwVersionUpdater.Core.PerMachine;

/// <inheritdoc />
public class LineStartActions(
    [NotNull] IReturnValueFromVmxLine returnValueFromVmxLine,
    [NotNull] IConvertAnnotationLineBreaks convertAnnotationLineBreaks)
    : ILineStartActions
{
    private readonly IReturnValueFromVmxLine _returnValueFromVmxLine = returnValueFromVmxLine ?? throw new ArgumentNullException(nameof(returnValueFromVmxLine));

    private readonly IConvertAnnotationLineBreaks
        _convertAnnotationLineBreaks = convertAnnotationLineBreaks ?? throw new ArgumentNullException(nameof(convertAnnotationLineBreaks));

    /// <inheritdoc />
    public Dictionary<string, Action<RawMachine, string>> Value => new()
                                                                   {
                                                                       {
                                                                           "virtualhw.version",
                                                                           (machine, line) =>
                                                                               machine.HwVersion = Convert.ToInt32(_returnValueFromVmxLine.ValueFor(line, "virtualhw.version"))
                                                                       },
                                                                       {
                                                                           "displayname",
                                                                           (machine, line) =>
                                                                               machine.DisplayName = _returnValueFromVmxLine.ValueFor(line, "displayname")
                                                                       },
                                                                       {
                                                                           "tools.syncTime",
                                                                           (machine, line) =>
                                                                               machine.SyncTimeWithHost = _returnValueFromVmxLine.ValueFor(line, "tools.syncTime")
                                                                       },
                                                                       {
                                                                           "tools.upgrade.policy",
                                                                           (machine, line) =>
                                                                               machine.ToolsUpgradePolicy = _returnValueFromVmxLine.ValueFor(line, "tools.upgrade.policy")
                                                                       },
                                                                       {
                                                                           "guestos",
                                                                           (machine, line) =>
                                                                               machine.GuestOs = _returnValueFromVmxLine.ValueFor(line, "guestos")
                                                                       },
                                                                       {
                                                                           "guestOS.detailed.data",
                                                                           (machine, line) =>
                                                                               machine.DetailedData = _returnValueFromVmxLine.ValueFor(line, "guestOS.detailed.data")
                                                                       },
                                                                       {
                                                                           "guestInfo.detailed.data",
                                                                           (machine, line) =>
                                                                               machine.DetailedData = _returnValueFromVmxLine.ValueFor(line, "guestInfo.detailed.data")
                                                                       },
                                                                       {
                                                                           "annotation",
                                                                           (machine, line) =>
                                                                               machine.Annotation =
                                                                                   _convertAnnotationLineBreaks.ValueFor(_returnValueFromVmxLine.ValueFor(line, "annotation"))
                                                                       },
                                                                       {
                                                                           "encryption.encryptedKey",
                                                                           (machine, line) =>
                                                                               machine.EncryptionEncryptedKey = _returnValueFromVmxLine.ValueFor(line, "encryption.encryptedKey")
                                                                       },
                                                                       {
                                                                           "encryption.keySafe",
                                                                           (machine, line) =>
                                                                               machine.EncryptionKeySafe = _returnValueFromVmxLine.ValueFor(line, "encryption.keySafe")
                                                                       },
                                                                       {
                                                                           "encryption.data",
                                                                           (machine, line) => machine.EncryptionData = _returnValueFromVmxLine.ValueFor(line, "encryption.data")
                                                                       },
                                                                       {
                                                                           "managedvm.autoAddVTPM",
                                                                           (machine, line) =>
                                                                               machine.ManagedVmAutoAddVTpm = _returnValueFromVmxLine.ValueFor(line, "managedvm.autoAddVTPM")
                                                                       },
                                                                       {
                                                                           "memsize",
                                                                           (machine, line) =>
                                                                               machine.MemSize = Convert.ToInt32(_returnValueFromVmxLine.ValueFor(line, "memsize"))
                                                                       }

                                                                       // Add other actions here...
                                                                   };
}