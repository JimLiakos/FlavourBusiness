using FlavourBusinessFacade;
using OOAdvantech;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Remoting.RestApi;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using FlavourBusinessFacade.HumanResources;
using FlavourBusinessFacade.ServicesContextResources;
using DontWaitApp;
using FlavourBusinessFacade.EndUsers;
using System.Reflection;
using OOAdvantech.Json.Linq;

#if DeviceDotNet
using DeviceUtilities.NetStandard;
using Xamarin.Forms;
using ZXing;
using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;
#else
using QRCoder;
//using System.Drawing.Imaging;
using MarshalByRefObject = System.MarshalByRefObject;
#endif

namespace WaiterApp.ViewModel
{



    /// <MetaDataID>{6fe8b63a-548e-4d58-9763-b765030a87d4}</MetaDataID>
    [HttpVisible]
    public interface ISecureUser : FlavourBusinessFacade.ViewModel.IUser
    {
        [GenerateEventConsumerProxy]
        event ObjectChangeStateHandle ObjectChangeState;


        /// <MetaDataID>{a5bb9008-1509-44ac-961c-170a742ba163}</MetaDataID>
        string SignInProvider { get; set; }

        /// <MetaDataID>{e3960766-524c-4b88-8e0e-a347501a87f1}</MetaDataID>
        string UserIdentity { get; set; }


    }

    /// <MetaDataID>{5ec91d09-d693-4c4e-9dca-858a9e09a233}</MetaDataID>
    public class WaiterPresentation : MarshalByRefObject, INotifyPropertyChanged, IWaiterPresentation, ISecureUser, FlavourBusinessFacade.ViewModel.ILocalization, OOAdvantech.Remoting.IExtMarshalByRefObject, IBoundObject
    {

        public WaiterPresentation()
        {
            this.FlavoursOrderServer = new DontWaitApp.FlavoursOrderServer() { WaiterView = true };
        }


        /// <MetaDataID>{d52da524-96a8-4f0b-ae0f-703be4348ff2}</MetaDataID>
        string lan = OOAdvantech.CultureContext.CurrentNeutralCultureInfo.Name;

        /// <MetaDataID>{0cff47a2-3b96-4019-bfab-e15d448b603f}</MetaDataID>
        public string Language { get { return lan; } }

        

        static IAppLifeTime AppLifeTime
        {
            get
            {
#if DeviceDotNet
                return Application.Current as IAppLifeTime;
#else
                return System.Windows.Application.Current as IAppLifeTime;
#endif
            }
        }

        public static OOAdvantech.SerializeTaskScheduler SerializeTaskScheduler
        {
            get
            {
                return AppLifeTime.SerializeTaskScheduler;
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public event ObjectChangeStateHandle ObjectChangeState;

        public event LaytheTableRequestHandle LayTheTableRequest;

        /// <MetaDataID>{ad37da77-4203-47d2-af7e-094cd499c17a}</MetaDataID>
        string _SignInProvider;
        /// <MetaDataID>{44230b90-69cb-4ff5-9841-bcb77b77de42}</MetaDataID>
        public string SignInProvider
        {
            get
            {
                return _SignInProvider;// ApplicationSettings.Current.SignInProvider;
            }
            set
            {
                _SignInProvider = value;// ApplicationSettings.Current.SignInProvider = value;
            }
        }
        /// <MetaDataID>{6eedef83-ab80-40e4-9185-212aad63e241}</MetaDataID>
        string _Email;
        /// <MetaDataID>{d26cb029-ed3f-4379-9722-7206c981bc32}</MetaDataID>
        public string Email
        {
            get
            {
                return _Email;
            }

            set
            {
                _Email = value;
            }
        }

        /// <MetaDataID>{58575c2e-ccc3-41be-bc9f-c27f33b0813b}</MetaDataID>
        string _FullName;
        /// <MetaDataID>{44faeb89-a15a-4a43-a992-71bed4bbc4da}</MetaDataID>
        public string FullName
        {
            get
            {
                return _FullName;
            }

            set
            {
                _FullName = value;
            }
        }

        /// <MetaDataID>{028887bc-c73a-433c-9cdb-fa58744c7321}</MetaDataID>
        string _Password;
        /// <MetaDataID>{bfc5b50d-9b83-4b4f-8a02-4d6cccb906cf}</MetaDataID>
        public string Password
        {
            get
            {
                return _Password;
            }

            set
            {
                _Password = value;
            }
        }

        /// <MetaDataID>{32de8669-c820-4d42-b1b6-513d2e268af6}</MetaDataID>
        string _ConfirmPassword;
        /// <MetaDataID>{80ba6caf-cdfd-4f0d-bfb3-2aeeaf3c83e7}</MetaDataID>
        public string ConfirmPassword
        {
            get
            {
                return _ConfirmPassword;
            }

            set
            {
                _ConfirmPassword = value;
            }
        }

        /// <MetaDataID>{57b07489-fa2b-4fce-94a6-dbb348583514}</MetaDataID>
        string _UserName;
        /// <MetaDataID>{d9aa7cb2-c431-409e-8a4b-860e48be357b}</MetaDataID>
        public string UserName
        {
            get
            {
                return _UserName;
            }
            set
            {
                _UserName = value;

            }
        }


        /// <MetaDataID>{207007a7-cf74-4413-b852-e0e780f36ada}</MetaDataID>
        string _UserIdentity;
        /// <MetaDataID>{06310bcd-8a04-480a-a42c-fdcd16e6be97}</MetaDataID>
        public string UserIdentity
        {
            get
            {
                return _UserIdentity;// ApplicationSettings.Current.SignInUserIdentity;
            }
            set
            {
                _UserIdentity = value;// ApplicationSettings.Current.SignInUserIdentity = value;
            }
        }

        /// <MetaDataID>{e034ad7c-99e5-4ef5-b1e6-9b920f95cf86}</MetaDataID>
        UserData UserData;
        /// <MetaDataID>{c025d0d0-aac4-40e2-b9cf-71dfac467296}</MetaDataID>
        AuthUser AuthUser;
        /// <MetaDataID>{a92261fd-5478-4e86-bd09-4c3ada5a47f2}</MetaDataID>
        public bool OnSignIn;

        //static string _AzureServerUrl = "http://localhost:8090/api/";
        //static string _AzureServerUrl = "http://192.168.2.8:8090/api/";
        //static string _AzureServerUrl = "http://192.168.2.5:8090/api/";

        static string _AzureServerUrl = "http://192.168.2.8:8090/api/";//org
        //static string _AzureServerUrl = "http://192.168.2.4:8090/api/";//Braxati
        //static string _AzureServerUrl = "http://10.0.0.13:8090/api/";//work


        /// <MetaDataID>{23fa1561-877f-4ca5-966a-f2bf40742e9f}</MetaDataID>
        static string AzureServerUrl
        {
            get
            {
                string azureStorageUrl = null;// OOAdvantech.Remoting.RestApi.RemotingServices.GetLocalIPAddress();
                if (azureStorageUrl == null)
                    azureStorageUrl = _AzureServerUrl;
                else
                    azureStorageUrl = "http://" + azureStorageUrl + ":8090/api/";

                return azureStorageUrl;
            }
        }


        /// <MetaDataID>{f55963f0-34db-4fb9-b9ef-14d064de47ec}</MetaDataID>
        string _PhoneNumber;
        private IWaiter Waiter;


        IList<IHallLayout> _Halls;
        public IList<IHallLayout> Halls
        {
            get
            {
                return _Halls;
                //if (this.InActiveShiftWork)
                //    return _Halls;
                //else
                //    return new List<IHallLayout>();
            }
        }

        /// <MetaDataID>{fac71003-9d13-430a-880e-956874e37449}</MetaDataID>
        [OOAdvantech.MetaDataRepository.HttpVisible]
        public string PhoneNumber
        {
            get
            {
                return _PhoneNumber;
            }
            set
            {
                _PhoneNumber = value;
            }
        }

        /// <MetaDataID>{4d660847-77de-4c66-bf0c-fe41b5da3cdc}</MetaDataID>
        public bool IsActiveWaiter => Waiter != null;


        Task<bool> SignInTask;

        /// <MetaDataID>{0b631e28-fc5c-46ab-85c1-944ce7ead3eb}</MetaDataID>
        [OOAdvantech.MetaDataRepository.HttpVisible]
        public async Task<bool> SignIn()
        {

            //System.IO.File.AppendAllLines(App.storage_path, new string[] { "SignIn " });
            System.Diagnostics.Debug.WriteLine("public async Task< bool> SignIn()");
            AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
            if (authUser == null)
            {
                authUser = DeviceAuthentication.AuthUser;
            }
            if (DeviceAuthentication.AuthUser == null)
            {

            }
            if (authUser == null)
            {
#if DeviceDotNet
#if DEBUG
                if (Device.RuntimePlatform == Device.iOS)
                {
                    //iOS stuff
                    return await IOSPseudoSignIn();
                }
#endif
#endif
                return false;
            }
            if (AuthUser != null && authUser.User_ID == AuthUser.User_ID)
            {
                GetMessages();
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
                    string assemblyData = "FlavourBusinessManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
                    string type = "FlavourBusinessManager.AuthFlavourBusiness";// typeof(FlavourBusinessManager.AuthFlavourBusiness).FullName;
                    System.Runtime.Remoting.Messaging.CallContext.SetData("AutUser", authUser);
                    string serverUrl = "http://localhost/FlavourBusinessWebApiRole/api/";
                    serverUrl = "http://localhost:8090/api/";
                    serverUrl = AzureServerUrl;


                    if (authUser != null && !string.IsNullOrWhiteSpace(ApplicationSettings.Current.WaiterObjectRef))
                    {
                        if (Waiter != null)
                            Waiter.ObjectChangeState -= Waiter_ObjectChangeState;

                        Waiter = RemotingServices.DerializeObjectRef<IWaiter>(ApplicationSettings.Current.WaiterObjectRef);
                        if (Waiter != null && Waiter.SignUpUserIdentity == authUser.User_ID)
                        {
                            AuthUser = authUser;
                            ActiveShiftWork = Waiter.ActiveShiftWork;
                            this._Halls = Waiter.GetServiceHalls();
                            foreach (var hall in this._Halls)
                            {
                                hall.FontsLink = "https://angularhost.z16.web.core.windows.net/graphicmenusresources/Fonts/Fonts.css";
                                (hall as RestaurantHallLayoutModel.HallLayout).SetShapesImagesRoot("https://angularhost.z16.web.core.windows.net/halllayoutsresources/Shapes/");
                            }



                            Waiter.ObjectChangeState += Waiter_ObjectChangeState;
                            Waiter.MessageReceived += MessageReceived;

#if DeviceDotNet
                            IDeviceOOAdvantechCore device = DependencyService.Get<IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
                            Waiter.DeviceFirebaseToken = device.FirebaseToken;
#endif
                            (this.FlavoursOrderServer as FlavoursOrderServer).CurrentUser = Waiter;
                            ApplicationSettings.Current.FriendlyName = Waiter.FullName;
                            GetMessages();


                            return true;

                        }

                    }

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


                    //sds.SendTimeout
                    //OOAdvantech.Remoting.RestApi.RemotingServices.T
                    authUser = DeviceAuthentication.AuthUser;
                    if (DeviceAuthentication.AuthUser == null)
                    {

                    }
                    if (authUser == null)
                    {

                    }

                    //New-SelfSignedCertificate -certstorelocation cert:\localmachine\my -dnsname www.mywebsite.com
                    authUser = DeviceAuthentication.AuthUser;
                    UserData = pAuthFlavourBusiness.SignIn();
                    if (UserData != null)
                    {
                        var role = UserData.Roles.Where(x => x.RoleType == UserData.RoleType.Waiter).FirstOrDefault();
                        if (role.RoleType == UserData.RoleType.Waiter)
                        {
                            if (Waiter != null)
                                Waiter.ObjectChangeState -= Waiter_ObjectChangeState;
                            Waiter = RemotingServices.CastTransparentProxy<IWaiter>(role.User);

                            string objectRef = RemotingServices.SerializeObjectRef(Waiter);
                            DontWaitApp.ApplicationSettings.Current.WaiterObjectRef = objectRef;

                            Waiter.ObjectChangeState += Waiter_ObjectChangeState;
                            Waiter.MessageReceived += MessageReceived;

#if DeviceDotNet
                            IDeviceOOAdvantechCore device = DependencyService.Get<IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;

                            Waiter.DeviceFirebaseToken = device.FirebaseToken;
#endif



                            ActiveShiftWork = Waiter.ActiveShiftWork;
                            (this.FlavoursOrderServer as FlavoursOrderServer).CurrentUser = Waiter;
                            ApplicationSettings.Current.FriendlyName = Waiter.FullName;
                            this._Halls = Waiter.GetServiceHalls();
                            foreach (var hall in this._Halls)
                            {
                                hall.FontsLink = "https://angularhost.z16.web.core.windows.net/graphicmenusresources/Fonts/Fonts.css";
                                (hall as RestaurantHallLayoutModel.HallLayout).SetShapesImagesRoot("https://angularhost.z16.web.core.windows.net/halllayoutsresources/Shapes/");
                            }

                            GetMessages();
                        }
                        //https://angularhost.z16.web.core.windows.net/halllayoutsresources/Shapes/DiningTableChairs020.svg

                        //role = UserData.Roles.Where(x => x.RoleType == UserData.RoleType.Organization).FirstOrDefault();
                        //if (role.RoleType == UserData.RoleType.Organization)
                        //{
                        //    Organization = RemotingServices.CastTransparentProxy<IOrganization>(role.User);
                        //    string administratorIdentity = "";


                        //    if (ServiceContextSupervisor != null)
                        //    {
                        //        administratorIdentity = ServiceContextSupervisor.SupervisorIdentity;
                        //        var flavoursServicesContext = Organization.GetFlavoursServicesContext(ServiceContextSupervisor.ServicesContextIdentity);
                        //        ServiceContextSupervisors[ServiceContextSupervisor.ServicesContextIdentity] = flavoursServicesContext.ServiceContextHumanResources.Supervisors.Where(x => x.SignUpUserIdentity != ServiceContextSupervisor.SignUpUserIdentity).ToList();
                        //    }
                        //    _ServicesContexts = Organization.ServicesContexts.Select(x => new ServicesContextPresentation(x, administratorIdentity)).OfType<IServicesContextPresentation>().ToList();
                        //}
                        //else
                        //    _ServicesContexts = new List<IServicesContextPresentation>();

                        AuthUser = authUser;
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

        async Task<bool> IOSPseudoSignIn()
        {

            //System.IO.File.AppendAllLines(App.storage_path, new string[] { "SignIn " });
            System.Diagnostics.Debug.WriteLine("public async Task< bool> SignIn()");
            AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
            if (authUser == null)
            {
                authUser = DeviceAuthentication.AuthUser;
            }
            if (DeviceAuthentication.AuthUser == null)
            {

            }

            if (AuthUser != null && authUser.User_ID == AuthUser.User_ID)
                return true;

            if (OnSignIn && SignInTask != null)
                return await SignInTask;
            else
            {

                SignInTask = Task<bool>.Run(async () =>
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


                        UserData = pAuthFlavourBusiness.SignInUser("aTUJ9abSzveGlWBmwLB4VbxBIw62");
                        if (UserData != null)
                        {
                            var role = UserData.Roles.Where(x => x.RoleType == UserData.RoleType.Waiter).FirstOrDefault();
                            if (role.RoleType == UserData.RoleType.Waiter)
                            {
                                if (Waiter != null)
                                    Waiter.ObjectChangeState -= Waiter_ObjectChangeState;

                                Waiter = RemotingServices.CastTransparentProxy<FlavourBusinessFacade.HumanResources.IWaiter>(role.User);
                                Waiter.ObjectChangeState += Waiter_ObjectChangeState;

                                Waiter.MessageReceived += MessageReceived;
                                ActiveShiftWork = Waiter.ActiveShiftWork;
                                (this.FlavoursOrderServer as FlavoursOrderServer).CurrentUser = Waiter;
                                ApplicationSettings.Current.FriendlyName = Waiter.FullName;
                                this._Halls = Waiter.GetServiceHalls();
                                foreach (var hall in this._Halls)
                                {
                                    hall.FontsLink = "https://angularhost.z16.web.core.windows.net/graphicmenusresources/Fonts/Fonts.css";
                                    (hall as RestaurantHallLayoutModel.HallLayout).SetShapesImagesRoot("https://angularhost.z16.web.core.windows.net/halllayoutsresources/Shapes/");
                                }

                                GetMessages();
                            }
                            //https://angularhost.z16.web.core.windows.net/halllayoutsresources/Shapes/DiningTableChairs020.svg

                            //role = UserData.Roles.Where(x => x.RoleType == UserData.RoleType.Organization).FirstOrDefault();
                            //if (role.RoleType == UserData.RoleType.Organization)
                            //{
                            //    Organization = RemotingServices.CastTransparentProxy<IOrganization>(role.User);
                            //    string administratorIdentity = "";


                            //    if (ServiceContextSupervisor != null)
                            //    {
                            //        administratorIdentity = ServiceContextSupervisor.SupervisorIdentity;
                            //        var flavoursServicesContext = Organization.GetFlavoursServicesContext(ServiceContextSupervisor.ServicesContextIdentity);
                            //        ServiceContextSupervisors[ServiceContextSupervisor.ServicesContextIdentity] = flavoursServicesContext.ServiceContextHumanResources.Supervisors.Where(x => x.SignUpUserIdentity != ServiceContextSupervisor.SignUpUserIdentity).ToList();
                            //    }
                            //    _ServicesContexts = Organization.ServicesContexts.Select(x => new ServicesContextPresentation(x, administratorIdentity)).OfType<IServicesContextPresentation>().ToList();
                            //}
                            //else
                            //    _ServicesContexts = new List<IServicesContextPresentation>();

                            AuthUser = authUser;
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
        private void MessageReceived(IMessageConsumer sender)
        {
            GetMessages();
        }

        private void Waiter_ObjectChangeState(object _object, string member)
        {
            if (member == nameof(IServicesContextWorker.ActiveShiftWork))
            {
                ObjectChangeState?.Invoke(this, nameof(ActiveShiftWorkStartedAt));

                GetMessages();
            }

        }


        /// <MetaDataID>{d38d4827-a9a7-48bb-b272-1f897c86cf1b}</MetaDataID>
        [OOAdvantech.MetaDataRepository.HttpVisible]
        public void SignOut()
        {
            UserData = new UserData();
            AuthUser = null;
            //Organization = null;
            //ServiceContextSupervisor = null;
            //_ServicesContexts.Clear();
        }


        /// <MetaDataID>{8bb16524-ef68-4a38-94f4-71776a04d4d5}</MetaDataID>
        [OOAdvantech.MetaDataRepository.HttpVisible]
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
                        pAuthFlavourBusiness = remoteObject as IAuthFlavourBusiness;

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
                    UserData = new UserData() { Email = this.Email, FullName = this.FullName, PhoneNumber = this.PhoneNumber };
                    UserData = pAuthFlavourBusiness.SignUp(UserData);

                    if (UserData != null)
                    {
                        //var role = UserData.Roles.Where(x => x.RoleType == UserData.RoleType.ServiceContextSupervisor).FirstOrDefault();
                        //if (role.RoleType == UserData.RoleType.ServiceContextSupervisor)
                        //    ServiceContextSupervisor = RemotingServices.CastTransparentProxy<IServiceContextSupervisor>(role.User);

                        //role = UserData.Roles.Where(x => x.RoleType == UserData.RoleType.Organization).FirstOrDefault();
                        //if (role.RoleType == UserData.RoleType.Organization)
                        //{
                        //    string administratorIdentity = "";
                        //    if (ServiceContextSupervisor != null)
                        //        administratorIdentity = ServiceContextSupervisor.SupervisorIdentity;

                        //    Organization = RemotingServices.CastTransparentProxy<IOrganization>(role.User);
                        //    _ServicesContexts = Organization.ServicesContexts.Select(x => new ServicesContextPresentation(x, administratorIdentity)).OfType<IServicesContextPresentation>().ToList();
                        //}
                        //else
                        //    _ServicesContexts = new List<IServicesContextPresentation>();

                        //if(Organization!=null&& ServiceContextSupervisor!=null)
                        //{
                        //    var serviceContex= Organization.GetFlavoursServicesContext(ServiceContextSupervisor.ServicesContextIdentity);
                        //    serviceContex.ObjectChangeState
                        //}
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

        /// <MetaDataID>{187d772f-a845-4262-9f4a-1efed97515fe}</MetaDataID>
        public async Task<bool> AssignWaiter()
        {
            return await Assign();
        }
#if DeviceDotNet
        public DeviceUtilities.NetStandard.ScanCode ScanCode = new DeviceUtilities.NetStandard.ScanCode();
#endif
        /// <MetaDataID>{9921c079-439e-4f42-8b4d-44130647d4b1}</MetaDataID>
        public async Task<bool> Assign()
        {
#if DeviceDotNet
            string waiterAssignKey = null;
            try
            {
                var result = await ScanCode.Scan("Hold your phone up to the place Identity", "Scanning will happen automatically");

                if (result == null || string.IsNullOrWhiteSpace(result.Text))
                    return false;
                waiterAssignKey = result.Text;
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
                this.Waiter = servicesContextManagment.AssignWaiterUser(waiterAssignKey);
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
            return true;
#endif

        }


        public DontWaitApp.IFlavoursOrderServer FlavoursOrderServer { get; private set; }


        IShiftWork ActiveShiftWork;

        public bool InActiveShiftWork
        {
            get
            {
                if (ActiveShiftWork != null)
                {
                    var startedAt = ActiveShiftWork.StartsAt;
                    var workingHours = ActiveShiftWork.PeriodInHours;

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

        public string AppIdentity => "com.microneme.dontwaitwaiterapp";

        public void SiftWorkStart(DateTime startedAt, double timespanInHours)
        {
            ActiveShiftWork = Waiter.NewShiftWork(startedAt, timespanInHours);
            GetMessages();
        }

        object MessagesLock = new object();
        private void GetMessages()
        {
            lock (MessagesLock)
            {
                if (Waiter != null)
                {
                    var message = Waiter.PeekMessage();
                    if (message != null && message.GetDataValue<ClientMessages>("ClientMessageType") == ClientMessages.LaytheTable)
                    {
                        if ((DateTime.UtcNow - message.MessageTimestamp.ToUniversalTime()).TotalMinutes > 20)
                            Waiter.RemoveMessage(message.MessageID);
                        else
                        {
                            if (message != null && InActiveShiftWork)
                            {

                                string servicesPointIdentity = message.GetDataValue<string>("ServicesPointIdentity");
                                LayTheTableRequest?.Invoke(this, message.MessageID, servicesPointIdentity);
                                //PartOfMealRequestMessageForward(message);
                                return;
                            }
                        }
                    }
                }
            }
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
                        //if (this.FoodServiceClientSession != null)
                        //    this.FoodServiceClientSession.DeviceResume();
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
                        //if (this.FoodServiceClientSession != null)
                        //    this.FoodServiceClientSession.DeviceSleep();
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



        public void ExtendSiftWorkStart(double timespanInHours)
        {
            throw new NotImplementedException();
        }

        public MarshalByRefObject GetObjectFromUri(string uri)
        {
            return FlavoursOrderServer as MarshalByRefObject;
        }
        public string GetString(string langCountry, string key)
        {
            JObject jObject = null;
            if (!Translations.TryGetValue(langCountry,out jObject))
            {
                GetTranslation(langCountry);
                jObject = Translations[langCountry];
                
            }

            var keyParts = key.Split('.');
            int i = 0;
            foreach (string member in keyParts)
            {
                if (jObject == null)
                    return null;
                JToken jToken = null;
                if (i == keyParts.Length - 1)
                {
                    if (jObject.TryGetValue(member, out jToken))
                    {
                        if (jToken is JValue)
                            return (jToken as JValue).Value as string;
                    }
                    return null;
                }

                if (jObject.TryGetValue(member, out jToken))
                {
                    jObject = jToken as JObject;

                }
                else
                    return null;
                i++;
            }

            return null;
        }


        public void SetString(string langCountry, string key, string newValue)
        {
            JObject jObject = null;
            if (!Translations.TryGetValue(langCountry, out jObject))
            {
                GetTranslation(langCountry);
                jObject = Translations[langCountry];

            }

            var keyParts = key.Split('.');
            int i = 0;
            foreach (string member in keyParts)
            {
                if (jObject == null)
                    return ;
                JToken jToken = null;
                if (i == keyParts.Length - 1)
                {
                    if (jObject.TryGetValue(member, out jToken))
                    {
                        if (jToken is JValue)
                            (jToken as JValue).Value = newValue;
                    }
                    else
                        jObject.Add(member, new JValue(newValue));
                }
                else
                {
                    if (jObject.TryGetValue(member, out jToken))
                        jObject = jToken as JObject;
                    else
                    {
                        jObject.Add(member, new JObject());
                        jObject = jObject[member] as JObject;
                    }
                }
                i++;
            }

        }

        Dictionary<string, JObject> Translations = new Dictionary<string, JObject>();

        public string GetTranslation(string langCountry)
        {
            if (Translations.ContainsKey(langCountry))
                return Translations[langCountry].ToString();
            string json = "{}";
            var assembly = Assembly.GetExecutingAssembly();
            string jsonName = assembly.GetManifestResourceNames().Where(x => x.Contains("i18n") && x.Contains(langCountry + ".json")).FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(jsonName))
            {
                using (var reader = new System.IO.StreamReader(assembly.GetManifestResourceStream(jsonName), Encoding.UTF8))
                {
                    json = reader.ReadToEnd();
                    Translations[langCountry]= JObject.Parse(json);
                    // Do something with the value
                }
            }
            return json;

        }
    }
}
