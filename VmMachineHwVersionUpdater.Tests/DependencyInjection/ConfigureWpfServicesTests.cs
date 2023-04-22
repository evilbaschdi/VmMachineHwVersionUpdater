using EvilBaschdi.Core.AppHelpers;
using EvilBaschdi.Core.Internal;
using FluentAssertions.Microsoft.Extensions.DependencyInjection;
using VmMachineHwVersionUpdater.Core.BasicApplication;
using VmMachineHwVersionUpdater.DependencyInjection;
using VmMachineHwVersionUpdater.ViewModels.Internal;

namespace VmMachineHwVersionUpdater.Tests.DependencyInjection;

public class ConfigureWpfServicesTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ConfigureWpfServices).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(ConfigureWpfServices sut)
    {
        sut.Should().BeAssignableTo<IConfigureWpfServices>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ConfigureWpfServices).GetMethods().Where(method => !method.IsAbstract));
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void RunFor_ForProvidedServiceCollection_ReturnsServiceCollectionWithInstances(
        ConfigureWpfServices sut
    )
    {
        // Arrange
        IServiceCollection dummyServiceCollection = new ServiceCollection();

        // Act
        sut.RunFor(dummyServiceCollection);

        // Assert
        dummyServiceCollection.Should().HaveCount(11);
        dummyServiceCollection.Should().HaveService<IConfigureListCollectionView>().WithImplementation<ConfigureListCollectionView>().AsSingleton();
        dummyServiceCollection.Should().HaveService<ICopyDirectoryWithFilesWithProgress>().WithImplementation<CopyDirectoryWithFilesWithProgress>().AsSingleton();
        dummyServiceCollection.Should().HaveService<ICopyDirectoryWithProgress>().WithImplementation<CopyDirectoryWithProgress>().AsSingleton();
        dummyServiceCollection.Should().HaveService<ICopyProgress>().WithImplementation<CopyProgress>().AsSingleton();
        dummyServiceCollection.Should().HaveService<ICurrentItemSource>().WithImplementation<CurrentItemSource>().AsSingleton();
        dummyServiceCollection.Should().HaveService<IFilterListCollectionView>().WithImplementation<FilterListCollectionView>().AsSingleton();
        dummyServiceCollection.Should().HaveService<IInitDefaultCommands>().WithImplementation<InitDefaultCommands>().AsSingleton();
        dummyServiceCollection.Should().HaveService<ILoadSearchOsItems>().WithImplementation<LoadSearchOsItems>().AsSingleton();
        dummyServiceCollection.Should().HaveService<IProcessByPath>().WithImplementation<ProcessByPath>().AsSingleton();
        dummyServiceCollection.Should().HaveService<ITaskbarItemProgressState>().WithImplementation<CurrentTaskbarItemProgressState>().AsSingleton();
    }
}