using VmMachineHwVersionUpdater.Avalonia.ViewModels.Internal;

namespace VmMachineHwVersionUpdater.Avalonia.Tests.ViewModels.Internal;

public class UpdateAllReactiveCommandTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(UpdateAllReactiveCommand).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(UpdateAllReactiveCommand sut)
    {
        sut.Should().BeAssignableTo<IUpdateAllReactiveCommand>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(UpdateAllReactiveCommand).GetMethods().Where(method => !method.IsAbstract));
    }
}