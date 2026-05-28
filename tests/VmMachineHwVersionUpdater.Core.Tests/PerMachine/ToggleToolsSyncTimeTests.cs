namespace VmMachineHwVersionUpdater.Core.Tests.PerMachine;

public class ToggleToolsSyncTimeTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ToggleToolsSyncTime).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(ToggleToolsSyncTime sut)
    {
        sut.Should().BeAssignableTo<IToggleToolsSyncTime>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ToggleToolsSyncTime).GetMethods().Where(method => !method.IsAbstract));
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void RunFor_WithTrue_SetsToolsSyncTimeToTrue(ToggleToolsSyncTime sut)
    {
        // Arrange
        var tempFile = Path.GetTempFileName();
        var vmxContent = "displayName = \"Test\"\ntools.syncTime = \"FALSE\"";

        try
        {
            File.WriteAllText(tempFile, vmxContent);

            // Act
            sut.RunFor(tempFile, true);

            // Assert
            var result = File.ReadAllText(tempFile);
            result.Should().Contain("tools.syncTime = \"TRUE\"");
        }
        finally
        {
            File.Delete(tempFile);
        }
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void RunFor_WithFalse_SetsToolsSyncTimeToFalse(ToggleToolsSyncTime sut)
    {
        // Arrange
        var tempFile = Path.GetTempFileName();
        var vmxContent = "displayName = \"Test\"\ntools.syncTime = \"TRUE\"";

        try
        {
            File.WriteAllText(tempFile, vmxContent);

            // Act
            sut.RunFor(tempFile, false);

            // Assert
            var result = File.ReadAllText(tempFile);
            result.Should().Contain("tools.syncTime = \"FALSE\"");
        }
        finally
        {
            File.Delete(tempFile);
        }
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void RunFor_WithMissingLine_AddsToolsSyncTimeLine(ToggleToolsSyncTime sut)
    {
        // Arrange
        var tempFile = Path.GetTempFileName();
        var vmxContent = "displayName = \"Test\"";

        try
        {
            File.WriteAllText(tempFile, vmxContent);

            // Act
            sut.RunFor(tempFile, true);

            // Assert
            var result = File.ReadAllText(tempFile);
            result.Should().Contain("tools.syncTime = \"TRUE\"");
        }
        finally
        {
            File.Delete(tempFile);
        }
    }
}