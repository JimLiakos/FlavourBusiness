using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlavourBusinessFacade;
using FlavourBusinessFacade.HumanResources;
using OOAdvantech;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Remoting.RestApi;

#if DeviceDotNet
using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;
#else
using MarshalByRefObject =System.MarshalByRefObject;
#endif

namespace ServiceContextManagerApp
{
    [OOAdvantech.MetaDataRepository.HttpVisible]
    public interface ISecureUser : FlavourBusinessFacade.ViewModel.IUser
    {
        [GenerateEventConsumerProxy]
        event ObjectChangeStateHandle ObjectChangeState;


        string SignInProvider { get; set; }

        string UserIdentity { get; set; }
    }
    public class ManagerPresenation : MarshalByRefObject, INotifyPropertyChanged, ISecureUser, OOAdvantech.Remoting.IExtMarshalByRefObject
    {

        //static string _AzureServerUrl = "http://localhost:8090/api/";
        //static string _AzureServerUrl = "http://192.168.2.8:8090/api/";
        //static string _AzureServerUrl = "http://192.168.2.7:8090/api/";
        //static string _AzureServerUrl = "http://10.0.0.14:8090/api/";
        static string _AzureServerUrl = "http://192.168.2.8:8090/api/";
        //static string _AzureServerUrl = "http://192.168.2.8:8090/api/";

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


        string _SignInProvider;
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

        string _UserName;
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



        public event PropertyChangedEventHandler PropertyChanged;
        public event ObjectChangeStateHandle ObjectChangeState;

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
        public void SignOut()
        {
            //  OOAdvantech.MetaDataRepository.HttpVisible fff=null;

            //SignedOut?.Invoke(this, EventArgs.Empty);
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
        public bool SignUp()
        {
            return false;
        }

        string _UserIdentity;
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

        public bool OnSignIn;
        [OOAdvantech.MetaDataRepository.HttpVisible]
        public async Task<bool> SignIn()
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
                    IAuthFlavourBusiness pAuthFlavourBusines = null;

                    try
                    {
                        pAuthFlavourBusines = OOAdvantech.Remoting.RestApi.RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData) as IAuthFlavourBusiness;
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

                    if (DeviceAuthentication.AuthUser == null)
                    {

                    }
                    ServiceContextSupervisor = pAuthFlavourBusines.SignInServiceContextSupervisor();


                    // organization.ObjectChangeState += Organization_ObjectChangeState;


                    if (ServiceContextSupervisor == null)
                        return false;
                    else
                        return true;
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

        [OOAdvantech.MetaDataRepository.HttpVisible]
        public void SaveUserProfile()
        {
        }


        IServiceContextSupervisor ServiceContextSupervisor;
    }
}
