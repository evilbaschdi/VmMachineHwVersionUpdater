namespace VmMachineHwVersionUpdater.Core.Tests.PerMachine;

public class UpdateMachineMemSizeTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(UpdateMachineMemSize).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(UpdateMachineMemSize sut)
    {
        sut.Should().BeAssignableTo<IUpdateMachineMemSize>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(UpdateMachineMemSize).GetMethods().Where(method => !method.IsAbstract));
    }

    [Fact]
    public void Constructor_InitializesWithCorrectKey()
    {
        // Act
        var sut = new UpdateMachineMemSize();

        // Assert
        sut.Should().NotBeNull();
        sut.Should().BeAssignableTo<UpsertVmxLine<int>>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void RunFor_WithNullPath_ThrowsArgumentNullException(
        UpdateMachineMemSize sut,
        int memSize)
    {
        // Act & Assert
        sut.Invoking(x => x.RunFor(null!, memSize))
            .Should().Throw<ArgumentNullException>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void RunFor_WithValidMemSize_ProcessesCorrectly(
        UpdateMachineMemSize sut,
        int memSize)
    {
        // Arrange
        var tempFile = Path.GetTempFileName();
        var vmxContent = "displayName = \"Test VM\"\nvirtualhw.version = \"19\"";

        try
        {
            File.WriteAllText(tempFile, vmxContent);

            // Act
            sut.RunFor(tempFile, memSize);

            // Assert
            var result = File.ReadAllText(tempFile);
            result.Should().Contain($"memsize = \"{memSize}\"");
        }
        finally
        {
            if (File.Exists(tempFile))
            {
                File.Delete(tempFile);
            }
        }
    }
}