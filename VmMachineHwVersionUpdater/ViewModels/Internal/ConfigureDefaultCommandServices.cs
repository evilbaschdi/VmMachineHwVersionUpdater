using System;
using EvilBaschdi.CoreExtended.Controls.About;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using VmMachineHwVersionUpdater.Core.PerMachine;

namespace VmMachineHwVersionUpdater.ViewModels.Internal
{
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
            services.AddSingleton<IArchiveDefaultCommand, ArchiveDefaultCommand>();
            services.AddSingleton<ICopyDefaultCommand, CopyDefaultCommand>();
            services.AddSingleton<IOpenWithCodeDefaultCommand, OpenWithCodeDefaultCommand>();
            services.AddSingleton<IAddEditAnnotationDefaultCommand, AddEditAnnotationDefaultCommand>();
            services.AddSingleton<IReloadDefaultCommand, ReloadDefaultCommand>();
            services.AddSingleton<IDeleteDefaultCommand, DeleteDefaultCommand>();
            services.AddSingleton<IGotToDefaultCommand, GotToDefaultCommand>();
            services.AddSingleton<IStartDefaultCommand, StartDefaultCommand>();
            services.AddSingleton<IUpdateAllDefaultCommand, UpdateAllDefaultCommand>();
            services.AddSingleton<IAboutWindowClickDefaultCommand, AboutWindowClickDefaultCommand>();
            services.AddSingleton<IAboutModel, AboutViewModel>();
            services.AddSingleton<IAboutContent, AboutContent>(AboutContentImplementation);
            services.AddSingleton(AboutWindowImplementation);
        }

        /// <summary>
        ///     Implementation of <see cref="AboutContent" />
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public AboutContent AboutContentImplementation(IServiceProvider serviceProvider)
        {
            var assembly = typeof(MainWindow).Assembly;
            return new AboutContent(assembly, $@"{AppDomain.CurrentDomain.BaseDirectory}\b.png");
        }

        /// <summary>
        ///     Implementation of <see cref="AboutWindow" />
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public AboutWindow AboutWindowImplementation(IServiceProvider serviceProvider)
        {
            var aboutModel = serviceProvider.GetService<IAboutModel>();

            return new()
                   {
                       DataContext = aboutModel
                   };
        }
    }
}