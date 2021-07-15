using System;
using System.Linq;
using System.Collections.Generic;
using FlavourBusinessFacade.RoomService;
using FlavourBusinessFacade.ServicesContextResources;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Remoting;
using FlavourBusinessToolKit;
using FlavourBusinessManager.RoomService;
using FlavourBusinessFacade;
using OOAdvantech.MetaDataRepository;

namespace FlavourBusinessManager.ServicePointRunTime
{
    /// <MetaDataID>{5a81aa2c-3c52-44e8-b1cd-103859bf22f5}</MetaDataID>
    public class PreparationStationRuntime : MarshalByRefObject, IExtMarshalByRefObject, IPreparationStationRuntime
    {


        internal static PreparationData GetPreparationData(ItemPreparation itemPreparation)
        {
            itemPreparation.LoadMenuItem();
            PreparationData preparationData = new PreparationData();
            if(itemPreparation.PreparationStation!=null)
            {
                preparationData.ItemPreparation = itemPreparation;
                preparationData.PreparationStationRuntime = ServicesContextRunTime.Current.GetPreparationStation(itemPreparation.PreparationStation.PreparationStationIdentity) as PreparationStationRuntime;
                preparationData.Duration = TimeSpan.FromMinutes((itemPreparation.PreparationStation as ServicesContextResources.PreparationStation).GetPreparationTimeSpanInMin(itemPreparation.MenuItem));
                return preparationData;
            }

            foreach (var preparationStationRuntime in ServicesContextRunTime.Current.PreparationStationRuntimes.Values.Where(x=>(x .PreparationStation as ServicesContextResources.PreparationStation).HasServicePointsPreparationInfos))
            {
                if (preparationStationRuntime.PreparationStation.CanPrepareItemFor(itemPreparation.MenuItem, itemPreparation.ClientSession.ServicePoint))
                {
                    preparationData.PreparationStationRuntime = preparationStationRuntime;
                    preparationData.Duration=TimeSpan.FromMinutes((preparationStationRuntime.PreparationStation as ServicesContextResources.PreparationStation).GetPreparationTimeSpanInMin(itemPreparation.MenuItem));
                    preparationData.ItemPreparation = itemPreparation;
                }
            }
            if(preparationData.PreparationStationRuntime==null)
            {
                foreach (var preparationStationRuntime in ServicesContextRunTime.Current.PreparationStationRuntimes.Values.Where(x => !(x.PreparationStation as ServicesContextResources.PreparationStation).HasServicePointsPreparationInfos))
                {
                    if (preparationStationRuntime.PreparationStation.CanPrepareItem(itemPreparation.MenuItem))
                    {
                        preparationData.PreparationStationRuntime = preparationStationRuntime;
                        preparationData.Duration = TimeSpan.FromMinutes((preparationStationRuntime.PreparationStation as ServicesContextResources.PreparationStation).GetPreparationTimeSpanInMin(itemPreparation.MenuItem));
                        preparationData.ItemPreparation = itemPreparation;
                    }
                }
            }
            if (preparationData.PreparationStationRuntime == null)
            {
                foreach (var preparationStationRuntime in ServicesContextRunTime.Current.PreparationStationRuntimes.Values)
                {
                    if (preparationStationRuntime.PreparationStation.CanPrepareItem(itemPreparation.MenuItem))
                    {
                        preparationData.PreparationStationRuntime = preparationStationRuntime;
                        preparationData.Duration = TimeSpan.FromMinutes((preparationStationRuntime.PreparationStation as ServicesContextResources.PreparationStation).GetPreparationTimeSpanInMin(itemPreparation.MenuItem));
                        preparationData.ItemPreparation = itemPreparation;
                    }
                }
            }








                return preparationData;
        }

        /// <MetaDataID>{82bd512e-74e9-49f2-8aac-00681aa07e89}</MetaDataID>
        public List<MenuModel.IMenuItem> GetNewerRestaurandMenuData(DateTime newerFromDate)
        {

            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(ObjectStorage.GetStorageOfObject(_PreparationStation));

            var servicesContextRunTime = (from runTime in storage.GetObjectCollection<ServicesContextRunTime>() select runTime).FirstOrDefault();

            if (servicesContextRunTime.RestaurantMenuDataLastModified > newerFromDate)
            {

                var fbstorage = (from servicesContextRunTimeStorage in servicesContextRunTime.Storages
                                 where servicesContextRunTimeStorage.FlavourStorageType == FlavourBusinessFacade.OrganizationStorages.RestaurantMenus
                                 select servicesContextRunTimeStorage).FirstOrDefault();
                var storageRef = new FlavourBusinessFacade.OrganizationStorageRef { StorageIdentity = fbstorage.StorageIdentity, FlavourStorageType = fbstorage.FlavourStorageType, Name = fbstorage.Name, Description = fbstorage.Description, StorageUrl = servicesContextRunTime.RestaurantMenuDataUri, TimeStamp = servicesContextRunTime.RestaurantMenuDataLastModified };

                RawStorageData rawStorageData = new RawStorageData(storageRef, null);

                OOAdvantech.Linq.Storage restMenusData = new OOAdvantech.Linq.Storage(OOAdvantech.PersistenceLayer.ObjectStorage.OpenStorage("RestMenusData", rawStorageData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider"));
                Dictionary<object, object> mappedObject = new Dictionary<object, object>();
                List<MenuModel.IMenuItem> menuFoodItems = (from menuItem in restMenusData.GetObjectCollection<MenuModel.IMenuItem>()
                                                           select menuItem).ToList().Select(x => new MenuModel.JsonViewModel.MenuFoodItem(x, mappedObject)).OfType<MenuModel.IMenuItem>().ToList();

                var jSetttings = OOAdvantech.Remoting.RestApi.Serialization.JsonSerializerSettings.TypeRefSerializeSettings;
                string jsonEx = OOAdvantech.Json.JsonConvert.SerializeObject(menuFoodItems, jSetttings);
                jSetttings = OOAdvantech.Remoting.RestApi.Serialization.JsonSerializerSettings.TypeRefDeserializeSettings;
                var sss = OOAdvantech.Json.JsonConvert.DeserializeObject<List<MenuModel.IMenuItem>>(jsonEx, jSetttings);



                return menuFoodItems;

            }
            else
                return new List<MenuModel.IMenuItem>();
        }

        /// <MetaDataID>{85904c8a-9285-43dd-8133-366cbafffedb}</MetaDataID>
        List<ServicePointPreparationItems> ServicePointsPreparationItems = new List<ServicePointPreparationItems>();
        /// <MetaDataID>{44d31e6c-0716-43d5-8aee-fb92af6ae103}</MetaDataID>
        public PreparationStationRuntime(IPreparationStation preparationStation)
        {
            _PreparationStation = preparationStation;



            OOAdvantech.Linq.Storage servicesContextStorage = new OOAdvantech.Linq.Storage(ObjectStorage.GetStorageOfObject(preparationStation));




            foreach (var servicePointPreparationItems in (from itemPreparation in (from item in servicesContextStorage.GetObjectCollection<IItemPreparation>()
                                                                                   //where item.State == ItemPreparationState.PreparationDelay|| item.State == ItemPreparationState.PendingPreparation || item.State == ItemPreparationState.OnPreparation
                                                                                   select item.Fetching(item.ClientSession)).ToArray()
                                                          group itemPreparation by itemPreparation.ClientSession.ServicePoint into ServicePointItems
                                                          select ServicePointItems))
            {
                var preparationItems = new System.Collections.Generic.List<IItemPreparation>();
                foreach (var item in servicePointPreparationItems.OfType<RoomService.ItemPreparation>())
                {
                    if (item.MenuItem == null)
                        item.LoadMenuItem();

                    if (preparationStation.CanPrepareItem(item.MenuItem))
                    {
                        RoomService.ItemPreparation itemPreparation = new RoomService.ItemPreparation(item.uid, item.MenuItemUri, item.Name);
                        itemPreparation.Update(item);
                        preparationItems.Add(itemPreparation);
                    }
                }
                ServicePointsPreparationItems.Add(new ServicePointPreparationItems(servicePointPreparationItems.Key, preparationItems));
            }

        }

        internal void AssignItemPreparation(ItemPreparation flavourItem)
        {

            flavourItem.PreparationStation = PreparationStation;
            var servicePointPreparationItems = ServicePointsPreparationItems.Where(x => x.ServicePoint == flavourItem.ClientSession.ServicePoint).FirstOrDefault();
            if (servicePointPreparationItems == null)
                ServicePointsPreparationItems.Add(new ServicePointPreparationItems(flavourItem.ClientSession.ServicePoint, new List<IItemPreparation>() { flavourItem }));
            else
                servicePointPreparationItems.PreparationItems.Add(flavourItem);

            PreparationItemChangeState?.Invoke()
        }

        /// <MetaDataID>{a66e6690-f78e-4a01-9df8-8dee98b2e386}</MetaDataID>
        public string Description { get => _PreparationStation.Description; }


        /// <exclude>Excluded</exclude>
        IPreparationStation _PreparationStation;

        public event PreparationItemChangeStateHandled PreparationItemChangeState;

        /// <MetaDataID>{bb6dcfe6-6b71-4c90-a652-da5190c5a413}</MetaDataID>
        public IPreparationStation PreparationStation => _PreparationStation;


        /// <MetaDataID>{397cadbc-bbeb-48f4-a6b8-8a4bbbc7c9ca}</MetaDataID>
        public IList<ServicePointPreparationItems> GetPreparationItems(List<ItemPreparationAbbreviation> itemsOnDevice)
        {
            return ServicePointsPreparationItems;
        }
        public string RestaurantMenuDataSharedUri
        {
            get
            {
                return ServicesContextRunTime.Current.RestaurantMenuDataSharedUri;
            }
        }
        /// <MetaDataID>{bb127f36-590c-4b69-af44-4ce09c3a91ef}</MetaDataID>
        internal void OnPreparationItemChangeState(ItemPreparation flavourItem)
        {

            if (flavourItem.MenuItem == null)
                flavourItem.LoadMenuItem();


            if (_PreparationStation.CanPrepareItem(flavourItem.MenuItem))
            {

            }
            else
            {

            }
        }
    }


    /// <MetaDataID>{0241f6f2-d035-4ec2-91ee-f2b41613abe3}</MetaDataID>
    struct PreparationData
    {
        public ItemPreparation ItemPreparation;

        public PreparationStationRuntime PreparationStationRuntime;

        public TimeSpan Duration;

    }
}