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
}