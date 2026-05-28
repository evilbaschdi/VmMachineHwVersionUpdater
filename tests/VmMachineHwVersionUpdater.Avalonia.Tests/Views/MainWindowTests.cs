using System.Reflection;
using VmMachineHwVersionUpdater.Avalonia.Views;

namespace VmMachineHwVersionUpdater.Avalonia.Tests.Views;

public class MainWindowTests : AvaloniaTestBase<TestApp>
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(MainWindow).GetConstructors());
    }

    [Fact]
    public void Constructor_ReturnsInterfaceName()
    {
        typeof(MainWindow).Should().BeAssignableTo<Window>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(MainWindow).GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                                           .Where(method => !method.IsAbstract & !method.IsSpecialName & method.Name != "InitializeComponent"));
    }
}