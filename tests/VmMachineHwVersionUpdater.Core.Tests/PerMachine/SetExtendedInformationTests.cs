namespace VmMachineHwVersionUpdater.Core.Tests.PerMachine;

public class SetExtendedInformationTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(SetExtendedInformation).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(SetExtendedInformation sut)
    {
        sut.Should().BeAssignableTo<ISetExtendedInformation>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(SetExtendedInformation).GetMethods().Where(method => !method.IsAbstract));
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void RunFor_WithAnnotation_SetsAnnotationEmoji(
        SetExtendedInformation sut,
        Machine machine)
    {
        // Arrange
        var rawMachine = new RawMachine { Annotation = "some note" };
        machine.IsEnabledForEditing = true;

        // Act
        sut.RunFor(rawMachine, machine);

        // Assert
        machine.ExtendedInformation.Should().Contain("📄");
        machine.ExtendedInformationToolTip.Should().Contain("has Annotation");
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void RunFor_WithManagedVmAutoAddVTpm_SetsLockEmoji(
        SetExtendedInformation sut,
        Machine machine)
    {
        // Arrange
        var rawMachine = new RawMachine { ManagedVmAutoAddVTpm = "TRUE" };
        machine.IsEnabledForEditing = true;

        // Act
        sut.RunFor(rawMachine, machine);

        // Assert
        machine.ExtendedInformation.Should().Contain("🔐");
        machine.ExtendedInformationToolTip.Should().Contain("has ManagedVmAutoAddVTpm");
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void RunFor_WhenNotEnabledForEditing_SetsSunglassesEmoji(
        SetExtendedInformation sut,
        Machine machine)
    {
        // Arrange
        var rawMachine = new RawMachine();
        machine.IsEnabledForEditing = false;

        // Act
        sut.RunFor(rawMachine, machine);

        // Assert
        machine.ExtendedInformation.Should().Contain("🕶");
        machine.ExtendedInformationToolTip.Should().Contain("is currently not enabled for editing");
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void RunFor_WithNoSpecialConditions_SetsEmptyExtendedInformation(
        SetExtendedInformation sut,
        Machine machine)
    {
        // Arrange
        var rawMachine = new RawMachine();
        machine.IsEnabledForEditing = true;

        // Act
        sut.RunFor(rawMachine, machine);

        // Assert
        machine.ExtendedInformation.Should().BeEmpty();
        machine.ExtendedInformationToolTip.Should().BeEmpty();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void RunFor_WithNullRawMachine_ThrowsArgumentNullException(
        SetExtendedInformation sut,
        Machine machine)
    {
        // Act & Assert
        var act = () => sut.RunFor(null!, machine);
        act.Should().Throw<ArgumentNullException>()
           .WithParameterName("rawMachine");
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void RunFor_WithNullMachine_ThrowsArgumentNullException(
        SetExtendedInformation sut)
    {
        // Arrange
        var rawMachine = new RawMachine();

        // Act & Assert
        var act = () => sut.RunFor(rawMachine, null!);
        act.Should().Throw<ArgumentNullException>()
           .WithParameterName("machine");
    }
}