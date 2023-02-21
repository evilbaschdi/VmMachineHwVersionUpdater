using VmMachineHwVersionUpdater.Core.BasicApplication;

namespace VmMachineHwVersionUpdater.Core.Tests.BasicApplication;

public class LoadTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(Load).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(Load sut)
    {
        sut.Should().BeAssignableTo<ILoad>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(Load).GetMethods().Where(method => !method.IsAbstract));
    }
}