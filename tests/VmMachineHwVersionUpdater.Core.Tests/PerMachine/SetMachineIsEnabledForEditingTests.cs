namespace VmMachineHwVersionUpdater.Core.Tests.PerMachine;

public class SetMachineIsEnabledForEditingTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(SetMachineIsEnabledForEditing).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(SetMachineIsEnabledForEditing sut)
    {
        sut.Should().BeAssignableTo<ISetMachineIsEnabledForEditing>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(SetMachineIsEnabledForEditing).GetMethods().Where(method => !method.IsAbstract));
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void RunFor_WhenMachineStateIsPaused_SetsIsEnabledForEditingToFalse(
        SetMachineIsEnabledForEditing sut,
        Machine machine)
    {
        // Arrange
        machine.MachineState = MachineState.Paused;

        // Act
        sut.RunFor(machine);

        // Assert
        machine.IsEnabledForEditing.Should().BeFalse();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void RunFor_WhenEncryptedWithoutGuestOs_SetsIsEnabledForEditingToFalse(
        SetMachineIsEnabledForEditing sut,
        IToggleToolsSyncTime toggleToolsSyncTime,
        IToggleToolsUpgradePolicy toggleToolsUpgradePolicy,
        IToggleMksEnable3D toggleMksEnable3D,
        IUpdateMachineVersion updateMachineVersion,
        IUpdateMachineMemSize updateMachineMemSize)
    {
        // Arrange
        var machine = new Machine(toggleToolsSyncTime, toggleToolsUpgradePolicy, toggleMksEnable3D, updateMachineVersion, updateMachineMemSize)
                      {
                          MachineState = MachineState.Off,
                          EncryptionKeySafe = "some-key-safe",
                          EncryptionData = "some-data",
                          GuestOs = null
                      };

        // Act
        sut.RunFor(machine);

        // Assert
        machine.IsEnabledForEditing.Should().BeFalse();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void RunFor_WhenNotPausedAndNotEncrypted_SetsIsEnabledForEditingToTrue(
        SetMachineIsEnabledForEditing sut,
        Machine machine)
    {
        // Arrange
        machine.MachineState = MachineState.Off;
        machine.GuestOs = "windows9-64";

        // Act
        sut.RunFor(machine);

        // Assert
        machine.IsEnabledForEditing.Should().BeTrue();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void RunFor_WithNullMachine_ThrowsArgumentNullException(
        SetMachineIsEnabledForEditing sut)
    {
        // Act & Assert
        var act = () => sut.RunFor(null!);
        act.Should().Throw<ArgumentNullException>()
           .WithParameterName("machine");
    }
}