using VmMachineHwVersionUpdater.Core.PerMachine;

namespace VmMachineHwVersionUpdater.Core.Tests.PerMachine;

public class SetMachineIsEnabledForEditingTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(SetMachineIsEnabledForEditing).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(SetMachineIsEnabledForEditing sut)
    {
        sut.Should().BeAssignableTo<ISetMachineIsEnabledForEditing>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(SetMachineIsEnabledForEditing).GetMethods().Where(method => !method.IsAbstract));
    }
}