﻿using EvilBaschdi.Core.Wpf;
using EvilBaschdi.Core.Wpf.Mvvm.ViewModel;

namespace VmMachineHwVersionUpdater.Wpf.ViewModels;

/// <inheritdoc cref="IAddEditAnnotationDialogViewModel" />
public class AddEditAnnotationDialogViewModel : ApplicationLayoutViewModel, IAddEditAnnotationDialogViewModel
{
    private readonly ICurrentItem _currentItem;
    private readonly IUpdateAnnotation _updateAnnotation;

    #region Constructor

    /// <summary>
    ///     Constructor
    /// </summary>
    public AddEditAnnotationDialogViewModel([NotNull] IApplicationLayout applicationLayout,
                                            [NotNull] IApplicationStyle applicationStyle,
                                            [NotNull] IUpdateAnnotation updateAnnotation,
                                            [NotNull] ICurrentItem currentItem)
        : base(applicationLayout, applicationStyle, true, false)
    {
        _updateAnnotation = updateAnnotation ?? throw new ArgumentNullException(nameof(updateAnnotation));
        _currentItem = currentItem ?? throw new ArgumentNullException(nameof(currentItem));
    }

    #endregion Constructor

    #region Properties

    /// <summary>
    ///     Binding
    /// </summary>
    public Machine SelectedMachine
    {
        get => _currentItem.Value;
        set
        {
            _currentItem.Value = value;
            OnPropertyChanged();
        }
    }

    /// <inheritdoc />
    public string AnnotationText
    {
        get => _updateAnnotation.Value;
        set => _updateAnnotation.Value = value;
    }

    #endregion Properties
}