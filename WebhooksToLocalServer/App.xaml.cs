using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WebhooksToLocalServer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Webhookservice Webhookservice;

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Webhookservice = new Webhookservice();
            Webhookservice.Start();


        }
    }
}
