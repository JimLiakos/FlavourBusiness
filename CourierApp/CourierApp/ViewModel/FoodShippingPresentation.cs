using DontWaitApp;
using FlavourBusinessFacade.EndUsers;
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
        /// <MetaDataID>{1c761f26-73e0-4f22-b419-e4abfa531864}</MetaDataID>
        public FoodShippingPresentation()
        {

        }


        /// <MetaDataID>{5d4a45e0-dd81-4390-a69c-1e694cfeb85d}</MetaDataID>
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

        /// <MetaDataID>{e3bb24f5-160e-43d0-a8ca-9c093b07794a}</MetaDataID>
        public void Dispose()
        {
            FoodShipping.ObjectChangeState -= FoodShippingChangeState;
            FoodShipping.ItemsStateChanged -= FoodShipping_ItemsStateChanged;

        }
        /// <MetaDataID>{1b2e57fc-c48b-4e66-8798-53493e3b9a05}</MetaDataID>
        public string ServiceBatchIdentity { get; set; }

        /// <MetaDataID>{3dee3162-c78f-4384-9ded-6a4d4354314c}</MetaDataID>
        public readonly IFoodShipping FoodShipping;
        /// <MetaDataID>{3eb1787b-42a7-420a-b419-9603d78540e7}</MetaDataID>
        public readonly CourierActivityPresentation CourierActivityPresentation;
        /// <MetaDataID>{ad9e2e1b-86c0-4ada-ba0c-fe7a19a28234}</MetaDataID>
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
        public IPlace Place { get => FoodShipping.Place; }

        public string ClientFullName { get => FoodShipping.ClientFullName; }

        public string PhoneNumber { get => FoodShipping.PhoneNumber; }

        public string DeliveryRemark { get => FoodShipping.DeliveryRemark; }

        public string NotesForClient { get => FoodShipping.NotesForClient; }

        /// <MetaDataID>{90ec5e57-2488-44b8-9665-87dbedb3de7f}</MetaDataID>
        private void FoodShipping_ItemsStateChanged(Dictionary<string, ItemPreparationState> newItemsState)
        {
            foreach (var preparedItem in (from preparedItemsContext in AllContextsOfPreparedItems
                                          from preparedItem in preparedItemsContext.PreparationItems
                                          where newItemsState.ContainsKey(preparedItem.uid)
                                          select preparedItem))
            {
                preparedItem.State = newItemsState[preparedItem.uid];
            }
            var ssd = State;
            CourierActivityPresentation.FoodShippingUpdated(this);
        }

        /// <MetaDataID>{1afa1a50-8a99-4c9b-9529-f7f29ed01c3a}</MetaDataID>
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

        /// <MetaDataID>{a9e2d873-1823-4c13-9256-dd5692a69ab3}</MetaDataID>
        public IList<ItemsPreparationContext> AllContextsOfPreparedItems { get; private set; }

        /// <MetaDataID>{36163777-6552-41c7-8f8a-a38439d8895b}</MetaDataID>
        public MenuData MenuData { get; private set; }
        /// <MetaDataID>{d83d6235-4a93-4c75-b467-a92f745d8f8d}</MetaDataID>
        public string Description { get; private set; }

        /// <MetaDataID>{15bd9216-894a-432b-85dc-cbc669d97438}</MetaDataID>
        public IList<ItemsPreparationContext> ContextsOfPreparedItems { get; private set; }
        /// <MetaDataID>{d1c27d0e-a3f3-4184-9450-507bf20f2bdb}</MetaDataID>
        public IList<ItemsPreparationContext> ContextsOfUnderPreparationItems { get; private set; }
        /// <MetaDataID>{adef1576-fb9e-4488-bc00-66781b6e4fae}</MetaDataID>
        public ServicePointType ServicePointType { get; private set; }

        /// <MetaDataID>{27b36d5d-39e9-46d4-b705-8c8ed78a8a2a}</MetaDataID>
        internal void Update()
        {
            FoodShippingChangeState(FoodShipping, null);
        }
    }
}
