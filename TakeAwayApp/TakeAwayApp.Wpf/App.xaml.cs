using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace TakeAwayApp.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
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

            DeviceSelectorWindow mainWindow = new DeviceSelectorWindow();
            mainWindow.Show();

            //FlavourBusinessApps.WaiterApp.WPF.WaiterApp.Startup();

            //MainWindow mainWindow = new MainWindow();
            //mainWindow.Show();

            var sds = Fraction.RealToFraction(0.8232323343, 0.01);

            base.OnStartup(e);
        }


        public struct Fraction
        {
            public Fraction(int n, int d)
            {
                N = n;
                D = d;
            }

            public int N { get; private set; }
            public int D { get; private set; }

            public static Fraction RealToFraction(double value, double accuracy)
            {
                if (accuracy <= 0.0 || accuracy >= 1.0)
                {
                    throw new ArgumentOutOfRangeException("accuracy", "Must be > 0 and < 1.");
                }

                int sign = Math.Sign(value);

                if (sign == -1)
                {
                    value = Math.Abs(value);
                }

                // Accuracy is the maximum relative error; convert to absolute maxError
                double maxError = sign == 0 ? accuracy : value * accuracy;

                int n = (int)Math.Floor(value);
                value -= n;

                if (value < maxError)
                {
                    return new Fraction(sign * n, 1);
                }

                if (1 - maxError < value)
                {
                    return new Fraction(sign * (n + 1), 1);
                }

                // The lower fraction is 0/1
                int lower_n = 0;
                int lower_d = 1;

                // The upper fraction is 1/1
                int upper_n = 1;
                int upper_d = 1;

                while (true)
                {
                    // The middle fraction is (lower_n + upper_n) / (lower_d + upper_d)
                    int middle_n = lower_n + upper_n;
                    int middle_d = lower_d + upper_d;

                    if (middle_d * (value + maxError) < middle_n)
                    {
                        // real + error < middle : middle is our new upper
                        upper_n = middle_n;
                        upper_d = middle_d;
                    }
                    else if (middle_n < (value - maxError) * middle_d)
                    {
                        // middle < real - error : middle is our new lower
                        lower_n = middle_n;
                        lower_d = middle_d;
                    }
                    else
                    {
                        // Middle is our best fraction
                        return new Fraction((n * middle_d + middle_n) * sign, middle_d);
                    }
                }
            }
        }

        /// <MetaDataID>{071f269f-6ca6-455d-a0b1-e6ba28ee8acd}</MetaDataID>
        protected override void OnActivated(EventArgs e)
        {
            OOAdvantech.Remoting.RestApi.Authentication.InitializeFirebase("demomicroneme");
            base.OnActivated(e);
        }

    }
}
