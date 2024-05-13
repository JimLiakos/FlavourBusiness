using Azure;
using System;
using System.Collections.Generic;
using System.Data.HashFunction.CRC;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FlavourBusinessManager.Infrastructure
{
    /// <MetaDataID>{7eb57f9f-7bcd-4687-b82b-b30f6debb4eb}</MetaDataID>
    public class GlobalResourceIdentity : Azure.Data.Tables.ITableEntity
    {


        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }

        public string ResourceFullIdentity { get; set; }



        public static string GetPartitionKey(string identity)
        {
            string partitionKey = CRCFactory.Instance.Create(CRCConfig).ComputeHash(System.Text.Encoding.UTF8.GetBytes(identity)).AsHexString();
            return partitionKey;
        }


        public static ICRCConfig CRCConfig
        {
            get
            {
                return System.Data.HashFunction.CRC.CRCConfig.CRC4_ITU;
            }
        }


        static Azure.Data.Tables.TableClient _GlobalResourceIdentityCloudTable;
        public static Azure.Data.Tables.TableClient GlobalResourceIdentityCloudTable
        {
            get
            {
                if (_GlobalResourceIdentityCloudTable == null)
                {

                    var tablesAccount = FlavourBusinessManagerApp.TablesAccount;


                    var table_a = tablesAccount.GetTableClient("GlobalResourceIdentities");

                    Azure.Pageable<Azure.Data.Tables.Models.TableItem> queryTableResults = tablesAccount.Query(String.Format("TableName eq '{0}'", "GlobalResourceIdentities"));
                    bool globalResourceIdentityCloudTable_Exist = queryTableResults.Count() > 0;




                    if (!globalResourceIdentityCloudTable_Exist)
                        table_a.CreateIfNotExists();


                    _GlobalResourceIdentityCloudTable = table_a;
                }
                return _GlobalResourceIdentityCloudTable;
            }
        }

        public static object GlobalResourceIdentityLock = new object();

        public static GlobalResourceIdentity GetGlobalResourceIdentity(string globalResourceFullIdentity, bool create)
        {
            lock (GlobalResourceIdentityLock)
            {
                if (string.IsNullOrWhiteSpace(globalResourceFullIdentity) == null)
                    return null;


                GlobalResourceIdentity globalResourceIdentity = (from a_globalResourceIdentity in GlobalResourceIdentityCloudTable.Query<GlobalResourceIdentity>()
                                                                 where a_globalResourceIdentity.ResourceFullIdentity == globalResourceFullIdentity
                                                                 select a_globalResourceIdentity).FirstOrDefault();


                if (globalResourceIdentity == null && create)
                {



                    var ticks = new DateTime(2022, 1, 1).Ticks;
                    var uniqueId = (DateTime.Now.Ticks - ticks).ToString("x");

                    string partitionKey = GetPartitionKey(uniqueId);

                    int numOfTries = 5;
                    while (numOfTries > 0)
                    {
                        globalResourceIdentity = new GlobalResourceIdentity() { PartitionKey = partitionKey, RowKey = uniqueId, ResourceFullIdentity = globalResourceFullIdentity };
                        try
                        {
                            var result = GlobalResourceIdentityCloudTable.AddEntity(globalResourceIdentity);
                            return globalResourceIdentity;
                        }
                        catch (RequestFailedException error)
                        {
                            if (error.ErrorCode != "EntityAlreadyExists")
                                throw;
                        }
                        uniqueId = (DateTime.Now.Ticks - ticks).ToString("x");

                        System.Threading.Thread.Sleep(100);
                        numOfTries--;
                        globalResourceIdentity = (from a_globalResourceIdentity in GlobalResourceIdentityCloudTable.Query<GlobalResourceIdentity>()
                                                  where a_globalResourceIdentity.ResourceFullIdentity == globalResourceFullIdentity
                                                  select a_globalResourceIdentity).FirstOrDefault();

                        if (globalResourceIdentity != null)
                            return globalResourceIdentity;

                    }

                }
                else
                {
                    return globalResourceIdentity;
                }

                return globalResourceIdentity;
            }
        }

        public static GlobalResourceIdentity GetGlobalResourceIdentityFor(string identity)
        {
            lock (GlobalResourceIdentityLock)
            {
                if (string.IsNullOrWhiteSpace(identity) == null)
                    return null;

                string partitionKey = GetPartitionKey(identity);

                GlobalResourceIdentity globalResourceIdentity = (from a_globalResourceIdentity in GlobalResourceIdentityCloudTable.Query<GlobalResourceIdentity>()
                                                                 where a_globalResourceIdentity.PartitionKey == partitionKey && a_globalResourceIdentity.RowKey == identity
                                                                 select a_globalResourceIdentity).FirstOrDefault();

                return globalResourceIdentity;



            }
        }
    }
}
