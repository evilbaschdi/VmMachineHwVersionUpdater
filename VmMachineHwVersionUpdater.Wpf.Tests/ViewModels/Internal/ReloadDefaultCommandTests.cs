using VmMachineHwVersionUpdater.Wpf.ViewModels.Internal;

namespace VmMachineHwVersionUpdater.Wpf.Tests.ViewModels.Internal;

public class ReloadDefaultCommandTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ReloadDefaultCommand).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(ReloadDefaultCommand sut)
    {
        sut.Should().BeAssignableTo<IReloadDefaultCommand>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ReloadDefaultCommand).GetMethods().Where(method => !method.IsAbstract & !method.Name.StartsWith("set_")));
    }
}