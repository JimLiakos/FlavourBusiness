using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Data.HashFunction.CRC;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlavourBusinessManager
{
    /// <MetaDataID>{a9076ab6-17a4-4cdc-8b3b-3f54956fcd5c}</MetaDataID>
    public class DeviceIDAbbreviation : Microsoft.Azure.Cosmos.Table.TableEntity
    {
        public static string GetAbbreviation(string deviceId)
        {
            string abbreviation = "101"+CRCFactory.Instance.Create(CRCConfig.CRC32).ComputeHash(System.Text.Encoding.UTF8.GetBytes(deviceId)).AsHexString().ToUpper();


            DeviceIDAbbreviation deviceIDAbbreviation = (from deviceIDAbbreviationEntry in DeviceIDAbbreviationTable.CreateQuery<DeviceIDAbbreviation>()
                                                         where deviceIDAbbreviationEntry.PartitionKey == "101" && deviceIDAbbreviationEntry.RowKey == abbreviation
                                                         select deviceIDAbbreviationEntry).FirstOrDefault();

            if (deviceIDAbbreviation == null)
            {
                deviceIDAbbreviation = new DeviceIDAbbreviation() { PartitionKey = "101", RowKey = abbreviation, DeviceId=deviceId };
                TableOperation insertOperation = TableOperation.Insert(deviceIDAbbreviation);
                try
                {
                    var result = DeviceIDAbbreviationTable.Execute(insertOperation);
                }
                catch (Exception error)
                {

                    return null;
                }
                return abbreviation;
            }


            return null;
        }

        public string DeviceId { get; set; }


        /// <exclude>Excluded</exclude>
        static Azure.Data.Tables.TableClient _DeviceIDAbbreviationTable;
        /// <MetaDataID>{8aa0e63d-2722-4cb9-b67a-c0f0c813a854}</MetaDataID>
        static Azure.Data.Tables.TableClient DeviceIDAbbreviationTable
        {
            get
            {
                if (_DeviceIDAbbreviationTable == null)
                {
                    //CloudStorageAccount account = FlavourBusinessManagerApp.CloudTableStorageAccount;
                    var tablesAccount = FlavourBusinessManagerApp.CloudTableStorageAccount_a;
                    //if ((string.IsNullOrWhiteSpace(FlavourBusinessManagerApp.FlavourBusinessStoragesAccountName) || FlavourBusinessManagerApp.FlavourBusinessStoragesAccountName == "devstoreaccount1"))
                    //    account = CloudStorageAccount.DevelopmentStorageAccount;
                    //else
                    //    account = new CloudStorageAccount(new StorageCredentials(FlavourBusinessManagerApp.FlavourBusinessStoragesAccountName, FlavourBusinessManagerApp.FlavourBusinessStoragesAccountkey), true);

                    //CloudTableClient tableClient = account.CreateCloudTableClient();
                    // CloudTable table = tableClient.GetTableReference("DeviceIDAbbreviation");
                    Azure.Data.Tables.TableClient table = tablesAccount.GetTableClient("DeviceIDAbbreviation");
                    Azure.Pageable<Azure.Data.Tables.Models.TableItem> queryTableResults = tablesAccount.Query(String.Format("TableName eq '{0}'", "DeviceIDAbbreviation"));
                    bool deviceIDAbbreviation_exist = queryTableResults.Count() > 0;
                    if (!deviceIDAbbreviation_exist)
                        table.CreateIfNotExists();

                    //if (!table.Exists())
                    //    table.CreateIfNotExists();
                    _DeviceIDAbbreviationTable = table;
                }
                return _DeviceIDAbbreviationTable;
            }
        }
    }
}
