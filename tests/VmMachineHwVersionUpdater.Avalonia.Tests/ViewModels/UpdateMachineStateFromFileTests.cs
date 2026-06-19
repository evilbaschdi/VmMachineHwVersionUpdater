using Avalonia.Headless.XUnit;
using Avalonia.Threading;
using VmMachineHwVersionUpdater.Avalonia.ViewModels;

namespace VmMachineHwVersionUpdater.Avalonia.Tests.ViewModels;

public class UpdateMachineStateFromFileTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(UpdateMachineStateFromFile).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(UpdateMachineStateFromFile sut)
    {
        sut.Should().BeAssignableTo<IUpdateMachineStateFromFile>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(UpdateMachineStateFromFile).GetMethods().Where(method => !method.IsAbstract));
    }

    #region UpdateFor Tests

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void UpdateFor_WithEmptyCollection_DoesNotThrow(UpdateMachineStateFromFile sut)
    {
        // Arrange
        var filePath = @"C:\VMs\Server1\vmware.log";
        var loadHelper = new LoadHelper { VmDataGridItemsSource = [] };

        // Act
        var act = () => sut.UpdateFor(filePath, loadHelper);

        // Assert
        act.Should().NotThrow();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void UpdateFor_WithNoMatchingMachine_DoesNotThrow(
        UpdateMachineStateFromFile sut,
        IToggleToolsSyncTime toggleToolsSyncTime,
        IToggleToolsUpgradePolicy toggleToolsUpgradePolicy,
        IToggleMksEnable3D toggleMksEnable3D,
        IUpdateMachineVersion updateMachineVersion,
        IUpdateMachineMemSize updateMachineMemSize)
    {
        // Arrange
        var filePath = @"C:\VMs\Server1\vmware.log";
        var otherMachine = new Machine(toggleToolsSyncTime, toggleToolsUpgradePolicy, toggleMksEnable3D, updateMachineVersion, updateMachineMemSize)
                           { Path = @"C:\VMs\Server2\Server2.vmx", DisplayName = "Other" };
        var loadHelper = new LoadHelper { VmDataGridItemsSource = [otherMachine] };

        // Act
        var act = () => sut.UpdateFor(filePath, loadHelper);

        // Assert
        act.Should().NotThrow();
    }

    [AvaloniaTheory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public async Task UpdateFor_WithLogFile_UpdatesLogInformation(
        [Frozen] IReadLogInformation readLogInformation,
        UpdateMachineStateFromFile sut,
        IToggleToolsSyncTime toggleToolsSyncTime,
        IToggleToolsUpgradePolicy toggleToolsUpgradePolicy,
        IToggleMksEnable3D toggleMksEnable3D,
        IUpdateMachineVersion updateMachineVersion,
        IUpdateMachineMemSize updateMachineMemSize)
    {
        // Arrange
        var machineDir = @"C:\VMs\Server1";
        var filePath = Path.Combine(machineDir, "vmware.log");
        var machine = new Machine(toggleToolsSyncTime, toggleToolsUpgradePolicy, toggleMksEnable3D, updateMachineVersion, updateMachineMemSize)
                      { Path = Path.Combine(machineDir, "Server1.vmx"), DisplayName = "Server1" };
        var loadHelper = new LoadHelper { VmDataGridItemsSource = [machine] };
        readLogInformation.ValueFor(machineDir).Returns(("2026-06-11", "0d ago"));

        // Act
        sut.UpdateFor(filePath, loadHelper);
        await Dispatcher.UIThread.InvokeAsync(() => { });

        // Assert
        machine.LogLastDate.Should().Be("2026-06-11");
        machine.LogLastDateDiff.Should().Be("0d ago");
    }

    #endregion
}