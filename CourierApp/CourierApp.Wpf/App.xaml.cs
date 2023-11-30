using OOAdvantech;
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
            var _this = this;
           var task1= SerializeTaskScheduler.AddTask(()=>{


                System.Diagnostics.Debug.WriteLine(System.Threading.Thread.CurrentThread.ManagedThreadId);
                System.Threading.Thread.Sleep(1000);
                var ere= _this.GetType().FullName;
                return true;

            });


            var task2 = SerializeTaskScheduler.AddTask(() => {


                System.Diagnostics.Debug.WriteLine(System.Threading.Thread.CurrentThread.ManagedThreadId);
                System.Threading.Thread.Sleep(1000);
                var ere = _this.GetType().FullName;

                return true;

            });
            var task3 = SerializeTaskScheduler.AddTask(() => {


                System.Diagnostics.Debug.WriteLine(System.Threading.Thread.CurrentThread.ManagedThreadId);
                System.Threading.Thread.Sleep(1000);
                var ere = _this.GetType().FullName;

                return true;

            });
            task1.Wait();
            int rtr = 0;
            task2.Wait();
            rtr = 2;
            task3.Wait();
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
