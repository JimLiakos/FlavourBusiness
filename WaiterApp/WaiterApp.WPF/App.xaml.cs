using DontWaitApp;
using OOAdvantech.Remoting.RestApi.Serialization;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace WaiterApp.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// <MetaDataID>{fe73c970-735e-480d-860c-2ffa544889ea}</MetaDataID>
    public partial class App : Application, OOAdvantech.IAppLifeTime
    {
        public static OOAdvantech.SerializeTaskScheduler SerializeTaskScheduler = new OOAdvantech.SerializeTaskScheduler();


        public event EventHandler ApplicationResuming;
        public event EventHandler ApplicationSleeping;
        OOAdvantech.SerializeTaskScheduler OOAdvantech.IAppLifeTime.SerializeTaskScheduler => SerializeTaskScheduler;


        /// <MetaDataID>{423722b5-0ce7-43a4-905c-a2df1aa4ccd0}</MetaDataID>
        protected override void OnStartup(StartupEventArgs e)
        {

            SerializeTaskScheduler.RunAsync();

            FlavourBusinessApps.WaiterApp.WPF.WaiterApp.Startup();

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();

            base.OnStartup(e);
        }

        /// <MetaDataID>{071f269f-6ca6-455d-a0b1-e6ba28ee8acd}</MetaDataID>
        protected override void OnActivated(EventArgs e)
        {
            OOAdvantech.Remoting.RestApi.Authentication.InitializeFirebase("demomicroneme");
            base.OnActivated(e);
        }
    }
}
