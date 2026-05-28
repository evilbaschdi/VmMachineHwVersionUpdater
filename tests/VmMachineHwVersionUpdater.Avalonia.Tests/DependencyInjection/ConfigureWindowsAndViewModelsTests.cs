using VmMachineHwVersionUpdater.Avalonia.DependencyInjection;
using VmMachineHwVersionUpdater.Avalonia.ViewModels;
using VmMachineHwVersionUpdater.Avalonia.Views;

namespace VmMachineHwVersionUpdater.Avalonia.Tests.DependencyInjection;

public class ConfigureWindowsAndViewModelsTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ConfigureWindowsAndViewModels).GetConstructors());
    }

    [Fact]
    public void Constructor_ReturnsInterfaceName()
    {
        typeof(ConfigureWindowsAndViewModels).Should().NotBeNull();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ConfigureWindowsAndViewModels).GetMethods().Where(method => !method.IsAbstract));
    }

    [Fact]
    public void AddWindowsAndViewModels_WithNullServices_ThrowsArgumentNullException()
    {
        // Act & Assert
        ((Action)(() => ConfigureWindowsAndViewModels.AddWindowsAndViewModels(null!)))
            .Should().Throw<ArgumentNullException>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void AddWindowsAndViewModels_RegistersExpectedServices(ServiceCollection serviceCollection)
    {
        // Act
        serviceCollection.AddWindowsAndViewModels();

        // Assert
        serviceCollection.Should().HaveService<AddEditAnnotationDialogViewModel>()
                         .AsSingleton();
        serviceCollection.Should().HaveService<AddEditAnnotationDialog>()
                         .AsTransient();
        serviceCollection.Should().HaveService<MainWindowViewModel>()
                         .AsSingleton();
    }
}