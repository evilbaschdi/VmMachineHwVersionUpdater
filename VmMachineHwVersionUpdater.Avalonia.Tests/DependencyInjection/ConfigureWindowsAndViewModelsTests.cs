using VmMachineHwVersionUpdater.Avalonia.DependencyInjection;

namespace VmMachineHwVersionUpdater.Avalonia.Tests.DependencyInjection;

public class ConfigureWindowsAndViewModelsTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ConfigureWindowsAndViewModels).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(ConfigureWindowsAndViewModels sut)
    {
        sut.Should().BeAssignableTo<IConfigureWindowsAndViewModels>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ConfigureWindowsAndViewModels).GetMethods().Where(method => !method.IsAbstract));
    }
}