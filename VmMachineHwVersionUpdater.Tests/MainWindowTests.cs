using System.Linq;
using AutoFixture.Idioms;
using EvilBaschdi.Testing;
using FluentAssertions;
using Xunit;

namespace VmMachineHwVersionUpdater.Tests;

public class MainWindowTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(MainOnLoaded).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(MainOnLoaded sut)
    {
        sut.Should().BeAssignableTo<IOnLoaded>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(MainOnLoaded).GetMethods().Where(method => !method.IsAbstract));
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void DataContext_ForCurrentWindow_HasInstance(
        MainOnLoaded sut)
    {
        // Arrange

        // Act
        var result = sut.DataContext;

        // Assert
        result.Should().NotBeNull();
    }
}