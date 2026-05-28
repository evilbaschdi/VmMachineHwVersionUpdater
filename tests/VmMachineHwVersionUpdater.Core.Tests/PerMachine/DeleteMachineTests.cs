namespace VmMachineHwVersionUpdater.Core.Tests.PerMachine;

public class DeleteMachineTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(DeleteMachine).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(DeleteMachine sut)
    {
        sut.Should().BeAssignableTo<IDeleteMachine>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(DeleteMachine).GetMethods().Where(method => !method.IsAbstract));
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void RunFor_WithNonExistentPath_DoesNotThrow(
        DeleteMachine sut)
    {
        // Act & Assert
        var act = () => sut.RunFor(@"C:\NonExistent\fake.vmx");
        act.Should().NotThrow();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void RunFor_WithNullPath_ThrowsArgumentNullException(
        DeleteMachine sut)
    {
        // Act & Assert
        var act = () => sut.RunFor(null!);
        act.Should().Throw<ArgumentNullException>()
           .WithParameterName("path");
    }
}