using VmMachineHwVersionUpdater.Wpf.Internal.Core;
using VmMachineHwVersionUpdater.Wpf.Views;

namespace VmMachineHwVersionUpdater.Wpf.Tests;

public class MainWindowTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(MainWindow).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(MainWindow sut)
    {
        sut.Should().BeAssignableTo<IOnLoaded>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(MainWindow).GetMethods().Where(method => !method.IsAbstract));
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void DataContext_ForCurrentWindow_HasInstance(
        MainWindow sut)
    {
        // Arrange

        // Act
        var result = sut.DataContext;

        // Assert
        result.Should().NotBeNull();
    }
}