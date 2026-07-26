using System.Reflection;
using Avalonia;
using Avalonia.Headless.XUnit;
using EvilBaschdi.About.Avalonia.DependencyInjection;
using EvilBaschdi.Core.Avalonia.DependencyInjection;
using EvilBaschdi.Core.Avalonia.Lifetime;
using VmMachineHwVersionUpdater.Avalonia.DependencyInjection;
using VmMachineHwVersionUpdater.Avalonia.ViewModels;
using VmMachineHwVersionUpdater.Avalonia.Views;

namespace VmMachineHwVersionUpdater.Avalonia.Tests;

public class AppTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(App).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(App sut)
    {
        sut.Should().BeAssignableTo<Application>();
        sut.Should().BeAssignableTo<ApplicationWithSplash>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(App).GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                                    .Where(method => !method.IsAbstract));
    }

    [AvaloniaFact]
    public void CreateMainWindow_ReturnsMainWindowWithViewModelDataContext()
    {
        // Arrange
        InitializeServices();
        var sut = new TestableApp();

        // Act
        var result = sut.InvokeCreateMainWindow();

        // Assert
        result.Should().BeOfType<MainWindow>();
        result.DataContext.Should().BeOfType<MainWindowViewModel>();
    }

    [AvaloniaFact]
    public void PreMainWindowCreation_SetsAppNameFromCurrent()
    {
        // Arrange
        var sut = new TestableApp();

        // Act
        sut.InvokePreMainWindowCreation();

        // Assert
        ApplicationServices.AppName.Should().Be(Application.Current?.Name);
    }

    [Fact]
    public void ResizeWithBorder400_IsFalse()
    {
        var sut = new TestableApp();
        sut.ExposedResizeWithBorder400.Should().BeTrue();
    }

    private static void InitializeServices()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddCoreServices();
        serviceCollection.AddAboutServices();
        serviceCollection.AddCommandServices();
        serviceCollection.AddAvaloniaServices();
        serviceCollection.AddReactiveCommandServices();
        serviceCollection.AddWindowsAndViewModels();

        ApplicationServices.Initialize(serviceCollection.BuildServiceProvider());
    }

    private class TestableApp : App
    {
        public bool ExposedResizeWithBorder400 => ResizeWithBorder400;
        public Window InvokeCreateMainWindow() => CreateMainWindow();
        public void InvokePreMainWindowCreation() => PreMainWindowCreation();
    }
}