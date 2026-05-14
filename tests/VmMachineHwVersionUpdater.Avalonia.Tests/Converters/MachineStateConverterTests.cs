using System.ComponentModel;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;
using FluentAvalonia.UI.Controls;
using VmMachineHwVersionUpdater.Avalonia.Converters;

namespace VmMachineHwVersionUpdater.Avalonia.Tests.Converters;

public class MachineStateConverterTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(MachineStateConverter).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(MachineStateConverter sut)
    {
        sut.Should().BeAssignableTo<IValueConverter>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(MachineStateConverter).GetMethods().Where(method => !method.IsAbstract & !method.Name.StartsWith("Convert")));
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Convert_ValueNotOfTypeMachineState_ReturnsBindingNotification(
        MachineStateConverter sut,
        string dummyValue,
        Type dummyType,
        CultureInfo dummyCultureInfo)
    {
        // Arrange

        // Act
        var result = sut.Convert(dummyValue, dummyType, null, dummyCultureInfo);

        // Assert
        result.Should().BeOfType<BindingNotification>();
        result.As<BindingNotification>().Error.Should().BeOfType<InvalidCastException>();
        result.As<BindingNotification>().ErrorType.Should().Be(BindingErrorType.Error);
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Convert_ValueNotValidMachineState_ReturnsBindingNotification(
        MachineStateConverter sut,
        Type dummyType,
        CultureInfo dummyCultureInfo)
    {
        // Arrange
        var dummyInt = -13;
        var dummyValue = (MachineState)dummyInt;

        // Act
        var result = sut.Convert(dummyValue, dummyType, null, dummyCultureInfo);

        // Assert
        result.Should().BeOfType<BindingNotification>();
        result.As<BindingNotification>().Error.Should().BeOfType<InvalidEnumArgumentException>();
        result.As<BindingNotification>().ErrorType.Should().Be(BindingErrorType.Error);
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Convert_ValueMachineStateOff_ReturnsFASymbolPlayFilled(
        MachineStateConverter sut,
        Type dummyType,
        CultureInfo dummyCultureInfo)
    {
        // Arrange
        // Act
        var result = sut.Convert(MachineState.Off, dummyType, null, dummyCultureInfo);

        // Assert
        result.Should().BeOfType<FASymbol>();
        result.As<FASymbol>().Should().Be(FASymbol.PlayFilled);
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Convert_ValueMachineStatePaused_ReturnsFASymbolPauseFilled(
        MachineStateConverter sut,
        Type dummyType,
        CultureInfo dummyCultureInfo)
    {
        // Arrange
        // Act
        var result = sut.Convert(MachineState.Paused, dummyType, null, dummyCultureInfo);

        // Assert
        result.Should().BeOfType<FASymbol>();
        result.As<FASymbol>().Should().Be(FASymbol.PauseFilled);
    }
}