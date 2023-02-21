using EvilBaschdi.Core.Internal;
using FluentAssertions.Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using VmMachineHwVersionUpdater.Core.BasicApplication;
using VmMachineHwVersionUpdater.Core.DependencyInjection;
using VmMachineHwVersionUpdater.Core.Models;
using VmMachineHwVersionUpdater.Core.PerMachine;
using VmMachineHwVersionUpdater.Core.Settings;

namespace VmMachineHwVersionUpdater.Core.Tests;

public class ConfigureCoreServicesTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ConfigureCoreServices).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(ConfigureCoreServices sut)
    {
        sut.Should().BeAssignableTo<IConfigureCoreServices>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ConfigureCoreServices).GetMethods().Where(method => !method.IsAbstract));
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void RunFor_ForProvidedServiceCollection_ReturnsServiceCollectionWithInstances(
        ConfigureCoreServices sut
    )
    {
        // Arrange
        IServiceCollection dummyServiceCollection = new ServiceCollection();

        // Act
        sut.RunFor(dummyServiceCollection);

        // Assert
        dummyServiceCollection.Should().HaveCount(25);
        dummyServiceCollection.Should().HaveService<IArchiveMachine>().WithImplementation<ArchiveMachine>().AsSingleton();
        dummyServiceCollection.Should().HaveService<IConvertAnnotationLineBreaks>().WithImplementation<ConvertAnnotationLineBreaks>().AsSingleton();
        dummyServiceCollection.Should().HaveService<ICopyMachine>().WithImplementation<CopyMachine>().AsSingleton();
        dummyServiceCollection.Should().HaveService<ICurrentItem>().WithImplementation<CurrentItem>().AsSingleton();
        dummyServiceCollection.Should().HaveService<IDeleteMachine>().WithImplementation<DeleteMachine>().AsSingleton();
        dummyServiceCollection.Should().HaveService<IFileListFromPath>().WithImplementation<FileListFromPath>().AsSingleton();
        dummyServiceCollection.Should().HaveService<IGuestOsesInUse>().WithImplementation<GuestOsesInUse>().AsSingleton();
        dummyServiceCollection.Should().HaveService<IGuestOsOutputStringMapping>().WithImplementation<GuestOsOutputStringMapping>().AsSingleton();
        dummyServiceCollection.Should().HaveService<IGuestOsStringMapping>().WithImplementation<GuestOsStringMapping>().AsSingleton();
        dummyServiceCollection.Should().HaveService<IHandleMachineFromPath>().WithImplementation<HandleMachineFromPath>().AsSingleton();
        dummyServiceCollection.Should().HaveService<ILoad>().WithImplementation<Load>().AsSingleton();
        dummyServiceCollection.Should().HaveService<IMachinesFromPath>().WithImplementation<MachinesFromPath>().AsSingleton();
        dummyServiceCollection.Should().HaveService<IParseVmxFile>().WithImplementation<ParseVmxFile>().AsSingleton();
        dummyServiceCollection.Should().HaveService<IPathSettings>().WithImplementation<PathSettings>().AsSingleton();
        dummyServiceCollection.Should().HaveService<IReadLogInformation>().WithImplementation<ReadLogInformation>().AsSingleton();
        dummyServiceCollection.Should().HaveService<IReturnValueFromVmxLine>().WithImplementation<ReturnValueFromVmxLine>().AsSingleton();
        dummyServiceCollection.Should().HaveService<ISetDisplayName>().WithImplementation<SetDisplayName>().AsSingleton();
        dummyServiceCollection.Should().HaveService<ISetMachineIsEnabledForEditing>().WithImplementation<SetMachineIsEnabledForEditing>().AsSingleton();
        dummyServiceCollection.Should().HaveService<ISettingsValid>().WithImplementation<SettingsValid>().AsSingleton();
        dummyServiceCollection.Should().HaveService<IToggleToolsSyncTime>().WithImplementation<ToggleToolsSyncTime>().AsSingleton();
        dummyServiceCollection.Should().HaveService<IToggleToolsUpgradePolicy>().WithImplementation<ToggleToolsUpgradePolicy>().AsSingleton();
        dummyServiceCollection.Should().HaveService<IUpdateAnnotation>().WithImplementation<UpdateAnnotation>().AsSingleton();
        dummyServiceCollection.Should().HaveService<IUpdateMachineVersion>().WithImplementation<UpdateMachineVersion>().AsSingleton();
        dummyServiceCollection.Should().HaveService<IVmPools>().WithImplementation<VmPools>().AsSingleton();
        dummyServiceCollection.Should().HaveService<IVmxLineStartsWith>().WithImplementation<VmxLineStartsWith>().AsSingleton();
    }
}