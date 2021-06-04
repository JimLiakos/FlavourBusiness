using System;
using System.IO;
using System.Xml.Linq;
using System.Net.Http;
using Microsoft.Azure.Storage.Blob;
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
                string azureStorageUrl =OOAdvantech.Remoting.RestApi.RemotingServices.GetLocalIPAddress();
                if (azureStorageUrl == null)
                    azureStorageUrl = AzureStorageUrl;
                else
                    azureStorageUrl = "http://"+ azureStorageUrl;



#if DEBUG

                return azureStorageUrl + RawStorageCloudBlob.CloudStorageAccount.BlobEndpoint.AbsolutePath;

                //return "https://angularhost.z16.web.core.windows.net";
#else
                return RawStorageCloudBlob.CloudStorageAccount.BlobStorageUri.PrimaryUri.AbsoluteUri;
#endif
            }
        }

     

        public static Microsoft.Azure.Storage.CloudStorageAccount CloudStorageAccount = Microsoft.Azure.Storage.CloudStorageAccount.DevelopmentStorageAccount;
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
            if (relativeUrl.IndexOf(CloudStorageAccount.BlobStorageUri.PrimaryUri.AbsoluteUri + "/")==0)
                relativeUrl= relativeUrl.Substring((CloudStorageAccount.BlobStorageUri.PrimaryUri.AbsoluteUri + "/").Length);

            string containerName = relativeUrl.Split('/')[0];

            string containerFilePath = relativeUrl.Substring(containerName.Length + 1);
            var blobClient = CloudStorageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(containerName);
            container.CreateIfNotExists();
            BlobContainerPermissions permissions = container.GetPermissions();
            permissions.PublicAccess = BlobContainerPublicAccessType.Container;
            container.SetPermissions(permissions);


            CloudBlockBlob blob = container.GetBlockBlobReference(containerFilePath);
            blob.Properties.ContentType = "application/xml";
            var deleted=  blob.DeleteIfExists();
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
        public RawStorageCloudBlob(string blobUrl,string storageName)
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
            var blobClient = CloudStorageAccount.CreateCloudBlobClient();
            return blobClient.GetBlobReferenceFromServer(new Uri(blobUrl)).Properties.LastModified;
        }
       public string StorageLocation { get; private set; }
    }
}
