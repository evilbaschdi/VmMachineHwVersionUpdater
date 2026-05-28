namespace VmMachineHwVersionUpdater.Core.Tests.Commands;

public class StartCommandTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(StartCommand).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(StartCommand sut)
    {
        sut.Should().BeAssignableTo<IStartCommand>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(StartCommand).GetMethods().Where(method => !method.IsAbstract));
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Run_WhenCurrentMachineValueIsNull_DoesNotCallProcessByPath(
        [Frozen] IProcessByPath processByPath,
        [Frozen] ICurrentMachine currentMachine,
        StartCommand sut)
    {
        // Arrange
        currentMachine.Value.Returns((Machine)null);

        // Act
        sut.Run();

        // Assert
        processByPath.DidNotReceive().RunFor(Arg.Any<string>());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Run_WhenMachinePathDoesNotExist_DoesNotCallProcessByPath(
        [Frozen] IProcessByPath processByPath,
        [Frozen] ICurrentMachine currentMachine,
        StartCommand sut,
        Machine machine)
    {
        // Arrange
        machine.Path = @"C:\NonExistent\fake.vmx";
        currentMachine.Value.Returns(machine);

        // Act
        sut.Run();

        // Assert
        processByPath.DidNotReceive().RunFor(Arg.Any<string>());
    }
}