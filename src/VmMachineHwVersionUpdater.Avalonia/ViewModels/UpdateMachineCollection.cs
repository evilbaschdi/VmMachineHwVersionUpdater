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
                                         RemoveAllByPath(loadValue, filePath);
                                         loadValue.VmDataGridItemsSource.Add(machine);
                                         RefreshView();
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
                                         var removed = RemoveAllByPath(loadValue, filePath);

                                         if (removed > 0)
                                         {
                                             RefreshView();
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

    private void RefreshView()
    {
        _configureDataGridCollectionView.Value.Refresh();
    }
}