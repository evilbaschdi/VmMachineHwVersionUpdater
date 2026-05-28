namespace VmMachineHwVersionUpdater.Core.Tests.Commands;

public class GoToCommandTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(GoToCommand).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(GoToCommand sut)
    {
        sut.Should().BeAssignableTo<IGoToCommand>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(GoToCommand).GetMethods().Where(method => !method.IsAbstract));
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Run_WhenCurrentMachineValueIsNull_DoesNotCallProcessByPath(
        [Frozen] IProcessByPath processByPath,
        [Frozen] ICurrentMachine currentMachine,
        GoToCommand sut)
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
        GoToCommand sut,
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