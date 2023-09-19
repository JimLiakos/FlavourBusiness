﻿using DontWaitApp;
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
using Xamarin.Forms;

namespace CourierApp.ViewModel
{



    /// <MetaDataID>{1230a8d4-5e45-4ebb-891e-af3db0b09974}</MetaDataID>
    public class CourierActivityPresentation : MarshalByRefObject, OOAdvantech.Remoting.IExtMarshalByRefObject, ICourierActivityPresentation, ISecureUser, IBoundObject
    {
        public string SignInProvider { get; set; }
        public string OAuthUserIdentity { get; set; }
        public string FullName { get; set; }
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

        public IList<UserData> GetNativeUsers()
        {
            throw new NotImplementedException();
            //return this.TakeAwayStation.GetNativeUsers();
            
        }

        public OOAdvantech.Remoting.MarshalByRefObject GetObjectFromUri(string uri)
        {
            throw new NotImplementedException();
        }

        public bool IsUsernameInUse(string username, OOAdvantech.Authentication.SignInProvider signInProvider)
        {
            throw new NotImplementedException();
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
            if (DeviceAuthentication.AuthUser == null)
            {
            }
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
                        if (authUser != null )
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
                                IDeviceOOAdvantechCore device = DependencyService.Get<IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
#if DeviceDotNet
                                TakeAwayCashier.DeviceFirebaseToken = device.FirebaseToken;
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
                            OAuthUserIdentity = UserData.OAuthUserIdentity;

                            foreach (var role in UserData.Roles.Where(x => x.RoleType == RoleType.TakeAwayCashier))
                            {
                                if (role.RoleType == RoleType.TakeAwayCashier)
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
                                    TakeAwayCashier.DeviceFirebaseToken = device.FirebaseToken;
                                    if (!device.IsBackgroundServiceStarted)
                                    {
                                        BackgroundServiceState serviceState = new BackgroundServiceState();
                                        device.RunInBackground(new Action(async () =>
                                        {
                                            var message = TakeAwayCashier.PeekMessage();
                                            TakeAwayCashier.MessageReceived += MessageReceived;
                                            do
                                            {
                                                System.Threading.Thread.Sleep(1000);

                                            } while (!serviceState.Terminate);

                                            TakeAwayCashier.MessageReceived -= MessageReceived;
                                            //if (Waiter is ITransparentProxy)
                                            //    (Waiter as ITransparentProxy).Reconnected -= WaiterPresentation_Reconnected;
                                        }), serviceState);
                                    }
#endif
                                    ActiveShiftWork = Courier.ActiveShiftWork;
                                    //UpdateServingBatches(Courier.GetServingBatches());
                                    

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

        public Task<bool> SignUp()
        {
            throw new NotImplementedException();
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
    }
}
