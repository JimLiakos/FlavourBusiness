using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace ServiceContextManagerApp
{
    /// <MetaDataID>{0b60add7-aaf9-4efd-9f4f-59612b1ab929}</MetaDataID>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScanPage : ContentPage
    {
        public delegate void ScanResultDelegate(ZXing.Result result);
        public event ScanResultDelegate OnScan;
        public ScanPage()
        {
            InitializeComponent();
            ToggleTorchButton.Clicked += ToggleTorchButton_Clicked;
            SizeChanged += ScanPage_SizeChanged;

            ZxingView.OnScanResult += (result) =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    // Stop analysis until we navigate away so we don't keep reading barcodes
                    ZxingView.IsAnalyzing = false;

                    OnScan?.Invoke(result);
                });
            };

         }
        double OrgToggleTorchButtonHeight;

        string _HeaderText;
        public string HeaderText
        {
            get
            {
                return _HeaderText;
            }
            set
            {
                _HeaderText = value;
                OnPropertyChanged(nameof(HeaderText));
            }
        }

        string _FooterText;
        public string FooterText
        {
            get
            {
                return _FooterText;
            }
            set
            {
                _FooterText = value;
                OnPropertyChanged(nameof(FooterText));
            }
        }


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

    public class ScanCode
    {

        public ScanCode()
        {
            ScanPage.OnScan += async (result) =>
            {
                if (OnScan)
                {
                    TaskCompletionSource<ZXing.Result> connectToServicePointTask = null;
                    lock (this)
                    {
                        connectToServicePointTask = ConnectToServicePointTask;
                        ConnectToServicePointTask = null;
                        OnScan = false;
                    }

       
                    // Navigate away
                    try
                    {
                        await ScanPage.Navigation.PopAsync();
                        connectToServicePointTask.SetResult(result);
                    }
                    catch (Exception error)
                    {
                        connectToServicePointTask.SetException(error);
                    }
                }
            };
            ScanPage.Disappearing += (object sender, EventArgs e) =>
            {
                OnScan = false;
                if(ConnectToServicePointTask!=null)
                    ConnectToServicePointTask.SetResult(null);
            };
        }
        ScanPage ScanPage = new ScanPage();
        TaskCompletionSource<ZXing.Result> ConnectToServicePointTask;
        bool OnScan = false;
        public Task<ZXing.Result>  Scan(string headerText,string footerText )
        {
            lock (this)
            {

                if (OnScan && ConnectToServicePointTask != null)
                    return ConnectToServicePointTask.Task;

                OnScan = true;
                ScanPage.HeaderText = headerText;
                ScanPage.FooterText = footerText;
                ConnectToServicePointTask = new TaskCompletionSource<ZXing.Result>();
            }
            Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
            {
                await (App.Current.MainPage as NavigationPage).CurrentPage.Navigation.PushAsync(ScanPage);
            });


            return ConnectToServicePointTask.Task;


        }
    }
}