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
using System.Windows;
using static FlavourBusinessFacade.Printing.Printer;

namespace PreparationStationDevice.Printing
{
    public class PrinterController
    {
        public readonly Printer Printer;

        public IPrintManager PrintManager { get; }

        public PrinterController(Printer printer, IPrintManager printManager)
        {
            Printer = printer;
            PrintManager = printManager;
            try
            {

                string localAppFolder = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

                string printerLocalAppFolder = localAppFolder;
                if (localAppFolder.IndexOf('\\') > -1)
                {

                    printerLocalAppFolder += @"\printingLocalService\" + printer.Identity;
                    if (!System.IO.Directory.Exists(printerLocalAppFolder))
                        System.IO.Directory.CreateDirectory(printerLocalAppFolder);
                    printerLocalAppFolder += @"\";

                }
                else
                {
                    printerLocalAppFolder += "/printingLocalService/" + printer.Identity;
                    if (!System.IO.Directory.Exists(printerLocalAppFolder))
                        System.IO.Directory.CreateDirectory(printerLocalAppFolder);
                    printerLocalAppFolder += "/";

                }
                PrinterFolder = printerLocalAppFolder;

                var dir = new System.IO.DirectoryInfo(PrinterFolder);
                foreach (var file in dir.GetFiles().OrderBy(x => x.LastWriteTime))
                {
                    try
                    {

                        lock (Spooler)
                            Spooler.Enqueue(OOAdvantech.Json.JsonConvert.DeserializeObject<FlavourBusinessFacade.Print.Printing>(System.IO.File.ReadAllText(file.FullName)));
                    }
                    catch (Exception error)
                    {
                    }
                }


            }
            catch (Exception error)
            {

                
            }            
            lock (PrintedDocumentIds)
            {
                string filePath = PrinterFolder + "PrintedDocumentIds.dat";
                try
                {
                    if (System.IO.File.Exists(filePath))
                    {
                        string printedDocumentIdsJson = System.IO.File.ReadAllText(filePath);
                        PrintedDocumentIds.AddRange(OOAdvantech.Json.JsonConvert.DeserializeObject<List<string>>(printedDocumentIdsJson));
                    }
                }
                catch (Exception error)
                {

                    throw;
                }
            }

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

       internal   List<string> PrintedDocumentIds = new List<string>();
        internal void Run()
        {
            lock (this)
            {
                if (PrinterStatusControllerTask != null)
                    return;


                PrinterStatusControllerTask = Task.Run(() =>
               {

                   try
                   {
                       do
                       {
                           try
                           {
                               bool pendingPrintings = false;
                               lock (Spooler)
                                   pendingPrintings = Spooler.Count > 0; 
                                   

                               if (pendingPrintings&&UpdatePinterStatus() == PrinterStatus.Online)
                               {
                                   FlavourBusinessFacade.Print.Printing printing = null;
                                   lock (Spooler)
                                   {
                                       printing = Spooler.Peek();
                                   }
                                   if (Print(printing.RawData))
                                   {
                                       lock (Spooler)
                                       {
                                           printing = Spooler.Dequeue();
                                           PrintedDocumentIds.Add(printing.ID);
                                           SavePrintedDocumentIds();

                                       }
                                       this.PrintManager.DocumentPrinted(printing.ID);
                                       PrintedDocumentIds.Remove(printing.ID);
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

        private void SavePrintedDocumentIds()
        {
            lock (PrintedDocumentIds)
            {
                string filePath = PrinterFolder + "PrintedDocumentIds.dat";
                string printedDocumentIdsJson = OOAdvantech.Json.JsonConvert.SerializeObject(PrintedDocumentIds);
                System.IO.File.WriteAllText(filePath, printedDocumentIdsJson);
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



                address = "10.0.0.142";
                port = 9100;
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

        internal void PrintOut(FlavourBusinessFacade.Print.Printing printing)
        {
             

            lock (Spooler)
            {

                Save(printing);
                Spooler.Enqueue(printing);
            }
        }

        string PrinterFolder;

        private void Save(FlavourBusinessFacade.Print.Printing printing)
        {
            string filePath = PrinterFolder + printing.ID + ".dat";
            string printingJson = OOAdvantech.Json.JsonConvert.SerializeObject(printing);
            System.IO.File.WriteAllText(filePath, printingJson);


        }
    }
}
