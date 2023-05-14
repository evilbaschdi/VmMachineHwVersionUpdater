using VmMachineHwVersionUpdater.Wpf.ViewModels.Internal;

namespace VmMachineHwVersionUpdater.Wpf.Tests.ViewModels.Internal;

public class OpenWithCodeDefaultCommandTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(OpenWithCodeDefaultCommand).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(OpenWithCodeDefaultCommand sut)
    {
        sut.Should().BeAssignableTo<IOpenWithCodeDefaultCommand>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(OpenWithCodeDefaultCommand).GetMethods().Where(method => !method.IsAbstract));
    }
}