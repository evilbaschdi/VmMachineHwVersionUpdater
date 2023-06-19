using EvilBaschdi.About.Core.Models;
using EvilBaschdi.About.Wpf;
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
    public void Constructor_ReturnsInterfaceName(ConfigureWindowsAndViewModels sut)
    {
        sut.Should().BeAssignableTo<IConfigureWindowsAndViewModels>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ConfigureWindowsAndViewModels).GetMethods().Where(method => !method.IsAbstract));
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void RunFor_ForProvidedServiceCollection_ReturnsServiceCollectionWithInstances(
        ConfigureWindowsAndViewModels sut
    )
    {
        // Arrange
        IServiceCollection dummyServiceCollection = new ServiceCollection();

        // Act
        sut.RunFor(dummyServiceCollection);

        // Assert
        dummyServiceCollection.Should().HaveCount(6);
        dummyServiceCollection.Should().HaveService<AddEditAnnotationDialogViewModel>().AsSingleton();
        dummyServiceCollection.Should().HaveService<AddEditAnnotationDialog>().WithImplementation<AddEditAnnotationDialog>().AsTransient();

        dummyServiceCollection.Should().HaveService<IAboutModel>().WithImplementation<AboutViewModel>().AsSingleton();
        dummyServiceCollection.Should().HaveService<AboutWindow>().WithImplementation<AboutWindow>().AsTransient();

        dummyServiceCollection.Should().HaveService<MainWindowViewModel>().AsSingleton();
        dummyServiceCollection.Should().HaveService<MainWindow>().WithImplementation<MainWindow>().AsTransient();
    }
}