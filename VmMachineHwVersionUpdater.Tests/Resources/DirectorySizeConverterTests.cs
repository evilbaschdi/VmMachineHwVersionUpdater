using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;
using VmMachineHwVersionUpdater.Resources;

namespace VmMachineHwVersionUpdater.Tests.Resources;

public class DirectorySizeConverterTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(DirectorySizeConverter).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(DirectorySizeConverter sut)
    {
        sut.Should().BeAssignableTo<IValueConverter>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(DirectorySizeConverter).GetMethods().Where(method => !method.IsAbstract));
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Convert_ForProvidedDirectorySize_ReturnsConvertedDirectorySize(
        DirectorySizeConverter sut,
        Machine machine0,
        Machine machine1
    )
    {
        // Arrange
        machine0.DirectorySizeGb = 13.13d;
        machine1.DirectorySizeGb = 42.42d;

        var collection = new ReadOnlyObservableCollection<object>(new()
                                                                  { machine0, machine1 });

        // Act
        var result = sut.Convert(collection, typeof(string), "4", CultureInfo.GetCultureInfo(1033));

        // Assert
        result.Should().Be("55.5498 GB");
    }
}