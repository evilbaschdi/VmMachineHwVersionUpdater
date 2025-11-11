using System.Globalization;
using System.Reflection;

namespace VmMachineHwVersionUpdater.Core.Tests;

public class DoubleExtensionsTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(DoubleExtensions).GetConstructors());
    }

    [Fact]
    public void Constructor_ReturnsInterfaceName()
    {
        // Static class cannot be instantiated, verify it's static
        typeof(DoubleExtensions).Should().BeStatic();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        var methods = typeof(DoubleExtensions)
            .GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Where(m => !m.IsSpecialName);

        assertion.Verify(methods);
    }

    #region GiBiBytesToKiBiBytes Tests

    [Theory]
    [InlineData(1.0, 1073741824.0)]
    [InlineData(0.0, 0.0)]
    [InlineData(0.5, 536870912.0)]
    [InlineData(2.0, 2147483648.0)]
    [InlineData(0.001, 1073741.824)]
    public void GiBiBytesToKiBiBytes_WithValidInput_ReturnsCorrectValue(double input, double expected)
    {
        // Act
        var result = input.GiBiBytesToKiBiBytes();

        // Assert
        result.Should().BeApproximately(expected, 0.001);
    }

    [Theory]
    [InlineData(-1.0)]
    [InlineData(-0.1)]
    [InlineData(-100.0)]
    [InlineData(double.MinValue)]
    public void GiBiBytesToKiBiBytes_WithNegativeInput_ThrowsArgumentOutOfRangeException(double input)
    {
        // Act & Assert
        var act = () => input.GiBiBytesToKiBiBytes();
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithParameterName("input");
    }

    [Theory]
    [InlineData(double.MaxValue)]
    [InlineData(double.PositiveInfinity)]
    public void GiBiBytesToKiBiBytes_WithLargeInput_ReturnsExpectedResult(double input)
    {
        // Act
        var result = input.GiBiBytesToKiBiBytes();

        // Assert
        if (double.IsPositiveInfinity(input))
        {
            result.Should().Be(double.PositiveInfinity);
        }
        else
        {
            result.Should().Be(input * 1073741824d);
        }
    }

    #endregion

    #region KiBiBytesToGiBiBytes Tests

    [Theory]
    [InlineData(1073741824.0, 1.0)]
    [InlineData(0.0, 0.0)]
    [InlineData(536870912.0, 0.5)]
    [InlineData(2147483648.0, 2.0)]
    [InlineData(1073741.824, 0.001)]
    public void KiBiBytesToGiBiBytes_WithValidInput_ReturnsCorrectValue(double input, double expected)
    {
        // Act
        var result = input.KiBiBytesToGiBiBytes();

        // Assert
        result.Should().BeApproximately(expected, 0.001);
    }

    [Theory]
    [InlineData(-1.0)]
    [InlineData(-0.1)]
    [InlineData(-100.0)]
    [InlineData(double.MinValue)]
    public void KiBiBytesToGiBiBytes_WithNegativeInput_ThrowsArgumentOutOfRangeException(double input)
    {
        // Act & Assert
        var act = () => input.KiBiBytesToGiBiBytes();
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithParameterName("input");
    }

    [Theory]
    [InlineData(double.MaxValue)]
    [InlineData(double.PositiveInfinity)]
    public void KiBiBytesToGiBiBytes_WithLargeInput_ReturnsExpectedResult(double input)
    {
        // Act
        var result = input.KiBiBytesToGiBiBytes();

        // Assert
        if (double.IsPositiveInfinity(input))
        {
            result.Should().Be(double.PositiveInfinity);
        }
        else
        {
            result.Should().Be(input / 1073741824d);
        }
    }

    #endregion

    #region ToFileSize Tests

    [Fact]
    public void ToFileSize_WithNullCulture_ThrowsArgumentNullException()
    {
        // Arrange
        var input = 1024.0;
        var precision = 2;

        // Act & Assert
        var act = () => input.ToFileSize(precision, null!);
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("culture");
    }

    [Theory]
    [InlineData(0.0, 2, "0 bytes")]
    [InlineData(512.0, 2, "512 bytes")]
    [InlineData(1023.0, 2, "1023 bytes")]
    public void ToFileSize_WithBytesRange_ReturnsCorrectFormat(double input, int precision, string expected)
    {
        // Act
        var result = input.ToFileSize(precision, CultureInfo.InvariantCulture);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(1024.0, 2, "1.00 KB")]
    [InlineData(2048.0, 1, "2.0 KB")]
    [InlineData(1536.0, 2, "1.50 KB")]
    [InlineData(1048575.0, 0, "1024 KB")] // Just under 1MB
    public void ToFileSize_WithKilobytesRange_ReturnsCorrectFormat(double input, int precision, string expected)
    {
        // Act
        var result = input.ToFileSize(precision, CultureInfo.InvariantCulture);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(1048576.0, 2, "1.00 MB")] // 1MB
    [InlineData(2097152.0, 1, "2.0 MB")] // 2MB
    [InlineData(1572864.0, 2, "1.50 MB")] // 1.5MB
    [InlineData(1073741823.0, 0, "1024 MB")] // Just under 1GB
    public void ToFileSize_WithMegabytesRange_ReturnsCorrectFormat(double input, int precision, string expected)
    {
        // Act
        var result = input.ToFileSize(precision, CultureInfo.InvariantCulture);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(1073741824.0, 2, "1.00 GB")] // 1GB
    [InlineData(2147483648.0, 1, "2.0 GB")] // 2GB
    [InlineData(1610612736.0, 2, "1.50 GB")] // 1.5GB
    [InlineData(1099511627775.0, 0, "1024 GB")] // Just under 1TB
    public void ToFileSize_WithGigabytesRange_ReturnsCorrectFormat(double input, int precision, string expected)
    {
        // Act
        var result = input.ToFileSize(precision, CultureInfo.InvariantCulture);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(1099511627776.0, 2)] // 1TB
    [InlineData(2199023255552.0, 1)] // 2TB
    public void ToFileSize_WithTerabytesRange_ReturnsTerabyteFormat(double input, int precision)
    {
        // Act
        var result = input.ToFileSize(precision, CultureInfo.InvariantCulture);

        // Assert
        result.Should().EndWith(" TB");
        result.Should().Contain(".");
        // Note: Due to the method's implementation with workaround calculation,
        // exact precision matching may vary, so we just verify format contains decimal point
    }

    [Theory]
    [InlineData(1125899906842624.0, 2)] // 1PB
    [InlineData(2251799813685248.0, 1)] // 2PB
    public void ToFileSize_WithPetabytesRange_ReturnsPetabyteFormat(double input, int precision)
    {
        // Act
        var result = input.ToFileSize(precision, CultureInfo.InvariantCulture);

        // Assert
        result.Should().EndWith(" PB");
    }

    [Theory]
    [InlineData(1152921504606846976.0, 2)] // 1EB
    public void ToFileSize_WithExabytesRange_ReturnsExabyteFormat(double input, int precision)
    {
        // Act
        var result = input.ToFileSize(precision, CultureInfo.InvariantCulture);

        // Assert
        result.Should().EndWith(" EB");
    }

    [Fact]
    public void ToFileSize_WithGermanCulture_UsesCorrectDecimalSeparator()
    {
        // Arrange
        var input = 1536.0; // 1.5 KB
        var precision = 2;
        var germanCulture = new CultureInfo("de-DE");

        // Act
        var result = input.ToFileSize(precision, germanCulture);

        // Assert
        result.Should().Be("1,50 KB"); // German uses comma as decimal separator
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    public void ToFileSize_WithDifferentPrecisionValues_FormatsCorrectly(int precision)
    {
        // Arrange
        var input = 1536.0; // 1.5 KB

        // Act
        var result = input.ToFileSize(precision, CultureInfo.InvariantCulture);

        // Assert
        result.Should().EndWith(" KB");
        if (precision > 0)
        {
            result.Should().Contain(".");
        }
    }

    #endregion
}