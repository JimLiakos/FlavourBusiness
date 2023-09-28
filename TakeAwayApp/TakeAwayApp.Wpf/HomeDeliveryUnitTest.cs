using CefSharp;
using MenuModel.JsonViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TakeAwayApp.ViewModel;

namespace TakeAwayApp.Wpf
{
    /// <MetaDataID>{8f5d3d21-a991-4457-b000-7d19d2b1c8ce}</MetaDataID>
    public class HomeDeliveryUnitTest
    {
        /// <MetaDataID>{0baf0495-b27f-42d3-bcea-10111dc76a57}</MetaDataID>
        FlavoursServiceOrderTakingStation FlavoursServiceOrderTakingStation;
        public HomeDeliveryUnitTest(FlavoursServiceOrderTakingStation flavoursServiceOrderTakingStation) {

            FlavoursServiceOrderTakingStation = flavoursServiceOrderTakingStation;

            var restaurantMenuDataSharedUri=FlavoursServiceOrderTakingStation.HomeDeliveryCallCenterStation.RestaurantMenuDataSharedUri;
            HttpClient httpClient = new HttpClient();
            var getJsonTask = httpClient.GetStringAsync(restaurantMenuDataSharedUri);
            getJsonTask.Wait();
            var json = getJsonTask.Result;
            var jSetttings = OOAdvantech.Remoting.RestApi.Serialization.JsonSerializerSettings.TypeRefDeserializeSettings;
            
            MenuItems = OOAdvantech.Json.JsonConvert.DeserializeObject<List<MenuModel.JsonViewModel.MenuFoodItem>>(json, jSetttings).ToDictionary(x => x.Uri);

             
        }
         
        public Dictionary<string, MenuFoodItem> MenuItems { get; }

        internal async Task AssignShippingsToCourierTest()
        {
           IHomeDeliverySession homeDeliverySession = await FlavoursServiceOrderTakingStation.NewHomeDeliverSession();
            homeDeliverySession.CallerPhone = "6972992632";

            var sdsd = homeDeliverySession.DeliveryPlace;

            var sdsds = homeDeliverySession.FoodServiceClientSession.OrderItems;

            var watchingOrders = FlavoursServiceOrderTakingStation.WatchingOrders.Where(x => x.SessionID == homeDeliverySession.FoodServiceClientSession.MainSessionID).FirstOrDefault();

            var preparation = watchingOrders.MealCourses.FirstOrDefault()?.FoodItemsInProgress.FirstOrDefault()?.PreparationStationIdentity;

        }
    }
}
