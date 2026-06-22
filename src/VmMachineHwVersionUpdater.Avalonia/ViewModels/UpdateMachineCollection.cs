using Avalonia.Collections;
using Avalonia.Threading;
using Microsoft.Extensions.Logging;

namespace VmMachineHwVersionUpdater.Avalonia.ViewModels;

/// <inheritdoc cref="IUpdateMachineCollection" />
public class UpdateMachineCollection(
    [NotNull] IConfigureDataGridCollectionView configureDataGridCollectionView,
    [NotNull] ILogger<UpdateMachineCollection> logger) : IUpdateMachineCollection
{
    private readonly IConfigureDataGridCollectionView _configureDataGridCollectionView =
        configureDataGridCollectionView ?? throw new ArgumentNullException(nameof(configureDataGridCollectionView));

    private readonly ILogger<UpdateMachineCollection> _logger =
        logger ?? throw new ArgumentNullException(nameof(logger));

    /// <inheritdoc />
    public void ReplaceByPath(LoadHelper loadValue, string filePath, Machine machine)
    {
        ArgumentNullException.ThrowIfNull(loadValue);
        ArgumentNullException.ThrowIfNull(filePath);
        ArgumentNullException.ThrowIfNull(machine);

        Dispatcher.UIThread.Post(() =>
                                 {
                                     try
                                     {
                                         MutateAndRefresh(() =>
                                                          {
                                                              RemoveAllByPath(loadValue, filePath);
                                                              loadValue.VmDataGridItemsSource.Add(machine);
                                                          });
                                         _logger.LogDebug("Machine updated in UI for {FilePath}", filePath);
                                     }
                                     catch (Exception ex)
                                     {
                                         _logger.LogError(ex, "Error updating UI for {FilePath}", filePath);
                                     }
                                 });
    }

    /// <inheritdoc />
    public void RemoveByPath(LoadHelper loadValue, string filePath)
    {
        ArgumentNullException.ThrowIfNull(loadValue);
        ArgumentNullException.ThrowIfNull(filePath);

        Dispatcher.UIThread.Post(() =>
                                 {
                                     try
                                     {
                                         var removed = 0;
                                         MutateAndRefresh(() => removed = RemoveAllByPath(loadValue, filePath));

                                         if (removed > 0)
                                         {
                                             _logger.LogDebug("Machine removed from UI for {FilePath}", filePath);
                                         }
                                     }
                                     catch (Exception ex)
                                     {
                                         _logger.LogError(ex, "Error removing machine from UI for {FilePath}", filePath);
                                     }
                                 });
    }

    private static int RemoveAllByPath(LoadHelper loadValue, string filePath)
    {
        var removed = 0;
        for (var i = loadValue.VmDataGridItemsSource.Count - 1; i >= 0; i--)
        {
            if (string.Equals(loadValue.VmDataGridItemsSource[i].Path, filePath, StringComparison.OrdinalIgnoreCase))
            {
                loadValue.VmDataGridItemsSource.RemoveAt(i);
                removed++;
            }
        }

        return removed;
    }

    /// <summary>
    ///     Mutates the underlying source collection while the active filter is suspended.
    ///     When a <see cref="DataGridCollectionView" /> has a filter applied, its internal
    ///     (filtered) indices diverge from the source collection indices. Mutating the source
    ///     then raises incremental change notifications that the view maps against the wrong
    ///     indices, which can make items vanish from the filtered view. Removing the filter
    ///     before mutating keeps the indices in sync, and re-applying it afterwards performs
    ///     a clean full refresh (sorting, grouping and filtering).
    /// </summary>
    private void MutateAndRefresh(Action mutate)
    {
        var view = _configureDataGridCollectionView.Value;
        var filter = view?.Filter;

        if (filter is not null)
        {
            view.Filter = null!;
        }

        mutate();

        if (filter is not null)
        {
            view.Filter = filter;
        }
        else
        {
            // ReSharper disable once ExpressionIsAlwaysNull
            // ReSharper disable once ConstantConditionalAccessQualifier
            view?.Refresh();
        }
    }
}