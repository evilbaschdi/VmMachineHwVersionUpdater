using System;
using EvilBaschdi.CoreExtended.Controls.About;
using EvilBaschdi.CoreExtended.Mvvm.ViewModel.Command;
using JetBrains.Annotations;

namespace VmMachineHwVersionUpdater.ViewModels.Internal
{
    /// <inheritdoc />
    public class AboutWindowClickDefaultCommand : IAboutWindowClickDefaultCommand
    {
        private readonly AboutWindow _aboutWindow;
        //private readonly IRoundCorners _roundCorners;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="aboutWindow"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public AboutWindowClickDefaultCommand([NotNull] AboutWindow aboutWindow)
        {
            _aboutWindow = aboutWindow ?? throw new ArgumentNullException(nameof(aboutWindow));
        }

        /// <inheritdoc />
        public void Run()
        {
            _aboutWindow.ShowDialog();
        }

        /// <inheritdoc />
        public DefaultCommand Value => new()
                                       {
                                           Command = new RelayCommand(_ => Run())
                                       };
    }
}