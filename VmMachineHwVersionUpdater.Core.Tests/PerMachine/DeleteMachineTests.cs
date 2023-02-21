using VmMachineHwVersionUpdater.Core.PerMachine;

namespace VmMachineHwVersionUpdater.Core.Tests.PerMachine;

public class DeleteMachineTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(DeleteMachine).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(DeleteMachine sut)
    {
        sut.Should().BeAssignableTo<IDeleteMachine>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(DeleteMachine).GetMethods().Where(method => !method.IsAbstract));
    }
}