using System.Linq;
using AutoFixture.Idioms;
using EvilBaschdi.Testing;
using FluentAssertions;
using VmMachineHwVersionUpdater.Core.Settings;
using Xunit;

namespace VmMachineHwVersionUpdater.Core.Tests.Settings;

public class GuestOsStringMappingTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(GuestOsStringMapping).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(GuestOsStringMapping sut)
    {
        sut.Should().BeAssignableTo<IGuestOsStringMapping>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(GuestOsStringMapping).GetMethods().Where(method => !method.IsAbstract));
    }

    //[Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    //public void Value_ForProvidedPath_ReturnsAppSettings(
    //    GuestOsStringMapping sut)
    //{
    //    // Arrange

    //    // Act
    //    var result = sut.Value;

    //    // Assert
    //}
}