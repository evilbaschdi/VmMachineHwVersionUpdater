namespace VmMachineHwVersionUpdater.Core.Tests.PerMachine;

public class AddEditAnnotationTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(AddEditAnnotation).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(AddEditAnnotation sut)
    {
        sut.Should().BeAssignableTo<IAddEditAnnotation>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(AddEditAnnotation).GetMethods().Where(method => !method.IsAbstract));
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void RunFor_WithExistingAnnotation_UpdatesAnnotation(AddEditAnnotation sut)
    {
        // Arrange
        var tempFile = Path.GetTempFileName();
        var vmxContent = "displayName = \"Test\"\nannotation = \"Old annotation\"";

        try
        {
            File.WriteAllText(tempFile, vmxContent);

            // Act
            sut.RunFor(tempFile, "New annotation");

            // Assert
            var result = File.ReadAllText(tempFile);
            result.Should().Contain("annotation = \"New annotation\"");
            result.Should().NotContain("Old annotation");
        }
        finally
        {
            File.Delete(tempFile);
        }
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void RunFor_WithMissingAnnotation_AddsAnnotation(AddEditAnnotation sut)
    {
        // Arrange
        var tempFile = Path.GetTempFileName();
        var vmxContent = "displayName = \"Test\"";

        try
        {
            File.WriteAllText(tempFile, vmxContent);

            // Act
            sut.RunFor(tempFile, "New annotation");

            // Assert
            var result = File.ReadAllText(tempFile);
            result.Should().Contain("annotation = \"New annotation\"");
        }
        finally
        {
            File.Delete(tempFile);
        }
    }
}