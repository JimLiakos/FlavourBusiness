using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DontWaitApp
{
    /// <summary>
    /// Interaction logic for DeviceSelectorWindow.xaml
    /// </summary>
    /// <MetaDataID>{1eb60a4f-9ada-4820-8d2c-371798fae1e7}</MetaDataID>
    public partial class DeviceSelectorWindow : Window
    {
        public DeviceSelectorWindow()
        {
            InitializeComponent();
            SelectedIndex = 0;


            DataContext = this;
        }


        public List<string> DevicesIDS
        {
            get
            {
                return new List<string>() { "91000000296", "93000000296", "95000000296", "97000000296" };
            }

        }

        public int SelectedIndex { get; set; }



        protected override void OnClosing(CancelEventArgs e)
        {
            //DisplayOpenPorts();
            OOAdvantech.Net.DeviceOOAdvantechCore.DebugDeviceID = DevicesIDS[SelectedIndex];
            ApplicationSettings.ExtraStoragePath = DevicesIDS[SelectedIndex];
            GenWebBrowser.WebBrowserOverlay.SetCefExtraCachePath(DevicesIDS[SelectedIndex]);
            //DisplayOpenPorts();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            base.OnClosing(e);
        }

       void DisplayOpenPorts()
        {
            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();

            IPEndPoint[] endPoints = ipProperties.GetActiveTcpListeners();
            TcpConnectionInformation[] tcpConnections =
                ipProperties.GetActiveTcpConnections();

            foreach (TcpConnectionInformation info in tcpConnections)
            {
                if (info.LocalEndPoint.Port == 9222)
                {
                    System.Diagnostics.Debug.WriteLine("Local: {0}:{1}\nRemote: {2}:{3}\nState: {4}\n",
                        info.LocalEndPoint.Address, info.LocalEndPoint.Port,
                        info.RemoteEndPoint.Address, info.RemoteEndPoint.Port,
                        info.State.ToString());
                }
            }
        }
    }
}
