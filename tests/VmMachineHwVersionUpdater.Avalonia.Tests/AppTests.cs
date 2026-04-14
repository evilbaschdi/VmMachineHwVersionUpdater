using System.Reflection;
using Avalonia;

namespace VmMachineHwVersionUpdater.Avalonia.Tests;

public class AppTests : AvaloniaTestBase<App>
{
    [Fact]
    public void Constructor_HasNullGuards()
    {
        // Constructor has no parameters, so no null guards to test
        typeof(App).GetConstructors().Should().NotBeEmpty();
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
        // Check what methods this class actually declares
        var methods = typeof(App)
                      .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                      .Where(m => !m.IsAbstract && !m.IsSpecialName)
                      .ToArray();

        // App class has Initialize() and OnFrameworkInitializationCompleted() methods,
        // but both have no parameters, so there are no null guards to test
        var methodsWithParameters = methods.Where(m => m.GetParameters().Length > 0).ToArray();

        if (methodsWithParameters.Length > 0)
        {
            assertion.Verify(methodsWithParameters);
        }

        // Verify that we found the expected methods (Initialize and OnFrameworkInitializationCompleted)
        methods.Should().Contain(m => m.Name == "Initialize", "App should override Initialize method");
        methods.Should().Contain(m => m.Name == "OnFrameworkInitializationCompleted",
            "App should override OnFrameworkInitializationCompleted method");
    }
}