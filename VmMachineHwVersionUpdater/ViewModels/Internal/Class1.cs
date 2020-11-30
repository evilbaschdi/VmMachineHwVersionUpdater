using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using EvilBaschdi.Core;
using EvilBaschdi.CoreExtended.Controls.About;
using EvilBaschdi.CoreExtended.Mvvm.ViewModel.Command;
using VmMachineHwVersionUpdater.Core.PerMachine;

namespace VmMachineHwVersionUpdater.ViewModels.Internal
{
    /// <inheritdoc />
    public class AboutWindowClickDefaultCommand : ICommandViewModel
    {
        /// <inheritdoc />
        public ICommand Command { get; set; } = new RelayCommand(_ => AboutWindowCommand());


        /// <inheritdoc />
        public string ImagePath{ get; set; }


        /// <inheritdoc />
        public string Text { get; set; } = "about";


        /// <inheritdoc />
        public Visibility Visibility{ get; set; }
       

        private static void AboutWindowCommand()
        {
            var assembly = typeof(MainWindow).Assembly;

            IAboutContent aboutWindowContent =
                new AboutContent(assembly, $@"{AppDomain.CurrentDomain.BaseDirectory}\b.png");
            var aboutWindow = new AboutWindow
                              {
                                  DataContext = new AboutViewModel(aboutWindowContent)
                              };
            aboutWindow.ShowDialog();
        }
    }

  



    public class ArchiveDefaultCommand : ICommandViewModel
    {
        public ICommand Command { get; set; }
        public string ImagePath { get; set; }
        public string Text { get; set; }
        public Visibility Visibility { get; set; }
    }


}
