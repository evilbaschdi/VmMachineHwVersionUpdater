using System.Windows.Data;

namespace VmMachineHwVersionUpdater.ViewModels.Internal;

/// <inheritdoc cref="IWritableValue{T}" />
/// <inheritdoc cref="ICachedValue{T}" />
public interface IConfigureListCollectionView : IWritableValue<ListCollectionView>, ICachedValue<ListCollectionView>, IDialogCoordinatorContext
{
}