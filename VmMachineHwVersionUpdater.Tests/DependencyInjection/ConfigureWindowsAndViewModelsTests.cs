using System.Linq;
using AutoFixture.Idioms;
using EvilBaschdi.CoreExtended.Controls.About;
using EvilBaschdi.Testing;
using FluentAssertions;
using FluentAssertions.Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using VmMachineHwVersionUpdater.DependencyInjection;
using VmMachineHwVersionUpdater.ViewModels;
using Xunit;

namespace VmMachineHwVersionUpdater.Tests.DependencyInjection
{
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
}