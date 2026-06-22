using Avalonia.Collections;
using Avalonia.Headless.XUnit;
using Avalonia.Threading;
using VmMachineHwVersionUpdater.Avalonia.ViewModels;

namespace VmMachineHwVersionUpdater.Avalonia.Tests.ViewModels;

public class UpdateMachineCollectionTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(UpdateMachineCollection).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(UpdateMachineCollection sut)
    {
        sut.Should().BeAssignableTo<IUpdateMachineCollection>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(UpdateMachineCollection).GetMethods().Where(method => !method.IsAbstract));
    }

    #region ReplaceByPath Tests

    [AvaloniaTheory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public async Task ReplaceByPath_WithExistingMachine_RemovesOldAndAddsNew(
        UpdateMachineCollection sut,
        IToggleToolsSyncTime toggleToolsSyncTime,
        IToggleToolsUpgradePolicy toggleToolsUpgradePolicy,
        IToggleMksEnable3D toggleMksEnable3D,
        IUpdateMachineVersion updateMachineVersion,
        IUpdateMachineMemSize updateMachineMemSize)
    {
        // Arrange
        var filePath = @"C:\VMs\Server1\Server1.vmx";
        var existingMachine = new Machine(toggleToolsSyncTime, toggleToolsUpgradePolicy, toggleMksEnable3D, updateMachineVersion, updateMachineMemSize)
                              { Path = filePath, DisplayName = "Old" };
        var newMachine = new Machine(toggleToolsSyncTime, toggleToolsUpgradePolicy, toggleMksEnable3D, updateMachineVersion, updateMachineMemSize)
                         { Path = filePath, DisplayName = "New" };
        var loadHelper = new LoadHelper { VmDataGridItemsSource = [existingMachine] };

        // Act
        sut.ReplaceByPath(loadHelper, filePath, newMachine);
        await Dispatcher.UIThread.InvokeAsync(() => { });

        // Assert
        loadHelper.VmDataGridItemsSource.Should().HaveCount(1);
        loadHelper.VmDataGridItemsSource[0].DisplayName.Should().Be("New");
    }

    [AvaloniaTheory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public async Task ReplaceByPath_WithMultipleDuplicates_RemovesAllAndAddsOne(
        UpdateMachineCollection sut,
        IToggleToolsSyncTime toggleToolsSyncTime,
        IToggleToolsUpgradePolicy toggleToolsUpgradePolicy,
        IToggleMksEnable3D toggleMksEnable3D,
        IUpdateMachineVersion updateMachineVersion,
        IUpdateMachineMemSize updateMachineMemSize)
    {
        // Arrange
        var filePath = @"C:\VMs\Server1\Server1.vmx";
        var dup1 = new Machine(toggleToolsSyncTime, toggleToolsUpgradePolicy, toggleMksEnable3D, updateMachineVersion, updateMachineMemSize)
                   { Path = filePath, DisplayName = "Dup1" };
        var dup2 = new Machine(toggleToolsSyncTime, toggleToolsUpgradePolicy, toggleMksEnable3D, updateMachineVersion, updateMachineMemSize)
                   { Path = filePath, DisplayName = "Dup2" };
        var newMachine = new Machine(toggleToolsSyncTime, toggleToolsUpgradePolicy, toggleMksEnable3D, updateMachineVersion, updateMachineMemSize)
                         { Path = filePath, DisplayName = "New" };
        var loadHelper = new LoadHelper { VmDataGridItemsSource = [dup1, dup2] };

        // Act
        sut.ReplaceByPath(loadHelper, filePath, newMachine);
        await Dispatcher.UIThread.InvokeAsync(() => { });

        // Assert
        loadHelper.VmDataGridItemsSource.Should().HaveCount(1);
        loadHelper.VmDataGridItemsSource[0].DisplayName.Should().Be("New");
    }

    [AvaloniaTheory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public async Task ReplaceByPath_WithNoExistingMachine_AddsNewMachine(
        UpdateMachineCollection sut,
        IToggleToolsSyncTime toggleToolsSyncTime,
        IToggleToolsUpgradePolicy toggleToolsUpgradePolicy,
        IToggleMksEnable3D toggleMksEnable3D,
        IUpdateMachineVersion updateMachineVersion,
        IUpdateMachineMemSize updateMachineMemSize)
    {
        // Arrange
        var filePath = @"C:\VMs\Server1\Server1.vmx";
        var newMachine = new Machine(toggleToolsSyncTime, toggleToolsUpgradePolicy, toggleMksEnable3D, updateMachineVersion, updateMachineMemSize)
                         { Path = filePath, DisplayName = "New" };
        var loadHelper = new LoadHelper { VmDataGridItemsSource = [] };

        // Act
        sut.ReplaceByPath(loadHelper, filePath, newMachine);
        await Dispatcher.UIThread.InvokeAsync(() => { });

        // Assert
        loadHelper.VmDataGridItemsSource.Should().HaveCount(1);
        loadHelper.VmDataGridItemsSource[0].Should().Be(newMachine);
    }

    #endregion

    #region RemoveByPath Tests

    [AvaloniaTheory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public async Task RemoveByPath_WithExistingMachine_RemovesIt(
        UpdateMachineCollection sut,
        IToggleToolsSyncTime toggleToolsSyncTime,
        IToggleToolsUpgradePolicy toggleToolsUpgradePolicy,
        IToggleMksEnable3D toggleMksEnable3D,
        IUpdateMachineVersion updateMachineVersion,
        IUpdateMachineMemSize updateMachineMemSize)
    {
        // Arrange
        var filePath = @"C:\VMs\Server1\Server1.vmx";
        var machine = new Machine(toggleToolsSyncTime, toggleToolsUpgradePolicy, toggleMksEnable3D, updateMachineVersion, updateMachineMemSize)
                      { Path = filePath, DisplayName = "Server1" };
        var loadHelper = new LoadHelper { VmDataGridItemsSource = [machine] };

        // Act
        sut.RemoveByPath(loadHelper, filePath);
        await Dispatcher.UIThread.InvokeAsync(() => { });

        // Assert
        loadHelper.VmDataGridItemsSource.Should().BeEmpty();
    }

    [AvaloniaTheory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public async Task RemoveByPath_WithNoMatchingMachine_DoesNothing(
        UpdateMachineCollection sut,
        IToggleToolsSyncTime toggleToolsSyncTime,
        IToggleToolsUpgradePolicy toggleToolsUpgradePolicy,
        IToggleMksEnable3D toggleMksEnable3D,
        IUpdateMachineVersion updateMachineVersion,
        IUpdateMachineMemSize updateMachineMemSize)
    {
        // Arrange
        var existingMachine = new Machine(toggleToolsSyncTime, toggleToolsUpgradePolicy, toggleMksEnable3D, updateMachineVersion, updateMachineMemSize)
                              { Path = @"C:\VMs\Other\Other.vmx", DisplayName = "Other" };
        var loadHelper = new LoadHelper { VmDataGridItemsSource = [existingMachine] };

        // Act
        sut.RemoveByPath(loadHelper, @"C:\VMs\Server1\Server1.vmx");
        await Dispatcher.UIThread.InvokeAsync(() => { });

        // Assert
        loadHelper.VmDataGridItemsSource.Should().HaveCount(1);
    }

    [AvaloniaTheory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public async Task RemoveByPath_WithCaseInsensitivePath_RemovesMachine(
        UpdateMachineCollection sut,
        IToggleToolsSyncTime toggleToolsSyncTime,
        IToggleToolsUpgradePolicy toggleToolsUpgradePolicy,
        IToggleMksEnable3D toggleMksEnable3D,
        IUpdateMachineVersion updateMachineVersion,
        IUpdateMachineMemSize updateMachineMemSize)
    {
        // Arrange
        var machine = new Machine(toggleToolsSyncTime, toggleToolsUpgradePolicy, toggleMksEnable3D, updateMachineVersion, updateMachineMemSize)
                      { Path = @"C:\VMs\Server1\Server1.vmx", DisplayName = "Server1" };
        var loadHelper = new LoadHelper { VmDataGridItemsSource = [machine] };

        // Act
        sut.RemoveByPath(loadHelper, @"c:\vms\server1\server1.vmx");
        await Dispatcher.UIThread.InvokeAsync(() => { });

        // Assert
        loadHelper.VmDataGridItemsSource.Should().BeEmpty();
    }

    #endregion

    #region Active Filter Tests

    [AvaloniaTheory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public async Task ReplaceByPath_WithActiveFilter_KeepsRenamedMachineVisible(
        [Frozen] IConfigureDataGridCollectionView configureDataGridCollectionView,
        UpdateMachineCollection sut,
        IToggleToolsSyncTime toggleToolsSyncTime,
        IToggleToolsUpgradePolicy toggleToolsUpgradePolicy,
        IToggleMksEnable3D toggleMksEnable3D,
        IUpdateMachineVersion updateMachineVersion,
        IUpdateMachineMemSize updateMachineMemSize)
    {
        // Arrange
        var serverPath = @"C:\VMs\Server1\Server1.vmx";

        // A machine that is filtered OUT so the view's filtered indices diverge from the source indices.
        var hiddenMachine = new Machine(toggleToolsSyncTime, toggleToolsUpgradePolicy, toggleMksEnable3D, updateMachineVersion, updateMachineMemSize)
                            { Path = @"C:\VMs\Linux\Linux.vmx", DisplayName = "Linux" };
        var serverMachine = new Machine(toggleToolsSyncTime, toggleToolsUpgradePolicy, toggleMksEnable3D, updateMachineVersion, updateMachineMemSize)
                            { Path = serverPath, DisplayName = "Server1-old" };

        var loadHelper = new LoadHelper { VmDataGridItemsSource = [hiddenMachine, serverMachine] };

        var view = new DataGridCollectionView(loadHelper.VmDataGridItemsSource)
                   {
                       Filter = item => ((Machine)item).DisplayName.Contains("Server", StringComparison.OrdinalIgnoreCase)
                   };
        configureDataGridCollectionView.Value.Returns(view);

        var renamedMachine = new Machine(toggleToolsSyncTime, toggleToolsUpgradePolicy, toggleMksEnable3D, updateMachineVersion, updateMachineMemSize)
                             { Path = serverPath, DisplayName = "Server1-new" };

        // Act
        sut.ReplaceByPath(loadHelper, serverPath, renamedMachine);
        await Dispatcher.UIThread.InvokeAsync(() => { });

        // Assert
        loadHelper.VmDataGridItemsSource.Should().Contain(renamedMachine);
        view.Cast<Machine>().Select(machine => machine.DisplayName).Should().ContainSingle().Which.Should().Be("Server1-new");
        view.Filter.Should().NotBeNull();
    }

    #endregion
}