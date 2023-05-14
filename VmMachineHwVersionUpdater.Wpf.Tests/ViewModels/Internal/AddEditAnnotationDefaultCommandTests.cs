using VmMachineHwVersionUpdater.Wpf.ViewModels.Internal;

namespace VmMachineHwVersionUpdater.Wpf.Tests.ViewModels.Internal;

public class AddEditAnnotationDefaultCommandTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(AddEditAnnotationDefaultCommand).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(AddEditAnnotationDefaultCommand sut)
    {
        sut.Should().BeAssignableTo<IAddEditAnnotationDefaultCommand>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(AddEditAnnotationDefaultCommand).GetMethods().Where(method => !method.IsAbstract));
    }
}