using FinanceFacade;
using FlavourBusinessFacade;
using FlavourBusinessFacade.ServicesContextResources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashierStationDevice
{
    /// <MetaDataID>{82be5194-83d3-4038-b878-d6225ab658cb}</MetaDataID>
    public class CashierController
    {
        /// <MetaDataID>{af4a528e-ef65-4e03-b887-e0ba7e094e13}</MetaDataID>
        static string AzureServerUrl = string.Format("http://{0}:8090/api/", FlavourBusinessFacade.ComputingResources.EndPoint.Server);

        ICashiersStationRuntime CashiersStation;
        /// <MetaDataID>{d76d5629-dbbc-4583-8610-6cd79f7f3c2c}</MetaDataID>
        public void Start()
        {
            string assemblyData = "FlavourBusinessManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
            string type = "FlavourBusinessManager.FlavoursServicesContextManagment";
            string serverUrl = AzureServerUrl;
            if (string.IsNullOrWhiteSpace(ApplicationSettings.Current.CommunicationCredentialKey))
                return;
            IFlavoursServicesContextManagment servicesContextManagment = OOAdvantech.Remoting.RestApi.RemotingServices.CastTransparentProxy<IFlavoursServicesContextManagment>(OOAdvantech.Remoting.RestApi.RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData));
            CashiersStation = servicesContextManagment.GetCashiersStationRuntime(ApplicationSettings.Current.CommunicationCredentialKey);
            try
            {




                CashiersStation.OpenTransactions += CashiersStation_OpenTransactions;
                var transctios = CashiersStation.GetOpenTransactions(null);


                

                foreach (var transaction in transctios)
                    AddPendingTransactionsForPrint(transaction);


            }
            catch (Exception error)
            {

                if (error.HResult == 800)
                {
                    if (!System.Diagnostics.EventLog.SourceExists("CashierStationDevice", "."))
                        System.Diagnostics.EventLog.CreateEventSource("CashierStationDevice", "Microneme");
                    System.Diagnostics.EventLog myLog = new System.Diagnostics.EventLog();
                    myLog.Source = "CashierStationDevice";
                    myLog.WriteEntry("There is already attached device to the cashier station", System.Diagnostics.EventLogEntryType.Error);

                    throw new CashierStationDeviceException("Only one cashier station device allowed to subscribe");
                }
            }
        }

        private void AddPendingTransactionsForPrint(ITransaction transaction)
        {

            var transactionPrinter = ApplicationSettings.Current.TransactionsPrinters.Where(x => x.IsDefault).FirstOrDefault();
            if(transactionPrinter!=null)
            {
                var jsonSerializerSettings = new OOAdvantech.Json.JsonSerializerSettings() { TypeNameHandling = OOAdvantech.Json.TypeNameHandling.All };
                string json = OOAdvantech.Json.JsonConvert.SerializeObject(transaction, jsonSerializerSettings);
               // ApplicationSettings.AppDataPath

               //var transactiona=  OOAdvantech.Json.JsonConvert.DeserializeObject<ITransaction>(json, jsonSerializerSettings);


            }
        }

        private void CashiersStation_OpenTransactions(ICashiersStationRuntime sender, string deviceUpdateEtag)
        {
            var transctios = CashiersStation.GetOpenTransactions(deviceUpdateEtag);

            foreach (var transaction in transctios)
                AddPendingTransactionsForPrint(transaction);


        }


        List<ITransaction> PendingTransactionsForPrint = new List<ITransaction>();

        public static Dictionary<string, int> VatAcounts = new Dictionary<string, int>{
            {"a1",0},
            {"b1",1},
            {"c1",2},
            {"d1",3},
            {"C54-00-70-0006",0},
            {"C54-00-70-0013",1},
            {"C54-00-70-0024",2},
            {"C54-00-70-0036",3},
            {"C54-00-70-0000",4},
            {"C54-00-79-0004",0},
            {"C54-00-79-0009",1},
            {"C54-00-79-0017",2},
            {"C54-00-79-0025",3}
        };

        public void PrintReceipt(ITransaction transaction, IDocumentSignDevice documentSigner)
        {

            string printText = null;
            Print(transaction, "13", "", false, "", null, out printText);

            string myafm = transaction.PayeeRegistrationNumber;
            string clientafm = "";
            string linikiID = "173";
            string series = "";
            string taxDocNumber = "12";
            decimal net_a = 0;
            decimal net_b = 0;
            decimal net_c = 0;
            decimal net_d = 0;
            decimal net_e = 0;
            decimal vat_a = 0;
            decimal vat_b = 0;
            decimal vat_c = 0;
            decimal vat_d = 0;
            decimal total_to_pay_poso = 0;

            foreach (var item in transaction.Items)
            {
                total_to_pay_poso += item.Price;

                if (VatAcounts[item.Taxes[0].AccountID] == 0)
                {
                    vat_a += item.Taxes[0].Amount;
                    net_a += item.Price - item.Taxes[0].Amount;
                }
                if (VatAcounts[item.Taxes[0].AccountID] == 1)
                {
                    vat_b += item.Taxes[0].Amount;
                    net_b += item.Price - item.Taxes[0].Amount;
                }

                if (VatAcounts[item.Taxes[0].AccountID] == 2)
                {
                    vat_c += item.Taxes[0].Amount;
                    net_c += item.Price - item.Taxes[0].Amount;
                }

                if (VatAcounts[item.Taxes[0].AccountID] == 3)
                {
                    vat_d += item.Taxes[0].Amount;
                    net_d += item.Price - item.Taxes[0].Amount;
                }

            }

            string afdsDoc = printText;

            string epsilon_line = string.Format(System.Globalization.CultureInfo.GetCultureInfo(1033), "{0};{1};;{2};{3};{4};", myafm, clientafm, linikiID, series, taxDocNumber);
            epsilon_line += string.Format(System.Globalization.CultureInfo.GetCultureInfo(1033), "{0:N2};{1:N2};{2:N2};{3:N2};{4:N2};", net_a, net_b, net_c, net_d, net_e);
            epsilon_line += string.Format(System.Globalization.CultureInfo.GetCultureInfo(1033), "{0:N2};{1:N2};{2:N2};{3:N2};", vat_a, vat_b, vat_c, vat_d);
            epsilon_line += string.Format(System.Globalization.CultureInfo.GetCultureInfo(1033), "{0:N2};0;;;", total_to_pay_poso);
            afdsDoc += documentSigner.PrepareEpsilonLine("[<]" + epsilon_line + "[>]");
            SignatureData signature = documentSigner.SignDocument(afdsDoc);
            if (string.IsNullOrWhiteSpace(signature.Signuture))
            {

            }
            else
                Print(transaction, "13", "", false, "", signature, out printText);

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

                byte[] data = Read(typeof(CashierStationDevice.DocumentSignDevice).Assembly.GetManifestResourceStream("CashierStationDevice.Resources.order.bin"));
                printReport = System.Text.Encoding.UTF8.GetString(data, 0, data.Length);




                System.IO.StringWriter printingStringWr = new System.IO.StringWriter();
                System.IO.StringReader report = new System.IO.StringReader(printReport);


                string lineString = report.ReadLine();
                while (lineString != null && lineString.IndexOf("&") == -1)
                {
                    if (lineString.IndexOf("#=#") != -1)
                    {
                        if (change == false)
                            lineString = lineString.Replace("#=#", "");
                        else
                            lineString = CenterStringOnLine(lineString, "Order changed");
                    }

                    //Order Code
                    string orderCode = "AA12";
                    if (lineString.IndexOf("~") != -1)
                        lineString = FixLengthReplace(orderCode, lineString, "~");

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
        private static byte[] Read(Stream stream)
        {
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, (int)stream.Length);
            return buffer;
        }
        /// <MetaDataID>{a4310878-48f3-4bd7-9871-1271c4dc783c}</MetaDataID>
        private static string FixLengthReplace(string tableNumber, string lineString, string signChar, string startSign = null)
        {
            if (startSign == null)
                startSign = signChar;
            int startPos = lineString.IndexOf(startSign);
            int endPos = lineString.LastIndexOf(signChar);


            string TableNumberStr = null;
            if (tableNumber.ToString().Length > endPos - startPos + 1)
                TableNumberStr = tableNumber.ToString().Substring(0, endPos - startPos + 1);
            else
                TableNumberStr = tableNumber.ToString();

            while (TableNumberStr.Length < endPos - startPos + 1)
                TableNumberStr += " ";
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
}
