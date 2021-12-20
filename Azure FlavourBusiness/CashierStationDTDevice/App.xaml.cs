using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CashierStationDevice;
using FlavourBusinessFacade.ServicesContextResources;
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

        public CashierController CashierController { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            //create the notifyicon (it's a resource declared in NotifyIconResources.xaml
            notifyIcon = (TaskbarIcon)FindResource("NotifyIcon");
            OOAdvantech.Remoting.RestApi.Authentication.InitializeFirebase("demomicroneme");
            CashierStationDevice.DocumentSignDevice.Init();

            CashierController = new CashierStationDevice.CashierController();
            CashierController = new CashierStationDevice.CashierController();

            try
            {
                CashierController.Start();
            }
            catch (CashierStationDeviceException error)
            {

                System.Windows.Application.Current.Shutdown();
            }


        }



        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            notifyIcon.Dispose(); //the icon would clean up automatically, but this is cleaner
            base.OnExit(e);
        }
    }
}
