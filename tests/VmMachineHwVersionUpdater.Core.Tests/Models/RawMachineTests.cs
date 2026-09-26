namespace VmMachineHwVersionUpdater.Core.Tests.Models;

public class RawMachineTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(RawMachine).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(RawMachine sut)
    {
        sut.Should().NotBeNull();
        sut.Should().BeOfType<RawMachine>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(RawMachine).GetMethods()
                                           .Where(method => !method.IsAbstract & !method.Name.StartsWith("set_")));
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void DefaultValues_AreEmptyStrings(RawMachine sut)
    {
        // Assert
        sut.Annotation.Should().BeEmpty();
        sut.DetailedData.Should().BeEmpty();
        sut.DisplayName.Should().BeEmpty();
        sut.EncryptionData.Should().BeEmpty();
        sut.EncryptionEncryptedKey.Should().BeEmpty();
        sut.EncryptionKeySafe.Should().BeEmpty();
        sut.GuestOs.Should().BeEmpty();
        sut.MksEnable3D.Should().BeEmpty();
        sut.ManagedVmAutoAddVTpm.Should().BeEmpty();
        sut.SyncTimeWithHost.Should().BeEmpty();
        sut.ToolsUpgradePolicy.Should().BeEmpty();
        sut.OSType.Should().BeEmpty();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void NumericProperties_DefaultToZero(RawMachine sut)
    {
        // Assert
        sut.HwVersion.Should().Be(0);
        sut.MemSize.Should().Be(0);
        sut.CpuCount.Should().Be(0);
        sut.VirtualBoxHwVersion.Should().Be(0);
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Properties_CanBeSetAndRetrieved(RawMachine sut)
    {
        sut.Annotation = "test annotation";
        sut.DisplayName = "My VM";
        sut.GuestOs = "windows9-64";
        sut.HwVersion = 21;
        sut.MemSize = 4096;
        sut.MachineType = MachineType.Vmx;

        // Assert
        sut.Annotation.Should().Be("test annotation");
        sut.DisplayName.Should().Be("My VM");
        sut.GuestOs.Should().Be("windows9-64");
        sut.HwVersion.Should().Be(21);
        sut.MemSize.Should().Be(4096);
        sut.MachineType.Should().Be(MachineType.Vmx);
    }
}