using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CourierApp
{
    /// <MetaDataID>{fb79f3c9-89fe-4efa-b3ed-73078ac2b894}</MetaDataID>
    public partial class App : Application
    {
        /// <MetaDataID>{f2aa7a20-355b-4754-a588-cfb9d23250e1}</MetaDataID>
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }

        /// <MetaDataID>{cafbaedf-880a-4314-971b-3a8796e365a3}</MetaDataID>
        protected override void OnStart()
        {
        }

        /// <MetaDataID>{b759c930-dff3-4bfc-a91c-038b6fb30d15}</MetaDataID>
        protected override void OnSleep()
        {
        }

        /// <MetaDataID>{646d5d92-8d90-4ee3-9dae-6bc9fb5d590a}</MetaDataID>
        protected override void OnResume()
        {
        }
    }
}
