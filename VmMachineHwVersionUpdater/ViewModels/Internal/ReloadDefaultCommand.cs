using System;
using EvilBaschdi.CoreExtended.Mvvm.ViewModel.Command;
using JetBrains.Annotations;

namespace VmMachineHwVersionUpdater.ViewModels.Internal
{
    /// <inheritdoc />
    public class ReloadDefaultCommand : IReloadDefaultCommand
    {
        private readonly IInit _init;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="init"></param>
        public ReloadDefaultCommand([NotNull] IInit init)
        {
            _init = init ?? throw new ArgumentNullException(nameof(init));
        }

        /// <inheritdoc />
        public DefaultCommand Value => new()
                                       {
                                           Command = new RelayCommand(_ => Run())
                                       };

        /// <inheritdoc />
        public void Run()
        {
            _init.Run();
        }
    }
}