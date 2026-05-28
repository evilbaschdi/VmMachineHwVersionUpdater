namespace VmMachineHwVersionUpdater.Core.Tests.PerMachine;

public class HandleMachineFromPathTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(HandleMachineFromPath).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(HandleMachineFromPath sut)
    {
        sut.Should().BeAssignableTo<IHandleMachineFromPath>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(HandleMachineFromPath).GetMethods().Where(method => !method.IsAbstract));
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void ValueFor_WithNullMachinePath_ThrowsArgumentNullException(
        HandleMachineFromPath sut)
    {
        // Act & Assert
        var act = () => sut.ValueFor(null!);
        act.Should().Throw<ArgumentNullException>()
           .WithParameterName("machinePath");
    }
}