using System.Reflection;
using Avalonia;
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

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public async Task CreateMainWindow_ReturnsMainWindowWithViewModelDataContext(TestableApp sut)
    {
        await RunOnHeadlessDispatcher(() =>
                                      {
                                          // Arrange
                                          InitializeServices();

                                          // Act
                                          var result = sut.InvokeCreateMainWindow();

                                          // Assert
                                          result.Should().BeOfType<MainWindow>();
                                          result.DataContext.Should().BeOfType<MainWindowViewModel>();
                                      });
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void PreMainWindowCreation_SetsAppNameFromCurrent(TestableApp sut)
    {
        // Act
        sut.InvokePreMainWindowCreation();

        // Assert
        ApplicationServices.AppName.Should().Be(Application.Current?.Name);
    }

    /// <summary>
    ///     Runs <paramref name="action" /> on Avalonia's headless dispatcher thread.
    ///     Replaces <c>[AvaloniaFact]</c>, which cannot be used while Avalonia.Headless.XUnit 12.1.2 is incompatible
    ///     with xunit.v3 4.0.0 (its discoverer throws during test discovery, before the test itself is ever reached).
    ///     See https://github.com/AvaloniaUI/Avalonia/issues/22072.
    /// </summary>
    /// <param name="action"></param>
    private static Task RunOnHeadlessDispatcher(Action action)
    {
        ArgumentNullException.ThrowIfNull(action);

        var session = HeadlessUnitTestSession.GetOrStartForAssembly(typeof(AppTests).Assembly);
        return session.Dispatch(action, TestContext.Current.CancellationToken);
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void ResizeWithBorder400_IsTrue(TestableApp sut)
    {
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

    public class TestableApp : App
    {
        public bool ExposedResizeWithBorder400 => ResizeWithBorder400;
        public Window InvokeCreateMainWindow() => CreateMainWindow();
        public void InvokePreMainWindowCreation() => PreMainWindowCreation();
    }
}