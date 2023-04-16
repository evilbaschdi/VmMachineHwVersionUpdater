﻿using Avalonia.Collections;
using EvilBaschdi.Core;

namespace VmMachineHwVersionUpdater.Avalonia.ViewModels;

/// <inheritdoc cref="IWritableValue{T}" />
/// <inheritdoc cref="ICachedValue{T}" />
public interface IConfigureDataGridCollectionView : IWritableValue<DataGridCollectionView>, ICachedValue<DataGridCollectionView>
{
}