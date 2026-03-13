namespace VmMachineHwVersionUpdater.Core.Tests.PerMachine;

public class ToggleMksEnable3DTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ToggleMksEnable3D).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(ToggleMksEnable3D sut)
    {
        sut.Should().BeAssignableTo<IToggleMksEnable3D>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ToggleMksEnable3D).GetMethods().Where(method => !method.IsAbstract));
    }
}