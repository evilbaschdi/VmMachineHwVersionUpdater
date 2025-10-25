using System.Collections.Concurrent;

namespace VmMachineHwVersionUpdater.Core.Tests.BasicApplication;

public class LoadSearchOsItemsTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(LoadSearchOsItems).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(LoadSearchOsItems sut)
    {
        sut.Should().BeAssignableTo<ILoadSearchOsItems>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(LoadSearchOsItems).GetMethods().Where(method => !method.IsAbstract));
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Value_ReturnsObservableCollectionWithExpectedStructure(
        [Frozen] ILoad load,
        [Frozen] IGuestOsesInUse guestOsesInUse,
        [Frozen] ISeparator separator,
        LoadSearchOsItems sut)
    {
        // Arrange
        var separatorValue = new object();
        var searchOsItems = new ConcurrentDictionary<string, bool>
        {
            ["Windows 11"] = true,
            ["Ubuntu 22.04"] = true
        };
        var guestOsItems = new ConcurrentDictionary<string, bool>
        {
            ["CentOS 7"] = true,
            ["Debian 11"] = true
        };

        var loadHelper = new LoadHelper
        {
            SearchOsItems = searchOsItems
        };
        load.Value.Returns(loadHelper);
        guestOsesInUse.Value.Returns(guestOsItems);
        separator.Value.Returns(separatorValue);

        // Act
        var result = sut.Value;

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCountGreaterThan(6); // empty string + 2 separators + at least 4 OS items
        result[0].Should().Be(string.Empty);
        result[1].Should().Be(separatorValue);
        result.Should().Contain("Windows 11");
        result.Should().Contain("Ubuntu 22.04");
        result.Should().Contain("CentOS 7");
        result.Should().Contain("Debian 11");
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Value_WithEmptySearchOsItems_ReturnsOnlyGuestOsItems(
        [Frozen] ILoad load,
        [Frozen] IGuestOsesInUse guestOsesInUse,
        [Frozen] ISeparator separator,
        LoadSearchOsItems sut)
    {
        // Arrange
        var separatorValue = new object();
        var searchOsItems = new ConcurrentDictionary<string, bool>();
        var guestOsItems = new ConcurrentDictionary<string, bool>
        {
            ["CentOS 7"] = true
        };

        var loadHelper = new LoadHelper
        {
            SearchOsItems = searchOsItems
        };
        load.Value.Returns(loadHelper);
        guestOsesInUse.Value.Returns(guestOsItems);
        separator.Value.Returns(separatorValue);

        // Act
        var result = sut.Value;

        // Assert
        result.Should().NotBeNull();
        result[0].Should().Be(string.Empty);
        result[1].Should().Be(separatorValue);
        result[2].Should().Be(separatorValue);
        result.Should().Contain("CentOS 7");
    }
}