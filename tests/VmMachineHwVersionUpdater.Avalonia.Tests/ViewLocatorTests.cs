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

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Match_WithViewModelBase_ReturnsTrue(ViewLocator sut, ViewModelBase dummyData)
    {
        // Act
        var result = sut.Match(dummyData);

        // Assert
        result.Should().Be(true);
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Match_WithNonViewModelBase_ReturnsFalse(ViewLocator sut, object dummyData)
    {
        // Act
        var result = sut.Match(dummyData);

        // Assert
        result.Should().Be(false);
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Build_WithUnresolvableType_ReturnsTextBlock(ViewLocator sut, ViewModelBase dummyData)
    {
        // Act
        var result = sut.Build(dummyData);

        // Assert
        result.Should().BeOfType<TextBlock>();
    }
}