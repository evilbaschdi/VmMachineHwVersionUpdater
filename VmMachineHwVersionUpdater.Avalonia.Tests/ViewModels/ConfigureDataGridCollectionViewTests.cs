using VmMachineHwVersionUpdater.Avalonia.ViewModels;

namespace VmMachineHwVersionUpdater.Avalonia.Tests.ViewModels;

public class ConfigureDataGridCollectionViewTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ConfigureDataGridCollectionView).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(ConfigureDataGridCollectionView sut)
    {
        sut.Should().BeAssignableTo<IConfigureDataGridCollectionView>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ConfigureDataGridCollectionView).GetMethods().Where(method => !method.IsAbstract));
    }
}