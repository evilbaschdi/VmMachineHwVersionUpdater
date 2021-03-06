﻿using System;
using System.Threading.Tasks;
using System.Windows;
using EvilBaschdi.CoreExtended.AppHelpers;
using EvilBaschdi.CoreExtended.Mvvm.ViewModel.Command;
using JetBrains.Annotations;
using MahApps.Metro.Controls.Dialogs;

namespace VmMachineHwVersionUpdater.ViewModels.Internal
{
    /// <inheritdoc />
    public class ReloadDefaultCommand : IReloadDefaultCommand
    {
        [NotNull] private readonly IDialogCoordinator _instance;
        private readonly IProcessByPath _processByPath;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="processByPath"></param>
        public ReloadDefaultCommand([NotNull] IDialogCoordinator instance, [NotNull] IProcessByPath processByPath)
        {
            _instance = instance ?? throw new ArgumentNullException(nameof(instance));
            _processByPath = processByPath ?? throw new ArgumentNullException(nameof(processByPath));
        }

        /// <inheritdoc />
        public DefaultCommand Value => new()
                                       {
                                           Command = new RelayCommand(async _ => await RunAsync())
                                       };

        /// <inheritdoc />
        public async Task RunAsync()
        {
            var controller = await _instance.ShowProgressAsync(DialogCoordinatorContext, "Application is restarting", "Please wait...");
            controller.SetIndeterminate();
            if (controller.IsOpen)
            {
                await Task.Run(() =>
                               {
                                   var app = Application.ResourceAssembly.Location.Replace("dll", "exe");
                                   var process = _processByPath.ValueFor(app);
                                   process.Start();
                                   process.WaitForInputIdle();
                               });

                await controller.CloseAsync();
                Application.Current.MainWindow?.Close();
            }
        }

        /// <inheritdoc />
        public object DialogCoordinatorContext { get; set; }
    }
}