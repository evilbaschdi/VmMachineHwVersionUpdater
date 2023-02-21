using VmMachineHwVersionUpdater.Core.PerMachine;

namespace VmMachineHwVersionUpdater.Core.Tests.PerMachine;

public class VmxLineStartsWithTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(VmxLineStartsWith).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(VmxLineStartsWith sut)
    {
        sut.Should().BeAssignableTo<IVmxLineStartsWith>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(VmxLineStartsWith).GetMethods().Where(method => !method.IsAbstract));
    }
}