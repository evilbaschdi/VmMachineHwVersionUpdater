namespace VmMachineHwVersionUpdater.Core.Tests.PerMachine;

public class VmxLineStartsWithTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(VmxLineStartsWith).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(VmxLineStartsWith sut)
    {
        sut.Should().BeAssignableTo<IVmxLineStartsWith>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(VmxLineStartsWith).GetMethods().Where(method => !method.IsAbstract));
    }

    [Theory]
    [NSubstituteOmitAutoPropertiesTrueInlineAutoData("displayname = \"Test\"", "displayname", true)]
    [NSubstituteOmitAutoPropertiesTrueInlineAutoData("virtualhw.version = \"21\"", "virtualhw.version", true)]
    [NSubstituteOmitAutoPropertiesTrueInlineAutoData("memsize = \"4096\"", "memsize", true)]
    [NSubstituteOmitAutoPropertiesTrueInlineAutoData("displayname = \"Test\"", "memsize", false)]
    [NSubstituteOmitAutoPropertiesTrueInlineAutoData("DISPLAYNAME = \"Test\"", "displayname", true)]
    [NSubstituteOmitAutoPropertiesTrueInlineAutoData("config.version = \"8\"", "virtualhw.version", false)]
    [NSubstituteOmitAutoPropertiesTrueInlineAutoData("", "displayname", false)]
    public void ValueFor_WithVariousInputs_ReturnsExpected(string line, string key, bool expected, VmxLineStartsWith sut)
    {
        // Act
        var result = sut.ValueFor(line, key);

        // Assert
        result.Should().Be(expected);
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void ValueFor_WithNullLine_ThrowsArgumentNullException(VmxLineStartsWith sut)
    {
        // Act & Assert
        sut.Invoking(x => x.ValueFor(null!, "key"))
           .Should().Throw<ArgumentNullException>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void ValueFor_WithNullKey_ThrowsArgumentNullException(VmxLineStartsWith sut)
    {
        // Act & Assert
        sut.Invoking(x => x.ValueFor("line", null!))
           .Should().Throw<ArgumentNullException>();
    }
}