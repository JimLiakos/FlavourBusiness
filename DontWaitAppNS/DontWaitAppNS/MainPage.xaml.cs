using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Reflection;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Transactions;
using OOAdvantech;
using OOAdvantech.DotNetMetaDataRepository;
using ZXing.Net.Mobile.Forms;

namespace DontWaitApp
{
    /// <MetaDataID>{0d87920d-1a36-4b04-8c08-f58d2e63b01d}</MetaDataID>
    public partial class MainPage : ContentPage
    {
        OOAdvantech.Synchronization.ReaderWriterLock readerWriterLock = new OOAdvantech.Synchronization.ReaderWriterLock();

        bool OnScan;
        public MainPage()
        {

            InitializeComponent();


            //scanPage.ZxingView.OnScanResult += (result) =>
            //  Device.BeginInvokeOnMainThread(async () =>
            //  {

            //      if (OnScan)
            //      {
            //          OnScan = false;
            //          // Stop analysis until we navigate away so we don't keep reading barcodes
            //          scanPage.ZxingView.IsAnalyzing = false;

            //          // Show an alert
            //          //  await DisplayAlert("Scanned Barcode", result.Text, "OK");

            //          // Navigate away
            //          await scanPage.Navigation.PopAsync();

            //      }
            //  });

            DeleteStorageCommand = new Command(() =>
              {

                  var deviceInstantiator = Xamarin.Forms.DependencyService.Get<OOAdvantech.IDeviceInstantiator>();
                  var fileSystem = deviceInstantiator.GetDeviceSpecific(typeof(IFileSystem)) as IFileSystem;
                  fileSystem.DeleteFile(@"\Abstractions.sqlite");

              });

            SQLiteCommand = new Command(() =>
            {

                ObjectStorage objectStorage = OpenStorage();
                OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(objectStorage);


                var OrdersData = (from order in storage.GetObjectCollection<AbstractionsAndPersistency.IOrder>()
                                  from orderDetail in order.OrderDetails
                                  where orderDetail.Price.Product.Name == "sprite"
                                  select new { orderDetail.Name, orderDetail = orderDetail.Fetching(orderDetail.Order) }).ToList();
                int k = 0;
                //LoadStorageWithObjects();

                //var deviceInstantiator = Xamarin.Forms.DependencyService.Get<OOAdvantech.IDeviceInstantiator>();
                //OOAdvantech.RDBMSMetaDataPersistenceRunTime.IDataBaseConnection DBConnaction = deviceInstantiator.GetDeviceSpecific(typeof(OOAdvantech.RDBMSMetaDataPersistenceRunTime.IDataBaseConnection))  as OOAdvantech.RDBMSMetaDataPersistenceRunTime.IDataBaseConnection;
                //DBConnaction.Open();
                //var command = DBConnaction.CreateCommand();

                //command.CommandText = @"CREATE TABLE [T_Client] (
                //                      [ObjectID] nvarchar(36) NOT NULL
                //                    , [ReferenceCount] int NULL
                //                    , [TypeID] int NOT NULL
                //                    , [Name] nvarchar(255) NULL
                //                    , CONSTRAINT [sqlite_autoindex_T_Client_1] PRIMARY KEY ([ObjectID])
                //                    )";

                //command.ExecuteNonQuery();
                //DBConnaction.Close();


            });

            //async
            LoadObjectsCommand = new Command(() =>
            {
                ObjectStorage objectStorage = OpenStorage();
                OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(objectStorage);

                LoadStorageWithObjects();
            });


            this.ButtonClickCommand = new Command(() =>
            {
                //var task = Task.Run(async () =>
                //{
                //    bool read = true;
                //    bool write = true;
                //    readerWriterLock.AcquireReaderLock(1000);
                //    while(read)
                //        await System.Threading.Tasks.Task.Delay(500);
                //    OOAdvantech.Synchronization.LockCookie lockCookie = readerWriterLock.UpgradeToWriterLock(1000);
                //    while (write)
                //        await System.Threading.Tasks.Task.Delay(500);
                //    read = true;
                //    readerWriterLock.DowngradeFromWriterLock(ref lockCookie);
                //    while (read)
                //        await System.Threading.Tasks.Task.Delay(500);


                //});
                ////task.Wait();

                //task = Task.Run(async () =>
                //{
                //    bool before = true;
                //    while (before)
                //        await System.Threading.Tasks.Task.Delay(500);


                //    bool read = true;
                //    bool write = true;
                //    readerWriterLock.AcquireReaderLock(1000);
                //    while (read)
                //        await System.Threading.Tasks.Task.Delay(500);
                //    OOAdvantech.Synchronization.LockCookie lockCookie = readerWriterLock.UpgradeToWriterLock(1000);
                //    while (write)
                //        await System.Threading.Tasks.Task.Delay(500);
                //    read = true;
                //    readerWriterLock.DowngradeFromWriterLock(ref lockCookie);
                //    while (read)
                //        await System.Threading.Tasks.Task.Delay(500);

                //});
                //task.Wait();






                //readerWriterLock.AcquireReaderLock(1000);
                //OOAdvantech.Synchronization.LockCookie lockCookie = readerWriterLock.UpgradeToWriterLock(1000);
                //readerWriterLock.DowngradeFromWriterLock(ref lockCookie);
                //readerWriterLock.ReleaseReaderLock();


                string rtr = typeof(OOAdvantech.DotNetMetaDataRepository.Assembly).GetTypeInfo().Assembly.FullName;
               // string rtra = typeof(OOAdvantech.PersistenceLayerRunTime.IObjectStorage).GetTypeInfo().Assembly.FullName;


                ///var assembly=  System.Reflection.Assembly.Load(new AssemblyName("MetaDataLoadingSystem, Version = 1.0.0.0, Culture = neutral, PublicKeyToken = 49f7113354405e7f"));
                try
                {


                    bool isGenericType = typeof(System.Collections.Generic.List<>).GetTypeInfo().IsGenericType;
                    bool isGenericTypeDefinition = typeof(System.Collections.Generic.List<>).GetTypeInfo().IsGenericTypeDefinition;

                    bool isGenericTypeA = typeof(System.Collections.Generic.List<object>).GetTypeInfo().IsGenericType;
                    bool isGenericTypeDefinitionA = typeof(System.Collections.Generic.List<object>).GetTypeInfo().IsGenericTypeDefinition;

                    ObjectStorage objectStorage = OpenStorage();
                    OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(objectStorage);


                    var dynResults = from client in storage.GetObjectCollection<AbstractionsAndPersistency.Client>()
                                     select new
                                     {
                                         client,
                                         clientName = client.Name,
                                         orders = from order in client.Orders
                                                  select new
                                                  {
                                                      orderName = order.Name,
                                                      order
                                                  }
                                     };

                    var dynOrdesInfos = from dynClient in dynResults
                                        from dynOrder in dynClient.orders
                                        group dynOrder by new { dynOrder.order.Client, dynOrder.order.OrderDate.Value.Year } into dynSaleGroup
                                        select new { dynSaleGroup.Key, dynSaleGroup.Key.Client.Name, dynSaleGroup };


                    var dynss = from rema in dynOrdesInfos
                                select new
                                {
                                    rema,
                                    count = rema.dynSaleGroup.Count(),
                                    clientName = rema.Key.Client.Name,
                                    orderDetails = from order in rema.dynSaleGroup
                                                   from orderDetail in order.order.OrderDetails
                                                   where orderDetail.Name.Like("Sprite_1*")
                                                   select new { orderDetail, orderDetail.Name }
                                };

                    foreach (var salesInfo in dynss)
                    {
                        int len = salesInfo.orderDetails.ToArray().Length;

                        foreach (var masa in salesInfo.rema.dynSaleGroup)
                        {

                        }
                    }



                    //LoadStorageWithObjects();

                    //var order = (from theOrder in storage.GetObjectCollection<AbstractionsAndPersistency.Order>()
                    //              select theOrder).FirstOrDefault();





                    //var orderDetails = order.OrderDetails;
                    //using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                    //{

                    //    AbstractionsAndPersistency.Client Client = new AbstractionsAndPersistency.Client("Mitsod");
                    //    objectStorage.CommitTransientObjectState(Client);
                    //    stateTransition.Consistent = true;
                    //}
                }
                catch (Exception error)
                {


                }


            });


            BarcodeScanCommand = new Command(async () =>
            {

                OnScan = true;

               // await(App.Current.MainPage as NavigationPage).CurrentPage.Navigation.PushAsync(scanPage);

                //// Create our custom overlay
                //var customOverlay = new StackLayout
                //{
                //    HorizontalOptions = LayoutOptions.FillAndExpand,
                //    VerticalOptions = LayoutOptions.FillAndExpand
                //};
                //var torch = new Button
                //{
                //    Text = "Toggle Torch"
                //};
                //torch.Clicked += delegate
                //{
                //    scanPage.ToggleTorch();
                //};
                //customOverlay.Children.Add(torch);

                //scanPage = new ZXingScannerPage(customOverlay: customOverlay);
                //scanPage.OnScanResult += (result) =>
                //{
                //    if (OnScan)
                //    {
                //        OnScan = false;
                //        scanPage.IsScanning = false;

                //        Device.BeginInvokeOnMainThread(() =>
                //        {
                //            // Navigation.PopAsync();
                //            // DisplayAlert("Scanned Barcode", result.Text, "OK");
                //        });
                //    }
                //};
                //OnScan = true;
                //await Navigation.PushAsync(scanPage);

                //var customScanPage = new ScanPage();//  FormsSample.CustomScanPage();
                //await Navigation.PushAsync(customScanPage);
                //scanPage = new ZXingScannerPage();
                //scanPage.StartScanning();
                //scanPage.OnScanResult += (result) =>
                //{
                //    scanPage.IsScanning = false;

                //    Device.BeginInvokeOnMainThread(() =>
                //    {
                //        Navigation.PopAsync();
                //        DisplayAlert("Scanned Barcode", result.Text, "OK");
                //    });
                //};

                //await Navigation.PushAsync(scanPage);
                //scanPage.AutoFocus();
            });
            BindingContext = this;
        }


        //ScanPage scanPage = new ScanPage();//  FormsSample.CustomScanPage()
        public void LoadStorageWithObjects()
        {
            try
            {

                //ObjectStorage abstractionsStorageSession = OpenStorage();

                //ObjectStorage storageSession = ObjectStorage.OpenStorage("Abstractions",
                //    @"localhost\Debug",
                //    "OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");
                //try
                //{
                //    storageSession.StorageMetaData.RegisterComponent(typeof(AbstractionsAndPersistency.IProduct).Assembly.FullName);
                //}
                //catch (System.Exception Errore)
                //{
                //    int sdf = 0;
                //}


                ObjectStorage abstractionsObjectStorage = OpenStorage();


                using (SystemStateTransition sysStateTransition = new SystemStateTransition())
                {

                    System.Type[] parameterTypes = new System.Type[1] { typeof(string) };
                    AbstractionsAndPersistency.IStorePlace spriteStorePlace = abstractionsObjectStorage.NewObject(typeof(AbstractionsAndPersistency.StorePlace), parameterTypes, "sprite") as AbstractionsAndPersistency.IStorePlace;
                    AbstractionsAndPersistency.IProduct sprite = abstractionsObjectStorage.NewObject(typeof(AbstractionsAndPersistency.Product), parameterTypes, "sprite") as AbstractionsAndPersistency.IProduct;
                    sprite.AddStorePlace(spriteStorePlace);
                    AbstractionsAndPersistency.IProductPrice spritePrice = abstractionsObjectStorage.NewObject(typeof(AbstractionsAndPersistency.ProductPrice), parameterTypes, "sprite") as AbstractionsAndPersistency.IProductPrice;
                    spritePrice.Product = sprite;
                    AbstractionsAndPersistency.IPriceList retailPriceList = abstractionsObjectStorage.NewObject(typeof(AbstractionsAndPersistency.PriceList), parameterTypes, "Retail Price List") as AbstractionsAndPersistency.IPriceList;
                    retailPriceList.AddProduct(spritePrice);

                    AbstractionsAndPersistency.IStorePlace cocaStorePlace = abstractionsObjectStorage.NewObject(typeof(AbstractionsAndPersistency.StorePlace), parameterTypes, "coca") as AbstractionsAndPersistency.IStorePlace;
                    AbstractionsAndPersistency.IProduct cocaCola = abstractionsObjectStorage.NewObject(typeof(AbstractionsAndPersistency.MaterialProduct), parameterTypes, "coca cola") as AbstractionsAndPersistency.IProduct;
                    cocaCola.AddStorePlace(cocaStorePlace);
                    AbstractionsAndPersistency.IProductPrice cocaColaPrice = abstractionsObjectStorage.NewObject(typeof(AbstractionsAndPersistency.SubProductPrice), parameterTypes, "coca cola") as AbstractionsAndPersistency.IProductPrice;
                    cocaColaPrice.Product = cocaCola;
                    AbstractionsAndPersistency.IPriceList tradePriceList = abstractionsObjectStorage.NewObject(typeof(AbstractionsAndPersistency.SubPriceList), parameterTypes, "Trade Price List") as AbstractionsAndPersistency.IPriceList;
                    tradePriceList.AddProduct(cocaColaPrice);
                    AbstractionsAndPersistency.IUnitMeasure unit = abstractionsObjectStorage.NewObject<AbstractionsAndPersistency.UnitMeasure>();
                    unit.Name = "Kilos";
                    int orderCount = 40;// 100;
                    while (orderCount > 0)
                    {



                        AbstractionsAndPersistency.Order order = abstractionsObjectStorage.NewObject(typeof(AbstractionsAndPersistency.Order)) as AbstractionsAndPersistency.Order;
                        order.Name = "AK_" + orderCount.ToString();
                        int count = 15;
                        while (count > 0)
                        {

                            AbstractionsAndPersistency.OrderDetail orderDetail = abstractionsObjectStorage.NewObject(typeof(AbstractionsAndPersistency.OrderDetail)) as AbstractionsAndPersistency.OrderDetail;
                            orderDetail.Name = "Sprite_" + count.ToString();
                            orderDetail.Price = spritePrice;
                            orderDetail.Quantity = new AbstractionsAndPersistency.Quantity(count, unit);
                            order.AddItem(orderDetail);

                            //orderDetail = abstractionsStorageSession.NewObject(typeof(AbstractionsAndPersistency.OrderDetail)) as AbstractionsAndPersistency.OrderDetail;
                            //orderDetail.Name = "coca cola_" + count.ToString();
                            //orderDetail.Price = cocaColaPrice;
                            //orderDetail.Quantity = new Quantity(count, null);
                            //order.AddItem(orderDetail);
                            count--;
                        }
                        AbstractionsAndPersistency.IClient client = abstractionsObjectStorage.NewObject(typeof(AbstractionsAndPersistency.Client), parameterTypes, "Liakos_" + orderCount.ToString()) as AbstractionsAndPersistency.IClient;
                        order.Client = client;
                        orderCount--;
                    }





                    sysStateTransition.Consistent = true;
                }


            }
            catch (System.Exception error)
            {
                int erka = 0;
            }

        }


        public async Task ValidateAddress(string addressText)
        {
            var id = System.Threading.Tasks.Task.CurrentId;
        }
        public ObjectStorage OpenStorage()
        {

            ObjectStorage storageSession = null;

            string storageName = "Abstractions";
            string storageLocation = @"\Abstractions.sqlite";
            string storageType = "OOAdvantech.SQLitePersistenceRunTime.StorageProvider";

            //string storageName = "Abstractions";
            //string storageLocation = @"\Abstractions.xml";
            //string storageType = "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider";

            //var deviceInstantiator = Xamarin.Forms.DependencyService.Get<OOAdvantech.IDeviceInstantiator>();
            try
            {
                storageSession = ObjectStorage.OpenStorage(storageName,
                                                            storageLocation,
                                                            storageType);
            }
            catch (OOAdvantech.PersistenceLayer.StorageException Error)
            {

                if (Error.Reason == StorageException.ExceptionReason.StorageDoesnotExist)
                {
                    storageSession = ObjectStorage.NewStorage(storageName,
                                                            storageLocation,
                                                            storageType);
                    // @"xe",
                    //"OOAdvantech.OraclePersistenceRunTime.EmbeddedStorageProvider");
                    //@"localhost",
                    //"OOAdvantech.MySQLPersistenceRunTime.EmbeddedStorageProvider");
                }
                else
                    throw Error;
                try
                {
                    storageSession.StorageMetaData.RegisterComponent(typeof(AbstractionsAndPersistency.IProduct).GetMetaData().Assembly.FullName);
                }
                catch (System.Exception Errore)
                {
                    int sdf = 0;
                }
            }
            catch (System.Exception Error)
            {
                int tt = 0;
            }
            return storageSession;


        }


        public ICommand DeleteStorageCommand { protected set; get; }
        public ICommand ButtonClickCommand { protected set; get; }

        public ICommand LoadObjectsCommand { protected set; get; }

        public ICommand SQLiteCommand { protected set; get; }

        public ICommand BarcodeScanCommand { protected set; get; }

        public string Title
        {
            get
            {
                return "Hello, XAML !!!";
            }
            set
            {

            }
        }
    }


    /// <MetaDataID>{845325c0-555a-474a-8fca-e70aa0725733}</MetaDataID>
    public interface IClassA
    {
        void Foot();
    }

    /// <MetaDataID>{2403d4e8-1d3c-4362-8703-9aa8f34959d1}</MetaDataID>
    public class ClassA
    {
        public virtual void Foo()
        {

        }
    }
    /// <MetaDataID>{96a6b2b7-394d-472c-b9c4-a07a42df4423}</MetaDataID>
    public class ClassB : ClassA, IClassA
    {
        public override void Foo()
        {
            base.Foo();
        }

        public void Foot()
        {
            throw new NotImplementedException();
        }
    }

}
