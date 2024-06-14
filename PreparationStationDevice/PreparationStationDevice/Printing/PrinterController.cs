using FlavourBusinessFacade.Printing;
using FlavourBusinessManager.Printing;
using System;
using System.Collections.Generic;
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

        public PrinterController(Printer printer,IPrintManager printManager)
        {
            Printer = printer;
            PrintManager = printManager;

        }

        public Task PrinterStatusControllerTask { get; private set; }

        internal void Run()
        {
            lock (this)
            {
                if(PrinterStatusControllerTask!=null)
                    return;
                    PrinterStatusControllerTask = Task.Run(() =>
                   {

                       do
                       {
                           PrinterStatus printerStatus = PrinterStatus.OffLine;

                           try
                           {
                               string address = Printer.Address.Split(':')[0];
                               int port = 0;
                               int.TryParse(Printer.Address.Split(':')[1], out port);
                               TcpClient tcpClient = new TcpClient();
                               //System.Threading.Thread.Sleep(30000);
                               tcpClient.Connect(address, port);
                               var networkStream = tcpClient.GetStream();
                               //var buffer = new byte[2] { 0x1b, 0x40 };
                               //networkStream.Write(buffer, 0, buffer.Length);
                               //networkStream.Flush();

                               byte[] command = new byte[3] { 0x10, 0x4, 1 };//command for printer status
                               networkStream.Write(command, 0, 3);
                               networkStream.Flush();

                               byte[] response = new byte[20];
                               int numOfBytes = networkStream.Read(response, 0, 1);
                               byte printerStatusByte = response[0];




                               tcpClient.Close();

                               if ((printerStatusByte & (byte)0b0001000) != 0)//Offline 
                               {
                                   printerStatus = PrinterStatus.OffLine;
                               }
                               else
                                   printerStatus = PrinterStatus.Online;


                           }
                           catch (Exception error)
                           {
                               printerStatus = PrinterStatus.OffLine;
                           }

                           if (Printer.Status != printerStatus)
                           {
                               PrintManager.UpdatePrinterStatus(Printer, printerStatus);
                               Printer.Status = printerStatus;
                           }

                       } while (true);


                   });
            }
        }
    }
}
