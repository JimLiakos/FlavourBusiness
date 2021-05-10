using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace DontWaitApp
{
    /// <MetaDataID>{0b60add7-aaf9-4efd-9f4f-59612b1ab929}</MetaDataID>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScanPage : ContentPage
    {
        public ScanPage()
        {
            InitializeComponent();
            ToggleTorchButton.Clicked += ToggleTorchButton_Clicked;
            SizeChanged += ScanPage_SizeChanged;
        }
        double OrgToggleTorchButtonHeight;
        private void ScanPage_SizeChanged(object sender, EventArgs e)
        {
            if (OrgToggleTorchButtonHeight == 0)
                OrgToggleTorchButtonHeight = ToggleTorchButton.Height;

            ToggleTorchButton.HeightRequest = OrgToggleTorchButtonHeight * 1.5;
        }

        private void ToggleTorchButton_Clicked(object sender, EventArgs e)
        {
            ZxingView.IsTorchOn = !ZxingView.IsTorchOn;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            ZxingView.IsScanning = true;
        }
        protected override void OnDisappearing()
        {
            ZxingView.IsScanning = false;
            base.OnDisappearing();
        }
    }
}