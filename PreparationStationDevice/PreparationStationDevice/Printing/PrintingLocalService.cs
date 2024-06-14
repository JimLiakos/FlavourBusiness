using FlavourBusinessFacade.Printing;
using FlavourBusinessManager.Printing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static FlavourBusinessFacade.Printing.Printer;

namespace PreparationStationDevice.Printing
{
    internal class PrintingLocalService
    {

        List<PrinterController> Printers = new List<PrinterController>();
        public PrintingLocalService(IPrintManager printManager)
        {
            PrintManager = printManager;
            if (PrintManager != null)
                Printers = PrintManager.Printers .Select(x=>new PrinterController(x, printManager)).ToList();
        }

        public Task PrintingServiceTask { get; private set; }

        public IPrintManager PrintManager { get; private set; }


        public void StartPrintingService()
        {



#if DeviceDotNet
            IDeviceOOAdvantechCore device = DependencyService.Get<IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
            if (!device.IsBackgroundServiceStarted)
            {



                BackgroundServiceState serviceState = new BackgroundServiceState();
                device.RunInBackground(new Action(async () =>
                {


                    //var printerPort = new IPort();

                    //ThermalPrinter printer = new ThermalPrinter(printerPort, 2, 180, 2);
                    //int status = printer.GetPrinterStatus();


                    //TcpClient tcpClient = new TcpClient();
                    //try
                    //{

                    //    tcpClient.Connect("10.0.0.142", 9100);
                    //    var networkStream = tcpClient.GetStream();
                    //    var buffer = new byte[2] { 0x1b, 0x40 };
                    //    networkStream.Write(buffer, 0, buffer.Length);
                    //    buffer = Encoding.ASCII.GetBytes("Liakos" + System.Environment.NewLine + System.Environment.NewLine + System.Environment.NewLine);
                    //    networkStream.Write(buffer, 0, buffer.Length);

                    //    //    NetworkStream stream = client.GetStream();
                    //    //    stream(new byte[2] { 0x1b, 0x40 });
                    //}
                    //catch (Exception error)
                    //{
                    //}

                    do
                    {
                        TcpClient tcpClient = new TcpClient();
                        tcpClient.Connect("10.0.0.142", 9100);
                        var connected = tcpClient.Connected;

                        System.Threading.Thread.Sleep(2000);




                        //Socket.Send(receiptStream);
                        //Socket.Disconnect(false);


                    } while (!serviceState.Terminate);
                }), serviceState);
            }
#else

#endif

 
            if (PrintingServiceTask == null || PrintingServiceTask.Status != TaskStatus.Running)
            {
                PrintingServiceTask = Task.Run(() =>
                {
                    foreach(var printer in Printers )
                    {
                        printer.Run();
                    }

                    PrintManager.DocumentPendingToPrint += (IPrintManager sender, string deviceUpdateEtag) =>
                    {

                        var printings = PrintManager.GetPendingPrintings(null, deviceUpdateEtag);

                        //var transctions = PrintManager.GetOpenTransactions(deviceUpdateEtag);
                    };
                    do
                    {


                        try
                        {

                            foreach (var printer in printers)
                            {

                            
                            }

                        }

                        catch (Exception error)
                        {


                        }

                        System.Threading.Thread.Sleep(5000);


                    } while (!TerminatePrintingWatcher);
                });
            }

        }

    }
}
