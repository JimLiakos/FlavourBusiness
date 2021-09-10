using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using OOAdvantech.Remoting.RestApi.Serialization;

namespace ServiceContextManagerApp.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// <MetaDataID>{dd711854-5598-4817-8d0c-1d729c403638}</MetaDataID>
    public partial class App : Application, OOAdvantech.IAppLifeTime
    {
        public static OOAdvantech.SerializeTaskScheduler SerializeTaskScheduler = new OOAdvantech.SerializeTaskScheduler();


        public event EventHandler ApplicationResuming;
        public event EventHandler ApplicationSleeping;
        OOAdvantech.SerializeTaskScheduler OOAdvantech.IAppLifeTime.SerializeTaskScheduler => SerializeTaskScheduler;


        protected override void OnStartup(StartupEventArgs e)
        {
            SerializeTaskScheduler.RunAsync();

            FlavourBusinessApps.ServiceContextManagerApp.WPF.ServiceContextManagerApp.Startup();

            ServiceContextManagerApp.WPF.MainWindow mainWindow = new MainWindow();
            mainWindow.Show();


            base.OnStartup(e);
        }

        protected override void OnActivated(EventArgs e)
        {
            OOAdvantech.Remoting.RestApi.Authentication.InitializeFirebase("demomicroneme");
            base.OnActivated(e);
        }
    }
}
