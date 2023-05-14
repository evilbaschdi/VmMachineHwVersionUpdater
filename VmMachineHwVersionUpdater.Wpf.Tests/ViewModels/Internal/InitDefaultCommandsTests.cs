using VmMachineHwVersionUpdater.Wpf.ViewModels.Internal;

namespace VmMachineHwVersionUpdater.Wpf.Tests.ViewModels.Internal;

public class InitDefaultCommandsTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(InitDefaultCommands).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(InitDefaultCommands sut)
    {
        sut.Should().BeAssignableTo<IInitDefaultCommands>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(InitDefaultCommands).GetMethods().Where(method => !method.IsAbstract & !method.Name.StartsWith("set_")));
    }
}