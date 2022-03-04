using System.Windows;
using EvilBaschdi.Core;
using EvilBaschdi.DependencyInjection;
using JetBrains.Annotations;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VmMachineHwVersionUpdater.Core.DependencyInjection;
using VmMachineHwVersionUpdater.DependencyInjection;
#if !DEBUG
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
        private readonly IHandleAppExit _handleAppExit;
        private readonly IHandleAppStartup<MainWindow> _handleAppStartup;
        private MainWindow _mainWindow;

        /// <inheritdoc />
        public App()
        {
            IHostInstance hostInstance = new HostInstance();
            IValueFor<Action<HostBuilderContext, IServiceCollection>, IServiceProvider> initServiceProviderByHostBuilder = new InitServiceProviderByHostBuilder(hostInstance);

            ServiceProvider = initServiceProviderByHostBuilder.ValueFor(ConfigureServiceCollection);

            _handleAppStartup = new HandleAppStartup<MainWindow>(hostInstance);
            _handleAppExit = new HandleAppExit(hostInstance);
        }

        /// <summary>
        ///     ServiceProvider for DependencyInjection
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        public static IServiceProvider ServiceProvider { get; set; }

        private void ConfigureServiceCollection(HostBuilderContext _, IServiceCollection services)
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

            IConfigureWindowsAndViewModels configureWindowsAndViewModels = new ConfigureWindowsAndViewModels();
            configureWindowsAndViewModels.RunFor(services);
        }

        /// <inheritdoc />
        protected override async void OnStartup(StartupEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }
#if !DEBUG
            ThemeManager.Current.SyncTheme(ThemeSyncMode.SyncAll);
#endif

            _mainWindow = await _handleAppStartup.ValueForAsync(ServiceProvider);
            _mainWindow.Show();
        }

        /// <inheritdoc />
        protected override async void OnExit([NotNull] ExitEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            await _handleAppExit.RunAsync();

            base.OnExit(e);
        }
    }
}