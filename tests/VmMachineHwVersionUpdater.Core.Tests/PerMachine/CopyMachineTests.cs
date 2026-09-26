using System.Reflection;
using EvilBaschdi.Core.Internal.Copy;

namespace VmMachineHwVersionUpdater.Core.Tests.PerMachine;

public class CopyMachineTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(CopyMachine).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(CopyMachine sut)
    {
        sut.Should().BeAssignableTo<ICopyMachine>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(CopyMachine).GetMethods(BindingFlags.Public | BindingFlags.Instance)
                                            .Where(method => !method.IsAbstract & !method.ReturnType.IsAssignableTo(typeof(Task))));
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public async Task RunForAsync_WithNullMachine_ThrowsArgumentNullException(
        CopyMachine sut)
    {
        // Act & Assert
        var act = () => sut.RunForAsync(null!, "newDir");
        await act.Should().ThrowAsync<ArgumentNullException>()
                 .WithParameterName("machine");
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public async Task RunForAsync_WithNullNewDirectoryName_ThrowsArgumentNullException(
        CopyMachine sut,
        Machine machine)
    {
        // Act & Assert
        var act = () => sut.RunForAsync(machine, null!);
        await act.Should().ThrowAsync<ArgumentNullException>()
                 .WithParameterName("newDirectoryName");
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public async Task RunForAsync_WithNonExistentFile_ReturnsWithoutCopying(
        [Frozen] ICopyDirectoryWithProgress copyDirectory,
        CopyMachine sut,
        Machine machine)
    {
        // Arrange
        machine.Path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"), "fake.vmx");

        // Act
        await sut.RunForAsync(machine, "newDir", TestContext.Current.CancellationToken);

        // Assert
        await copyDirectory.DidNotReceive()
                           .RunForAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public async Task RunForAsync_WithExistingFile_CopiesToDirectoryUnderPool(
        [Frozen] ICopyDirectoryWithProgress copyDirectory,
        [Frozen] IToggleToolsSyncTime toggleToolsSyncTime,
        [Frozen] IToggleToolsUpgradePolicy toggleToolsUpgradePolicy,
        [Frozen] IToggleMksEnable3D toggleMksEnable3D,
        [Frozen] IUpdateMachineVersion updateMachineVersion,
        [Frozen] IUpdateMachineMemSize updateMachineMemSize,
        CopyMachine sut)
    {
        var testRoot = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        var poolPath = Path.Combine(testRoot, "Pool");
        var sourcePath = Path.Combine(poolPath, "Nested", "Existing Machine");
        Directory.CreateDirectory(sourcePath);
        var machineFilePath = Path.Combine(sourcePath, "machine.vmx");
        await File.WriteAllTextAsync(machineFilePath, string.Empty, TestContext.Current.CancellationToken);

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
        copyDirectory.RunForAsync(sourcePath, Path.Combine(poolPath, "Copied Machine"), Arg.Any<CancellationToken>())
                     .Returns(Task.CompletedTask);

        try
        {
            await sut.RunForAsync(machine, "Copied Machine", TestContext.Current.CancellationToken);

            await copyDirectory.Received(1)
                               .RunForAsync(sourcePath,
                                   Path.Combine(poolPath, "Copied Machine"),
                                   TestContext.Current.CancellationToken);
        }
        finally
        {
            Directory.Delete(testRoot, true);
        }
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public async Task RunForAsync_WithDirectoryTraversalName_ThrowsArgumentException(CopyMachine sut, Machine machine)
    {
        var act = () => sut.RunForAsync(machine, "..\\Outside");

        await act.Should().ThrowAsync<ArgumentException>()
                 .WithParameterName("newDirectoryName");
    }
}