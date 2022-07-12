﻿using FinanceFacade;
using FlavourBusinessFacade;
using FlavourBusinessFacade.RoomService;
using FlavourBusinessFacade.ServicesContextResources;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CashierStationDevice
{
    /// <MetaDataID>{82be5194-83d3-4038-b878-d6225ab658cb}</MetaDataID>
    public class CashierController
    {
        /// <MetaDataID>{af4a528e-ef65-4e03-b887-e0ba7e094e13}</MetaDataID>
        static string AzureServerUrl = string.Format("http://{0}:8090/api/", FlavourBusinessFacade.ComputingResources.EndPoint.Server);



        /// <MetaDataID>{1e6f893a-0c22-495e-bf82-87e67c84b6e9}</MetaDataID>
        public CashierController()
        {
            ApplicationSettings.Current.ObjectChangeState += ApplicationSettings_ObjectChangeState;
        }

        /// <MetaDataID>{4d603572-27d1-467e-85ef-266b8912c0d2}</MetaDataID>
        private void ApplicationSettings_ObjectChangeState(object _object, string member)
        {
            if (member == nameof(ApplicationSettings.CommunicationCredentialKey))
            {
                if (string.IsNullOrWhiteSpace(ApplicationSettings.Current.CommunicationCredentialKey))
                {
                    if (ApplicationSettings.Current.CashiersStation != null)
                        ApplicationSettings.Current.CashiersStation.OpenTransactions -= CashiersStation_OpenTransactions;
                }
                else
                {

                    string assemblyData = "FlavourBusinessManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
                    string type = "FlavourBusinessManager.FlavoursServicesContextManagment";
                    string serverUrl = AzureServerUrl;
                    IFlavoursServicesContextManagment servicesContextManagment = OOAdvantech.Remoting.RestApi.RemotingServices.CastTransparentProxy<IFlavoursServicesContextManagment>(OOAdvantech.Remoting.RestApi.RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData));
                    var cashiersStation = servicesContextManagment.GetCashiersStationRuntime(ApplicationSettings.Current.CommunicationCredentialKey);
                    if (cashiersStation != ApplicationSettings.Current.CashiersStation)
                    {
                        if (ApplicationSettings.Current.CashiersStation != null)
                            ApplicationSettings.Current.CashiersStation.OpenTransactions -= CashiersStation_OpenTransactions;
                        ApplicationSettings.Current.CashiersStation = cashiersStation;
                        ApplicationSettings.Current.CashiersStation.OpenTransactions += CashiersStation_OpenTransactions;
                    }
                }


            }
        }


        /// <MetaDataID>{4c0c5a25-49c9-4012-b591-c599cb07d069}</MetaDataID>
        public void Stop()
        {

            ApplicationSettings.Current.CashiersStation.OpenTransactions += CashiersStation_OpenTransactions;
        }



        /// <MetaDataID>{d76d5629-dbbc-4583-8610-6cd79f7f3c2c}</MetaDataID>
        public void Start()
        {
            string assemblyData = "FlavourBusinessManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
            string type = "FlavourBusinessManager.FlavoursServicesContextManagment";
            string serverUrl = AzureServerUrl;
            if (string.IsNullOrWhiteSpace(ApplicationSettings.Current.CommunicationCredentialKey))
                return;
            IFlavoursServicesContextManagment servicesContextManagment = OOAdvantech.Remoting.RestApi.RemotingServices.CastTransparentProxy<IFlavoursServicesContextManagment>(OOAdvantech.Remoting.RestApi.RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData));
            ApplicationSettings.Current.CashiersStation = servicesContextManagment.GetCashiersStationRuntime(ApplicationSettings.Current.CommunicationCredentialKey);
            if (ApplicationSettings.Current.CashiersStation == null)
                return;
            ApplicationSettings.Current.CompanyHeader = OOAdvantech.Json.JsonConvert.DeserializeObject<CompanyHeader>((ApplicationSettings.Current.CashiersStation as ICashierStation).CashierStationDeviceData);

            var path = ApplicationSettings.AppDataPath;
            string[] filePaths = Directory.GetFiles(path, "*.json");
            foreach (var filePath in filePaths)
            {
                var json = File.ReadAllText(filePath);
                var jsonSerializerSettings = new OOAdvantech.Json.JsonSerializerSettings() { TypeNameHandling = OOAdvantech.Json.TypeNameHandling.All };
                var transaction = OOAdvantech.Json.JsonConvert.DeserializeObject<ITransaction>(json, jsonSerializerSettings);

                lock (Transactions)
                {
                    Transactions.Add(transaction);
                }


            }


            int numofTries = 0;
            Retry:
            try
            {
                Issuer = (ApplicationSettings.Current.CashiersStation as ICashierStation).Issuer;
                ApplicationSettings.Current.CashiersStation.OpenTransactions += CashiersStation_OpenTransactions;
                var transctions = ApplicationSettings.Current.CashiersStation.GetOpenTransactions(null);
                AddPendingTransactionsForPrint(transctions);


            }
            catch (Exception error)
            {

                if (error.HResult == 800)
                {
                    if (numofTries++ <= 5)
                    {
                        System.Threading.Thread.Sleep(1000);
                        goto Retry;
                    }

                    if (!System.Diagnostics.EventLog.SourceExists("CashierStationDevice", "."))
                        System.Diagnostics.EventLog.CreateEventSource("CashierStationDevice", "Microneme");
                    System.Diagnostics.EventLog myLog = new System.Diagnostics.EventLog();
                    myLog.Source = "CashierStationDevice";
                    myLog.WriteEntry("There is already attached device to the cashier station", System.Diagnostics.EventLogEntryType.Error);

                    throw new CashierStationDeviceException("Only one cashier station device allowed to subscribe");
                }
            }
            Task.Run(() =>
            {

                int index = 0;
                while (true)
                {

                    FinanceFacade.ITransaction transaction = null;
                    lock (Transactions)
                    {
                        if (index >= Transactions.Count)
                            index = 0;
                        if (Transactions.Count > 0)
                            transaction = Transactions[index++];
                    }
                    try
                    {
                        if (transaction != null)
                        {
                            CompanyHeader companyHeader = new CompanyHeader()
                            {
                                Address = "Στρ. Μακρυγιάννη 40" + Environment.NewLine + "Μοσχάτο,Νότιος Τομέας Αθηνών" + Environment.NewLine + "TK 183 44",
                                Title = "Colosseo Pizza Pasta",
                                Subtitle = "Πιτσαρία",
                                FiscalData = "ΑΦΜ 047362769 ΔΟΥ Μοσχάτου",
                                ContatInfo = "Τηλ. 2109407777"


                            };
                            PrintReceipt(transaction, CashierStationDevice.DocumentSignDevice.CurrentDocumentSignDevice, ApplicationSettings.Current.CompanyHeader);
                        }
                    }
                    catch (Exception error)
                    {


                    }
                    System.Threading.Thread.Sleep(500);
                }
            });

        }
        internal List<ITransaction> Transactions = new List<ITransaction>();
        object PendingTransactionsLock = new object();
        private void AddPendingTransactionsForPrint(List<ITransaction> transactions)
        {

            lock (PendingTransactionsLock)
            {
                var transactionPrinter = ApplicationSettings.Current.TransactionsPrinters.Where(x => x.IsDefault).FirstOrDefault();
                if (transactionPrinter != null)
                {
                    var controlledTransactionsUris = Transactions.Select(x => x.Uri).ToList();



                    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                    {
                        foreach (var transaction in transactions)
                        {
                            if (!controlledTransactionsUris.Contains(transaction.Uri))
                            {
                                if (transaction.GetAutoNumber() == null)
                                {
                                    var autoNumber = ++transactionPrinter.AutoNumber;
                                    transaction.SetAutoNumber(autoNumber);
                                    transaction.SetCodePage(transactionPrinter.PrinterCodePage);
                                    transaction.SetSeries(transactionPrinter.Series);
                                    transaction.SetTransactionTypeID("173");
                                    if (!string.IsNullOrWhiteSpace(transactionPrinter.RawPrinterAddress))
                                        transaction.SetRawPrinterAddress(transactionPrinter.RawPrinterAddress);
                                }
                                var serviceBatchUri = transaction.GetServiceBatchUri();
                                if (!string.IsNullOrWhiteSpace(serviceBatchUri))
                                {
                                    IServingBatch ServingBatch = ApplicationSettings.Current.CashiersStation.GetServingBatch(serviceBatchUri);

                                    if (ServingBatch.ServicePoint.ServicePointType == ServicePointType.HallServicePoint)
                                        transaction.SetPropertyValue("ServicePointDescription", Properties.Resources.HallServicePointLabel + " " + ServingBatch.ServicePoint.Description);
                                }

                                transaction.Save(ApplicationSettings.AppDataPath);

                                lock (Transactions)
                                {
                                    Transactions.Add(transaction);
                                }
                            }
                        }
                        stateTransition.Consistent = true;
                    }




                }
            }

            //var transactiona=  OOAdvantech.Json.JsonConvert.DeserializeObject<ITransaction>(json, jsonSerializerSettings);



        }

        private void CashiersStation_OpenTransactions(ICashiersStationRuntime sender, string deviceUpdateEtag)
        {
            Task.Run(() =>
            {
                var transctions = ApplicationSettings.Current.CashiersStation.GetOpenTransactions(deviceUpdateEtag);

                AddPendingTransactionsForPrint(transctions);
            });


        }


        List<ITransaction> PendingTransactionsForPrint = new List<ITransaction>();

        public static Dictionary<string, int> VatAcounts = new Dictionary<string, int>{
            {"a1",0},
            {"b1",1},
            {"c1",2},
            {"d1",3},
            {"C54.00.70.0006",0},
            {"C54.00.70.0013",1},
            {"C54.00.70.0024",2},
            {"C54.00.70.0036",3},
            {"C54.00.70.0000",4},
            {"C54.00.79.0004",0},
            {"C54.00.79.0009",1},
            {"C54.00.79.0017",2},
            {"C54.00.79.0025",3}
        };

        public IFisicalParty Issuer { get; private set; }

        public void PrintReceipt(ITransaction transaction, IDocumentSignDevice documentSigner, CompanyHeader companyHeader)
        {
            string printText = null;

            if (string.IsNullOrWhiteSpace(transaction.GetPropertyValue("Signature")))
            {


                printText = GetReceiptRawPrint(transaction, companyHeader, transaction.GetPropertyValue("ServicePointDescription"), "");

                EpsilonLineData epsilonLineData = new EpsilonLineData()
                {
                    afm_publisher = Issuer.VATNumber,
                    afm_recipient = "",
                    transactionTypeID = transaction.GetTransactionTypeID(),
                    series = transaction.GetSeries(),
                    taxDocNumber = transaction.GetAutoNumber()?.ToString(),
                    net_a = 0,
                    net_b = 0,
                    net_c = 0,
                    net_d = 0,
                    net_e = 0,
                    vat_a = 0,
                    vat_b = 0,
                    vat_c = 0,
                    vat_d = 0,
                    total_to_pay_poso = 0
                };

                foreach (var item in transaction.Items)
                {
                    epsilonLineData.total_to_pay_poso += item.Price;
                    if (item.Taxes.Count > 0 && VatAcounts.ContainsKey(item.Taxes[0].AccountID))
                    {
                        if (VatAcounts[item.Taxes[0].AccountID] == 0)
                        {
                            epsilonLineData.vat_a += item.Taxes[0].Amount;
                            epsilonLineData.net_a += item.Price - item.Taxes[0].Amount;
                        }
                        if (VatAcounts[item.Taxes[0].AccountID] == 1)
                        {
                            epsilonLineData.vat_b += item.Taxes[0].Amount;
                            epsilonLineData.net_b += item.Price - item.Taxes[0].Amount;
                        }

                        if (VatAcounts[item.Taxes[0].AccountID] == 2)
                        {
                            epsilonLineData.vat_c += item.Taxes[0].Amount;
                            epsilonLineData.net_c += item.Price - item.Taxes[0].Amount;
                        }

                        if (VatAcounts[item.Taxes[0].AccountID] == 3)
                        {
                            epsilonLineData.vat_d += item.Taxes[0].Amount;
                            epsilonLineData.net_d += item.Price - item.Taxes[0].Amount;
                        }
                    }

                }

                string afdsDoc = printText;
                if (documentSigner.IsOnline)
                {
                  
                    
                    SignatureData signature = documentSigner.SignDocument(afdsDoc, epsilonLineData);
                    if (!string.IsNullOrWhiteSpace(signature.Signuture))
                    {
                        transaction.SetPropertyValue("Signature", signature.Signuture);
                        transaction.SetPropertyValue("QRCode", signature.QRCode);
                        transaction.Save(ApplicationSettings.AppDataPath);
                    }
                }
            }
            printText = GetReceiptRawPrint(transaction, companyHeader, transaction.GetPropertyValue("ServicePointDescription"), "");
            int? codePage = transaction.GetCodePage();
            if (!string.IsNullOrWhiteSpace(transaction.GetPropertyValue("Signature")))
            {
                if (!string.IsNullOrWhiteSpace(transaction.GetRawPrinterAddress()))
                {
                    byte[] receiptStream = null;
                    if (codePage == null || codePage == 0)
                        receiptStream = System.Text.ASCIIEncoding.ASCII.GetBytes(printText);
                    else
                        receiptStream = Encoding.GetEncoding(codePage.Value).GetBytes(printText);

                    try
                    {
                        if (!transaction.IsTransactionPrinted() || transaction.PrintAgain)
                        {
                            System.Net.Sockets.Socket Socket = new System.Net.Sockets.Socket(System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
                            Socket.Connect(transaction.GetRawPrinterAddress(), 9100);
                            Socket.Send(new byte[2] { 0x1b, 0x40 });

                            Socket.Send(receiptStream);
                            Socket.Disconnect(false);
                            if (!transaction.IsTransactionPrinted())
                            {
                                transaction.SetTransactionPrinted(true);
                                transaction.InvoiceNumber = transaction.GetAutoNumber().ToString();
                                if (!string.IsNullOrWhiteSpace(transaction.GetSeries()))
                                    transaction.InvoiceNumber = transaction.GetSeries() + " " + transaction.InvoiceNumber;
                            }

                            transaction.PrintAgain = false;



                        }
                        lock (Transactions)
                        {
                            ApplicationSettings.Current.CashiersStation.TransactionCommited(transaction);

                            this.Transactions.Remove(transaction);
                            transaction.Save(ApplicationSettings.AppDataPath);
                            transaction.Move(ApplicationSettings.AppDataPath, ApplicationSettings.AppDataPath + @"\completedTransactions");
                        }


                    }
                    catch (Exception error)
                    {

                    }
                }
            }


        }


        public static string GetReceiptRawPrint(ITransaction transaction, CompanyHeader companyHeader, string tableLine, string comments)
        {

            System.Collections.Generic.Dictionary<string, decimal> taxesSums = new Dictionary<string, decimal>();

            byte[] data = Read(typeof(CashierStationDevice.SamtecNext).Assembly.GetManifestResourceStream("CashierStationDevice.Resources.Receipt.bin"));
            string printReport = System.Text.Encoding.UTF8.GetString(data, 0, data.Length);


            System.IO.StringWriter rawPrintingWriter = new System.IO.StringWriter();
            System.IO.StringReader report = new System.IO.StringReader(printReport);


            string lineString = report.ReadLine();

            #region writes receipt header area
            while (lineString != null && lineString.IndexOf("&") == -1)//Ampersand & defines the end of header area and the start of details
            {

                #region DateTime line
                if (lineString.IndexOf("^") != -1 || lineString.IndexOf("%") != -1)
                {
                    int len = lineString.Length;

                    int year = System.DateTime.Now.Year - 2000;



                    string yearstr = year.ToString();
                    if (yearstr.Length == 1)
                        yearstr = "0" + yearstr;

                    lineString = lineString.Replace("^", string.Format("{0}/{1}/{2}", DateTime.Now.Day, DateTime.Now.Month, yearstr));

                    string hourStr = DateTime.Now.Hour.ToString();
                    if (hourStr.Length == 1)
                        hourStr = "0" + hourStr;

                    string minuteStr = DateTime.Now.Minute.ToString();
                    if (minuteStr.Length == 1)
                        minuteStr = "0" + minuteStr;
                    lineString = lineString.Replace("%", string.Format("{0}:{1}", hourStr, minuteStr));
                }
                #endregion

                // Series receipt number
                string series_autonumber = (transaction.GetPropertyValue("Series") + "  " + transaction.GetPropertyValue("AutoNumber")).Trim();

                if (lineString.IndexOf("@") != -1)
                    lineString = FixLengthReplace(series_autonumber, lineString, "@").Trim();

                //table number 
                #region Defines the table (service point) description 
                if (lineString.IndexOf("`") != -1)
                {
                    if (!string.IsNullOrEmpty(tableLine))
                        lineString = FixLengthReplace(tableLine, lineString, "`");
                    else
                    {
                        lineString = report.ReadLine();
                        continue;
                    }
                }
                #endregion

                #region company header
                //string title,
                if (lineString.IndexOf("$") != -1)
                    lineString = FixLengthReplace(companyHeader.Title, lineString, "$").Trim();

                //string productUser,
                if (lineString.IndexOf("=") != -1)
                    lineString = FixLengthReplace(companyHeader.Subtitle, lineString, "=").Trim();


                //string address,
                if (lineString.IndexOf("£") != -1)
                    lineString = FixLengthReplace(companyHeader.Address, lineString, "£").Trim();

                //string organizationVat,
                if (lineString.IndexOf("§") != -1)
                    lineString = FixLengthReplace(companyHeader.FiscalData, lineString, "§").Trim();

                //string ContuctInfo,
                if (lineString.IndexOf("©") != -1)
                    lineString = FixLengthReplace(companyHeader.ContatInfo, lineString, "©").Trim();
                #endregion

                //Order Code
                if (lineString.IndexOf("~") != -1)
                {
                    string orderCode = transaction.GetServiceBatchID();
                    if (!string.IsNullOrWhiteSpace(orderCode))
                        lineString = FixLengthReplace(orderCode, lineString, "~");
                    lineString = null;
                }
                if (lineString != null)
                    rawPrintingWriter.WriteLine(lineString);
                lineString = report.ReadLine();
            }

            #endregion

            string transactionItemTemplateLine = lineString;
            string ingrSpace = null;

            lineString = report.ReadLine();
            while (lineString != null && lineString.IndexOf("##") == -1)
            {
                ingrSpace += lineString;
                lineString = report.ReadLine();
            }


            foreach (var transactionItem in transaction.Items)
            {
                foreach (var taxAmount in transactionItem.Taxes)
                {
                    if (!taxesSums.ContainsKey(taxAmount.AccountID))
                        taxesSums.Add(taxAmount.AccountID, 0);

                    decimal totalOnVat = taxesSums[taxAmount.AccountID];
                    totalOnVat += transactionItem.Price;
                    taxesSums[taxAmount.AccountID] = transactionItem.Price;
                }


                string currOrderItemLine = transactionItemTemplateLine;
                currOrderItemLine = FixLengthReplace(((double)transactionItem.Quantity).ToString(), currOrderItemLine, "&", null, TextJustify.Right);
                currOrderItemLine = FixLengthReplace(transactionItem.Name, currOrderItemLine, "*");
                currOrderItemLine = FixLengthReplace(GetPriceAsString(transactionItem.Price), currOrderItemLine, "!", "!!", TextJustify.Right);

                foreach (var taxAmount in transactionItem.Taxes)
                    currOrderItemLine = FixLengthReplace((taxAmount.TaxRate * 100).ToString(), currOrderItemLine, "|", null, TextJustify.Right);

                rawPrintingWriter.WriteLine(currOrderItemLine);
            }

            lineString = report.ReadLine();
            string neglineString = "";

            #region writes the receipt footer
            while (lineString != null) //footer
            {

                #region Discount
                if (lineString.IndexOf("?!") != -1)
                {
                    string discDescription = "";
                    if (transaction.DiscountRate != 0)
                    {
                        discDescription = (transaction.DiscountRate * 100).ToString() + "%";
                    }

                    if (transaction.DiscountAmount != 0)
                    {
                        if (string.IsNullOrWhiteSpace(discDescription))
                            discDescription = transaction.DiscountAmount.ToString("C");
                        else
                            discDescription = discDescription + " & " + transaction.DiscountAmount.ToString("C");
                    }

                    lineString = FixLengthReplace(discDescription, lineString, "?!");
                    if (string.IsNullOrWhiteSpace(discDescription))
                        lineString = "";
                }

                #endregion

                // Total
                if (lineString.IndexOf("?") != -1)
                    lineString = FixLengthReplace(GetPriceAsString(transaction.Amount.Amount), lineString, "?");


                //string thankfull message,
                if (lineString.IndexOf("¥") != -1)
                    lineString = FixLengthReplace(companyHeader.Thankfull, lineString, "¥").Trim();


                // Comments
                if (lineString.IndexOf("!@#$%^") != -1)
                {
                    System.IO.StringReader LastString = new System.IO.StringReader(comments);
                    string LastStringline = LastString.ReadLine();
                    if (LastStringline == null)
                        lineString = "";

                    while (LastStringline != null)
                    {
                        string NextLine = LastString.ReadLine();
                        if (NextLine != null)
                            rawPrintingWriter.WriteLine(LastStringline);
                        else
                            lineString = LastStringline;
                        LastStringline = NextLine;
                    }
                }

                // aade document sign 
                if (lineString.IndexOf("!@") != -1)
                {
                    if (transaction.GetPropertyValue("Signature") != null)
                    {

                        string docSignuture = transaction.GetPropertyValue("Signature");
                        do
                        {
                            rawPrintingWriter.WriteLine(FixLengthReplaceWithRemain(docSignuture, lineString, "!@", out docSignuture));
                        }
                        while (docSignuture != null);
                    }
                }
                else if (lineString.IndexOf(" | ") != -1)
                {
                    // taxes sums
                    foreach (var entry in taxesSums)
                    {
                        string VatLine = "" + entry.Key.ToString() + "%   " + GetPriceAsString((double)entry.Value);//entry.Value.ToString();
                        rawPrintingWriter.WriteLine(VatLine);
                    }
                }
                else if (lineString.IndexOf("$qrcode$") != -1)
                {
                    // Qr code
                    if (!string.IsNullOrWhiteSpace(transaction.GetPropertyValue("QRCode")))
                    {
                        lineString = lineString.Replace("$qrcode$", GetQrcodeData(transaction.GetPropertyValue("QRCode")));
                        rawPrintingWriter.WriteLine(lineString);
                    }
                    else
                        lineString = lineString.Replace("$qrcode$", "");

                }
                else
                {
                    rawPrintingWriter.WriteLine(lineString);
                    if (neglineString.Length > 0)
                    {
                        rawPrintingWriter.WriteLine(neglineString);
                        neglineString = "";
                    }
                }
                lineString = report.ReadLine();

            }



            #endregion
            string finalOrderStr = rawPrintingWriter.ToString();


            byte[] outByteStr = System.Text.ASCIIEncoding.Default.GetBytes(finalOrderStr);

            return finalOrderStr;




        }


        public static bool Print(ITransaction transaction, string tableNumber, string ReportPath, bool change, string comments, SignatureData signuture, out string printTxt)
        {


            printTxt = null;
            try
            {

                string Water = "";
                System.Collections.Generic.Dictionary<string, decimal> VatSums = new Dictionary<string, decimal>();

                decimal price = 0;
                decimal negprice = 0;

                string printReport = null;

                byte[] data = Read(typeof(CashierStationDevice.SamtecNext).Assembly.GetManifestResourceStream("CashierStationDevice.Resources.order.bin"));
                printReport = System.Text.Encoding.UTF8.GetString(data, 0, data.Length);




                System.IO.StringWriter printingStringWr = new System.IO.StringWriter();
                System.IO.StringReader report = new System.IO.StringReader(printReport);


                string lineString = report.ReadLine();
                while (lineString != null && lineString.IndexOf("&") == -1)//Header  area
                {
                    if (lineString.IndexOf("#=#") != -1)
                    {
                        if (change == false)
                            lineString = lineString.Replace("#=#", "");
                        else
                            lineString = CenterStringOnLine(lineString, "Order changed");
                    }


                    //Order Code

                    if (lineString.IndexOf("~") != -1)
                    {
                        string orderCode = transaction.GetServiceBatchID();
                        if (string.IsNullOrWhiteSpace(orderCode))
                            lineString = FixLengthReplace(orderCode, lineString, "~");
                        else
                        {
                            lineString = report.ReadLine();
                            continue;
                        }
                    }

                    //table number 
                    if (lineString.IndexOf("@") != -1)
                        lineString = FixLengthReplace(tableNumber, lineString, "@");

                    //waiter
                    if (lineString.IndexOf("+") != -1)
                        lineString = FixLengthReplace(Water, lineString, "+");


                    #region DateTime line
                    if (lineString.IndexOf("^") != -1 || lineString.IndexOf("%") != -1)
                    {
                        int len = lineString.Length;

                        int year = System.DateTime.Now.Year - 2000;

                        if (ReportPath.IndexOf("WaiterClear.txt") == 1)
                            year = DateTime.Now.Year - 2000;

                        string yearstr = year.ToString();
                        if (yearstr.Length == 1)
                            yearstr = "0" + yearstr;

                        if (ReportPath.IndexOf("WaiterClear.txt") == -1)
                            lineString = lineString.Replace("^", string.Format("{0}/{1}/{2}", DateTime.Now.Day, DateTime.Now.Month, yearstr));
                        else
                            lineString = lineString.Replace("^", string.Format("{0}/{1}/{2}", DateTime.Now.Day, DateTime.Now.Month, yearstr));



                        string hourStr = DateTime.Now.Hour.ToString();
                        if (hourStr.Length == 1)
                            hourStr = "0" + hourStr;

                        string minuteStr = DateTime.Now.Minute.ToString();
                        if (minuteStr.Length == 1)
                            minuteStr = "0" + minuteStr;
                        lineString = lineString.Replace("%", string.Format("{0}:{1}", hourStr, minuteStr));
                    }
                    #endregion




                    if (lineString.IndexOf("$") != -1)
                    {
                        string companyName = "";// PropertiesUI.GetCompanyName();
                        int len = (lineString.Length - companyName.Length) / 2;
                        while (len >= 0)
                        {
                            companyName = " " + companyName;
                            len--;
                        }
                        printingStringWr.WriteLine(companyName);
                    }
                    else
                        printingStringWr.WriteLine(lineString);
                    lineString = report.ReadLine();
                }
                //System.Windows.Forms.MessageBox.Show(PageHeader);
                string OrderItemLine = lineString;
                string ingrSpace = null;

                lineString = report.ReadLine();
                while (lineString != null && lineString.IndexOf("##") == -1)
                {
                    ingrSpace += lineString;
                    lineString = report.ReadLine();
                }

                string IngredientLine = lineString;




                foreach (var orderItem in transaction.Items)
                {


                    foreach (var taxAmount in orderItem.Taxes)
                    {
                        if (!VatSums.ContainsKey(taxAmount.AccountID))
                            VatSums.Add(taxAmount.AccountID, 0);

                        decimal totalOnVat = VatSums[taxAmount.AccountID];
                        totalOnVat += orderItem.Price;
                        VatSums[taxAmount.AccountID] = orderItem.Price;
                    }


                    if (orderItem.Price > 0)
                        price += orderItem.Price;
                    else
                        negprice += orderItem.Price;


                    string currOrderItemLine = OrderItemLine;
                    currOrderItemLine = FixLengthReplace(orderItem.Quantity.ToString(), currOrderItemLine, "&");
                    currOrderItemLine = FixLengthReplace(orderItem.Name, currOrderItemLine, "*");
                    currOrderItemLine = FixLengthReplace(GetPriceAsString(orderItem.Price), currOrderItemLine, "!", "!!");
                    printingStringWr.WriteLine(currOrderItemLine);



                }

                lineString = report.ReadLine();
                string neglineString = "";

                while (lineString != null)
                {
                    if (lineString.IndexOf("?") != -1)
                    {
                        if (negprice != 0)
                            neglineString = FixLengthReplace(GetPriceAsString(negprice), lineString, "?");
                        lineString = FixLengthReplace(GetPriceAsString(price), lineString, "?");
                    }

                    if (lineString.IndexOf("!@#$%^") != -1)
                    {



                        System.IO.StringReader LastString = new System.IO.StringReader(comments);
                        string LastStringline = LastString.ReadLine();
                        if (LastStringline == null)
                            lineString = "";

                        while (LastStringline != null)
                        {
                            string NextLine = LastString.ReadLine();
                            if (NextLine != null)
                                printingStringWr.WriteLine(LastStringline);
                            else
                                lineString = LastStringline;
                            LastStringline = NextLine;

                        }

                    }


                    if (lineString.IndexOf("!@") != -1)
                    {
                        if (signuture != null)
                        {
                            string docSignuture = signuture.Signuture;
                            do
                            {
                                printingStringWr.WriteLine(FixLengthReplaceWithRemain(signuture.Signuture, lineString, "!@", out docSignuture));

                            }
                            while (docSignuture != null);
                        }
                    }
                    else if (lineString.IndexOf(" | ") != -1)
                    {
                        foreach (var entry in VatSums)
                        {
                            string VatLine = "" + entry.Key.ToString() + "%   " + GetPriceAsString((double)entry.Value);//entry.Value.ToString();

                            printingStringWr.WriteLine(VatLine);
                        }

                    }
                    else
                    {

                        printingStringWr.WriteLine(lineString);
                        if (neglineString.Length > 0)
                        {
                            printingStringWr.WriteLine(neglineString);
                            neglineString = "";
                        }
                    }
                    lineString = report.ReadLine();

                }



                string finalOrderStr = printingStringWr.ToString();
                if (signuture != null && string.IsNullOrWhiteSpace(signuture.QRCode))
                    finalOrderStr = finalOrderStr.Replace("qrcode", GetQrcodeData(signuture.QRCode));

                //System.IO.Stream OutorderPrintStream = System.IO.File.Open("\\Temp\\OutOrderWin.txt", System.IO.FileMode.OpenOrCreate);
                //OutorderPrintStream.SetLength(0);
                byte[] outByteStr = System.Text.ASCIIEncoding.Default.GetBytes(finalOrderStr);

                //OutorderPrintStream.Write(outByteStr, 0, outByteStr.Length);
                //OutorderPrintStream.Close();
                printTxt = finalOrderStr;
                return true;
                int ewte = 0;
            }
            catch (System.Exception error)
            {
                int tst = 0;
            }

            return false;

        }




        /// <MetaDataID>{661189ff-24a2-4352-ad42-af1762b7bf6e}</MetaDataID>
        internal static byte[] Read(Stream stream)
        {
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, (int)stream.Length);
            return buffer;
        }
        /// <MetaDataID>{a4310878-48f3-4bd7-9871-1271c4dc783c}</MetaDataID>
        private static string FixLengthReplace(string text, string lineString, string signChar, string startSign = null, TextJustify textJustify = TextJustify.Left)
        {
            if (text == null)
                text = "";

            if (startSign == null)
                startSign = signChar;
            int startPos = lineString.IndexOf(startSign);
            int endPos = lineString.LastIndexOf(signChar);


            string TableNumberStr = null;
            if (text.ToString().Length > endPos - startPos + 1)
                TableNumberStr = text.ToString().Substring(0, endPos - startPos + 1);
            else
                TableNumberStr = text.ToString();

            while (TableNumberStr.Length < endPos - startPos + 1)
            {
                if (textJustify == TextJustify.Left)
                    TableNumberStr += " ";
                else
                    TableNumberStr = TableNumberStr.Insert(0, " ");
            }




            string TableChars = null;
            while (startPos <= endPos)
            {
                startPos++;
                TableChars += signChar;
            }
            lineString = lineString.Replace(TableChars, TableNumberStr);
            int rtr = 0;
            return lineString;
        }



        /// <MetaDataID>{597db01e-1ff3-4f60-a6f0-76570d120071}</MetaDataID>
        private static string CenterStringOnLine(string lineString, string stringToCenter)
        {
            int len = (lineString.Length - stringToCenter.Length) / 2;
            while (len >= 0)
            {
                stringToCenter = " " + stringToCenter;
                len--;
            }
            return stringToCenter;
        }




        /// <MetaDataID>{a8674a01-c8c5-4d5c-a9dc-07bed3a898e1}</MetaDataID>
        private static string FixLengthReplaceWithRemain(string inputString, string lineString, string signChar, out string remainString)
        {
            string startSign = null;
            if (startSign == null)
                startSign = signChar;
            int startPos = lineString.IndexOf(startSign);
            int endPos = lineString.LastIndexOf(signChar) + signChar.Length - 1;


            string TableNumberStr = null;
            if (inputString.ToString().Length > endPos - startPos + 1)
            {
                TableNumberStr = inputString.ToString().Substring(0, endPos - startPos + 1);
                remainString = inputString.ToString().Substring(TableNumberStr.Length);
            }
            else
            {
                remainString = null;
                TableNumberStr = inputString.ToString();
            }

            while (TableNumberStr.Length < endPos - startPos + 1)
                TableNumberStr += " ";
            string TableChars = null;
            while (startPos <= endPos)
            {
                startPos += signChar.Length;
                TableChars += signChar;
            }
            lineString = lineString.Replace(TableChars, TableNumberStr);
            int rtr = 0;
            return lineString;
        }

        /// <MetaDataID>{ce5a0191-6c8f-46c8-a1f2-8b0c407a5fa3}</MetaDataID>
        private static string GetPriceAsString(object price)
        {
            return string.Format("{0:N2}", price);
        }





        private static string GetQrcodeData(string qrData)
        {
            //Convert ASCII/Decimal
            string ESC = Convert.ToString((char)27);
            string GS = Convert.ToString((char)29);
            string center = ESC + "a" + (char)1; //align center
            string left = ESC + "a" + (char)0; //align left
            string bold_on = ESC + "E" + (char)1; //turn on bold mode
            string bold_off = ESC + "E" + (char)0; //turn off bold mode
            string cut = ESC + "d" + (char)1 + GS + "V" + (char)66; //add 1 extra line before partial cut
            string initp = ESC + (char)64; //initialize printer
            string buffer = "";// store all the data that want to be printed"; //store all the data that want to be printed

            //buffer += "  .  \n"; //to enter or add newline: \n
            //buffer += left;
            //buffer += "This text is align left!\n"; //align left is already set as default if not specified
            //                                        //Print QRCode

            //Encoding.GetEncoding(1253);
            Encoding m_encoding = Encoding.GetEncoding("iso-8859-1"); //set encoding for QRCode
            int store_len = (qrData).Length + 3;
            byte store_pL = (byte)(store_len % 256);
            byte store_pH = (byte)(store_len / 256);
            buffer += initp; //initialize printer
            buffer += center;
            buffer += m_encoding.GetString(new byte[] { 29, 40, 107, 4, 0, 49, 65, 50, 0 });
            buffer += m_encoding.GetString(new byte[] { 29, 40, 107, 3, 0, 49, 67, 8 });
            buffer += m_encoding.GetString(new byte[] { 29, 40, 107, 3, 0, 49, 69, 48 });
            buffer += m_encoding.GetString(new byte[] { 29, 40, 107, store_pL, store_pH, 49, 80, 48 });
            buffer += qrData;
            buffer += m_encoding.GetString(new byte[] { 29, 40, 107, 3, 0, 49, 81, 48 });
            //Cut Receipt
            //buffer += cut + initp;
            //Send to Printer
            return buffer;
        }
    }

    /// <MetaDataID>{23f6cd11-d207-425b-b22b-e287dc016fad}</MetaDataID>
    [Persistent]
    [BackwardCompatibilityID("{23f6cd11-d207-425b-b22b-e287dc016fad}")]
    public class CompanyHeader
    {
        /// <MetaDataID>{904deb7e-9e69-4ec4-b3ad-531334ee11f5}</MetaDataID>
        string _Title;
        /// <MetaDataID>{53f82f10-dca0-4511-90f8-b3e1158a90d2}</MetaDataID>
        public string Title
        {
            get => _Title;
            set
            {
                if (_Title != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Title = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <MetaDataID>{167876a5-1820-455c-aca4-233c72225fd3}</MetaDataID>
        string _Subtitle;
        /// <MetaDataID>{206009ec-6240-4335-a14f-a3d3590721d5}</MetaDataID>
        public string Subtitle
        {
            get => _Subtitle;
            set
            {
                if (_Subtitle != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Subtitle = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <MetaDataID>{69e25577-f93d-473b-a30e-768430ee626a}</MetaDataID>
        string _ContatInfo;//email telephone etc

        /// <MetaDataID>{c202f2f2-3b32-49f0-95e2-af79bb3e9eae}</MetaDataID>
        public string ContatInfo
        {
            get => _ContatInfo;
            set
            {
                if (_ContatInfo != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ContatInfo = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <MetaDataID>{1590b9d2-c40e-4a8d-a74d-9665d2f977b6}</MetaDataID>
        string _FiscalData;//vat and etc
        /// <MetaDataID>{2a622010-32f9-457a-b4fe-66902f556e9a}</MetaDataID>
        public string FiscalData
        {
            get => _FiscalData;
            set
            {
                if (_FiscalData != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _FiscalData = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <MetaDataID>{f379f0e3-ffd0-4119-8d34-2029d1947605}</MetaDataID>
        string _Address;// defines the company address

        /// <MetaDataID>{e507875b-b5c5-4ef8-a195-61843b537bed}</MetaDataID>
        public string Address
        {
            get => _Address;
            set
            {
                if (_Address != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Address = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <MetaDataID>{8e63f3db-c59c-4ec9-a865-84b8ea7de1b0}</MetaDataID>
        string _Thankfull;
        /// <MetaDataID>{4cb9c309-6d2e-4927-9c56-d42c6f24b011}</MetaDataID>
        public string Thankfull
        {
            get => _Thankfull;
            set
            {
                if (_Thankfull != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Thankfull = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }




    }

    /// <MetaDataID>{12442051-2e56-49cf-9290-742c17dee613}</MetaDataID>
    enum TextJustify
    {
        Left = 1,
        Right = 2,
        Center = 3
    }
}
