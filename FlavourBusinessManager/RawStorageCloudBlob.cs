using System;
using System.IO;
using System.Xml.Linq;
using System.Net.Http;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.Storage.Auth;
using Microsoft.Azure.Storage;

namespace FlavourBusinessManager
{
    /// <MetaDataID>{8ed3c48d-e699-489f-bbb7-68a40c1f3e54}</MetaDataID>
    public class RawStorageCloudBlob : OOAdvantech.PersistenceLayer.IRawStorageData
    {
        //static string AzureStorageUrl = "http://192.168.2.8";
        //static string AzureStorageUrl = "http://192.168.2.5";
        //static string AzureStorageUrl = "http://192.168.2.5";

        //static string AzureStorageUrl = "http://192.168.2.8";//org
        //static string AzureStorageUrl = "http://192.168.2.4";//Braxati
        //static string AzureStorageUrl = "http://10.0.0.13";//work
        static string AzureStorageUrl = string.Format("http://{0}", FlavourBusinessFacade.ComputingResources.EndPoint.Server);





        public static string RootUri
        {
            get
            {
                string azureStorageUrl = OOAdvantech.Remoting.RestApi.RemotingServices.GetLocalIPAddress();
                if (azureStorageUrl == null)
                    azureStorageUrl = AzureStorageUrl;
                else
                    azureStorageUrl = "http://" + azureStorageUrl;



#if DEBUG

                return azureStorageUrl + RawStorageCloudBlob.CloudBlobStorageAccount.BlobEndpoint.AbsolutePath;

                //return "https://angularhost.z16.web.core.windows.net";
#else
                return azureStorageUrl + RawStorageCloudBlob.CloudBlobStorageAccount.BlobEndpoint.AbsolutePath;
#endif
            }
        }



        //public static Microsoft.Azure.Storage.CloudStorageAccount CloudBlobStorageAccount = Microsoft.Azure.Storage.CloudStorageAccount.DevelopmentStorageAccount;

        static Microsoft.Azure.Storage.CloudStorageAccount CloudBlobStorageAccount
        {
            get
            {
                return FlavourBusinessManagerApp.CloudBlobStorageAccount;
                //if (_CloudBlobStorageAccount == null)
                //    _CloudBlobStorageAccount = new CloudStorageAccount(new StorageCredentials(FlavourBusinessManagerApp.FlavourBusinessStoragesAccountName, FlavourBusinessManagerApp.FlavourBusinessStoragesAccountkey), true);
                //return _CloudBlobStorageAccount;
            }
        }



        //public static Microsoft.Azure.Cosmos.Table.CloudStorageAccount CloudTableStorageAccount = Microsoft.Azure.Cosmos.Table.CloudStorageAccount.DevelopmentStorageAccount;
        //static Microsoft.Azure.Cosmos.Table.CloudStorageAccount _CloudTableStorageAccount;
        static Microsoft.Azure.Cosmos.Table.CloudStorageAccount CloudTableStorageAccount
        {
            get
            {
                return FlavourBusinessManagerApp.CloudTableStorageAccount;
                //if (_CloudTableStorageAccount == null)
                //    _CloudTableStorageAccount = new Microsoft.Azure.Cosmos.Table.CloudStorageAccount(new Microsoft.Azure.Cosmos.Table.StorageCredentials(FlavourBusinessManagerApp.FlavourBusinessStoragesAccountName, FlavourBusinessManagerApp.FlavourBusinessStoragesAccountkey), true);

                //return _CloudTableStorageAccount;
            }
        }



        //public string RootContainer=null;
        public static string RootContainer = "$web";

        public static string BlobsStorageHttpAbsoluteUri
        {
            get
            {
                if (FlavourBusinessManagerApp.RootContainer == "$web")
                {
                    return "https://angularhost.z16.web.core.windows.net/";
                }
                else
                    return RawStorageCloudBlob.CloudBlobStorageAccount.BlobStorageUri.PrimaryUri.AbsoluteUri + "/";

                //return RawStorageCloudBlob.CloudStorageAccount.BlobStorageUri.PrimaryUri.AbsoluteUri + "$web/";
                //return "https://angularhost.z16.web.core.windows.net/";

                //Microsoft.Azure.Storage.CloudStorageAccount

            }
        }

        public static string BlobsInternalAbsoluteUri
        {
            get
            {
                ////return RawStorageCloudBlob.CloudStorageAccount.BlobStorageUri.PrimaryUri.AbsoluteUri + "/";
                return RawStorageCloudBlob.CloudBlobStorageAccount.BlobStorageUri.PrimaryUri.AbsoluteUri + FlavourBusinessManagerApp.RootContainer + "/";
            }
        }



        public object RawData
        {
            get
            {
                return StorageDoc;
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public bool IsReadonly => false;

        XDocument StorageDoc;
        string BlobUrl;
        public void SaveRawData()
        {

            MemoryStream memoryStream = new MemoryStream();

            StorageDoc.Save(memoryStream, SaveOptions.None);

            memoryStream.Position = 0;
            string relativeUrl = BlobUrl;
            if (relativeUrl.IndexOf(BlobsInternalAbsoluteUri) == 0)
                relativeUrl = relativeUrl.Substring((BlobsInternalAbsoluteUri).Length);

            string containerName = relativeUrl.Split('/')[0];

            string containerFilePath = relativeUrl.Substring(containerName.Length + 1);
            var blobClient = CloudBlobStorageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(containerName);
            container.CreateIfNotExists();
            BlobContainerPermissions permissions = container.GetPermissions();
            permissions.PublicAccess = BlobContainerPublicAccessType.Container;
            container.SetPermissions(permissions);


            CloudBlockBlob blob = container.GetBlockBlobReference(containerFilePath);
            blob.Properties.ContentType = "application/xml";
            var deleted = blob.DeleteIfExists();
            //blob.cr.CreateOrReplace(AccessCondition.GenerateIfNotExistsCondition(), null, null);
            //fileUploadEntry.AppendBlob = blob;
            blob.UploadFromStream(memoryStream);

        }



        public RawStorageCloudBlob(XDocument storageDoc, string blobUrl)
        {
            StorageDoc = storageDoc;
            BlobUrl = blobUrl;
            StorageLocation = blobUrl;
        }
        public string StorageName { get; }
        public RawStorageCloudBlob(string blobUrl, string storageName)
        {
            StorageName = storageName;
            HttpClient httpClient = new HttpClient();
            var dataStreamTask = httpClient.GetStreamAsync(blobUrl);
            dataStreamTask.Wait();
            var dataStream = dataStreamTask.Result;
            StorageDoc = XDocument.Load(dataStream);
            BlobUrl = blobUrl;
            StorageLocation = blobUrl;
        }



        public static DateTimeOffset? GetBlobLastModified(string blobUrl)
        {
            try
            {

                var blobClient = Microsoft.Azure.Storage.CloudStorageAccount.DevelopmentStorageAccount.CreateCloudBlobClient();
                return blobClient.GetBlobReferenceFromServer(new Uri(BlobsInternalAbsoluteUri + blobUrl)).Properties.LastModified;

            }
            catch (Exception error)
            {

                throw;
            }
        }
        public string StorageLocation { get; private set; }
    }
}
