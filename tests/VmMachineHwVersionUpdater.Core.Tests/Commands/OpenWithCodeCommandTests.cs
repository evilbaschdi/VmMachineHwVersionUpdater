using System.Reflection;

namespace VmMachineHwVersionUpdater.Core.Tests.Commands;

public class OpenWithCodeCommandTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(OpenWithCodeCommand).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(OpenWithCodeCommand sut)
    {
        sut.Should().BeAssignableTo<IOpenWithCodeCommand>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(OpenWithCodeCommand).GetMethods(BindingFlags.Public | BindingFlags.Instance)
                                                    .Where(method => !method.IsAbstract & !method.ReturnType.IsAssignableTo(typeof(Task))));
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public async Task RunAsync_WhenCurrentMachineValueIsNull_CallsProcessByPathWithNullPath(
        [Frozen] IProcessByPath processByPath,
        [Frozen] ICurrentMachine currentMachine,
        OpenWithCodeCommand sut)
    {
        // Arrange
        currentMachine.Value.Returns((Machine)null);

        // Act
        await sut.RunAsync(TestContext.Current.CancellationToken);

        // Assert
        processByPath.Received(1).RunFor("vscode://file/");
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public async Task RunAsync_WhenMachinePathDoesNotExist_CallsProcessByPathWithFormattedPath(
        [Frozen] IProcessByPath processByPath,
        [Frozen] ICurrentMachine currentMachine,
        OpenWithCodeCommand sut,
        Machine machine)
    {
        // Arrange
        machine.Path = @"C:\NonExistent\fake.vmx";
        currentMachine.Value.Returns(machine);

        // Act
        await sut.RunAsync(TestContext.Current.CancellationToken);

        // Assert
        processByPath.Received(1).RunFor(@"vscode://file/C:\NonExistent\fake.vmx");
    }
}