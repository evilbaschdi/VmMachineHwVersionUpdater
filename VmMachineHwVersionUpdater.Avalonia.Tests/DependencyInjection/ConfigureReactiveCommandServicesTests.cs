using VmMachineHwVersionUpdater.Avalonia.DependencyInjection;

namespace VmMachineHwVersionUpdater.Avalonia.Tests.DependencyInjection;

public class ConfigureReactiveCommandServicesTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ConfigureReactiveCommandServices).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ConfigureReactiveCommandServices).GetMethods().Where(method => !method.IsAbstract));
    }
}