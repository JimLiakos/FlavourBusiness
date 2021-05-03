using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using NotifyIconWpf;

namespace CashierStationDTDevice
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// <MetaDataID>{434331bc-73af-41f9-9edf-a2766af5435b}</MetaDataID>
    public partial class App : Application
    {
        private TaskbarIcon notifyIcon;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //create the notifyicon (it's a resource declared in NotifyIconResources.xaml
            notifyIcon = (TaskbarIcon)FindResource("NotifyIcon");

            CashierStationDevice.DocumentSignDevice.Init();

        }

        protected override void OnExit(ExitEventArgs e)
        {
            notifyIcon.Dispose(); //the icon would clean up automatically, but this is cleaner
            base.OnExit(e);
        }
    }
}
