using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlavourBusinessFacade;
using OOAdvantech.Remoting.RestApi;

using OOAdvantech;
using OOAdvantech.MetaDataRepository;



namespace DontWaitApp
{
    /// <MetaDataID>{ceaf19ab-2b52-45d6-a7f7-5dd4e251ed92}</MetaDataID>
    [OOAdvantech.MetaDataRepository.HttpVisible]
    public class FoodServiceClientVM : OOAdvantech.Remoting.IExtMarshalByRefObject, FlavourBusinessFacade.ViewModel.IUser
    {
        [GenerateEventConsumerProxy]
        public event ObjectChangeStateHandle ObjectChanged;

        List<SIMCardData> SIMCards;

        public FoodServiceClientVM()
        {


#if !DeviceDotNet
            var sd = typeof(OOAdvantech.Net.DeviceInstantiator).Assembly.GetCustomAttributes(true);
#endif
            var deviceInstantiator = Xamarin.Forms.DependencyService.Get<OOAdvantech.IDeviceInstantiator>();
            OOAdvantech.IDeviceOOAdvantechCore device = deviceInstantiator.GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
            //_LinePhoneNumber= device.GetLinePhoneNumber(0);

            SIMCards = device.LinesPhoneNumbers.ToList();
            _LinesPhoneNumbers = device.LinesPhoneNumbers.Select(x => x.SIMCardDescription).ToList();

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
        public string UserIdentity
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
        static string AzureServerUrl = "http://10.0.0.13:8090/api/";//work


        [OOAdvantech.MetaDataRepository.HttpVisible]
        public async Task<bool> SignIn()
        {
            System.Diagnostics.Debug.WriteLine("public async Task< bool> SignIn()");
            AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
            if (authUser == null)
            {

            }
            if(authUser.Firebase_Sign_in_Provider.ToLower()=="google.com")
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
                    ObjectChanged?.Invoke(this, null);
                    return false;
                }
                else
                {
                    _LinePhoneNumber = endUser.SIMCardData.SIMCardDescription;
                    _FullName = endUser.Name;
                    ObjectChanged?.Invoke(this, null);


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


                pAuthFlavourBusines.UpdateEndUserProfile(new EndUserData() { Email = this.Email, Name = this.FullName,SIMCard=selectedSimCard });

            });
            //SwitchOnOffPopupView?.Invoke(this, EventArgs.Empty);

        }

        [OOAdvantech.MetaDataRepository.HttpVisible]
        public bool SignUp()
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
            var endUser = pAuthFlavourBusines.SignUpEndUser(new EndUserData() { Email = this.Email, Name = this.FullName,SIMCard= selectedSimCard });
            //Organization.CurrentOrganization = organization;
            return endUser != null;

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
               var selectedSimCard= (from simCard in SIMCards
                 where simCard.SIMCardDescription == value
                 select simCard).FirstOrDefault();
                
                _LinePhoneNumber = value;
            }
        }
        /// <exclude>Excluded</exclude>
        List<string> _LinesPhoneNumbers;

        [OOAdvantech.MetaDataRepository.HttpVisible]
        public List< string> LinesPhoneNumbers
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

        string _FullName;
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

        public string UserName
        {
            get
            {
                return ApplicationSettings.Current.SignInUserName;
            }
            set
            {
                ApplicationSettings.Current.SignInUserName = value;
            }
        }
    }
}
