using Avalonia.Controls;
using VmMachineHwVersionUpdater.Avalonia.Views;

namespace VmMachineHwVersionUpdater.Avalonia.Tests.Views;

public class AddEditAnnotationDialogTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(AddEditAnnotationDialog).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(AddEditAnnotationDialog sut)
    {
        sut.Should().BeAssignableTo<Window>();
    }

    //[Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    //public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    //{
    //    assertion.Verify(typeof(AddEditAnnotationDialog).GetMethods().Where(method => !method.IsAbstract
    //                                                                        ));
    //}
}