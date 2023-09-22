using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CourierApp.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, OOAdvantech.IAppLifeTime
    {
        public static OOAdvantech.SerializeTaskScheduler SerializeTaskScheduler = new OOAdvantech.SerializeTaskScheduler();
        OOAdvantech.SerializeTaskScheduler OOAdvantech.IAppLifeTime.SerializeTaskScheduler => SerializeTaskScheduler;
        public event EventHandler ApplicationResuming;
        public event EventHandler ApplicationSleeping;
        protected override void OnStartup(StartupEventArgs e)
        {

            SerializeTaskScheduler.RunAsync();

            DeviceSelectorWindow mainWindow = new DeviceSelectorWindow();
            mainWindow.Show();

            //FlavourBusinessApps.WaiterApp.WPF.WaiterApp.Startup();

            //MainWindow mainWindow = new MainWindow();
            //mainWindow.Show();

           // var sds = Fraction.RealToFraction(0.8232323343, 0.01);

            base.OnStartup(e);
        }


        protected override void OnActivated(EventArgs e)
        {
            OOAdvantech.Remoting.RestApi.Authentication.InitializeFirebase("demomicroneme");
            base.OnActivated(e);
        }
    }
}
