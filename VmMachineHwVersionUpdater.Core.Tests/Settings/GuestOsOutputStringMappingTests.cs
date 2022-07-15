using System.IO;
using Microsoft.Extensions.Configuration;
using VmMachineHwVersionUpdater.Core.Settings;

namespace VmMachineHwVersionUpdater.Core.Tests.Settings;

public class GuestOsOutputStringMappingTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(GuestOsOutputStringMapping).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(GuestOsOutputStringMapping sut)
    {
        sut.Should().BeAssignableTo<IGuestOsOutputStringMapping>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(GuestOsOutputStringMapping).GetMethods().Where(method => !method.IsAbstract));
    }

    [Theory]
    [NSubstituteOmitAutoPropertiesTrueAutoData]
    public void ValueFor_Windows9srv64_ReturnsWindowsServer2016OrLaterX64(
        [Frozen] IGuestOsStringMapping guestOsStringMapping,
        GuestOsOutputStringMapping sut)
    {
        // Arrange
        guestOsStringMapping.Value.Returns(new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("Settings\\GuestOsStringMapping.json").Build());

        // Act
        var result = sut.ValueFor("windows9srv-64");

        // Assert

        result.Should().Be("Windows Server 2016");
    }

    [Theory]
    [NSubstituteOmitAutoPropertiesTrueAutoData]
    public void ValueFor_Windows964_ReturnsWindows10X64(
        [Frozen] IGuestOsStringMapping guestOsStringMapping,
        GuestOsOutputStringMapping sut)
    {
        // Arrange
        guestOsStringMapping.Value.Returns(new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("Settings\\GuestOsStringMapping.json").Build());

        // Act
        var result = sut.ValueFor("windows9-64");

        // Assert

        result.Should().Be("Windows 10 and later x64");
    }
}