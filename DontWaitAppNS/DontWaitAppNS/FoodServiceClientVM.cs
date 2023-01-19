using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlavourBusinessFacade;
using OOAdvantech.Remoting.RestApi;

using OOAdvantech;
using OOAdvantech.MetaDataRepository;
using FlavourBusinessFacade.EndUsers;
using Xamarin.Forms;
#if DeviceDotNet
using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;
#else
using MarshalByRefObject = System.MarshalByRefObject;
#endif

namespace DontWaitApp
{
    /// <MetaDataID>{ceaf19ab-2b52-45d6-a7f7-5dd4e251ed92}</MetaDataID>
    [OOAdvantech.MetaDataRepository.HttpVisible]
    public class FoodServiceClientVM : MarshalByRefObject, OOAdvantech.Remoting.IExtMarshalByRefObject, FlavourBusinessFacade.ViewModel.ISecureUser
    {
        [GenerateEventConsumerProxy]
        public event ObjectChangeStateHandle ObjectChangeState;

        public string OAuthUserIdentity { get; set; }

        List<OOAdvantech.SIMCardData> SIMCards;

        IFlavoursOrderServer FlavoursOrderServer;
        public FoodServiceClientVM(IFlavoursOrderServer flavoursOrderServer)
        {

            FlavoursOrderServer = flavoursOrderServer;

#if !DeviceDotNet
            var sd = typeof(OOAdvantech.Net.DeviceInstantiator).Assembly.GetCustomAttributes(true);
#endif
            var deviceInstantiator = Xamarin.Forms.DependencyService.Get<OOAdvantech.IDeviceInstantiator>();
            OOAdvantech.IDeviceOOAdvantechCore device = deviceInstantiator.GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
            //_LinePhoneNumber= device.GetLinePhoneNumber(0);

            SIMCards = device.LinesPhoneNumbers.ToList();
            _LinesPhoneNumbers = device.LinesPhoneNumbers.Select(x => x.SIMCardDescription).ToList();

            if(ApplicationSettings.Current?.ClientAsGuest!=null)
            {
                _PhoneNumber=ApplicationSettings.Current?.ClientAsGuest.PhoneNumber;
            }

        }

        string _PhoneNumber;
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
                if ((this.FlavoursOrderServer as FlavoursOrderServer)?.CurrentUser!=null)
                    (this.FlavoursOrderServer as FlavoursOrderServer).CurrentUser.PhoneNumber=_PhoneNumber;
                if(ApplicationSettings.Current.ClientAsGuest!=null&&ApplicationSettings.Current.ClientAsGuest!=(this.FlavoursOrderServer as FlavoursOrderServer)?.CurrentUser)
                    ApplicationSettings.Current.ClientAsGuest.PhoneNumber=value;
            }
        }

        [OOAdvantech.MetaDataRepository.HttpVisible]
        public void OnPageLoaded()
        {
        }
        [OOAdvantech.MetaDataRepository.HttpVisible]
        public void OnPageSizeChanged(double width, double height)
        {
        }

        [OOAdvantech.MetaDataRepository.HttpVisible]
        public string AuthUserIdentity
        {
            get
            {
                return ApplicationSettings.Current.SignInUserIdentity;
            }
            set
            {
                ApplicationSettings.Current.SignInUserIdentity = value;
            }
        }

        [OOAdvantech.MetaDataRepository.HttpVisible]
        public string SignInProvider
        {
            get
            {
                return ApplicationSettings.Current.SignInProvider;
            }
            set
            {
                ApplicationSettings.Current.SignInProvider = value;
            }
        }


        string _Address;
        [OOAdvantech.MetaDataRepository.HttpVisible]
        public string Address
        {
            get
            {
                return _Address;
            }

            set
            {
                _Address = value;
            }
        }




        [OOAdvantech.MetaDataRepository.HttpVisible]
        public void SetMessage(string message)
        {

        }



        //static string AzureServerUrl = "http://192.168.2.8:8090/api/";//org
        //static string AzureServerUrl = "http://192.168.2.4:8090/api/";//Braxati
        //static string AzureServerUrl = "http://10.0.0.13:8090/api/";//work
        static string AzureServerUrl = string.Format("http://{0}:8090/api/", FlavourBusinessFacade.ComputingResources.EndPoint.Server);


        [OOAdvantech.MetaDataRepository.HttpVisible]
        public async Task<bool> SignInOld()
        {
            System.Diagnostics.Debug.WriteLine("public async Task< bool> SignIn()");
            AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
            if (authUser == null)
            {

            }
            if (authUser.Firebase_Sign_in_Provider.ToLower()=="google.com")
                UserName = authUser.Email;

            return await Task<bool>.Run(() =>
            {
                string assemblyData = "FlavourBusinessManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
                string type = "FlavourBusinessManager.AuthFlavourBusiness";// typeof(FlavourBusinessManager.AuthFlavourBusiness).FullName;
                System.Runtime.Remoting.Messaging.CallContext.SetData("AutUser", authUser);
                string serverUrl = "http://localhost/FlavourBusinessWebApiRole/api/";
                serverUrl = "http://localhost:8090/api/";

                serverUrl = AzureServerUrl;
                IAuthFlavourBusiness pAuthFlavourBusines = OOAdvantech.Remoting.RestApi.RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData) as IAuthFlavourBusiness;

                var endUser = pAuthFlavourBusines.SignInEndUser();
                //Organization.CurrentOrganization = organization;

                // organization.ObjectChangeState += Organization_ObjectChangeState;
                _LinePhoneNumber = null;

                if (endUser == null)
                {
                    ObjectChangeState?.Invoke(this, null);
                    return false;
                }
                else
                {
                    _LinePhoneNumber = endUser.SIMCardData.SIMCardDescription;
                    _FullName = endUser.Name;
                    ObjectChangeState?.Invoke(this, null);


                    //_Address = organization.Address;
                    //_PhoneNumber = organization.PhoneNumber;
                    //   organization.Address= _Address;
                    //GetOrgenizationRestMenus(organization as IResourceManager);

                    //SignedIn?.Invoke(this, EventArgs.Empty);
                    return true;
                }


            });

            //pAuthFlavourBusines.SignUpOwner(new OrganizationData() { Email = "jim.liakos@gmail.com", Name = "jim", Trademark = "Liakos" });




        }

        EndUserData EndUserData;

        public IFoodServiceClient FoodServiceClient { get; private set; }
        public bool OnSignIn { get; private set; }
        public Task<bool> SignInTask { get; private set; }

        /// <MetaDataID>{0b631e28-fc5c-46ab-85c1-944ce7ead3eb}</MetaDataID>
        [OOAdvantech.MetaDataRepository.HttpVisible]
        public async Task<bool> SignIn()
        {

            //System.IO.File.AppendAllLines(App.storage_path, new string[] { "SignIn " });
            System.Diagnostics.Debug.WriteLine("public async Task< bool> SignIn()");
            OOAdvantech.Remoting.RestApi.AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as OOAdvantech.Remoting.RestApi.AuthUser;
            if (authUser == null)
                authUser = DeviceAuthentication.AuthUser;

            if (authUser == null)
                return false;
            var ddd = FoodServiceClient?.Identity;
            if (FoodServiceClient != null && FoodServiceClient.OAuthUserIdentity == authUser.User_ID)
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
                        catch (System.Exception error)
                        {
                            throw;
                        }
                        authUser = DeviceAuthentication.AuthUser;

                        FoodServiceClient = pAuthFlavourBusiness.SignInEndUser();
                        if (FoodServiceClient != null)
                        {
                            FullName = FoodServiceClient.FullName;
                            UserName = FoodServiceClient.UserName;
                            Email = FoodServiceClient.Email;
                            if (string.IsNullOrWhiteSpace(UserName))
                                UserName=Email;

                            ApplicationSettings.Current.FriendlyName = FoodServiceClient.FriendlyName;

#if DeviceDotNet
                            IDeviceOOAdvantechCore device = Xamarin.Forms.DependencyService.Get<IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
                            FoodServiceClient.DeviceFirebaseToken = device.FirebaseToken;
#endif
                            (this.FlavoursOrderServer as FlavoursOrderServer).CurrentUser =  FoodServiceClient;
                            (this.FlavoursOrderServer as FlavoursOrderServer).AuthUser = authUser;
                            OAuthUserIdentity = FoodServiceClient.OAuthUserIdentity;
                            ObjectChangeState?.Invoke(this, null);

                            return true;
                        }
                        else
                        {
                            if (string.IsNullOrWhiteSpace(FullName))
                            {
                                FullName = ApplicationSettings.Current.FriendlyName;
                                if (string.IsNullOrWhiteSpace(UserName))
                                    UserName = authUser.Email;
                                ObjectChangeState?.Invoke(this, null);
                            }
                            else if (string.IsNullOrWhiteSpace(UserName))
                            {
                                UserName = authUser.Email;
                                ObjectChangeState?.Invoke(this, null);
                            }


                            return false;
                        }

                    }
                    catch (System.Exception error)
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



        [OOAdvantech.MetaDataRepository.HttpVisible]
        public void SaveUserProfile()
        {
            Task<bool>.Run(() =>
            {
                string assemblyData = "FlavourBusinessManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
                string type = "FlavourBusinessManager.AuthFlavourBusiness";// typeof(FlavourBusinessManager.AuthFlavourBusiness).FullName;
                AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
                string serverUrl = "http://localhost/FlavourBusinessWebApiRole/api/";
                serverUrl = "http://localhost:8090/api/";
                serverUrl = AzureServerUrl;
                IAuthFlavourBusiness pAuthFlavourBusines = OOAdvantech.Remoting.RestApi.RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData) as IAuthFlavourBusiness;
                var selectedSimCard = (from simCard in SIMCards
                                       where simCard.SIMCardDescription == _LinePhoneNumber
                                       select new FlavourBusinessFacade.EndUsers.SIMCardData()
                                       {
                                           SIMCardDescription=simCard.SIMCardDescription,
                                           SIMCardIdentity=simCard.SIMCardIdentity,
                                           SIMCardPhoneNumber=simCard.SIMCardPhoneNumber
                                       }).FirstOrDefault();


                pAuthFlavourBusines.UpdateEndUserProfile(new EndUserData() { Email = this.Email, Name = this.FullName, SIMCard=selectedSimCard });

            });
            //SwitchOnOffPopupView?.Invoke(this, EventArgs.Empty);

        }


        [OOAdvantech.MetaDataRepository.HttpVisible]
        public bool SignUpOld()
        {
            string assemblyData = "FlavourBusinessManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
            string type = "FlavourBusinessManager.AuthFlavourBusiness";// typeof(FlavourBusinessManager.AuthFlavourBusiness).FullName;
            AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
            string serverUrl = "http://localhost/FlavourBusinessWebApiRole/api/";
            serverUrl = "http://localhost:8090/api/";
            serverUrl = AzureServerUrl;
            //var selectedSimCard = (from simCard in SIMCards
            //                       where simCard.SIMCardDescription == _LinePhoneNumber
            //                       select simCard).FirstOrDefault();
            var selectedSimCard = (from simCard in SIMCards
                                   where simCard.SIMCardDescription == _LinePhoneNumber
                                   select new FlavourBusinessFacade.EndUsers.SIMCardData()
                                   {
                                       SIMCardDescription = simCard.SIMCardDescription,
                                       SIMCardIdentity = simCard.SIMCardIdentity,
                                       SIMCardPhoneNumber = simCard.SIMCardPhoneNumber
                                   }).FirstOrDefault();

            IAuthFlavourBusiness pAuthFlavourBusines = OOAdvantech.Remoting.RestApi.RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData) as IAuthFlavourBusiness;
            var endUser = pAuthFlavourBusines.SignUpEndUser(new EndUserData() { Email = this.Email, Name = this.FullName, SIMCard= selectedSimCard });
            //Organization.CurrentOrganization = organization;
            return endUser != null;

        }

        /// <MetaDataID>{8bb16524-ef68-4a38-94f4-71776a04d4d5}</MetaDataID>
        [OOAdvantech.MetaDataRepository.HttpVisible]
        public async Task<bool> SignUp()
        {
            System.Diagnostics.Debug.WriteLine("public async Task< bool> SignIn()");
            AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
            if (authUser == null)
                authUser = DeviceAuthentication.AuthUser;

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
                    catch (System.Exception error)
                    {
                        throw;
                    }

                    if (authUser == null)
                    {

                    }
                    EndUserData = new EndUserData() { Email = authUser.Email, Name = ApplicationSettings.Current.FriendlyName, Identity = authUser.User_ID };
                    FoodServiceClient = pAuthFlavourBusiness.SignUpEndUser(EndUserData);

                    if (FoodServiceClient != null)
                    {
                        FullName = FoodServiceClient.FullName;
                        UserName = FoodServiceClient.UserName;
                        Email = FoodServiceClient.Email;
                        if (string.IsNullOrWhiteSpace(UserName))
                            UserName=Email;

                        ApplicationSettings.Current.FriendlyName = FoodServiceClient.FriendlyName;

#if DeviceDotNet
                        IDeviceOOAdvantechCore device = DependencyService.Get<IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
                        FoodServiceClient.DeviceFirebaseToken = device.FirebaseToken;
#endif
                        (FlavoursOrderServer as FlavoursOrderServer).CurrentUser = FoodServiceClient;
                        (FlavoursOrderServer as FlavoursOrderServer).AuthUser = authUser;
                        ObjectChangeState?.Invoke(this, null);
                        OAuthUserIdentity = FoodServiceClient.OAuthUserIdentity;
                        return true;
                    }
                    else
                        return false;

                }
                catch (System.Exception error)
                {

                    throw;
                }
                finally
                {
                    OnSignIn = false;
                }
            });
        }



        /// <MetaDataID>{d38d4827-a9a7-48bb-b272-1f897c86cf1b}</MetaDataID>
        [OOAdvantech.MetaDataRepository.HttpVisible]
        public void SignOut()
        {
            EndUserData = new EndUserData();
            (FlavoursOrderServer as FlavoursOrderServer).AuthUser = null;
            FoodServiceClient = null;
            FullName = null;
            UserName = null;
            Email = null;
            (FlavoursOrderServer as FlavoursOrderServer).CurrentUser = null;

            ObjectChangeState?.Invoke(this, null);
        }

        /// <exclude>Excluded</exclude>
        string _LinePhoneNumber;

        [OOAdvantech.MetaDataRepository.HttpVisible]
        public string LinePhoneNumber
        {
            get
            {

                //return "";
                return _LinePhoneNumber;
            }
            set
            {
                var selectedSimCard = (from simCard in SIMCards
                                       where simCard.SIMCardDescription == value
                                       select simCard).FirstOrDefault();

                _LinePhoneNumber = value;
            }
        }
        /// <exclude>Excluded</exclude>
        List<string> _LinesPhoneNumbers;

        [OOAdvantech.MetaDataRepository.HttpVisible]
        public List<string> LinesPhoneNumbers
        {
            get
            {
                return _LinesPhoneNumbers;
            }
        }

        string _ConfirmPassword;

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


        string _Email;
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

        /// <exclude>Excluded</exclude>
        string _FullName;
        public string FullName
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_FullName))
                    return _FullName;


                return ApplicationSettings.Current.FriendlyName;
            }
            set
            {
                _FullName = value;
                if (!string.IsNullOrWhiteSpace(value))
                    ApplicationSettings.Current.FriendlyName = value;
            }
        }

        /// <exclude>Excluded</exclude>
        string _Password;
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
        /// <exclude>Excluded</exclude>
        string _UserName;

        public string UserName
        {
            get
            {
                return _UserName;
                //return ApplicationSettings.Current.SignInUserName;
            }
            set
            {
                _UserName = value;
                //ApplicationSettings.Current.SignInUserName = value;
            }
        }
    }
}
