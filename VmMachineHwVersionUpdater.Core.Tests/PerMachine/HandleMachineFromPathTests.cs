namespace VmMachineHwVersionUpdater.Core.Tests.PerMachine;

public class HandleMachineFromPathTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(HandleMachineFromPath).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(HandleMachineFromPath sut)
    {
        sut.Should().BeAssignableTo<IHandleMachineFromPath>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(HandleMachineFromPath).GetMethods().Where(method => !method.IsAbstract));
    }
}