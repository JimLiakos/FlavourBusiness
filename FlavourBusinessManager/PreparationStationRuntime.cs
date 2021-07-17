  
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
using OOAdvantech.Transactions;
using System.Threading.Tasks;

namespace FlavourBusinessManager.ServicePointRunTime
{
    ///// <MetaDataID>{5a81aa2c-3c52-44e8-b1cd-103859bf22f5}</MetaDataID>
    //public class PreparationStationRuntimeA : MarshalByRefObject, IExtMarshalByRefObject, IPreparationStationRuntime
    //{





    //    /// <MetaDataID>{eafe182a-025b-4a0c-8b6a-6ca41812cb8e}</MetaDataID>
    //    internal static PreparationData GetPreparationData(ItemPreparation itemPreparation)
    //    {
    //        itemPreparation.LoadMenuItem();
    //        PreparationData preparationData = new PreparationData();
    //        if (itemPreparation.PreparationStation != null)
    //        {
    //            preparationData.ItemPreparation = itemPreparation;
    //            preparationData.PreparationStationRuntime = ServicesContextRunTime.Current.GetPreparationStation(itemPreparation.PreparationStation.PreparationStationIdentity) as PreparationStationRuntime;
    //            preparationData.Duration = TimeSpan.FromMinutes((itemPreparation.PreparationStation as ServicesContextResources.PreparationStation).GetPreparationTimeSpanInMin(itemPreparation.MenuItem));
    //            return preparationData;
    //        }

    //        foreach (var preparationStationRuntime in ServicesContextRunTime.Current.PreparationStationRuntimes.Values.Where(x => (x.PreparationStation as ServicesContextResources.PreparationStation).HasServicePointsPreparationInfos))
    //        {
    //            if (preparationStationRuntime.PreparationStation.CanPrepareItemFor(itemPreparation.MenuItem, itemPreparation.ClientSession.ServicePoint))
    //            {
    //                preparationData.PreparationStationRuntime = preparationStationRuntime;
    //                preparationData.Duration = TimeSpan.FromMinutes((preparationStationRuntime.PreparationStation as ServicesContextResources.PreparationStation).GetPreparationTimeSpanInMin(itemPreparation.MenuItem));
    //                preparationData.ItemPreparation = itemPreparation;
    //            }
    //        }
    //        if (preparationData.PreparationStationRuntime == null)
    //        {
    //            foreach (var preparationStationRuntime in ServicesContextRunTime.Current.PreparationStationRuntimes.Values.Where(x => !(x.PreparationStation as ServicesContextResources.PreparationStation).HasServicePointsPreparationInfos))
    //            {
    //                if (preparationStationRuntime.PreparationStation.CanPrepareItem(itemPreparation.MenuItem))
    //                {
    //                    preparationData.PreparationStationRuntime = preparationStationRuntime;
    //                    preparationData.Duration = TimeSpan.FromMinutes((preparationStationRuntime.PreparationStation as ServicesContextResources.PreparationStation).GetPreparationTimeSpanInMin(itemPreparation.MenuItem));
    //                    preparationData.ItemPreparation = itemPreparation;
    //                }
    //            }
    //        }
    //        if (preparationData.PreparationStationRuntime == null)
    //        {
    //            foreach (var preparationStationRuntime in ServicesContextRunTime.Current.PreparationStationRuntimes.Values)
    //            {
    //                if (preparationStationRuntime.PreparationStation.CanPrepareItem(itemPreparation.MenuItem))
    //                {
    //                    preparationData.PreparationStationRuntime = preparationStationRuntime;
    //                    preparationData.Duration = TimeSpan.FromMinutes((preparationStationRuntime.PreparationStation as ServicesContextResources.PreparationStation).GetPreparationTimeSpanInMin(itemPreparation.MenuItem));
    //                    preparationData.ItemPreparation = itemPreparation;
    //                }
    //            }
    //        }








    //        return preparationData;
    //    }

    //    /// <MetaDataID>{82bd512e-74e9-49f2-8aac-00681aa07e89}</MetaDataID>
    //    public List<MenuModel.IMenuItem> GetNewerRestaurandMenuData(DateTime newerFromDate)
    //    {

    //        OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(ObjectStorage.GetStorageOfObject(_PreparationStation));

    //        var servicesContextRunTime = (from runTime in storage.GetObjectCollection<ServicesContextRunTime>() select runTime).FirstOrDefault();

    //        if (servicesContextRunTime.RestaurantMenuDataLastModified > newerFromDate)
    //        {

    //            var fbstorage = (from servicesContextRunTimeStorage in servicesContextRunTime.Storages
    //                             where servicesContextRunTimeStorage.FlavourStorageType == FlavourBusinessFacade.OrganizationStorages.RestaurantMenus
    //                             select servicesContextRunTimeStorage).FirstOrDefault();
    //            var storageRef = new FlavourBusinessFacade.OrganizationStorageRef { StorageIdentity = fbstorage.StorageIdentity, FlavourStorageType = fbstorage.FlavourStorageType, Name = fbstorage.Name, Description = fbstorage.Description, StorageUrl = servicesContextRunTime.RestaurantMenuDataUri, TimeStamp = servicesContextRunTime.RestaurantMenuDataLastModified };

    //            RawStorageData rawStorageData = new RawStorageData(storageRef, null);

    //            OOAdvantech.Linq.Storage restMenusData = new OOAdvantech.Linq.Storage(OOAdvantech.PersistenceLayer.ObjectStorage.OpenStorage("RestMenusData", rawStorageData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider"));
    //            Dictionary<object, object> mappedObject = new Dictionary<object, object>();
    //            List<MenuModel.IMenuItem> menuFoodItems = (from menuItem in restMenusData.GetObjectCollection<MenuModel.IMenuItem>()
    //                                                       select menuItem).ToList().Select(x => new MenuModel.JsonViewModel.MenuFoodItem(x, mappedObject)).OfType<MenuModel.IMenuItem>().ToList();

    //            var jSetttings = OOAdvantech.Remoting.RestApi.Serialization.JsonSerializerSettings.TypeRefSerializeSettings;
    //            string jsonEx = OOAdvantech.Json.JsonConvert.SerializeObject(menuFoodItems, jSetttings);
    //            jSetttings = OOAdvantech.Remoting.RestApi.Serialization.JsonSerializerSettings.TypeRefDeserializeSettings;
    //            var sss = OOAdvantech.Json.JsonConvert.DeserializeObject<List<MenuModel.IMenuItem>>(jsonEx, jSetttings);



    //            return menuFoodItems;

    //        }
    //        else
    //            return new List<MenuModel.IMenuItem>();
    //    }

    //    /// <MetaDataID>{85904c8a-9285-43dd-8133-366cbafffedb}</MetaDataID>
    //    List<ServicePointPreparationItems> ServicePointsPreparationItems = new List<ServicePointPreparationItems>();
    //    /// <MetaDataID>{44d31e6c-0716-43d5-8aee-fb92af6ae103}</MetaDataID>
    //    public PreparationStationRuntime(IPreparationStation preparationStation)
    //    {
    //        _PreparationStation = preparationStation;

    //        string nn = preparationStation.Description;

    //        OOAdvantech.Linq.Storage servicesContextStorage = new OOAdvantech.Linq.Storage(ObjectStorage.GetStorageOfObject(preparationStation));


    //        foreach (var servicePointPreparationItems in (from itemPreparation in (from item in servicesContextStorage.GetObjectCollection<IItemPreparation>()
    //                                                                                   //where item.State == ItemPreparationState.PreparationDelay|| item.State == ItemPreparationState.PendingPreparation || item.State == ItemPreparationState.OnPreparation
    //                                                                               select item.Fetching(item.ClientSession)).ToArray()
    //                                                      group itemPreparation by itemPreparation.ClientSession.ServicePoint into ServicePointItems
    //                                                      select ServicePointItems))
    //        {
    //            var preparationItems = new System.Collections.Generic.List<IItemPreparation>();
    //            foreach (var item in servicePointPreparationItems.OfType<RoomService.ItemPreparation>())
    //            {
    //                if (item.MenuItem == null)
    //                    item.LoadMenuItem();

    //                if (preparationStation.CanPrepareItem(item.MenuItem))
    //                {
    //                    RoomService.ItemPreparation itemPreparation = new RoomService.ItemPreparation(item.uid, item.MenuItemUri, item.Name);
    //                    itemPreparation.Update(item);
    //                    preparationItems.Add(itemPreparation);
    //                }
    //            }
    //            ServicePointsPreparationItems.Add(new ServicePointPreparationItems(servicePointPreparationItems.Key, preparationItems));
    //        }

    //        Task.Run(() =>
    //        {

    //            while (true)
    //            {
    //                lock (DeviceUpdateLock)
    //                {
    //                    if ((PreparationStation as ServicesContextResources.PreparationStation).DeviceUpdateEtag != null)
    //                    {
    //                        long numberOfTicks = 0;
    //                        if (long.TryParse((PreparationStation as ServicesContextResources.PreparationStation).DeviceUpdateEtag, out numberOfTicks))
    //                        {
    //                            DateTime myDate = new DateTime(numberOfTicks);
    //                            if ((DateTime.Now - myDate).TotalSeconds > 3)
    //                            {
    //                                if (RaiseEventTimeStamp == null || (DateTime.UtcNow - RaiseEventTimeStamp.Value).TotalSeconds > 30)
    //                                {
    //                                    _PreparationItemsChangeState?.Invoke(this, (PreparationStation as ServicesContextResources.PreparationStation).DeviceUpdateEtag);
    //                                    RaiseEventTimeStamp = DateTime.UtcNow;
    //                                }
    //                            }
    //                        }
    //                    }
    //                }
    //                System.Threading.Thread.Sleep(1000);
    //            }

    //        });

    //    }

    //    DateTime? RaiseEventTimeStamp;

    //    public object DeviceUpdateLock = new object();
    //    /// <MetaDataID>{d65435e4-edc6-4442-aa7d-72b3e8a13cee}</MetaDataID>
    //    internal void AssignItemPreparation(ItemPreparation flavourItem)
    //    {

    //        lock (DeviceUpdateLock)
    //        {
    //            if (flavourItem.PreparationStation != PreparationStation)
    //            {
    //                flavourItem.PreparationStation = PreparationStation;
    //                flavourItem.ObjectChangeState += FlavourItem_ObjectChangeState;

    //                var servicePointPreparationItems = ServicePointsPreparationItems.Where(x => x.ServicePoint == flavourItem.ClientSession.ServicePoint).FirstOrDefault();
    //                if (servicePointPreparationItems == null)
    //                    ServicePointsPreparationItems.Add(new ServicePointPreparationItems(flavourItem.ClientSession.ServicePoint, new List<IItemPreparation>() { flavourItem }));
    //                else
    //                    servicePointPreparationItems.PreparationItems.Add(flavourItem);

    //                if ((PreparationStation as ServicesContextResources.PreparationStation).DeviceUpdateEtag == null)
    //                    (PreparationStation as ServicesContextResources.PreparationStation).DeviceUpdateEtag = System.DateTime.Now.Ticks.ToString();
    //            }

    //        }

    //    }

    //    /// <MetaDataID>{19ecb02a-c2c6-4f65-88c9-55af858d8473}</MetaDataID>
    //    private void FlavourItem_ObjectChangeState(object _object, string member)
    //    {
    //        lock (DeviceUpdateLock)
    //        {
    //            (PreparationStation as ServicesContextResources.PreparationStation).DeviceUpdateEtag = System.DateTime.Now.Ticks.ToString();
    //        }
    //    }

    //    /// <MetaDataID>{a66e6690-f78e-4a01-9df8-8dee98b2e386}</MetaDataID>
    //    public string Description { get => _PreparationStation.Description; }


    //    /// <exclude>Excluded</exclude>
    //    IPreparationStation _PreparationStation;

    //    public event PreparationItemsChangeStateHandled _PreparationItemsChangeState;

    //    public event PreparationItemsChangeStateHandled PreparationItemsChangeState
    //    {
    //        add
    //        {
    //            _PreparationItemsChangeState += value;
    //        }
    //        remove
    //        {
    //            _PreparationItemsChangeState -= value;
    //        }
    //    }

    //    /// <MetaDataID>{bb6dcfe6-6b71-4c90-a652-da5190c5a413}</MetaDataID>
    //    public IPreparationStation PreparationStation => _PreparationStation;


    //    /// <MetaDataID>{397cadbc-bbeb-48f4-a6b8-8a4bbbc7c9ca}</MetaDataID>
    //    public IList<ServicePointPreparationItems> GetPreparationItems(List<ItemPreparationAbbreviation> itemsOnDevice, string deviceUpdateEtag)
    //    {
    //        if (deviceUpdateEtag == (PreparationStation as ServicesContextResources.PreparationStation).DeviceUpdateEtag)
    //        {

    //            lock (DeviceUpdateLock)
    //            {
    //                (PreparationStation as ServicesContextResources.PreparationStation).DeviceUpdateEtag = null;
    //                RaiseEventTimeStamp = null;

    //            }

    //        }
    //        return ServicePointsPreparationItems;
    //    }
    //    /// <MetaDataID>{eff630b5-4530-4f7c-8b74-f05999ef96ad}</MetaDataID>
    //    public string RestaurantMenuDataSharedUri
    //    {
    //        get
    //        {
    //            return ServicesContextRunTime.Current.RestaurantMenuDataSharedUri;
    //        }
    //    }
    //    /// <MetaDataID>{bb127f36-590c-4b69-af44-4ce09c3a91ef}</MetaDataID>
    //    internal void OnPreparationItemChangeState(ItemPreparation flavourItem)
    //    {

    //        if (flavourItem.MenuItem == null)
    //            flavourItem.LoadMenuItem();


    //        if (_PreparationStation.CanPrepareItem(flavourItem.MenuItem))
    //        {

    //        }
    //        else
    //        {

    //        }
    //    }
    //}


    ///// <MetaDataID>{0241f6f2-d035-4ec2-91ee-f2b41613abe3}</MetaDataID>
    //struct PreparationData
    //{
    //    /// <MetaDataID>{a3a3dd1c-2b8f-494e-a91b-7662e37b40b8}</MetaDataID>
    //    public ItemPreparation ItemPreparation;

    //    /// <MetaDataID>{12f62d94-bd1a-4c95-8bdb-4e58145b4469}</MetaDataID>
    //    public PreparationStationRuntime PreparationStationRuntime;

    //    /// <MetaDataID>{644e17a2-c58b-413c-bc65-b9b8d39c0729}</MetaDataID>
    //    public TimeSpan Duration;

    //}
}
