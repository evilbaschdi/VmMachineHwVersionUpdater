namespace VmMachineHwVersionUpdater.Core.Tests.BasicApplication;

public class FilterItemSourceTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(FilterItemSource).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(FilterItemSource sut)
    {
        sut.Should().BeAssignableTo<IFilterItemSource>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(FilterItemSource).GetMethods().Where(method => !method.IsAbstract));
    }

    private Machine CreateTestMachine(string displayName = "", string annotation = "", string guestOs = "")
    {
        var toggleToolsSyncTime = Substitute.For<IToggleToolsSyncTime>();
        var toggleToolsUpgradePolicy = Substitute.For<IToggleToolsUpgradePolicy>();
        var updateMachineVersion = Substitute.For<IUpdateMachineVersion>();
        var updateMachineMemSize = Substitute.For<IUpdateMachineMemSize>();

        return new Machine(toggleToolsSyncTime, toggleToolsUpgradePolicy, updateMachineVersion, updateMachineMemSize)
        {
            DisplayName = displayName,
            Annotation = annotation,
            GuestOs = guestOs
        };
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void ValueFor_WithEmptySearchTexts_ReturnsTrue(
        FilterItemSource sut)
    {
        // Arrange
        var machine = CreateTestMachine();
        var value = (machine, string.Empty, string.Empty);

        // Act
        var result = sut.ValueFor(value);

        // Assert
        result.Should().BeTrue();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void ValueFor_WithNoFilterSearchOs_ReturnsTrue(
        FilterItemSource sut,
        string searchFilterText)
    {
        // Arrange
        var machine = CreateTestMachine(displayName: searchFilterText);
        var value = (machine, "(no filter)", searchFilterText);

        // Act
        var result = sut.ValueFor(value);

        // Assert
        result.Should().BeTrue();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void ValueFor_WithMatchingGuestOs_ReturnsTrue(
        FilterItemSource sut,
        string guestOs)
    {
        // Arrange
        var machine = CreateTestMachine(guestOs: guestOs);
        var value = (machine, guestOs.Substring(0, Math.Min(3, guestOs.Length)), string.Empty);

        // Act
        var result = sut.ValueFor(value);

        // Assert
        result.Should().BeTrue();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void ValueFor_WithNonMatchingGuestOs_ReturnsFalse(
        FilterItemSource sut)
    {
        // Arrange
        var machine = CreateTestMachine(guestOs: "windows11-64");
        var value = (machine, "ubuntu", string.Empty);

        // Act
        var result = sut.ValueFor(value);

        // Assert
        result.Should().BeFalse();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void ValueFor_WithMatchingDisplayName_ReturnsTrue(
        FilterItemSource sut,
        string displayName)
    {
        // Arrange
        var machine = CreateTestMachine(displayName: displayName);
        var searchText = displayName.Substring(0, Math.Min(3, displayName.Length));
        var value = (machine, string.Empty, searchText);

        // Act
        var result = sut.ValueFor(value);

        // Assert
        result.Should().BeTrue();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void ValueFor_WithMatchingAnnotation_ReturnsTrue(
        FilterItemSource sut,
        string annotation)
    {
        // Arrange
        var machine = CreateTestMachine(annotation: annotation);
        var searchText = annotation.Substring(0, Math.Min(3, annotation.Length));
        var value = (machine, string.Empty, searchText);

        // Act
        var result = sut.ValueFor(value);

        // Assert
        result.Should().BeTrue();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void ValueFor_WithWildcardSearch_ReturnsTrue(
        FilterItemSource sut)
    {
        // Arrange
        var machine = CreateTestMachine(displayName: "Test Virtual Machine");
        var value = (machine, string.Empty, "*TVM*");

        // Act
        var result = sut.ValueFor(value);

        // Assert
        result.Should().BeTrue();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void ValueFor_WithNonMatchingSearch_ReturnsFalse(
        FilterItemSource sut)
    {
        // Arrange
        var machine = CreateTestMachine(displayName: "Windows VM", annotation: "Test machine");
        var value = (machine, string.Empty, "NonExistent");

        // Act
        var result = sut.ValueFor(value);

        // Assert
        result.Should().BeFalse();
    }
}