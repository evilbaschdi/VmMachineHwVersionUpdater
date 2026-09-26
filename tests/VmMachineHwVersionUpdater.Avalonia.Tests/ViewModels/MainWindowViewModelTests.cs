using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
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
    public void RefreshMachineData_PreservesSortDescriptions(
        [Frozen] IConfigureDataGridCollectionView configureDataGridCollectionView,
        [Frozen] IComparer comparer,
        MainWindowViewModel sut)
    {
        var currentView = new DataGridCollectionView(new List<Machine>());
        var userSort = new DataGridComparerSortDescription(comparer, ListSortDirection.Descending);
        currentView.SortDescriptions.Add(userSort);

        var refreshedView = new DataGridCollectionView(new List<Machine>());
        configureDataGridCollectionView.Value.Returns(_ => currentView, _ => refreshedView);

        typeof(MainWindowViewModel)
           .GetMethod("RefreshMachineData", BindingFlags.Instance | BindingFlags.NonPublic)!
           .Invoke(sut, null);

        refreshedView.SortDescriptions.Should().ContainSingle();
        refreshedView.SortDescriptions[0].Should().BeSameAs(userSort);
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void RefreshMachineData_PreservesSelectedMachineByPath(
        [Frozen] ICurrentMachine currentMachine,
        [Frozen] IConfigureDataGridCollectionView configureDataGridCollectionView,
        Machine selectedMachine,
        Machine firstMachine,
        Machine secondMachine,
        Machine refreshedSelectedMachine,
        MainWindowViewModel sut)
    {
        const string selectedPath = "C:\\VMs\\selected.vmx";
        selectedMachine.Path = selectedPath;
        refreshedSelectedMachine.Path = OperatingSystem.IsWindows()
                                            ? "c:\\vms\\SELECTED.vmx"
                                            : selectedPath;

        currentMachine.Value.Returns(selectedMachine);
        var currentView = new DataGridCollectionView(new List<Machine> { selectedMachine });
        var refreshedView = new DataGridCollectionView(new List<Machine>
                                                        {
                                                            firstMachine,
                                                            secondMachine,
                                                            refreshedSelectedMachine
                                                        });
        configureDataGridCollectionView.Value.Returns(_ => currentView, _ => refreshedView);

        typeof(MainWindowViewModel)
           .GetMethod("RefreshMachineData", BindingFlags.Instance | BindingFlags.NonPublic)!
           .Invoke(sut, null);

        currentMachine.Received(1).Value = refreshedSelectedMachine;
        refreshedView.CurrentItem.Should().BeSameAs(refreshedSelectedMachine);
    }
}