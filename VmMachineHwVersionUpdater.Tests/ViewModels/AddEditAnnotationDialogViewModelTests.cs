using EvilBaschdi.Core.Wpf.Mvvm.ViewModel;
using VmMachineHwVersionUpdater.ViewModels;

namespace VmMachineHwVersionUpdater.Tests.ViewModels;

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
        Assert.IsAssignableFrom<IAddEditAnnotationDialogViewModel>(sut);
        Assert.IsAssignableFrom<ApplicationStyleViewModel>(sut);
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(AddEditAnnotationDialogViewModel).GetMethods().Where(method => !method.IsAbstract & !method.Name.StartsWith("set_")));
    }
}