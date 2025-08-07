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
        UpdateMachineVersion sut,
        int dummyNewVersion,
        Machine dummyMachine0,
        Machine dummyMachine1,
        Machine dummyMachine2,
        string dummyPath0,
        string dummyPath1,
        string dummyPath2
    )
    {
        // Arrange
        dummyMachine0.IsEnabledForEditing = true;
        dummyMachine1.IsEnabledForEditing = true;
        dummyMachine2.IsEnabledForEditing = false;

        dummyMachine0.Path = dummyPath0;
        dummyMachine1.Path = dummyPath1;
        dummyMachine2.Path = dummyPath2;

        var machines = new[]
                       {
                           dummyMachine0,
                           dummyMachine1,
                           dummyMachine2
                       }.AsParallel();

        // Act
        sut.RunFor(machines, dummyNewVersion);

        // Assert
        sut.Received(1).RunFor(dummyPath0, dummyNewVersion);
        sut.Received(1).RunFor(dummyPath1, dummyNewVersion);
        sut.Received(1).RunFor(dummyPath2, dummyNewVersion);
    }
}