using OOAdvantech.Remoting.RestApi.Serialization;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

namespace MenuMaker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public App()
        {
            

            OOAdvantech.PersistenceLayer.StorageServerInstanceLocatorEx.SetStorageInstanceLocationServerUrl("http://192.168.2.4:8090/api/Storages");
            

            OOAdvantech.Remoting.RestApi.RemotingServices.ServerPublicUrl = "http://192.168.2.4:8090/api/";
            var ss = OOAdvantech.PersistenceLayer.StorageServerInstanceLocator.Current.GetSorageMetaData("7021ec91-37df-4417-8c1a-a6afb012fd09");

            var FontFamilies = System.Windows.Media.Fonts.GetFontFamilies(@"C:\ProgramData\Microneme\DontWaitWater\FontFiles\").ToList();
        }
        protected override void OnStartup(StartupEventArgs e)
        {

            var sdsd = MenuMaker.Properties.Resources.MenuMakerActivitiesLabel;
            //Type[] ParamTypes = new Type[] { typeof(System.Reflection.Assembly) };

            //var type = typeof(OOAdvantech.DotNetMetaDataRepository.Assembly);
            //System.Reflection.ConstructorInfo constructorInfo = type.GetConstructor(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public, null, ParamTypes, null);



            LoadRestApiTypeNamesDictionary();
            MenuPresentationModel.MenuStyles.Accent.ResourcesRootPath = @"C:\ProgramData\Microneme\DontWaitWater\";
            MenuPresentationModel.MenuStyles.PageStyle.ResourcesRootPath = @"C:\ProgramData\Microneme\DontWaitWater\";
            OOAdvantech.Remoting.RestApi.Authentication.InitializeFirebase("demomicroneme");

            StyleableWindow.FontDialog.InitFonts();
            List<string> mList = new List<string>() { "asd", "ASD" };
            int rt = mList.IndexOf(null);
            FrameworkElement.LanguageProperty.OverrideMetadata(
                                               typeof(FrameworkElement),
                                               new FrameworkPropertyMetadata(
                                                   XmlLanguage.GetLanguage(
                                                   CultureInfo.CurrentCulture.IetfLanguageTag)));


            //EventManager.RegisterClassHandler(typeof(UIElement), Window.PreviewMouseDownEvent, new MouseButtonEventHandler(OnPreviewMouseDown));

            // Backup(@"F:\NewPc\Azure blob storage\Backup");
            //Restore(@"F:\NewPc\Azure blob storage\Backup", "DevStorage", "", "");


            base.OnStartup(e);
        }

        private static void LoadRestApiTypeNamesDictionary()
        {
            SerializationBinder.NamesTypesDictionary["Array"] = typeof(object[]);
            SerializationBinder.NamesTypesDictionary["String"] = typeof(string);
            SerializationBinder.NamesTypesDictionary["Number"] = typeof(double);
            SerializationBinder.NamesTypesDictionary["Date"] = typeof(DateTime);
            SerializationBinder.NamesTypesDictionary["Array"] = typeof(List<>);
            SerializationBinder.NamesTypesDictionary["Map"] = typeof(Dictionary<,>);
            SerializationBinder.TypesNamesDictionary[typeof(bool)] = "Boolean";
            SerializationBinder.TypesNamesDictionary[typeof(int)] = "Number";
            SerializationBinder.TypesNamesDictionary[typeof(double)] = "Number";
            SerializationBinder.TypesNamesDictionary[typeof(decimal)] = "Number";
            SerializationBinder.TypesNamesDictionary[typeof(float)] = "Number";
            SerializationBinder.TypesNamesDictionary[typeof(short)] = "Number";
            SerializationBinder.TypesNamesDictionary[typeof(long)] = "Number";
            SerializationBinder.TypesNamesDictionary[typeof(string)] = "String";
            SerializationBinder.TypesNamesDictionary[typeof(DateTime)] = "Date";
            SerializationBinder.TypesNamesDictionary[typeof(object[])] = "Array";
            SerializationBinder.TypesNamesDictionary[typeof(List<>)] = "Array";
            SerializationBinder.TypesNamesDictionary[typeof(System.Collections.ObjectModel.ReadOnlyCollection<>)] = "Array";
            SerializationBinder.TypesNamesDictionary[typeof(Dictionary<,>)] = "Map";

            SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.MenuFoodItem"] = typeof(MenuModel.JsonViewModel.MenuFoodItem);
            SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.MenuItemPrice"] = typeof(MenuModel.JsonViewModel.MenuItemPrice);
            SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.OptionMenuItemSpecific"] = typeof(MenuModel.JsonViewModel.OptionMenuItemSpecific);
            SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.Option"] = typeof(MenuModel.JsonViewModel.Option);

            SerializationBinder.NamesTypesDictionary["FlavourBusinessManager.RoomService.ItemPreparation"] = typeof(FlavourBusinessManager.RoomService.ItemPreparation);
            SerializationBinder.NamesTypesDictionary["FlavourBusinessManager.RoomService.OptionChange"] = typeof(FlavourBusinessManager.RoomService.OptionChange);

            //SerializationBinder.NamesTypesDictionary["RestaurantHallLayoutModel.HallLayout"] = typeof(RestaurantHallLayoutModel.HallLayout);
            //SerializationBinder.NamesTypesDictionary["RestaurantHallLayoutModel.ServicePointShape"] = typeof(RestaurantHallLayoutModel.ServicePointShape);
            //SerializationBinder.NamesTypesDictionary["RestaurantHallLayoutModel.Shape"] = typeof(RestaurantHallLayoutModel.Shape);
            //SerializationBinder.NamesTypesDictionary["RestaurantHallLayoutModel.ShapesGroup"] = typeof(RestaurantHallLayoutModel.ShapesGroup);
            SerializationBinder.NamesTypesDictionary["UIBaseEx.Margin"] = typeof(UIBaseEx.Margin);
            SerializationBinder.NamesTypesDictionary["UIBaseEx.FontData"] = typeof(UIBaseEx.FontData);


            SerializationBinder.TypesNamesDictionary[typeof(FlavourBusinessManager.RoomService.ItemPreparation)] = "FlavourBusinessManager.RoomService.ItemPreparation";
            SerializationBinder.TypesNamesDictionary[typeof(FlavourBusinessManager.RoomService.OptionChange)] = "FlavourBusinessManager.RoomService.OptionChange";

            //SerializationBinder.TypesNamesDictionary[typeof(RestaurantHallLayoutModel.HallLayout)] = "RestaurantHallLayoutModel.HallLayout";
            //SerializationBinder.TypesNamesDictionary[typeof(RestaurantHallLayoutModel.ServicePointShape)] = "RestaurantHallLayoutModel.ServicePointShape";
            //SerializationBinder.TypesNamesDictionary[typeof(RestaurantHallLayoutModel.Shape)] = "RestaurantHallLayoutModel.Shape";
            //SerializationBinder.TypesNamesDictionary[typeof(RestaurantHallLayoutModel.ShapesGroup)] = "RestaurantHallLayoutModel.ShapesGroup";
            SerializationBinder.TypesNamesDictionary[typeof(UIBaseEx.Margin)] = "UIBaseEx.Margin";
            SerializationBinder.TypesNamesDictionary[typeof(UIBaseEx.FontData)] = "UIBaseEx.FontData";
        }

    }
    
}
