namespace VmMachineHwVersionUpdater.Core.Tests.PerMachine;

public class UpdateMachineVersionTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(UpdateMachineVersion).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(UpdateMachineVersion sut)
    {
        sut.Should().BeAssignableTo<IUpdateMachineVersion>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void RunFor_MachinesNull_ThrowsException(
        UpdateMachineVersion sut,
        int dummyNewVersion)
    {
        // Arrange

        // Act
        var result = Record.Exception(() => sut.RunFor(null, dummyNewVersion));

        // Assert
        result.Should().BeOfType<ArgumentNullException>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void RunFor_MachinesNotNull_CallsUpsertVmxLineRunFor(
        int dummyNewVersion,
        Machine dummyMachine0,
        Machine dummyMachine1,
        Machine dummyMachine2
    )
    {
        // Arrange
        var sut = new UpdateMachineVersion();

        // Create temporary files for testing
        var tempPath0 = Path.GetTempFileName();
        var tempPath1 = Path.GetTempFileName();
        var tempPath2 = Path.GetTempFileName();

        try
        {
            // Create basic VMX content for testing
            var vmxContent = "displayName = \"Test Machine\"\nvirtualhw.version = \"10\"";
            File.WriteAllText(tempPath0, vmxContent);
            File.WriteAllText(tempPath1, vmxContent);
            File.WriteAllText(tempPath2, vmxContent);

            dummyMachine0.IsEnabledForEditing = true;
            dummyMachine1.IsEnabledForEditing = true;
            dummyMachine2.IsEnabledForEditing = false; // This one should be skipped

            dummyMachine0.Path = tempPath0;
            dummyMachine1.Path = tempPath1;
            dummyMachine2.Path = tempPath2;

            var machines = new[]
            {
                dummyMachine0,
                dummyMachine1,
                dummyMachine2
            }.AsParallel();

            // Act
            sut.RunFor(machines, dummyNewVersion);

            // Assert - verify that the enabled machines had their files updated
            var content0 = File.ReadAllText(tempPath0);
            var content1 = File.ReadAllText(tempPath1);
            var content2 = File.ReadAllText(tempPath2);

            content0.Should().Contain($"virtualhw.version = \"{dummyNewVersion}\"");
            content1.Should().Contain($"virtualhw.version = \"{dummyNewVersion}\"");
            content2.Should().Contain("virtualhw.version = \"10\""); // Should remain unchanged
        }
        finally
        {
            // Clean up temporary files
            File.Delete(tempPath0);
            File.Delete(tempPath1);
            File.Delete(tempPath2);
        }
    }
}