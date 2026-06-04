using System.ComponentModel;

namespace VmMachineHwVersionUpdater.Core.Tests.Models;

public class MachineTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(Machine).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(Machine sut)
    {
        sut.Should().BeAssignableTo<INotifyPropertyChanged>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(Machine).GetMethods()
                                        .Where(method => !method.IsAbstract &
                                                         !method.Name.StartsWith("set_") &
                                                         !method.Name.StartsWith("add_") &
                                                         !method.Name.StartsWith("remove_")));
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void SimpleProperties_CanBeSetAndRetrieved(
        IToggleToolsSyncTime toggleToolsSyncTime,
        IToggleToolsUpgradePolicy toggleToolsUpgradePolicy,
        IToggleMksEnable3D toggleMksEnable3D,
        IUpdateMachineVersion updateMachineVersion,
        IUpdateMachineMemSize updateMachineMemSize)
    {
        // Act
        var machine = new Machine(toggleToolsSyncTime, toggleToolsUpgradePolicy, toggleMksEnable3D, updateMachineVersion, updateMachineMemSize)
                      {
                          Path = @"C:\VMs\test.vmx",
                          DisplayName = "My Test VM",
                          GuestOs = "windows9-64",
                          Directory = @"C:\VMs",
                          MachineState = MachineState.Off,
                          MachineType = MachineType.Vmx,
                          Annotation = "Test annotation"
                      };

        // Assert
        machine.Path.Should().Be(@"C:\VMs\test.vmx");
        machine.DisplayName.Should().Be("My Test VM");
        machine.GuestOs.Should().Be("windows9-64");
        machine.Directory.Should().Be(@"C:\VMs");
        machine.MachineState.Should().Be(MachineState.Off);
        machine.MachineType.Should().Be(MachineType.Vmx);
        machine.Annotation.Should().Be("Test annotation");
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Properties_WhenChanged_TriggerPropertyChanged(
        IToggleToolsSyncTime toggleToolsSyncTime,
        IToggleToolsUpgradePolicy toggleToolsUpgradePolicy,
        IToggleMksEnable3D toggleMksEnable3D,
        IUpdateMachineVersion updateMachineVersion,
        IUpdateMachineMemSize updateMachineMemSize)
    {
        // Arrange
        var machine = new Machine(toggleToolsSyncTime, toggleToolsUpgradePolicy, toggleMksEnable3D, updateMachineVersion, updateMachineMemSize);
        using var monitoredMachine = machine.Monitor();

        // Act
        machine.MachineState = MachineState.Paused;
        machine.LogLastDate = "2024-01-01";
        machine.LogLastDateDiff = "1 day ago";
        machine.IsEnabledForEditing = true;
        machine.ExtendedInformation = "icon";
        machine.ExtendedInformationToolTip = "tooltip";

        // Assert
        monitoredMachine.Should().RaisePropertyChangeFor(m => m.MachineState);
        monitoredMachine.Should().RaisePropertyChangeFor(m => m.LogLastDate);
        monitoredMachine.Should().RaisePropertyChangeFor(m => m.LogLastDateDiff);
        monitoredMachine.Should().RaisePropertyChangeFor(m => m.IsEnabledForEditing);
        monitoredMachine.Should().RaisePropertyChangeFor(m => m.ExtendedInformation);
        monitoredMachine.Should().RaisePropertyChangeFor(m => m.ExtendedInformationToolTip);
    }
}