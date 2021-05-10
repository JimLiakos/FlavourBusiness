using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreparationStationDevice.WPF
{
    /// <MetaDataID>{f3b24633-1802-44fd-8627-b0f857f147e6}</MetaDataID>
    public class StorageLocatorEx : OOAdvantech.PersistenceLayer.IStorageLocatorEx
    {

        /// <MetaDataID>{daa15be2-e4d9-4e1b-a4b6-119bc540312b}</MetaDataID>
        public StorageLocatorEx()
        {
            OOAdvantech.Remoting.RestApi.RemotingServices.ServerPublicUrl = AzureServerUrl;
        }
        //static string AzureServerUrl = "http://localhost:8090/api/";
        //static string AzureServerUrl = "http://192.168.2.5:8090/api/";
        //static strinb AzureServerUrl = "http://192.168.2.10:8090/api/";
        /// <MetaDataID>{769f2150-0023-4da5-938f-6fe9d41ba982}</MetaDataID>

        static string AzureServerUrl = "http://192.168.2.4:8090/api/";//Braxati
        //static string AzureServerUrl = "http://192.168.2.8:8090/api/";//org
        //static string AzureServerUrl = "http://10.0.0.8:8090/api/";//work


        /// <MetaDataID>{ce0e578c-c823-438c-9d16-1df97086d515}</MetaDataID>
        public OOAdvantech.MetaDataRepository.StorageMetaData GetSorageMetaData(string storageIdentity)
        {
            System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();
            string serverUrl = OOAdvantech.Remoting.RestApi.RemotingServices.ServerPublicUrl.Substring(0, OOAdvantech.Remoting.RestApi.RemotingServices.ServerPublicUrl.IndexOf("/api"));
            OOAdvantech.PersistenceLayer.StoragesClient storagesClient = new OOAdvantech.PersistenceLayer.StoragesClient(httpClient);

            storagesClient.BaseUrl = serverUrl;
            var task = storagesClient.GetAsync(storageIdentity);
            if (task.Wait(TimeSpan.FromSeconds(2)))
                return task.Result;
            else
                return new OOAdvantech.MetaDataRepository.StorageMetaData();
        }
    }
}
