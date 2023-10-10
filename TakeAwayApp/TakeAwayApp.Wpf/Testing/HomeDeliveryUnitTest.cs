using CefSharp;
using FlavourBusinessFacade;
using FlavourBusinessFacade.EndUsers;
using FlavourBusinessFacade.HomeDelivery;
using FlavourBusinessFacade.RoomService;
using FlavourBusinessFacade.ServicesContextResources;
using FlavourBusinessFacade.ViewModel;
using FlavourBusinessManager.EndUsers;
using FlavourBusinessManager.ServicesContextResources;
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
        private List<FoodServiceClient> ClientsForUnitTest;

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

            var mockClientsJson = System.Text.Encoding.UTF8.GetString(Properties.Resources.mockClients);
            ClientsForUnitTest = OOAdvantech.Json.JsonConvert.DeserializeObject<List<FlavourBusinessManager.EndUsers.FoodServiceClient>>(mockClientsJson, settings);


            this.FlavoursServiceOrderTakingStation.HomeDeliveryCallCenterStation.FoodServicesSessionsSimulator.NewSimulateSession += FoodServicesSessionsSimulator_NewSimulateSession;


            this.FlavoursServiceOrderTakingStation.HomeDeliveryCallCenterStation.FoodServicesSessionsSimulator.StartClientSideSimulation(SessionType.HomeDelivery);
            this.FlavoursServiceOrderTakingStation.ObjectChangeState += FlavoursServiceOrderTakingStation_ObjectChangeState;
            
            //  this.FlavoursServiceOrderTakingStation.HomeDeliveryCallCenterStation.FoodServicesSessionsSimulator.NewSimulateSession -= FoodServicesSessionsSimulator_NewSimulateSession;

            //homeDeliverySession.OrderCommit();

        }

        private void FlavoursServiceOrderTakingStation_ObjectChangeState(object _object, string member)
        {
            if(member=="WatchingOrders")
                MakeOrdersAvalableForShipping();
        }

        private void MakeOrdersAvalableForShipping()
        {

            foreach (var watchingOrder in FlavoursServiceOrderTakingStation.WatchingOrders.Where(x => x.ServerWatchingOrder.ClientIdentity?.IndexOf("org_client_sim") == 0).ToList())
            {
                var ereere = watchingOrder.MealCourses.FirstOrDefault().FoodItemsInProgress;
                foreach (var foodItemsInProgress in watchingOrder.MealCourses.FirstOrDefault().FoodItemsInProgress)
                {

                    IPreparationStationRuntime preparationStation = null;
                    if (!string.IsNullOrWhiteSpace(foodItemsInProgress.PreparationStationIdentity))
                    {
                        if (!PreparationSations.TryGetValue(foodItemsInProgress.PreparationStationIdentity, out preparationStation))
                        {
                            var servicesContextManagment = FlavoursServiceOrderTakingStation.GetServicesContextManagment();

                            preparationStation = servicesContextManagment.GetPreparationStationRuntime(foodItemsInProgress.PreparationStationIdentity);
                            PreparationSations[foodItemsInProgress.PreparationStationIdentity] = preparationStation;
                        }
                        if(foodItemsInProgress.PreparationItems.Any(x=>x.State.IsInPreviousState(ItemPreparationState.Serving)))
                            preparationStation.ItemsServing(foodItemsInProgress.PreparationItems.Select(x => x.uid).ToList());

                    }
                }
            }
            
        }

        Dictionary<string, IPreparationStationRuntime> PreparationSations = new Dictionary<string, IPreparationStationRuntime>();

        private async void FoodServicesSessionsSimulator_NewSimulateSession(List<IItemPreparation> sessionItems)
        {

            var preparationItems = sessionItems.OfType<FlavourBusinessManager.RoomService.ItemPreparation>().Where(x => x.LoadMenuItem(MenuItems) != null).ToList();

            var homeDeliverySession = await this.FlavoursServiceOrderTakingStation.NewHomeDeliverySession();
            var client = ClientsForUnitTest.FirstOrDefault();
            ClientsForUnitTest.Remove(client);

            homeDeliverySession.CallerPhone = client.PhoneNumber;
            homeDeliverySession.DeliveryPlace = client.DeliveryPlaces[0];
            var homeDeliveryServicePoint = homeDeliverySession.GetNeighborhoodFoodServers(homeDeliverySession.DeliveryPlace.Location).FirstOrDefault();
            homeDeliverySession.HomeDeliveryServicePoint = homeDeliveryServicePoint;
            (homeDeliverySession.SessionClient.FoodServiceClient as FoodServiceClient).Synchronize(client);

            foreach (var preparation in preparationItems)
            {
                homeDeliverySession.FoodServiceClientSession.AddItem(preparation);
            }
            await homeDeliverySession.OrderCommit();


            //var watchingOrder = FlavoursServiceOrderTakingStation.WatchingOrders.Where(x => x.SessionID==homeDeliverySession.FoodServiceClientSession.MainSessionID).FirstOrDefault();

            //foreach (var foodItemsInProgress in watchingOrder.MealCourses.FirstOrDefault().FoodItemsInProgress)
            //{

            //    IPreparationStationRuntime preparationStation = null;
            //    if (!string.IsNullOrWhiteSpace(foodItemsInProgress.PreparationStationIdentity))
            //    {
            //        if (!PreparationSations.TryGetValue(foodItemsInProgress.PreparationStationIdentity, out preparationStation))
            //        {
            //            var servicesContextManagment = FlavoursServiceOrderTakingStation.GetServicesContextManagment();

            //            preparationStation = servicesContextManagment.GetPreparationStationRuntime(foodItemsInProgress.PreparationStationIdentity);
            //            PreparationSations[foodItemsInProgress.PreparationStationIdentity]= preparationStation;
            //        }
            //        preparationStation.ItemsPrepared(foodItemsInProgress.PreparationItems.Select(x => x.uid).ToList());

            //    }
            //}
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
