using Microsoft.Azure.Cosmos.Table;
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
    public class LogMessage : Microsoft.Azure.Cosmos.Table.TableEntity
    {


        public static CloudStorageAccount CloudTableStorageAccount { get; private set; }
        public static CloudTableClient TableClient { get; private set; }
        public string Message { get; set; }


        public static void WriteLog(string message)
        {
            try
            {
                if (CloudTableStorageAccount == null)
                    CloudTableStorageAccount = new Microsoft.Azure.Cosmos.Table.CloudStorageAccount(new Microsoft.Azure.Cosmos.Table.StorageCredentials("angularhost", "YxNQAvlMWX7e7Dz78w/WaV3Z9VlISStF+Xp2DGigFScQmEuC/bdtiFqKqagJhNIwhsgF9aWHZIcpnFHl4bHHKw=="), true);

                if (TableClient == null)
                    TableClient = CloudTableStorageAccount.CreateCloudTableClient();

                CloudTable logMessageTable = TableClient.GetTableReference("LogMessage");
                if (!logMessageTable.Exists())
                    logMessageTable.CreateIfNotExists();

                LogMessage logMessage = new LogMessage();
                logMessage.PartitionKey = "AAA";
                logMessage.RowKey = Guid.NewGuid().ToString();
                logMessage.Message = message;


                TableOperation insertOperation = TableOperation.Insert(logMessage);
                var executeResult = logMessageTable.Execute(insertOperation);
            }
            catch (Exception error)
            {
            }



        }
    }
}
