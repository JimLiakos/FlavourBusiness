using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
using TakeAwayApp.ViewModel;

namespace TakeAwayApp.Wpf
{
    /// <summary>
    /// Interaction logic for DeviceSelectorWindow.xaml
    /// </summary>
    /// <MetaDataID>{cb79d129-129a-4a28-9957-b44292612d54}</MetaDataID>
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
                return new List<string>() { "61000000296", "62000000296", "65000000296", "66000000296" };
            }

        }

        public int SelectedIndex { get; set; }
        public HomeDeliveryUnitTest HomeDeliveryUnitTest { get; private set; }

        protected override async void OnClosing(CancelEventArgs e)
        {
            OOAdvantech.Net.DeviceOOAdvantechCore.DebugDeviceID = DevicesIDS[SelectedIndex];

            GenWebBrowser.WebBrowserOverlay.SetCefExtraCachePath(DevicesIDS[SelectedIndex]);

            FlavourBusinessApps.TakeAwayApp.WPF.TakeAwayApp.Startup(DevicesIDS[SelectedIndex]);
            
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            if (await (mainWindow.DataContext as FlavoursServiceOrderTakingStation)?.IsDeliveryCallCenterStationActive)
            {
                HomeDeliveryUnitTest = new HomeDeliveryUnitTest(mainWindow.DataContext as FlavoursServiceOrderTakingStation);
               await HomeDeliveryUnitTest.AssignShippingsToCourierTest();
            }
            base.OnClosing(e);
        }
    }
}
