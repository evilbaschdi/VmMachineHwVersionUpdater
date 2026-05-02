using Avalonia.Controls.Templates;
using VmMachineHwVersionUpdater.Avalonia.ViewModels;

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

    [Fact]
    public void Match_WithViewModelBase_ReturnsTrue()
    {
        // Arrange
        var sut = new ViewLocator();
        var dummyData = Substitute.For<ViewModelBase>();

        // Act
        var result = sut.Match(dummyData);

        // Assert
        result.Should().Be(true);
    }

    [Fact]
    public void Match_WithNonViewModelBase_ReturnsFalse()
    {
        // Arrange
        var sut = new ViewLocator();
        var dummyData = new object();

        // Act
        var result = sut.Match(dummyData);

        // Assert
        result.Should().Be(false);
    }

    [Fact]
    public void Build_WithUnresolvableType_ReturnsTextBlock()
    {
        // Arrange
        var sut = new ViewLocator();
        var dummyData = Substitute.For<ViewModelBase>();

        // Act
        var result = sut.Build(dummyData);

        // Assert
        result.Should().BeOfType<TextBlock>();
    }
}