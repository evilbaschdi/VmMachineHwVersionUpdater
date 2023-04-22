namespace VmMachineHwVersionUpdater.Core.Tests.PerMachine;

public class ToggleToolsSyncTimeTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ToggleToolsSyncTime).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(ToggleToolsSyncTime sut)
    {
        sut.Should().BeAssignableTo<IToggleToolsSyncTime>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ToggleToolsSyncTime).GetMethods().Where(method => !method.IsAbstract));
    }
}