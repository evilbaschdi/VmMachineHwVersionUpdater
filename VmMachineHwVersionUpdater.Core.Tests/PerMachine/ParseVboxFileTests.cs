namespace VmMachineHwVersionUpdater.Core.Tests.PerMachine;

public class ParseVboxFileTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ParseVboxFile).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(ParseVboxFile sut)
    {
        sut.Should().BeAssignableTo<IParseVboxFile>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ParseVboxFile).GetMethods().Where(method => !method.IsAbstract));
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void ValueFor_WithNullFile_ThrowsArgumentNullException(ParseVboxFile sut)
    {
        // Act & Assert
        sut.Invoking(x => x.ValueFor(null!))
            .Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ValueFor_WithValidVboxFile_ParsesCorrectly()
    {
        // Arrange
        var sut = new ParseVboxFile();
        var tempFile = Path.GetTempFileName();
        var vboxContent = """
                          <?xml version="1.0"?>
                          <VirtualBox xmlns="http://www.virtualbox.org/" version="1.16-windows">
                            <Machine uuid="{test-uuid}" name="TestVM" OSType="Windows_11_64" version="1">
                              <Description>Test VM Description</Description>
                              <Hardware>
                                <Memory RAMSize="2048"/>
                                <Platform>
                                  <CPU count="2"/>
                                </Platform>
                              </Hardware>
                            </Machine>
                          </VirtualBox>
                          """;

        try
        {
            File.WriteAllText(tempFile, vboxContent);

            // Act
            var result = sut.ValueFor(tempFile);

            // Assert
            result.Should().NotBeNull();
            result.MachineType.Should().Be(MachineType.Vbox);
            result.DisplayName.Should().Be("TestVM");
            result.OSType.Should().Be("Windows_11_64");
            result.VirtualBoxHwVersion.Should().Be(1);
            result.Annotation.Should().Be("Test VM Description");
            result.MemSize.Should().Be(2048);
            result.CpuCount.Should().Be(2);
        }
        finally
        {
            File.Delete(tempFile);
        }
    }

    [Fact]
    public void ValueFor_WithMinimalVboxFile_ReturnsBasicRawMachine()
    {
        // Arrange
        var sut = new ParseVboxFile();
        var tempFile = Path.GetTempFileName();
        var vboxContent = """
                          <?xml version="1.0"?>
                          <VirtualBox xmlns="http://www.virtualbox.org/" version="1.16-windows">
                          </VirtualBox>
                          """;

        try
        {
            File.WriteAllText(tempFile, vboxContent);

            // Act
            var result = sut.ValueFor(tempFile);

            // Assert
            result.Should().NotBeNull();
            result.MachineType.Should().Be(MachineType.Vbox);
            result.DisplayName.Should().BeEmpty();
            result.OSType.Should().BeEmpty();
            result.VirtualBoxHwVersion.Should().Be(0);
            result.MemSize.Should().Be(0);
            result.CpuCount.Should().Be(0);
        }
        finally
        {
            File.Delete(tempFile);
        }
    }
}