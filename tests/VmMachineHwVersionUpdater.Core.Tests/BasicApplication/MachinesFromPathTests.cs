using SWalter.Core.Internal;
using SWalter.Core.Model;

namespace VmMachineHwVersionUpdater.Core.Tests.BasicApplication;

public class MachinesFromPathTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(MachinesFromPath).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(MachinesFromPath sut)
    {
        sut.Should().BeAssignableTo<IMachinesFromPath>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(MachinesFromPath).GetMethods().Where(method => !method.IsAbstract));
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Value_WhenOneMachineCannotBeRead_StillReturnsOtherMachines(
        [Frozen] IPathSettings pathSettings,
        [Frozen] IHandleMachineFromPath handleMachineFromPath,
        [Frozen] IFileListFromPath fileListFromPath,
        Machine readableMachine,
        MachinesFromPath sut)
    {
        var machinePoolPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(machinePoolPath);

        try
        {
            var readablePath = Path.Combine(machinePoolPath, "readable.vmx");
            var unreadablePath = Path.Combine(machinePoolPath, "unreadable.vmx");
            pathSettings.VmPool.Returns([machinePoolPath]);
            pathSettings.ArchivePath.Returns([]);
            fileListFromPath.ValueFor(machinePoolPath, Arg.Any<FileListFromPathFilter>())
                            .Returns([readablePath, unreadablePath]);
            handleMachineFromPath.ValueFor(Arg.Is<MachinePath>(path => path.MachineFilePath == readablePath))
                                 .Returns(readableMachine);
            handleMachineFromPath.ValueFor(Arg.Is<MachinePath>(path => path.MachineFilePath == unreadablePath))
                                 .Returns(_ => throw new FormatException("Invalid VMX data"));

            var result = sut.Value;

            result.Should().ContainSingle().Which.Should().BeSameAs(readableMachine);
        }
        finally
        {
            Directory.Delete(machinePoolPath, true);
        }
    }
}