namespace VmMachineHwVersionUpdater.Core.Tests.PerMachine;

public class ChangeDisplayNameTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ChangeDisplayName).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(ChangeDisplayName sut)
    {
        sut.Should().BeAssignableTo<IChangeDisplayName>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ChangeDisplayName).GetMethods().Where(method => !method.IsAbstract));
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void RunFor_WithExistingDisplayName_UpdatesDisplayName(ChangeDisplayName sut)
    {
        // Arrange
        var tempFile = Path.GetTempFileName();
        var vmxContent = "displayName = \"Old Name\"\nvirtualhw.version = \"19\"";

        try
        {
            File.WriteAllText(tempFile, vmxContent);

            // Act
            sut.RunFor(tempFile, "New VM Name");

            // Assert
            var result = File.ReadAllText(tempFile);
            result.Should().Contain("displayName = \"New VM Name\"");
            result.Should().NotContain("Old Name");
        }
        finally
        {
            File.Delete(tempFile);
        }
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void RunFor_WithMissingDisplayName_AddsDisplayNameLine(ChangeDisplayName sut)
    {
        // Arrange
        var tempFile = Path.GetTempFileName();
        var vmxContent = "virtualhw.version = \"19\"";

        try
        {
            File.WriteAllText(tempFile, vmxContent);

            // Act
            sut.RunFor(tempFile, "Added Name");

            // Assert
            var result = File.ReadAllText(tempFile);
            result.Should().Contain("displayName = \"Added Name\"");
        }
        finally
        {
            File.Delete(tempFile);
        }
    }
}