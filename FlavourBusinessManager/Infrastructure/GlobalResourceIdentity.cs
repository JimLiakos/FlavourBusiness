using Azure;
using Azure.Data.Tables;
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
        public DateTime? ExpiringDateTime { get; set; }



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



                    //var ticks = new DateTime(2022, 1, 1).Ticks;
                    //var uniqueId = (DateTime.Now.Ticks - ticks).ToString("x");

                    var uniqueId = RandomIdGenerator.GetBase62(10);

                    string partitionKey = GetPartitionKey(uniqueId);

                    int numOfTries = 15;
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
                        //uniqueId = (DateTime.Now.Ticks - ticks).ToString("x");
                        uniqueId = RandomIdGenerator.GetBase62(10);
                        partitionKey = GetPartitionKey(uniqueId);

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



        public static GlobalResourceIdentity GetExpiringGlobalResourceIdentity(string globalResourceFullIdentity, int expiringTimespan)
        {
            bool create = true;
            lock (GlobalResourceIdentityLock)
            {

                if (string.IsNullOrWhiteSpace(globalResourceFullIdentity) == null)
                    return null;

                GlobalResourceIdentity globalResourceIdentity = (from a_globalResourceIdentity in GlobalResourceIdentityCloudTable.Query<GlobalResourceIdentity>()
                                                                 where a_globalResourceIdentity.ResourceFullIdentity == globalResourceFullIdentity &&
                                                                  a_globalResourceIdentity.ExpiringDateTime > DateTime.UtcNow 
                                                                 select a_globalResourceIdentity).FirstOrDefault();



                try
                {


                    string utcNowAsFilterString = (DateTime.UtcNow ).ToString("yyyy-MM-ddTHH:mm:ssZ");

                    var expiredGlobalResourceIdentities = (from a_globalResourceIdentity in GlobalResourceIdentityCloudTable.Query<GlobalResourceIdentity>($"PartitionKey  eq 'EXPIRING' and ExpiringDateTime lt '{utcNowAsFilterString}'")
                                                           select a_globalResourceIdentity).ToList();
                    List<TableTransactionAction> actions = new List<TableTransactionAction>();
                    foreach (var identity in expiredGlobalResourceIdentities)
                    {
                        //ITableEntity tt=identity.RowKey
                        actions.Add(new TableTransactionAction(TableTransactionActionType.Delete, identity));
                        if (actions.Count == 100)
                        {
                            GlobalResourceIdentityCloudTable.SubmitTransaction(actions);
                            actions = new List<TableTransactionAction>();
                        }

                    }
                    if (actions.Count > 0)
                    {
                        GlobalResourceIdentityCloudTable.SubmitTransaction(actions);
                        actions = new List<TableTransactionAction>();
                    }
                }
                catch (Exception error)
                {
                }

                


                if (globalResourceIdentity == null && create)
                {

                    //var ticks = new DateTime(2022, 1, 1).Ticks;
                    //var uniqueId = (DateTime.Now.Ticks - ticks).ToString("x");


                    var uniqueId = RandomIdGenerator.GetBase62(10);

                    string partitionKey = "EXPIRING";// GetPartitionKey(uniqueId);

                    int numOfTries = 15;
                    while (numOfTries > 0)
                    {
                        globalResourceIdentity = new GlobalResourceIdentity() { PartitionKey = partitionKey, RowKey = uniqueId, ResourceFullIdentity = globalResourceFullIdentity };
                        try
                        {
                            globalResourceIdentity.ExpiringDateTime = DateTime.UtcNow + TimeSpan.FromMinutes(expiringTimespan);
                            var result = GlobalResourceIdentityCloudTable.AddEntity(globalResourceIdentity);
                            return globalResourceIdentity;
                        }
                        catch (RequestFailedException error)
                        {
                            if (error.ErrorCode != "EntityAlreadyExists")
                                throw;
                        }
                        //uniqueId = (DateTime.Now.Ticks - ticks).ToString("x");
                        uniqueId = RandomIdGenerator.GetBase62(10);

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


        public static GlobalResourceIdentity GetGlobalResourceIdentityForExpiringKey(string identity)
        {
            lock (GlobalResourceIdentityLock)
            {
                if (string.IsNullOrWhiteSpace(identity) == null)
                    return null;

                string partitionKey = "EXPIRING";// GetPartitionKey(identity);

                GlobalResourceIdentity globalResourceIdentity = (from a_globalResourceIdentity in GlobalResourceIdentityCloudTable.Query<GlobalResourceIdentity>()
                                                                 where a_globalResourceIdentity.PartitionKey == partitionKey && a_globalResourceIdentity.RowKey == identity
                                                                 select a_globalResourceIdentity).FirstOrDefault();

                return globalResourceIdentity;



            }
        }
    }


    /// <MetaDataID>{9870a468-a2e7-429c-bae8-5955282236b3}</MetaDataID>
    public static class RandomIdGenerator
    {
        private static char[] _base62chars =
            "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz"
            .ToCharArray();

        private static Random _random = new Random();

        public static string GetBase62(int length)
        {
            var sb = new StringBuilder(length);

            for (int i = 0; i < length; i++)
                sb.Append(_base62chars[_random.Next(62)]);

            return sb.ToString();
        }

        public static string GetBase36(int length)
        {
            var sb = new StringBuilder(length);

            for (int i = 0; i < length; i++)
                sb.Append(_base62chars[_random.Next(36)]);

            return sb.ToString();
        }
    }
}
