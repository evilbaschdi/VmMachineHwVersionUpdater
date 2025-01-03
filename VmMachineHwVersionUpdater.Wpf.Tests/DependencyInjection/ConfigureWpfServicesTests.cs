using EvilBaschdi.Core.Wpf;
using VmMachineHwVersionUpdater.Wpf.DependencyInjection;
using VmMachineHwVersionUpdater.Wpf.ViewModels.Internal;

namespace VmMachineHwVersionUpdater.Wpf.Tests.DependencyInjection;

public class ConfigureWpfServicesTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ConfigureWpfServices).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ConfigureWpfServices).GetMethods().Where(method => !method.IsAbstract));
    }

    [Fact]
    public void RunFor_ForProvidedServiceCollection_ReturnsServiceCollectionWithInstances()
    {
        // Arrange
        IServiceCollection dummyServiceCollection = new ServiceCollection();

        // Act
        dummyServiceCollection.AddWpfServices();

        // Assert
        dummyServiceCollection.Should().HaveCount(8);
        dummyServiceCollection.Should().HaveService<IApplicationLayout>().WithImplementation<ApplicationLayout>().AsSingleton();
        dummyServiceCollection.Should().HaveService<IApplicationStyle>().WithImplementation<ApplicationStyle>().AsSingleton();
        dummyServiceCollection.Should().HaveService<IApplyMicaBrush>().WithImplementation<ApplyMicaBrush>().AsSingleton();
        dummyServiceCollection.Should().HaveService<IConfigureListCollectionView>().WithImplementation<ConfigureListCollectionView>().AsSingleton();
        dummyServiceCollection.Should().HaveService<ICurrentItemSource>().WithImplementation<CurrentItemSource>().AsSingleton();
        dummyServiceCollection.Should().HaveService<IFilterListCollectionView>().WithImplementation<FilterListCollectionView>().AsSingleton();
        dummyServiceCollection.Should().HaveService<ISeparator>().WithImplementation<SystemWindowsControlsSeparator>().AsTransient();
        dummyServiceCollection.Should().HaveService<ITaskbarItemProgressState>().WithImplementation<CurrentTaskbarItemProgressState>().AsSingleton();
    }
}