using VmMachineHwVersionUpdater.Avalonia.ViewModels;

namespace VmMachineHwVersionUpdater.Avalonia.Tests.ViewModels;

public class FilterDataGridCollectionViewTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(FilterDataGridCollectionView).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(FilterDataGridCollectionView sut)
    {
        sut.Should().BeAssignableTo<IFilterDataGridCollectionView>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(FilterDataGridCollectionView).GetMethods().Where(method => !method.IsAbstract));
    }
}