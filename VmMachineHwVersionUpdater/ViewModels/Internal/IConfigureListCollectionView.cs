using System.Windows.Data;
using EvilBaschdi.Core;

namespace VmMachineHwVersionUpdater.ViewModels.Internal
{
    /// <inheritdoc cref="IWritableValue{T}" />
    /// <inheritdoc cref="ICachedValue{T}" />
    public interface IConfigureListCollectionView : IWritableValue<ListCollectionView>, ICachedValue<ListCollectionView>
    {
    }
}