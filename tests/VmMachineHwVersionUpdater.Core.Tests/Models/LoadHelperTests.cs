using System.Collections.Concurrent;
using System.Collections.ObjectModel;

namespace VmMachineHwVersionUpdater.Core.Tests.Models;

public class LoadHelperTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(LoadHelper).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(LoadHelper sut)
    {
        sut.Should().NotBeNull();
        sut.Should().BeOfType<LoadHelper>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(LoadHelper).GetMethods()
                                           .Where(method => !method.IsAbstract & !method.Name.StartsWith("set_")));
    }

    [Fact]
    public void SearchOsItems_CanBeSetAndRetrieved()
    {
        // Arrange
        var sut = new LoadHelper();
        var items = new ConcurrentDictionary<string, bool>();
        items.TryAdd("windows9-64", true);

        // Act
        sut.SearchOsItems = items;

        // Assert
        sut.SearchOsItems.Should().BeSameAs(items);
        sut.SearchOsItems.Should().ContainKey("windows9-64");
    }

    [Fact]
    public void UpdateAllHwVersion_CanBeSetAndRetrieved()
    {
        // Arrange
        var sut = new LoadHelper();

        // Act
        sut.UpdateAllHwVersion = 21.0;

        // Assert
        sut.UpdateAllHwVersion.Should().Be(21.0);
    }

    [Fact]
    public void UpdateAllTextBlocks_CanBeSetAndRetrieved()
    {
        // Arrange
        var sut = new LoadHelper();

        // Act
        sut.UpdateAllTextBlocks = "3 machines listed";

        // Assert
        sut.UpdateAllTextBlocks.Should().Be("3 machines listed");
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void VmDataGridItemsSource_CanBeSetAndRetrieved(
        Machine machine)
    {
        // Arrange
        var sut = new LoadHelper();
        var collection = new ObservableCollection<Machine> { machine };

        // Act
        sut.VmDataGridItemsSource = collection;

        // Assert
        sut.VmDataGridItemsSource.Should().HaveCount(1);
        sut.VmDataGridItemsSource.Should().Contain(machine);
    }
}