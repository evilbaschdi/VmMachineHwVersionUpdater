using VmMachineHwVersionUpdater.Wpf.ViewModels.Internal;

namespace VmMachineHwVersionUpdater.Wpf.Tests.ViewModels.Internal;

public class UpdateAllDefaultCommandTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(UpdateAllDefaultCommand).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(UpdateAllDefaultCommand sut)
    {
        sut.Should().BeAssignableTo<IUpdateAllDefaultCommand>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(UpdateAllDefaultCommand).GetMethods().Where(method => !method.IsAbstract));
    }
}