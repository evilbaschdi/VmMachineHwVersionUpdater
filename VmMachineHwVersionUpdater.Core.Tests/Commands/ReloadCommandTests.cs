using VmMachineHwVersionUpdater.Core.Commands;

namespace VmMachineHwVersionUpdater.Core.Tests.Commands;

public class ReloadCommandTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ReloadCommand).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(ReloadCommand sut)
    {
        sut.Should().BeAssignableTo<IReloadCommand>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ReloadCommand).GetMethods().Where(method => !method.IsAbstract));
    }
}