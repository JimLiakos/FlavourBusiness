using FlavourBusinessFacade.RoomService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Json;

namespace ServiceContextManagerApp
{
    /// <MetaDataID>{e2ff48b0-ff9f-41df-b754-a190bfc34d1e}</MetaDataID>
    public class ΜealCourse
    {
        /// <MetaDataID>{a3a9913d-4df3-42a1-9424-7fbd636669ac}</MetaDataID>
        
        [JsonIgnore]
        public IMealCourse ServerSideMealCourse { get; }
        


        public string Description { get; }
        public IList<ItemsPreparationContext> FoodItemsInProgress { get; }

        /// <MetaDataID>{f2cb7dc5-4e40-4f3a-a09a-dda9dcd27a0b}</MetaDataID>
        public ΜealCourse(IMealCourse serverSideMealCourse)
        {
            ServerSideMealCourse = serverSideMealCourse;
            Description = ServerSideMealCourse.Meal.Session.Description+" - " +ServerSideMealCourse.Name;

            FoodItemsInProgress = serverSideMealCourse.FoodItemsInProgress;
        }


    }
}
