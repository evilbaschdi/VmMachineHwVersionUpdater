using VmMachineHwVersionUpdater.Avalonia.DependencyInjection;
using VmMachineHwVersionUpdater.Avalonia.ViewModels.Internal;

namespace VmMachineHwVersionUpdater.Avalonia.Tests.DependencyInjection;

public class ConfigureReactiveCommandServicesTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ConfigureReactiveCommandServices).GetConstructors());
    }

    [Fact]
    public void Constructor_ReturnsInterfaceName()
    {
        typeof(ConfigureReactiveCommandServices).Should().NotBeNull();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ConfigureReactiveCommandServices).GetMethods().Where(method => !method.IsAbstract));
    }

    [Fact]
    public void AddReactiveCommandServices_WithNullServices_ThrowsArgumentNullException()
    {
        // Act & Assert
        ((Action)(() => ConfigureReactiveCommandServices.AddReactiveCommandServices(null!)))
            .Should().Throw<ArgumentNullException>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void AddReactiveCommandServices_RegistersExpectedServices(ServiceCollection serviceCollection)
    {
        // Act
        serviceCollection.AddReactiveCommandServices();

        // Assert
        serviceCollection.Should().HaveService<IInitReactiveCommands>()
                         .WithImplementation<InitReactiveCommands>()
                         .AsSingleton();
        serviceCollection.Should().HaveService<IAboutWindowReactiveCommand>()
                         .WithImplementation<AboutWindowReactiveCommand>()
                         .AsSingleton();
        serviceCollection.Should().HaveService<IAddEditAnnotationReactiveCommand>()
                         .WithImplementation<AddEditAnnotationReactiveCommand>()
                         .AsSingleton();
        serviceCollection.Should().HaveService<IArchiveReactiveCommand>()
                         .WithImplementation<ArchiveReactiveCommand>()
                         .AsSingleton();
        serviceCollection.Should().HaveService<ICopyReactiveCommand>()
                         .WithImplementation<CopyReactiveCommand>()
                         .AsSingleton();
        serviceCollection.Should().HaveService<IDeleteReactiveCommand>()
                         .WithImplementation<DeleteReactiveCommand>()
                         .AsSingleton();
        serviceCollection.Should().HaveService<IGoToReactiveCommand>()
                         .WithImplementation<GoToReactiveCommand>()
                         .AsSingleton();
        serviceCollection.Should().HaveService<IOpenWithCodeReactiveCommand>()
                         .WithImplementation<OpenWithCodeReactiveCommand>()
                         .AsSingleton();
        serviceCollection.Should().HaveService<IReloadReactiveCommand>()
                         .WithImplementation<ReloadReactiveCommand>()
                         .AsSingleton();
        serviceCollection.Should().HaveService<IRenameReactiveCommand>()
                         .WithImplementation<RenameReactiveCommand>()
                         .AsSingleton();
        serviceCollection.Should().HaveService<IStartReactiveCommand>()
                         .WithImplementation<StartReactiveCommand>()
                         .AsSingleton();
        serviceCollection.Should().HaveService<IUpdateAllReactiveCommand>()
                         .WithImplementation<UpdateAllReactiveCommand>()
                         .AsSingleton();
    }
}