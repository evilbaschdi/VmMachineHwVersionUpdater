namespace VmMachineHwVersionUpdater.Core.Tests.PerMachine;

public class ArchiveMachineTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ArchiveMachine).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(ArchiveMachine sut)
    {
        sut.Should().BeAssignableTo<IArchiveMachine>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ArchiveMachine).GetMethods().Where(method => !method.IsAbstract));
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void RunFor_WithNonExistentFile_ReturnsWithoutMoving(
        ArchiveMachine sut,
        Machine machine)
    {
        // Arrange
        machine.Path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"), "fake.vmx");

        // Act & Assert
        var act = () => sut.RunFor(machine);
        act.Should().NotThrow();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void RunFor_WithNullMachine_ThrowsArgumentNullException(
        ArchiveMachine sut)
    {
        // Act & Assert
        var act = () => sut.RunFor(null!);
        act.Should().Throw<ArgumentNullException>()
           .WithParameterName("machine");
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void RunFor_WithExistingFile_MovesDirectoryToArchivePreservingPathSegments(
        [Frozen] IPathSettings pathSettings,
        [Frozen] IToggleToolsSyncTime toggleToolsSyncTime,
        [Frozen] IToggleToolsUpgradePolicy toggleToolsUpgradePolicy,
        [Frozen] IToggleMksEnable3D toggleMksEnable3D,
        [Frozen] IUpdateMachineVersion updateMachineVersion,
        [Frozen] IUpdateMachineMemSize updateMachineMemSize,
        ArchiveMachine sut)
    {
        var testRoot = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        var poolPath = Path.Combine(testRoot, "Pool");
        var sourcePath = Path.Combine(poolPath, "Nested", "Existing Machine");
        var archivePath = Path.Combine(poolPath, "_archive");
        Directory.CreateDirectory(sourcePath);
        var machineFilePath = Path.Combine(sourcePath, "machine.vmx");
        File.WriteAllText(machineFilePath, string.Empty);

        var machine = new Machine(
            toggleToolsSyncTime,
            toggleToolsUpgradePolicy,
            toggleMksEnable3D,
            updateMachineVersion,
            updateMachineMemSize)
        {
            Directory = poolPath,
            Path = machineFilePath
        };
        pathSettings.ArchivePath.Returns([archivePath]);

        try
        {
            sut.RunFor(machine);

            Directory.Exists(Path.Combine(archivePath, "Nested", "Existing Machine")).Should().BeTrue();
            Directory.Exists(sourcePath).Should().BeFalse();
        }
        finally
        {
            Directory.Delete(testRoot, true);
        }
    }
}