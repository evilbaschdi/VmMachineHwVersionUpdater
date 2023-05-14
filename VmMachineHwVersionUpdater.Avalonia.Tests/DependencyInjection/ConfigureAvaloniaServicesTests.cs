using VmMachineHwVersionUpdater.Avalonia.DependencyInjection;

namespace VmMachineHwVersionUpdater.Avalonia.Tests.DependencyInjection;

public class ConfigureAvaloniaServicesTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ConfigureAvaloniaServices).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(ConfigureAvaloniaServices sut)
    {
        sut.Should().BeAssignableTo<IConfigureAvaloniaServices>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ConfigureAvaloniaServices).GetMethods().Where(method => !method.IsAbstract));
    }
}