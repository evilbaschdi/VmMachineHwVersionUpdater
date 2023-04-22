namespace VmMachineHwVersionUpdater.Core.Tests.PerMachine;

public class ReturnValueFromVmxLineTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ReturnValueFromVmxLine).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(ReturnValueFromVmxLine sut)
    {
        sut.Should().BeAssignableTo<IReturnValueFromVmxLine>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ReturnValueFromVmxLine).GetMethods().Where(method => !method.IsAbstract));
    }
}