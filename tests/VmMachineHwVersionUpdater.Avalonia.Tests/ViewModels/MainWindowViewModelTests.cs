using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Avalonia.Collections;
using VmMachineHwVersionUpdater.Avalonia.ViewModels;

namespace VmMachineHwVersionUpdater.Avalonia.Tests.ViewModels;

public class MainWindowViewModelTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(MainWindowViewModel).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(MainWindowViewModel sut)
    {
        sut.Should().BeAssignableTo<IMainWindowViewModel>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(MainWindowViewModel).GetMethods().Where(method => !method.IsAbstract
                                                                                  & !method.Name.StartsWith("set_")
                                                                                  & !method.Name.StartsWith("add_")
                                                                                  & !method.Name.StartsWith("remove_")));
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public async Task RefreshMachineData_PreservesSortDescriptionsWhenCreatingTheFirstCollectionView(
        [Frozen] ILoad load,
        [Frozen] IConfigureDataGridCollectionView configureDataGridCollectionView,
        [Frozen] IComparer comparer,
        MainWindowViewModel sut)
    {
        var refreshedLoad = new LoadHelper
                           {
                               VmDataGridItemsSource = []
                           };
        load.Value.Returns(new LoadHelper());
        load.Refresh().Returns(refreshedLoad);

        var currentView = new DataGridCollectionView(new List<Machine>());
        var userSort = new DataGridComparerSortDescription(comparer, ListSortDirection.Descending);
        currentView.SortDescriptions.Add(userSort);

        var refreshedView = new DataGridCollectionView(new List<Machine>());
        configureDataGridCollectionView.Value.Returns(_ => currentView, _ => refreshedView);

        await RefreshMachineData(sut);

        refreshedView.SortDescriptions.Should().ContainSingle();
        refreshedView.SortDescriptions[0].Should().BeSameAs(userSort);
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public async Task RefreshMachineData_UpdatesExistingMachineAndPreservesViewAndSelection(
        [Frozen] ILoad load,
        [Frozen] ICurrentMachine currentMachine,
        [Frozen] IConfigureDataGridCollectionView configureDataGridCollectionView,
        [Frozen] IFilterDataGridCollectionView filterDataGridCollectionView,
        Machine selectedMachine,
        Machine refreshedMachine,
        MainWindowViewModel sut)
    {
        const string selectedPath = "C:\\VMs\\selected.vmx";
        selectedMachine.Path = selectedPath;
        selectedMachine.DisplayName = "Before refresh";
        refreshedMachine.Path = OperatingSystem.IsWindows()
                                    ? "c:\\vms\\SELECTED.vmx"
                                    : selectedPath;
        refreshedMachine.DisplayName = "After refresh";
        refreshedMachine.HwVersion = selectedMachine.HwVersion + 1;

        currentMachine.Value.Returns(selectedMachine);
        var currentItems = new ObservableCollection<Machine> { selectedMachine };
        var refreshedItems = new ObservableCollection<Machine> { refreshedMachine };
        load.Value.Returns(new LoadHelper
                           {
                               VmDataGridItemsSource = currentItems
                           });
        var refreshedLoad = new LoadHelper
                           {
                               VmDataGridItemsSource = refreshedItems
                           };
        load.Refresh().Returns(refreshedLoad);

        var currentView = new DataGridCollectionView(currentItems);
        configureDataGridCollectionView.Value.Returns(currentView);
        sut.SearchOsText = "Windows";
        sut.SearchFilterText = "developer";
        filterDataGridCollectionView.ClearReceivedCalls();

        await RefreshMachineData(sut);

        currentItems.Should().ContainSingle().Which.Should().BeSameAs(selectedMachine);
        selectedMachine.DisplayName.Should().Be("After refresh");
        selectedMachine.HwVersion.Should().Be(refreshedMachine.HwVersion);
        refreshedLoad.VmDataGridItemsSource.Should().BeSameAs(currentItems);
        configureDataGridCollectionView.Value.Should().BeSameAs(currentView);
        sut.SelectedMachine.Should().BeSameAs(selectedMachine);
        currentView.CurrentItem.Should().BeSameAs(selectedMachine);
        filterDataGridCollectionView.Received().RunFor(("Windows", "developer"));
        load.Received(1).Value = refreshedLoad;
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public async Task RefreshMachineData_RestoresFilterTextIfTheOsItemsBindingClearsSelection(
        [Frozen] ILoad load,
        [Frozen] IFilterDataGridCollectionView filterDataGridCollectionView,
        [Frozen] IConfigureDataGridCollectionView configureDataGridCollectionView,
        MainWindowViewModel sut)
    {
        const string searchOsText = "Windows 11";
        const string searchFilterText = "development";
        sut.SearchOsText = searchOsText;
        sut.SearchFilterText = searchFilterText;
        filterDataGridCollectionView.ClearReceivedCalls();

        var currentItems = new ObservableCollection<Machine>();
        var currentLoad = new LoadHelper
                          {
                              VmDataGridItemsSource = currentItems
                          };
        var refreshedLoad = new LoadHelper
                            {
                                VmDataGridItemsSource = []
                            };
        load.Value.Returns(currentLoad);
        load.Refresh().Returns(refreshedLoad);
        configureDataGridCollectionView.Value.Returns(new DataGridCollectionView(currentItems));
        sut.PropertyChanged += (_, args) =>
        {
            if (args.PropertyName != nameof(MainWindowViewModel.SearchOsItemCollection))
            {
                return;
            }

            sut.SearchOsText = string.Empty;
            sut.SearchFilterText = string.Empty;
        };

        await RefreshMachineData(sut);

        sut.SearchOsText.Should().Be(searchOsText);
        sut.SearchFilterText.Should().Be(searchFilterText);
        filterDataGridCollectionView.Received().RunFor((searchOsText, searchFilterText));
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public async Task RefreshMachineData_KeepsExistingMachineWhenItsFileStillExistsButCouldNotBeLoaded(
        [Frozen] ILoad load,
        [Frozen] IConfigureDataGridCollectionView configureDataGridCollectionView,
        Machine machine,
        Machine deletedMachine,
        MainWindowViewModel sut)
    {
        var machinePoolPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(machinePoolPath);

        try
        {
            machine.Directory = machinePoolPath;
            machine.Path = Path.Combine(machinePoolPath, "machine.vmx");
            machine.GuestOs = "Windows 11";
            machine.HwVersion = 17;
            File.WriteAllText(machine.Path, "virtualhw.version = \"17\"");
            deletedMachine.Directory = machinePoolPath;
            deletedMachine.Path = Path.Combine(machinePoolPath, "deleted.vmx");

            var currentItems = new ObservableCollection<Machine> { machine, deletedMachine };
            var refreshedLoad = new LoadHelper
                               {
                                   VmDataGridItemsSource = []
                               };
            load.Value.Returns(new LoadHelper
                               {
                                   VmDataGridItemsSource = currentItems
                               });
            load.Refresh().Returns(refreshedLoad);
            configureDataGridCollectionView.Value.Returns(new DataGridCollectionView(currentItems));

            await RefreshMachineData(sut);

            currentItems.Should().ContainSingle().Which.Should().BeSameAs(machine);
            refreshedLoad.VmDataGridItemsSource.Should().BeSameAs(currentItems);
            refreshedLoad.SearchOsItems.Should().ContainKey("Windows 11");
            refreshedLoad.UpdateAllTextBlocks.Should().Be("Update all 1 machines to version");
            refreshedLoad.UpdateAllHwVersion.Should().Be(17);
        }
        finally
        {
            Directory.Delete(machinePoolPath, true);
        }
    }

    private static Task RefreshMachineData(MainWindowViewModel sut) => sut.RefreshMachineDataAsync();
}