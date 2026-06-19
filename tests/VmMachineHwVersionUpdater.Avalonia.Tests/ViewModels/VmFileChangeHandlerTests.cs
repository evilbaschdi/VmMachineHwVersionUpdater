using System.Collections.ObjectModel;
using NSubstitute.ExceptionExtensions;
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

    #region OnFileChanged Tests

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void OnFileChanged_WithNullLoadValue_DoesNotThrow(
        [Frozen] ILoad load,
        [Frozen] IVmFileWatcher vmFileWatcher,
        VmFileChangeHandler sut)
    {
        // Arrange
        load.Value.Returns((LoadHelper)null);
        sut.Start();

        // Act
        var act = () => vmFileWatcher.FileChanged += Raise.Event<Action<VmFileChangedEventArgs>>(
                      new VmFileChangedEventArgs { FilePath = @"C:\VMs\test.vmx", ChangeType = VmFileChangeType.Changed });

        // Assert
        act.Should().NotThrow();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void OnFileChanged_WithChangedVmxFile_CallsUpdateMachineCollection(
        [Frozen] ILoad load,
        [Frozen] IVmFileWatcher vmFileWatcher,
        [Frozen] IResolveMachinePoolPath resolveMachinePoolPath,
        [Frozen] IHandleMachineFromPath handleMachineFromPath,
        [Frozen] IUpdateMachineCollection updateMachineCollection,
        VmFileChangeHandler sut,
        Machine machine)
    {
        // Arrange
        var filePath = @"C:\VMs\Server1\Server1.vmx";
        var loadHelper = new LoadHelper { VmDataGridItemsSource = new ObservableCollection<Machine>() };
        load.Value.Returns(loadHelper);
        resolveMachinePoolPath.ValueFor(filePath).Returns(@"C:\VMs");
        handleMachineFromPath.ValueFor(Arg.Any<MachinePath>()).Returns(machine);
        sut.Start();

        // Act
        vmFileWatcher.FileChanged += Raise.Event<Action<VmFileChangedEventArgs>>(
            new VmFileChangedEventArgs { FilePath = filePath, ChangeType = VmFileChangeType.Changed });

        // Assert
        updateMachineCollection.Received(1).ReplaceByPath(loadHelper, filePath, machine);
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void OnFileChanged_WithCreatedVmxFile_CallsUpdateMachineCollection(
        [Frozen] ILoad load,
        [Frozen] IVmFileWatcher vmFileWatcher,
        [Frozen] IResolveMachinePoolPath resolveMachinePoolPath,
        [Frozen] IHandleMachineFromPath handleMachineFromPath,
        [Frozen] IUpdateMachineCollection updateMachineCollection,
        VmFileChangeHandler sut,
        Machine machine)
    {
        // Arrange
        var filePath = @"C:\VMs\Server1\Server1.vmx";
        var loadHelper = new LoadHelper { VmDataGridItemsSource = new ObservableCollection<Machine>() };
        load.Value.Returns(loadHelper);
        resolveMachinePoolPath.ValueFor(filePath).Returns(@"C:\VMs");
        handleMachineFromPath.ValueFor(Arg.Any<MachinePath>()).Returns(machine);
        sut.Start();

        // Act
        vmFileWatcher.FileChanged += Raise.Event<Action<VmFileChangedEventArgs>>(
            new VmFileChangedEventArgs { FilePath = filePath, ChangeType = VmFileChangeType.Created });

        // Assert
        updateMachineCollection.Received(1).ReplaceByPath(loadHelper, filePath, machine);
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void OnFileChanged_WithChangedLogFile_CallsUpdateMachineStateFromFile(
        [Frozen] ILoad load,
        [Frozen] IVmFileWatcher vmFileWatcher,
        [Frozen] IUpdateMachineStateFromFile updateMachineStateFromFile,
        [Frozen] IUpdateMachineCollection updateMachineCollection,
        VmFileChangeHandler sut)
    {
        // Arrange
        var filePath = @"C:\VMs\Server1\vmware.log";
        var loadHelper = new LoadHelper { VmDataGridItemsSource = new ObservableCollection<Machine>() };
        load.Value.Returns(loadHelper);
        sut.Start();

        // Act
        vmFileWatcher.FileChanged += Raise.Event<Action<VmFileChangedEventArgs>>(
            new VmFileChangedEventArgs { FilePath = filePath, ChangeType = VmFileChangeType.Changed });

        // Assert
        updateMachineStateFromFile.Received(1).UpdateFor(filePath, loadHelper);
        updateMachineCollection.DidNotReceive().ReplaceByPath(Arg.Any<LoadHelper>(), Arg.Any<string>(), Arg.Any<Machine>());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void OnFileChanged_WithChangedVmssFile_CallsUpdateMachineStateFromFile(
        [Frozen] ILoad load,
        [Frozen] IVmFileWatcher vmFileWatcher,
        [Frozen] IUpdateMachineStateFromFile updateMachineStateFromFile,
        VmFileChangeHandler sut)
    {
        // Arrange
        var filePath = @"C:\VMs\Server1\Server1.vmss";
        var loadHelper = new LoadHelper { VmDataGridItemsSource = new ObservableCollection<Machine>() };
        load.Value.Returns(loadHelper);
        sut.Start();

        // Act
        vmFileWatcher.FileChanged += Raise.Event<Action<VmFileChangedEventArgs>>(
            new VmFileChangedEventArgs { FilePath = filePath, ChangeType = VmFileChangeType.Changed });

        // Assert
        updateMachineStateFromFile.Received(1).UpdateFor(filePath, loadHelper);
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void OnFileChanged_WithDeletedVmxFile_CallsRemoveByPath(
        [Frozen] ILoad load,
        [Frozen] IVmFileWatcher vmFileWatcher,
        [Frozen] IUpdateMachineCollection updateMachineCollection,
        VmFileChangeHandler sut)
    {
        // Arrange
        var filePath = @"C:\VMs\Server1\Server1.vmx";
        var loadHelper = new LoadHelper { VmDataGridItemsSource = new ObservableCollection<Machine>() };
        load.Value.Returns(loadHelper);
        sut.Start();

        // Act
        vmFileWatcher.FileChanged += Raise.Event<Action<VmFileChangedEventArgs>>(
            new VmFileChangedEventArgs { FilePath = filePath, ChangeType = VmFileChangeType.Deleted });

        // Assert
        updateMachineCollection.Received(1).RemoveByPath(loadHelper, filePath);
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void OnFileChanged_WithDeletedLogFile_CallsUpdateMachineStateFromFile(
        [Frozen] ILoad load,
        [Frozen] IVmFileWatcher vmFileWatcher,
        [Frozen] IUpdateMachineStateFromFile updateMachineStateFromFile,
        [Frozen] IUpdateMachineCollection updateMachineCollection,
        VmFileChangeHandler sut)
    {
        // Arrange
        var filePath = @"C:\VMs\Server1\vmware.log";
        var loadHelper = new LoadHelper { VmDataGridItemsSource = new ObservableCollection<Machine>() };
        load.Value.Returns(loadHelper);
        sut.Start();

        // Act
        vmFileWatcher.FileChanged += Raise.Event<Action<VmFileChangedEventArgs>>(
            new VmFileChangedEventArgs { FilePath = filePath, ChangeType = VmFileChangeType.Deleted });

        // Assert
        updateMachineStateFromFile.Received(1).UpdateFor(filePath, loadHelper);
        updateMachineCollection.DidNotReceive().RemoveByPath(Arg.Any<LoadHelper>(), Arg.Any<string>());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void OnFileChanged_WithRenamedVmxFile_CallsRemoveAndReplace(
        [Frozen] ILoad load,
        [Frozen] IVmFileWatcher vmFileWatcher,
        [Frozen] IResolveMachinePoolPath resolveMachinePoolPath,
        [Frozen] IHandleMachineFromPath handleMachineFromPath,
        [Frozen] IUpdateMachineCollection updateMachineCollection,
        VmFileChangeHandler sut,
        Machine machine)
    {
        // Arrange
        var oldFilePath = @"C:\VMs\Server1\OldName.vmx";
        var newFilePath = @"C:\VMs\Server1\NewName.vmx";
        var loadHelper = new LoadHelper { VmDataGridItemsSource = new ObservableCollection<Machine>() };
        load.Value.Returns(loadHelper);
        resolveMachinePoolPath.ValueFor(newFilePath).Returns(@"C:\VMs");
        handleMachineFromPath.ValueFor(Arg.Any<MachinePath>()).Returns(machine);
        sut.Start();

        // Act
        vmFileWatcher.FileChanged += Raise.Event<Action<VmFileChangedEventArgs>>(
            new VmFileChangedEventArgs { FilePath = newFilePath, OldFilePath = oldFilePath, ChangeType = VmFileChangeType.Renamed });

        // Assert
        updateMachineCollection.Received(1).RemoveByPath(loadHelper, oldFilePath);
        updateMachineCollection.Received(1).ReplaceByPath(loadHelper, newFilePath, machine);
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void OnFileChanged_WithRenamedLogFile_CallsUpdateMachineStateFromFile(
        [Frozen] ILoad load,
        [Frozen] IVmFileWatcher vmFileWatcher,
        [Frozen] IUpdateMachineStateFromFile updateMachineStateFromFile,
        VmFileChangeHandler sut)
    {
        // Arrange
        var oldFilePath = @"C:\VMs\Server1\old.log";
        var newFilePath = @"C:\VMs\Server1\vmware.log";
        var loadHelper = new LoadHelper { VmDataGridItemsSource = new ObservableCollection<Machine>() };
        load.Value.Returns(loadHelper);
        sut.Start();

        // Act
        vmFileWatcher.FileChanged += Raise.Event<Action<VmFileChangedEventArgs>>(
            new VmFileChangedEventArgs { FilePath = newFilePath, OldFilePath = oldFilePath, ChangeType = VmFileChangeType.Renamed });

        // Assert
        updateMachineStateFromFile.Received(1).UpdateFor(newFilePath, loadHelper);
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void OnFileChanged_WithNoPoolPath_DoesNotCallUpdateMachineCollection(
        [Frozen] ILoad load,
        [Frozen] IVmFileWatcher vmFileWatcher,
        [Frozen] IResolveMachinePoolPath resolveMachinePoolPath,
        [Frozen] IUpdateMachineCollection updateMachineCollection,
        VmFileChangeHandler sut)
    {
        // Arrange
        var filePath = @"D:\Other\Server1.vmx";
        var loadHelper = new LoadHelper { VmDataGridItemsSource = new ObservableCollection<Machine>() };
        load.Value.Returns(loadHelper);
        resolveMachinePoolPath.ValueFor(filePath).Returns((string)null);
        sut.Start();

        // Act
        vmFileWatcher.FileChanged += Raise.Event<Action<VmFileChangedEventArgs>>(
            new VmFileChangedEventArgs { FilePath = filePath, ChangeType = VmFileChangeType.Changed });

        // Assert
        updateMachineCollection.DidNotReceive().ReplaceByPath(Arg.Any<LoadHelper>(), Arg.Any<string>(), Arg.Any<Machine>());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void OnFileChanged_WithNullMachineResult_DoesNotCallReplaceByPath(
        [Frozen] ILoad load,
        [Frozen] IVmFileWatcher vmFileWatcher,
        [Frozen] IResolveMachinePoolPath resolveMachinePoolPath,
        [Frozen] IHandleMachineFromPath handleMachineFromPath,
        [Frozen] IUpdateMachineCollection updateMachineCollection,
        VmFileChangeHandler sut)
    {
        // Arrange
        var filePath = @"C:\VMs\Server1\Server1.vmx";
        var loadHelper = new LoadHelper { VmDataGridItemsSource = new ObservableCollection<Machine>() };
        load.Value.Returns(loadHelper);
        resolveMachinePoolPath.ValueFor(filePath).Returns(@"C:\VMs");
        handleMachineFromPath.ValueFor(Arg.Any<MachinePath>()).Returns((Machine)null);
        sut.Start();

        // Act
        vmFileWatcher.FileChanged += Raise.Event<Action<VmFileChangedEventArgs>>(
            new VmFileChangedEventArgs { FilePath = filePath, ChangeType = VmFileChangeType.Changed });

        // Assert
        updateMachineCollection.DidNotReceive().ReplaceByPath(Arg.Any<LoadHelper>(), Arg.Any<string>(), Arg.Any<Machine>());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void OnFileChanged_WithDirectoryNotFoundException_CallsRetryPolicy(
        [Frozen] ILoad load,
        [Frozen] IVmFileWatcher vmFileWatcher,
        [Frozen] IResolveMachinePoolPath resolveMachinePoolPath,
        [Frozen] IHandleMachineFromPath handleMachineFromPath,
        [Frozen] IFileAccessRetryPolicy fileAccessRetryPolicy,
        VmFileChangeHandler sut)
    {
        // Arrange
        var filePath = @"C:\VMs\Server1\Server1.vmx";
        var loadHelper = new LoadHelper { VmDataGridItemsSource = new ObservableCollection<Machine>() };
        load.Value.Returns(loadHelper);
        resolveMachinePoolPath.ValueFor(filePath).Returns(@"C:\VMs");
        handleMachineFromPath.ValueFor(Arg.Any<MachinePath>()).Throws(new DirectoryNotFoundException("not found"));
        fileAccessRetryPolicy.TryExecuteAsync(Arg.Any<Func<Task<Machine>>>(), Arg.Any<string>(), Arg.Any<Func<int, Exception, Task>>())
                             .Returns((false, (Machine)null));
        sut.Start();

        // Act
        vmFileWatcher.FileChanged += Raise.Event<Action<VmFileChangedEventArgs>>(
            new VmFileChangedEventArgs { FilePath = filePath, ChangeType = VmFileChangeType.Changed });

        // Assert
        fileAccessRetryPolicy.Received(1).TryExecuteAsync(Arg.Any<Func<Task<Machine>>>(), filePath, Arg.Any<Func<int, Exception, Task>>());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void OnFileChanged_WithIOException_CallsRetryPolicy(
        [Frozen] ILoad load,
        [Frozen] IVmFileWatcher vmFileWatcher,
        [Frozen] IResolveMachinePoolPath resolveMachinePoolPath,
        [Frozen] IHandleMachineFromPath handleMachineFromPath,
        [Frozen] IFileAccessRetryPolicy fileAccessRetryPolicy,
        VmFileChangeHandler sut)
    {
        // Arrange
        var filePath = @"C:\VMs\Server1\Server1.vmx";
        var loadHelper = new LoadHelper { VmDataGridItemsSource = new ObservableCollection<Machine>() };
        load.Value.Returns(loadHelper);
        resolveMachinePoolPath.ValueFor(filePath).Returns(@"C:\VMs");
        handleMachineFromPath.ValueFor(Arg.Any<MachinePath>()).Throws(new IOException("file locked"));
        fileAccessRetryPolicy.TryExecuteAsync(Arg.Any<Func<Task<Machine>>>(), Arg.Any<string>(), Arg.Any<Func<int, Exception, Task>>())
                             .Returns((false, (Machine)null));
        sut.Start();

        // Act
        vmFileWatcher.FileChanged += Raise.Event<Action<VmFileChangedEventArgs>>(
            new VmFileChangedEventArgs { FilePath = filePath, ChangeType = VmFileChangeType.Changed });

        // Assert
        fileAccessRetryPolicy.Received(1).TryExecuteAsync(Arg.Any<Func<Task<Machine>>>(), filePath, Arg.Any<Func<int, Exception, Task>>());
    }

    #endregion
}