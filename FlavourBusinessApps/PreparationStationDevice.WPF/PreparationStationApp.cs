﻿using OOAdvantech.Remoting.RestApi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreparationStationDevice.WPF
{
    /// <MetaDataID>{58068d61-c538-45eb-940d-45e27989af68}</MetaDataID>
    public class PreparationStationApp
    {
        public static void Startup(string extraStoragePath)
        {
            ApplicationSettings.ExtraStoragePath = extraStoragePath;

            SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.MenuFoodItem"] = typeof(MenuModel.JsonViewModel.MenuFoodItem);
            SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.MenuItemPrice"] = typeof(MenuModel.JsonViewModel.MenuItemPrice);
            SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.OptionMenuItemSpecific"] = typeof(MenuModel.JsonViewModel.OptionMenuItemSpecific);
            SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.Option"] = typeof(MenuModel.JsonViewModel.Option);

            SerializationBinder.NamesTypesDictionary["FlavourBusinessManager.RoomService.ItemPreparation"] = typeof(FlavourBusinessManager.RoomService.ItemPreparation);
            SerializationBinder.NamesTypesDictionary["FlavourBusinessManager.RoomService.OptionChange"] = typeof(FlavourBusinessManager.RoomService.OptionChange);




            SerializationBinder.NamesTypesDictionary["PreparationStationDevice.PreparationStationItem"] = typeof(PreparationStationDevice.PreparationStationItem);
            SerializationBinder.NamesTypesDictionary["PreparationStationDevice.Ingredient"] = typeof(PreparationStationDevice.Ingredient);


            SerializationBinder.TypesNamesDictionary[typeof(FlavourBusinessManager.RoomService.ItemPreparation)] = "FlavourBusinessManager.RoomService.ItemPreparation";
            SerializationBinder.TypesNamesDictionary[typeof(FlavourBusinessManager.RoomService.OptionChange)] = "FlavourBusinessManager.RoomService.OptionChange";

            SerializationBinder.TypesNamesDictionary[typeof(PreparationStationDevice.PreparationStationItem)] = "PreparationStationDevice.PreparationStationItem";
            SerializationBinder.TypesNamesDictionary[typeof(PreparationStationDevice.Ingredient)] = "PreparationStationDevice.Ingredient";


            OOAdvantech.PersistenceLayer.StorageServerInstanceLocatorEx.SetStorageInstanceLocationServerUrl("http://192.168.2.2:8090/api/Storages");

        }
    }
}
