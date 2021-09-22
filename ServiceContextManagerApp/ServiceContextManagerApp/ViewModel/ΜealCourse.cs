using FlavourBusinessFacade.RoomService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Json;
using UIBaseEx;

namespace FlavourBusinessManager.RoomService.ViewModel
{
    /// <MetaDataID>{e2ff48b0-ff9f-41df-b754-a190bfc34d1e}</MetaDataID>
    public class MealCourse
    {
        /// <MetaDataID>{a3a9913d-4df3-42a1-9424-7fbd636669ac}</MetaDataID>

        [JsonIgnore]
        public IMealCourse ServerSideMealCourse { get; }

        

        public string Description { get; }
        public IList<ItemsPreparationContext> FoodItemsInProgress { get; }

        public DontWaitApp.MenuData MenuData { get; set; }

        [JsonIgnore]
        ServiceContextManagerApp.ServicesContextPresentation ServicesContextPresentation;

        /// <MetaDataID>{f2cb7dc5-4e40-4f3a-a09a-dda9dcd27a0b}</MetaDataID>
        public MealCourse(IMealCourse serverSideMealCourse, ServiceContextManagerApp.ServicesContextPresentation servicesContextPresentation)
        {
            ServerSideMealCourse = serverSideMealCourse;
            ServicesContextPresentation = servicesContextPresentation;

            Description = ServerSideMealCourse.Meal.Session.Description + " - " + ServerSideMealCourse.Name;

            FoodItemsInProgress = serverSideMealCourse.FoodItemsInProgress;
            var sessionData = ServerSideMealCourse.SessionData;
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

            serverSideMealCourse.ItemsStateChanged += ServerSideMealCourse_ItemsStateChanged;



        }

        private void ServerSideMealCourse_ItemsStateChanged(Dictionary<string, ItemPreparationState> newItemsState)
        {
            ServicesContextPresentation.OnItemsStateChanged(newItemsState);


        }
    }
}
