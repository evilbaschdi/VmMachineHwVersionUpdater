namespace VmMachineHwVersionUpdater.Core.Tests.PerMachine;

public class ToggleMksEnable3dTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ToggleMksEnable3d).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(ToggleMksEnable3d sut)
    {
        sut.Should().BeAssignableTo<IToggleMksEnable3d>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ToggleMksEnable3d).GetMethods().Where(method => !method.IsAbstract));
    }
}