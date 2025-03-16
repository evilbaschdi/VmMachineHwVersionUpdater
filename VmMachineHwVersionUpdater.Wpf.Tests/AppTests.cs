using System.Windows;

namespace VmMachineHwVersionUpdater.Wpf.Tests;

public class AppTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(App).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(App sut)
    {
        sut.Should().BeAssignableTo<Application>();
    }

    //[Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    //public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    //{
    //    assertion.Verify(typeof(App).GetMethods().Where(method => !method.IsAbstract & !method.Name.StartsWith("set")));
    //}
}