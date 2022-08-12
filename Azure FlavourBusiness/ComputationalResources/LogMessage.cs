using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalResources
{
    /// <MetaDataID>{c915eefe-2ee8-46c3-86dd-e28d97c48653}</MetaDataID>
    public class ErrorLog : OOAdvantech.MetaDataRepository.IErrorLog
    {
        public void WriteError(string message)
        {
            LogMessage.WriteLog(message);
        }
    }
    /// <MetaDataID>{6bd32a19-05c7-4413-9f3a-df92fbbc2b18}</MetaDataID>
    public class LogMessage : Azure.Data.Tables.ITableEntity
    {


        public static Azure.Data.Tables.TableServiceClient TablesAccount;

        public static Azure.Data.Tables.TableClient LogMessageTable ;

        public static object LogMessageLock = new object();

        public string Message { get; set; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        public static void WriteLog(string message)
        {
            try
            {
                lock (LogMessageLock)
                {
                    if (TablesAccount == null)
                    {
                        Uri endPoint = new Uri(string.Format("https://{0}.table.core.windows.net", "angularhost"));
                        TablesAccount = new Azure.Data.Tables.TableServiceClient(endPoint, new Azure.Data.Tables.TableSharedKeyCredential("angularhost", "YxNQAvlMWX7e7Dz78w/WaV3Z9VlISStF+Xp2DGigFScQmEuC/bdtiFqKqagJhNIwhsgF9aWHZIcpnFHl4bHHKw=="));
                        LogMessageTable = TablesAccount.GetTableClient("LogMessage");
                        Pageable<Azure.Data.Tables.Models.TableItem> queryTableResults = TablesAccount.Query(String.Format("TableName eq '{0}'", "LogMessage"));
                        bool LogMessage_exist = queryTableResults.Count() > 0;
                        if (!LogMessage_exist)
                            LogMessageTable.CreateIfNotExists();

                    }



                    LogMessage logMessage = new LogMessage();
                    logMessage.PartitionKey = "AAA";
                    logMessage.RowKey = Guid.NewGuid().ToString();
                    logMessage.Message = message;

                    LogMessageTable.AddEntity(logMessage); 
                }
                //TableOperation insertOperation = TableOperation.Insert(logMessage);
                //var executeResult = logMessageTable.Execute(insertOperation);
            }
            catch (Exception error)
            {
            }



        }
    }
}
