using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using NotifyIconWpf;
using ROOT.CIMV2.Win32;

namespace CallerIDDevice
{
    /// <summary>
    /// Simple application. Check the XAML for comments.
    /// </summary>
    /// <MetaDataID>{6b5310ca-6097-4835-b902-04751c62edd7}</MetaDataID>
    public partial class App : Application
    {
        private TaskbarIcon notifyIcon;
        Tapi2CallerID Tapi2CallerID;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);




            //create the notifyicon (it's a resource declared in NotifyIconResources.xaml
            notifyIcon = (TaskbarIcon)FindResource("NotifyIcon");

            Tapi2CallerID = new Tapi2CallerID();

            foreach (POTSModem modem in POTSModem.GetInstances())
            {
                Console.WriteLine(modem.Description);


            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            notifyIcon.Dispose(); //the icon would clean up automatically, but this is cleaner
            base.OnExit(e);
        }
    }
}
