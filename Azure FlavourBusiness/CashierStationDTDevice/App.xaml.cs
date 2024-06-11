using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CashierStationDevice;
using FlavourBusinessFacade.ServicesContextResources;
using FlavourBusinessManager;
using NotifyIconWpf;

namespace CashierStationDTDevice
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// <MetaDataID>{434331bc-73af-41f9-9edf-a2766af5435b}</MetaDataID>
    public partial class App : Application
    {


        private TaskbarIcon notifyIcon;

        public CashierController CashierController { get; private set; }

        public decimal GetRandomNumber(double minimum, double maximum)
        {
            Random random = new Random();
            var _decimal=(decimal) (random.NextDouble() * (maximum - minimum) + minimum);
            return Math.Round(_decimal, 2);

        }

        protected async override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //Random random=new Random();
            //random.NextDouble();
            //List<decimal> decimals = new List<decimal>(); 
            //int count = 140;
            //while(count>0)
            //{
            //    count--;
            //    decimals.Add(GetRandomNumber(1, 18));
            //    System.Threading.Thread.Sleep(200);
            //}
            //decimal[] amounts = decimals.ToArray(); //new decimal[] { 4.65M, 7.33M, 12.40M, 15.44M };
            //decimal[] netAmounts=amounts.Select(x => x/(decimal)1.13).ToArray();

            //decimal[] vatAmounts = amounts.Select(x => x- x / (decimal)1.13).ToArray();
            

            //decimal[] roundedNetAmounts = netAmounts.Select(x => Math.Round(x, 2)).ToArray();
            //decimal[] roundedVatAmounts = vatAmounts.Select(x => Math.Round(x, 2)).ToArray();

            //GreekTaxAuthority.MyData.InvoicesDoc.Test(); 
            //for (int i = 0;i<amounts.Length;i++)
            //{
            //    if(roundedNetAmounts[i] + roundedVatAmounts[i] != amounts[i])
            //    {
            //        var dif = (roundedNetAmounts[i] + roundedVatAmounts[i]) - amounts[i];
            //    }
            //    else
            //    {

            //    }senTe
            //}
            FireBase.Init();
            //FireB
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            var aadeFiscalParty = CashierStationDevice.aadeUtil.aadeFiscalParty.GetPartyInfo("800696676", "800696676ARION", "800696676arion", "800696676");

            //create the notifyicon (it's a resource declared in NotifyIconResources.xaml
            notifyIcon = (TaskbarIcon)FindResource("NotifyIcon");
            
            OOAdvantech.Remoting.RestApi.Authentication.InitializeFirebase("demomicroneme", new FlavourBusinessApps.FirebaseAuth());


            if (!string.IsNullOrWhiteSpace(ApplicationSettings.Current.DocumentSignerType))
            {
                if(ApplicationSettings.Current.DocumentSignerType== typeof(SamtecNext).Name)
                    CashierStationDevice.DocumentSignDevice.Init(new SamtecNext());
                if (ApplicationSettings.Current.DocumentSignerType == typeof(RBSDocSigner).Name)
                {
                    var rbsDocSigner = new RBSDocSigner();
                    if (!string.IsNullOrWhiteSpace(ApplicationSettings.Current.DocumentSignerOutputFolder))
                        rbsDocSigner.SetOutputFolder(ApplicationSettings.Current.DocumentSignerOutputFolder);

                    rbsDocSigner.Start(ApplicationSettings.Current.DocumentSignerDeviceIPAddress, ApplicationSettings.Current.AESKey, ApplicationSettings.Current.AADESendDataUrl);
                    CashierStationDevice.DocumentSignDevice.Init(rbsDocSigner);
                }

            }
            

            StartPrinterEmulation();

            Task.Run(() =>
            {
                CashierController = new CashierStationDevice.CashierController();

                try
                {
                    CashierController.Start();
                }
                catch (CashierStationDeviceException error)
                {
                    Application.Current.Dispatcher.Invoke(() => { Application.Current.Shutdown(); });

                }
            });



        }

        private void StartPrinterEmulation()
        {

            Task.Run(() =>
            {
                //runningPrint = true;
                TcpListener server = null;
                try
                {
                    // Set the TcpListener on port 13000.
                    Int32 port = 9100;// 13000;
                    System.Net.IPAddress localAddr = System.Net.IPAddress.Parse("127.0.0.1");

                    // TcpListener server = new TcpListener(port);
                    server = new TcpListener(IPAddress.Any/*localAddr*/, port);

                    // Start listening for client requests.
                    server.Start();

                    // Buffer for reading data
                    Byte[] bytes = new Byte[65536];

                    //string filePath = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\arionsoftware\PrintCapture\output.txt";
                    // Enter the listening loop.
                    while (true)
                    {
                        string data = null;


                        //File.AppendAllText(filePath, "Waiting for a connection... " + Environment.NewLine);
                        // Perform a blocking call to accept requests.
                        // You could also use server.AcceptSocket() here.
                        TcpClient client = server.AcceptTcpClient();

                        Console.WriteLine("Connected!");
                        //File.AppendAllText(filePath, "Connected!" + Environment.NewLine);
                        data = null;

                        // Get a stream object for reading and writing

                        NetworkStream nstream = client.GetStream();



                        int i;
                        // Loop to receive all the data sent by the client.
                        while ((i = nstream.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            data += Encoding.GetEncoding(1253).GetString(bytes, 0, i);
                        }


                        // Shutdown and end connection
                        client.Close();
                        string filePath = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\arionsoftware\PrintCapture\output.txt";

                    }
                }
                catch (SocketException error)
                {
                    Console.WriteLine("SocketException: {0}", error);


                }
                catch (Exception error)
                {
                    string filePath = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\arionsoftware\PrintCapture\output.txt";
                    string err = string.Format("Exception: {0}", error);
                    File.AppendAllText(filePath, err + Environment.NewLine);

                }
                finally
                {
                    // Stop listening for new clients.
                    server.Stop();
                }

                //  ReadFile(@"\\10.0.0.45\d\DATA_Disk1\Company\Data\Technical\Arion - Peldata\davou\firstFile\filetext.txt");
                //runningPrint = false;
                //dispatcherTimer.Start();
            });

        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            notifyIcon.Dispose(); //the icon would clean up automatically, but this is cleaner
            base.OnExit(e);
        }
    }
}
