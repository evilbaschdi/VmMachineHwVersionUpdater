using System;
using EvilBaschdi.CoreExtended.AppHelpers;
using EvilBaschdi.CoreExtended.Mvvm.ViewModel.Command;
using JetBrains.Annotations;
using VmMachineHwVersionUpdater.Core.Models;

namespace VmMachineHwVersionUpdater.ViewModels.Internal
{
    /// <inheritdoc />
    public class StartDefaultCommand : IStartDefaultCommand
    {
        private readonly IProcessByPath _processByPath;
        private readonly ICurrentItem _currentItem;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="processByPath"></param>
        /// <param name="currentItem"></param>
        public StartDefaultCommand([NotNull] IProcessByPath processByPath, [NotNull] ICurrentItem currentItem)
        {
            _processByPath = processByPath ?? throw new ArgumentNullException(nameof(processByPath));
            _currentItem = currentItem ?? throw new ArgumentNullException(nameof(currentItem));
        }

        /// <inheritdoc />
        public DefaultCommand Value => new()
                                       {
                                           Command = new RelayCommand(_ => Run())
                                       };

        /// <inheritdoc />
        public void Run()
        {
            _processByPath.RunFor(_currentItem.Value.Path);
        }
    }
}