using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CashierStationDTDevice
{
    /// <summary>
    /// Provides bindable properties and commands for the NotifyIcon. In this sample, the
    /// view model is assigned to the NotifyIcon in XAML. Alternatively, the startup routing
    /// in App.xaml.cs could have created this view model, and assigned it to the NotifyIcon.
    /// </summary>
    /// <MetaDataID>{5edf56f2-57b6-456c-803d-bf36c0b3f005}</MetaDataID>
    public class NotifyIconViewModel
    {

        public NotifyIconViewModel()
        {
            CashierStationDevice.DocumentSignDevice.CurrentDocumentSignDevice.DeviceStatusChanged += CurrentDocumentSignDevice_DeviceStatusChanged;
        }

        private void CurrentDocumentSignDevice_DeviceStatusChanged(object sender, CashierStationDevice.DocumentSignDevice.DeviceStatus e)
        {
            var messages = CashierStationDevice.DocumentSignDevice.CurrentDocumentSignDevice.CheckStatusForError();
            if(messages.Count==0)
                System.Diagnostics.Debug.WriteLine("OK");

            foreach (string message in CashierStationDevice.DocumentSignDevice.CurrentDocumentSignDevice.CheckStatusForError())
                System.Diagnostics.Debug.WriteLine(message);
            
        }

        /// <summary>
        /// Shows a window, if none is already open.
        /// </summary>
        public ICommand ShowWindowCommand
        {
            get
            {
                return new DelegateCommand
                {
                    CanExecuteFunc = () => Application.Current.MainWindow == null,
                    CommandAction = () =>
                    {
                        Application.Current.MainWindow = new MainWindow();
                        Application.Current.MainWindow.Show();
                    }
                };
            }
        }

        /// <summary>
        /// Hides the main window. This command is only enabled if a window is open.
        /// </summary>
        public ICommand HideWindowCommand
        {
            get
            {
                return new DelegateCommand
                {
                    CommandAction = () => Application.Current.MainWindow.Close(),
                    CanExecuteFunc = () => Application.Current.MainWindow != null
                };
            }
        }


        /// <summary>
        /// Shuts down the application.
        /// </summary>
        public ICommand ExitApplicationCommand
        {
            get
            {
                return new DelegateCommand { CommandAction = () => Application.Current.Shutdown() };
            }
        }
    }


    /// <summary>
    /// Simplistic delegate command for the demo.
    /// </summary>
    /// <MetaDataID>{4ee4bda6-7a33-488b-818b-4b87dca9ba69}</MetaDataID>
    public class DelegateCommand : ICommand
    {
        public Action CommandAction { get; set; }
        public Func<bool> CanExecuteFunc { get; set; }

        public void Execute(object parameter)
        {
            CommandAction();
        }

        public bool CanExecute(object parameter)
        {
            return CanExecuteFunc == null || CanExecuteFunc();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
