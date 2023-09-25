using DontWaitApp;
using FlavourBusinessFacade;
using FlavourBusinessFacade.EndUsers;
using FlavourBusinessFacade.HumanResources;
using FlavourBusinessFacade.ServicesContextResources;
using FlavourBusinessFacade.ViewModel;
using OOAdvantech;

using OOAdvantech.Remoting.RestApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//using static QRCoder.PayloadGenerator;
#if DeviceDotNet
using Xamarin.Forms;
using DeviceUtilities.NetStandard;
using Xamarin.Essentials;
using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;
#else
using MarshalByRefObject = System.MarshalByRefObject;
#endif


namespace CourierApp.ViewModel
{



    /// <MetaDataID>{1230a8d4-5e45-4ebb-891e-af3db0b09974}</MetaDataID>
    public class CourierActivityPresentation : MarshalByRefObject, OOAdvantech.Remoting.IExtMarshalByRefObject, ICourierActivityPresentation, ISecureUser, IBoundObject, IDevicePermissions
    {
        public string SignInProvider { get; set; }
        public string OAuthUserIdentity { get; set; }
        public string FullName { get; set; }

        public string Address { get; set; }

        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string PhoneNumber { get; set; }

        public event ObjectChangeStateHandle ObjectChangeState;

        public void CreateUserWithEmailAndPassword(string emailVerificationCode)
        {
            IAuthFlavourBusiness pAuthFlavourBusiness = null;
            try
            {
                UserData userData = new UserData() { UserName = UserName, Email = Email, FullName = FullName, PhoneNumber = PhoneNumber };
                pAuthFlavourBusiness = GetFlavourBusinessAuth();
                pAuthFlavourBusiness.SignUpUserWithEmailAndPassword(Email, Password, userData, emailVerificationCode);
            }
            catch (System.Net.WebException error)
            {
                throw;
            }
            catch (Exception error)
            {
                throw;
            }
        }


        IList<UserData> NativeUsers;

        public IList<UserData> GetNativeUsers()
        {
            lock (this)
            {
                if (NativeUsers!=null)
                    return NativeUsers;
            }

            if (string.IsNullOrWhiteSpace(ApplicationSettings.Current.ServiceContextDevice))
                return new List<UserData>();


            string assemblyData = "FlavourBusinessManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
            string type = "FlavourBusinessManager.AuthFlavourBusiness";// typeof(FlavourBusinessManager.AuthFlavourBusiness).FullName;
            //System.Runtime.Remoting.Messaging.CallContext.SetData("AutUser", authUser);
            string serverUrl = "http://localhost/FlavourBusinessWebApiRole/api/";
            serverUrl = "http://localhost:8090/api/";
            serverUrl = AzureServerUrl;
            IAuthFlavourBusiness pAuthFlavourBusiness = null;
            try
            {
                var remoteObject = RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData);
                pAuthFlavourBusiness = RemotingServices.CastTransparentProxy<IAuthFlavourBusiness>(remoteObject);
            }
            catch (System.Net.WebException error)
            {
                throw;
            }
            catch (Exception error)
            {
                throw;
            }
            IFlavoursServicesContextManagment servicesContextManagment = OOAdvantech.Remoting.RestApi.RemotingServices.CastTransparentProxy<IFlavoursServicesContextManagment>(OOAdvantech.Remoting.RestApi.RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData));
            string serviceContextIdentity = ApplicationSettings.Current.ServiceContextDevice;
            List<UserData> nativeUsers = pAuthFlavourBusiness.GetNativeUsers(serviceContextIdentity, RoleType.Courier).ToList();

            lock (this)
            {
                NativeUsers=nativeUsers;
            }
             

            return nativeUsers; ;
            //return new List<UserData>();
        }



        public bool IsUsernameInUse(string username, OOAdvantech.Authentication.SignInProvider signInProvider)
        {
            IAuthFlavourBusiness pAuthFlavourBusiness = null;


            try
            {
                pAuthFlavourBusiness = GetFlavourBusinessAuth();
                return pAuthFlavourBusiness.IsUsernameInUse(username, signInProvider);
            }
            catch (System.Net.WebException error)
            {
                throw;
            }
            catch (Exception error)
            {
                throw;
            }
        }

        public void SaveUserProfile()
        {
            throw new NotImplementedException();
        }

        public void SendVerificationEmail(string emailAddress)
        {
            throw new NotImplementedException();
        }
        ICourier Courier;
        AuthUser AuthUser;
        private Task<bool> SignInTask;
        private bool OnSignIn;

        public async Task<bool> SignIn()
        {

            //System.IO.File.AppendAllLines(App.storage_path, new string[] { "SignIn " });
            System.Diagnostics.Debug.WriteLine("public async Task< bool> SignIn()");
            AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
            if (authUser == null)
                authUser = DeviceAuthentication.AuthUser;
            if (authUser == null)
                return false;

            if (AuthUser != null && authUser.User_ID == AuthUser.User_ID)
            {
                Task.Run(async () =>
                {
                    await Task.Delay(5000);
                    GetMessages();
                });

                ObjectChangeState?.Invoke(this, null);
                if (Courier != null)
                    OAuthUserIdentity = Courier.OAuthUserIdentity;
                return true;
            }

            if (OnSignIn && SignInTask != null)
                return await SignInTask;
            else
            {
                SignInTask = Task<bool>.Run(async () =>
                {
                    OnSignIn = true;
                    try
                    {
                        if (authUser != null)
                        {
                            if (Courier != null)
                            {
                                Courier.ObjectChangeState -= Courier_ObjectChangeState;
                                Courier.MessageReceived -= MessageReceived;
                                //Courier.ServingBatchesChanged -= ServingBatchesChanged;
                                if (Courier is ITransparentProxy)
                                    (Courier as ITransparentProxy).Reconnected -= CourierActivityPresentation_Reconnected;
                            }
                            if (Courier != null && Courier.OAuthUserIdentity == authUser.User_ID)
                            {
                                AuthUser = authUser;
                                ActiveShiftWork = Courier.ActiveShiftWork;
                                //UpdateServingBatches(Courier.GetServingBatches());
                                Courier.ObjectChangeState += Courier_ObjectChangeState;
                                Courier.MessageReceived += MessageReceived;
                                //Courier.ServingBatchesChanged += ServingBatchesChanged;
                                if (Courier is ITransparentProxy)
                                    (Courier as ITransparentProxy).Reconnected += CourierActivityPresentation_Reconnected;

#if DeviceDotNet
                                IDeviceOOAdvantechCore device = DependencyService.Get<IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
                                Courier.DeviceFirebaseToken = device.FirebaseToken;
#endif
                                //ApplicationSettings.Current.FriendlyName = Courier.FullName;
                                GetMessages();

                                OAuthUserIdentity = Courier.OAuthUserIdentity;
                                return true;

                            }
                        }
                        IAuthFlavourBusiness pAuthFlavourBusiness = null;

                        try
                        {
                            pAuthFlavourBusiness = GetFlavourBusinessAuth();
                        }
                        catch (System.Net.WebException error)
                        {
                            throw;
                        }
                        catch (Exception error)
                        {
                            throw;
                        }

                        this.UserData = pAuthFlavourBusiness.SignIn();
                        if (UserData != null)
                        {
                            FullName = UserData.FullName;
                            UserName = UserData.UserName;
                            PhoneNumber = UserData.PhoneNumber;
                            Email = UserData.Email;
                            //OAuthUserIdentity = UserData.OAuthUserIdentity;

                            foreach (var role in UserData.Roles.Where(x => x.RoleType == RoleType.Courier))
                            {
                                if (role.RoleType == RoleType.Courier)
                                {
                                    if (Courier != null)
                                    {
                                        Courier.ObjectChangeState -= Courier_ObjectChangeState;
                                        Courier.MessageReceived -= MessageReceived;
                                        //Courier.ServingBatchesChanged -= ServingBatchesChanged;
                                        if (Courier is ITransparentProxy)
                                            (Courier as ITransparentProxy).Reconnected -= CourierActivityPresentation_Reconnected;
                                    }
                                    Courier = RemotingServices.CastTransparentProxy<ICourier>(role.User);
                                    if (Courier == null)
                                        continue;
                                    string objectRef = RemotingServices.SerializeObjectRef(Courier);
                                    ApplicationSettings.Current.CourierObjectRef = objectRef;
                                    Courier.ObjectChangeState += Courier_ObjectChangeState;
                                    Courier.MessageReceived += MessageReceived;
                                    //Courier.ServingBatchesChanged += ServingBatchesChanged;
                                    if (Courier is ITransparentProxy)
                                        (Courier as ITransparentProxy).Reconnected += CourierActivityPresentation_Reconnected;


#if DeviceDotNet
                                    IDeviceOOAdvantechCore device = DependencyService.Get<IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
                                    Courier.DeviceFirebaseToken = device.FirebaseToken;
                                    if (!device.IsBackgroundServiceStarted)
                                    {
                                        BackgroundServiceState serviceState = new BackgroundServiceState();
                                        device.RunInBackground(new Action(async () =>
                                        {
                                            var message = Courier.PeekMessage();
                                            Courier.MessageReceived += MessageReceived;
                                            do
                                            {
                                                System.Threading.Thread.Sleep(1000);

                                            } while (!serviceState.Terminate);

                                            Courier.MessageReceived -= MessageReceived;
                                            //if (Waiter is ITransparentProxy)
                                            //    (Waiter as ITransparentProxy).Reconnected -= WaiterPresentation_Reconnected;
                                        }), serviceState);
                                    }
#endif
                                    ActiveShiftWork = Courier.ActiveShiftWork;
                                    GetMessages();
                                }
                            }
                            //https://angularhost.z16.web.core.windows.net/halllayoutsresources/Shapes/DiningTableChairs020.svg


                            AuthUser = authUser;
                            if (Courier != null)
                                OAuthUserIdentity = Courier.OAuthUserIdentity;
                            ObjectChangeState?.Invoke(this, null);
                            return true;
                        }
                        else
                            return false;


                    }
                    catch (Exception error)
                    {

                        throw;
                    }
                    finally
                    {
                        OnSignIn = false;
                    }
                });

                var result = await SignInTask;
                SignInTask = null;
                return result;

            }

        }

        private void CourierActivityPresentation_Reconnected(object sender)
        {
            throw new NotImplementedException();
        }

        private void MessageReceived(IMessageConsumer sender)
        {

        }

        /// <MetaDataID>{0989d879-8309-46fc-ba3a-947448f9bfb4}</MetaDataID>
        private void Courier_ObjectChangeState(object _object, string member)
        {
            if (member == nameof(IServicesContextWorker.ActiveShiftWork))
            {
                ObjectChangeState?.Invoke(this, nameof(ActiveShiftWorkStartedAt));
                GetMessages();
            }
        }
        private void GetMessages()
        {

        }
        public UserData SignInNativeUser(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public void SignOut()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SignUp()
        {
            System.Diagnostics.Debug.WriteLine("public async Task< bool> SignIn()");
            AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
            if (authUser == null)
            {
                authUser = DeviceAuthentication.AuthUser;
            }
            if (DeviceAuthentication.AuthUser == null)
            {

            }
            return await Task<bool>.Run(async () =>
            {

                OnSignIn = true;
                try
                {

                    string assemblyData = "FlavourBusinessManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
                    string type = "FlavourBusinessManager.AuthFlavourBusiness";// typeof(FlavourBusinessManager.AuthFlavourBusiness).FullName;
                    System.Runtime.Remoting.Messaging.CallContext.SetData("AutUser", authUser);
                    string serverUrl = "http://localhost/FlavourBusinessWebApiRole/api/";
                    serverUrl = "http://localhost:8090/api/";
                    serverUrl = AzureServerUrl;
                    IAuthFlavourBusiness pAuthFlavourBusiness = null;
                    try
                    {
                        var remoteObject = RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData);
                        pAuthFlavourBusiness = RemotingServices.CastTransparentProxy<IAuthFlavourBusiness>(remoteObject);
                    }
                    catch (System.Net.WebException error)
                    {
                        throw;
                    }
                    catch (Exception error)
                    {
                        throw;
                    }
                    if (authUser == null)
                    {
                    }
                    UserData = new UserData() { Email = this.Email, FullName = this.FullName, PhoneNumber = this.PhoneNumber, Address = this.Address };
                    UserData = pAuthFlavourBusiness.SignUp(UserData);

                    if (UserData != null)
                    {
                        FullName = UserData.FullName;
                        UserName = UserData.UserName;
                        PhoneNumber = UserData.PhoneNumber;
                        //Address = UserData.Address;
                        //OAuthUserIdentity = UserData.OAuthUserIdentity;
                        AuthUser = authUser;
                        ObjectChangeState?.Invoke(this, null);
                        return true;
                    }
                    else
                        return false;

                }
                catch (Exception error)
                {
                    throw;
                }
                finally
                {
                    OnSignIn = false;
                }
            });
        }


        IShiftWork ActiveShiftWork;

        public DateTime ActiveShiftWorkStartedAt
        {
            get
            {
                if (InActiveShiftWork)
                    return ActiveShiftWork.StartsAt;
                else
                    return DateTime.MinValue;
            }
        }

        public DateTime ActiveShiftWorkEndsAt
        {
            get
            {
                if (InActiveShiftWork)
                    return ActiveShiftWork.StartsAt + TimeSpan.FromHours(ActiveShiftWork.PeriodInHours);
                else
                    return DateTime.MinValue;
            }
        }


        public bool InActiveShiftWork
        {
            get
            {
                if (ActiveShiftWork != null)
                {
                    var startedAt = ActiveShiftWork.StartsAt;
                    var workingHours = ActiveShiftWork.PeriodInHours;

                    var billingPayments = (ActiveShiftWork as IDebtCollection)?.BillingPayments;

                    var hour = System.DateTime.UtcNow.Hour + (((double)System.DateTime.UtcNow.Minute) / 60);
                    hour = Math.Round((hour * 2)) / 2;
                    var utcNow = DateTime.UtcNow.Date + TimeSpan.FromHours(hour);
                    if (utcNow >= startedAt.ToUniversalTime() && utcNow <= startedAt.ToUniversalTime() + TimeSpan.FromHours(workingHours))
                    {
                        return true;
                    }
                    else
                    {
                        ActiveShiftWork = null;
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }
        static string AzureServerUrl = string.Format("http://{0}:8090/api/", FlavourBusinessFacade.ComputingResources.EndPoint.Server);

        public UserData UserData { get; private set; }

        private static IAuthFlavourBusiness GetFlavourBusinessAuth()
        {
            IAuthFlavourBusiness pAuthFlavourBusiness;
            OOAdvantech.Remoting.RestApi.AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as OOAdvantech.Remoting.RestApi.AuthUser;
            string assemblyData = "FlavourBusinessManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
            string type = "FlavourBusinessManager.AuthFlavourBusiness";// typeof(FlavourBusinessManager.AuthFlavourBusiness).FullName;
            System.Runtime.Remoting.Messaging.CallContext.SetData("AutUser", authUser);
            string serverUrl = AzureServerUrl;
            var remoteObject = RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData);
            pAuthFlavourBusiness = RemotingServices.CastTransparentProxy<IAuthFlavourBusiness>(remoteObject);
            return pAuthFlavourBusiness;
        }
        public void ShowAppPermissions()
        {
#if DeviceDotNet
            AppInfo.ShowSettingsUI();
#endif
        }
        public async Task<bool> RequestPermissionsForQRCodeScan()
        {
#if DeviceDotNet
            return (await Permissions.RequestAsync<Permissions.Camera>()) == PermissionStatus.Granted;
#else
            return true;
#endif


        }
#if DeviceDotNet
        public ScanCode ScanCode = new ScanCode();
#endif
        public async Task<bool> AssignCourier()
        {
#if DeviceDotNet
            string courierAssignKey = null;
            try
            {
                var result = await ScanCode.Scan("Hold your phone up to the Waiter Identity", "Scanning will happen automatically");

                if (result == null || string.IsNullOrWhiteSpace(result.Text))
                    return false;
                courierAssignKey = result.Text;
            }
            catch (Exception error)
            {
                return false;
            }

            string assemblyData = "FlavourBusinessManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
            string type = "FlavourBusinessManager.FlavoursServicesContextManagment";
            string serverUrl = AzureServerUrl;

            AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
            if (authUser == null)
                authUser = DeviceAuthentication.AuthUser;
            try
            {
                IFlavoursServicesContextManagment servicesContextManagment = RemotingServices.CastTransparentProxy<IFlavoursServicesContextManagment>(OOAdvantech.Remoting.RestApi.RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData));
                this.Courier = servicesContextManagment.AssignCourierUser(courierAssignKey);
                type = "FlavourBusinessManager.AuthFlavourBusiness";
                var remoteObject = RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData);
                var pAuthFlavourBusiness = RemotingServices.CastTransparentProxy<IAuthFlavourBusiness>(remoteObject);
                UserData = pAuthFlavourBusiness.SignIn();
                return true;
            }
            catch (Exception error)
            {
                return false;
            }



            //lock (this)
            //{
            //    if (OnScan && ConnectToServicePointTask != null)
            //        return ConnectToServicePointTask.Task;

            //    OnScan = true;
            //    ConnectToServicePointTask = new TaskCompletionSource<bool>();
            //}
            //Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
            //{
            //    await (App.Current.MainPage as NavigationPage).CurrentPage.Navigation.PushAsync(ScanPage);
            //});


            //return ConnectToServicePointTask.Task;
#else
            return false;
#endif

        }

        public async Task<UserData> AssignDevice()
        {
#if DeviceDotNet
            string deviceAssignKey = null;
            try
            {
                var result = await ScanCode.Scan("Hold your phone up to the Waiter Identity", "Scanning will happen automatically");

                if (result == null || string.IsNullOrWhiteSpace(result.Text))
                    return null;
                deviceAssignKey = result.Text;
            }
            catch (Exception error)
            {
                return null;
            }

            string assemblyData = "FlavourBusinessManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
            string type = "FlavourBusinessManager.FlavoursServicesContextManagment";
            string serverUrl = AzureServerUrl;

            AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
            if (authUser == null)
                authUser = DeviceAuthentication.AuthUser;
            try
            {
                IFlavoursServicesContextManagment servicesContextManagment = RemotingServices.CastTransparentProxy<IFlavoursServicesContextManagment>(OOAdvantech.Remoting.RestApi.RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData));

                NativeUserSignInData nativeUserData= servicesContextManagment.AssignDeviceToNativeUser(deviceAssignKey);

                ApplicationSettings.Current.ServiceContextDevice = nativeUserData.ServiceContextIdentity;
                //this.Waiter = servicesContextManagment.AssignWaiterUser(waiterAssignKey);
                //type = "FlavourBusinessManager.AuthFlavourBusiness";
                //var remoteObject = RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData);
                //var pAuthFlavourBusiness = RemotingServices.CastTransparentProxy<IAuthFlavourBusiness>(remoteObject);
                //UserData = pAuthFlavourBusiness.SignIn();
                 lock (this)
                    NativeUsers=null;


                return new UserData() { Email =nativeUserData.FireBaseUserName,Password= nativeUserData.FireBasePasword };
            }
            catch (Exception error)
            {
                return null;
            }



            //lock (this)
            //{
            //    if (OnScan && ConnectToServicePointTask != null)
            //        return ConnectToServicePointTask.Task;

            //    OnScan = true;
            //    ConnectToServicePointTask = new TaskCompletionSource<bool>();
            //}
            //Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
            //{
            //    await (App.Current.MainPage as NavigationPage).CurrentPage.Navigation.PushAsync(ScanPage);
            //});


            //return ConnectToServicePointTask.Task;
#else

            var deviceAssignKey = "7f9bde62e6da45dc8c5661ee2220a7b0;cc05236e47984bda895ea287c33e5fe4";

            try
            {
                string assemblyData = "FlavourBusinessManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
                string type = "FlavourBusinessManager.FlavoursServicesContextManagment";
                string serverUrl = AzureServerUrl;

                IFlavoursServicesContextManagment servicesContextManagment = RemotingServices.CastTransparentProxy<IFlavoursServicesContextManagment>(OOAdvantech.Remoting.RestApi.RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData));

                NativeUserSignInData nativeUserData = servicesContextManagment.AssignDeviceToNativeUser(deviceAssignKey);

                
                ApplicationSettings.Current.ServiceContextDevice = nativeUserData.ServiceContextIdentity;

                lock (this)
                    NativeUsers=null;
                return new UserData() { Email =nativeUserData.FireBaseUserName, Password= nativeUserData.FireBasePasword };
            }
            catch (Exception error)
            {
                return null;
            }


            return null;
#endif

        }


        public MarshalByRefObject GetObjectFromUri(string uri)
        {
            return this;
        }
        public string DeviceName
        {
            get
            {
#if DeviceDotNet
                return DeviceInfo.Name;
#else
                return "";
#endif

                //DeviceInfo.Name;
            }
        }
        public async Task<bool> CheckPermissionsForQRCodeScan()
        {
#if DeviceDotNet

            return (await Xamarin.Essentials.Permissions.CheckStatusAsync<Permissions.Camera>()) == PermissionStatus.Granted;
#else
            return false;
#endif

        }


    }
}
