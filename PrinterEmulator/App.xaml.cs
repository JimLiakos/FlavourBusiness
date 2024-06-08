using NotifyIconWpf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;

namespace PrinterEmulator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private TaskbarIcon notifyIcon;

        protected async override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            //FireB
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;


            //create the notifyicon (it's a resource declared in NotifyIconResources.xaml
            notifyIcon = (TaskbarIcon)FindResource("NotifyIcon");

            //OOAdvantech.Remoting.RestApi.Authentication.InitializeFirebase("demomicroneme", new FlavourBusinessApps.FirebaseAuth());


            StartPrinterEmulation(Printers);


        }

        internal static List<Printer> Printers = new List<Printer>() { new Printer() { Port = 9001 } };

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
        }

        List<PrinterTCPListener> printerTCPListeners = new List<PrinterTCPListener>();
        private void StartPrinterEmulation(List<Printer> printers)
        {


            //Task.Run(async () =>
            //{

            foreach (var printer in printers)
            {
                PrinterTCPListener printerTCPListener = new PrinterTCPListener(printer);
                printerTCPListeners.Add(printerTCPListener);
                printerTCPListener.RunAsync();

            }

            //    Dictionary<Task<TcpClient>, Tuple<int, TcpListener>> tcpListeners = new Dictionary<Task<TcpClient>, Tuple<int, TcpListener>>();

            //foreach (var printer in printers)
            //{
            //    var tcpListener = new TcpListener(IPAddress.Any, printer.Port);

            //    tcpListener.Start();

            //    var task = tcpListener.AcceptTcpClientAsync();
            //    var tcpListenerPortPair = new Tuple<int, TcpListener>(printer.Port, tcpListener);

            //    tcpListeners.Add(task, tcpListenerPortPair);
            //}

            //Task<TcpClient> tcpClientTask;

            //while ((tcpClientTask = await Task.WhenAny(tcpListeners.Keys)) != null)
            //{
            //    var tcpListenerPortPair = tcpListeners[tcpClientTask];
            //    var port = tcpListenerPortPair.Item1;
            //    var tcpListener = tcpListenerPortPair.Item2;
            //    var printer = printers.Where(x => x.Port == port).FirstOrDefault();

            //    tcpListeners.Remove(tcpClientTask);

            //    // This needs to be async. What to do with its Task?
            //    // It cannot be awaited here.
            //    var handlerTask = HandleByPortNumber(tcpClientTask.Result, printer);

            //    var task = tcpListener.AcceptTcpClientAsync();

            //    tcpListeners.Add(task, tcpListenerPortPair);
            //}



            ////runningPrint = true;
            //TcpListener server = null;
            //try
            //{
            //    // Set the TcpListener on port 13000.
            //    Int32 port = 9100;// 13000;
            //    System.Net.IPAddress localAddr = System.Net.IPAddress.Parse("127.0.0.1");

            //    // TcpListener server = new TcpListener(port);
            //    server = new TcpListener(IPAddress.Any/*localAddr*/, port);

            //    // Start listening for client requests.
            //    server.Start();

            //    // Buffer for reading data
            //    Byte[] bytes = new Byte[65536];

            //    //string filePath = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\arionsoftware\PrintCapture\output.txt";
            //    // Enter the listening loop.
            //    while (true)
            //    {
            //        string data = null;


            //        //File.AppendAllText(filePath, "Waiting for a connection... " + Environment.NewLine);
            //        // Perform a blocking call to accept requests.
            //        // You could also use server.AcceptSocket() here.
            //        TcpClient client = server.AcceptTcpClient();

            //        Console.WriteLine("Connected!");
            //        //File.AppendAllText(filePath, "Connected!" + Environment.NewLine);
            //        data = null;

            //        // Get a stream object for reading and writing

            //        NetworkStream nstream = client.GetStream();



            //        int i;
            //        // Loop to receive all the data sent by the client.
            //        while ((i = nstream.Read(bytes, 0, bytes.Length)) != 0)
            //        {
            //            data += System.Text.Encoding.GetEncoding(1253).GetString(bytes, 0, i);
            //        }


            //        // Shutdown and end connection
            //        client.Close();
            //        string filePath = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\arionsoftware\PrintCapture\output.txt";

            //    }
            //}
            //catch (SocketException error)
            //{
            //    Console.WriteLine("SocketException: {0}", error);


            //}
            //catch (Exception error)
            //{
            //    string filePath = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\arionsoftware\PrintCapture\output.txt";
            //    string err = string.Format("Exception: {0}", error);
            //    File.AppendAllText(filePath, err + Environment.NewLine);

            //}
            //finally
            //{
            //    // Stop listening for new clients.
            //    server.Stop();
            //}

            ////  ReadFile(@"\\10.0.0.45\d\DATA_Disk1\Company\Data\Technical\Arion - Peldata\davou\firstFile\filetext.txt");
            ////runningPrint = false;
            ////dispatcherTimer.Start();
            //});

        }

        //private object HandleByPortNumber(TcpClient client, Printer printer)
        //{

        //    Task.Run(() =>
        //    {

        //        while (true)
        //        {
        //            string data = null;
        //            // Buffer for reading data
        //            Byte[] bytes = new Byte[65536];

        //            //File.AppendAllText(filePath, "Waiting for a connection... " + Environment.NewLine);
        //            // Perform a blocking call to accept requests.
        //            // You could also use server.AcceptSocket() here.
        //            //TcpClient client = server.AcceptTcpClient();

        //            //                    Console.WriteLine("Connected!");
        //            //File.AppendAllText(filePath, "Connected!" + Environment.NewLine);
        //            data = null;

        //            // Get a stream object for reading and writing


        //            NetworkStream nstream = client.GetStream();



        //            int i;
        //            // Loop to receive all the data sent by the client.
        //            while ((i = nstream.Read(bytes, 0, bytes.Length)) != 0)
        //            {
        //                string text = System.Text.Encoding.GetEncoding(1253).GetString(bytes, 0, i);
        //                System.Diagnostics.Debug.WriteLine(text);
        //                data += text;

        //                if (i == 3 && bytes[0] == 0x10 && bytes[1] == 0x4 && bytes[2] == 0x1)//get printer status
        //                {
        //                    nstream.Write(printer.Status.Take(1).ToArray(), 0, 1);
        //                    nstream.Flush();
        //                }



        //            }

        //            if (!client.Connected)
        //                break;

        //            System.Threading.Thread.Sleep(70);
        //            // Shutdown and end connection
        //            //client.Close();
        //            //string filePath = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\arionsoftware\PrintCapture\output.txt";

        //        }
        //        //});

        //        return null;

        //    });
        //}
    }
}
