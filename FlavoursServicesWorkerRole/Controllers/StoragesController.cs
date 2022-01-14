using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Microsoft.Azure.Cosmos.Table;

namespace FlavoursServicesWorkerRole.Controllers
{

    /// <summary>
    /// Storages
    /// </summary>
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class StoragesController : ApiController
    {
     /// <summary>
     /// get storage meta data
     /// </summary>
     /// <param name="storageIdentity"></param>
     /// <returns>
     /// Return storage meta data
     /// </returns>
        public OOAdvantech.MetaDataRepository.StorageMetaData Get(string storageIdentity)
        {
            try
            {
                CloudStorageAccount account = FlavourBusinessManager.FlavourBusinessManagerApp.CloudTableStorageAccount;

                //if (StorageLocation.ToLower() == @"DevStorage".ToLower() && string.IsNullOrWhiteSpace(userName))
                //account = FlavourBusinessManager.RawStorageCloudBlob.CloudBlobStorageAccount;
                //else
                //    account = new CloudStorageAccount(new StorageCredentials(userName, userName), StorageLocation, true);

                CloudTableClient tableClient = account.CreateCloudTableClient();
                CloudTable table = tableClient.GetTableReference("PublicStorageMetaDataEntry");
                if (!table.Exists())
                    table.CreateIfNotExists();

                PublicStorageMetaDataEntry storageMetadataEntry = (from publicStorageMetadaEntry in table.CreateQuery<PublicStorageMetaDataEntry>()
                                                                   where publicStorageMetadaEntry.RowKey == storageIdentity
                                                                   select publicStorageMetadaEntry).FirstOrDefault();

                if (storageMetadataEntry != null)
                {
                    return new OOAdvantech.MetaDataRepository.StorageMetaData()
                    {
                        MultipleObjectContext = storageMetadataEntry.MultipleObjectContext,
                        NativeStorageID = storageMetadataEntry.NativeStorageID,
                        StorageIdentity = storageMetadataEntry.RowKey,
                        StorageLocation = storageMetadataEntry.StorageLocation,
                        StorageName = storageMetadataEntry.StorageName,
                        StorageType = storageMetadataEntry.StorageType
                    };
                }
                else
                    return null;
            }
            catch (Exception error)
            {

                throw;
            }
        }

        public string Post([FromBody]OOAdvantech.MetaDataRepository.StorageMetaData storageMetaData)
        {

            CloudStorageAccount account = FlavourBusinessManager.FlavourBusinessManagerApp.CloudTableStorageAccount;

            //if (StorageLocation.ToLower() == @"DevStorage".ToLower() && string.IsNullOrWhiteSpace(userName))
            //account = CloudStorageAccount.DevelopmentStorageAccount;
            //else
            //    account = new CloudStorageAccount(new StorageCredentials(userName, userName), StorageLocation, true);

            CloudTableClient tableClient = account.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("PublicStorageMetaDataEntry");
            if (!table.Exists())
                table.CreateIfNotExists();


            PublicStorageMetaDataEntry storageMetadataEntry = (from publicStorageMetadaEntry in table.CreateQuery<PublicStorageMetaDataEntry>()
                                                               where publicStorageMetadaEntry.RowKey == storageMetaData.StorageIdentity
                                                               select publicStorageMetadaEntry).FirstOrDefault();


            if (storageMetadataEntry == null)
            {
                storageMetadataEntry = new PublicStorageMetaDataEntry();
                storageMetadataEntry.PartitionKey = "AAA";
                storageMetadataEntry.RowKey = storageMetaData.StorageIdentity;
                storageMetadataEntry.StorageName = storageMetaData.StorageName;
                storageMetadataEntry.StorageLocation = storageMetaData.StorageLocation;
                storageMetadataEntry.StorageType = storageMetaData.StorageType;
                storageMetadataEntry.NativeStorageID = storageMetaData.NativeStorageID;
                storageMetadataEntry.MultipleObjectContext = storageMetaData.MultipleObjectContext;
                TableOperation insertOperation = TableOperation.Insert(storageMetadataEntry);
                var result = table.Execute(insertOperation);
                return "";


                //storageMetadataEntry.ServerEndPoints = storageMetaData.ServerEndPoints


            }
            else
                return "Already exist storage meta data entry";




        }


    }


    public class PublicStorageMetaDataEntry : Microsoft.Azure.Cosmos.Table.TableEntity
    {
        public bool MultipleObjectContext { get; set; }
        public string NativeStorageID { get; set; }
        public string StorageLocation { get; set; }
        public string StorageName { get; set; }
        public string StorageType { get; set; }
    }

}
