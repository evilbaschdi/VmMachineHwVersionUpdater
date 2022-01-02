using System;
using System.IO;
using System.Threading.Tasks;
using JetBrains.Annotations;
using VmMachineHwVersionUpdater.Core.Models;

namespace VmMachineHwVersionUpdater.Core.PerMachine;

/// <inheritdoc />
public class ParseVmxFile : IParseVmxFile
{
    [NotNull] private readonly IConvertAnnotationLineBreaks _convertAnnotationLineBreaks;
    [NotNull] private readonly IReturnValueFromVmxLine _returnValueFromVmxLine;
    [NotNull] private readonly IVmxLineStartsWith _vmxLineStartsWith;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="returnValueFromVmxLine"></param>
    /// <param name="vmxLineStartsWith"></param>
    /// <param name="convertAnnotationLineBreaks"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public ParseVmxFile([NotNull] IReturnValueFromVmxLine returnValueFromVmxLine, [NotNull] IVmxLineStartsWith vmxLineStartsWith,
                        [NotNull] IConvertAnnotationLineBreaks convertAnnotationLineBreaks)
    {
        _returnValueFromVmxLine = returnValueFromVmxLine ?? throw new ArgumentNullException(nameof(returnValueFromVmxLine));
        _vmxLineStartsWith = vmxLineStartsWith ?? throw new ArgumentNullException(nameof(vmxLineStartsWith));
        _convertAnnotationLineBreaks = convertAnnotationLineBreaks ?? throw new ArgumentNullException(nameof(convertAnnotationLineBreaks));
    }

    /// <inheritdoc />
    public RawMachine ValueFor(string file)
    {
        var rawMachine = new RawMachine();
        var readAllLines = File.ReadAllLines(file);

        Parallel.ForEach(readAllLines,
            line =>
            {
                // ReSharper disable StringLiteralTypo
                switch (line, StringComparer.InvariantCultureIgnoreCase)
                {
                    case var _ when _vmxLineStartsWith.ValueFor(line, "virtualhw.version"):
                        var rawHwVersion = _returnValueFromVmxLine.ValueFor(line, "virtualhw.version");
                        rawMachine.HwVersion = !string.IsNullOrWhiteSpace(rawHwVersion) ? Convert.ToInt32(rawHwVersion) : 0;
                        break;
                    case var _ when _vmxLineStartsWith.ValueFor(line, "displayname"):
                        rawMachine.DisplayName = _returnValueFromVmxLine.ValueFor(line, "displayname");
                        break;
                    case var _ when _vmxLineStartsWith.ValueFor(line, "tools.syncTime"):
                        rawMachine.SyncTimeWithHost = _returnValueFromVmxLine.ValueFor(line, "tools.syncTime");
                        break;
                    case var _ when _vmxLineStartsWith.ValueFor(line, "tools.upgrade.policy"):
                        rawMachine.ToolsUpgradePolicy = _returnValueFromVmxLine.ValueFor(line, "tools.upgrade.policy");
                        break;
                    case var _ when _vmxLineStartsWith.ValueFor(line, "guestos")
                                    && !_vmxLineStartsWith.ValueFor(line, "guestos.detailed.data")
                                    && !_vmxLineStartsWith.ValueFor(line, "guestInfo.detailed.data"):
                        rawMachine.GuestOs = _returnValueFromVmxLine.ValueFor(line, "guestos");
                        break;
                    case var _ when _vmxLineStartsWith.ValueFor(line, "guestOS.detailed.data") && string.IsNullOrWhiteSpace(rawMachine.DetailedData):
                        rawMachine.DetailedData = _returnValueFromVmxLine.ValueFor(line, "guestOS.detailed.data");
                        break;
                    case var _ when _vmxLineStartsWith.ValueFor(line, "guestInfo.detailed.data"):
                        rawMachine.DetailedData = _returnValueFromVmxLine.ValueFor(line, "guestInfo.detailed.data");
                        break;
                    case var _ when _vmxLineStartsWith.ValueFor(line, "annotation"):
                        var rawAnnotation = _returnValueFromVmxLine.ValueFor(line, "annotation");
                        rawMachine.Annotation = _convertAnnotationLineBreaks.ValueFor(rawAnnotation);
                        break;
                    case var _ when _vmxLineStartsWith.ValueFor(line, "encryption.encryptedKey"):
                        rawMachine.EncryptionEncryptedKey = _returnValueFromVmxLine.ValueFor(line, "encryption.encryptedKey");
                        break;
                    case var _ when _vmxLineStartsWith.ValueFor(line, "encryption.keySafe"):
                        rawMachine.EncryptionKeySafe = _returnValueFromVmxLine.ValueFor(line, "encryption.keySafe");
                        break;
                    case var _ when _vmxLineStartsWith.ValueFor(line, "encryption.data"):
                        rawMachine.EncryptionData = _returnValueFromVmxLine.ValueFor(line, "encryption.data");
                        break;
                    case var _ when _vmxLineStartsWith.ValueFor(line, "managedvm.autoAddVTPM"):
                        rawMachine.ManagedVmAutoAddVTpm = _returnValueFromVmxLine.ValueFor(line, "managedvm.autoAddVTPM");
                        break;
                }
                // ReSharper restore StringLiteralTypo
            });

        return rawMachine;
    }
}