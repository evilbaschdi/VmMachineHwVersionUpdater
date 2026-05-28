namespace VmMachineHwVersionUpdater.Core.Tests.PerMachine;

public class ReadLogInformationTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ReadLogInformation).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(ReadLogInformation sut)
    {
        sut.Should().BeAssignableTo<IReadLogInformation>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ReadLogInformation).GetMethods().Where(method => !method.IsAbstract));
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void ValueFor_WithNonExistentDirectory_ReturnsEmptyStrings(
        ReadLogInformation sut)
    {
        // Act
        var (logLastDate, logLastDateDiff) = sut.ValueFor(@"C:\NonExistent\Directory");

        // Assert
        logLastDate.Should().BeEmpty();
        logLastDateDiff.Should().BeEmpty();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void ValueFor_WithNullLogDirectory_ThrowsArgumentNullException(
        ReadLogInformation sut)
    {
        // Act & Assert
        var act = () => sut.ValueFor(null!);
        act.Should().Throw<ArgumentNullException>()
           .WithParameterName("logDirectory");
    }
}