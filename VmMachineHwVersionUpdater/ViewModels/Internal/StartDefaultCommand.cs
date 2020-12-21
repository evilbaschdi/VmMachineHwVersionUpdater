using System;
using EvilBaschdi.CoreExtended.AppHelpers;
using EvilBaschdi.CoreExtended.Mvvm.ViewModel.Command;
using JetBrains.Annotations;

namespace VmMachineHwVersionUpdater.ViewModels.Internal
{
    /// <inheritdoc />
    public class StartDefaultCommand : IStartDefaultCommand
    {
        private readonly IProcessByPath _processByPath;
        private readonly ISelectedMachine _selectedMachine;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="processByPath"></param>
        /// <param name="selectedMachine"></param>
        public StartDefaultCommand([NotNull] IProcessByPath processByPath, [NotNull] ISelectedMachine selectedMachine)
        {
            _processByPath = processByPath ?? throw new ArgumentNullException(nameof(processByPath));
            _selectedMachine = selectedMachine ?? throw new ArgumentNullException(nameof(selectedMachine));
        }

        /// <inheritdoc />
        public DefaultCommand Value => new()
                                       {
                                           Command = new RelayCommand(_ => Run())
                                       };

        /// <inheritdoc />
        public void Run()
        {
            _processByPath.RunFor(_selectedMachine.Value.Path);
        }
    }
}