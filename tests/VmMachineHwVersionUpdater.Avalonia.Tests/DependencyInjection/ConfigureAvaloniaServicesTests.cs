using System.Collections;
using EvilBaschdi.Core.Avalonia.Lifetime;
using VmMachineHwVersionUpdater.Avalonia.DependencyInjection;
using VmMachineHwVersionUpdater.Avalonia.ViewModels;

namespace VmMachineHwVersionUpdater.Avalonia.Tests.DependencyInjection;

public class ConfigureAvaloniaServicesTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ConfigureAvaloniaServices).GetConstructors());
    }

    [Fact]
    public void Constructor_ReturnsInterfaceName()
    {
        typeof(ConfigureAvaloniaServices).Should().NotBeNull();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ConfigureAvaloniaServices).GetMethods().Where(method => !method.IsAbstract));
    }

    [Fact]
    public void AddAvaloniaServices_WithNullServices_ThrowsArgumentNullException()
    {
        // Act & Assert
        ((Action)(() => ConfigureAvaloniaServices.AddAvaloniaServices(null!)))
            .Should().Throw<ArgumentNullException>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void AddAvaloniaServices_RegistersExpectedServices(ServiceCollection serviceCollection)
    {
        // Act
        serviceCollection.AddAvaloniaServices();

        // Assert
        serviceCollection.Should().HaveService<ISeparator>()
                         .WithImplementation<AvaloniaControlsSeparator>()
                         .AsTransient();
        serviceCollection.Should().HaveService<IComparer>()
                         .WithImplementation<MachineComparer>()
                         .AsSingleton();
        serviceCollection.Should().HaveService<IConfigureDataGridCollectionView>()
                         .WithImplementation<ConfigureDataGridCollectionView>()
                         .AsSingleton();
        serviceCollection.Should().HaveService<IFilterDataGridCollectionView>()
                         .WithImplementation<FilterDataGridCollectionView>()
                         .AsSingleton();
        serviceCollection.Should().HaveService<IVmFileChangeHandler>()
                         .WithImplementation<VmFileChangeHandler>()
                         .AsSingleton();
        serviceCollection.Should().HaveService<IMainWindowByApplicationLifetime>()
                         .WithImplementation<MainWindowByApplicationLifetime>()
                         .AsSingleton();
    }
}