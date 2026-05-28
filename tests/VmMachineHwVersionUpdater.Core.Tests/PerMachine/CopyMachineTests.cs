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
        machine.Path = @"C:\NonExistent\fake.vmx";

        // Act
        await sut.RunForAsync(machine, "newDir", TestContext.Current.CancellationToken);

        // Assert
        await copyDirectory.DidNotReceive()
                           .RunForAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>());
    }
}