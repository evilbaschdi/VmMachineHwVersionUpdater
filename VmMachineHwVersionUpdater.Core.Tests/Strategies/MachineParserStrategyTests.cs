namespace VmMachineHwVersionUpdater.Core.Tests.Strategies;

public class MachineParserStrategyTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(MachineParserStrategy).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(MachineParserStrategy sut)
    {
        sut.Should().BeAssignableTo<IMachineParserStrategy>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(MachineParserStrategy).GetMethods().Where(method => !method.IsAbstract));
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Parse_WithVmxExtension_CallsParseVmxFile(
        [Frozen] IParseVmxFile parseVmxFile,
        MachineParserStrategy sut,
        string fileName,
        RawMachine expectedResult)
    {
        // Arrange
        var filePath = $"{fileName}.vmx";
        parseVmxFile.ValueFor(filePath).Returns(expectedResult);

        // Act
        var result = sut.Parse(filePath);

        // Assert
        result.Should().Be(expectedResult);
        parseVmxFile.Received(1).ValueFor(filePath);
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Parse_WithVboxExtension_CallsParseVboxFile(
        [Frozen] IParseVboxFile parseVboxFile,
        MachineParserStrategy sut,
        string fileName,
        RawMachine expectedResult)
    {
        // Arrange
        var filePath = $"{fileName}.vbox";
        parseVboxFile.ValueFor(filePath).Returns(expectedResult);

        // Act
        var result = sut.Parse(filePath);

        // Assert
        result.Should().Be(expectedResult);
        parseVboxFile.Received(1).ValueFor(filePath);
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Parse_WithUnsupportedExtension_ThrowsNotSupportedException(
        MachineParserStrategy sut,
        string fileName)
    {
        // Arrange
        var filePath = $"{fileName}.unsupported";

        // Act & Assert
        sut.Invoking(x => x.Parse(filePath))
            .Should().Throw<NotSupportedException>()
            .WithMessage("File extension '.unsupported' is not supported.");
    }
}