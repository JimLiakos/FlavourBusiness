using DontWaitApp;
using OOAdvantech.Json.Serialization;
using OOAdvantech.Remoting.RestApi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlavourBusinessApps.TakeAwayApp.WPF
{
    /// <MetaDataID>{8f191801-4c6c-4a70-b7e9-818f84ea701c}</MetaDataID>
    public class TakeAwayApp
    {
        class Tesa
        {
            public string Name { get; set; }
            public double? Age { get; set; }
            public int? torder { get; set; }

        }
        public static void Startup(string deviceID)
        {

            OOAdvantech.Net.DeviceOOAdvantechCore.DebugDeviceID = deviceID;
            ApplicationSettings.ExtraStoragePath = deviceID;

            //List<Tesa> col = new List<Tesa>() { new Tesa() { Name = "las" }, new Tesa() { Name = "las" ,torder=0}, new Tesa() { Name = "las" }, new Tesa() { Name = "las" } };
            
            //try
            //{

            //   //var asdsa  =new DefaultContractResolver().CreateProperties(typeof(FlavourBusinessManager.EndUsers.Place),OOAdvantech.Json.MemberSerialization.OptOut);
            //  // var sd= asdsa.OrderBy(p => p.Order ?? -1).ToList();
            //    var res = col.OrderBy(x => x.torder==null? 1: x.torder).ToList();
            //    OOAdvantech.Json.JsonConvert.DeserializeObject<List<FlavourBusinessManager.EndUsers.Place>>("[]");
            //}
            //catch (Exception error)
            //{

                
            //}

            SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.MenuFoodItem"] = typeof(MenuModel.JsonViewModel.MenuFoodItem);
            SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.MenuItemPrice"] = typeof(MenuModel.JsonViewModel.MenuItemPrice);
            SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.OptionMenuItemSpecific"] = typeof(MenuModel.JsonViewModel.OptionMenuItemSpecific);
            SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.Option"] = typeof(MenuModel.JsonViewModel.Option);

            SerializationBinder.NamesTypesDictionary["WaiterApp.ViewModel.ServingBatchPresentation"] = typeof(global::WaiterApp.ViewModel.ServingBatchPresentation);


            SerializationBinder.NamesTypesDictionary["FlavourBusinessManager.RoomService.ItemPreparation"] = typeof(FlavourBusinessManager.RoomService.ItemPreparation);
            SerializationBinder.NamesTypesDictionary["FlavourBusinessManager.RoomService.OptionChange"] = typeof(FlavourBusinessManager.RoomService.OptionChange);
            SerializationBinder.NamesTypesDictionary["MenuModel.MealType"] = typeof(MenuModel.MealType);
            SerializationBinder.NamesTypesDictionary["MenuModel.FixedMealType"] = typeof(MenuModel.FixedMealType);
            SerializationBinder.NamesTypesDictionary["MenuModel.MealCourseType"] = typeof(MenuModel.MealCourseType);


            SerializationBinder.NamesTypesDictionary["RestaurantHallLayoutModel.HallLayout"] = typeof(RestaurantHallLayoutModel.HallLayout);
            SerializationBinder.NamesTypesDictionary["RestaurantHallLayoutModel.ServicePointShape"] = typeof(RestaurantHallLayoutModel.ServicePointShape);
            SerializationBinder.NamesTypesDictionary["RestaurantHallLayoutModel.Shape"] = typeof(RestaurantHallLayoutModel.Shape);
            SerializationBinder.NamesTypesDictionary["RestaurantHallLayoutModel.ShapesGroup"] = typeof(RestaurantHallLayoutModel.ShapesGroup);
            SerializationBinder.NamesTypesDictionary["UIBaseEx.Margin"] = typeof(UIBaseEx.Margin);
            SerializationBinder.NamesTypesDictionary["UIBaseEx.FontData"] = typeof(UIBaseEx.FontData);
            SerializationBinder.NamesTypesDictionary["FlavourBusinessManager.EndUsers.Place"] = typeof(FlavourBusinessManager.EndUsers.Place);
            SerializationBinder.NamesTypesDictionary["FlavourBusinessFacade.EndUsers.Coordinate"] = typeof(FlavourBusinessFacade.EndUsers.Coordinate);
            SerializationBinder.NamesTypesDictionary["FlavourBusinessFacade.HomeDeliveryServicePointAbbreviation"] = typeof(FlavourBusinessFacade.HomeDeliveryServicePointAbbreviation);
            SerializationBinder.NamesTypesDictionary["TakeAwayApp.ViewModel.WatchingOrderPresentation"] = typeof(global::TakeAwayApp.ViewModel.WatchingOrderPresentation);
            SerializationBinder.NamesTypesDictionary["FlavourBusinessManager.RoomService.ViewModel.MealCourse"] = typeof(FlavourBusinessManager.RoomService.ViewModel.MealCourse);

            





            SerializationBinder.TypesNamesDictionary[typeof(FlavourBusinessManager.RoomService.ItemPreparation)] = "FlavourBusinessManager.RoomService.ItemPreparation";
            SerializationBinder.TypesNamesDictionary[typeof(FlavourBusinessManager.RoomService.OptionChange)] = "FlavourBusinessManager.RoomService.OptionChange";
            SerializationBinder.TypesNamesDictionary[typeof(MenuModel.MealType)] = "MenuModel.MealType";
            SerializationBinder.TypesNamesDictionary[typeof(MenuModel.FixedMealType)] = "MenuModel.FixedMealType";
            SerializationBinder.TypesNamesDictionary[typeof(MenuModel.MealCourseType)] = "MenuModel.MealCourseType";
            SerializationBinder.TypesNamesDictionary[typeof(global::WaiterApp.ViewModel.ServingBatchPresentation)] = "WaiterApp.ViewModel.ServingBatchPresentation";

            


            SerializationBinder.TypesNamesDictionary[typeof(RestaurantHallLayoutModel.HallLayout)] = "RestaurantHallLayoutModel.HallLayout";
            SerializationBinder.TypesNamesDictionary[typeof(RestaurantHallLayoutModel.ServicePointShape)] = "RestaurantHallLayoutModel.ServicePointShape";
            SerializationBinder.TypesNamesDictionary[typeof(RestaurantHallLayoutModel.Shape)] = "RestaurantHallLayoutModel.Shape";
            SerializationBinder.TypesNamesDictionary[typeof(RestaurantHallLayoutModel.ShapesGroup)] = "RestaurantHallLayoutModel.ShapesGroup";
            SerializationBinder.TypesNamesDictionary[typeof(FlavourBusinessManager.EndUsers.Place)] = "FlavourBusinessManager.EndUsers.Place";
            SerializationBinder.TypesNamesDictionary[typeof(FlavourBusinessFacade.EndUsers.Coordinate)] = "FlavourBusinessFacade.EndUsers.Coordinate";
            SerializationBinder.TypesNamesDictionary[typeof(FlavourBusinessFacade.HomeDeliveryServicePointAbbreviation)]="FlavourBusinessFacade.HomeDeliveryServicePointAbbreviation";

            SerializationBinder.TypesNamesDictionary[typeof(global::TakeAwayApp.ViewModel.WatchingOrderPresentation)] = "TakeAwayApp.ViewModel.WatchingOrderPresentation";
            SerializationBinder.TypesNamesDictionary[typeof(FlavourBusinessManager.RoomService.ViewModel.MealCourse)] = "FlavourBusinessManager.RoomService.ViewModel.MealCourse";

            


            SerializationBinder.TypesNamesDictionary[typeof(UIBaseEx.Margin)] = "UIBaseEx.Margin";
            SerializationBinder.TypesNamesDictionary[typeof(UIBaseEx.FontData)] = "UIBaseEx.FontData";
        }
    }
}
