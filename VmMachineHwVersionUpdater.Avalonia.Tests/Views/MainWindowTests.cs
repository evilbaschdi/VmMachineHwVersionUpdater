using Avalonia.Controls;
using VmMachineHwVersionUpdater.Avalonia.Views;

namespace VmMachineHwVersionUpdater.Avalonia.Tests.Views;

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
        sut.Should().BeAssignableTo<Window>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(MainWindow).GetMethods().Where(method => !method.IsAbstract));
    }
}