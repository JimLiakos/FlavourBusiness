using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FlavourBusinessFacade;
using FlavourBusinessFacade.EndUsers;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Remoting;
using OOAdvantech.Remoting.RestApi;
using Xamarin.Forms;

namespace DontWaitApp
{


    public delegate void NewMessmateHandler(IFlavoursOrderServer flavoursOrderServer, string messmateName);


    /// <MetaDataID>{90fe460d-7996-49ca-a085-054466973111}</MetaDataID>
    [OOAdvantech.MetaDataRepository.HttpVisible]
    public interface IFlavoursOrderServer
    {
        string Identity { get; set; }
        string Name { get; set; }
        string Trademark { get; set; }
        string Foo();
        void Speak(string text);
        Task<MenuData> ConnectToServicePoint();

        IUser EndUser { get; set; }

        [GenerateEventConsumerProxy]
        event NewMessmateHandler MessmateAdded;

        Task<string> GetFriendlyName();

        void SetFriendlyName(string firendlyName);


    }



    /// <MetaDataID>{dcf0d40a-e2ed-4711-aa96-8ee72a121740}</MetaDataID>
    class FlavoursOrderServer : MarshalByRefObject, IFlavoursOrderServer, OOAdvantech.Remoting.IExtMarshalByRefObject
    {


        public FlavoursOrderServer()
        {
            MessmateAdded += FlavoursOrderServer_MessmateAdded;
            _EndUser = new FoodServiceClientVM();

#if DeviceDotNet
            ScanPage.ZxingView.OnScanResult += (result) =>
             Device.BeginInvokeOnMainThread(async () =>
             {
                 if (OnScan)
                 {
                     TaskCompletionSource<MenuData> connectToServicePointTask = null;
                     lock (this)
                     {
                         connectToServicePointTask = ConnectToServicePointTask;
                         ConnectToServicePointTask = null;
                         OnScan = false;
                     }
                     // Stop analysis until we navigate away so we don't keep reading barcodes
                     ScanPage.ZxingView.IsAnalyzing = false;

                     // Show an alert
                     // await App.Current.MainPage.DisplayAlert("Scanned Barcode", result.Text, "OK");

                     // Navigate away
                     try
                     {
                         await ScanPage.Navigation.PopAsync();
                         FoodServiceClientSession = await GetFoodServiceSession("");
                         var storeRef = FoodServiceClientSession.Menu;
                         MenuData menuData = new MenuData() { MenuName = storeRef.Name, MenuRoot = storeRef.StorageUrl.Substring(0, storeRef.StorageUrl.LastIndexOf("/") + 1), MenuFile = storeRef.StorageUrl.Substring(storeRef.StorageUrl.LastIndexOf("/") + 1) };
                         connectToServicePointTask.SetResult(menuData);
                     }
                     catch (Exception error)
                     {
                         connectToServicePointTask.SetResult(new MenuData());

                     }
                     

                 }
             });

            ScanPage.Disappearing += (object sender, EventArgs e) =>
             {
                 OnScan = false;
                 if (ConnectToServicePointTask != null)
                     ConnectToServicePointTask.SetResult(new MenuData());
             };
#endif
        }

        private void FlavoursOrderServer_MessmateAdded(IFlavoursOrderServer flavoursOrderServer, string messmateName)
        {

        }

        string _Identity;
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
        string _Name;
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
        int count = 0;

        string _Trademark;
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
        IUser _EndUser;
        public IUser EndUser
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

        public string Foo()
        {

            MessmateAdded?.Invoke(this, "Liakos");

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
        //static string AzureServerUrl = "http://192.168.2.4:8090/api/";
        static string AzureServerUrl = "http://192.168.2.7:8090/api/";
        //static string AzureServerUrl = "http://10.0.0.10:8090/api/";
#if DeviceDotNet
        ScanPage ScanPage = new ScanPage();//  FormsSample.CustomScanPage()
#endif
        TaskCompletionSource<MenuData> ConnectToServicePointTask;
        //var taskCompletionSource = new TaskCompletionSource<ResponseData>();
        bool OnScan = false;
        public Task<MenuData> ConnectToServicePoint()
        {


#if DeviceDotNetk
            lock (this)
            {
                if (OnScan && ConnectToServicePointTask != null)
                    return ConnectToServicePointTask.Task;

                OnScan = true;
                ConnectToServicePointTask = new TaskCompletionSource<MenuData>();
            }
            Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
            {
                await (App.Current.MainPage as NavigationPage).CurrentPage.Navigation.PushAsync(ScanPage);
            });
            return ConnectToServicePointTask.Task;
#else
            return Task<MenuData>.Run(async () =>
           {

               FoodServiceClientSession = await GetFoodServiceSession("");
               var storeRef = FoodServiceClientSession.Menu;
               MenuData menuData = new MenuData() { MenuName = storeRef.Name, MenuRoot = storeRef.StorageUrl.Substring(0, storeRef.StorageUrl.LastIndexOf("/") + 1), MenuFile = storeRef.StorageUrl.Substring(storeRef.StorageUrl.LastIndexOf("/") + 1) };

               return menuData;

           });
#endif
        }

        IFoodServiceClientSession FoodServiceClientSession;

        Task<IFoodServiceClientSession> GetFoodServiceSession(string servicePointID)
        {
            return Task<IFoodServiceClientSession>.Run(async () =>
            {
                MenuData menuData = new MenuData();
                string servicePoint = "7f9bde62e6da45dc8c5661ee2220a7b0;9967813ee9d943db823ca97779eb9fd7";
                //string servicePoint = "ca33b38f5c634fd49c50af60b042f910;8dedb45522ad479480e113c59d4bbdd0";
                //servicePoint = "7f9bde62e6da45dc8c5661ee2220a7b0;8dedb45522ad479480e113c59d4bbdd0";
                //// servicePoint = "6746e4178dd041f09a7b4130af0edacf;6171631179bf4c26aeb99546fdce6a7a";
                //servicePoint = "b5ec4ed264c142adb26b73c95b185544;9967813ee9d943db823ca97779eb9fd7";


                string assemblyData = "FlavourBusinessManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
                string type = "FlavourBusinessManager.FlavoursServicesContextManagment";
                string serverUrl = AzureServerUrl;
                IFlavoursServicesContextManagment servicesContextManagment = OOAdvantech.Remoting.RestApi.RemotingServices.CastTransparentProxy<IFlavoursServicesContextManagment>(OOAdvantech.Remoting.RestApi.RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData));
                servicesContextManagment.ObjectChangeState += ServicesContextManagment_ObjectChangeState;

                var deviceInstantiator = Xamarin.Forms.DependencyService.Get<OOAdvantech.IDeviceInstantiator>();
                OOAdvantech.IDeviceOOAdvantechCore device = deviceInstantiator.GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;

                var foodServiceClientSession = servicesContextManagment.GetClientSession(servicePoint, await GetFriendlyName(), device.DeviceID, null);


                //menuData.MenuRoot = "http://192.168.2.3/devstoreaccount1/usersfolder/ykCR5c6aHVUUpGJ8J7ZqpLLY97i1/Menus/0bb39514-b297-436c-8554-c5e5a52486ac/";
                //menuData.MenuFile = "Marzano Phone.json";
                //menuData.MenuName = "Marzano Phone";
                //menuData.MenuRoot = "http://192.168.2.10/devstoreaccount1/usersfolder/ykCR5c6aHVUUpGJ8J7ZqpLLY97i1/Menus/0bb39514-b297-436c-8554-c5e5a52486ac/";
                //menuData.MenuFile = "Marzano Phones.json";
                //menuData.MenuName = "Marzano Phone";

                return foodServiceClientSession;
            });

        }
        private void ServicesContextManagment_ObjectChangeState(object _object, string member)
        {
            var obj = OOAdvantech.Remoting.RestApi.RemotingServices.CastTransparentProxy<IFlavoursServicesContextManagment>(_object);
            //throw new NotImplementedException();
        }

        OOAdvantech.Speech.ITextToSpeech TextToSpeech;

        public event NewMessmateHandler MessmateAdded;

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

        public async Task<string> GetFriendlyName()
        {
            var deviceInstantiator = Xamarin.Forms.DependencyService.Get<OOAdvantech.IDeviceInstantiator>();
            OOAdvantech.IDeviceOOAdvantechCore device = deviceInstantiator.GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
            //string sdfd= device.
            return ApplicationSettings.Current.FriendlyName;
            //return "";

        }

        public void SetFriendlyName(string friendlyName)
        {
            ApplicationSettings.Current.FriendlyName = friendlyName;
            //throw new NotImplementedException();
        }
    }

    /// <MetaDataID>{5718fadd-9a57-4d87-a6ea-ba669ab3388a}</MetaDataID>
    public class MenuData
    {
        public string MenuName { get; set; }
        public string MenuFile { get; set; }
        public string MenuRoot { get; set; }

    }


}

