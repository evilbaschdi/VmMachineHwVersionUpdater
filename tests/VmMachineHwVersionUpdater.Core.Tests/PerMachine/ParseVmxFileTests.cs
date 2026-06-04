namespace VmMachineHwVersionUpdater.Core.Tests.PerMachine;

public class ParseVmxFileTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ParseVmxFile).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(ParseVmxFile sut)
    {
        sut.Should().BeAssignableTo<IParseVmxFile>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ParseVmxFile).GetMethods().Where(method => !method.IsAbstract));
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void ValueFor_WithFullVmxFile_ParsesAllProperties(
        VmxLineStartsWith vmxLineStartsWith,
        ReturnValueFromVmxLine returnValueFromVmxLine,
        ConvertAnnotationLineBreaks convertAnnotationLineBreaks)
    {
        // Arrange
        var sut = new ParseVmxFile(vmxLineStartsWith, new LineStartActions(returnValueFromVmxLine, convertAnnotationLineBreaks));
        var tempFile = Path.GetTempFileName();
        var vmxContent = """
                         .encoding = "windows-1252"
                         config.version = "8"
                         virtualhw.version = "21"
                         displayname = "Windows 11 Dev"
                         tools.syncTime = "TRUE"
                         tools.upgrade.policy = "upgradeAtPowerCycle"
                         guestos = "windows9-64"
                         guestOS.detailed.data = "bitness=64 distroName=Windows 11"
                         annotation = "Test VM|0D|0ASecond line"
                         encryption.encryptedKey = "abc123"
                         encryption.keySafe = "keySafeValue"
                         encryption.data = "encData"
                         managedvm.autoAddVTPM = "TRUE"
                         memsize = "4096"
                         mks.enable3d = "TRUE"
                         """;

        try
        {
            File.WriteAllText(tempFile, vmxContent);

            // Act
            var result = sut.ValueFor(tempFile);

            // Assert
            result.Should().NotBeNull();
            result.MachineType.Should().Be(MachineType.Vmx);
            result.HwVersion.Should().Be(21);
            result.DisplayName.Should().Be("Windows 11 Dev");
            result.SyncTimeWithHost.Should().Be("TRUE");
            result.ToolsUpgradePolicy.Should().Be("upgradeAtPowerCycle");
            result.GuestOs.Should().Be("windows9-64");
            result.DetailedData.Should().Be("bitness=64 distroName=Windows 11");
            result.Annotation.Should().Be("Test VM\r\nSecond line");
            result.EncryptionEncryptedKey.Should().Be("abc123");
            result.EncryptionKeySafe.Should().Be("keySafeValue");
            result.EncryptionData.Should().Be("encData");
            result.ManagedVmAutoAddVTpm.Should().Be("TRUE");
            result.MemSize.Should().Be(4096);
            result.MksEnable3D.Should().Be("TRUE");
        }
        finally
        {
            File.Delete(tempFile);
        }
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void ValueFor_WithMinimalVmxFile_ReturnsDefaults(
        VmxLineStartsWith vmxLineStartsWith,
        ReturnValueFromVmxLine returnValueFromVmxLine,
        ConvertAnnotationLineBreaks convertAnnotationLineBreaks)
    {
        // Arrange
        var sut = new ParseVmxFile(vmxLineStartsWith, new LineStartActions(returnValueFromVmxLine, convertAnnotationLineBreaks));
        var tempFile = Path.GetTempFileName();
        var vmxContent = """
                         .encoding = "windows-1252"
                         config.version = "8"
                         """;

        try
        {
            File.WriteAllText(tempFile, vmxContent);

            // Act
            var result = sut.ValueFor(tempFile);

            // Assert
            result.Should().NotBeNull();
            result.MachineType.Should().Be(MachineType.Vmx);
            result.HwVersion.Should().Be(0);
            result.DisplayName.Should().BeEmpty();
            result.GuestOs.Should().BeEmpty();
            result.MemSize.Should().Be(0);
        }
        finally
        {
            File.Delete(tempFile);
        }
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void ValueFor_WithGuestOsDetailedData_PrefersGuestInfoDetailedData(
        VmxLineStartsWith vmxLineStartsWith,
        ReturnValueFromVmxLine returnValueFromVmxLine,
        ConvertAnnotationLineBreaks convertAnnotationLineBreaks)
    {
        // Arrange
        var sut = new ParseVmxFile(vmxLineStartsWith, new LineStartActions(returnValueFromVmxLine, convertAnnotationLineBreaks));
        var tempFile = Path.GetTempFileName();
        var vmxContent = """
                         guestOS.detailed.data = "bitness=64 distroName=Windows 10"
                         guestInfo.detailed.data = "bitness=64 distroName=Windows 11"
                         """;

        try
        {
            File.WriteAllText(tempFile, vmxContent);

            // Act
            var result = sut.ValueFor(tempFile);

            // Assert
            result.DetailedData.Should().Be("bitness=64 distroName=Windows 11");
        }
        finally
        {
            File.Delete(tempFile);
        }
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void ValueFor_WithOnlyGuestInfoDetailedData_UsesGuestInfoData(
        VmxLineStartsWith vmxLineStartsWith,
        ReturnValueFromVmxLine returnValueFromVmxLine,
        ConvertAnnotationLineBreaks convertAnnotationLineBreaks)
    {
        // Arrange
        var sut = new ParseVmxFile(vmxLineStartsWith, new LineStartActions(returnValueFromVmxLine, convertAnnotationLineBreaks));
        var tempFile = Path.GetTempFileName();
        var vmxContent = """
                         guestInfo.detailed.data = "bitness=64 distroName=Ubuntu 22.04"
                         """;

        try
        {
            File.WriteAllText(tempFile, vmxContent);

            // Act
            var result = sut.ValueFor(tempFile);

            // Assert
            result.DetailedData.Should().Be("bitness=64 distroName=Ubuntu 22.04");
        }
        finally
        {
            File.Delete(tempFile);
        }
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void ValueFor_WithNullFile_ThrowsArgumentNullException(
        VmxLineStartsWith vmxLineStartsWith,
        ReturnValueFromVmxLine returnValueFromVmxLine,
        ConvertAnnotationLineBreaks convertAnnotationLineBreaks)
    {
        // Arrange
        var sut = new ParseVmxFile(vmxLineStartsWith, new LineStartActions(returnValueFromVmxLine, convertAnnotationLineBreaks));

        // Act & Assert
        sut.Invoking(x => x.ValueFor(null!))
           .Should().Throw<ArgumentNullException>();
    }
}