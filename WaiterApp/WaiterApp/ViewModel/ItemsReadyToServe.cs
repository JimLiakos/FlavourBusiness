using DontWaitApp;
using FlavourBusinessFacade.RoomService;
using FlavourBusinessFacade.ServicesContextResources;
using System;
using System.Collections.Generic;
using System.Text;

namespace WaiterApp.ViewModel
{
    /// <MetaDataID>{c6caec47-b340-48ee-8f09-428622c0945c}</MetaDataID>
    public class ServingBatchPresentation:System.IDisposable
    {
        public ServingBatchPresentation()
        {

        }

        public void Dispose()
        {

        }
        public string ServiceBatchIdentity { get; set; }

        public readonly IServingBatch ServingBatch;
        public readonly  WaiterPresentation WaiterPresentation;
        public ServingBatchPresentation(IServingBatch servingBatch,WaiterPresentation waiterPresentation)
        {
            WaiterPresentation = waiterPresentation;
            ServingBatch = servingBatch;
            ServingBatch.ObjectChangeState += ServingBatchChangeState;
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
                DefaultMealTypeUri = sessionData.DefaultMealTypeUri
            };
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

            WaiterPresentation.ServingBatchUpdated(this);
        }

        public IList<ItemsPreparationContext> AllContextsOfPreparedItems { get; private set; }

        public MenuData MenuData { get; private set; }
        public string Description { get; private set; }
        public string ServicesPointIdentity { get; private set; }
        public IList<ItemsPreparationContext> ContextsOfPreparedItems { get; private set; }
        public IList<ItemsPreparationContext> ContextsOfUnderPreparationItems { get; private set; }
        public ServicePointType ServicePointType { get; private set; }
    }
}
