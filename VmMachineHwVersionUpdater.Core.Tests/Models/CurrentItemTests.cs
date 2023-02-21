using VmMachineHwVersionUpdater.Core.Models;

namespace VmMachineHwVersionUpdater.Core.Tests.Models;

public class CurrentItemTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(CurrentItem).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(CurrentItem sut)
    {
        sut.Should().BeAssignableTo<ICurrentItem>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(CurrentItem).GetMethods().Where(method => !method.IsAbstract & !method.Name.StartsWith("set_")));
    }
}