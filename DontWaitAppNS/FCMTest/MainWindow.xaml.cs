﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FCMTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window ,System.ComponentModel.INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void SendBtn_Click(object sender, RoutedEventArgs e)
        {
            //dXrguneiDxg:APA91bHEqKEUfTUMAr6wJowMC4nDgPeg6Qn6JnCtr4mb3cfKbLBBh_zvLOofgpN-2uMaNIs987spk7p0DZNMneXgG7-47gEppVom_Lt2L04DxSgCJq-5FJgLVr-JJSLQQV4KgawugzPL
            CloudNotificationManager.SendMessage(DeviceID,Guid.NewGuid().ToString("N"), $"Hello from Desto {Environment.NewLine} Hello from Desto {Environment.NewLine} Hello from Desto", "Desto", "https://asfameazure.blob.core.windows.net/destopesto/images/019b894f-c22f-4be0-ae1f-0c700431dd7d.png");
        }

        public string DeviceID { get; set; } = "cuUFBmEIXEy4kxlKkcnOum:APA91bFKBRs-Bq2GC7XImLdn1Z484QUpAs4to8o1cfEOMkFVdQiOp8tPdrJ_PhSU147FA-unc5iPQj6W0x_sYAQ0cfVkNVm909irO8Poi3vI5EXALriJeM4MtA5icUhNhxyubFG62kjd";
    }
}
