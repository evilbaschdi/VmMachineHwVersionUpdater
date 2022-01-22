using AutoFixture.Idioms;
using EvilBaschdi.Testing;
using Xunit;

namespace VmMachineHwVersionUpdater.Tests;

public class MainWindowTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(MainWindow).GetConstructors());
    }

    //[Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    //public void Constructor_ReturnsInterfaceName(MainWindow sut)
    //{
    //    sut.Should().BeAssignableTo<MetroWindow>();
    //}

    //[Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    //public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    //{
    //    assertion.Verify(typeof(MainWindow).GetMethods().Where(method => !method.IsAbstract));
    //}
}