using VmMachineHwVersionUpdater.Wpf.DependencyInjection;
using VmMachineHwVersionUpdater.Wpf.ViewModels;
using VmMachineHwVersionUpdater.Wpf.Views;

namespace VmMachineHwVersionUpdater.Wpf.Tests.DependencyInjection;

public class ConfigureWindowsAndViewModelsTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ConfigureWindowsAndViewModels).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ConfigureWindowsAndViewModels).GetMethods().Where(method => !method.IsAbstract));
    }

    [Fact]
    public void RunFor_ForProvidedServiceCollection_ReturnsServiceCollectionWithInstances()
    {
        // Arrange
        IServiceCollection dummyServiceCollection = new ServiceCollection();

        // Act
        dummyServiceCollection.AddWindowsAndViewModels();

        // Assert
        dummyServiceCollection.Should().HaveCount(4);
        dummyServiceCollection.Should().HaveService<IAddEditAnnotationDialogViewModel>().WithImplementation<AddEditAnnotationDialogViewModel>().AsSingleton();
        dummyServiceCollection.Should().HaveService<AddEditAnnotationDialog>().AsTransient();

        dummyServiceCollection.Should().HaveService<MainWindowViewModel>().AsSingleton();
        dummyServiceCollection.Should().HaveService<MainWindow>().AsTransient();
    }
}