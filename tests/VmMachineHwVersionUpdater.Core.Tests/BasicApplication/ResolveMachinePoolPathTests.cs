namespace VmMachineHwVersionUpdater.Core.Tests.BasicApplication;

public class ResolveMachinePoolPathTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ResolveMachinePoolPath).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(ResolveMachinePoolPath sut)
    {
        sut.Should().BeAssignableTo<IResolveMachinePoolPath>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ResolveMachinePoolPath).GetMethods().Where(method => !method.IsAbstract));
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void ValueFor_WithMatchingPool_ReturnsLongestMatch(
        [Frozen] IPathSettings pathSettings,
        ResolveMachinePoolPath sut)
    {
        // Arrange
        pathSettings.VmPool.Returns(["C:\\VMs", "C:\\VMs\\Production"]);
        pathSettings.ArchivePath.Returns(["C:\\VMs\\Archive"]);

        // Act
        var result = sut.ValueFor("C:\\VMs\\Production\\Server1\\Server1.vmx");

        // Assert
        result.Should().Be("C:\\VMs\\Production");
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void ValueFor_WithNoMatchingPool_ReturnsNull(
        [Frozen] IPathSettings pathSettings,
        ResolveMachinePoolPath sut)
    {
        // Arrange
        pathSettings.VmPool.Returns(["C:\\VMs"]);
        pathSettings.ArchivePath.Returns(["C:\\Archive"]);

        // Act
        var result = sut.ValueFor("D:\\Other\\Server1.vmx");

        // Assert
        result.Should().BeNull();
    }
}