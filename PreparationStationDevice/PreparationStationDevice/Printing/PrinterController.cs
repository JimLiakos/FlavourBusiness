using FlavourBusinessFacade.Print;
using FlavourBusinessFacade.Printing;
using FlavourBusinessManager.Printing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static FlavourBusinessFacade.Printing.Printer;

namespace PreparationStationDevice.Printing
{
    public class PrinterController
    {
        private Printer Printer;

        public IPrintManager PrintManager { get; }

        public PrinterController(Printer printer, IPrintManager printManager)
        {
            Printer = printer;
            PrintManager = printManager;

        }

        public List<FlavourBusinessFacade.Print.Printing> Printings
        {
            get
            {
                lock (Spooler)
                    return Spooler.ToList();
            }
        }
        Queue<FlavourBusinessFacade.Print.Printing> Spooler { get; } = new Queue<FlavourBusinessFacade.Print.Printing>();

        bool TerminatePrinterWatcher = false;

        public Task PrinterStatusControllerTask { get; private set; }

        internal void Run()
        {
            lock (this)
            {
                if (PrinterStatusControllerTask!=null)
                    return;


                PrinterStatusControllerTask = Task.Run(() =>
               {

                   try
                   {
                       do
                       {
                           try
                           {
                               if (UpdatePinterStatus()==PrinterStatus.Online)
                               {
                                   FlavourBusinessFacade.Print.Printing printing = null;
                                   lock (Spooler)
                                   {
                                       printing = Spooler.Peek();
                                   }
                                   if (Print(printing.RawData))
                                   {
                                       this.PrintManager.DocumentPrinted(printing.ID);

                                   }
                               }
                           }
                           catch (Exception error)
                           {
                           }
                           System.Threading.Thread.Sleep(1000);

                       } while (!TerminatePrinterWatcher);
                   }
                   catch (Exception error)
                   {
                       throw;
                   }

               });
            }
        }

        private bool Print(byte[] rawData)
        {
            throw new NotImplementedException();
        }

        private PrinterStatus UpdatePinterStatus()
        {
            PrinterStatus printerStatus = PrinterStatus.OffLine;
            TcpClient tcpClient = new TcpClient();
            try
            {
                string address = Printer.Address.Split(':')[0];
                int port = 0;
                int.TryParse(Printer.Address.Split(':')[1], out port);



                address ="10.0.0.142";
                port=9100;
                //System.Threading.Thread.Sleep(30000);
                System.Diagnostics.Debug.WriteLine("try to connect Printer.Address");
                var connectTask = tcpClient.ConnectAsync(address, port);
                if (connectTask.Wait(1500))
                {
                    System.Diagnostics.Debug.WriteLine("connected to Printer.Address");
                    var networkStream = tcpClient.GetStream();


                    byte[] command = new byte[3] { 0x10, 0x4, 1 };//command for printer status
                    networkStream.Write(command, 0, 3);
                    networkStream.Flush();

                    byte[] response = new byte[20];
                    var readTask = networkStream.ReadAsync(response, 0, 1);

                    if (readTask.Wait(3000)) //maximum time to response
                    {
                        int numOfBytes = readTask.Result;
                        byte printerStatusByte = response[0];

                        if ((printerStatusByte & (byte)0b0001000) != 0)//Offline 
                        {
                            printerStatus = PrinterStatus.OffLine;
                        }
                        else
                            printerStatus = PrinterStatus.Online;
                    }
                    else
                        printerStatus = PrinterStatus.OffLine;
                    tcpClient.Close();
                }
                else
                {
                    printerStatus = PrinterStatus.OffLine;
                    try
                    {
                        // send offline  and continue to wait for connection
                        if (Printer.Status != printerStatus)
                        {
                            PrintManager.UpdatePrinterStatus(Printer, printerStatus);
                            Printer.Status = printerStatus;
                        }
                    }
                    catch (Exception error)
                    {
                    }

                    connectTask.Wait();

                    try
                    {
                        tcpClient.Close();
                    }
                    catch (Exception error)
                    {
                    }
                    System.Diagnostics.Debug.WriteLine("fail to connect to Printer.Address");
                    System.Threading.Thread.Sleep(3000);
                }
            }
            catch (Exception error)
            {
                printerStatus = PrinterStatus.OffLine;

                try
                {
                    tcpClient.Close();
                }
                catch (Exception)
                {
                }
            }
            try
            {
                if (Printer.Status != printerStatus)
                {
                    PrintManager.UpdatePrinterStatus(Printer, printerStatus);
                    Printer.Status = printerStatus;
                }
            }
            catch (Exception error)
            {
            }
            return printerStatus;
        }
    }
}
