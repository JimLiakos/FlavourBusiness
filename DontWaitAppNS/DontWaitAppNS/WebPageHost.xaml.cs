using OOAdvantech.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DontWaitAppNS
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WebPageHost : ContentPage
    {
        public WebPageHost(string url)
        {
            InitializeComponent();
            hybridWebView.Uri = "https://www.google.com/";

            BackLabel.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(async () => { await Navigation.PopAsync(true); })
            });  

        }
    }
}