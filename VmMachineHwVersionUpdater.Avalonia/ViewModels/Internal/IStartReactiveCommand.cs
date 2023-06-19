namespace VmMachineHwVersionUpdater.Avalonia.ViewModels.Internal;

/// <inheritdoc />
public interface IStartReactiveCommand : IReactiveCommandUnitRun
{
}

/// <inheritdoc />
public interface IReloadReactiveCommand : IReactiveCommandUnitRun
{
}

/// <inheritdoc cref="IReloadReactiveCommand" />
/// <inheritdoc cref="ReactiveCommandUnitRun" />
class ReloadReactiveCommand : ReactiveCommandUnitRun, IReloadReactiveCommand
{
    public override void Run()
    {
        throw new NotImplementedException();
    }
}

/// <inheritdoc />
public interface IUpdateAllReactiveCommand : IReactiveCommandUnitRun
{
}

/// <inheritdoc cref="IUpdateAllReactiveCommand" />
/// <inheritdoc cref="ReactiveCommandUnitRun" />
class UpdateAllReactiveCommand : ReactiveCommandUnitRun, IUpdateAllReactiveCommand
{
    public override void Run()
    {
        throw new NotImplementedException();
    }
}

/// <inheritdoc />
public interface IRenameReactiveCommand : IReactiveCommandUnitRun
{
}

/// <inheritdoc cref="IRenameReactiveCommand" />
/// <inheritdoc cref="ReactiveCommandUnitRun" />
class RenameReactiveCommand : ReactiveCommandUnitRun, IRenameReactiveCommand
{
    public override void Run()
    {
        throw new NotImplementedException();
    }
}

/// <inheritdoc />
public interface IGotToReactiveCommand : IReactiveCommandUnitRun
{
}

/// <inheritdoc cref="IGotToReactiveCommand" />
/// <inheritdoc cref="ReactiveCommandUnitRun" />
class GotToReactiveCommand : ReactiveCommandUnitRun, IGotToReactiveCommand
{
    public override void Run()
    {
        throw new NotImplementedException();
    }
}

/// <inheritdoc />
public interface IDeleteReactiveCommand : IReactiveCommandUnitRun
{
}

/// <inheritdoc cref="IDeleteReactiveCommand" />
/// <inheritdoc cref="ReactiveCommandUnitRun" />
class DeleteReactiveCommand : ReactiveCommandUnitRun, IDeleteReactiveCommand
{
    public override void Run()
    {
        throw new NotImplementedException();
    }
}

/// <inheritdoc />
public interface ICopyReactiveCommand : IReactiveCommandUnitRun
{
}

/// <inheritdoc cref="ICopyReactiveCommand" />
/// <inheritdoc cref="ReactiveCommandUnitRun" />
class CopyReactiveCommand : ReactiveCommandUnitRun, ICopyReactiveCommand
{
    public override void Run()
    {
        throw new NotImplementedException();
    }
}

/// <inheritdoc />
public interface IArchiveReactiveCommand : IReactiveCommandUnitRun
{
}

/// <inheritdoc cref="IArchiveReactiveCommand" />
/// <inheritdoc cref="ReactiveCommandUnitRun" />
class ArchiveReactiveCommand : ReactiveCommandUnitRun, IArchiveReactiveCommand
{
    public override void Run()
    {
        throw new NotImplementedException();
    }
}

/// <inheritdoc />
public interface IAddEditAnnotationReactiveCommand : IReactiveCommandUnitRun
{
}

/// <inheritdoc cref="IAddEditAnnotationReactiveCommand" />
/// <inheritdoc cref="ReactiveCommandUnitRun" />
class AddEditAnnotationReactiveCommand : ReactiveCommandUnitRun, IAddEditAnnotationReactiveCommand
{
    public override void Run()
    {
        throw new NotImplementedException();
    }
}