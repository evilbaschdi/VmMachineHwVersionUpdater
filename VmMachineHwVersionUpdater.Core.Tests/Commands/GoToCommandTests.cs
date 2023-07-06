using VmMachineHwVersionUpdater.Core.Commands;

namespace VmMachineHwVersionUpdater.Core.Tests.Commands;

public class GoToCommandTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(GoToCommand).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(GoToCommand sut)
    {
        sut.Should().BeAssignableTo<IGoToCommand>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(GoToCommand).GetMethods().Where(method => !method.IsAbstract));
    }
}