using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using OOAdvantech.Remoting.RestApi.Serialization;

namespace ServiceContextManagerApp.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {


        protected override void OnStartup(StartupEventArgs e)
        {
            //SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.MenuFoodItem"] = typeof(MenuModel.JsonViewModel.MenuFoodItem);
            //SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.MenuItemPrice"] = typeof(MenuModel.JsonViewModel.MenuItemPrice);
            //SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.OptionMenuItemSpecific"] = typeof(MenuModel.JsonViewModel.OptionMenuItemSpecific);
            //SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.Option"] = typeof(MenuModel.JsonViewModel.Option);

            //SerializationBinder.NamesTypesDictionary["FlavourBusinessManager.RoomService.ItemPreparation"] = typeof(FlavourBusinessManager.RoomService.ItemPreparation);
            //SerializationBinder.NamesTypesDictionary["FlavourBusinessManager.RoomService.OptionChange"] = typeof(FlavourBusinessManager.RoomService.OptionChange);

            //SerializationBinder.NamesTypesDictionary["PreparationStationDevice.PreparationStationItem"] = typeof(PreparationStationDevice.PreparationStationItem);
            //SerializationBinder.NamesTypesDictionary["PreparationStationDevice.Ingredient"] = typeof(PreparationStationDevice.Ingredient);


            //SerializationBinder.TypesNamesDictionary[typeof(FlavourBusinessManager.RoomService.ItemPreparation)] = "FlavourBusinessManager.RoomService.ItemPreparation";
            //SerializationBinder.TypesNamesDictionary[typeof(FlavourBusinessManager.RoomService.OptionChange)] = "FlavourBusinessManager.RoomService.OptionChange";

            //SerializationBinder.TypesNamesDictionary[typeof(PreparationStationDevice.PreparationStationItem)] = "PreparationStationDevice.PreparationStationItem";
            //SerializationBinder.TypesNamesDictionary[typeof(PreparationStationDevice.Ingredient)] = "PreparationStationDevice.Ingredient";

            //OOAdvantech.PersistenceLayer.StorageServerInstanceLocator.AddStorageLocatorExtender(new StorageLocatorEx());
            base.OnStartup(e);
        }

        protected override void OnActivated(EventArgs e)
        {
            OOAdvantech.Remoting.RestApi.Authentication.InitializeFirebase("demomicroneme");
            base.OnActivated(e);
        }
    }
}
