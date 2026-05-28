namespace VmMachineHwVersionUpdater.Core.Tests.PerMachine;

public class ReturnValueFromVmxLineTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ReturnValueFromVmxLine).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(ReturnValueFromVmxLine sut)
    {
        sut.Should().BeAssignableTo<IReturnValueFromVmxLine>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ReturnValueFromVmxLine).GetMethods().Where(method => !method.IsAbstract));
    }

    [Theory]
    [NSubstituteOmitAutoPropertiesTrueInlineAutoData("displayname = \"Windows 11 Dev\"", "displayname", "Windows 11 Dev")]
    [NSubstituteOmitAutoPropertiesTrueInlineAutoData("virtualhw.version = \"21\"", "virtualhw.version", "21")]
    [NSubstituteOmitAutoPropertiesTrueInlineAutoData("memsize = \"4096\"", "memsize", "4096")]
    [NSubstituteOmitAutoPropertiesTrueInlineAutoData("guestos = \"windows9-64\"", "guestos", "windows9-64")]
    [NSubstituteOmitAutoPropertiesTrueInlineAutoData("tools.syncTime = \"TRUE\"", "tools.syncTime", "TRUE")]
    [NSubstituteOmitAutoPropertiesTrueInlineAutoData("mks.enable3d = \"FALSE\"", "mks.enable3d", "FALSE")]
    public void ValueFor_WithValidLine_ReturnsExtractedValue(string line, string key, string expected, ReturnValueFromVmxLine sut)
    {
        // Act
        var result = sut.ValueFor(line, key);

        // Assert
        result.Should().Be(expected);
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void ValueFor_WithQuotedValueContainingSpaces_ReturnsFullValue(ReturnValueFromVmxLine sut)
    {
        // Act
        var result = sut.ValueFor("displayname = \"My Test VM\"", "displayname");

        // Assert
        result.Should().Be("My Test VM");
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void ValueFor_WithNullLine_ThrowsArgumentNullException(ReturnValueFromVmxLine sut)
    {
        // Act & Assert
        sut.Invoking(x => x.ValueFor(null!, "key"))
           .Should().Throw<ArgumentNullException>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void ValueFor_WithNullKey_ThrowsArgumentNullException(ReturnValueFromVmxLine sut)
    {
        // Act & Assert
        sut.Invoking(x => x.ValueFor("line", null!))
           .Should().Throw<ArgumentNullException>();
    }
}