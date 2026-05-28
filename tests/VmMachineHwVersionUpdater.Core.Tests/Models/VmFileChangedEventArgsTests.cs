namespace VmMachineHwVersionUpdater.Core.Tests.Models;

public class VmFileChangedEventArgsTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(VmFileChangedEventArgs).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(VmFileChangedEventArgs sut)
    {
        sut.Should().NotBeNull();
        sut.Should().BeOfType<VmFileChangedEventArgs>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        var methods = typeof(VmFileChangedEventArgs).GetMethods()
                                                    .Where(method => !method.IsAbstract & !method.Name.StartsWith("set_"));
        assertion.Verify(methods);
    }

    #region Property Tests

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void FilePath_CanBeSetAndRetrieved(string expectedPath)
    {
        // Act
        var sut = new VmFileChangedEventArgs { FilePath = expectedPath };

        // Assert
        sut.FilePath.Should().Be(expectedPath);
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void OldFilePath_CanBeSetAndRetrieved(string expectedPath)
    {
        // Act
        var sut = new VmFileChangedEventArgs { OldFilePath = expectedPath };

        // Assert
        sut.OldFilePath.Should().Be(expectedPath);
    }

    [Theory]
    [NSubstituteOmitAutoPropertiesTrueInlineAutoData(VmFileChangeType.Changed)]
    [NSubstituteOmitAutoPropertiesTrueInlineAutoData(VmFileChangeType.Created)]
    [NSubstituteOmitAutoPropertiesTrueInlineAutoData(VmFileChangeType.Deleted)]
    [NSubstituteOmitAutoPropertiesTrueInlineAutoData(VmFileChangeType.Renamed)]
    public void ChangeType_CanBeSetAndRetrieved(VmFileChangeType expectedType)
    {
        // Act
        var sut = new VmFileChangedEventArgs { ChangeType = expectedType };

        // Assert
        sut.ChangeType.Should().Be(expectedType);
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void OldFilePath_DefaultValue_IsNull(VmFileChangedEventArgs sut)
    {
        // Assert
        sut.OldFilePath.Should().BeNull();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void ChangeType_DefaultValue_IsChanged(VmFileChangedEventArgs sut)
    {
        // Assert
        sut.ChangeType.Should().Be(VmFileChangeType.Changed);
    }

    #endregion
}