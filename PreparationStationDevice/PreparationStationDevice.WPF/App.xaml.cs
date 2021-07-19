using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using OOAdvantech.Remoting.RestApi.Serialization;

namespace PreparationStationDevice.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// <MetaDataID>{dc5867f9-72db-4125-8c62-4f0391370c3e}</MetaDataID>
    public partial class App : Application, OOAdvantech.IAppLifeTime
    {
        public static OOAdvantech.SerializeTaskScheduler SerializeTaskScheduler = new OOAdvantech.SerializeTaskScheduler();

        public event EventHandler ApplicationResuming;
        public event EventHandler ApplicationSleeping;
        OOAdvantech.SerializeTaskScheduler OOAdvantech.IAppLifeTime.SerializeTaskScheduler => SerializeTaskScheduler;



        protected override void OnStartup(StartupEventArgs e)
        {
            SerializeTaskScheduler.RunAsync();

            DeviceSelectorWindow mainWindow = new DeviceSelectorWindow();
            mainWindow.Show();

            base.OnStartup(e);
        }
    }
}
