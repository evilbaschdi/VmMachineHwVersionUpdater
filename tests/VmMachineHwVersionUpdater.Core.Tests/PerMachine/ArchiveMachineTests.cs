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
        machine.Path = @"C:\NonExistent\fake.vmx";

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
}