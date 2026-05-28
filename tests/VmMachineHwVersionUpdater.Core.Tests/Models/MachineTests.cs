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
    public void HwVersion_WhenInitializedWithNoSubscriber_DoesNotCallUpdateMachineVersion(
        IToggleToolsSyncTime toggleToolsSyncTime,
        IToggleToolsUpgradePolicy toggleToolsUpgradePolicy,
        IToggleMksEnable3D toggleMksEnable3D,
        [Frozen] IUpdateMachineVersion updateMachineVersion,
        IUpdateMachineMemSize updateMachineMemSize)
    {
        // Arrange & Act - PropertyChanged has no subscribers during init
        var machine = new Machine(toggleToolsSyncTime, toggleToolsUpgradePolicy, toggleMksEnable3D, updateMachineVersion, updateMachineMemSize)
                      {
                          Path = @"C:\VMs\test.vmx",
                          IsEnabledForEditing = true,
                          HwVersion = 21
                      };

        // Assert
        updateMachineVersion.DidNotReceive().RunFor(Arg.Any<string>(), Arg.Any<int>());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void SyncTimeWithHost_WhenInitializedWithNoSubscriber_DoesNotCallToggleToolsSyncTime(
        [Frozen] IToggleToolsSyncTime toggleToolsSyncTime,
        IToggleToolsUpgradePolicy toggleToolsUpgradePolicy,
        IToggleMksEnable3D toggleMksEnable3D,
        IUpdateMachineVersion updateMachineVersion,
        IUpdateMachineMemSize updateMachineMemSize)
    {
        // Arrange & Act
        var machine = new Machine(toggleToolsSyncTime, toggleToolsUpgradePolicy, toggleMksEnable3D, updateMachineVersion, updateMachineMemSize)
                      {
                          Path = @"C:\VMs\test.vmx",
                          IsEnabledForEditing = true,
                          SyncTimeWithHost = true
                      };

        // Assert
        toggleToolsSyncTime.DidNotReceive().RunFor(Arg.Any<string>(), Arg.Any<bool>());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void AutoUpdateTools_WhenInitializedWithNoSubscriber_DoesNotCallToggleToolsUpgradePolicy(
        IToggleToolsSyncTime toggleToolsSyncTime,
        [Frozen] IToggleToolsUpgradePolicy toggleToolsUpgradePolicy,
        IToggleMksEnable3D toggleMksEnable3D,
        IUpdateMachineVersion updateMachineVersion,
        IUpdateMachineMemSize updateMachineMemSize)
    {
        // Arrange & Act
        var machine = new Machine(toggleToolsSyncTime, toggleToolsUpgradePolicy, toggleMksEnable3D, updateMachineVersion, updateMachineMemSize)
                      {
                          Path = @"C:\VMs\test.vmx",
                          IsEnabledForEditing = true,
                          AutoUpdateTools = true
                      };

        // Assert
        toggleToolsUpgradePolicy.DidNotReceive().RunFor(Arg.Any<string>(), Arg.Any<bool>());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Accelerate3DGraphics_WhenInitializedWithNoSubscriber_DoesNotCallToggleMksEnable3D(
        IToggleToolsSyncTime toggleToolsSyncTime,
        IToggleToolsUpgradePolicy toggleToolsUpgradePolicy,
        [Frozen] IToggleMksEnable3D toggleMksEnable3D,
        IUpdateMachineVersion updateMachineVersion,
        IUpdateMachineMemSize updateMachineMemSize)
    {
        // Arrange & Act
        var machine = new Machine(toggleToolsSyncTime, toggleToolsUpgradePolicy, toggleMksEnable3D, updateMachineVersion, updateMachineMemSize)
                      {
                          Path = @"C:\VMs\test.vmx",
                          IsEnabledForEditing = true,
                          Accelerate3DGraphics = true
                      };

        // Assert
        toggleMksEnable3D.DidNotReceive().RunFor(Arg.Any<string>(), Arg.Any<bool>());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void HwVersion_WhenNotEnabledForEditing_DoesNotCallUpdateMachineVersion(
        [Frozen] IUpdateMachineVersion updateMachineVersion,
        IToggleToolsSyncTime toggleToolsSyncTime,
        IToggleToolsUpgradePolicy toggleToolsUpgradePolicy,
        IToggleMksEnable3D toggleMksEnable3D,
        IUpdateMachineMemSize updateMachineMemSize)
    {
        // Arrange & Act
        var machine = new Machine(toggleToolsSyncTime, toggleToolsUpgradePolicy, toggleMksEnable3D, updateMachineVersion, updateMachineMemSize)
                      {
                          Path = @"C:\VMs\test.vmx",
                          IsEnabledForEditing = false,
                          HwVersion = 21
                      };

        // Assert
        updateMachineVersion.DidNotReceive().RunFor(Arg.Any<string>(), Arg.Any<int>());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void MemSize_WhenInitializedWithNoSubscriber_DoesNotCallUpdateMachineMemSize(
        IToggleToolsSyncTime toggleToolsSyncTime,
        IToggleToolsUpgradePolicy toggleToolsUpgradePolicy,
        IToggleMksEnable3D toggleMksEnable3D,
        IUpdateMachineVersion updateMachineVersion,
        [Frozen] IUpdateMachineMemSize updateMachineMemSize)
    {
        // Arrange & Act
        var machine = new Machine(toggleToolsSyncTime, toggleToolsUpgradePolicy, toggleMksEnable3D, updateMachineVersion, updateMachineMemSize)
                      {
                          Path = @"C:\VMs\test.vmx",
                          IsEnabledForEditing = true,
                          MemSize = 4
                      };

        // Assert
        updateMachineMemSize.DidNotReceive().RunFor(Arg.Any<string>(), Arg.Any<int>());
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
}