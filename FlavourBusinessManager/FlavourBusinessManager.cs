using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using ComputationalResources;
using FlavourBusinessFacade;
using Microsoft.WindowsAzure.ServiceRuntime;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Transactions;

namespace FlavourBusinessManager
{
    /// <MetaDataID>{b05e48c6-8e14-4848-9a52-569bcb3d484d}</MetaDataID>
    public static class FlavourBusinessManagerApp
    {

        public static string FlavourBusinessStoragesAccountName;
        public static string FlavourBusinessStoragesAccountkey;
        public static string FlavourBusinessStoragesLocation;

        /// <MetaDataID>{9af6c9da-a163-47fb-9dea-70ff6193ef7e}</MetaDataID>
        public static void Init(string flavourBusinessStoragesAccountName, string flavourBusinessStoragesAccountkey, string flavourBusinessStoragesLocation,string rootContainer)
        {
            if (flavourBusinessStoragesAccountName == Microsoft.Azure.Storage.CloudStorageAccount.DevelopmentStorageAccount.Credentials.AccountName)
            {
                CloudTableStorageAccount = Microsoft.Azure.Cosmos.Table.CloudStorageAccount.DevelopmentStorageAccount;
                CloudBlobStorageAccount = Microsoft.Azure.Storage.CloudStorageAccount.DevelopmentStorageAccount;
            }
            else
            {
                CloudTableStorageAccount = new Microsoft.Azure.Cosmos.Table.CloudStorageAccount(new Microsoft.Azure.Cosmos.Table.StorageCredentials(flavourBusinessStoragesAccountName, flavourBusinessStoragesAccountkey), true);
                CloudBlobStorageAccount = new Microsoft.Azure.Storage.CloudStorageAccount(new Microsoft.Azure.Storage.Auth.StorageCredentials(flavourBusinessStoragesAccountName, flavourBusinessStoragesAccountkey), true);
                RootContainer = rootContainer;
            }



            FlavourBusinessStoragesAccountName = flavourBusinessStoragesAccountName;
            FlavourBusinessStoragesAccountkey = flavourBusinessStoragesAccountkey;
            FlavourBusinessStoragesLocation = flavourBusinessStoragesLocation;

            OOAdvantech.Remoting.RestApi.Serialization.SerializationBinder.NamesTypesDictionary["FlavourBusinessManager.RoomService.ItemPreparation"] = typeof(FlavourBusinessManager.RoomService.ItemPreparation);
            OOAdvantech.Remoting.RestApi.Serialization.SerializationBinder.NamesTypesDictionary["FlavourBusinessManager.RoomService.OptionChange"] = typeof(FlavourBusinessManager.RoomService.OptionChange);

            //var businessesResourcesInitTask = Task.Run(() =>
            //{
            
            var objectsStorage = OpenFlavourBusinessesResourcesStorage(flavourBusinessStoragesAccountName, flavourBusinessStoragesAccountkey, flavourBusinessStoragesLocation);
            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(objectsStorage);


            var flavourBusinessManagmentContext = (from computingContext in storage.GetObjectCollection<IsolatedComputingContext>()
                                                   where computingContext.ContextID == StandardComputingContext.FlavourBusinessManagmenContext
                                                   select computingContext).FirstOrDefault();

            if (flavourBusinessManagmentContext == null)
            {
                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {
                    flavourBusinessManagmentContext = new IsolatedComputingContext();
                    flavourBusinessManagmentContext.ContextID = StandardComputingContext.FlavourBusinessManagmenContext;
                    flavourBusinessManagmentContext.Description = "Manage flavour business";
                    objectsStorage.CommitTransientObjectState(flavourBusinessManagmentContext);
                    flavourBusinessManagmentContext.ComputingResourceID = 0;
                    stateTransition.Consistent = true;
                }
            }
            //});

            //var authManagerTask = Task.Run(() =>
            //{
                if (OrganizationsManager.AuthFlavourBusiness == null)
                    OrganizationsManager.AuthFlavourBusiness = new AuthFlavourBusiness();
            //});


            //Task.WhenAll(businessesResourcesInitTask, businessesResourcesInitTask).Wait();
            //FlavoursServicesContextsMonitoringTimer.AutoReset = false;


        }





        static ObjectStorage FlavourBusinessesResourcesStorage;
        public static Microsoft.Azure.Cosmos.Table.CloudStorageAccount CloudTableStorageAccount { get; set; }

        public static Microsoft.Azure.Storage.CloudStorageAccount CloudBlobStorageAccount { get; set; }
        public static string RootContainer { get; set; }

        /// <MetaDataID>{4ce71c7e-e41d-445a-ad42-c239c64c53c5}</MetaDataID>
        public static ObjectStorage OpenFlavourBusinessesResourcesStorage(string accountName, string acountkey, string flavourBusinessStoragesLocation)
        {


            if (FlavourBusinessesResourcesStorage != null)
                return FlavourBusinessesResourcesStorage;

            ObjectStorage storageSession = null;
            string storageName = "FlavourBusinessesResources";
            string storageLocation = "DevStorage";
            
            if (!string.IsNullOrWhiteSpace(flavourBusinessStoragesLocation))
                storageLocation = flavourBusinessStoragesLocation;

            string storageType = "OOAdvantech.WindowsAzureTablesPersistenceRunTime.StorageProvider";
            retryToOpenResourcesStorage:
            try
            {
                storageSession = ObjectStorage.OpenStorage(storageName,
                                                            storageLocation,
                                                            storageType,accountName,acountkey);
                //storageSession.StorageMetaData.RegisterComponent(typeof(IsolatedComputingContext).Assembly.FullName);
                //storageSession.StorageMetaData.RegisterComponent(typeof(IOrganization).Assembly.FullName);
                //storageSession.StorageMetaData.RegisterComponent(typeof(Organization).Assembly.FullName);


            }
            catch (OOAdvantech.PersistenceLayer.StorageException Error)
            {
                if (Error.Reason == StorageException.ExceptionReason.StorageDoesnotExist)
                {
                    //only worker role instance 0 can create ComputingResource storage 
                    //the other instances wait instance 0 to create
                    if (ComputingCluster.CurrentComputingResource.ResourceIndex == 0)
                        storageSession = ObjectStorage.NewStorage(storageName,
                                                                storageLocation,
                                                                storageType);
                    else
                    {
                        System.Threading.Thread.Sleep(2000);

                        goto retryToOpenResourcesStorage;
                    }
                }
                else
                    throw Error;
                try
                {
                    storageSession.StorageMetaData.RegisterComponent(typeof(IsolatedComputingContext).Assembly.FullName);
                    storageSession.StorageMetaData.RegisterComponent(typeof(Organization).Assembly.FullName);
                }
                catch (System.Exception Errore)
                {
                    int sdf = 0;
                }
            }
            catch (System.Exception Error)
            {
                int tt = 0;
            }
            if (storageSession == null)
            {
                int dd = 0;
            }
            FlavourBusinessesResourcesStorage = storageSession;
            return storageSession;
        }



        /// <MetaDataID>{2345f79e-a2e6-4b4b-b7a5-8da8f9dd43bb}</MetaDataID>
        static private IDemoService GetAProxy(string serviceUrl)
        {
            NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
            EndpointAddress endpointAddress
                = new EndpointAddress(serviceUrl);

            return new ChannelFactory<IDemoService>
                (binding, endpointAddress).CreateChannel();
        }
    }


    /// <MetaDataID>{6ce56a6b-28ab-4d13-a89f-372a1bab6fb0}</MetaDataID>
    public class StandardComputingContext
    {
        /// <MetaDataID>{2a7e29cc-095d-434b-9f25-7b1162989921}</MetaDataID>
        public const string ComputingContext = "055980081b674aec9e774e8403cdc972";

        /// <MetaDataID>{19587b57-346a-445a-8f67-60a136e3bb8c}</MetaDataID>
        public const string FlavourBusinessManagmenContext = "14e60576b74146cb839c7904298f11c2";

    }
}