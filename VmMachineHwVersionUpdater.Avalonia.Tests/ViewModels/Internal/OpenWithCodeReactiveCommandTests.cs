using VmMachineHwVersionUpdater.Avalonia.ViewModels.Internal;

namespace VmMachineHwVersionUpdater.Avalonia.Tests.ViewModels.Internal;

public class OpenWithCodeReactiveCommandTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(OpenWithCodeReactiveCommand).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(OpenWithCodeReactiveCommand sut)
    {
        sut.Should().BeAssignableTo<IOpenWithCodeReactiveCommand>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(OpenWithCodeReactiveCommand).GetMethods().Where(method => !method.IsAbstract));
    }
}