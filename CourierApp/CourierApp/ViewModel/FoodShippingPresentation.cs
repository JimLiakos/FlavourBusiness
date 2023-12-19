using DontWaitApp;
using FlavourBusinessFacade.EndUsers;
using FlavourBusinessFacade.RoomService;
using FlavourBusinessFacade.ServicesContextResources;
using FlavourBusinessFacade.Shipping;
using FlavourBusinessManager.RoomService;
using OOAdvantech;
using ServiceContextManagerApp;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Globalization;
using OOAdvantech.Json;
using FlavourBusinessManager.RoomService.ViewModel;

#if !DeviceDotNet
using ServiceContextManagerApp;
using System;
#endif
namespace CourierApp.ViewModel
{
    /// <MetaDataID>{8fc5d800-f1f2-491e-a909-6264040cab63}</MetaDataID>
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
                var foodShippingItems = (from itemsPreprationContext in this.AllContextsOfPreparedItems
                                         from preparedItem in itemsPreprationContext.PreparationItems
                                         select preparedItem).OfType<FlavourBusinessManager.RoomService.ItemPreparation>().ToList();

                if (foodShippingItems.All(x => x.State == ItemPreparationState.OnRoad))
                    return ItemPreparationState.OnRoad;


                if (foodShippingItems.All(x => x.State == ItemPreparationState.Served))
                    return ItemPreparationState.Served;


                if (foodShippingItems.All(x => x.State == ItemPreparationState.Canceled))
                    return ItemPreparationState.Canceled;



                if (foodShippingItems.All(x => x.IsInFollowingState(ItemPreparationState.OnRoad)))
                    return ItemPreparationState.Served;


                if (foodShippingItems.Any(x => x.IsInPreviousState(ItemPreparationState.Serving)))
                    return ItemPreparationState.InPreparation;

                return ItemPreparationState.Serving;

            }
        }

        public event ObjectChangeStateHandle ObjectChangeState;

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
        //public readonly CourierActivityPresentation CourierActivityPresentation;
        /// <MetaDataID>{ad9e2e1b-86c0-4ada-ba0c-fe7a19a28234}</MetaDataID>
        public FoodShippingPresentation(IFoodShipping foodShipping)
        {
            //CourierActivityPresentation = courierActivityPresentation;
            string servicesContextIdentity = foodShipping.ServicesContextIdentity;
            FoodShipping = foodShipping;

            //foodShipping = foodShipping.Fetching(_foodShipping => _foodShipping.Caching(x => new { 
            //    x.ClientFullName, 
            //    x.PhoneNumber,
            //    x.MealCourseUri,
            //    x.Identity,
            //    x.MealCourse,
            //    x.DeliveryRemark,
            //    x.NotesForClient,
            //    x.ServicePoint
            //}));

            FoodShipping.ObjectChangeState += FoodShippingChangeState;
            FoodShipping.ItemsStateChanged += FoodShipping_ItemsStateChanged;
            ServiceBatchIdentity = foodShipping.MealCourseUri;
            List<ItemsPreparationContext> allContextsOfPreparedItems = new List<ItemsPreparationContext>();
            Description = foodShipping.Description;
            ContextsOfPreparedItems = foodShipping.ContextsOfPreparedItems;

            Identity = foodShipping.Identity;


            if (ContextsOfPreparedItems == null)
                ContextsOfPreparedItems = new List<ItemsPreparationContext>();

            if (ContextsOfPreparedItems == null)
                ContextsOfPreparedItems = new List<ItemsPreparationContext>();

            allContextsOfPreparedItems.AddRange(ContextsOfPreparedItems);

            ContextsOfUnderPreparationItems = foodShipping.ContextsOfUnderPreparationItems;

            if (ContextsOfUnderPreparationItems == null)
                ContextsOfUnderPreparationItems = new List<ItemsPreparationContext>();


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
            foreach (var preparationItem in (from preparedItemsContext in AllContextsOfPreparedItems
                                             from preparedItem in preparedItemsContext.PreparationItems
                                             where newItemsState.ContainsKey(preparedItem.uid)
                                             select preparedItem))
            {

                preparationItem.State = newItemsState[preparationItem.uid];
            }
            var ssd = State;
            ObjectChangeState?.Invoke(this, null);

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
        public string Identity { get; set; }

        /// <MetaDataID>{d1c27d0e-a3f3-4184-9450-507bf20f2bdb}</MetaDataID>
        public IList<ItemsPreparationContext> ContextsOfUnderPreparationItems { get; private set; }
        /// <MetaDataID>{adef1576-fb9e-4488-bc00-66781b6e4fae}</MetaDataID>
        public ServicePointType ServicePointType { get; private set; }
        public CourierPresentation Courier { get; set; }

        /// <MetaDataID>{27b36d5d-39e9-46d4-b705-8c8ed78a8a2a}</MetaDataID>
        internal void Update()
        {
            FoodShippingChangeState(FoodShipping, null);
        }


        public List<TipOption> TipOptions
        {
            get
            {
                var isoCurrencySymbol = this.ContextsOfPreparedItems.FirstOrDefault()?.PreparationItems.OfType<ItemPreparation>().FirstOrDefault()?.ISOCurrencySymbol;
                if (string.IsNullOrWhiteSpace(isoCurrencySymbol))
                    isoCurrencySymbol = System.Globalization.RegionInfo.CurrentRegion.ISOCurrencySymbol;
                return new List<TipOption>() {
                new TipOption() { Amount=0, ISOCurrencySymbol= isoCurrencySymbol },
                new TipOption() { Amount=0.5M, ISOCurrencySymbol= isoCurrencySymbol},
                new TipOption() { Amount=1, ISOCurrencySymbol= isoCurrencySymbol },
                new TipOption() { Amount=1.5M, ISOCurrencySymbol= isoCurrencySymbol },
                new TipOption() { Amount=2, ISOCurrencySymbol= isoCurrencySymbol },
                new TipOption() { Amount=2.5M, ISOCurrencySymbol= isoCurrencySymbol },
                new TipOption() { Amount=3, ISOCurrencySymbol= isoCurrencySymbol },
                new TipOption() { Amount=3.5M, ISOCurrencySymbol= isoCurrencySymbol },
                new TipOption() { Amount=4M, ISOCurrencySymbol= isoCurrencySymbol }};
            }
        }
        public List<ReturnReason> ReturnReasons
        {
            get
            {
                var returnReasons = OOAdvantech.Remoting.RestApi.RemotingServices.CastTransparentProxy<IHomeDeliveryServicePoint>(FoodShipping.ServicePoint).ReturnReasons;
                return returnReasons;

                //return new List<ReturnReason>() { 
                //    new ReturnReason("WRNGPROD",new Dictionary<string, string>() { { "el", "Λάθος Προϊόν" }, { "en", "Wrong Product" } }),
                //    new ReturnReason("WRNGORD",new Dictionary<string, string>() { { "el", "Λαθος Παραγγελία" }, { "en", "Wrong Order" } }),
                //    new ReturnReason("LTDLV",new Dictionary<string, string>() { { "el", "Αργοπορημένη Παραγγελία" }, { "en", "Late Delivery" } }),
                //    new ReturnReason("BDQLPROD",new Dictionary<string, string>() { { "el", "Κακής Ποιότητας Προϊον" }, { "en", "Bad Quality Product" } })

                //};
            }
        }


        public List<CurrencyOption> CurrencyOptions
        {
            get
            {
                var isoCurrencySymbol = this.ContextsOfPreparedItems.FirstOrDefault()?.PreparationItems.OfType<ItemPreparation>().FirstOrDefault()?.ISOCurrencySymbol;
                if (string.IsNullOrWhiteSpace(isoCurrencySymbol))
                    isoCurrencySymbol = System.Globalization.RegionInfo.CurrentRegion.ISOCurrencySymbol;
                return new List<CurrencyOption>() {
                new CurrencyOption() { Amount=0.05M, ISOCurrencySymbol= isoCurrencySymbol },
                new CurrencyOption() { Amount=0.1M, ISOCurrencySymbol= isoCurrencySymbol },
                new CurrencyOption() { Amount=0.2M, ISOCurrencySymbol= isoCurrencySymbol },
                new CurrencyOption() { Amount=0.5M, ISOCurrencySymbol= isoCurrencySymbol },                new CurrencyOption() { Amount=1, ISOCurrencySymbol= isoCurrencySymbol },
                new CurrencyOption() { Amount=2, ISOCurrencySymbol= isoCurrencySymbol},
                new CurrencyOption() { Amount=5, ISOCurrencySymbol= isoCurrencySymbol },
                new CurrencyOption() { Amount=10, ISOCurrencySymbol= isoCurrencySymbol },
                new CurrencyOption() { Amount=20, ISOCurrencySymbol= isoCurrencySymbol },
                new CurrencyOption() { Amount=50, ISOCurrencySymbol= isoCurrencySymbol },
                new CurrencyOption() { Amount=100, ISOCurrencySymbol= isoCurrencySymbol },
                new CurrencyOption() { Amount=200, ISOCurrencySymbol= isoCurrencySymbol },
                new CurrencyOption() { Amount=500, ISOCurrencySymbol= isoCurrencySymbol }};
            }
        }



    }


}
