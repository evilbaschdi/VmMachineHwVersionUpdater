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
        assertion.Verify(typeof(CopyMachine).GetMethods().Where(method => !method.IsAbstract));
    }
}