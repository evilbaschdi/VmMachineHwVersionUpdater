using EvilBaschdi.CoreExtended.Mvvm.ViewModel;
using VmMachineHwVersionUpdater.ViewModels;

namespace VmMachineHwVersionUpdater.Tests.ViewModels;

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
        sut.Should().BeAssignableTo<ApplicationStyleViewModel>();
        sut.Should().BeAssignableTo<IMainWindowViewModel>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(MainWindowViewModel).GetMethods()
                                                    .Where(method => !method.IsAbstract &
                                                                     !method.Name.StartsWith("set_") &
                                                                     !method.Name.StartsWith("add_") &
                                                                     !method.Name.StartsWith("remove_")));
    }
}