using System;
using System.IO;
using EvilBaschdi.CoreExtended.AppHelpers;
using EvilBaschdi.CoreExtended.Mvvm.ViewModel.Command;
using JetBrains.Annotations;

namespace VmMachineHwVersionUpdater.ViewModels.Internal
{
    /// <inheritdoc />
    public class OpenWithCodeDefaultCommand : IOpenWithCodeDefaultCommand
    {
        private readonly IProcessByPath _processByPath;
        private readonly ISelectedMachine _selectedMachine;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="selectedMachine"></param>
        /// <param name="processByPath"></param>
        public OpenWithCodeDefaultCommand([NotNull] ISelectedMachine selectedMachine, [NotNull] IProcessByPath processByPath)
        {
            _selectedMachine = selectedMachine ?? throw new ArgumentNullException(nameof(selectedMachine));
            _processByPath = processByPath ?? throw new ArgumentNullException(nameof(processByPath));
        }

        /// <inheritdoc />
        public DefaultCommand Value => new()
                                       {
                                           Command = new RelayCommand(_ => Run())
                                       };

        /// <inheritdoc />
        public void Run()
        {
            if (!File.Exists(_selectedMachine.Value.Path))
            {
                return;
            }

            _processByPath.RunFor($"vscode://file/{_selectedMachine.Value.Path}");
        }
    }
}