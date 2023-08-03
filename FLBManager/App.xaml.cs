using FlavourBusinessManager;
using FlavourBusinessManager.RoomService;
using OOAdvantech.Json;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Remoting.RestApi.Serialization;
using QRCoder;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Device.Location;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;

namespace FLBManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// <MetaDataID>{8dce946e-b282-4c6a-98e2-74b5ec4baa41}</MetaDataID>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {

            string foodCategory = "Καφέδες(47); Σουβλάκια(61); Pizza(48); Κινέζικη(18); Κρέπες(21); Burgers(46); Sushi(26); Γλυκά(24); Μαγειρευτά(40); Ζυμαρικά(36); Μεξικάνικη(5); Νηστίσιμα(16); Βάφλες(10); Ινδική(13); Vegan(8); Brunch(20); Vegetarian(5); Hot Dog(5); Γαλακτοκομικά(1); Ασιατική(24); Σφολιάτες(8); Θαλασσινά(21); Σαλάτες(44); Ζαχαροπλαστείο(10); Cocktails(16); Ιταλική(16); Ψητά - Grill(95); Αρτοποιήματα(2); Sandwich(33); Παγωτό(21); Kebab(12); Πεϊνιρλί(7); Ποτά(13); Ethnic(10); Cool food(6); Κοτόπουλα(5); Ανατολίτικη(8); Φαλάφελ(6); Τσέχικη(1); Κεντροευρωπαϊκή(1); Μεζεδοπωλείο(7); Μεσογειακή(9); Ελληνική(9); Πατσάς(1); Snacks(4); Donuts(2); Πιροσκί(1); Λουκουμάδες(3)";

            var foodCategoriesNames = foodCategory.Split(';').Select(x => x.Split('(')[0]).ToList();

            GeoCoordinateWatcher watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);



            var location = watcher.Position.Location;
            System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("el");
            System.Threading.Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("el");

            //try
            //{
            //    var auth = new AuthFlavourBusiness();
            //}
            //catch (Exception error)
            //{
            //}
            //List<FlavourBusinessFacade.RoomService.IItemPreparation> itemPreparations = new List<FlavourBusinessFacade.RoomService.IItemPreparation>();
            //FlavourBusinessManager.RoomService.ItemPreparation itemPreparation = new FlavourBusinessManager.RoomService.ItemPreparation() { State = FlavourBusinessFacade.RoomService.ItemPreparationState.IsPrepared };
            //itemPreparations.Add(itemPreparation);
            //itemPreparation = new FlavourBusinessManager.RoomService.ItemPreparation() { State = FlavourBusinessFacade.RoomService.ItemPreparationState.Serving };
            //itemPreparations.Add(itemPreparation);
            //itemPreparation = new FlavourBusinessManager.RoomService.ItemPreparation() { State = FlavourBusinessFacade.RoomService.ItemPreparationState.Committed };
            //itemPreparations.Add(itemPreparation);

            //var sdsd = itemPreparations.GetMinimumCommonItemPreparationState();
            //var ww = sdsd.ToString();

            //var start = DateTime.Now;
            //FlavourBusinessManagerApp.Init("angularhost", "YxNQAvlMWX7e7Dz78w/WaV3Z9VlISStF+Xp2DGigFScQmEuC/bdtiFqKqagJhNIwhsgF9aWHZIcpnFHl4bHHKw==", "angularhost", "$web");

            //var loadTimeSpan = DateTime.Now - start;
            //System.Diagnostics.Debug.WriteLine(loadTimeSpan);
            //Type[] ParamTypes = new Type[] { typeof(System.Reflection.Assembly) };

            //var type = typeof(OOAdvantech.DotNetMetaDataRepository.Assembly);
            //System.Reflection.ConstructorInfo constructorInfo = type.GetConstructor(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public, null, ParamTypes, null);


            //string storageName = "jimliakosgmailcom";
            //string storageLocation = "DevStorage";
            //string storageType = "OOAdvantech.WindowsAzureTablesPersistenceRunTime.StorageProvider";

            //var demoStorage = ObjectStorage.OpenStorage(storageName, storageLocation, storageType);


            //OOAdvantech.Linq.Storage servicesContextStorage = new OOAdvantech.Linq.Storage(demoStorage);

            //var itemsTimeSpans = (from itemTimeSpan in servicesContextStorage.GetObjectCollection<FlavourBusinessManager.ServicesContextResources.ItemPreparationTimeSpan>()
            //                      select itemTimeSpan).ToList();

            //var preparationStationStatistics = (from itemTimeSpan in itemsTimeSpans//servicesContextStorage.GetObjectCollection<FlavourBusinessManager.ServicesContextResources.ItemPreparationTimeSpan>()
            //                                    group itemTimeSpan by itemTimeSpan.ItemsInfoObjectUri into preparationItemsInfoStatistics
            //                                    select preparationItemsInfoStatistics).ToList();

            //foreach (var preparationItemsInfoStatistics in preparationStationStatistics)
            //{
            //    var preparationTimeSpan = preparationItemsInfoStatistics.Sum(x => (x.EndsAt - x.StartsAt).TotalMinutes * x.InformationValue) / preparationItemsInfoStatistics.Sum(x => x.InformationValue);
            //    var preparationForecastTimeSpan = preparationItemsInfoStatistics.Sum(x => x.ActualTimeSpanInMin) / preparationItemsInfoStatistics.Count();
            //    var itemsPreparationInf = (from itemsPreparationInfo in servicesContextStorage.GetObjectCollection<FlavourBusinessManager.ServicesContextResources.ItemsPreparationInfo>()
            //                               where itemsPreparationInfo.ItemsInfoObjectUri == preparationItemsInfoStatistics.Key
            //                               select itemsPreparationInfo).FirstOrDefault();
            //}

            // var sds = items[0].ClientSession;
            //var sAsdsd = new AuthFlavourBusiness();

            // var objSt= AuthFlavourBusiness.OpenFlavourBusinessesStorage();
            //var sdsao= (objSt.StorageMetaData as OOAdvantech.WindowsAzureTablesPersistenceRunTime.Storage).Components.Select(x => x.Identity.ToString()).ToArray();
            //var comp = (objSt.StorageMetaData as OOAdvantech.WindowsAzureTablesPersistenceRunTime.Storage).Components[6];
            //OOAdvantech.Json.JsonConvert.SerializeObject(objSt.StorageMetaData);

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


            EventManager.RegisterClassHandler(typeof(UIElement), Window.PreviewMouseDownEvent, new MouseButtonEventHandler(OnPreviewMouseDown));




            //var obk= FlavourBusinessManagerApp.OpenFlavourBusinessesResourcesStorage("angularhost", "YxNQAvlMWX7e7Dz78w/WaV3Z9VlISStF+Xp2DGigFScQmEuC/bdtiFqKqagJhNIwhsgF9aWHZIcpnFHl4bHHKw==", "angularhost");


            //Backup(@"F:\NewPc\Azure blob storage\Backup");
            //Backup(@"F:\X-Drive\Source\OpenVersions\FlavourBusiness\Data\Backup");
            //Backup(@"F:\X-Drive\Source\OpenVersions\FlavourBusiness\FlavourBusinessApps\Backup");

            //Backup(@"F:\myproject\terpo\OpenVersions\FlavourBusiness\FlavourBusinessApps\Backup");

            //Restore(@"F:\NewPc\Azure blob storage\Backup", "DevStorage", "", "", true);
            //Restore(@"C:\Projects\OpenVersions\FlavourBusiness\FlavourBusinessApps\Backup", "DevStorage", "", "", true);

            //Restore(@"F:\X-Drive\Source\OpenVersions\FlavourBusiness\FlavourBusinessApps\Backup", "DevStorage", "", "",true);

            //Restore(@"F:\myproject\terpo\OpenVersions\FlavourBusiness\FlavourBusinessApps\Backup", "DevStorage", "", "",true);
            //Restore(@"F:\X-Drive\Source\OpenVersions\FlavourBusiness\Data\Backup", "DevStorage", "", "", true);

            //Restore(@"F:\NewPc\Azure blob storage\Backup", "DevStorage", "", "");

            //Restore(@"F:\NewPc\Azure blob storage\Backup", "DevStorage", "", "");
            //Restore(@"F:\NewPc\Azure blob storage\Backup", "angularhost", "angularhost", "YxNQAvlMWX7e7Dz78w/WaV3Z9VlISStF+Xp2DGigFScQmEuC/bdtiFqKqagJhNIwhsgF9aWHZIcpnFHl4bHHKw==");


            //string storageName = "jimliakosgmailcom";
            //string storageLocation = "DevStorage";
            //string storageType = "OOAdvantech.WindowsAzureTablesPersistenceRunTime.StorageProvider";

            //var storage = new OOAdvantech.Linq.Storage(ObjectStorage.OpenStorage(storageName,
            //                                                storageLocation,
            //                                                storageType));

            //var m_meal = (from meal in storage.GetObjectCollection<FlavourBusinessManager.RoomService.Meal>()
            //              select meal).FirstOrDefault();
            //var sds = m_meal.Courses;

            //CreateQRCodeCards();
            // PreparationTimeEstimator();

            base.OnStartup(e);
        }


        void CreateQRCodeCards()
        {

            int code = 1001;
            string color = HexConverter(Color.DeepPink);
            string codeValue = code.ToString() + color.Replace("#", "_");

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(codeValue, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            var qrCodeImage = qrCode.GetGraphic(20, color, "#FFFFFF", true);

            qrCodeImage.Save(@"f:\qrImage" + code.ToString() + ".png", System.Drawing.Imaging.ImageFormat.Png);

            code = 1002;
            color = HexConverter(Color.DarkOrange);
            codeValue = code.ToString() + color.Replace("#", "_");
            qrGenerator = new QRCodeGenerator();
            qrCodeData = qrGenerator.CreateQrCode(codeValue, QRCodeGenerator.ECCLevel.Q);
            qrCode = new QRCode(qrCodeData);
            qrCodeImage = qrCode.GetGraphic(20, color, "#FFFFFF", true);
            qrCodeImage.Save(@"f:\qrImage" + code.ToString() + ".png", System.Drawing.Imaging.ImageFormat.Png);


            code = 1003;
            color = HexConverter(Color.MediumBlue);
            codeValue = code.ToString() + color.Replace("#", "_");
            qrGenerator = new QRCodeGenerator();
            qrCodeData = qrGenerator.CreateQrCode(codeValue, QRCodeGenerator.ECCLevel.Q);
            qrCode = new QRCode(qrCodeData);
            qrCodeImage = qrCode.GetGraphic(20, color, "#FFFFFF", true);
            qrCodeImage.Save(@"f:\qrImage" + code.ToString() + ".png", System.Drawing.Imaging.ImageFormat.Png);

            code = 1004;
            color = HexConverter(Color.Brown);
            codeValue = code.ToString() + color.Replace("#", "_");
            qrGenerator = new QRCodeGenerator();
            qrCodeData = qrGenerator.CreateQrCode(codeValue, QRCodeGenerator.ECCLevel.Q);
            qrCode = new QRCode(qrCodeData);
            qrCodeImage = qrCode.GetGraphic(20, color, "#FFFFFF", true);
            qrCodeImage.Save(@"f:\qrImage" + code.ToString() + ".png", System.Drawing.Imaging.ImageFormat.Png);

            code = 1005;
            color = HexConverter(Color.Teal);
            codeValue = code.ToString() + color.Replace("#", "_");
            qrGenerator = new QRCodeGenerator();
            qrCodeData = qrGenerator.CreateQrCode(codeValue, QRCodeGenerator.ECCLevel.Q);
            qrCode = new QRCode(qrCodeData);
            qrCodeImage = qrCode.GetGraphic(20, color, "#FFFFFF", true);
            qrCodeImage.Save(@"f:\qrImage" + code.ToString() + ".png", System.Drawing.Imaging.ImageFormat.Png);
        }
        private static String HexConverter(System.Drawing.Color c)
        {
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }

        static bool HelpMode = false;
        static void OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

            if (FLBAuthentication.Views.SignInWindow.Current != null)
                FLBAuthentication.Views.SignInWindow.Current.OnPreviewMouseDown(sender, e);

            UIElement uiElement = sender as UIElement;
            if (uiElement != null)
            {
                if (uiElement.Uid == "serma")
                {
                    if (HelpMode)
                        e.Handled = true;
                }
            }
        }

        public App()
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += CurrentDomain_UnhandledException;
            OOAdvantech.PersistenceLayer.StorageServerInstanceLocatorEx.SetStorageInstanceLocationServerUrl(string.Format("http://{0}:8090/api/Storages", FlavourBusinessFacade.ComputingResources.EndPoint.Server));
            OOAdvantech.Remoting.RestApi.RemotingServices.ServerPublicUrl = string.Format("http://{0}:8090/api/", FlavourBusinessFacade.ComputingResources.EndPoint.Server);
            var FontFamilies = System.Windows.Media.Fonts.GetFontFamilies(@"C:\ProgramData\Microneme\DontWaitWater\FontFiles\").ToList();


            Dictionary<char, char> directory = new Dictionary<char, char> { { 'ά', 'α' }, { 'έ', 'ε' }, { 'ή', 'η' }, { 'ί', 'ι' }, { 'ό', 'ο' }, { 'ύ', 'υ' }, { 'ώ', 'ω' } };

            var sds = JsonConvert.SerializeObject(directory);


        }
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs args)
        {

            Exception e = (Exception)args.ExceptionObject;
            InformEventLog(e);
            Console.WriteLine("MyHandler caught : " + e.Message);
            Console.WriteLine("StackTrace caught : " + e.StackTrace);
            Console.WriteLine("Runtime terminating: {0}", args.IsTerminating);
            MessageBox.Show(e.Message + " " + e.StackTrace);
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
            SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.Level"] = typeof(MenuModel.JsonViewModel.Level);
            SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.ScaleType"] = typeof(MenuModel.JsonViewModel.ScaleType);
            SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.PartofMeal"] = typeof(MenuModel.JsonViewModel.PartofMeal);
            SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.Tag"] = typeof(MenuModel.JsonViewModel.Tag);
            SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.PartofMeal"] = typeof(MenuModel.JsonViewModel.PartofMeal);
            SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.MealType"] = typeof(MenuModel.JsonViewModel.MealType);
            SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.MealCourseType"] = typeof(MenuModel.JsonViewModel.MealCourseType);
            SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.OptionGroup"] = typeof(MenuModel.JsonViewModel.OptionGroup);
            SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.ItemSelectorOption"] = typeof(MenuModel.JsonViewModel.ItemSelectorOption);

            SerializationBinder.NamesTypesDictionary["FlavourBusinessManager.RoomService.ItemPreparation"] = typeof(FlavourBusinessManager.RoomService.ItemPreparation);
            SerializationBinder.NamesTypesDictionary["FlavourBusinessManager.RoomService.OptionChange"] = typeof(FlavourBusinessManager.RoomService.OptionChange);
            SerializationBinder.NamesTypesDictionary["FlavourBusinessManager.EndUsers.Place"] = typeof(FlavourBusinessManager.EndUsers.Place);
            SerializationBinder.NamesTypesDictionary["FlavourBusinessFacade.EndUsers.Coordinate"] = typeof(FlavourBusinessFacade.EndUsers.Coordinate);
            
            SerializationBinder.NamesTypesDictionary["MenuModel.MealType"] = typeof(MenuModel.MealType);
            SerializationBinder.NamesTypesDictionary["MenuModel.FixedMealType"] = typeof(MenuModel.FixedMealType);
            SerializationBinder.NamesTypesDictionary["MenuModel.MealCourseType"] = typeof(MenuModel.MealCourseType);
            SerializationBinder.NamesTypesDictionary["FlavourBusinessManager.EndUsers.Place"] = typeof(FlavourBusinessManager.EndUsers.Place);


            SerializationBinder.NamesTypesDictionary["RestaurantHallLayoutModel.HallLayout"] = typeof(RestaurantHallLayoutModel.HallLayout);
            SerializationBinder.NamesTypesDictionary["RestaurantHallLayoutModel.ServicePointShape"] = typeof(RestaurantHallLayoutModel.ServicePointShape);
            SerializationBinder.NamesTypesDictionary["RestaurantHallLayoutModel.Shape"] = typeof(RestaurantHallLayoutModel.Shape);
            SerializationBinder.NamesTypesDictionary["RestaurantHallLayoutModel.ShapesGroup"] = typeof(RestaurantHallLayoutModel.ShapesGroup);
            SerializationBinder.NamesTypesDictionary["UIBaseEx.Margin"] = typeof(UIBaseEx.Margin);
            SerializationBinder.NamesTypesDictionary["UIBaseEx.FontData"] = typeof(UIBaseEx.FontData);
            SerializationBinder.NamesTypesDictionary["FLBManager.ViewModel.FoodTypeTagPresentation"] = typeof(FLBManager.ViewModel.FoodTypeTagPresentation);

              

            SerializationBinder.TypesNamesDictionary[typeof(FlavourBusinessManager.RoomService.ItemPreparation)] = "FlavourBusinessManager.RoomService.ItemPreparation";
            SerializationBinder.TypesNamesDictionary[typeof(FlavourBusinessManager.RoomService.OptionChange)] = "FlavourBusinessManager.RoomService.OptionChange";
            SerializationBinder.TypesNamesDictionary[typeof(FlavourBusinessManager.EndUsers.Place)] = "FlavourBusinessManager.EndUsers.Place";
            SerializationBinder.TypesNamesDictionary[typeof(FlavourBusinessFacade.EndUsers.Coordinate)] = "FlavourBusinessFacade.EndUsers.Coordinate";
            
            SerializationBinder.TypesNamesDictionary[typeof(MenuModel.MealType)] = "MenuModel.MealType";
            SerializationBinder.TypesNamesDictionary[typeof(MenuModel.FixedMealType)] = "MenuModel.FixedMealType";
            SerializationBinder.TypesNamesDictionary[typeof(MenuModel.MealCourseType)] = "MenuModel.MealCourseType";
            SerializationBinder.TypesNamesDictionary[typeof(FlavourBusinessManager.EndUsers.Place)] = "FlavourBusinessManager.EndUsers.Place";


            SerializationBinder.TypesNamesDictionary[typeof(RestaurantHallLayoutModel.HallLayout)] = "RestaurantHallLayoutModel.HallLayout";
            SerializationBinder.TypesNamesDictionary[typeof(RestaurantHallLayoutModel.ServicePointShape)] = "RestaurantHallLayoutModel.ServicePointShape";
            SerializationBinder.TypesNamesDictionary[typeof(RestaurantHallLayoutModel.Shape)] = "RestaurantHallLayoutModel.Shape";
            SerializationBinder.TypesNamesDictionary[typeof(RestaurantHallLayoutModel.ShapesGroup)] = "RestaurantHallLayoutModel.ShapesGroup";
            SerializationBinder.TypesNamesDictionary[typeof(UIBaseEx.Margin)] = "UIBaseEx.Margin";
            SerializationBinder.TypesNamesDictionary[typeof(UIBaseEx.FontData)] = "UIBaseEx.FontData";
             
            SerializationBinder.TypesNamesDictionary[typeof(MenuModel.JsonViewModel.MenuFoodItem)] = "MenuModel.JsonViewModel.MenuFoodItem";
            SerializationBinder.TypesNamesDictionary[typeof(MenuModel.JsonViewModel.MenuItemPrice)] = "MenuModel.JsonViewModel.MenuItemPrice";
            SerializationBinder.TypesNamesDictionary[typeof(MenuModel.JsonViewModel.OptionMenuItemSpecific)] = "MenuModel.JsonViewModel.OptionMenuItemSpecific";
            SerializationBinder.TypesNamesDictionary[typeof(MenuModel.JsonViewModel.Option)] = "MenuModel.JsonViewModel.Option";
            SerializationBinder.TypesNamesDictionary[typeof(MenuModel.JsonViewModel.OptionGroup)] = "MenuModel.JsonViewModel.OptionGroup";
            SerializationBinder.TypesNamesDictionary[typeof(MenuModel.JsonViewModel.ItemSelectorOption)] = "MenuModel.JsonViewModel.ItemSelectorOption";

            SerializationBinder.TypesNamesDictionary[typeof(MenuModel.JsonViewModel.Level)] = "MenuModel.JsonViewModel.Level";
            SerializationBinder.TypesNamesDictionary[typeof(MenuModel.JsonViewModel.ScaleType)] = "MenuModel.JsonViewModel.ScaleType";

            SerializationBinder.TypesNamesDictionary[typeof(MenuModel.JsonViewModel.PartofMeal)] = "MenuModel.JsonViewModel.PartofMeal";
            SerializationBinder.TypesNamesDictionary[typeof(MenuModel.JsonViewModel.Tag)] = "MenuModel.JsonViewModel.Tag";
            SerializationBinder.TypesNamesDictionary[typeof(MenuModel.JsonViewModel.PartofMeal)] = "MenuModel.JsonViewModel.PartofMeal";
            SerializationBinder.TypesNamesDictionary[typeof(MenuModel.JsonViewModel.MealType)] = "MenuModel.JsonViewModel.MealType";
            SerializationBinder.TypesNamesDictionary[typeof(MenuModel.JsonViewModel.MealCourseType)] = "MenuModel.JsonViewModel.MealCourseType";
            SerializationBinder.TypesNamesDictionary[typeof(FLBManager.ViewModel.FoodTypeTagPresentation)] = "FLBManager.ViewModel.FoodTypeTagPresentation";



        }


        public static void InformEventLog(System.Exception Error)
        {
#if !DeviceDotNet
            if (!System.Diagnostics.EventLog.SourceExists("MenuDesgner", "."))
            {
                System.Diagnostics.EventLog.CreateEventSource("MenuDesgner", "MenuDesgnerApp");
            }

            //TODO γεμισει με message το log file τοτε παράγει exception
            System.Diagnostics.EventLog myLog = new System.Diagnostics.EventLog();
            myLog.Source = "MenuDesgner";
            if (myLog.OverflowAction != System.Diagnostics.OverflowAction.OverwriteAsNeeded)
                myLog.ModifyOverflowPolicy(System.Diagnostics.OverflowAction.OverwriteAsNeeded, 0);

            System.Diagnostics.Debug.WriteLine(
                Error.Message + Error.StackTrace);
            myLog.WriteEntry(Error.Message + Error.StackTrace, System.Diagnostics.EventLogEntryType.Error);
            System.Exception InerError = Error.InnerException;
            while (InerError != null)
            {
                //TODO να διαχειρίζωμε σωστά το log file μηπως και κάνει overflow

                System.Diagnostics.Debug.WriteLine(
                    InerError.Message + InerError.StackTrace);
                myLog.WriteEntry(InerError.Message + InerError.StackTrace, System.Diagnostics.EventLogEntryType.Error);
                InerError = InerError.InnerException;
            }
#endif
        }


        private static void BackupRestoreTest()
        {
            string backupFolder = @"E:\backup";

            // UsersTest();
            // Backup(backupFolder);



            //using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            //{
            //    foreach (var rtable in (demoStorage.StorageMetaData as OOAdvantech.MetaDataRepository.Namespace).OwnedElements.OfType<OOAdvantech.RDBMSMetaDataRepository.Table>())
            //    {
            //        var name = rtable.Name;
            //        var dname = rtable.DataBaseTableName;

            //        if (name.IndexOf("_")!=-1)
            //            continue;
            //        rtable.Name = name;
            //        rtable.DataBaseTableName = name;
            //    } 
            //    stateTransition.Consistent = true;
            //}


            //var id = demoStorage.StorageMetaData.StorageIdentity;
            string storageLocation = "https://angularhost.blob.core.windows.net";
            string accountName = "angularhost";
            string accountKey = "YxNQAvlMWX7e7Dz78w/WaV3Z9VlISStF+Xp2DGigFScQmEuC/bdtiFqKqagJhNIwhsgF9aWHZIcpnFHl4bHHKw==";


            //Restore(backupFolder, "DevStorage", "", "");

            //ObjectStorage storageSession = null;
            //string storageName = "jimliakosgmailcom";
            //storageLocation = "DevStorage";
            //string storageType = "OOAdvantech.WindowsAzureTablesPersistenceRunTime.StorageProvider";

            //try
            //{
            //    var objectStorage = ObjectStorage.OpenStorage(storageName,
            //                                                 storageLocation,
            //                                                 storageType, "", "");


            //    lock (objectStorage)
            //    {
            //        try
            //        {
            //            if (objectStorage.StorageMetaData.CheckForVersionUpgrate(typeof(IOrganization).Assembly.FullName))
            //            {
            //                objectStorage.StorageMetaData.RegisterComponent(typeof(IOrganization).Assembly.FullName);
            //                objectStorage.StorageMetaData.RegisterComponent(typeof(Organization).Assembly.FullName);
            //            }
            //            else if (objectStorage.StorageMetaData.CheckForVersionUpgrate(typeof(Organization).Assembly.FullName))
            //            {
            //                objectStorage.StorageMetaData.RegisterComponent(typeof(Organization).Assembly.FullName);
            //            }
            //        }
            //        catch (Exception error)
            //        {
            //            throw;
            //        }
            //    }

            //    //objectStorage.StorageMetaData.RegisterComponent(typeof(Organization).Assembly.FullName, new System.Xml.Linq.XDocument());

            //}
            //catch(System.Exception error)
            //{

            //}
            int ss = 0;
            //Restore(backupFolder, storageLocation, accountName, accountKey);




            //try
            //{
            //    var demoStorage = ObjectStorage.OpenStorage(storageName, storageLocation, storageType);

            //    (demoStorage as OOAdvantech.WindowsAzureTablesPersistenceRunTime.ObjectStorage).ClearTemporaryFiles();

            //}
            //catch (Exception error)
            //{
            //    ObjectStorage.Repair( "jimliakosgmailcom", "DevStorage", "OOAdvantech.WindowsAzureTablesPersistenceRunTime.StorageProvider", false);

            //}


            //storageName = "FlavourBusinesses";
            //archive = new OOAdvantech.WindowsAzureTablesPersistenceRunTime.CloudBlockBlobArchive(string.Format(@"e:\backup\{0}.dat", storageName));
            //ObjectStorage.Restore(archive, storageName, "DevStorage", "OOAdvantech.WindowsAzureTablesPersistenceRunTime.StorageProvider", false);


            //storageName = "FlavourBusinessesResources";
            //archive = new OOAdvantech.WindowsAzureTablesPersistenceRunTime.CloudBlockBlobArchive(string.Format(@"e:\backup\{0}.dat", storageName));
            //ObjectStorage.Restore(archive, storageName, "DevStorage", "OOAdvantech.WindowsAzureTablesPersistenceRunTime.StorageProvider", false);



            //var objectStorage = ObjectStorage.OpenStorage(storageName,
            //                                           storageLocation,
            //                                           storageType);

            //objectStorage.Backup(atchive);





            // WebBrowserHelper.FixBrowserVersion();

            // string rtr= "9CAF873D-8193-4DDA-A712-0F4BADB1A25F".ToLower();
        }

        private static void Backup(string backupFolder)
        {

            if (!System.IO.Directory.Exists(backupFolder))
                System.IO.Directory.CreateDirectory(backupFolder);
            string storageName = "FlavourBusinesses";
            string storageLocation = "DevStorage";
            string storageType = "OOAdvantech.WindowsAzureTablesPersistenceRunTime.StorageProvider";


            var demoStorage = ObjectStorage.OpenStorage(storageName, storageLocation, storageType);
            OOAdvantech.WindowsAzureTablesPersistenceRunTime.CloudBlockBlobArchive archive = new OOAdvantech.WindowsAzureTablesPersistenceRunTime.CloudBlockBlobArchive(string.Format(@"{0}\{1}.dat", backupFolder, storageName));
            demoStorage.Backup(archive);

            storageName = "FlavourBusinessesResources";
            demoStorage = ObjectStorage.OpenStorage(storageName, storageLocation, storageType);
            archive = new OOAdvantech.WindowsAzureTablesPersistenceRunTime.CloudBlockBlobArchive(string.Format(@"{0}\{1}.dat", backupFolder, storageName));
            demoStorage.Backup(archive);

            storageName = "jimliakosgmailcom";
            string suffix = "";
            int i = 1;
            while (System.IO.File.Exists(string.Format(@"{0}\{1}.dat", backupFolder, storageName + suffix)))
                suffix = (i++).ToString();

            demoStorage = ObjectStorage.OpenStorage(storageName, storageLocation, storageType); // (storageName, "angularhost", storageType, "angularhost", "YxNQAvlMWX7e7Dz78w/WaV3Z9VlISStF+Xp2DGigFScQmEuC/bdtiFqKqagJhNIwhsgF9aWHZIcpnFHl4bHHKw==");
            archive = new OOAdvantech.WindowsAzureTablesPersistenceRunTime.CloudBlockBlobArchive(string.Format(@"{0}\{1}.dat", backupFolder, storageName + suffix));
            demoStorage.Backup(archive);
        }
        private static void Restore(string backupFolder, string storageLocation, string accountName, string accountKey, bool overrideObjectStorage)
        {
            try
            {
                var archive = new OOAdvantech.WindowsAzureTablesPersistenceRunTime.CloudBlockBlobArchive(string.Format(@"{0}\{1}.dat", backupFolder, "FlavourBusinesses"));
                ObjectStorage.Restore(archive, "FlavourBusinesses", storageLocation, "OOAdvantech.WindowsAzureTablesPersistenceRunTime.StorageProvider", false, accountName, accountKey);

            }
            catch (Exception error)
            {
            }


            try
            {
                var archive = new OOAdvantech.WindowsAzureTablesPersistenceRunTime.CloudBlockBlobArchive(string.Format(@"{0}\{1}.dat", backupFolder, "FlavourBusinessesResources"));
                ObjectStorage.Restore(archive, "FlavourBusinessesResources", storageLocation, "OOAdvantech.WindowsAzureTablesPersistenceRunTime.StorageProvider", false, accountName, accountKey);

            }
            catch (Exception error)
            {
            }
            try
            {
                var archive = new OOAdvantech.WindowsAzureTablesPersistenceRunTime.CloudBlockBlobArchive(string.Format(@"{0}\{1}.dat", backupFolder, "jimliakosgmailcom"));
                ObjectStorage.Restore(archive, "jimliakosgmailcom", storageLocation, "OOAdvantech.WindowsAzureTablesPersistenceRunTime.StorageProvider", false, accountName, accountKey, overrideObjectStorage);

            }
            catch (Exception error)
            {
            }

        }


        private static void MetaDataRepositoryMultiThreadTest()
        {
            OOAdvantech.Collections.Generic.Set<string> multiThreadSet = new OOAdvantech.Collections.Generic.Set<string>();
            multiThreadSet.Add("Lora");
            multiThreadSet.Add("nora");

            var taska = Task.Run(() =>
            {
                int i = 100;
                while (i > 0)
                {
                    multiThreadSet.Add(i.ToString());
                    System.Threading.Thread.Sleep(2);
                    i--;
                }

                foreach (var rtype in typeof(OOAdvantech.RDBMSMetaDataRepository.Attribute).Assembly.GetTypes())
                {
                    var attributep = OOAdvantech.MetaDataRepository.Classifier.GetClassifier(rtype).Features.OfType<OOAdvantech.MetaDataRepository.Attribute>().Where(x => x.Name == "Description").FirstOrDefault();
                    System.Threading.Thread.Sleep(2);

                }


                Debug.WriteLine(" ");
            });
            var taskb = Task.Run(() =>
            {
                int i = 100;
                while (i > 0)
                {

                    multiThreadSet.Add(i.ToString());
                    System.Threading.Thread.Sleep(2);
                    i--;
                }
                foreach (var rtype in typeof(OOAdvantech.RDBMSMetaDataRepository.Attribute).Assembly.GetTypes())
                {
                    var attributep = OOAdvantech.MetaDataRepository.Classifier.GetClassifier(rtype).Features.OfType<OOAdvantech.MetaDataRepository.Attribute>().Where(x => x.Name == "Description").FirstOrDefault();
                    System.Threading.Thread.Sleep(2);
                }
                Debug.WriteLine(" ");
            });
            var taskc = Task.Run(() =>
            {
                int i = 100;
                while (i > 0)
                {
                    System.Threading.Thread.Sleep(2);
                    multiThreadSet.Add(i.ToString());
                    i--;
                }
                foreach (var rtype in typeof(OOAdvantech.RDBMSMetaDataRepository.Attribute).Assembly.GetTypes())
                {
                    var attributep = OOAdvantech.MetaDataRepository.Classifier.GetClassifier(rtype).Features.OfType<OOAdvantech.MetaDataRepository.Attribute>().Where(x => x.Name == "Description").FirstOrDefault();
                    System.Threading.Thread.Sleep(2);
                }
                Debug.WriteLine(" ");
            });
            var taskd = Task.Run(() =>
            {
                System.Threading.Thread.Sleep(2);
                foreach (var item in multiThreadSet.ToThreadSafeList())
                {
                    System.Threading.Thread.Sleep(2);
                }
                foreach (var item in multiThreadSet.ToThreadSafeList())
                {
                    System.Threading.Thread.Sleep(2);
                }
                foreach (var rtype in typeof(OOAdvantech.RDBMSMetaDataRepository.Attribute).Assembly.GetTypes())
                {
                    foreach (var sds in OOAdvantech.MetaDataRepository.Classifier.GetClassifier(rtype).GetRoles(true))
                    {
                        foreach (var real in sds.AssociationEndRealizations)
                        {

                        }
                    }
                    System.Threading.Thread.Sleep(2);
                }
                Debug.WriteLine(" ");
            });
            var taske = Task.Run(() =>
            {
                System.Threading.Thread.Sleep(2);
                foreach (var item in multiThreadSet.ToThreadSafeList())
                    System.Threading.Thread.Sleep(2);
                foreach (var rtype in typeof(OOAdvantech.RDBMSMetaDataRepository.Attribute).Assembly.GetTypes().Reverse())
                {
                    System.Threading.Thread.Sleep(2);
                    AssociateRoles(rtype);
                }
                Debug.WriteLine(" ");
            });
            var taskf = Task.Run(() =>
            {
                System.Threading.Thread.Sleep(2);
                foreach (var item in multiThreadSet.ToThreadSafeList())
                    System.Threading.Thread.Sleep(2);
                foreach (var rtype in typeof(OOAdvantech.RDBMSMetaDataRepository.Attribute).Assembly.GetTypes().Reverse())
                {
                    System.Threading.Thread.Sleep(2);
                    AssociateRoles(rtype);
                }
                Debug.WriteLine(" ");
            });
            var taskme = Task.Run(() =>
            {
                System.Threading.Thread.Sleep(2);
                foreach (var item in multiThreadSet.ToThreadSafeList())
                    System.Threading.Thread.Sleep(2);

                foreach (var rtype in typeof(MenuModel.ScaleType).Assembly.GetTypes().Reverse())
                {
                    System.Threading.Thread.Sleep(2);
                    AssociateRoles(rtype);
                }
            });
            var taskmf = Task.Run(() =>
            {
                System.Threading.Thread.Sleep(2);
                foreach (var item in multiThreadSet.ToThreadSafeList())
                    System.Threading.Thread.Sleep(2);
                foreach (var rtype in typeof(MenuModel.ScaleType).Assembly.GetTypes())
                {
                    System.Threading.Thread.Sleep(2);
                    AssociateRoles(rtype);
                }
            });

            taska.Wait();
            taskb.Wait();
            taskc.Wait();
            taskd.Wait();
            taske.Wait();
            taskf.Wait();
            taskme.Wait();
            taskmf.Wait();
        }

        private static void AssociateRoles(Type rtype)
        {
            foreach (var sds in OOAdvantech.MetaDataRepository.Classifier.GetClassifier(rtype).GetAssociateRoles(true))
            {
                foreach (var real in sds.AssociationEndRealizations)
                {
                    OOAdvantech.MetaDataRepository.Class _class = real.Namespace as OOAdvantech.MetaDataRepository.Class;
                    if (_class != null)
                    {
                        _class.IsPersistent(real.Specification);
                        _class.IsMultilingual(real.Specification);
                        _class.HasReferentialIntegrity(real.Specification);
                    }
                }
            }
        }


    }



}
