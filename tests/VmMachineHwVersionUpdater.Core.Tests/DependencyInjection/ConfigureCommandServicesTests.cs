namespace VmMachineHwVersionUpdater.Core.Tests.DependencyInjection;

public class ConfigureCommandServicesTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ConfigureCommandServices).GetConstructors());
    }

    [Fact]
    public void Constructor_ReturnsInterfaceName()
    {
        typeof(ConfigureCommandServices).Should().NotBeNull();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ConfigureCommandServices).GetMethods().Where(method => !method.IsAbstract));
    }

    [Fact]
    public void AddCommandServices_WithNullServices_ThrowsArgumentNullException()
    {
        // Act & Assert
        ((Action)(() => ConfigureCommandServices.AddCommandServices(null!)))
            .Should().Throw<ArgumentNullException>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void AddCommandServices_RegistersExpectedServices(ServiceCollection serviceCollection)
    {
        // Act
        serviceCollection.AddCommandServices();

        // Assert
        serviceCollection.Should().HaveService<IGoToCommand>()
                         .WithImplementation<GoToCommand>()
                         .AsSingleton();
        serviceCollection.Should().HaveService<IOpenWithCodeCommand>()
                         .WithImplementation<OpenWithCodeCommand>()
                         .AsSingleton();
        serviceCollection.Should().HaveService<IReloadCommand>()
                         .WithImplementation<ReloadCommand>()
                         .AsSingleton();
        serviceCollection.Should().HaveService<IStartCommand>()
                         .WithImplementation<StartCommand>()
                         .AsSingleton();
    }
}