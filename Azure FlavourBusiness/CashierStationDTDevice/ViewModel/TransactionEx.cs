using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CashierStationDevice
{
    /// <MetaDataID>{4fb2ce31-3a6d-4478-829e-062a8e167168}</MetaDataID>
    public static class TransactionEx
    {
        public static string GetServiceBatchID(this FinanceFacade.ITransaction tranasaction)
        {
            return tranasaction.GetPropertyValue("ServiceBatchID");
        }
    
        public static void SetServiceBatchID(this FinanceFacade.ITransaction tranasaction, string series)
        {
            tranasaction.SetPropertyValue("ServiceBatchID", series);
        }


        public static string GetSeries(this FinanceFacade.ITransaction tranasaction)
        {
            return tranasaction.GetPropertyValue("Series");
        }
        /// <MetaDataID>{4bee4e30-80a3-4a07-bdb2-2fe8facecd0c}</MetaDataID>
        public static void SetSeries(this FinanceFacade.ITransaction tranasaction, string series)
        {
            tranasaction.SetPropertyValue("Series", series);
        }


        public static string GetRawPrinterAddress(this FinanceFacade.ITransaction tranasaction)
        {
            return tranasaction.GetPropertyValue("RawPrinterAddress");
        }
        public static void SetRawPrinterAddress(this FinanceFacade.ITransaction tranasaction, string RawPrinterAddress)
        {
            tranasaction.SetPropertyValue("RawPrinterAddress", RawPrinterAddress);
        }

        /// <MetaDataID>{3d6d0d23-cc6b-47b4-90c7-8f0d75925cdd}</MetaDataID>
        public static int? GetAutoNumber(this FinanceFacade.ITransaction tranasaction)
        {
            string value = tranasaction.GetPropertyValue("AutoNumber");
            if (value == null)
                return null;
            int autoNumber = 0;
            if (int.TryParse(value, out autoNumber))
                return autoNumber;
            else
                return null;
        }
        /// <MetaDataID>{de28e577-f2d8-4227-92e0-adad0adad4cd}</MetaDataID>
        public static void SetAutoNumber(this FinanceFacade.ITransaction tranasaction, int autoNumber)
        {
            tranasaction.SetPropertyValue("AutoNumber", autoNumber.ToString());
        }

        public static int? GetCodePage(this FinanceFacade.ITransaction tranasaction)
        {
            string value = tranasaction.GetPropertyValue("CodePage");
            if (value == null)
                return null;
            int codePage = 0;
            if (int.TryParse(value, out codePage))
                return codePage;
            else
                return null;
        }

      
        public static void SetCodePage(this FinanceFacade.ITransaction tranasaction, int codePage)
        {
            tranasaction.SetPropertyValue("CodePage", codePage.ToString());
        }

        public static void SetTransactionPrinted(this FinanceFacade.ITransaction tranasaction, bool transactionPrinted)
        {
            tranasaction.SetPropertyValue("TransactionPrinted", transactionPrinted.ToString());
        }

        public static bool IsTransactionPrinted(this FinanceFacade.ITransaction tranasaction)
        {
            string value = tranasaction.GetPropertyValue("TransactionPrinted");
            if (value == null)
                return false;
            bool transactionPrinted=false;
            if (bool.TryParse(value, out transactionPrinted))
                return transactionPrinted;
            else
                return false;
        }

        /// <MetaDataID>{879f703e-28aa-473d-bb8e-0caca8edb26a}</MetaDataID>

        /// <MetaDataID>{1f04f6f2-dba1-4571-99e6-4edcf9c02ec6}</MetaDataID>
        public static string GetTransactionTypeID(this FinanceFacade.ITransaction tranasaction)
        {
            return tranasaction.GetPropertyValue("TransactionTypeID");
        }
        /// <MetaDataID>{96d9e7b4-a2a4-4daa-9115-611815228062}</MetaDataID>
        public static void SetTransactionTypeID(this FinanceFacade.ITransaction tranasaction, string transactionTypeID)
        {
            tranasaction.SetPropertyValue("TransactionTypeID", transactionTypeID);
        }

        public static void Save(this FinanceFacade.ITransaction transaction,string folder)
        {
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            string transactionFileName = r.Replace(transaction.Uri, "_").Replace("-", "");

            var path = folder + "\\" + transactionFileName + ".json";
            string turi = transaction.Uri;
            var jsonSerializerSettings = new OOAdvantech.Json.JsonSerializerSettings() { TypeNameHandling = OOAdvantech.Json.TypeNameHandling.All };
            string json = OOAdvantech.Json.JsonConvert.SerializeObject(transaction, jsonSerializerSettings);

            File.WriteAllText(path, json);
        }
        public static void Move(this FinanceFacade.ITransaction transaction, string fromFolder,string toFolder)
        {
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            string transactionFileName = r.Replace(transaction.Uri, "_").Replace("-", "");

            var source = fromFolder + "\\" + transactionFileName + ".json";
            var target = toFolder + "\\" + transactionFileName + ".json";
            if (!Directory.Exists(toFolder))
                Directory.CreateDirectory(toFolder);
            if (File.Exists(target))
                File.Delete(target);
            File.Move(source, target);

         
        }
    }
}
