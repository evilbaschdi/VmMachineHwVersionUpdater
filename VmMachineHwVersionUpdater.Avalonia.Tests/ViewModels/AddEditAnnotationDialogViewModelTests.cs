using VmMachineHwVersionUpdater.Avalonia.ViewModels;

namespace VmMachineHwVersionUpdater.Avalonia.Tests.ViewModels;

public class AddEditAnnotationDialogViewModelTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(AddEditAnnotationDialogViewModel).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(AddEditAnnotationDialogViewModel sut)
    {
        sut.Should().BeAssignableTo<IAddEditAnnotationDialogViewModel>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(AddEditAnnotationDialogViewModel).GetMethods()
                                                                 .Where(method => !method.IsAbstract & !method.Name.StartsWith("set_") &
                                                                                  !method.Name.StartsWith("add_") &
                                                                                  !method.Name.StartsWith("remove_")));
    }
}