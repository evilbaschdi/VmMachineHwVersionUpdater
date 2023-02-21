using VmMachineHwVersionUpdater.ViewModels.Internal;

namespace VmMachineHwVersionUpdater.Tests.ViewModels.Internal;

public class LoadSearchOsItemsTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(LoadSearchOsItems).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(LoadSearchOsItems sut)
    {
        sut.Should().BeAssignableTo<ILoadSearchOsItems>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(LoadSearchOsItems).GetMethods().Where(method => !method.IsAbstract));
    }
}