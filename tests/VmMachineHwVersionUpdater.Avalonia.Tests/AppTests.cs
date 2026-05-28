using System.Reflection;
using Avalonia;

namespace VmMachineHwVersionUpdater.Avalonia.Tests;

public class AppTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(App).GetConstructors());
    }

    [Fact]
    public void Constructor_ReturnsInterfaceName()
    {
        // Test that the type is assignable to Application without actually instantiating
        typeof(App).Should().BeAssignableTo<Application>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(App).GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                                    .Where(method => !method.IsAbstract));
    }
}