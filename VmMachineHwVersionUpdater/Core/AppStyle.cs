using System;
using System.Windows;
using System.Windows.Controls;
using MahApps.Metro;

namespace VmMachineHwVersionUpdater.Core
{
    /// <summary>
    /// </summary>
    public class AppStyle : IAppStyle
    {
        /// <summary>
        ///     Accent of Application Style.
        /// </summary>
        private Accent _styleAccent = ThemeManager.DetectAppStyle(Application.Current).Item2;

        /// <summary>
        ///     Theme of Application Style.
        /// </summary>
        private AppTheme _styleTheme = ThemeManager.DetectAppStyle(Application.Current).Item1;

        private readonly MainWindow _mainWindow;
        private readonly IAppSettings _appSettings;

        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.
        /// </summary>
        /// <param name="mainWindow"></param>
        /// <param name="appSettings"></param>
        public AppStyle(MainWindow mainWindow, IAppSettings appSettings)
        {
            if(mainWindow == null)
            {
                throw new ArgumentNullException(nameof(mainWindow));
            }
            if(appSettings == null)
            {
                throw new ArgumentNullException(nameof(appSettings));
            }
            _mainWindow = mainWindow;
            _appSettings = appSettings;
        }

        /// <summary>
        /// </summary>
        public void Load()
        {
            _mainWindow.Width = SystemParameters.PrimaryScreenWidth - 400;
            _mainWindow.Height = SystemParameters.PrimaryScreenHeight - 400;
            _mainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            if(!string.IsNullOrWhiteSpace(_appSettings.Accent))
            {
                _styleAccent = ThemeManager.GetAccent(_appSettings.Accent);
            }
            if(!string.IsNullOrWhiteSpace(_appSettings.Theme))
            {
                _styleTheme = ThemeManager.GetAppTheme(_appSettings.Theme);
            }

            _mainWindow.Accent.SelectedValue = _styleAccent.Name;

            switch(_styleTheme.Name)
            {
                case "BaseDark":
                    _mainWindow.Dark.IsChecked = true;
                    _mainWindow.Light.IsChecked = false;
                    break;

                case "BaseLight":
                    _mainWindow.Dark.IsChecked = false;
                    _mainWindow.Light.IsChecked = true;
                    break;
            }

            SetStyle();

            foreach(var accent in ThemeManager.Accents)
            {
                _mainWindow.Accent.Items.Add(accent.Name);
            }
        }

        /// <summary>
        /// </summary>
        private void SetStyle()
        {
            ThemeManager.ChangeAppStyle(Application.Current, _styleAccent, _styleTheme);
        }

        /// <summary>
        ///     Accent of application style.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SetAccent(object sender, SelectionChangedEventArgs e)
        {
            _styleAccent = ThemeManager.GetAccent(_mainWindow.Accent.SelectedValue.ToString());
            SetStyle();
        }

        /// <summary>
        ///     Theme of application style.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="routedEventArgs"></param>
        public void SetTheme(object sender, RoutedEventArgs routedEventArgs)
        {
            // get the theme from the current application
            var style = ThemeManager.DetectAppStyle(Application.Current);

            var radiobutton = (RadioButton) sender;
            _styleTheme = style.Item1;

            switch(radiobutton.Name)
            {
                case "Dark":
                    _styleTheme = ThemeManager.GetAppTheme("BaseDark");
                    break;

                case "Light":
                    _styleTheme = ThemeManager.GetAppTheme("BaseLight");
                    break;

                default:
                    _styleTheme = style.Item1;
                    break;
            }

            SetStyle();
        }

        /// <summary>
        /// </summary>
        public void SaveStyle()
        {
            _appSettings.Accent = _styleAccent.Name;
            _appSettings.Theme = _styleTheme.Name;
        }
    }
}