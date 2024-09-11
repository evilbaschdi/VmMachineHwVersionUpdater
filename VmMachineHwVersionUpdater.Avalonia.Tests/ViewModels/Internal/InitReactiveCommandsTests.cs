using VmMachineHwVersionUpdater.Avalonia.ViewModels.Internal;

namespace VmMachineHwVersionUpdater.Avalonia.Tests.ViewModels.Internal;

public class InitReactiveCommandsTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(InitReactiveCommands).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(InitReactiveCommands sut)
    {
        sut.Should().BeAssignableTo<IInitReactiveCommands>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(InitReactiveCommands).GetMethods().Where(method => !method.IsAbstract
                                                                                   & !method.Name.StartsWith("set_")
                                                                                   & !method.Name.StartsWith("add_")
                                                                                   & !method.Name.StartsWith("remove_")));
    }
}