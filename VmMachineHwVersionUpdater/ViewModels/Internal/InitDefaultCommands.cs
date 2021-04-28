using System;
using EvilBaschdi.Core;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using VmMachineHwVersionUpdater.Core.PerMachine;

namespace VmMachineHwVersionUpdater.ViewModels.Internal
{
    /// <inheritdoc />
    public class InitDefaultCommands : IInitDefaultCommands
    {
        private readonly IAboutWindowClickDefaultCommand _aboutWindowClickDefaultCommand;
        private readonly IAddEditAnnotationDefaultCommand _addEditAnnotationDefaultCommand;
        private readonly IArchiveDefaultCommand _archiveDefaultCommand;
        private readonly IDeleteDefaultCommand _deleteDefaultCommand;
        private readonly IGotToDefaultCommand _gotToDefaultCommand;
        private readonly IOpenWithCodeDefaultCommand _openWithCodeDefaultCommand;
        private readonly IReloadDefaultCommand _reloadDefaultCommand;
        private readonly IStartDefaultCommand _startDefaultCommand;
        private readonly IUpdateAllDefaultCommand _updateAllDefaultCommand;


        /// <summary>
        ///     Constructor
        /// </summary>
        public InitDefaultCommands([NotNull] IAboutWindowClickDefaultCommand aboutWindowClickDefaultCommand,
                                   [NotNull] IArchiveDefaultCommand archiveDefaultCommand,
                                   [NotNull] IOpenWithCodeDefaultCommand openWithCodeDefaultCommand,
                                   [NotNull] IAddEditAnnotationDefaultCommand addEditAnnotationDefaultCommand,
                                   [NotNull] IDeleteDefaultCommand deleteDefaultCommand,
                                   [NotNull] IGotToDefaultCommand gotToDefaultCommand,
                                   [NotNull] IReloadDefaultCommand reloadDefaultCommand,
                                   [NotNull] IStartDefaultCommand startDefaultCommand,
                                   [NotNull] IUpdateAllDefaultCommand updateAllDefaultCommand
        )
        {
            _aboutWindowClickDefaultCommand = aboutWindowClickDefaultCommand ?? throw new ArgumentNullException(nameof(aboutWindowClickDefaultCommand));
            _archiveDefaultCommand = archiveDefaultCommand ?? throw new ArgumentNullException(nameof(archiveDefaultCommand));
            _openWithCodeDefaultCommand = openWithCodeDefaultCommand ?? throw new ArgumentNullException(nameof(openWithCodeDefaultCommand));
            _addEditAnnotationDefaultCommand = addEditAnnotationDefaultCommand ?? throw new ArgumentNullException(nameof(addEditAnnotationDefaultCommand));
            _deleteDefaultCommand = deleteDefaultCommand ?? throw new ArgumentNullException(nameof(deleteDefaultCommand));
            _gotToDefaultCommand = gotToDefaultCommand ?? throw new ArgumentNullException(nameof(gotToDefaultCommand));
            _reloadDefaultCommand = reloadDefaultCommand ?? throw new ArgumentNullException(nameof(reloadDefaultCommand));
            _startDefaultCommand = startDefaultCommand ?? throw new ArgumentNullException(nameof(startDefaultCommand));
            _updateAllDefaultCommand = updateAllDefaultCommand ?? throw new ArgumentNullException(nameof(updateAllDefaultCommand));
        }

        /// <inheritdoc />
        public IAboutWindowClickDefaultCommand AboutWindowClickDefaultCommand { get; set; }

        /// <inheritdoc />
        public IArchiveDefaultCommand ArchiveDefaultCommand { get; set; }

        /// <inheritdoc />
        public IOpenWithCodeDefaultCommand OpenWithCodeDefaultCommand { get; set; }

        /// <inheritdoc />
        public IAddEditAnnotationDefaultCommand AddEditAnnotationDefaultCommand { get; set; }

        /// <inheritdoc />
        public IDeleteDefaultCommand DeleteDefaultCommand { get; set; }

        /// <inheritdoc />
        public IGotToDefaultCommand GotToDefaultCommand { get; set; }

        /// <inheritdoc />
        public IReloadDefaultCommand ReloadDefaultCommand { get; set; }

        /// <inheritdoc />
        public IStartDefaultCommand StartDefaultCommand { get; set; }

        /// <inheritdoc />
        public IUpdateAllDefaultCommand UpdateAllDefaultCommand { get; set; }

        /// <inheritdoc />
        public object DialogCoordinatorContext { get; set; }

        /// <inheritdoc />
        public void Run()
        {
            AboutWindowClickDefaultCommand = _aboutWindowClickDefaultCommand;
            ArchiveDefaultCommand = _archiveDefaultCommand;
            OpenWithCodeDefaultCommand = _openWithCodeDefaultCommand;
            AddEditAnnotationDefaultCommand = _addEditAnnotationDefaultCommand;
            ReloadDefaultCommand = _reloadDefaultCommand;
            DeleteDefaultCommand = _deleteDefaultCommand;
            GotToDefaultCommand = _gotToDefaultCommand;
            StartDefaultCommand = _startDefaultCommand;
            UpdateAllDefaultCommand = _updateAllDefaultCommand;

            ArchiveDefaultCommand.DialogCoordinatorContext = DialogCoordinatorContext;
            DeleteDefaultCommand.DialogCoordinatorContext = DialogCoordinatorContext;
            ReloadDefaultCommand.DialogCoordinatorContext = DialogCoordinatorContext;
        }
    }

    /// <inheritdoc />
    public interface IConfigureDefaultCommandServices : IRunFor<IServiceCollection>
    {
    }

    /// <inheritdoc />
    public class ConfigureDefaultCommandServices : IConfigureDefaultCommandServices
    {
        /// <inheritdoc />
        public void RunFor([NotNull] IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddSingleton<IAddEditAnnotation, AddEditAnnotation>();
            services.AddSingleton<IAboutWindowClickDefaultCommand, AboutWindowClickDefaultCommand>();
            services.AddSingleton<IArchiveDefaultCommand, ArchiveDefaultCommand>();
            services.AddSingleton<IOpenWithCodeDefaultCommand, OpenWithCodeDefaultCommand>();
            services.AddSingleton<IAddEditAnnotationDefaultCommand, AddEditAnnotationDefaultCommand>();
            services.AddSingleton<IReloadDefaultCommand, ReloadDefaultCommand>();
            services.AddSingleton<IDeleteDefaultCommand, DeleteDefaultCommand>();
            services.AddSingleton<IGotToDefaultCommand, GotToDefaultCommand>();
            services.AddSingleton<IStartDefaultCommand, StartDefaultCommand>();
            services.AddSingleton<IUpdateAllDefaultCommand, UpdateAllDefaultCommand>();
        }
    }
}