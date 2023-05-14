using Avalonia.Controls.Templates;

namespace VmMachineHwVersionUpdater.Avalonia.Tests;

public class ViewLocatorTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ViewLocator).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(ViewLocator sut)
    {
        sut.Should().BeAssignableTo<IDataTemplate>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ViewLocator).GetMethods().Where(method => !method.IsAbstract));
    }
}