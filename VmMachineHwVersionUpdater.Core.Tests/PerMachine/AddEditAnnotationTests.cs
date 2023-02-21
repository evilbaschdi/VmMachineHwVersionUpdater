using VmMachineHwVersionUpdater.Core.PerMachine;

namespace VmMachineHwVersionUpdater.Core.Tests.PerMachine;

public class AddEditAnnotationTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(AddEditAnnotation).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(AddEditAnnotation sut)
    {
        sut.Should().BeAssignableTo<IAddEditAnnotation>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(AddEditAnnotation).GetMethods().Where(method => !method.IsAbstract));
    }
}