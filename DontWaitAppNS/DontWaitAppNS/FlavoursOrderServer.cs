using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FlavourBusinessFacade;
using FlavourBusinessFacade.EndUsers;
using OOAdvantech.MetaDataRepository;

using OOAdvantech.Remoting.RestApi;
using RestaurantHallLayoutModel;
using FlavourBusinessFacade.RoomService;
using System.Configuration;

using OOAdvantech.BinaryFormatter;
using System.Drawing;
using OOAdvantech;
using FlavourBusinessManager.RoomService;




#if DeviceDotNet
using Xamarin.Forms;
using Xamarin.Essentials;
using ZXing.Net.Mobile.Forms;
using ZXing;
using ZXing.QrCode;
using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;
#else
using QRCoder;
using System.IO;
using System.Drawing.Imaging;
using System;
#endif

namespace DontWaitApp
{
    /// <MetaDataID>{cab2cac1-0d34-4bcd-b2c4-81e4a9f915c3}</MetaDataID>
    class FlavoursOrderServer : MarshalByRefObject, IFlavoursOrderServer, FlavourBusinessFacade.ViewModel.ILocalization, OOAdvantech.Remoting.IExtMarshalByRefObject, IBoundObject
    {


        /// <MetaDataID>{d87c2614-3408-428f-80d7-3d590f546a27}</MetaDataID>
        public static OOAdvantech.SerializeTaskScheduler SerializeTaskScheduler
        {
            get
            {
#if DeviceDotNet
                return (Application.Current as IAppLifeTime).SerializeTaskScheduler;
#else
                return (System.Windows.Application.Current as IAppLifeTime).SerializeTaskScheduler;

#endif
            }
        }

        /// <MetaDataID>{cc704161-f4c2-454b-9ff6-010d1e190a4b}</MetaDataID>
        public FlavoursOrderServer()
        {



            _EndUser = new FoodServiceClientVM();

            var deviceInstantiator = Xamarin.Forms.DependencyService.Get<OOAdvantech.IDeviceInstantiator>();
            OOAdvantech.IDeviceOOAdvantechCore device = deviceInstantiator.GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;


#if DeviceDotNet
            (Application.Current as IAppLifeTime).ApplicationResuming += ApplicationResuming;
            (Application.Current as IAppLifeTime).ApplicationSleeping += ApplicationSleeping;

            device.MessageReceived += Device_MessageReceived;


            //ScanPage.ZxingView.OnScanResult += (result) =>
            // Device.BeginInvokeOnMainThread(async () =>
            // {
            //     if (OnScan)
            //     {
            //         TaskCompletionSource<bool> connectToServicePointTask = null;
            //         lock (this)
            //         {
            //             connectToServicePointTask = ConnectToServicePointTask;
            //             ConnectToServicePointTask = null;
            //             OnScan = false;
            //         }
            //         // Stop analysis until we navigate away so we don't keep reading barcodes
            //         ScanPage.ZxingView.IsAnalyzing = false;

            //         // Show an alert
            //         // await App.Current.MainPage.DisplayAlert("Scanned Barcode", result.Text, "OK");

            //         // Navigate away
            //         try
            //         {
            //             await ScanPage.Navigation.PopAsync();
            //             var clientSessionData = await GetFoodServiceSession(result.Text);
            //             var foodServiceClientSession = clientSessionData.FoodServiceClientSession;
            //             //if (FoodServiceClientSession != clientSessionData.FoodServiceClientSession)
            //             {
            //                 if (FoodServiceClientSession != null)
            //                 {
            //                     FoodServiceClientSession.MessageReceived -= MessageReceived;
            //                     FoodServiceClientSession.ObjectChangeState -= FoodServiceClientSessionChangeState;
            //                     FoodServiceClientSession.ItemStateChanged -= FoodServiceClientSessionItemStateChanged;
            //                     FoodServiceClientSession.ItemsStateChanged -= FoodServiceClientSession_ItemsStateChanged;
            //                 }
            //                 FoodServiceClientSession = clientSessionData.FoodServiceClientSession;

            //                 SessionID = clientSessionData.FoodServiceClientSession.SessionID;
            //                 ApplicationSettings.Current.LastClientSessionID = SessionID;

            //                 RefreshMessmates();

            //                 foreach (var flavourItem in FoodServiceClientSession.FlavourItems)
            //                     OrderItems[flavourItem.uid] = flavourItem as ItemPreparation;

            //                 foreach (var flavourItem in FoodServiceClientSession.SharedItems)
            //                     OrderItems[flavourItem.uid] = flavourItem as ItemPreparation;


            //                 ClientSessionToken = clientSessionData.Token;
            //                 FoodServiceClientSession.MessageReceived += MessageReceived;
            //                 FoodServiceClientSession.ObjectChangeState += FoodServiceClientSessionChangeState;
            //                 FoodServiceClientSession.ItemStateChanged += FoodServiceClientSessionItemStateChanged;
            //                 FoodServiceClientSession.ItemsStateChanged += FoodServiceClientSession_ItemsStateChanged;

            //             }
            //             var storeRef = FoodServiceClientSession.Menu;

            //             MenuData menuData = new MenuData()
            //             {
            //                 MenuName = storeRef.Name,
            //                 MenuRoot = storeRef.StorageUrl.Substring(0, storeRef.StorageUrl.LastIndexOf("/") + 1),
            //                 MenuFile = storeRef.StorageUrl.Substring(storeRef.StorageUrl.LastIndexOf("/") + 1),
            //                 ClientSessionID = FoodServiceClientSession.SessionID,
            //                 ServicePointIdentity = clientSessionData.ServicePointIdentity

            //             };
            //             menuData.OrderItems = OrderItems.Values.ToList();
            //             MenuData = menuData;
            //             ApplicationSettings.Current.LastServicePoinMenuData = menuData;
            //             _ObjectChangeState?.Invoke(this, nameof(MenuData));

            //             connectToServicePointTask.SetResult(true);
            //         }
            //         catch (Exception error)
            //         {
            //             connectToServicePointTask.SetResult(false);

            //         }


            //     }
            // });

            //ScanPage.Disappearing += (object sender, EventArgs e) =>
            // {
            //     OnScan = false;
            //     if (ConnectToServicePointTask != null)
            //         ConnectToServicePointTask.SetResult(false);
            // };
#endif
        }

        /// <MetaDataID>{01f8d08e-6e0b-434c-88f3-7da0722f7af5}</MetaDataID>
        private void ApplicationResuming(object sender, EventArgs e)
        {

            SerializeTaskScheduler.AddTask(async () =>
            {
                int tries = 30;
                while (tries > 0)
                {
                    try
                    {
                        if (this.FoodServiceClientSession != null)
                            this.FoodServiceClientSession.DeviceResume();
                        GetMessages();
                        break;
                    }
                    catch (System.Net.WebException commError)
                    {
                        await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(1));
                    }
                    catch (Exception error)
                    {
                        var er = error;
                        await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(1));
                    }
                }
                return true;
            });



        }

        /// <MetaDataID>{efa7ab05-f66c-42a1-850c-c825f70ce948}</MetaDataID>
        private void ApplicationSleeping(object sender, EventArgs e)
        {
            SerializeTaskScheduler.AddTask(async () =>
            {
                int tries = 30;
                while (tries > 0)
                {
                    try
                    {
                        if (this.FoodServiceClientSession != null)
                            this.FoodServiceClientSession.DeviceSleep();
                        break;
                    }
                    catch (System.Net.WebException commError)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(1));
                    }
                    catch (Exception error)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(1));
                    }
                }
                return true;
            });

        }

        /// <MetaDataID>{9714f9b4-1950-4bd5-9a10-a482c42d355e}</MetaDataID>
        object MessagesLock = new object();

#if DeviceDotNet
        private void Device_MessageReceived(OOAdvantech.IRemoteMessage remoteMessage)
        {
            if (remoteMessage.Data.ContainsKey("MessageID") && FoodServiceClientSession != null)
                MessageReceived(FoodServiceClientSession);
        }
#endif



        public event WebViewLoadedHandle OnWebViewLoaded;

        /// <MetaDataID>{d52da524-96a8-4f0b-ae0f-703be4348ff2}</MetaDataID>
        string lan = OOAdvantech.CultureContext.CurrentNeutralCultureInfo.Name;

        /// <MetaDataID>{0cff47a2-3b96-4019-bfab-e15d448b603f}</MetaDataID>
        public string Language { get { return lan; } }

        public string FontsLink { get; set; } = "https://angularhost.z16.web.core.windows.net/graphicmenusresources/Fonts/Fonts.css";

        /// <MetaDataID>{08ad11d2-20bf-4763-b9ea-733efa47a3e1}</MetaDataID>
        public string Path
        {
            get
            {


                string path = ApplicationSettings.Current.Path;
                if (!string.IsNullOrWhiteSpace(path) && path.Split('/').Length > 0)
                {
                    if (MenuData.ServicePointIdentity != path.Split('/')[0])
                        path = "";
                }

                return path;
            }
            set
            {
                string path = value;
                if (!string.IsNullOrWhiteSpace(path) && path.Split('/').Length > 0)
                {
                    if (MenuData.ServicePointIdentity != path.Split('/')[0])
                        path = "";
                }
                ApplicationSettings.Current.Path = path;

            }
        }




        /// <MetaDataID>{94695399-7052-4246-a471-e57eab60ecf3}</MetaDataID>
        public async void WebViewAttached()
        {

            //if (!string.IsNullOrWhiteSpace(ApplicationSettings.Current.LastServicePointIdentity))
            //{
            //    var clientSessionData = await GetFoodServiceSession(ApplicationSettings.Current.LastServicePointIdentity, false);
            //}

        }
        /// <MetaDataID>{61a707f3-a8c3-4528-8171-0f505f779c28}</MetaDataID>
        public string GetClientSessionQRCode(string color)
        {

            if (ApplicationSettings.Current.LastServicePoinMenuData == null)
                return null;
            string SigBase64 = "";
            string codeValue = "MealInvitation;" + ApplicationSettings.Current.LastServicePoinMenuData.ServicePointIdentity + ";" + ApplicationSettings.Current.LastServicePoinMenuData.ClientSessionID;
#if DeviceDotNet
            var barcodeWriter = new BarcodeWriterGeneric()
            {
                Format = ZXing.BarcodeFormat.QR_CODE,
                Options = new ZXing.Common.EncodingOptions
                {
                    Height = 400,
                    Width = 400
                }
            };


            var bitmapMatrix = barcodeWriter.Encode(codeValue);
            var width = bitmapMatrix.Width;
            var height = bitmapMatrix.Height;
            int[] pixelsImage = new int[width * height];
            SkiaSharp.SKBitmap qrCodeImage = new SkiaSharp.SKBitmap(width, height);

            SkiaSharp.SKColor fgColor = SkiaSharp.SKColors.Black;
            if (!SkiaSharp.SKColor.TryParse(color, out fgColor))
                fgColor = SkiaSharp.SKColors.Black;

            var pixels = qrCodeImage.Pixels;
            int k = 0;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (bitmapMatrix[j, i])
                        pixels[k++] = fgColor;
                    else
                        pixels[k++] = SkiaSharp.SKColors.White;
                }
            }
            qrCodeImage.Pixels = pixels;

            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                SkiaSharp.SKData d = SkiaSharp.SKImage.FromBitmap(qrCodeImage).Encode(SkiaSharp.SKEncodedImageFormat.Png, 100);
                d.SaveTo(ms);
                byte[] byteImage = ms.ToArray();
                SigBase64 = @"data:image/png;base64," + Convert.ToBase64String(byteImage);
            }



#else
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(codeValue, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            var qrCodeImage = qrCode.GetGraphic(20, color, "#FFFFFF", true);

            using (System.IO.MemoryStream ms = new MemoryStream())
            {
                qrCodeImage.Save(ms, ImageFormat.Png);
                byte[] byteImage = ms.ToArray();
                SigBase64 = @"data:image/png;base64," + Convert.ToBase64String(byteImage);
            }
#endif

            return SigBase64;
            //return @"data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAABGdBTUEAALGPC/xhBQAAAAlwSFlzAAAA0QAAANEBqyQtcAAAABp0RVh0U29mdHdhcmUAUGFpbnQuTkVUIHYzLjUuMTAw9HKhAAAAhUlEQVQ4T+2MsQ2AMAwEEQVVFmCjVAzBFMxGhcQ2bEBlHL9DCDwLIIqT7Pf7GhHhjH1QZifQjkJDf14VcdJMJY/AinjYlc1JM5VUixXK86AsTpqp5P0ZGQSYqeQqiF7AM7IiwJ4lMWdXQat0546sFiDrrOt7OTCY4AYNT37BRwSTwW6GNAdnJbxPs8oKKwAAAABJRU5ErkJggg==";

        }

        /// <MetaDataID>{ebf9e3ce-2957-4519-82cf-8aa08b910d88}</MetaDataID>
        public void WebViewLoaded()
        {
            OnWebViewLoaded?.Invoke();
            GetMessages();
        }

        /// <MetaDataID>{5f5cd24b-0745-4fc5-b8b1-bab7378d1868}</MetaDataID>
        string _Identity;
        /// <MetaDataID>{a91d4215-30da-4406-9b28-4d6ef692ff85}</MetaDataID>
        public string Identity
        {
            get
            {
                return _Identity;
            }

            set
            {
                _Identity = value;
            }
        }
        /// <MetaDataID>{3786f954-db74-4151-af46-e70e04c9c3c8}</MetaDataID>
        string _Name;
        /// <MetaDataID>{303dedc3-0c36-40b0-aab6-c5ee74dee937}</MetaDataID>
        public string Name
        {
            get
            {
                return _Name;
            }

            set
            {
                _Name = value;
            }
        }
        /// <MetaDataID>{f3f6b6b1-bc55-4fa7-9fd9-f17fbcd7b9e8}</MetaDataID>
        int count = 0;

        /// <MetaDataID>{8854bfe2-67ed-42ca-964b-f4b37a1493c1}</MetaDataID>
        string _Trademark;
        /// <MetaDataID>{65f97bd7-2d60-4296-8453-f92181151fd2}</MetaDataID>
        public string Trademark
        {
            get
            {
                count++;
                return _Trademark + count.ToString();
            }
            set
            {
                _Trademark = value;
            }
        }

        /// <exclude>Excluded</exclude>
        FlavourBusinessFacade.ViewModel.IUser _EndUser;
        /// <MetaDataID>{ec560ede-ff68-4507-993e-b06ec3c912ed}</MetaDataID>
        public FlavourBusinessFacade.ViewModel.IUser EndUser
        {
            get
            {
                return _EndUser;
            }
            set
            {
                _EndUser = value;
            }
        }

        /// <MetaDataID>{1756d916-aa99-42fa-83f8-b0d19447c13a}</MetaDataID>
        public string ISOCurrencySymbol
        {
            get
            {
                return System.Globalization.RegionInfo.CurrentRegion.ISOCurrencySymbol;
            }
        }

        /// <MetaDataID>{9da82eed-28ed-4ce5-8ed0-5f28130b64df}</MetaDataID>
        public bool WaiterView { get; set; }

        /// <MetaDataID>{63add039-3bee-4d8b-8d15-002e6e67aa63}</MetaDataID>
        public IList<ItemPreparation> PreparationItems => OrderItems.ToList();

        /// <MetaDataID>{dafbd4cf-83a8-4890-9196-10a75bfd3529}</MetaDataID>
        public string Foo()
        {

            //MessmateAdded?.Invoke(this, "Liakos");

            //try
            //{

            //    string assemblyData = "FlavourBusinessManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
            //    string type = "FlavourBusinessManager.FlavoursServicesContextManagment";// typeof(FlavourBusinessManager.AuthFlavourBusiness).FullName;
            //    string serverUrl = "http://localhost/FlavourBusinessWebApiRole/api/";
            //    serverUrl = "http://localhost:8090/api/"; //"http://192.168.2.7:8090/api/"
            //    IFlavoursServicesContextManagment pAuthFlavourBusines = OOAdvantech.Remoting.RestApi.RemotingServices.CastTransparentProxy<IFlavoursServicesContextManagment>(OOAdvantech.Remoting.RestApi.RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData));
            //    var storeRef = pAuthFlavourBusines.GetMenu("sd");

            //    pAuthFlavourBusines = OOAdvantech.Remoting.RestApi.RemotingServices.CastTransparentProxy<IFlavoursServicesContextManagment>(OOAdvantech.Remoting.RestApi.RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData));
            //    storeRef = pAuthFlavourBusines.GetMenu("sd");


            //}
            //catch (Exception error)
            //{

            //}
            return "Liakos";
        }
        //static string AzureServerUrl = "http://localhost:8090/api/";
        //static string AzureServerUrl = "http://192.168.2.5:8090/api/";
        //static string AzureServerUrl = "http://192.168.2.8:8090/api/";
        /// <MetaDataID>{ce3e3555-c6b1-4fa2-ab40-abac321bce3d}</MetaDataID>
        //static string AzureServerUrl = "http://192.168.2.8:8090/api/";



        //static string AzureServerUrl = "http://192.168.2.8:8090/api/";//org
        //static string AzureServerUrl = "http://192.168.2.4:8090/api/";//Braxati
        //static string AzureServerUrl = "http://10.0.0.13:8090/api/";//work
        static string AzureServerUrl = string.Format("http://{0}:8090/api/", FlavourBusinessFacade.ComputingResources.EndPoint.Server);



#if DeviceDotNet
        public DeviceUtilities.NetStandard.ScanCode ScanCode = new DeviceUtilities.NetStandard.ScanCode();
#endif
        /// <MetaDataID>{c6516b08-c06b-457c-bf15-f69621afd9f8}</MetaDataID>
        //TaskCompletionSource<bool> ConnectToServicePointTask;
        //var taskCompletionSource = new TaskCompletionSource<ResponseData>();
        /// <MetaDataID>{659129a5-1d1a-4845-97be-6d2ceb6c84d8}</MetaDataID>
        //bool OnScan = false;
        /// <MetaDataID>{47842b22-c95e-4a09-8f5b-a1eaaba8b014}</MetaDataID>
        public Task<bool> ConnectToServicePoint(string servicePointIdentity = "")
        {
#if IOSEmulator
              return Task<MenuData>.Run(async () =>
           {
               var foodServiceClientSession = await GetFoodServiceSession("");

               if (FoodServiceClientSession != foodServiceClientSession)
               {
                   if (FoodServiceClientSession != null)
                       FoodServiceClientSession.MessageReceived -= MessageReceived;
                   FoodServiceClientSession = foodServiceClientSession;
                   FoodServiceClientSession.MessageReceived += MessageReceived;
               }
               var storeRef = FoodServiceClientSession.Menu;
               MenuData menuData = new MenuData() { MenuName = storeRef.Name, MenuRoot = storeRef.StorageUrl.Substring(0, storeRef.StorageUrl.LastIndexOf("/") + 1), MenuFile = storeRef.StorageUrl.Substring(storeRef.StorageUrl.LastIndexOf("/") + 1) };
               menuData.OrderItems = OrderItems.Values.ToList();
               return menuData;
           });
#else

#if DeviceDotNet2
            lock (this)
            {
                if (OnScan && ConnectToServicePointTask != null)
                    return ConnectToServicePointTask.Task;

                OnScan = true;
                ConnectToServicePointTask = new TaskCompletionSource<bool>();
            }
            Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
            {
                await (App.Current.MainPage as NavigationPage).CurrentPage.Navigation.PushAsync(ScanPage);
            });
            return ConnectToServicePointTask.Task;
#else
            return Task<MenuData>.Run(async () =>
            {
                //var result = await ScanCode.Scan("Hold your phone up to the place Identity", "Scanning will happen automatically");
                var clientSessionData = await GetFoodServiceSession(servicePointIdentity);

                //if (FoodServiceClientSession != clientSessionData.FoodServiceClientSession)
                {
                    if (FoodServiceClientSession != null)
                    {
                        FoodServiceClientSession.MessageReceived -= MessageReceived;
                        FoodServiceClientSession.ObjectChangeState -= FoodServiceClientSessionChangeState;
                        FoodServiceClientSession.ItemStateChanged -= FoodServiceClientSessionItemStateChanged;
                        FoodServiceClientSession.ItemsStateChanged -= FoodServiceClientSession_ItemsStateChanged;
                    }
                    FoodServiceClientSession = clientSessionData.FoodServiceClientSession;



                    SessionID = clientSessionData.FoodServiceClientSession.SessionID;
                    ApplicationSettings.Current.LastClientSessionID = SessionID;

                    RefreshMessmates();

                    foreach (var flavourItem in FoodServiceClientSession.FlavourItems)
                    {
                        var cachedOrderItem = OrderItems.Where(x => x.uid == flavourItem.uid).FirstOrDefault();
                        if (cachedOrderItem != null)
                            OrderItems.Remove(cachedOrderItem);
                        OrderItems.Add(flavourItem as ItemPreparation);
                    }

                    foreach (var flavourItem in FoodServiceClientSession.SharedItems)
                    {
                        var cachedOrderItem = OrderItems.Where(x => x.uid == flavourItem.uid).FirstOrDefault();
                        if (cachedOrderItem != null)
                            OrderItems.Remove(cachedOrderItem);
                        OrderItems.Add(flavourItem as ItemPreparation);
                    }

                    ClientSessionToken = clientSessionData.Token;
                    FoodServiceClientSession.MessageReceived += MessageReceived;
                    FoodServiceClientSession.ObjectChangeState += FoodServiceClientSessionChangeState;
                    FoodServiceClientSession.ItemStateChanged += FoodServiceClientSessionItemStateChanged;
                    FoodServiceClientSession.ItemsStateChanged += FoodServiceClientSession_ItemsStateChanged;

                }
                var storeRef = FoodServiceClientSession.Menu;
#if !DeviceDotNet
                storeRef.StorageUrl = "https://dev-localhost/" + storeRef.StorageUrl.Substring(storeRef.StorageUrl.IndexOf("devstoreaccount1"));
#endif


                MenuData menuData = new MenuData()
                {
                    MenuName = storeRef.Name,
                    MenuRoot = storeRef.StorageUrl.Substring(0, storeRef.StorageUrl.LastIndexOf("/") + 1),
                    MenuFile = storeRef.StorageUrl.Substring(storeRef.StorageUrl.LastIndexOf("/") + 1),
                    ClientSessionID = FoodServiceClientSession.SessionID,
                    ServicePointIdentity = clientSessionData.ServicePointIdentity,
                    ServicesPointName = clientSessionData.ServicesPointName,
                    ServicesContextLogo = clientSessionData.ServicesContextLogo,
                    DefaultMealTypeUri = clientSessionData.DefaultMealTypeUri,
                    ServedMealTypesUris = clientSessionData.ServedMealTypesUris

                };
                menuData.OrderItems = OrderItems.ToList();
                MenuData = menuData;
                ApplicationSettings.Current.LastServicePoinMenuData = menuData;
                _ObjectChangeState?.Invoke(this, nameof(MenuData));

                return true;
            });
#endif
#endif
        }


        //public Task<MenuData> GetServicePointData(string servicePointIdentity)


        /// <MetaDataID>{73f366a8-2d78-456f-aacd-b70114704a4f}</MetaDataID>
        public MenuData MenuData { get => ApplicationSettings.Current.LastServicePoinMenuData; set => ApplicationSettings.Current.LastServicePoinMenuData = value; }

        /// <MetaDataID>{b2483c93-a9f2-41f9-b223-ea85797d1490}</MetaDataID>
        object ClientSessionLock = new object();

        /// <MetaDataID>{1507e7fe-4972-41be-a996-b8e3ccee29d3}</MetaDataID>
        Dictionary<string, Task<bool>> GetServicePointDataTasks = new Dictionary<string, Task<bool>>();

        /// <MetaDataID>{69e24f40-d506-4e2e-b839-9d288ab735f1}</MetaDataID>
        public Task<bool> IsSessionActive()
        {
            return Task<bool>.Run(async () =>
            {
                if (FoodServiceClientSession != null)
                    return true;
                else
                {
                    if (!string.IsNullOrWhiteSpace(ApplicationSettings.Current.LastServicePoinMenuData.ServicePointIdentity))
                    {

                        while (!await GetServicePointData(MenuData.ServicePointIdentity))
                        {
                            if (string.IsNullOrWhiteSpace(Path) || Path.Split('/')[0] != MenuData.ServicePointIdentity)
                                break;
                        }

                        if (FoodServiceClientSession != null)
                            return true;
                        else
                        {
                            ApplicationSettings.Current.LastServicePoinMenuData = new MenuData() { OrderItems=new List<ItemPreparation>()};
                            ApplicationSettings.Current.LastClientSessionID = "";
                            Path = "";

                            return false;
                        }
                    }
                    return false;
                }
            });
        }
        /// <summary>
        /// this method gets data from service point for client and synchronize caching data 
        /// </summary>
        /// <param name="servicePointIdentity"></param>
        /// <returns>
        /// true when device connected to server successfully 
        /// otherwise return false
        /// </returns>
        /// <MetaDataID>{f877c152-061c-4142-aed6-50cc431a1a05}</MetaDataID>
        public Task<bool> GetServicePointData(string servicePointIdentity)
        {
            lock (ClientSessionLock)
            {
                Task<bool> getServicePointDataTask = null;
                GetServicePointDataTasks.TryGetValue(servicePointIdentity, out getServicePointDataTask);
                if (getServicePointDataTask != null && !getServicePointDataTask.IsCompleted)
                    return getServicePointDataTask; // returns the active task to get service point data

                //There isn't active task.
                //Starts task to get service point data
                getServicePointDataTask = Task<bool>.Run(async () =>
                {

                    try
                    {
                        DateTime start = DateTime.UtcNow;
                        var clientSessionData = await GetFoodServiceSession(servicePointIdentity, false);
                        if (FoodServiceClientSession != null)
                        {
                            FoodServiceClientSession.MessageReceived -= MessageReceived;
                            FoodServiceClientSession.ObjectChangeState -= FoodServiceClientSessionChangeState;
                            FoodServiceClientSession.ItemStateChanged -= FoodServiceClientSessionItemStateChanged;
                            FoodServiceClientSession.ItemsStateChanged -= FoodServiceClientSession_ItemsStateChanged;
                        }
                        FoodServiceClientSession = clientSessionData.FoodServiceClientSession;
                        if (FoodServiceClientSession == null)
                        {
                            //There isn't active session for service point and client
                            //Clear cache  the last session has ended
                            OrderItems.Clear();
                            this.MenuData = new MenuData();
                            return true;
                        }

                        SessionID = clientSessionData.FoodServiceClientSession.SessionID;
                        if (ApplicationSettings.Current.LastClientSessionID != SessionID)
                            OrderItems.Clear(); //Clear cache for new session

                        

                        ApplicationSettings.Current.LastClientSessionID = SessionID;


                        #region synchronize cached order items
                        foreach (var flavourItem in FoodServiceClientSession.FlavourItems)
                        {
                            var cachedOrderItem = OrderItems.Where(x => x.uid == flavourItem.uid).FirstOrDefault();
                            if (cachedOrderItem != null)
                                OrderItems.Remove(cachedOrderItem);
                            OrderItems.Add(flavourItem as ItemPreparation);
                        }
                        var cou = FoodServiceClientSession.FlavourItems.Count;
                        foreach (var orderItem in OrderItems.Where(x=>x.SessionID== SessionID))
                        {
                            var flavourItem = FoodServiceClientSession.FlavourItems.Where(x => x.uid == orderItem.uid).FirstOrDefault();
                            if (flavourItem == null)
                                FoodServiceClientSession.AddItem(orderItem);
                        }
                        
                        cou = FoodServiceClientSession.FlavourItems.Count;

                        foreach (var flavourItem in FoodServiceClientSession.SharedItems)
                        {
                            var cachedOrderItem = OrderItems.Where(x => x.uid == flavourItem.uid).FirstOrDefault();
                            if (cachedOrderItem != null)
                                OrderItems.Remove(cachedOrderItem);
                            OrderItems.Add(flavourItem as ItemPreparation);
                        }
                        #endregion

                        RefreshMessmates();

                        ClientSessionToken = clientSessionData.Token;
                        FoodServiceClientSession.MessageReceived += MessageReceived;
                        FoodServiceClientSession.ObjectChangeState += FoodServiceClientSessionChangeState;
                        FoodServiceClientSession.ItemStateChanged += FoodServiceClientSessionItemStateChanged;
                        FoodServiceClientSession.ItemsStateChanged += FoodServiceClientSession_ItemsStateChanged;


                        var storeRef = FoodServiceClientSession.Menu;
#if !DeviceDotNet
                        storeRef.StorageUrl = "https://dev-localhost/" + storeRef.StorageUrl.Substring(storeRef.StorageUrl.IndexOf("devstoreaccount1"));
#endif

                        MenuData menuData = new MenuData()
                        {
                            MenuName = storeRef.Name,
                            MenuRoot = storeRef.StorageUrl.Substring(0, storeRef.StorageUrl.LastIndexOf("/") + 1),
                            MenuFile = storeRef.StorageUrl.Substring(storeRef.StorageUrl.LastIndexOf("/") + 1),
                            ClientSessionID = FoodServiceClientSession.SessionID,
                            ServicePointIdentity = clientSessionData.ServicePointIdentity,
                            DefaultMealTypeUri=clientSessionData.DefaultMealTypeUri,
                            ServedMealTypesUris = clientSessionData.ServedMealTypesUris

                        };
                        menuData.OrderItems = OrderItems.ToList();
                        //var s = menuData.OrderItems[0].OptionsChanges[0].ItemPreparation;
                        MenuData = menuData;
                        _ObjectChangeState?.Invoke(this, nameof(MenuData));

                        GetMessages();
                    }
                    catch (Exception error)
                    {

                        return false;
                    }
                    return true;

                });
                GetServicePointDataTasks[servicePointIdentity] = getServicePointDataTask;
                return getServicePointDataTask;
            }


        }

        /// <MetaDataID>{6133970c-efe1-4c70-a5c0-2162b7b2dda9}</MetaDataID>
        Message ActivePartOfMealMessage;
        /// <MetaDataID>{ca16b127-1d5f-46e5-aac6-633a14ae9794}</MetaDataID>
        private void GetMessages()
        {
            lock (MessagesLock)
            {
                if (FoodServiceClientSession != null)
                {
                    var message = FoodServiceClientSession.PeekMessage();

                    if (message != null && message.GetDataValue<ClientMessages>("ClientMessageType") == ClientMessages.PartOfMealRequest)
                    {
                        PartOfMealRequestMessageForward(message);
                        return;
                    }

                    if (message != null && message.GetDataValue<ClientMessages>("ClientMessageType") == ClientMessages.MenuItemProposal)
                    {
                        MenuItemProposalMessageForword(message);
                        return;
                    }

                    if (message != null && message.GetDataValue<ClientMessages>("ClientMessageType") == ClientMessages.ShareItemHasChange)
                    {
                        ShareItemHasChangeMessageForward(message);
                        //string uid = message.GetDataValue<string>("SharedItemUid");
                        //clientMessage.Data["SharedItemUid"] = uid;
                        //clientMessage.Data["ItemOwningSession"] = isShared;
                        //clientMessage.Data["ShareInSessions"] = shareInSessions;
                    }
                    if (message != null && message.GetDataValue<ClientMessages>("ClientMessageType") == ClientMessages.YouMustDecide)
                    {
                        YouMustDecideMessageForward(message);
                    }


                }

            }
        }

        /// <MetaDataID>{4a4ab174-50dd-4220-88c1-cb6e1d925628}</MetaDataID>
        private void YouMustDecideMessageForward(Message message)
        {
            FoodServiceClientSession.RemoveMessage(message.MessageID);
            Task.Run(() =>
            {
                try
                {
                    var messmates = (from clientSession in FoodServiceClientSession.GetMealParticipants()
                                     select new Messmate(clientSession, OrderItems)).ToList();


                    messmates = (from messmate in messmates
                                 from preparationItem in messmate.PreparationItems
                                 where preparationItem.State == ItemPreparationState.Committed
                                 select messmate).ToList();

                    _MessmatesWaitForYouToDecide?.Invoke(this, messmates, message.MessageID);
                }
                catch (Exception error)
                {
                }
            });
        }

        /// <MetaDataID>{24360209-3b94-4a93-8d33-364f5c406bae}</MetaDataID>
        private void ShareItemHasChangeMessageForward(Message message)
        {
            string itemUid = message.GetDataValue<string>("SharedItemUid");
            string itemOwningSession = message.GetDataValue<string>("ItemOwningSession");
            string itemChangeSession = message.GetDataValue<string>("itemChangeSession");
            Messmate changeItemMessmate = Messmates.Where(x => x.ClientSessionID == itemChangeSession).FirstOrDefault();


            bool isShared = message.GetDataValue<bool>("IsShared");
            List<string> shareInSessions = message.GetDataValue<List<string>>("ShareInSessions");
            FoodServiceClientSessionItemStateChanged(itemUid, itemOwningSession, isShared, shareInSessions);
            _SharedItemChanged?.Invoke(this, changeItemMessmate, itemUid, message.MessageID);
            FoodServiceClientSession.RemoveMessage(message.MessageID);
        }

        /// <MetaDataID>{1ad75d89-e702-4d8e-8577-316b6b2a7977}</MetaDataID>
        private void MenuItemProposalMessageForword(Message message)
        {
            if (_MenuItemProposal != null)
            {
                if (MessmatesLoaded)
                {
                    var messmate = (from theMessmate in this.Messmates
                                    where theMessmate.ClientSessionID == message.GetDataValue("ClientSessionID") as string
                                    select theMessmate).FirstOrDefault();
                    if (messmate == null)
                    {
                        messmate = (from theMessmate in this.Messmates
                                    where theMessmate.ClientSessionID == message.GetDataValue("ClientSessionID") as string
                                    select theMessmate).FirstOrDefault();
                        if (messmate != null)
                            FoodServiceClientSession.RemoveMessage(message.MessageID);
                        return;
                    }
                    string menuItemUri = message.GetDataValue<string>("MenuItemUri");

                    _MenuItemProposal?.Invoke(this, messmate, menuItemUri, message.MessageID);
                }
            }
            else
                MenuItemProposalMessage = message;
        }

        /// <MetaDataID>{61e17863-b7ff-4963-9d5b-12ab551a9369}</MetaDataID>
        private void PartOfMealRequestMessageForward(Message message)
        {
            if (_PartOfMealRequest != null)
            {
                if (ActivePartOfMealMessage != null && ActivePartOfMealMessage.MessageID == message.MessageID)
                    return;
                if (MessmatesLoaded)
                {
                    ActivePartOfMealMessage = message;
                    var messmate = (from theMessmate in this.CandidateMessmates
                                    where theMessmate.ClientSessionID == message.GetDataValue("ClientSessionID") as string
                                    select theMessmate).FirstOrDefault();

                    if (messmate == null)
                    {
                        var candidateMessmates = (from clientSession in FoodServiceClientSession.GetPeopleNearMe()
                                                  select new Messmate(clientSession, OrderItems)).ToList();
                        this.CandidateMessmates = candidateMessmates;
                        messmate = (from theMessmate in this.CandidateMessmates
                                    where theMessmate.ClientSessionID == message.GetDataValue("ClientSessionID") as string
                                    select theMessmate).FirstOrDefault();

                    }
                    if (messmate == null)
                    {
                        messmate = (from theMessmate in this.Messmates
                                    where theMessmate.ClientSessionID == message.GetDataValue("ClientSessionID") as string
                                    select theMessmate).FirstOrDefault();
                        if (messmate != null)
                        {
                            FoodServiceClientSession.RemoveMessage(message.MessageID);
                            return;
                        }
                    }
                    _PartOfMealRequest?.Invoke(this, messmate, message.MessageID);

#if DeviceDotNet

                    var deviceInstantiator = Xamarin.Forms.DependencyService.Get<OOAdvantech.IDeviceInstantiator>();
                    OOAdvantech.IDeviceOOAdvantechCore device = deviceInstantiator.GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
                    if (device.IsinSleepMode)
                    {
                        var t = Task.Run(async delegate
                        {
                            while (device.IsinSleepMode)
                            {
                                await Task.Delay(2000);
                                if (!device.IsinSleepMode)
                                    break;

                                device.PlaySound();
                                await Task.Delay(2000);
                                if (!device.IsinSleepMode)
                                    break;

                                var duration = TimeSpan.FromSeconds(1);
                                Vibration.Vibrate(duration);

                            }

                        });
                    }
#endif
                }
            }
            else
                PartOfMealMessage = message;

            return;
        }

        /// <MetaDataID>{2784806b-281e-44df-af8d-dd847bb7c8a9}</MetaDataID>
        private void FoodServiceClientSession_ItemsStateChanged(Dictionary<string, ItemPreparationState> newItemsState)
        {

            foreach (var entry in newItemsState)
            {
                foreach (var preparationItem in (from mesmate in Messmates
                                                 from preparationItem in mesmate.PreparationItems
                                                 where preparationItem.uid == entry.Key
                                                 select preparationItem))
                {
                    preparationItem.State = entry.Value;
                }
                foreach (var preparationItem in (
                                                 from preparationItem in PreparationItems
                                                 where preparationItem.uid == entry.Key
                                                 select preparationItem))
                {
                    preparationItem.State = entry.Value;
                }

            }
            _ObjectChangeState?.Invoke(this, null);
        }

        /// <MetaDataID>{eaba9b2e-24d1-40c9-a56c-245a47e9c3cb}</MetaDataID>
        private void FoodServiceClientSessionItemStateChanged(string itemUid, string itemOwnerSession, bool isShared, List<string> shareInSessions)
        {
            #region the item removed and isn't shared

            if (shareInSessions.Count == 0)
            {
                foreach (var messmate in Messmates)
                {
                    //removes item from messmates sharing options
                    var shareItem = messmate.PreparationItems.Where(x => x.uid == itemUid).FirstOrDefault();
                    if (shareItem != null)
                        messmate.RemovePreparationItem(shareItem);
                }
            }
            #endregion

            #region the item state changed
            if (!string.IsNullOrWhiteSpace(itemOwnerSession) && shareInSessions.Count > 0)
            {
                ItemPreparation preparationItem = GetItemFromOwner(itemUid, itemOwnerSession);
                if (preparationItem == null)
                    throw new NullReferenceException("There isn't item to update");
                //updates order items





                if (shareInSessions.Contains(FoodServiceClientSession.SessionID))
                {
                    var orderItem = OrderItems.Where(x => x.uid == itemUid).FirstOrDefault();
                    if (orderItem != null)
                        OrderItems.Remove(orderItem);
                    OrderItems.Add(preparationItem);
                }

                //Updates the messmates which have sessionID among the item shareInSessions.
                foreach (var messmate in (from theMessmate in this.Messmates
                                          where shareInSessions.Contains(theMessmate.ClientSessionID)
                                          select theMessmate))
                {

                    var oldPreparationItem = messmate.PreparationItems.Where(x => x.uid == itemUid).FirstOrDefault();
                    if (oldPreparationItem == null)
                        messmate.AddPreparationItem(preparationItem);
                    else
                    {
                        if (oldPreparationItem.SessionID == preparationItem.SessionID)
                            oldPreparationItem.Update(preparationItem);
                        else
                        {
                            messmate.RemovePreparationItem(oldPreparationItem);
                            messmate.AddPreparationItem(preparationItem);
                        }
                    }
                }



            }

            //Removes this item from the messmates which had the preparation item and its session isn't among item shareInSessions.

            foreach (var messmate in (from theMessmate in this.Messmates
                                      where theMessmate.HasItemWithUid(itemUid) && !shareInSessions.Contains(theMessmate.ClientSessionID)
                                      select theMessmate))
            {
                messmate.RemovePreparationItem(messmate.PreparationItems.Where(x => x.uid == itemUid).First());
            }
            #endregion

            var menuData = this.MenuData;
            menuData.OrderItems = OrderItems.ToList();
            MenuData = menuData;

            _ObjectChangeState?.Invoke(this, null);



        }

        /// <MetaDataID>{6a9b4e5b-be1a-40c7-bc79-3d3a86e8ad87}</MetaDataID>
        private ItemPreparation GetItemFromOwner(string itemUid, string itemOwnerSession)
        {

            if (FoodServiceClientSession.SessionID == itemOwnerSession)
                return FoodServiceClientSession.GetItem(itemUid) as ItemPreparation;
            else
            {

                var messmate = (from theMessmate in this.Messmates
                                where theMessmate.ClientSessionID == itemOwnerSession
                                select theMessmate).FirstOrDefault();
                return messmate.ClientSession.GetItem(itemUid) as ItemPreparation;
            }

        }

        /// <MetaDataID>{d00b0aa2-02b8-4a14-8736-7f83e78c53b9}</MetaDataID>
        private ItemPreparation GetItemFromOwning(string itemUid, string itemOwningSession)
        {
            ItemPreparation preparationItem = null;
            if (!string.IsNullOrWhiteSpace(itemOwningSession))
            {
                if (FoodServiceClientSession.SessionID == itemOwningSession)
                    preparationItem = FoodServiceClientSession.GetItem(itemUid) as ItemPreparation;
                else
                {

                    var messmate = (from theMessmate in this.Messmates
                                    where theMessmate.ClientSessionID == itemOwningSession
                                    select theMessmate).FirstOrDefault();
                    preparationItem = messmate.ClientSession.GetItem(itemUid) as ItemPreparation;
                }
            }

            return preparationItem;
        }

        /// <MetaDataID>{0dce1877-b96f-4558-98b6-66fb704a80f1}</MetaDataID>
        private void FoodServiceClientSessionChangeState(object _object, string member)
        {
            if (member == nameof(IFoodServiceClientSession.FlavourItems))
            {
                OrderItems.Clear();
                //foreach (var flavourItem in FoodServiceClientSession.FlavourItems)
                //    OrderItems[flavourItem.uid] = flavourItem as ItemPreparation;
                //foreach (var flavourItem in FoodServiceClientSession.SharedItems)
                //    OrderItems[flavourItem.uid] = flavourItem as ItemPreparation;


                OrderItems.AddRange(FoodServiceClientSession.FlavourItems.OfType<ItemPreparation>());
                OrderItems.AddRange(FoodServiceClientSession.SharedItems.OfType<ItemPreparation>());

                ///RefreshMessmates();
                //var menuData = MenuData;
                //menuData.OrderItems = OrderItems.Values.ToList();
                //MenuData= menuData;

                var menuData = this.MenuData;
                menuData.OrderItems = OrderItems.ToList();
                MenuData = menuData;

                _ObjectChangeState?.Invoke(this, null);


            }
            else
                RefreshMessmates();



        }

        /// <MetaDataID>{837f1943-f15c-49d8-a50b-2832acab8771}</MetaDataID>
        private void MessageReceived(IMessageConsumer sender)
        {
            GetMessages();
        }


        /// <MetaDataID>{e31b5e23-a2de-4d2e-879b-727f7ff1dc41}</MetaDataID>
        IFoodServiceClientSession _FoodServiceClientSession;

        /// <MetaDataID>{be42123c-b940-44e6-91d1-d2a9e2b2ca7a}</MetaDataID>
        IFoodServiceClientSession FoodServiceClientSession
        {
            get
            {
                return _FoodServiceClientSession;
            }
            set
            {
                if (_FoodServiceClientSession == null && value != null)
                    value.DeviceResume();

                if (_FoodServiceClientSession is ITransparentProxy)
                    (_FoodServiceClientSession as ITransparentProxy).Reconnected -= FlavoursOrderServer_Reconnected;

                _FoodServiceClientSession = value;

                if (_FoodServiceClientSession is ITransparentProxy)
                    (_FoodServiceClientSession as ITransparentProxy).Reconnected += FlavoursOrderServer_Reconnected;

            }
        }


        public string LocalGet()
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                var responseTask = httpClient.GetStringAsync("http://localhost/devstoreaccount1/usersfolder/ykCR5c6aHVUUpGJ8J7ZqpLLY97i1/Menus/0bb39514-b297-436c-8554-c5e5a52486ac/v109/Marzano Phone.json");
                responseTask.Wait();
                string data = responseTask.Result;
                return data;

            }
            catch (Exception error)
            {

                throw;
            }
        }


        public string AppIdentity => throw new NotImplementedException();

        /// <MetaDataID>{c8a403e0-d804-40a6-8543-4bee48d1319f}</MetaDataID>
        private void FlavoursOrderServer_Reconnected(object sender)
        {

            var foodServiceClientSession = RemotingServices.CastTransparentProxy<IFoodServiceClientSession>(sender);
            OrderItems.Clear();

            //foreach (var flavourItem in FoodServiceClientSession.FlavourItems)
            //    OrderItems[flavourItem.uid] = flavourItem as ItemPreparation;

            //foreach (var flavourItem in FoodServiceClientSession.SharedItems)
            //    OrderItems[flavourItem.uid] = flavourItem as ItemPreparation;

            OrderItems.AddRange(FoodServiceClientSession.FlavourItems.OfType<ItemPreparation>());
            OrderItems.AddRange(FoodServiceClientSession.SharedItems.OfType<ItemPreparation>());



            var menuData = this.MenuData;
            menuData.OrderItems = OrderItems.ToList();
            MenuData = menuData;


            //foreach (var messmate in Messmates)
            //    messmate.Refresh(OrderItems);
            _ObjectChangeState?.Invoke(this, null);
        }

        /// <MetaDataID>{2cb552e5-2926-44aa-a051-93f9d81d4045}</MetaDataID>
        string ClientSessionToken;

        /// <MetaDataID>{9e5492a1-32f6-4fb9-929d-f0d7a65fd727}</MetaDataID>
        IList<Messmate> CandidateMessmates = new List<Messmate>();
        /// <MetaDataID>{50126694-0421-469a-980e-029ead5df9e4}</MetaDataID>
        private bool MessmatesLoaded;

        /// <MetaDataID>{7872a80f-383a-4e7e-a7c0-c81c99c9573a}</MetaDataID>
        public IList<Messmate> GetCandidateMessmates()
        {
            if (CandidateMessmates.Count == 0 && Messmates.Count == 0)
                GetMessmatesFromServer().Wait(2);
            return CandidateMessmates;
            //return Task<MenuData>.Run(async () =>
            //{

            //    CandidateMessmates = (from clientSession in FoodServiceClientSession.GetPeopleNearMe()
            //                          select new Messmate(clientSession, OrderItems)).ToList();
            //    return CandidateMessmates;

            //});

        }


        /// <MetaDataID>{d1b44dee-1753-48a2-89ba-fa7df5d31ecc}</MetaDataID>
        IList<Messmate> Messmates = new List<Messmate>();
        /// <MetaDataID>{af6e8091-4784-46b6-82af-70336ac8b8fd}</MetaDataID>
        public IList<Messmate> GetMessmates()
        {
            return Messmates;
            //return Task<IList<Messmate>>.Run(async () =>
            //{
            //    Messmates = (from clientSession in FoodServiceClientSession.GetMealParticipants()
            //                 select new Messmate(clientSession, OrderItems)).ToList();
            //    return Messmates;
            //});

        }

        public void RefreshMessmates()
        {
            GetMessmatesFromServer();
        }
        /// <MetaDataID>{4f182b25-801d-44a9-bb30-ca0320c4c7d5}</MetaDataID>
        public Task GetMessmatesFromServer()
        {
            return Task.Run(() =>
            {
                try
                {
                    if (WaiterView)
                    {

                        var messmates = (from clientSession in FoodServiceClientSession.GetServicePointParticipants()
                                         select new Messmate(clientSession, OrderItems)).ToList();
                        Messmates = messmates;
                        MessmatesLoaded = true;

                    }
                    else
                    {
                        var messmates = (from clientSession in FoodServiceClientSession.GetMealParticipants()
                                         select new Messmate(clientSession, OrderItems)).ToList();
                        var candidateMessmates = (from clientSession in FoodServiceClientSession.GetPeopleNearMe()
                                                  select new Messmate(clientSession, OrderItems)).ToList();
                        Messmates = messmates;
                        CandidateMessmates = candidateMessmates;

                        MessmatesLoaded = true;
                        GetMessages();
                    }
                    _ObjectChangeState?.Invoke(this, null);
                }
                catch (Exception error)
                {
                }
            });
        }

        /// <MetaDataID>{61c4b585-5ae6-4b38-8922-b9e56fe6d335}</MetaDataID>
        public void MealInvitation(Messmate messmate)
        {
            Task.Run(() =>
            {
                var clientSession = (from theMessmate in this.CandidateMessmates
                                     where theMessmate.ClientSessionID == messmate.ClientSessionID
                                     select theMessmate.ClientSession).FirstOrDefault();


                clientSession.MealInvitation(this.FoodServiceClientSession);
            });
        }

        /// <MetaDataID>{15847442-73d8-4818-a521-954119101cb7}</MetaDataID>
        public void CancelMealInvitation(Messmate messmate)
        {
            Task.Run(() =>
            {
                var clientSession = (from theMessmate in this.CandidateMessmates
                                     where theMessmate.ClientSessionID == messmate.ClientSessionID
                                     select theMessmate.ClientSession).FirstOrDefault();

                if (clientSession != null)
                    clientSession.CancelMealInvitation(this.FoodServiceClientSession);
            });
        }

        /// <MetaDataID>{afbf90c0-ee2f-4692-9ef0-8ed3c80f7bd2}</MetaDataID>
        public IUser CurrentUser;

        /// <MetaDataID>{0a4b8da3-005a-4dad-9728-12c5fb1ea1dd}</MetaDataID>
        Task<ClientSessionData> GetFoodServiceSession(string servicePointID, bool create = true)
        {
            return Task<IFoodServiceClientSession>.Run(async () =>
            {
                try
                {
                    MenuData menuData = new MenuData();
                    string servicePoint = "7f9bde62e6da45dc8c5661ee2220a7b0;9967813ee9d943db823ca97779eb9fd7";
                    if (servicePointID.Split(';').Length > 2)
                        servicePoint = servicePointID;

                    //string servicePoint = "ca33b38f5c634fd49c50af60b042f910;8dedb45522ad479480e113c59d4bbdd0";
                    //servicePoint = "7f9bde62e6da45dc8c5661ee2220a7b0;8dedb45522ad479480e113c59d4bbdd0";
                    //// servicePoint = "6746e4178dd041f09a7b4130af0edacf;6171631179bf4c26aeb99546fdce6a7a";
                    //servicePoint = "b5ec4ed264c142adb26b73c95b185544;9967813ee9d943db823ca97779eb9fd7";


                    string assemblyData = "FlavourBusinessManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
                    string type = "FlavourBusinessManager.FlavoursServicesContextManagment";
                    string serverUrl = AzureServerUrl;
                    IFlavoursServicesContextManagment servicesContextManagment = RemotingServices.CastTransparentProxy<IFlavoursServicesContextManagment>(OOAdvantech.Remoting.RestApi.RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData));
                    servicesContextManagment.ObjectChangeState += ServicesContextManagment_ObjectChangeState;


                    OOAdvantech.IDeviceOOAdvantechCore device = Xamarin.Forms.DependencyService.Get<OOAdvantech.IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;

                    var foodServiceClientSession = servicesContextManagment.GetClientSession(servicePoint, await GetFriendlyName(), device.DeviceID, device.FirebaseToken, null, CurrentUser, create);


                    //menuData.MenuRoot = "http://192.168.2.3/devstoreaccount1/usersfolder/ykCR5c6aHVUUpGJ8J7ZqpLLY97i1/Menus/0bb39514-b297-436c-8554-c5e5a52486ac/";
                    //menuData.MenuFile = "Marzano Phone.json";
                    //menuData.MenuName = "Marzano Phone";
                    //menuData.MenuRoot = "http://192.168.2.8/devstoreaccount1/usersfolder/ykCR5c6aHVUUpGJ8J7ZqpLLY97i1/Menus/0bb39514-b297-436c-8554-c5e5a52486ac/";
                    //menuData.MenuFile = "Marzano Phones.json";
                    //menuData.MenuName = "Marzano Phone";


                    return foodServiceClientSession;
                }
                catch (Exception error)
                {

                    throw;
                }
            });

        }
        /// <MetaDataID>{8e393af4-d3d0-4dc5-b9c5-8043d5950566}</MetaDataID>
        private void ServicesContextManagment_ObjectChangeState(object _object, string member)
        {
            var obj = OOAdvantech.Remoting.RestApi.RemotingServices.CastTransparentProxy<IFlavoursServicesContextManagment>(_object);
            //throw new NotImplementedException();
        }



        /// <MetaDataID>{7de752f6-71be-4fb9-8c3b-789d04c70d8e}</MetaDataID>
        OOAdvantech.Speech.ITextToSpeech TextToSpeech;

        [OOAdvantech.MetaDataRepository.HttpInVisible]
        event OOAdvantech.ObjectChangeStateHandle _ObjectChangeState;

        [OOAdvantech.MetaDataRepository.HttpInVisible]
        public event OOAdvantech.ObjectChangeStateHandle ObjectChangeState
        {
            add
            {
                _ObjectChangeState += value;
            }
            remove
            {
                _ObjectChangeState -= value;
            }
        }


        [OOAdvantech.MetaDataRepository.HttpInVisible]
        event PartOfMealRequestHandle _PartOfMealRequest;

        [OOAdvantech.MetaDataRepository.HttpInVisible]
        public event PartOfMealRequestHandle PartOfMealRequest
        {
            add
            {
                _PartOfMealRequest += value;

                if (PartOfMealMessage != null)
                {
                    var message = PartOfMealMessage;
                    PartOfMealMessage = null;
                    Task.Run(() =>
                    {
                        if (MessmatesLoaded)
                        {
                            GetCandidateMessmates();
                            var messmate = (from theMessmate in this.CandidateMessmates
                                            where theMessmate.ClientSessionID == message.GetDataValue("ClientSessionID") as string
                                            select theMessmate).FirstOrDefault();

                            if (messmate == null)
                            {
                                messmate = (from theMessmate in this.Messmates
                                            where theMessmate.ClientSessionID == message.GetDataValue("ClientSessionID") as string
                                            select theMessmate).FirstOrDefault();
                                if (messmate != null)
                                    FoodServiceClientSession.RemoveMessage(message.MessageID);

                                return;
                            }

                            _PartOfMealRequest?.Invoke(this, messmate, message.MessageID);
                        }
                    });
                }

            }
            remove
            {
                _PartOfMealRequest -= value;
            }
        }

        [OOAdvantech.MetaDataRepository.HttpInVisible]
        event SharedItemChangedHandle _SharedItemChanged;

        [OOAdvantech.MetaDataRepository.HttpInVisible]
        public event SharedItemChangedHandle SharedItemChanged
        {
            add
            {
                _SharedItemChanged += value;
            }
            remove
            {
                _SharedItemChanged -= value;
            }
        }


        [OOAdvantech.MetaDataRepository.HttpInVisible]
        event MessmatesWaitForYouToDecideHandle _MessmatesWaitForYouToDecide;

        [OOAdvantech.MetaDataRepository.HttpInVisible]
        public event MessmatesWaitForYouToDecideHandle MessmatesWaitForYouToDecide
        {
            add
            {
                _MessmatesWaitForYouToDecide += value;
            }
            remove
            {
                _MessmatesWaitForYouToDecide -= value;
            }
        }







        [OOAdvantech.MetaDataRepository.HttpInVisible]
        event MenuItemProposalHandle _MenuItemProposal;

        [OOAdvantech.MetaDataRepository.HttpInVisible]
        public event MenuItemProposalHandle MenuItemProposal
        {
            add
            {
                _MenuItemProposal += value;

                if (MenuItemProposalMessage != null)
                {
                    var message = MenuItemProposalMessage;
                    MenuItemProposalMessage = null;
                    Task.Run(() =>
                    {

                        if (MessmatesLoaded)
                        {
                            var messmate = (from theMessmate in this.Messmates
                                            where theMessmate.ClientSessionID == message.GetDataValue("ClientSessionID") as string
                                            select theMessmate).FirstOrDefault();

                            if (messmate == null)
                            {
                                messmate = (from theMessmate in this.Messmates
                                            where theMessmate.ClientSessionID == message.GetDataValue("ClientSessionID") as string
                                            select theMessmate).FirstOrDefault();
                                if (messmate != null)
                                    FoodServiceClientSession.RemoveMessage(message.MessageID);

                                return;
                            }
                            string menuItemUri = message.GetDataValue<string>("MenuItemUri");

                            _MenuItemProposal?.Invoke(this, messmate, menuItemUri, message.MessageID);
                        }
                    });
                }

            }
            remove
            {
                _MenuItemProposal -= value;
            }
        }



        /// <MetaDataID>{3b8f1bd2-0697-4d3e-81d2-e304771ac9b6}</MetaDataID>
        public void Speak(string text)
        {


            // MessmateAdded?.Invoke(this, "Liakos");


            if (TextToSpeech == null)
            {
                var deviceInstantiator = Xamarin.Forms.DependencyService.Get<OOAdvantech.IDeviceInstantiator>();
                TextToSpeech = deviceInstantiator.GetDeviceSpecific(typeof(OOAdvantech.Speech.ITextToSpeech)) as OOAdvantech.Speech.ITextToSpeech;
            }
            if (TextToSpeech != null)
                TextToSpeech.Speak(text);


        }

        /// <MetaDataID>{75fc6cad-1e29-4aa3-97fa-14462e970f67}</MetaDataID>
        public Task<string> GetFriendlyName()
        {
            return Task.Run<string>(() =>
            {
                var deviceInstantiator = Xamarin.Forms.DependencyService.Get<OOAdvantech.IDeviceInstantiator>();
                OOAdvantech.IDeviceOOAdvantechCore device = deviceInstantiator.GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
                //string sdfd= device.
                string friendlyName = ApplicationSettings.Current.FriendlyName;
                return friendlyName;
            });
            //return ApplicationSettings.Current.FriendlyName;
            //return "";

        }

        /// <MetaDataID>{60462cb1-1349-470d-87c9-923b2e7fe825}</MetaDataID>
        public void SetFriendlyName(string friendlyName)
        {
            ApplicationSettings.Current.FriendlyName = friendlyName;

            if (this.FoodServiceClientSession != null&& this.FoodServiceClientSession.ClientName!= friendlyName)
            {
                #region Sets client name of active session a sync for unstable connection 
                SerializeTaskScheduler.AddTask(async () =>
                  {
                      int tries = 30; //try for 30 time 
                      while (tries > 0)
                      {
                          try
                          {
                              if (this.FoodServiceClientSession != null)
                                  this.FoodServiceClientSession.ClientName = friendlyName;
                          }
                          catch (System.Net.WebException commError)
                          {
                              await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(2));
                          }
                          catch (Exception error)
                          {
                              await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(2));
                          }
                      }
                      return true;
                  }); 
                #endregion
            }
            
        }
        /// <MetaDataID>{8e9519da-bc4b-4953-8bde-7f95bea4a1f5}</MetaDataID>
        public async Task<bool> SendItemForPreparation()
        {
            Dictionary<string, ItemPreparationState> itemsNewState = null;

            //if (!WaiterView)
            itemsNewState = this.FoodServiceClientSession.Commit(OrderItems.OfType<IItemPreparation>().ToList());
            //else
            //    itemsNewState = this.FoodServiceClientSession.Prepare(OrderItems.OfType<IItemPreparation>().ToList());

            foreach (var itemNewState in itemsNewState)
            {
                var item = this.OrderItems.Where(x => x.uid == itemNewState.Key).FirstOrDefault();
                item.State = item.State;
            }
            return true;
        }

        /// <MetaDataID>{11b11a15-ce5b-4d4f-aaaf-ff6e6427b9f1}</MetaDataID>
        public async Task<bool> CheckPermissionsForServicePointScan()
        {
#if DeviceDotNet
#if IOSEmulator
                    return true;
#else

            var status = await Plugin.Permissions.CrossPermissions.Current.CheckPermissionStatusAsync(Plugin.Permissions.Abstractions.Permission.Camera);
            return status == Plugin.Permissions.Abstractions.PermissionStatus.Granted;
#endif
#else
            return false;
#endif
        }

        /// <MetaDataID>{d20a2a27-4af2-4642-a402-373c188de241}</MetaDataID>
        public async Task<bool> RequestPermissionsForServicePointScan()
        {
#if DeviceDotNet
            var status = (await Plugin.Permissions.CrossPermissions.Current.RequestPermissionsAsync(Plugin.Permissions.Abstractions.Permission.Camera))[Plugin.Permissions.Abstractions.Permission.Camera];
            return status == Plugin.Permissions.Abstractions.PermissionStatus.Granted;
#else
            return true;
#endif
        }

        /// <MetaDataID>{aeabec87-3214-4488-b5a6-77cba7b5bf51}</MetaDataID>
        public async Task<bool> RequestPermissionsForBatteryOptimazation()
        {
#if DeviceDotNet

            var batteryOptimazationPermission = await Xamarin.Essentials.Permissions.CheckStatusAsync<Xamarin.Essentials.Permissions.Battery>();
            if (batteryOptimazationPermission == Xamarin.Essentials.PermissionStatus.Denied)
                batteryOptimazationPermission = await Xamarin.Essentials.Permissions.RequestAsync<Xamarin.Essentials.Permissions.Battery>();
            return batteryOptimazationPermission == Xamarin.Essentials.PermissionStatus.Granted;
#else
            return true;
#endif
        }

        /// <MetaDataID>{67a87990-9414-48bc-8d1d-bc20335eb6ea}</MetaDataID>
        List<ItemPreparation> OrderItems = new List<ItemPreparation>();

        /// <MetaDataID>{9748ebb1-5f4e-4afc-839d-40a2b3667c25}</MetaDataID>
        public void AddItem(ItemPreparation item)
        {

            OrderItems.Add(item);
            var menuData = MenuData;
            menuData.OrderItems = OrderItems.ToList();
            MenuData = menuData;
            SerializeTaskScheduler.AddTask(async () =>
            {
                var datetime = DateTime.Now;
                string timestamp = DateTime.Now.ToLongTimeString() + ":" + datetime.Millisecond.ToString();

                int tries = 30;
                while (tries > 0)
                {
                    try
                    {
                        this.FoodServiceClientSession.AddItem(item);
                        int cou = this.FoodServiceClientSession.FlavourItems.Count;
                        break;
                    }
                    catch (System.Net.WebException commError)
                    {
                        await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(1));
                    }
                    catch (Exception error)
                    {
                        var er = error;
                        await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(1));
                    }
                }
                return true;
            });
        }

        /// <MetaDataID>{045d624f-d908-4164-a3be-e9d408ed4b70}</MetaDataID>
        public void AddSharingItem(ItemPreparation item)
        {
            OrderItems.Add(item);
            var menuData = MenuData;
            menuData.OrderItems = OrderItems.ToList();
            MenuData = menuData;

            SerializeTaskScheduler.AddTask(async () =>
            {
                var datetime = DateTime.Now;
                string timestamp = DateTime.Now.ToLongTimeString() + ":" + datetime.Millisecond.ToString();

                int tries = 30;
                while (tries > 0)
                {
                    try
                    {
                        this.FoodServiceClientSession.AddSharingItem(item);
                        break;
                    }
                    catch (System.Net.WebException commError)
                    {
                        await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(1));
                    }
                    catch (Exception error)
                    {
                        var er = error;
                        await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(1));
                    }
                }
                return true;
            });


        }

        /// <MetaDataID>{0f73c1f0-60e8-4045-b512-6f80f7a71888}</MetaDataID>
        public void ItemChanged(FlavourBusinessManager.RoomService.ItemPreparation item)
        {
            bool hasChanges = OrderItemsDictionary[item.uid].Update(item);
            if (hasChanges)
            {
                var menuData = MenuData;
                menuData.OrderItems = OrderItems.ToList();
                MenuData = menuData;
                SerializeTaskScheduler.AddTask(async () =>
                {
                    var datetime = DateTime.Now;
                    string timestamp = DateTime.Now.ToLongTimeString() + ":" + datetime.Millisecond.ToString();

                    int tries = 30;
                    while (tries > 0)
                    {
                        try
                        {
                            FoodServiceClientSession.ItemChanged(item);
                            break;
                        }
                        catch (System.Net.WebException commError)
                        {
                            await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(1));
                        }
                        catch (Exception error)
                        {
                            var er = error;
                            await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(1));
                        }
                    }
                    return true;
                });
            }
        }

        Dictionary<string, ItemPreparation> OrderItemsDictionary
        {
            get
            {
                Dictionary<string, ItemPreparation> orderItemsDictionary = new Dictionary<string, ItemPreparation>();
                foreach (var orderItem in OrderItems)
                    orderItemsDictionary[orderItem.uid] = orderItem;
                return orderItemsDictionary;
                    //OrderItems.ToDictionary(x => x.uid);
            }
        }


        /// <MetaDataID>{ea4782f3-0b61-4478-a9e0-9deaac39207e}</MetaDataID>
        public void RemoveItem(ItemPreparation item)
        {


            if (OrderItemsDictionary.ContainsKey(item.uid))
                OrderItemsDictionary[item.uid].Update(item);

            foreach (var shareItem in (from messmate in Messmates
                                       from itemPreparation in messmate.PreparationItems
                                       where itemPreparation.uid == item.uid
                                       select itemPreparation))
            {
                shareItem.Update(item);
            }

            var orderItem = OrderItems.Where(x => x.uid == item.uid).FirstOrDefault();
            if (orderItem != null)
                OrderItems.Remove(orderItem);

            var menuData = MenuData;
            menuData.OrderItems = OrderItems.ToList();
            MenuData = menuData;
            SerializeTaskScheduler.AddTask(async () =>
            {
                var datetime = DateTime.Now;
                string timestamp = DateTime.Now.ToLongTimeString() + ":" + datetime.Millisecond.ToString();

                int tries = 30;
                while (tries > 0)
                {
                    try
                    {
                        if (this.FoodServiceClientSession != null)
                        {
                            if (item.SessionID == SessionID)
                            {
                                this.FoodServiceClientSession.RemoveItem(item);
                                int cou = this.FoodServiceClientSession.FlavourItems.Count;
                                break;
                            }
                            else
                            {
                                this.FoodServiceClientSession.RemoveSharingItem(item);
                                break;

                            }
                        }
                        else
                        {
                            await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(1));
                        }
                    }
                    catch (System.Net.WebException commError)
                    {
                        await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(1));
                    }
                    catch (Exception error)
                    {
                        var er = error;
                        await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(1));
                    }
                }
                _ObjectChangeState?.Invoke(this, null);
                return true;
            });

        }

        /// <MetaDataID>{546af2d0-8204-44e9-83fc-4d0d782e30f3}</MetaDataID>
        public void AcceptInvitation(Messmate messmate, string messageID)
        {


            if (ActivePartOfMealMessage != null && ActivePartOfMealMessage.MessageID == messageID)
                ActivePartOfMealMessage = null;

            var clientSession = (from theMessmate in this.CandidateMessmates
                                 where theMessmate.ClientSessionID == messmate.ClientSessionID
                                 select theMessmate.ClientSession).FirstOrDefault();

            var ss = clientSession.ClientName;

            try
            {
                this.FoodServiceClientSession.AcceptMealInvitation(ClientSessionToken, clientSession);
                FoodServiceClientSession.RemoveMessage(messageID);
                GetMessages();
            }
            catch (Exception authenticationError)
            {

                Task<MenuData>.Run(async () =>
                {
                    var clientSessionData = await GetFoodServiceSession("");
                    this.FoodServiceClientSession = clientSessionData.FoodServiceClientSession;
                    RefreshMessmates();
                    this.ClientSessionToken = clientSessionData.Token;
                    this.FoodServiceClientSession.AcceptMealInvitation(ClientSessionToken, clientSession);
                });
            }

        }

        /// <MetaDataID>{a4bed371-81f6-45e5-bd80-e825438ff80e}</MetaDataID>
        public void DenyInvitation(Messmate messmate, string messageID)
        {
            if (ActivePartOfMealMessage != null && ActivePartOfMealMessage.MessageID == messageID)
                ActivePartOfMealMessage = null;


            FoodServiceClientSession.RemoveMessage(messageID);
            var clientSession = (from theMessmate in this.CandidateMessmates
                                 where theMessmate.ClientSessionID == messmate.ClientSessionID
                                 select theMessmate.ClientSession).FirstOrDefault();
            clientSession.MealInvitationDenied(FoodServiceClientSession);
            GetMessages();
        }
        /// <MetaDataID>{50033e95-3a4d-492d-98ba-f14f0e359418}</MetaDataID>
        public void EndOfMenuItemProposal(Messmate messmate, string messageID)
        {
            FoodServiceClientSession.RemoveMessage(messageID);
            GetMessages();
        }

        /// <MetaDataID>{c36bf954-e18c-4f82-aef4-ea94b18a4747}</MetaDataID>
        public void SuggestMenuItem(Messmate messmate, string menuItemUri)
        {
            var clientSession = (from theMessmate in this.Messmates
                                 where theMessmate.ClientSessionID == messmate.ClientSessionID
                                 select theMessmate.ClientSession).FirstOrDefault();

            clientSession.MenuItemProposal(FoodServiceClientSession, menuItemUri);

        }

        /// <MetaDataID>{3b2f428f-f1e1-4845-b4e7-6ec49a2502fb}</MetaDataID>
        public Task<HallLayout> GetHallLayout()
        {
            return Task<HallLayout>.Run(async () =>
            {
                try
                {
                    MenuData menuData = new MenuData();
                    string servicePoint = "7f9bde62e6da45dc8c5661ee2220a7b0;9967813ee9d943db823ca97779eb9fd7";


                    string assemblyData = "FlavourBusinessManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
                    string type = "FlavourBusinessManager.FlavoursServicesContextManagment";
                    string serverUrl = AzureServerUrl;
                    IFlavoursServicesContextManagment servicesContextManagment = RemotingServices.CastTransparentProxy<IFlavoursServicesContextManagment>(OOAdvantech.Remoting.RestApi.RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData));
                    servicesContextManagment.ObjectChangeState += ServicesContextManagment_ObjectChangeState;

                    var deviceInstantiator = Xamarin.Forms.DependencyService.Get<OOAdvantech.IDeviceInstantiator>();
                    OOAdvantech.IDeviceOOAdvantechCore device = deviceInstantiator.GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
                    FlavourBusinessFacade.ServicesContextResources.IHallLayout hallLayout = servicesContextManagment.GetHallLayout(servicePoint);


                    if (hallLayout is HallLayout)
                    {
                        (hallLayout as HallLayout).SetShapesImagesRoot("http://192.168.2.8/devstoreaccount1/halllayoutsresources/Shapes/");
                        var sds = hallLayout.Name;
                    }


                    return hallLayout as HallLayout;
                }
                catch (Exception error)
                {

                    throw;
                }
            });
        }

        /// <MetaDataID>{7fd664dc-cdda-488a-ab38-6670e874af11}</MetaDataID>
        public Task<bool> CheckPermissionsPassivePushNotification()
        {

#if DeviceDotNet2
            var deviceInstantiator = Xamarin.Forms.DependencyService.Get<OOAdvantech.IDeviceInstantiator>();
            OOAdvantech.IBatteryInfo batteryInfo = deviceInstantiator.GetDeviceSpecific(typeof(OOAdvantech.IBatteryInfo)) as OOAdvantech.IBatteryInfo;
            return Task<bool>.Run(()=> { return batteryInfo.CheckIsEnableBatteryOptimizations(); });
#else
            return Task<bool>.Run(() => { return true; });
#endif


        }

        /// <MetaDataID>{c6fbb6fd-26d1-4a63-bb9a-1b68e79f780c}</MetaDataID>
        public async Task<bool> RequestPermissionsPassivePushNotification()
        {
#if DeviceDotNet
            var deviceInstantiator = Xamarin.Forms.DependencyService.Get<OOAdvantech.IDeviceInstantiator>();
            OOAdvantech.IBatteryInfo batteryInfo = deviceInstantiator.GetDeviceSpecific(typeof(OOAdvantech.IBatteryInfo)) as OOAdvantech.IBatteryInfo;
            batteryInfo.StartSetting();
            return await CheckPermissionsPassivePushNotification();

#else
            return true;
#endif


        }
        /// <MetaDataID>{cebd99a0-8d4c-420a-a861-5aabc396dae5}</MetaDataID>
        Message PartOfMealMessage;

        /// <MetaDataID>{d048d779-11d6-4c80-b308-24e58a0359f8}</MetaDataID>
        Message MenuItemProposalMessage;

        /// <MetaDataID>{3e58cac8-fc04-4ff0-9ec9-68df0268646b}</MetaDataID>
        private string SessionID;

        /// <MetaDataID>{7c812852-1690-4bdb-bbb4-2605f03476ab}</MetaDataID>
        internal async void Initialize()
        {
            if(MenuData.OrderItems!=null)
                OrderItems = MenuData.OrderItems.ToList();
            if (!string.IsNullOrWhiteSpace(ApplicationSettings.Current.LastServicePoinMenuData.ServicePointIdentity))
            {
                while (!await GetServicePointData(MenuData.ServicePointIdentity))
                {
                    if (string.IsNullOrWhiteSpace(Path) || Path.Split('/')[0] != MenuData.ServicePointIdentity)
                        break;
                }
                if (this.FoodServiceClientSession != null)
                {
                    GetMessages();

                }
                else
                {

                    _ObjectChangeState?.Invoke(this, nameof(FoodServiceClientSession));
                    ApplicationSettings.Current.LastServicePoinMenuData = new MenuData() { OrderItems = new List<ItemPreparation>() }; 
                    ApplicationSettings.Current.LastClientSessionID = "";
                    Path = "";

                }
            }
        }

        /// <MetaDataID>{ba41dc5e-2e26-4830-963e-e9c3e5077f12}</MetaDataID>
        public MarshalByRefObject GetObjectFromUri(string uri)
        {
            return this;
        }
        public string GetString(string langCountry, string key)
        {
            return "";
        }

        public string GetTranslation(string langCountry)
        {
            throw new NotImplementedException();
        }

        public void SetString(string langCountry, string key, string newValue)
        {
            throw new NotImplementedException();
        }
    }

}
