namespace VmMachineHwVersionUpdater.Core.Tests.Models;

public class MachinePathTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(MachinePath).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(MachinePath sut)
    {
        sut.Should().NotBeNull();
        sut.Should().BeOfType<MachinePath>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        // Filter out property setters for init-only properties as they don't need null guards
        var methods = typeof(MachinePath).GetMethods()
            .Where(method => !method.IsAbstract && !method.Name.StartsWith("set_"));
        assertion.Verify(methods);
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void MachinePoolPath_CanBeSetAndRetrieved(string expectedPath)
    {
        // Act
        var sut = new MachinePath { MachinePoolPath = expectedPath };

        // Assert
        sut.MachinePoolPath.Should().Be(expectedPath);
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void MachineFilePath_CanBeSetAndRetrieved(string expectedPath)
    {
        // Act
        var sut = new MachinePath { MachineFilePath = expectedPath };

        // Assert
        sut.MachineFilePath.Should().Be(expectedPath);
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void BothPaths_CanBeSetAndRetrieved(string expectedPoolPath, string expectedFilePath)
    {
        // Act
        var sut = new MachinePath
        {
            MachinePoolPath = expectedPoolPath,
            MachineFilePath = expectedFilePath
        };

        // Assert
        sut.MachinePoolPath.Should().Be(expectedPoolPath);
        sut.MachineFilePath.Should().Be(expectedFilePath);
    }
}