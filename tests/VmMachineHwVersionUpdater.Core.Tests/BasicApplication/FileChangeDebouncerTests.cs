namespace VmMachineHwVersionUpdater.Core.Tests.BasicApplication;

public class FileChangeDebouncerTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(FileChangeDebouncer).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(FileChangeDebouncer sut)
    {
        sut.Should().BeAssignableTo<IFileChangeDebouncer>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(FileChangeDebouncer).GetMethods().Where(method => !method.IsAbstract));
    }

    #region Dispose Tests

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Dispose_WhenCalled_CanBeCalledMultipleTimes(FileChangeDebouncer sut)
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
    public void Dispose_WhenCalled_ImplementsIDisposable(FileChangeDebouncer sut)
    {
        // Assert
        sut.Should().BeAssignableTo<IDisposable>();
    }

    #endregion

    #region Debounce Tests

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public async Task Debounce_WithSingleCall_ExecutesCallbackAfterInterval(FileChangeDebouncer sut)
    {
        // Arrange
        var executed = false;

        // Act
        sut.Debounce("key1", () => executed = true);
        await Task.Delay(1500, TestContext.Current.CancellationToken);

        // Assert
        executed.Should().BeTrue();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public async Task Debounce_WithMultipleCalls_ExecutesOnlyLastCallback(FileChangeDebouncer sut)
    {
        // Arrange
        var callCount = 0;
        var lastValue = string.Empty;

        // Act
        sut.Debounce("key1", () =>
                             {
                                 callCount++;
                                 lastValue = "first";
                             });
        await Task.Delay(100, TestContext.Current.CancellationToken);
        sut.Debounce("key1", () =>
                             {
                                 callCount++;
                                 lastValue = "second";
                             });
        await Task.Delay(100, TestContext.Current.CancellationToken);
        sut.Debounce("key1", () =>
                             {
                                 callCount++;
                                 lastValue = "third";
                             });
        await Task.Delay(1500, TestContext.Current.CancellationToken);

        // Assert
        callCount.Should().Be(1);
        lastValue.Should().Be("third");
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public async Task Debounce_WithDifferentKeys_ExecutesBothCallbacks(FileChangeDebouncer sut)
    {
        // Arrange
        var executed1 = false;
        var executed2 = false;

        // Act
        sut.Debounce("key1", () => executed1 = true);
        sut.Debounce("key2", () => executed2 = true);
        await Task.Delay(1500, TestContext.Current.CancellationToken);

        // Assert
        executed1.Should().BeTrue();
        executed2.Should().BeTrue();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Debounce_AfterDispose_DoesNotExecuteCallback(FileChangeDebouncer sut)
    {
        // Arrange
        var executed = false;
        sut.Dispose();

        // Act
        sut.Debounce("key1", () => executed = true);

        // Assert
        executed.Should().BeFalse();
    }

    #endregion

    #region CancelAll Tests

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public async Task CancelAll_WithPendingCallbacks_PreventsExecution(FileChangeDebouncer sut)
    {
        // Arrange
        var executed = false;
        sut.Debounce("key1", () => executed = true);

        // Act
        sut.CancelAll();
        await Task.Delay(1500, TestContext.Current.CancellationToken);

        // Assert
        executed.Should().BeFalse();
    }

    #endregion
}