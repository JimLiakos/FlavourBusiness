﻿using OOAdvantech.Remoting.RestApi.Serialization;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TakeAwayApp
{
    /// <MetaDataID>{50f35b6d-bba8-4dea-bda3-c9a6898e0a0a}</MetaDataID>
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            string storageMetadataGetFullUrl = string.Format("http://{0}:8090/api/Storages", FlavourBusinessFacade.ComputingResources.EndPoint.Server);
            // OOAdvantech.PersistenceLayer.StorageServerInstanceLocatorEx.SetStorageInstanceLocationServerUrl(storageMetadataGetFullUrl);

            SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.MenuFoodItem"] = typeof(MenuModel.JsonViewModel.MenuFoodItem);
            SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.MenuItemPrice"] = typeof(MenuModel.JsonViewModel.MenuItemPrice);
            SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.OptionMenuItemSpecific"] = typeof(MenuModel.JsonViewModel.OptionMenuItemSpecific);
            SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.Option"] = typeof(MenuModel.JsonViewModel.Option);
            SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.OptionGroup"] = typeof(MenuModel.JsonViewModel.OptionGroup);

            SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.ScaleType"] = typeof(MenuModel.JsonViewModel.ScaleType);
            SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.Level"] = typeof(MenuModel.JsonViewModel.Level);
            SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.CustomizedPrice"] = typeof(MenuModel.JsonViewModel.CustomizedPrice);
            SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.ItemSelectorOption"] = typeof(MenuModel.JsonViewModel.ItemSelectorOption);


            //SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.PartofMeal"] = typeof(MenuModel.JsonViewModel.PartofMeal);
            //SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.MealType"] = typeof(MenuModel.JsonViewModel.MealType);
            //SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.MealCourseType"] = typeof(MenuModel.JsonViewModel.MealCourseType);
            SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.Tag"] = typeof(MenuModel.JsonViewModel.Tag);
            //SerializationBinder.NamesTypesDictionary["MenuModel.ITag"] = typeof(MenuModel.ITag);
            SerializationBinder.NamesTypesDictionary["MenuModel.Tag"] = typeof(MenuModel.JsonViewModel.Tag);
            SerializationBinder.NamesTypesDictionary["FlavourBusinessManager.EndUsers.Place"] = typeof(FlavourBusinessManager.EndUsers.Place);
            SerializationBinder.NamesTypesDictionary["FlavourBusinessFacade.EndUsers.Coordinate"] = typeof(FlavourBusinessFacade.EndUsers.Coordinate);
            SerializationBinder.NamesTypesDictionary["FlavourBusinessFacade.HomeDeliveryServicePointInfo"] = typeof(FlavourBusinessFacade.HomeDeliveryServicePointInfo);
            SerializationBinder.NamesTypesDictionary["FlavourBusinessFacade.HomeDeliveryServicePointAbbreviation"] = typeof(FlavourBusinessFacade.HomeDeliveryServicePointAbbreviation);
            SerializationBinder.NamesTypesDictionary["TakeAwayApp.ViewModel.WatchingOrderPresentation"] = typeof(global::TakeAwayApp.ViewModel.WatchingOrderPresentation);
            SerializationBinder.NamesTypesDictionary["FlavourBusinessManager.RoomService.ViewModel.MealCourse"] = typeof(FlavourBusinessManager.RoomService.ViewModel.MealCourse);


            SerializationBinder.NamesTypesDictionary["FlavourBusinessManager.RoomService.ItemPreparation"] = typeof(FlavourBusinessManager.RoomService.ItemPreparation);
            SerializationBinder.NamesTypesDictionary["FlavourBusinessManager.RoomService.OptionChange"] = typeof(FlavourBusinessManager.RoomService.OptionChange);

            SerializationBinder.NamesTypesDictionary["RestaurantHallLayoutModel.HallLayout"] = typeof(RestaurantHallLayoutModel.HallLayout);
            SerializationBinder.NamesTypesDictionary["RestaurantHallLayoutModel.ServicePointShape"] = typeof(RestaurantHallLayoutModel.ServicePointShape);
            SerializationBinder.NamesTypesDictionary["RestaurantHallLayoutModel.Shape"] = typeof(RestaurantHallLayoutModel.Shape);
            SerializationBinder.NamesTypesDictionary["RestaurantHallLayoutModel.ShapesGroup"] = typeof(RestaurantHallLayoutModel.ShapesGroup);
            SerializationBinder.NamesTypesDictionary["UIBaseEx.Margin"] = typeof(UIBaseEx.Margin);
            SerializationBinder.NamesTypesDictionary["UIBaseEx.FontData"] = typeof(UIBaseEx.FontData);



            SerializationBinder.TypesNamesDictionary[typeof(MenuModel.JsonViewModel.MenuFoodItem)] = "MenuModel.JsonViewModel.MenuFoodItem, MenuModel";
            SerializationBinder.TypesNamesDictionary[typeof(MenuModel.JsonViewModel.MenuItemPrice)] = "MenuModel.JsonViewModel.MenuItemPrice";
            SerializationBinder.TypesNamesDictionary[typeof(MenuModel.JsonViewModel.OptionMenuItemSpecific)] = "MenuModel.JsonViewModel.OptionMenuItemSpecific";
            SerializationBinder.TypesNamesDictionary[typeof(MenuModel.JsonViewModel.Option)] = "MenuModel.JsonViewModel.Option";
            SerializationBinder.TypesNamesDictionary[typeof(MenuModel.JsonViewModel.OptionGroup)] = "MenuModel.JsonViewModel.OptionGroup";

            SerializationBinder.TypesNamesDictionary[typeof(MenuModel.JsonViewModel.ScaleType)]= "MenuModel.JsonViewModel.ScaleType";
            SerializationBinder.TypesNamesDictionary[typeof(MenuModel.JsonViewModel.Level)] = "MenuModel.JsonViewModel.Level";
            SerializationBinder.TypesNamesDictionary[typeof(MenuModel.JsonViewModel.CustomizedPrice)] = "MenuModel.JsonViewModel.CustomizedPrice";
            //SerializationBinder.TypesNamesDictionary[typeof(MenuModel.JsonViewModel.PartofMeal)] = "MenuModel.JsonViewModel.PartofMeal";
            //SerializationBinder.TypesNamesDictionary[typeof(MenuModel.JsonViewModel.MealCourseType)] = "MenuModel.JsonViewModel.MealCourseType";
            //SerializationBinder.TypesNamesDictionary[typeof(MenuModel.JsonViewModel.MealType)] = "MenuModel.JsonViewModel.MealType";
            SerializationBinder.TypesNamesDictionary[typeof(MenuModel.JsonViewModel.Tag)] = "MenuModel.JsonViewModel.Tag";
            //SerializationBinder.TypesNamesDictionary[typeof(MenuModel.ITag)] = "MenuModel.ITag";
            SerializationBinder.TypesNamesDictionary[typeof(FlavourBusinessManager.EndUsers.Place)] = "FlavourBusinessManager.EndUsers.Place";
            SerializationBinder.TypesNamesDictionary[typeof(FlavourBusinessFacade.EndUsers.Coordinate)] = "FlavourBusinessFacade.EndUsers.Coordinate";
            SerializationBinder.TypesNamesDictionary[typeof(FlavourBusinessFacade.HomeDeliveryServicePointInfo)] = "FlavourBusinessFacade.HomeDeliveryServicePointInfo";
            SerializationBinder.TypesNamesDictionary[typeof(FlavourBusinessFacade.HomeDeliveryServicePointAbbreviation)] = "FlavourBusinessFacade.HomeDeliveryServicePointAbbreviation";
            SerializationBinder.TypesNamesDictionary[typeof(global::TakeAwayApp.ViewModel.WatchingOrderPresentation)] = "TakeAwayApp.ViewModel.WatchingOrderPresentation";
            SerializationBinder.TypesNamesDictionary[typeof(FlavourBusinessManager.RoomService.ViewModel.MealCourse)] = "FlavourBusinessManager.RoomService.ViewModel.MealCourse";




            SerializationBinder.TypesNamesDictionary[typeof(FlavourBusinessManager.RoomService.ItemPreparation)] = "FlavourBusinessManager.RoomService.ItemPreparation";
            SerializationBinder.TypesNamesDictionary[typeof(FlavourBusinessManager.RoomService.OptionChange)] = "FlavourBusinessManager.RoomService.OptionChange";

            SerializationBinder.TypesNamesDictionary[typeof(RestaurantHallLayoutModel.HallLayout)] = "RestaurantHallLayoutModel.HallLayout";
            SerializationBinder.TypesNamesDictionary[typeof(RestaurantHallLayoutModel.ServicePointShape)] = "RestaurantHallLayoutModel.ServicePointShape";
            SerializationBinder.TypesNamesDictionary[typeof(RestaurantHallLayoutModel.Shape)] = "RestaurantHallLayoutModel.Shape";
            SerializationBinder.TypesNamesDictionary[typeof(RestaurantHallLayoutModel.ShapesGroup)] = "RestaurantHallLayoutModel.ShapesGroup";



            SerializationBinder.TypesNamesDictionary[typeof(UIBaseEx.Margin)] = "UIBaseEx.Margin";
            SerializationBinder.TypesNamesDictionary[typeof(UIBaseEx.FontData)] = "UIBaseEx.FontData";
            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
