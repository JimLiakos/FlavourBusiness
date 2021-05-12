using OOAdvantech.PersistenceLayer;
using OOAdvantech.Remoting.RestApi.Serialization;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
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


            EventManager.RegisterClassHandler(typeof(UIElement), Window.PreviewMouseDownEvent, new MouseButtonEventHandler(OnPreviewMouseDown));

            // Backup(@"F:\NewPc\Azure blob storage\Backup");
            //Restore(@"F:\NewPc\Azure blob storage\Backup", "DevStorage", "", "");

            // Restore(@"C:\Projects\Azure blob storage\Backup", "DevStorage", "", "");


            base.OnStartup(e);
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
            OOAdvantech.PersistenceLayer.StorageServerInstanceLocatorEx.SetStorageInstanceLocationServerUrl("http://192.168.2.2:8090/api/Storages");
            OOAdvantech.Remoting.RestApi.RemotingServices.ServerPublicUrl = "http://192.168.2.2:8090/api/";
            var FontFamilies = System.Windows.Media.Fonts.GetFontFamilies(@"C:\ProgramData\Microneme\DontWaitWater\FontFiles\").ToList();
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

            SerializationBinder.NamesTypesDictionary["FlavourBusinessManager.RoomService.ItemPreparation"] = typeof(FlavourBusinessManager.RoomService.ItemPreparation);
            SerializationBinder.NamesTypesDictionary["FlavourBusinessManager.RoomService.OptionChange"] = typeof(FlavourBusinessManager.RoomService.OptionChange);

            SerializationBinder.NamesTypesDictionary["RestaurantHallLayoutModel.HallLayout"] = typeof(RestaurantHallLayoutModel.HallLayout);
            SerializationBinder.NamesTypesDictionary["RestaurantHallLayoutModel.ServicePointShape"] = typeof(RestaurantHallLayoutModel.ServicePointShape);
            SerializationBinder.NamesTypesDictionary["RestaurantHallLayoutModel.Shape"] = typeof(RestaurantHallLayoutModel.Shape);
            SerializationBinder.NamesTypesDictionary["RestaurantHallLayoutModel.ShapesGroup"] = typeof(RestaurantHallLayoutModel.ShapesGroup);
            SerializationBinder.NamesTypesDictionary["UIBaseEx.Margin"] = typeof(UIBaseEx.Margin);
            SerializationBinder.NamesTypesDictionary["UIBaseEx.FontData"] = typeof(UIBaseEx.FontData);


            SerializationBinder.TypesNamesDictionary[typeof(FlavourBusinessManager.RoomService.ItemPreparation)] = "FlavourBusinessManager.RoomService.ItemPreparation";
            SerializationBinder.TypesNamesDictionary[typeof(FlavourBusinessManager.RoomService.OptionChange)] = "FlavourBusinessManager.RoomService.OptionChange";

            SerializationBinder.TypesNamesDictionary[typeof(RestaurantHallLayoutModel.HallLayout)] = "RestaurantHallLayoutModel.HallLayout";
            SerializationBinder.TypesNamesDictionary[typeof(RestaurantHallLayoutModel.ServicePointShape)] = "RestaurantHallLayoutModel.ServicePointShape";
            SerializationBinder.TypesNamesDictionary[typeof(RestaurantHallLayoutModel.Shape)] = "RestaurantHallLayoutModel.Shape";
            SerializationBinder.TypesNamesDictionary[typeof(RestaurantHallLayoutModel.ShapesGroup)] = "RestaurantHallLayoutModel.ShapesGroup";
            SerializationBinder.TypesNamesDictionary[typeof(UIBaseEx.Margin)] = "UIBaseEx.Margin";
            SerializationBinder.TypesNamesDictionary[typeof(UIBaseEx.FontData)] = "UIBaseEx.FontData";
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
            demoStorage = ObjectStorage.OpenStorage(storageName, storageLocation, storageType);
            archive = new OOAdvantech.WindowsAzureTablesPersistenceRunTime.CloudBlockBlobArchive(string.Format(@"{0}\{1}.dat", backupFolder, storageName));
            demoStorage.Backup(archive);
        }
        private static void Restore(string backupFolder, string storageLocation, string accountName, string accountKey)
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
                ObjectStorage.Restore(archive, "jimliakosgmailcom", storageLocation, "OOAdvantech.WindowsAzureTablesPersistenceRunTime.StorageProvider", false, accountName, accountKey);

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
