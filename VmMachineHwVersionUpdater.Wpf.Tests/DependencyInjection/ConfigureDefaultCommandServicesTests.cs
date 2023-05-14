using EvilBaschdi.About.Core;
using EvilBaschdi.Core.Wpf;
using VmMachineHwVersionUpdater.Wpf.DependencyInjection;
using VmMachineHwVersionUpdater.Wpf.ViewModels.Internal;

namespace VmMachineHwVersionUpdater.Wpf.Tests.DependencyInjection;

public class ConfigureDefaultCommandServicesTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ConfigureDefaultCommandServices).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(ConfigureDefaultCommandServices sut)
    {
        sut.Should().BeAssignableTo<IConfigureDefaultCommandServices>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ConfigureDefaultCommandServices).GetMethods().Where(method => !method.IsAbstract));
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void RunFor_ForProvidedServiceCollection_ReturnsServiceCollectionWithInstances(
        ConfigureDefaultCommandServices sut
    )
    {
        // Arrange
        IServiceCollection dummyServiceCollection = new ServiceCollection();

        // Act
        sut.RunFor(dummyServiceCollection);

        // Assert
        dummyServiceCollection.Should().HaveCount(15);
        dummyServiceCollection.Should().HaveService<IAboutContent>().WithImplementation<AboutContent>().AsSingleton();
        dummyServiceCollection.Should().HaveService<IAboutWindowClickDefaultCommand>().WithImplementation<AboutWindowClickDefaultCommand>().AsSingleton();
        dummyServiceCollection.Should().HaveService<IAddEditAnnotation>().WithImplementation<AddEditAnnotation>().AsSingleton();
        dummyServiceCollection.Should().HaveService<IAddEditAnnotationDefaultCommand>().WithImplementation<AddEditAnnotationDefaultCommand>().AsSingleton();
        dummyServiceCollection.Should().HaveService<IArchiveDefaultCommand>().WithImplementation<ArchiveDefaultCommand>().AsSingleton();
        dummyServiceCollection.Should().HaveService<IChangeDisplayName>().WithImplementation<ChangeDisplayName>().AsSingleton();
        dummyServiceCollection.Should().HaveService<ICopyDefaultCommand>().WithImplementation<CopyDefaultCommand>().AsSingleton();
        dummyServiceCollection.Should().HaveService<ICurrentAssembly>().WithImplementation<CurrentAssembly>().AsSingleton();
        dummyServiceCollection.Should().HaveService<IDeleteDefaultCommand>().WithImplementation<DeleteDefaultCommand>().AsSingleton();
        dummyServiceCollection.Should().HaveService<IGotToDefaultCommand>().WithImplementation<GotToDefaultCommand>().AsSingleton();
        dummyServiceCollection.Should().HaveService<IOpenWithCodeDefaultCommand>().WithImplementation<OpenWithCodeDefaultCommand>().AsSingleton();
        dummyServiceCollection.Should().HaveService<IReloadDefaultCommand>().WithImplementation<ReloadDefaultCommand>().AsSingleton();
        dummyServiceCollection.Should().HaveService<IRenameDefaultCommand>().WithImplementation<RenameDefaultCommand>().AsSingleton();
        dummyServiceCollection.Should().HaveService<IStartDefaultCommand>().WithImplementation<StartDefaultCommand>().AsSingleton();
        dummyServiceCollection.Should().HaveService<IUpdateAllDefaultCommand>().WithImplementation<UpdateAllDefaultCommand>().AsSingleton();
    }
}