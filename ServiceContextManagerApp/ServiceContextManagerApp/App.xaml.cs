using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ServiceContextManagerApp
{
    public partial class App : Application
    {
       // public static string storage_path;

        public App()
        {
            InitializeComponent();
           
            //MainPage = new FacebookSignIn();
            //MainPage = new MainPage();
            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
