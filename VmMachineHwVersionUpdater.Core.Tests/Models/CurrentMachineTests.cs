namespace VmMachineHwVersionUpdater.Core.Tests.Models;

public class CurrentMachineTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(CurrentMachine).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(CurrentMachine sut)
    {
        sut.Should().BeAssignableTo<ICurrentMachine>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(CurrentMachine).GetMethods().Where(method => !method.IsAbstract & !method.Name.StartsWith("set_")));
    }
}