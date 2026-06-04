using EvilBaschdi.Core.Internal;

namespace VmMachineHwVersionUpdater.Core.Tests;

public class ConfigureCoreServicesTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ConfigureCoreServices).GetConstructors());
    }

    [Fact]
    public void Constructor_ReturnsInterfaceName()
    {
        typeof(ConfigureCoreServices).Should().NotBeNull();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ConfigureCoreServices).GetMethods().Where(method => !method.IsAbstract));
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void RunFor_ForProvidedServiceCollection_ReturnsServiceCollectionWithInstances(
        ServiceCollection serviceCollection)
    {
        // Act
        serviceCollection.AddCoreServices();

        // Assert
        serviceCollection.Should().HaveCount(50);
        serviceCollection.Should().HaveService<IArchiveMachine>()
                         .WithImplementation<ArchiveMachine>()
                         .AsSingleton();
        serviceCollection.Should().HaveService<IConvertAnnotationLineBreaks>()
                         .WithImplementation<ConvertAnnotationLineBreaks>()
                         .AsSingleton();
        serviceCollection.Should().HaveService<ICopyMachine>()
                         .WithImplementation<CopyMachine>()
                         .AsSingleton();
        serviceCollection.Should().HaveService<ICurrentMachine>()
                         .WithImplementation<CurrentMachine>()
                         .AsSingleton();
        serviceCollection.Should().HaveService<IDeleteMachine>()
                         .WithImplementation<DeleteMachine>()
                         .AsSingleton();
        serviceCollection.Should().HaveService<IFileAccessRetryPolicy>()
                         .WithImplementation<FileAccessRetryPolicy>()
                         .AsSingleton();
        serviceCollection.Should().HaveService<IFileListFromPath>()
                         .WithImplementation<FileListFromPath>()
                         .AsSingleton();
        serviceCollection.Should().HaveService<IGuestOsesInUse>()
                         .WithImplementation<GuestOsesInUse>()
                         .AsSingleton();
        serviceCollection.Should().HaveService<IGuestOsOutputStringMapping>()
                         .WithImplementation<GuestOsOutputStringMapping>()
                         .AsSingleton();
        serviceCollection.Should().HaveService<IGuestOsStringMapping>()
                         .WithImplementation<GuestOsStringMapping>()
                         .AsSingleton();
        serviceCollection.Should().HaveService<IHandleMachineFromPath>()
                         .WithImplementation<HandleMachineFromPath>()
                         .AsSingleton();
        serviceCollection.Should().HaveService<ILoad>()
                         .WithImplementation<Load>()
                         .AsSingleton();
        serviceCollection.Should().HaveService<IMachinesFromPath>()
                         .WithImplementation<MachinesFromPath>()
                         .AsSingleton();
        serviceCollection.Should().HaveService<IMachineParserStrategy>()
                         .WithImplementation<MachineParserStrategy>()
                         .AsSingleton();
        serviceCollection.Should().HaveService<IParseVboxFile>()
                         .WithImplementation<ParseVboxFile>()
                         .AsSingleton();
        serviceCollection.Should().HaveService<IParseVmxFile>()
                         .WithImplementation<ParseVmxFile>()
                         .AsSingleton();
        serviceCollection.Should().HaveService<IPathSettings>()
                         .WithImplementation<PathSettings>()
                         .AsSingleton();
        serviceCollection.Should().HaveService<IReadLogInformation>()
                         .WithImplementation<ReadLogInformation>()
                         .AsSingleton();
        serviceCollection.Should().HaveService<IReturnValueFromVmxLine>()
                         .WithImplementation<ReturnValueFromVmxLine>().AsSingleton();
        serviceCollection.Should().HaveService<ISetDisplayName>()
                         .WithImplementation<SetDisplayName>()
                         .AsSingleton();
        serviceCollection.Should().HaveService<ISetMachineIsEnabledForEditing>()
                         .WithImplementation<SetMachineIsEnabledForEditing>()
                         .AsSingleton();
        serviceCollection.Should().HaveService<IToggleToolsSyncTime>().WithImplementation<ToggleToolsSyncTime>()
                         .AsSingleton();
        serviceCollection.Should().HaveService<IToggleToolsUpgradePolicy>()
                         .WithImplementation<ToggleToolsUpgradePolicy>()
                         .AsSingleton();
        serviceCollection.Should().HaveService<IUpdateAnnotation>()
                         .WithImplementation<UpdateAnnotation>()
                         .AsSingleton();
        serviceCollection.Should().HaveService<IUpdateMachineVersion>()
                         .WithImplementation<UpdateMachineVersion>()
                         .AsSingleton();
        serviceCollection.Should().HaveService<IUpdateMachineMemSize>()
                         .WithImplementation<UpdateMachineMemSize>()
                         .AsSingleton();
        serviceCollection.Should().HaveService<IVmPools>()
                         .WithImplementation<VmPools>()
                         .AsSingleton();
        serviceCollection.Should().HaveService<IVmxLineStartsWith>()
                         .WithImplementation<VmxLineStartsWith>()
                         .AsSingleton();
        serviceCollection.Should().HaveService<IToggleMksEnable3D>()
                         .WithImplementation<ToggleMksEnable3D>()
                         .AsSingleton();
    }
}