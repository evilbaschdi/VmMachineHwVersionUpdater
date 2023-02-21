using VmMachineHwVersionUpdater.ViewModels.Internal;

namespace VmMachineHwVersionUpdater.Tests.ViewModels.Internal;

public class StartDefaultCommandTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(StartDefaultCommand).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(StartDefaultCommand sut)
    {
        sut.Should().BeAssignableTo<IStartDefaultCommand>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(StartDefaultCommand).GetMethods().Where(method => !method.IsAbstract));
    }
}