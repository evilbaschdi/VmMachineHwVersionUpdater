namespace VmMachineHwVersionUpdater.Core.Tests.PerMachine;

public class LineStartActionsTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(LineStartActions).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(LineStartActions sut)
    {
        sut.Should().BeAssignableTo<ILineStartActions>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(LineStartActions).GetMethods().Where(method => !method.IsAbstract));
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Value_ReturnsActionDictionaryWithExpectedKeys(LineStartActions sut)
    {
        // Act
        var result = sut.Value;

        // Assert
        result.Should().NotBeNull();
        result.Should().ContainKeys(
            "virtualhw.version",
            "displayname",
            "tools.syncTime",
            "tools.upgrade.policy",
            "guestos",
            "guestOS.detailed.data",
            "guestInfo.detailed.data",
            "annotation",
            "encryption.encryptedKey",
            "encryption.keySafe",
            "encryption.data",
            "managedvm.autoAddVTPM",
            "memsize");
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Value_VirtualHwVersionAction_SetsHwVersionOnMachine(
        [Frozen] IReturnValueFromVmxLine returnValueFromVmxLine,
        LineStartActions sut,
        RawMachine machine,
        string line)
    {
        // Arrange
        const int expectedVersion = 19;
        returnValueFromVmxLine.ValueFor(line, "virtualhw.version").Returns(expectedVersion.ToString());

        // Act
        sut.Value["virtualhw.version"](machine, line);

        // Assert
        machine.HwVersion.Should().Be(expectedVersion);
        returnValueFromVmxLine.Received(1).ValueFor(line, "virtualhw.version");
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Value_DisplayNameAction_SetsDisplayNameOnMachine(
        [Frozen] IReturnValueFromVmxLine returnValueFromVmxLine,
        LineStartActions sut,
        RawMachine machine,
        string line,
        string expectedDisplayName)
    {
        // Arrange
        returnValueFromVmxLine.ValueFor(line, "displayname").Returns(expectedDisplayName);

        // Act
        sut.Value["displayname"](machine, line);

        // Assert
        machine.DisplayName.Should().Be(expectedDisplayName);
        returnValueFromVmxLine.Received(1).ValueFor(line, "displayname");
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Value_AnnotationAction_SetsAnnotationOnMachine(
        [Frozen] IReturnValueFromVmxLine returnValueFromVmxLine,
        [Frozen] IConvertAnnotationLineBreaks convertAnnotationLineBreaks,
        LineStartActions sut,
        RawMachine machine,
        string line,
        string rawAnnotation,
        string convertedAnnotation)
    {
        // Arrange
        returnValueFromVmxLine.ValueFor(line, "annotation").Returns(rawAnnotation);
        convertAnnotationLineBreaks.ValueFor(rawAnnotation).Returns(convertedAnnotation);

        // Act
        sut.Value["annotation"](machine, line);

        // Assert
        machine.Annotation.Should().Be(convertedAnnotation);
        returnValueFromVmxLine.Received(1).ValueFor(line, "annotation");
        convertAnnotationLineBreaks.Received(1).ValueFor(rawAnnotation);
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Value_MemSizeAction_SetsMemSizeOnMachine(
        [Frozen] IReturnValueFromVmxLine returnValueFromVmxLine,
        LineStartActions sut,
        RawMachine machine,
        string line)
    {
        // Arrange
        const int expectedMemSize = 4096;
        returnValueFromVmxLine.ValueFor(line, "memsize").Returns(expectedMemSize.ToString());

        // Act
        sut.Value["memsize"](machine, line);

        // Assert
        machine.MemSize.Should().Be(expectedMemSize);
        returnValueFromVmxLine.Received(1).ValueFor(line, "memsize");
    }
}