using System;
using System.Windows;
using JetBrains.Annotations;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VmMachineHwVersionUpdater.Core;
using VmMachineHwVersionUpdater.ViewModels;
using VmMachineHwVersionUpdater.ViewModels.Internal;
#if (!DEBUG)
using ControlzEx.Theming;

#endif

namespace VmMachineHwVersionUpdater
{
    /// <inheritdoc />
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    // ReSharper disable once RedundantExtendsListEntry
    public partial class App : Application
    {
        private readonly IHost _host;

        /// <inheritdoc />
        public App()
        {
            _host = Host.CreateDefaultBuilder()
                        .ConfigureServices((_, services) => { ConfigureServices(services); })
                        .Build();

            ServiceProvider = _host.Services;
        }

        /// <summary>
        ///     ServiceProvider for DependencyInjection
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        public static IServiceProvider ServiceProvider { get; set; }


        /// <inheritdoc />
        protected override async void OnStartup(StartupEventArgs e)
        {
#if (!DEBUG)
            ThemeManager.Current.SyncTheme(ThemeSyncMode.SyncAll);
#endif

            await _host.StartAsync();

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices([NotNull] IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddSingleton(_ => DialogCoordinator.Instance);

            IConfigureWpfServices configureWpfServices = new ConfigureWpfServices();
            configureWpfServices.RunFor(services);

            IConfigureCoreServices configureCoreServices = new ConfigureCoreServices();
            configureCoreServices.RunFor(services);

            IConfigureDefaultCommandServices configureDefaultCommandServices = new ConfigureDefaultCommandServices();
            configureDefaultCommandServices.RunFor(services);

            services.AddSingleton<AddEditAnnotationDialogViewModel>();
            services.AddTransient(typeof(AddEditAnnotationDialog));

            services.AddSingleton<MainWindowViewModel>();
            services.AddTransient(typeof(MainWindow));
        }

        /// <inheritdoc />
        protected override async void OnExit([NotNull] ExitEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            //todo dispose

            using (_host)
            {
                await _host.StopAsync(TimeSpan.FromSeconds(5));
            }

            base.OnExit(e);
        }
    }
}