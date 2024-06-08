using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PrinterEmulator
{
    /// <summary>
    /// Provides bindable properties and commands for the NotifyIcon. In this sample, the
    /// view model is assigned to the NotifyIcon in XAML. Alternatively, the startup routing
    /// in App.xaml.cs could have created this view model, and assigned it to the NotifyIcon.
    /// </summary>
    /// <MetaDataID>{5edf56f2-57b6-456c-803d-bf36c0b3f005}</MetaDataID>
    public class NotifyIconViewModel
    {

        /// <MetaDataID>{a4aef359-3bbd-4d8c-94f1-3eafa13088da}</MetaDataID>
        public NotifyIconViewModel()
        {
        }

        /// <MetaDataID>{50ef7321-c75a-4ff0-9861-e7a4e8fdfe70}</MetaDataID>
        private void CurrentDocumentSignDevice_DeviceStatusChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Shows a window, if none is already open.
        /// </summary>
        /// <MetaDataID>{87d29262-4fc9-43f4-843e-6bdddbeb0246}</MetaDataID>
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
        /// <MetaDataID>{0a687de0-51cf-4f7f-8f84-30b8c659f9b9}</MetaDataID>
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
        /// <MetaDataID>{a9346726-a5e5-472d-b630-2ec8841fc77b}</MetaDataID>
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
        /// <MetaDataID>{aeb03afe-a492-4085-8154-5758e7cd85a5}</MetaDataID>
        public Action CommandAction { get; set; }
        /// <MetaDataID>{0ca86a5e-9e97-493d-b806-5e2fc9bf72ab}</MetaDataID>
        public Func<bool> CanExecuteFunc { get; set; }

        /// <MetaDataID>{853ca41e-6f8d-4d2c-a535-48836c6685dd}</MetaDataID>
        public void Execute(object parameter)
        {
            CommandAction();
        }

        /// <MetaDataID>{fe25b2b6-00d2-475b-b103-47b8171d837d}</MetaDataID>
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
