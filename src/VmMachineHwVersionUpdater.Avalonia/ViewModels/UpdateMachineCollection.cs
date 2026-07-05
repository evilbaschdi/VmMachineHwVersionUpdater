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
                MutateAndRefresh(loadValue, () => ReplaceByPathInSource(loadValue, filePath, machine));
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
                MutateAndRefresh(loadValue, () => removed = RemoveAllByPath(loadValue, filePath));

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

    private static void ReplaceByPathInSource(LoadHelper loadValue, string filePath, Machine machine)
    {
        var matchingIndices = new List<int>();

        for (var i = 0; i < loadValue.VmDataGridItemsSource.Count; i++)
        {
            if (string.Equals(loadValue.VmDataGridItemsSource[i].Path, filePath, StringComparison.OrdinalIgnoreCase))
            {
                matchingIndices.Add(i);
            }
        }

        switch (matchingIndices.Count)
        {
            case 0:
                loadValue.VmDataGridItemsSource.Add(machine);
                return;
            case 1:
                loadValue.VmDataGridItemsSource[matchingIndices[0]].ApplyFrom(machine);
                return;
            default:
                for (var i = matchingIndices.Count - 1; i >= 0; i--)
                {
                    loadValue.VmDataGridItemsSource.RemoveAt(matchingIndices[i]);
                }

                loadValue.VmDataGridItemsSource.Add(machine);
                break;
        }
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
    private void MutateAndRefresh(LoadHelper loadValue, Action mutate)
    {
        var view = _configureDataGridCollectionView.Value;

        if (view is null)
        {
            mutate();
            return;
        }

        Dispatcher.UIThread.Post(() =>
        {
            try
            {
                mutate();
                RefreshView(loadValue, view);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Collection refresh failed for file watcher update");
            }
        });
    }

    private void RefreshView(LoadHelper loadValue, DataGridCollectionView view)
    {
        try
        {
            var currentFilter = TryGetFilter(view);

            if (currentFilter is null)
            {
                view.Refresh();
                return;
            }

            var replacementView = CreateReplacementView(loadValue.VmDataGridItemsSource, currentFilter);
            _configureDataGridCollectionView.Value = replacementView;
        }
        catch (Exception ex) when (IsFilterMutationException(ex))
        {
            _logger.LogDebug(ex,
                "Filtered collection view could not be refreshed in place; replacing it on the next tick");
            Dispatcher.UIThread.Post(() => RefreshView(loadValue, view));
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Collection refresh failed for file watcher update");
        }
    }

    private static DataGridCollectionView CreateReplacementView(System.Collections.IEnumerable source,
        Func<object, bool> filter)
    {
        var replacementView = new DataGridCollectionView(source)
        {
            Filter = filter
        };

        return replacementView;
    }

    private static Func<object, bool> TryGetFilter(DataGridCollectionView view)
    {
        try
        {
            return view.Filter;
        }
        catch (Exception)
        {
            return null;
        }
    }

    private static bool IsFilterMutationException(Exception ex)
    {
        return ex.Message.Contains("Filter", StringComparison.OrdinalIgnoreCase);
    }
}