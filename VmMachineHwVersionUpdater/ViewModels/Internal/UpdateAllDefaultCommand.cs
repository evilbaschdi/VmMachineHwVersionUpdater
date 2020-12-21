using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Shell;
using EvilBaschdi.CoreExtended.Mvvm.ViewModel.Command;
using JetBrains.Annotations;

namespace VmMachineHwVersionUpdater.ViewModels.Internal
{
    /// <inheritdoc />
    public class UpdateAllDefaultCommand : IUpdateAllDefaultCommand
    {
        private readonly ICurrentItemSource _currentItemSource;
        private readonly IInit _init;
        private readonly ITaskbarItemProgressState _taskbarItemProgressState;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="init"></param>
        /// <param name="currentItemSource"></param>
        /// <param name="taskbarItemProgressState"></param>
        public UpdateAllDefaultCommand([NotNull] IInit init, [NotNull] ICurrentItemSource currentItemSource, [NotNull] ITaskbarItemProgressState taskbarItemProgressState)
        {
            _init = init ?? throw new ArgumentNullException(nameof(init));
            _currentItemSource = currentItemSource ?? throw new ArgumentNullException(nameof(currentItemSource));
            _taskbarItemProgressState = taskbarItemProgressState ?? throw new ArgumentNullException(nameof(taskbarItemProgressState));
        }

        /// <inheritdoc />
        public DefaultCommand Value => new()
                                       {
                                           Command = new RelayCommand(async _ => await RunTask())
                                       };

        /// <inheritdoc />
        public async Task RunTask()
        {
            _taskbarItemProgressState.Value = TaskbarItemProgressState.Indeterminate;

            var task = Task.Factory.StartNew(Run);
            await task.ConfigureAwait(true);

            _taskbarItemProgressState.Value = TaskbarItemProgressState.Normal;

            _init.Run();
        }

        /// <inheritdoc />
        public void Run()
        {
            var version = _init.Load.Value.UpdateAllHwVersion;
            if (!version.HasValue)
            {
                return;
            }

            var innerVersion = Convert.ToInt32(version.Value);
            var localList = _currentItemSource.Value.AsParallel().Where(vm => vm.HwVersion != innerVersion).ToList();
            _init.UpdateMachineVersion.RunFor(localList, innerVersion);
        }
    }
}