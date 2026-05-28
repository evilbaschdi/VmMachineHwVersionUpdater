namespace VmMachineHwVersionUpdater.Core.Tests.BasicApplication;

public class VmFileWatcherTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(VmFileWatcher).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(VmFileWatcher sut)
    {
        sut.Should().BeAssignableTo<IVmFileWatcher>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(VmFileWatcher).GetMethods().Where(method => !method.IsAbstract
                                                                            & !method.Name.StartsWith("add_")
                                                                            & !method.Name.StartsWith("remove_")));
    }

    #region Dispose Tests

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Dispose_WhenCalled_CanBeCalledMultipleTimes(VmFileWatcher sut)
    {
        // Act
        var act = () =>
                  {
                      sut.Dispose();
                      sut.Dispose();
                  };

        // Assert
        act.Should().NotThrow();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Dispose_WhenCalled_ImplementsIDisposable(VmFileWatcher sut)
    {
        // Assert
        sut.Should().BeAssignableTo<IDisposable>();
    }

    #endregion

    #region Stop Tests

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Stop_WhenCalledWithoutStart_DoesNotThrow(VmFileWatcher sut)
    {
        // Act
        var act = () => sut.Stop();

        // Assert
        act.Should().NotThrow();
    }

    #endregion
}