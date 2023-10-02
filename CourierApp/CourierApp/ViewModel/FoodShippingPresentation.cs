using DontWaitApp;
using FlavourBusinessFacade.RoomService;
using FlavourBusinessFacade.ServicesContextResources;
using FlavourBusinessFacade.Shipping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourierApp.ViewModel
{
    public class FoodShippingPresentation : System.IDisposable
    {
        public FoodShippingPresentation()
        {

        }


        public ItemPreparationState State
        {
            get
            {
                var foodShippingItems = (from itemsPreprationContext in this.ContextsOfPreparedItems
                                         from preparedItem in itemsPreprationContext.PreparationItems
                                         select preparedItem).OfType<FlavourBusinessManager.RoomService.ItemPreparation>().ToList();

                if (foodShippingItems.All(x => x.State == ItemPreparationState.OnRoad))
                    return ItemPreparationState.OnRoad;

                if (foodShippingItems.All(x => x.IsInFollowingState(ItemPreparationState.OnRoad)))
                    return ItemPreparationState.Served;

                return ItemPreparationState.Serving;

            }
        }

        public void Dispose()
        {
            FoodShipping.ObjectChangeState -= FoodShippingChangeState;
            FoodShipping.ItemsStateChanged -= FoodShipping_ItemsStateChanged;

        }
        public string ServiceBatchIdentity { get; set; }

        public readonly IFoodShipping FoodShipping;
        public readonly CourierActivityPresentation CourierActivityPresentation;
        public FoodShippingPresentation(IFoodShipping foodShipping, CourierActivityPresentation courierActivityPresentation)
        {
            CourierActivityPresentation = courierActivityPresentation;
            string servicesContextIdentity = courierActivityPresentation.Courier.ServicesContextIdentity;
            FoodShipping = foodShipping;
            FoodShipping.ObjectChangeState += FoodShippingChangeState;
            FoodShipping.ItemsStateChanged += FoodShipping_ItemsStateChanged;
            ServiceBatchIdentity = foodShipping.MealCourseUri;
            List<ItemsPreparationContext> allContextsOfPreparedItems = new List<ItemsPreparationContext>();
            Description = foodShipping.Description;
            ContextsOfPreparedItems = foodShipping.ContextsOfPreparedItems;
            allContextsOfPreparedItems.AddRange(ContextsOfPreparedItems);

            ContextsOfUnderPreparationItems = foodShipping.ContextsOfUnderPreparationItems;
            allContextsOfPreparedItems.AddRange(ContextsOfUnderPreparationItems);

            ServicePointType = foodShipping.ServicePointType;

            AllContextsOfPreparedItems = allContextsOfPreparedItems;
            var sessionData = foodShipping.MealCourse.SessionData;
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
                ServicePointIdentity = servicesContextIdentity + ";" + sessionData.ServicePointIdentity,
            };
        }

        private void FoodShipping_ItemsStateChanged(Dictionary<string, ItemPreparationState> newItemsState)
        {
            foreach (var preparedItem in (from preparedItemsContext in AllContextsOfPreparedItems
                                          from preparedItem in preparedItemsContext.PreparationItems
                                          where newItemsState.ContainsKey(preparedItem.uid)
                                          select preparedItem))
            {
                preparedItem.State = newItemsState[preparedItem.uid];
            }
            //CourierActivityPresentation.FoodShippingUpdated(this);
        }

        private void FoodShippingChangeState(object _object, string member)
        {
            ServiceBatchIdentity = FoodShipping.MealCourseUri;
            List<ItemsPreparationContext> allContextsOfPreparedItems = new List<ItemsPreparationContext>();
            Description = FoodShipping.Description;
            
            ContextsOfPreparedItems = FoodShipping.ContextsOfPreparedItems;
            allContextsOfPreparedItems.AddRange(ContextsOfPreparedItems);

            ContextsOfUnderPreparationItems = FoodShipping.ContextsOfUnderPreparationItems;
            allContextsOfPreparedItems.AddRange(ContextsOfUnderPreparationItems);

            ServicePointType = FoodShipping.ServicePointType;

            AllContextsOfPreparedItems = allContextsOfPreparedItems;

            //CourierActivityPresentation.FoodShippingUpdated(this);
        }

        public IList<ItemsPreparationContext> AllContextsOfPreparedItems { get; private set; }

        public MenuData MenuData { get; private set; }
        public string Description { get; private set; }
        
        public IList<ItemsPreparationContext> ContextsOfPreparedItems { get; private set; }
        public IList<ItemsPreparationContext> ContextsOfUnderPreparationItems { get; private set; }
        public ServicePointType ServicePointType { get; private set; }

        internal void Update()
        {
            FoodShippingChangeState(FoodShipping, null);
        }
    }
}
