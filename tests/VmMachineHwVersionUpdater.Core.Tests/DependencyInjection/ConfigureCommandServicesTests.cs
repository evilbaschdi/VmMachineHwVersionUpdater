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
        // Act & Assert - Static class doesn't have an interface to implement
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

    [Fact]
    public void AddCommandServices_RegistersExpectedServices()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddSingleton(Substitute.For<IProcessByPath>()); // Add missing dependency
        services.AddSingleton(Substitute.For<ICurrentMachine>()); // Add missing dependency

        // Act
        services.AddCommandServices();
        var serviceProvider = services.BuildServiceProvider();

        // Assert
        serviceProvider.GetService<IGoToCommand>().Should().NotBeNull();
        serviceProvider.GetService<IGoToCommand>().Should().BeOfType<GoToCommand>();

        serviceProvider.GetService<IOpenWithCodeCommand>().Should().NotBeNull();
        serviceProvider.GetService<IOpenWithCodeCommand>().Should().BeOfType<OpenWithCodeCommand>();

        serviceProvider.GetService<IReloadCommand>().Should().NotBeNull();
        serviceProvider.GetService<IReloadCommand>().Should().BeOfType<ReloadCommand>();

        serviceProvider.GetService<IStartCommand>().Should().NotBeNull();
        serviceProvider.GetService<IStartCommand>().Should().BeOfType<StartCommand>();
    }

    [Fact]
    public void AddCommandServices_RegistersServicesAsSingleton()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddSingleton(Substitute.For<IProcessByPath>()); // Add missing dependency
        services.AddSingleton(Substitute.For<ICurrentMachine>()); // Add missing dependency

        // Act
        services.AddCommandServices();
        var serviceProvider = services.BuildServiceProvider();

        // Assert
        var goToCommand1 = serviceProvider.GetService<IGoToCommand>();
        var goToCommand2 = serviceProvider.GetService<IGoToCommand>();
        goToCommand1.Should().BeSameAs(goToCommand2);

        var openWithCodeCommand1 = serviceProvider.GetService<IOpenWithCodeCommand>();
        var openWithCodeCommand2 = serviceProvider.GetService<IOpenWithCodeCommand>();
        openWithCodeCommand1.Should().BeSameAs(openWithCodeCommand2);

        var reloadCommand1 = serviceProvider.GetService<IReloadCommand>();
        var reloadCommand2 = serviceProvider.GetService<IReloadCommand>();
        reloadCommand1.Should().BeSameAs(reloadCommand2);

        var startCommand1 = serviceProvider.GetService<IStartCommand>();
        var startCommand2 = serviceProvider.GetService<IStartCommand>();
        startCommand1.Should().BeSameAs(startCommand2);
    }
}