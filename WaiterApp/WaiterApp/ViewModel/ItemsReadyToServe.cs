using DontWaitApp;
using FlavourBusinessFacade.RoomService;
using FlavourBusinessFacade.ServicesContextResources;
using OOAdvantech;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WaiterApp.ViewModel
{
    /// <MetaDataID>{c6caec47-b340-48ee-8f09-428622c0945c}</MetaDataID>
    public class ServingBatchPresentation : System.IDisposable
    {
        public ServingBatchPresentation()
        {

        }


        public ItemPreparationState State
        {
            get
            {
                var servingBatchItems = (from itemsPreprationContext in this.ContextsOfPreparedItems
                                         from preparedItem in itemsPreprationContext.PreparationItems
                                         select preparedItem).OfType<FlavourBusinessManager.RoomService.ItemPreparation>().ToList();

                 if (servingBatchItems.All(x=>x.State==ItemPreparationState.OnRoad))
                    return ItemPreparationState.OnRoad;

                if (servingBatchItems.All(x => x.IsInFollowingState(ItemPreparationState.OnRoad)))
                    return ItemPreparationState.Served;

                return ItemPreparationState.Serving;

            }
        }

        public void Dispose()
        {
            ServingBatch.ObjectChangeState -= ServingBatchChangeState;
            ServingBatch.ItemsStateChanged -= ServingBatch_ItemsStateChanged;

        }
        public string ServiceBatchIdentity { get; set; }

        public readonly IServingBatch ServingBatch;
        //public readonly WaiterPresentation WaiterPresentation;
        public ServingBatchPresentation(IServingBatch servingBatch)
        {
            //WaiterPresentation = waiterPresentation;
            string servicesContextIdentity = servingBatch.ServicesContextIdentity;
            ServingBatch = servingBatch;
            ServingBatch.ObjectChangeState += ServingBatchChangeState;
            ServingBatch.ItemsStateChanged += ServingBatch_ItemsStateChanged;
            ServiceBatchIdentity = servingBatch.MealCourseUri;
            List<ItemsPreparationContext> allContextsOfPreparedItems = new List<ItemsPreparationContext>();
            Description = servingBatch.Description;
            ServicesPointIdentity = servingBatch.ServicesPointIdentity;
            ContextsOfPreparedItems = servingBatch.ContextsOfPreparedItems;
            allContextsOfPreparedItems.AddRange(ContextsOfPreparedItems);

            ContextsOfUnderPreparationItems = servingBatch.ContextsOfUnderPreparationItems;
            allContextsOfPreparedItems.AddRange(ContextsOfUnderPreparationItems);

            ServicePointType = servingBatch.ServicePointType;

            AllContextsOfPreparedItems = allContextsOfPreparedItems;
            var sessionData = servingBatch.MealCourse.SessionData;
            var storeRef = sessionData.Menu;
#if !DeviceDotNet
             
            storeRef.StorageUrl = "https://dev-localhost/" + storeRef.StorageUrl.Substring(storeRef.StorageUrl.IndexOf("devstoreaccount1"));
#endif
            string menuRoot = storeRef.StorageUrl.Substring(0, storeRef.StorageUrl.LastIndexOf("/") + 1);
            string menuFile = storeRef.StorageUrl.Substring(storeRef.StorageUrl.LastIndexOf("/") + 1);

            MenuData = new DontWaitApp.MenuData()
            {
                MenuName = storeRef.Name,
                MenuRoot = storeRef.StorageUrl.Substring(0, storeRef.StorageUrl.LastIndexOf("/") + 1),
                MenuFile = storeRef.StorageUrl.Substring(storeRef.StorageUrl.LastIndexOf("/") + 1),
                DefaultMealTypeUri = sessionData.DefaultMealTypeUri,
                ServicePointIdentity= servicesContextIdentity+";"+sessionData.ServicePointIdentity,
            };
        }

        public event ObjectChangeStateHandle ObjectChangeState; 

        private void ServingBatch_ItemsStateChanged(Dictionary<string, ItemPreparationState> newItemsState)
        {
            foreach (var preparedItem in (from preparedItemsContext in AllContextsOfPreparedItems
                                          from preparedItem in preparedItemsContext.PreparationItems
                                          where newItemsState.ContainsKey(preparedItem.uid)
                                          select preparedItem))
            {
                preparedItem.State = newItemsState[preparedItem.uid];
            }
            ObjectChangeState?.Invoke(this, null);
            
        }

        private void ServingBatchChangeState(object _object, string member)
        {
            ServiceBatchIdentity = ServingBatch.MealCourseUri;
            List<ItemsPreparationContext> allContextsOfPreparedItems = new List<ItemsPreparationContext>();
            Description = ServingBatch.Description;
            ServicesPointIdentity = ServingBatch.ServicesPointIdentity;
            ContextsOfPreparedItems = ServingBatch.ContextsOfPreparedItems;
            allContextsOfPreparedItems.AddRange(ContextsOfPreparedItems);

            ContextsOfUnderPreparationItems = ServingBatch.ContextsOfUnderPreparationItems;
            allContextsOfPreparedItems.AddRange(ContextsOfUnderPreparationItems);

            ServicePointType = ServingBatch.ServicePointType;

            AllContextsOfPreparedItems = allContextsOfPreparedItems;
            ObjectChangeState?.Invoke(this, null);
            
        }

        public IList<ItemsPreparationContext> AllContextsOfPreparedItems { get; private set; }

        public MenuData MenuData { get; private set; }
        public string Description { get; private set; }
        public string ServicesPointIdentity { get; private set; }
        public IList<ItemsPreparationContext> ContextsOfPreparedItems { get; private set; }
        public IList<ItemsPreparationContext> ContextsOfUnderPreparationItems { get; private set; }
        public ServicePointType ServicePointType { get; private set; }

        internal void Update()
        {
            ServingBatchChangeState(ServingBatch, null);
        }
    }
}
