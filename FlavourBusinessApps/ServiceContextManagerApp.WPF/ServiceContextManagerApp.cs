﻿using OOAdvantech.Remoting.RestApi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlavourBusinessApps.ServiceContextManagerApp.WPF
{
    /// <MetaDataID>{c7734dfe-91d5-4243-a2ee-685cd785b83c}</MetaDataID>
    public class ServiceContextManagerApp
    {
        public static void Startup()
        {

            //OOAdvantech.Net.DeviceOOAdvantechCore.DebugDeviceID = "81000000296";
            //ApplicationSettings.ExtraStoragePath = "81000000296";



            SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.MenuFoodItem"] = typeof(MenuModel.JsonViewModel.MenuFoodItem);
            SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.MenuItemPrice"] = typeof(MenuModel.JsonViewModel.MenuItemPrice);
            SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.OptionMenuItemSpecific"] = typeof(MenuModel.JsonViewModel.OptionMenuItemSpecific);
            SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.Option"] = typeof(MenuModel.JsonViewModel.Option);

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

            SerializationBinder.NamesTypesDictionary["FlavourBusinessManager.RoomService.ViewModel.MealCourse"] = typeof(FlavourBusinessManager.RoomService.ViewModel.MealCourse);

            SerializationBinder.NamesTypesDictionary["WaiterApp.ViewModel.ServingBatchPresentation"] = typeof(global::WaiterApp.ViewModel.ServingBatchPresentation);
            SerializationBinder.NamesTypesDictionary["FlavourBusinessFacade.RoomService.DelayedServingBatchAbbreviation"] = typeof(FlavourBusinessFacade.RoomService.DelayedServingBatchAbbreviation);
            SerializationBinder.NamesTypesDictionary["CourierApp.ViewModel.FoodShippingPresentation"] = typeof(global::CourierApp.ViewModel.FoodShippingPresentation);
            SerializationBinder.NamesTypesDictionary["FlavourBusinessManager.EndUsers.Place"] = typeof(FlavourBusinessManager.EndUsers.Place);
            SerializationBinder.NamesTypesDictionary["FlavourBusinessFacade.Shipping.ReturnReason"] = typeof(FlavourBusinessFacade.Shipping.ReturnReason);

            SerializationBinder.TypesNamesDictionary[typeof(FlavourBusinessManager.RoomService.ItemPreparation)] = "FlavourBusinessManager.RoomService.ItemPreparation";
            SerializationBinder.TypesNamesDictionary[typeof(FlavourBusinessManager.RoomService.OptionChange)] = "FlavourBusinessManager.RoomService.OptionChange";
            SerializationBinder.TypesNamesDictionary[typeof(MenuModel.MealType)] = "MenuModel.MealType";
            SerializationBinder.TypesNamesDictionary[typeof(MenuModel.FixedMealType)] = "MenuModel.FixedMealType";
            SerializationBinder.TypesNamesDictionary[typeof(MenuModel.MealCourseType)] = "MenuModel.MealCourseType";


            SerializationBinder.TypesNamesDictionary[typeof(RestaurantHallLayoutModel.HallLayout)] = "RestaurantHallLayoutModel.HallLayout";
            SerializationBinder.TypesNamesDictionary[typeof(RestaurantHallLayoutModel.ServicePointShape)] = "RestaurantHallLayoutModel.ServicePointShape";
            SerializationBinder.TypesNamesDictionary[typeof(RestaurantHallLayoutModel.Shape)] = "RestaurantHallLayoutModel.Shape";
            SerializationBinder.TypesNamesDictionary[typeof(RestaurantHallLayoutModel.ShapesGroup)] = "RestaurantHallLayoutModel.ShapesGroup";

            SerializationBinder.TypesNamesDictionary[typeof(UIBaseEx.Margin)] = "UIBaseEx.Margin";
            SerializationBinder.TypesNamesDictionary[typeof(UIBaseEx.FontData)] = "UIBaseEx.FontData";
            SerializationBinder.TypesNamesDictionary[typeof(FlavourBusinessManager.RoomService.ViewModel.MealCourse)] = "FlavourBusinessManager.RoomService.ViewModel.MealCourse";
            SerializationBinder.TypesNamesDictionary[typeof(FlavourBusinessFacade.RoomService.DelayedServingBatchAbbreviation)] = "FlavourBusinessFacade.RoomService.DelayedServingBatchAbbreviation";
            SerializationBinder.TypesNamesDictionary[typeof(global::WaiterApp.ViewModel.ServingBatchPresentation)] = "WaiterApp.ViewModel.ServingBatchPresentation";
            SerializationBinder.TypesNamesDictionary[typeof(global::CourierApp.ViewModel.FoodShippingPresentation)] = "CourierApp.ViewModel.FoodShippingPresentation";
            SerializationBinder.TypesNamesDictionary[typeof(FlavourBusinessFacade.Shipping.ReturnReason)] = "FlavourBusinessFacade.Shipping.ReturnReason";

            SerializationBinder.TypesNamesDictionary[typeof(global::FlavourBusinessManager.EndUsers.Place)] = "FlavourBusinessManager.EndUsers.Place";
            

        } 
    }
}
