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

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void ValueFor_WithEmptySearchTexts_ReturnsTrue(
        FilterItemSource sut,
        Machine machine)
    {
        // Arrange
        var value = (machine, string.Empty, string.Empty);

        // Act
        var result = sut.ValueFor(value);

        // Assert
        result.Should().BeTrue();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void ValueFor_WithNoFilterSearchOs_ReturnsTrue(
        FilterItemSource sut,
        Machine machine,
        string searchFilterText)
    {
        // Arrange
        machine.DisplayName = searchFilterText;
        var value = (machine, "(no filter)", searchFilterText);

        // Act
        var result = sut.ValueFor(value);

        // Assert
        result.Should().BeTrue();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void ValueFor_WithMatchingGuestOs_ReturnsTrue(
        FilterItemSource sut,
        Machine machine,
        string guestOs)
    {
        // Arrange
        machine.GuestOs = guestOs;
        var value = (machine, guestOs[..Math.Min(3, guestOs.Length)], string.Empty);

        // Act
        var result = sut.ValueFor(value);

        // Assert
        result.Should().BeTrue();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void ValueFor_WithNonMatchingGuestOs_ReturnsFalse(
        FilterItemSource sut,
        Machine machine)
    {
        // Arrange
        machine.GuestOs = "windows11-64";
        var value = (machine, "ubuntu", string.Empty);

        // Act
        var result = sut.ValueFor(value);

        // Assert
        result.Should().BeFalse();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void ValueFor_WithMatchingDisplayName_ReturnsTrue(
        FilterItemSource sut,
        Machine machine,
        string displayName)
    {
        // Arrange
        machine.DisplayName = displayName;
        var searchText = displayName[..Math.Min(3, displayName.Length)];
        var value = (machine, string.Empty, searchText);

        // Act
        var result = sut.ValueFor(value);

        // Assert
        result.Should().BeTrue();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void ValueFor_WithMatchingAnnotation_ReturnsTrue(
        FilterItemSource sut,
        Machine machine,
        string annotation)
    {
        // Arrange
        machine.DisplayName = string.Empty;
        machine.Annotation = annotation;
        var searchText = annotation[..Math.Min(3, annotation.Length)];
        var value = (machine, string.Empty, searchText);

        // Act
        var result = sut.ValueFor(value);

        // Assert
        result.Should().BeTrue();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void ValueFor_WithWildcardSearch_ReturnsTrue(
        FilterItemSource sut,
        Machine machine)
    {
        // Arrange
        machine.DisplayName = "Test Virtual Machine";
        var value = (machine, string.Empty, "*TVM*");

        // Act
        var result = sut.ValueFor(value);

        // Assert
        result.Should().BeTrue();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void ValueFor_WithNonMatchingSearch_ReturnsFalse(
        FilterItemSource sut,
        Machine machine)
    {
        // Arrange
        machine.DisplayName = "Windows VM";
        machine.Annotation = "Test machine";
        var value = (machine, string.Empty, "NonExistent");

        // Act
        var result = sut.ValueFor(value);

        // Assert
        result.Should().BeFalse();
    }
}