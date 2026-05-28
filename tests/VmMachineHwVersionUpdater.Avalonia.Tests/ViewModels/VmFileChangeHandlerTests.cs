using VmMachineHwVersionUpdater.Avalonia.ViewModels;

namespace VmMachineHwVersionUpdater.Avalonia.Tests.ViewModels;

public class VmFileChangeHandlerTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(VmFileChangeHandler).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(VmFileChangeHandler sut)
    {
        sut.Should().BeAssignableTo<IVmFileChangeHandler>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(VmFileChangeHandler).GetMethods().Where(method => !method.IsAbstract));
    }

    #region Start Tests

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Start_WhenCalled_StartsFileWatcher(
        [Frozen] IVmFileWatcher vmFileWatcher,
        VmFileChangeHandler sut)
    {
        // Act
        sut.Start();

        // Assert
        vmFileWatcher.Received(1).Start();
    }

    #endregion

    #region Stop Tests

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Stop_WhenCalled_StopsFileWatcher(
        [Frozen] IVmFileWatcher vmFileWatcher,
        VmFileChangeHandler sut)
    {
        // Act
        sut.Stop();

        // Assert
        vmFileWatcher.Received(1).Stop();
    }

    #endregion

    #region Dispose Tests

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Dispose_WhenCalled_CanBeCalledMultipleTimes(VmFileChangeHandler sut)
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
    public void Dispose_WhenCalled_StopsFileWatcher(
        [Frozen] IVmFileWatcher vmFileWatcher,
        VmFileChangeHandler sut)
    {
        // Act
        sut.Dispose();

        // Assert
        vmFileWatcher.Received(1).Stop();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Dispose_WhenCalled_ImplementsIDisposable(VmFileChangeHandler sut)
    {
        // Assert
        sut.Should().BeAssignableTo<IDisposable>();
    }

    #endregion
}