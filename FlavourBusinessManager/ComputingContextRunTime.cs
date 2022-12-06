using OOAdvantech.PersistenceLayer;
using OOAdvantech.Remoting;
using System.Linq;
using OOAdvantech.Transactions;
using FlavourBusinessFacade.ComputingResources;
using System.Collections.Generic;

namespace FlavourBusinessManager.ComputingResources
{
    /// <MetaDataID>{cc175d1f-ffb4-4163-9c62-3ada07da3a84}</MetaDataID>
    [OOAdvantech.MetaDataRepository.HttpVisible]
    public class ComputingContextRunTime : MonoStateClass,  IExtMarshalByRefObject
    {
        public IList<EndPoint> UpdateComputingClusterAgent()
        {
            
            ComputingCluster.CurrentComputingCluster.UpdateComputingCluster();
            return ComputingCluster.CurrentComputingResource.CommunicationEndpoints;
        }

        /// <MetaDataID>{f20f9e9d-c06f-440b-afdc-1a8f38dd82ea}</MetaDataID>
        public FlavourBusinessFacade.IFlavoursServicesContextRuntime GetServicesContextRuntime(string storageName, string storageLocation, string servicePointIdentity, bool create = false)
        {


            storageLocation = "DevStorage";
            string storageType = "OOAdvantech.WindowsAzureTablesPersistenceRunTime.StorageProvider";

            ObjectStorage objectStorage = null;
            try
            {
                objectStorage = ObjectStorage.OpenStorage(storageName,
                                                            storageLocation,
                                                            storageType);

                OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(objectStorage);
                var servicePointRuntime = (from theServicePointRun in storage.GetObjectCollection<ServicePointRunTime.ServicesContextRunTime>()
                                    where theServicePointRun.ServicesContextIdentity == servicePointIdentity
                                    select theServicePointRun).FirstOrDefault();

                

                if(servicePointRuntime==null&&create)
                {

                    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                    {
                        servicePointRuntime = new ServicePointRunTime.ServicesContextRunTime();
                        servicePointRuntime.ServicesContextIdentity = servicePointIdentity;
                        objectStorage.CommitTransientObjectState(servicePointRuntime); 
                        stateTransition.Consistent = true;
                    }
                }

                return servicePointRuntime;
            }
            catch (OOAdvantech.PersistenceLayer.StorageException Error)
            {
                if (Error.Reason == StorageException.ExceptionReason.StorageDoesnotExist)
                {
                    objectStorage = ObjectStorage.NewStorage(storageName, storageLocation, storageType);
                }
                else
                    throw Error;

                objectStorage.StorageMetaData.RegisterComponent(typeof(Organization).Assembly.FullName);
            }
            catch (System.Exception Error)
            {
                int tt = 0;
            }

            return null;
        }
    }
}