using CefSharp;
using FlavourBusinessFacade;
using FlavourBusinessFacade.EndUsers;
using FlavourBusinessFacade.ViewModel;
using FlavourBusinessManager.EndUsers;
using MenuModel.JsonViewModel;
using OOAdvantech.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
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
        public HomeDeliveryUnitTest(FlavoursServiceOrderTakingStation flavoursServiceOrderTakingStation)
        {

            FlavoursServiceOrderTakingStation = flavoursServiceOrderTakingStation;

            var restaurantMenuDataSharedUri = FlavoursServiceOrderTakingStation.HomeDeliveryCallCenterStation.RestaurantMenuDataSharedUri;
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
            var settings = new JsonSerializerSettings
            {
                Converters = {
                     new AbstractConverter<FlavourBusinessManager.EndUsers.Place,IPlace>(),
                     new AbstractConverter<UserData ,FlavourBusinessFacade.IUser>()

                    }
            };

            var mockClientsJson = System.Text.Encoding.Default.GetString(Properties.Resources.mockClients);
           var clientsForUnitTest= OOAdvantech.Json.JsonConvert.DeserializeObject<List<FlavourBusinessManager.EndUsers.FoodServiceClient>>(mockClientsJson, settings);


            IHomeDeliverySession homeDeliverySession = await FlavoursServiceOrderTakingStation.NewHomeDeliverSession();
            homeDeliverySession.CallerPhone = clientsForUnitTest.FirstOrDefault().PhoneNumber;

            (homeDeliverySession.SessionClient.FoodServiceClient as FoodServiceClient).Synchronize(clientsForUnitTest.FirstOrDefault());

            homeDeliverySession.DeliveryPlace=homeDeliverySession.SessionClient.FoodServiceClient.DeliveryPlaces.FirstOrDefault();

            
            

            string tt = OOAdvantech.Json.JsonConvert.SerializeObject(homeDeliverySession.SessionClient.FoodServiceClient);

            var sdsds = homeDeliverySession.FoodServiceClientSession.OrderItems;

            var watchingOrders = FlavoursServiceOrderTakingStation.WatchingOrders.Where(x => x.SessionID == homeDeliverySession.FoodServiceClientSession.MainSessionID).FirstOrDefault();

            var preparation = watchingOrders.MealCourses.FirstOrDefault()?.FoodItemsInProgress.FirstOrDefault()?.PreparationStationIdentity;
            //homeDeliverySession.OrderCommit();

        }
    }


    public class AbstractConverter<TReal, TAbstract>
    : JsonConverter where TReal : TAbstract
    {
        public override Boolean CanConvert(Type objectType)
            => objectType == typeof(TAbstract);

        public override Object ReadJson(JsonReader reader, Type type, Object value, JsonSerializer jser)
            => jser.Deserialize<TReal>(reader);

        public override void WriteJson(JsonWriter writer, Object value, JsonSerializer jser)
            => jser.Serialize(writer, value);
    }
}
