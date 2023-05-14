using VmMachineHwVersionUpdater.Avalonia.ViewModels.Internal;

namespace VmMachineHwVersionUpdater.Avalonia.Tests.ViewModels.Internal;

public class StartReactiveCommandTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(StartReactiveCommand).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(StartReactiveCommand sut)
    {
        sut.Should().BeAssignableTo<IStartReactiveCommand>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(StartReactiveCommand).GetMethods().Where(method => !method.IsAbstract));
    }
}