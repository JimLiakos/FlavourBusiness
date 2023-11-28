using DontWaitApp;
using FlavourBusinessFacade;
using FlavourBusinessFacade.EndUsers;
using FlavourBusinessFacade.HumanResources;
using FlavourBusinessFacade.RoomService;
using FlavourBusinessFacade.ServicesContextResources;
using FlavourBusinessFacade.Shipping;
using FlavourBusinessFacade.ViewModel;
using FlavourBusinessManager.RoomService;

using OOAdvantech;

using OOAdvantech.Remoting.RestApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIBaseEx;
using OOAdvantech.Json.Linq;
using FinanceFacade;
using ServiceContextManagerApp;
using MenuPresentationModel.MenuCanvas;
using System.Windows.Media;


//using static QRCoder.PayloadGenerator;
#if DeviceDotNet
using Xamarin.Forms;
using DeviceUtilities.NetStandard;
using Xamarin.Essentials;
using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;
#else
using MarshalByRefObject = System.MarshalByRefObject;
using WaiterApp.ViewModel;
using FlavourBusinessManager.HumanResources;
using FlavourBusinessManager.Shipping;

#endif


namespace CourierApp.ViewModel
{



    /// <MetaDataID>{1230a8d4-5e45-4ebb-891e-af3db0b09974}</MetaDataID>
    public class CourierActivityPresentation : MarshalByRefObject, OOAdvantech.Remoting.IExtMarshalByRefObject, ILocalization, IFontsResolver,ICourierActivityPresentation, ISecureUser, IBoundObject, IDevicePermissions
    {
        /// <MetaDataID>{097f34b0-62a3-4cc2-9836-290ed314d154}</MetaDataID>
        public string SignInProvider { get; set; }
        /// <MetaDataID>{de9782b2-5434-43b6-8444-14465451ac48}</MetaDataID> 
        public string OAuthUserIdentity { get; set; }
        /// <MetaDataID>{f9b39c2f-80f4-441b-953e-3bd8ea4b5977}</MetaDataID>
        public string FullName { get; set; }

        /// <MetaDataID>{30f68b8e-f78e-4dfd-908c-add1a754dd37}</MetaDataID>
        public string Address { get; set; }

        /// <MetaDataID>{b8f8b568-1932-46f5-8e78-3a532319adbe}</MetaDataID>
        public string UserName { get; set; }
        /// <MetaDataID>{da63ed1a-a2f2-40d8-abf8-e30b56f0ec3f}</MetaDataID>
        public string Email { get; set; }
        /// <MetaDataID>{b47e1f05-956d-4b85-ac25-05af8bf76818}</MetaDataID>
        public string Password { get; set; }
        /// <MetaDataID>{d063d773-9266-421a-b0b4-09176d190651}</MetaDataID>
        public string ConfirmPassword { get; set; }
        /// <MetaDataID>{97a784a6-6261-4d0b-8d40-dae40ea29521}</MetaDataID>
        public string PhoneNumber { get; set; }

        public event ObjectChangeStateHandle ObjectChangeState;



        /// <MetaDataID>{4c90f0e2-26af-4b28-bae1-e799b9f2e5c9}</MetaDataID>
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
        public bool UseAssignedPaymentTerminal { get; private set; }

        public async Task Pay(FinanceFacade.IPayment payment, PaymentMethod paymentMethod, decimal tipAmount)
        {
            if (payment.State == PaymentState.Completed)
                return;

            if (paymentMethod == FinanceFacade.PaymentMethod.PaymentGateway)
            {
#if DeviceDotNet
                var paymentService = new OOAdvantech.Pay.PaymentService();
                if (await paymentService.Pay(payment, tipAmount, FlavourBusinessFacade.ComputingResources.EndPoint.Server, Device.RuntimePlatform == "iOS"))
                {
                    RemotingServices.InvalidateCacheData(payment as MarshalByRefObject);
                    var state = payment.State;
                    if (state==FinanceFacade.PaymentState.Completed)
                    {
                        System.Diagnostics.Debug.WriteLine("FinanceFacade.PaymentState.Completed");
                    }
                    return;
                }

#endif
                return;
            }
            else if (paymentMethod == FinanceFacade.PaymentMethod.Card)
            {
                if (this.UseAssignedPaymentTerminal)
                {

                    var vivaWalletPos = Xamarin.Forms.DependencyService.Get<VivaWalletPos.IPos>();
                    var paymentData = await vivaWalletPos.ReceivePayment(payment.Amount, tipAmount);

                    payment.CardPaymentCompleted(paymentData.CardType, paymentData.AccountNum, true, paymentData.TransactionID, tipAmount);
                    return;

                }
                else
                {
                    payment.CardPaymentCompleted(null, null, true, null, tipAmount);
                    return;
                }
            }
            else if (paymentMethod == FinanceFacade.PaymentMethod.Cash)
            {
                payment.CashPaymentCompleted(tipAmount);
                return;
            }
            else
                return;

        }

        public IBill GetBill(List<SessionItemPreparationAbbreviation> itemPreparations, string foodShippingIdentity)
        {

            var foodShippingPresentation = this.AssignedFoodShippings.Where(x => x.Identity == foodShippingIdentity).FirstOrDefault();
            var bill = this.Courier.GetBill(itemPreparations, foodShippingPresentation.FoodShipping);
            return bill;// this.Waiter.GetBill(itemPreparations, (foodServicesClientSessionPresentation as FoodServicesClientSessionViewModel).FoodServicesClientSession);
        }

        public CourierActivityPresentation(bool useAssignedPaymentTerminal)
        {
            _FoodShippings = new ViewModelWrappers<IFoodShipping, FoodShippingPresentation>();
            _FoodShippings.OnNewViewModelWrapper += OnNewViewModelWrapper;
            _AssignedFoodShippings = new ViewModelWrappers<IFoodShipping, FoodShippingPresentation>();
            _AssignedFoodShippings.OnNewViewModelWrapper += OnNewViewModelWrapper;
            this.UseAssignedPaymentTerminal = useAssignedPaymentTerminal;
        }

        private void OnNewViewModelWrapper(ViewModelWrappers<IFoodShipping, FoodShippingPresentation> sender, IFoodShipping key, FoodShippingPresentation foodShipping)
        {
            foodShipping.ObjectChangeState += FoodShipping_ObjectChangeState;


        }

        private void FoodShipping_ObjectChangeState(object _object, string member)
        {
            FoodShippingUpdated(_object as FoodShippingPresentation);
        }


        /// <MetaDataID>{7c38008c-e8cc-47cf-b413-45519af70ebc}</MetaDataID>
        IList<UserData> NativeUsers;

        /// <MetaDataID>{0fa03740-e253-41f3-8e34-43a3fb41e7b2}</MetaDataID>
        public IList<UserData> GetNativeUsers()
        {
            lock (this)
            {
                if (NativeUsers != null)
                    return NativeUsers;
            }

            if (string.IsNullOrWhiteSpace(ApplicationSettings.Current.ServiceContextDevice))
                return new List<UserData>();


            IAuthFlavourBusiness pAuthFlavourBusiness = null;

            pAuthFlavourBusiness = GetAuthFlavourBusiness();

            string serviceContextIdentity = ApplicationSettings.Current.ServiceContextDevice;
            List<UserData> nativeUsers = pAuthFlavourBusiness.GetNativeUsers(serviceContextIdentity, RoleType.Courier).ToList();

            lock (this)
            {
                NativeUsers = nativeUsers;
            }




            return nativeUsers; ;
            //return new List<UserData>();
        }

        /// <MetaDataID>{159917af-601d-4c03-8efd-00ff8779e414}</MetaDataID>
        private static IAuthFlavourBusiness GetAuthFlavourBusiness()
        {
            IAuthFlavourBusiness pAuthFlavourBusiness;
            string assemblyData = "FlavourBusinessManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
            string type = "FlavourBusinessManager.AuthFlavourBusiness";// typeof(FlavourBusinessManager.AuthFlavourBusiness).FullName;
            //System.Runtime.Remoting.Messaging.CallContext.SetData("AutUser", authUser);
            string serverUrl = "http://localhost/FlavourBusinessWebApiRole/api/";
            serverUrl = "http://localhost:8090/api/";
            serverUrl = AzureServerUrl;

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

            return pAuthFlavourBusiness;
        }

        /// <MetaDataID>{460a1ef3-009d-4642-a527-7a835664b746}</MetaDataID>
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

        /// <MetaDataID>{5a7c3ea7-ce94-40a0-8138-d9188a22ebd6}</MetaDataID>
        public void SaveUserProfile()
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{a1e8e1b1-10b2-434a-91ef-8d1262c39bd5}</MetaDataID>
        public void SendVerificationEmail(string emailAddress)
        {
            throw new NotImplementedException();
        }

        ICourierPresentation _CourierPresentation;
        public ICourierPresentation CourierPresentation
        {
            get
            {
                if (InActiveShiftWork)
                {
                    if (_CourierPresentation==null)
                        _CourierPresentation= new CourierPresentation(_Courier, null);
                    return _CourierPresentation;
                }
                else
                    return null;
            }
        }
        /// <MetaDataID>{ba70c705-d7e0-478d-a619-a86934e80a34}</MetaDataID>
        public ICourier Courier => _Courier;

        /// <MetaDataID>{b6d6d245-a984-4aa0-b2c2-3a9d96a39aa4}</MetaDataID>
        ICourier _Courier;
        /// <MetaDataID>{feb97af6-fab9-4f85-b5a9-f234ff036b54}</MetaDataID>
        AuthUser AuthUser;
        /// <MetaDataID>{7c356551-9dd0-4247-a6b9-3a68914d8881}</MetaDataID>
        private Task<bool> SignInTask;
        /// <MetaDataID>{bd780d43-f60b-4b83-92cc-38c769c2d505}</MetaDataID>
        private bool OnSignIn;

        /// <MetaDataID>{6411a2df-99bb-4f14-bd82-26ad8e306ed2}</MetaDataID>
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
                    //await Task.Delay(5000);
                    GetMessages();
                });

                ObjectChangeState?.Invoke(this, null);
                if (_Courier != null)
                    OAuthUserIdentity = _Courier.OAuthUserIdentity;
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
                            if (_Courier != null)
                            {
                                _Courier.ObjectChangeState -= Courier_ObjectChangeState;
                                _Courier.MessageReceived -= MessageReceived;
                                _Courier.FoodShippingsChanged -= FoodShippingsChanged;
                                //Courier.ServingBatchesChanged -= ServingBatchesChanged;
                                if (_Courier is ITransparentProxy)
                                    (_Courier as ITransparentProxy).Reconnected -= CourierActivityPresentation_Reconnected;
                            }
                            if (_Courier != null && _Courier.OAuthUserIdentity == authUser.User_ID)
                            {
                                AuthUser = authUser;
                                ActiveShiftWork = _Courier.ActiveShiftWork as IServingShiftWork;
                                if (ActiveShiftWork != null)
                                    UpdateFoodShippings(Courier.GetFoodShippings());
                                _Courier.ObjectChangeState += Courier_ObjectChangeState;
                                _Courier.MessageReceived += MessageReceived;
                                _Courier.FoodShippingsChanged += FoodShippingsChanged;
                                //Courier.ServingBatchesChanged += ServingBatchesChanged;
                                if (_Courier is ITransparentProxy)
                                    (_Courier as ITransparentProxy).Reconnected += CourierActivityPresentation_Reconnected;

#if DeviceDotNet
                                IDeviceOOAdvantechCore device = DependencyService.Get<IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
                                _Courier.DeviceFirebaseToken = device.FirebaseToken;
#endif
                                //ApplicationSettings.Current.FriendlyName = Courier.FullName;
                                GetMessages();

                                OAuthUserIdentity = _Courier.OAuthUserIdentity;
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
                                    if (_Courier != null)
                                    {
                                        _Courier.ObjectChangeState -= Courier_ObjectChangeState;
                                        _Courier.MessageReceived -= MessageReceived;
                                        //Courier.ServingBatchesChanged -= ServingBatchesChanged;
                                        if (_Courier is ITransparentProxy)
                                            (_Courier as ITransparentProxy).Reconnected -= CourierActivityPresentation_Reconnected;
                                    }
                                    _Courier = RemotingServices.CastTransparentProxy<ICourier>(role.User);
                                    if (_Courier == null)
                                        continue;


                                    ActiveShiftWork = _Courier.ActiveShiftWork as IServingShiftWork;
                                    if (ActiveShiftWork != null)
                                    {
                                        _CourierPresentation=new CourierPresentation(_Courier, null);
                                        UpdateFoodShippings(Courier.GetFoodShippings());
                                    }

                                    string objectRef = RemotingServices.SerializeObjectRef(_Courier);
                                    ApplicationSettings.Current.CourierObjectRef = objectRef;
                                    _Courier.ObjectChangeState += Courier_ObjectChangeState;
                                    _Courier.MessageReceived += MessageReceived;
                                    _Courier.FoodShippingsChanged += FoodShippingsChanged;
                                    //Courier.ServingBatchesChanged += ServingBatchesChanged;
                                    if (_Courier is ITransparentProxy)
                                        (_Courier as ITransparentProxy).Reconnected += CourierActivityPresentation_Reconnected;


#if DeviceDotNet
                                    IDeviceOOAdvantechCore device = DependencyService.Get<IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
                                    _Courier.DeviceFirebaseToken = device.FirebaseToken;
                                    if (!device.IsBackgroundServiceStarted)
                                    {
                                        BackgroundServiceState serviceState = new BackgroundServiceState();
                                        device.RunInBackground(new Action(async () =>
                                        {
                                            var message = _Courier.PeekMessage();
                                            _Courier.MessageReceived += MessageReceived;
                                            do
                                            {
                                                System.Threading.Thread.Sleep(1000);

                                            } while (!serviceState.Terminate);

                                            _Courier.MessageReceived -= MessageReceived;
                                            //if (Waiter is ITransparentProxy)
                                            //    (Waiter as ITransparentProxy).Reconnected -= WaiterPresentation_Reconnected;
                                        }), serviceState);
                                    }
#endif
                                    ActiveShiftWork = _Courier.ActiveShiftWork as IServingShiftWork;
                                    GetMessages();
                                }
                            }
                            //https://angularhost.z16.web.core.windows.net/halllayoutsresources/Shapes/DiningTableChairs020.svg


                            AuthUser = authUser;
                            if (_Courier != null)
                                OAuthUserIdentity = _Courier.OAuthUserIdentity;
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

        private void FoodShippingsChanged()
        {
            if (ActiveShiftWork != null)
            {
                GetServingUpdates();
                //UpdateFoodShippings(Courier.GetFoodShippings());
            }

        }

        /// <MetaDataID>{c57f087c-6cb6-4bc6-a6b9-1b46a58a9884}</MetaDataID>
        private void CourierActivityPresentation_Reconnected(object sender)
        {
            GetServingUpdates();


        }

        /// <MetaDataID>{d99357a4-d51b-450e-9dcb-2c2c0f96b6c8}</MetaDataID>
        private void MessageReceived(IMessageConsumer sender)
        {
            try
            {
                GetMessages();
            }
            catch (Exception error)
            {
            }
        }

        /// <MetaDataID>{0989d879-8309-46fc-ba3a-947448f9bfb4}</MetaDataID>
        private void Courier_ObjectChangeState(object _object, string member)
        {
            if (member == nameof(IServicesContextWorker.ActiveShiftWork))
            {
                var activeShiftWork = Courier.ActiveShiftWork;
                if (activeShiftWork != ActiveShiftWork)
                {
                    ActiveShiftWork = activeShiftWork as IServingShiftWork;
                    UpdateFoodShippings(Courier.GetFoodShippings());

                }
                ObjectChangeState?.Invoke(this, nameof(ActiveShiftWorkStartedAt));
                GetMessages();
            }
        }
        public event ItemsReadyToServeRequesttHandle ItemsReadyToServeRequest;

        object MessagesLock = new object();


        /// <MetaDataID>{62127532-eedf-43e7-9726-a9fbcd3e4d7e}</MetaDataID>
        private void GetMessages()
        {
            lock (MessagesLock)
            {
                if (Courier != null)
                {
                    var message = Courier.PeekMessage();
                    if (message != null && message.GetDataValue<ClientMessages>("ClientMessageType") == ClientMessages.ItemsReadyToServe)
                    {
                        if ((DateTime.UtcNow - message.MessageTimestamp.ToUniversalTime()).TotalMinutes > 20)
                            Courier.RemoveMessage(message.MessageID);
                        else
                        {
                            if (message != null && InActiveShiftWork)
                            {
                                GetServingUpdates();

                                string servicesPointIdentity = message.GetDataValue<string>("ServicesPointIdentity");
                                ItemsReadyToServeRequest?.Invoke(this, message.MessageID, servicesPointIdentity);
                                //PartOfMealRequestMessageForward(message);
                                return;
                            }
                        }
                    }

                }
            }
        }



        private void GetServingUpdates()
        {
            List<ItemPreparationAbbreviation> servingItemsOnDevice = (from servingBatch in this.FoodShippings
                                                                      from itemsContext in servingBatch.AllContextsOfPreparedItems
                                                                      from itemPreparation in itemsContext.PreparationItems
                                                                      select new ItemPreparationAbbreviation() { uid = itemPreparation.uid, StateTimestamp = itemPreparation.StateTimestamp }).ToList();

            ServingBatchUpdates servingBatchUpdates = Courier.GetFoodShippingUpdates(servingItemsOnDevice);

            var foodShippings = servingBatchUpdates.ServingBatches.Where(x => !x.IsAssigned).Select(x => RemotingServices.CastTransparentProxy<IFoodShipping>(x)).OfType<IFoodShipping>().ToList();
            foreach (var foodShipping in foodShippings)
            {

                var foodShippingPresentation = _FoodShippings.GetViewModelFor(foodShipping, foodShipping);
                foodShippingPresentation.Update();
                if (_AssignedFoodShippings.ContainsKey(foodShipping))
                    _AssignedFoodShippings.Remove(foodShipping);

            }

            var asignedServingBatches = servingBatchUpdates.ServingBatches.Where(x => x.IsAssigned).Select(x => RemotingServices.CastTransparentProxy<IFoodShipping>(x)).ToList();

            foreach (var assignedFoodShipping in asignedServingBatches)
            {
                var servingBatchPresentation = _AssignedFoodShippings.GetViewModelFor(assignedFoodShipping, assignedFoodShipping);
                servingBatchPresentation.Update();
                if (_FoodShippings.ContainsKey(assignedFoodShipping))
                    _FoodShippings.Remove(assignedFoodShipping);

            }


            if (servingBatchUpdates.RemovedServingItems.Count > 0)
            {
                foreach (var foodShippingPresentation in FoodShippings.ToList())
                {
                    bool allItemsRemoved = true;
                    foreach (var servingItem in from servingItemContext in foodShippingPresentation.AllContextsOfPreparedItems
                                                from servingItem in servingItemContext.PreparationItems
                                                select servingItem)
                    {
                        if (servingBatchUpdates.RemovedServingItems.Where(x => x.uid == servingItem.uid).Count() == 0)
                        {
                            allItemsRemoved = false;
                            break;
                        }
                    }
                    if (allItemsRemoved)
                    {
                        _FoodShippings.Remove(foodShippingPresentation.FoodShipping);
                        foodShippingPresentation.Dispose();
                        foodShippingPresentation.ObjectChangeState -= FoodShipping_ObjectChangeState;
                    }

                }
            }
            ObjectChangeState?.Invoke(this, "FoodShippings");
        }
        public void ItemsReadyToServeMessageReceived(string messageID)
        {
            Courier.RemoveMessage(messageID);
        }

        /// <MetaDataID>{f946afd9-a98d-417d-911e-6c3d43fcedd2}</MetaDataID>
        public UserData SignInNativeUser(string userName, string password)
        {


            var pAuthFlavourBusiness = GetAuthFlavourBusiness();
            string serviceContextIdentity = ApplicationSettings.Current.ServiceContextDevice;

            return pAuthFlavourBusiness.SignInNativeUser(serviceContextIdentity, userName, password);

        }

        /// <MetaDataID>{32595d88-b02e-422c-aebc-0069527fa135}</MetaDataID>
        public void SignOut()
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{1be63b02-8287-4309-8b9f-198f67aca03d}</MetaDataID>
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


        /// <MetaDataID>{34925387-d771-493f-8895-a673afa88129}</MetaDataID>
       public IServingShiftWork ActiveShiftWork { get; private set; }

        /// <MetaDataID>{a8ad8be0-6614-4c17-8f1f-db9b878796ed}</MetaDataID>
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

        /// <MetaDataID>{b1ba1918-b084-42f8-8e3f-a4e3fef869d7}</MetaDataID>
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
   


        /// <MetaDataID>{98a2d3fb-2f90-4cde-9bf2-a400f08f29c0}</MetaDataID>
        public bool InActiveShiftWork
        {
            get
            {
                if (ActiveShiftWork?.IsActive() == true)
                    return true;
                else
                    return false;
            }
        }


        /// <MetaDataID>{e0cae251-d210-41e4-9c12-e3da35bcc002}</MetaDataID>
        public async void ShiftWorkStart(DateTime startedAt, double timespanInHours)
        {
            ActiveShiftWork = _Courier.NewShiftWork(startedAt, timespanInHours) as IServingShiftWork;

            if (ActiveShiftWork != null)
            {
                UpdateFoodShippings(Courier.GetFoodShippings());

                //IDeviceOOAdvantechCore device = Xamarin.Forms.DependencyService.Get<IDeviceInstantiator>().GetDeviceSpecific(typeof(IDeviceOOAdvantechCore)) as IDeviceOOAdvantechCore;
                //_TakeAwaySession = await FlavoursOrderServer.GetFoodServicesClientSessionViewModel(TakeAwayStation.GetUncommittedFoodServiceClientSession(TakeAwayStation.Description, device.DeviceID, FlavourBusinessFacade.DeviceType.Desktop, device.FirebaseToken));
                ObjectChangeState?.Invoke(this, nameof(ActiveShiftWork));
            }


        }


        /// <MetaDataID>{f9f3e407-f92f-4376-8ba6-96dc71a245ae}</MetaDataID>
        static string AzureServerUrl = string.Format("http://{0}:8090/api/", FlavourBusinessFacade.ComputingResources.EndPoint.Server);

        /// <MetaDataID>{a4df9ea8-2a57-4900-b6af-224e4470e13b}</MetaDataID>
        public UserData UserData { get; private set; }

        /// <MetaDataID>{e3c19a77-a9d9-45d6-add9-c5b346042474}</MetaDataID>
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
        /// <MetaDataID>{24b0a336-c5ea-4555-ae0a-080a6bcac807}</MetaDataID>
        public void ShowAppPermissions()
        {
#if DeviceDotNet
            AppInfo.ShowSettingsUI();
#endif
        }
        /// <MetaDataID>{220c4e52-b953-4ef9-bc49-c7a281c7ad4f}</MetaDataID>
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
        /// <MetaDataID>{257f42e6-8d57-413c-9b46-aa784622b389}</MetaDataID>
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
                this._Courier = servicesContextManagment.AssignCourierUser(courierAssignKey);
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

        /// <MetaDataID>{5cc6f81d-6009-486a-be8a-e098239a3f33}</MetaDataID>
        public async Task<UserData> AssignDeviceToNativeUserCourier()
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

                NativeUserSignInData nativeUserData = servicesContextManagment.AssignDeviceToNativeUser(deviceAssignKey);

                ApplicationSettings.Current.ServiceContextDevice = nativeUserData.ServiceContextIdentity;
                //this.Waiter = servicesContextManagment.AssignWaiterUser(waiterAssignKey);
                //type = "FlavourBusinessManager.AuthFlavourBusiness";
                //var remoteObject = RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData);
                //var pAuthFlavourBusiness = RemotingServices.CastTransparentProxy<IAuthFlavourBusiness>(remoteObject);
                //UserData = pAuthFlavourBusiness.SignIn();
                lock (this)
                    NativeUsers = null;


                return new UserData() { Email = nativeUserData.FireBaseUserName, Password = nativeUserData.FireBasePasword };
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

            var deviceAssignKey = "7f9bde62e6da45dc8c5661ee2220a7b0;2418a7c2395e4d96acd7b43e34505682";

            try
            {
                string assemblyData = "FlavourBusinessManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
                string type = "FlavourBusinessManager.FlavoursServicesContextManagment";
                string serverUrl = AzureServerUrl;

                IFlavoursServicesContextManagment servicesContextManagment = RemotingServices.CastTransparentProxy<IFlavoursServicesContextManagment>(OOAdvantech.Remoting.RestApi.RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData));

                NativeUserSignInData nativeUserData = servicesContextManagment.AssignDeviceToNativeUser(deviceAssignKey);


                ApplicationSettings.Current.ServiceContextDevice = nativeUserData.ServiceContextIdentity;

                lock (this)
                    NativeUsers = null;
                return new UserData() { Email = nativeUserData.FireBaseUserName, Password = nativeUserData.FireBasePasword };
            }
            catch (Exception error)
            {
                return null;
            }


            return null;
#endif

        }


        /// <MetaDataID>{320fed93-6710-49d9-a737-cafbc9aa8e2c}</MetaDataID>
        public MarshalByRefObject GetObjectFromUri(string uri)
        {
            return this;
        }
        /// <MetaDataID>{8cbb1e48-7efb-4ef4-9540-d12392c25895}</MetaDataID>
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


        /// <MetaDataID>{5be6b1d5-82d9-4cd6-b0c7-afa7da2a2c26}</MetaDataID>
        public async Task<bool> CheckPermissionsForQRCodeScan()
        {
#if DeviceDotNet

            return (await Xamarin.Essentials.Permissions.CheckStatusAsync<Permissions.Camera>()) == PermissionStatus.Granted;
#else
            return false;
#endif

        }





        private void UpdateFoodShippings(IList<IFoodShipping> allFoodShippings)
        {

            var foodShippings = allFoodShippings.Where(x => !x.IsAssigned).ToList();
            foreach (var servingBatch in foodShippings)
                _FoodShippings.GetViewModelFor(servingBatch, servingBatch);

            var asignedFoodShippings = allFoodShippings.Where(x => x.IsAssigned).ToList();

            foreach (var assignedServingBatch in asignedFoodShippings)
                _AssignedFoodShippings.GetViewModelFor(assignedServingBatch, assignedServingBatch);


            foreach (var servingBatch in _FoodShippings.Keys.Where(x => !foodShippings.Contains(x)).ToList())
            {
                _FoodShippings[servingBatch].Dispose();
                _FoodShippings.Remove(servingBatch);
                servingBatch.ObjectChangeState -= FoodShipping_ObjectChangeState;
            }

            foreach (var assignedFoodShippingh in _AssignedFoodShippings.Keys.Where(x => !asignedFoodShippings.Contains(x)).ToList())
            {
                _AssignedFoodShippings[assignedFoodShippingh].Dispose();
                _AssignedFoodShippings.Remove(assignedFoodShippingh);
                assignedFoodShippingh.ObjectChangeState -= FoodShipping_ObjectChangeState;
            }

            ObjectChangeState?.Invoke(this, nameof(FoodShippings));
        }

        ViewModelWrappers<IFoodShipping, FoodShippingPresentation> _FoodShippings;

        public List<FoodShippingPresentation> FoodShippings => _FoodShippings.Values.OrderBy(x => x.FoodShipping.SortID).ToList();

        ViewModelWrappers<IFoodShipping, FoodShippingPresentation> _AssignedFoodShippings;

        public List<FoodShippingPresentation> AssignedFoodShippings => _AssignedFoodShippings.Values.Where(x => x.State.IsInTheSameOrPreviousState(ItemPreparationState.OnRoad)).OrderBy(x => x.FoodShipping.SortID).ToList();


        /// <MetaDataID>{90227b57-2da0-4165-85c8-518932ba9c49}</MetaDataID>
        public void ExtendShiftWorkStart(double timespanInHours)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AssignFoodShipping(string foodShippingIdentity)
        {
            var foodShipping = FoodShippings.Where(x => x.ServiceBatchIdentity == foodShippingIdentity).FirstOrDefault();

            if (foodShipping != null)
            {

                _AssignedFoodShippings[foodShipping.FoodShipping] = _FoodShippings[foodShipping.FoodShipping];
                _FoodShippings.Remove(foodShipping.FoodShipping);


                return await SerializeTaskScheduler.AddTask(async () =>
                  {
                      int tries = 30;
                      while (tries > 0)
                      {
                          try
                          {
                              this.Courier.AssignFoodShipping(foodShipping.FoodShipping);
                              return true;
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
            else
            {
                var assignedFoodShipping = AssignedFoodShippings.Where(x => x.ServiceBatchIdentity == foodShippingIdentity).FirstOrDefault();
                if (assignedFoodShipping != null)
                    return true;
            }
            return false;
        }

        public void PrintFoodShippingReceipt(string serviceBatchIdentity)
        {
            var servingBatch = AssignedFoodShippings.Where(x => x.ServiceBatchIdentity == serviceBatchIdentity).FirstOrDefault();

            if (servingBatch != null)
            {
                servingBatch.FoodShipping.PrintReceiptAgain();
            }
        }

        public async Task<bool> DeAssignFoodShipping(string foodShippingIdentity)
        {
            var servingBatch = AssignedFoodShippings.Where(x => x.ServiceBatchIdentity == foodShippingIdentity).FirstOrDefault();
            if (servingBatch != null)
            {

                _FoodShippings[servingBatch.FoodShipping] = _AssignedFoodShippings[servingBatch.FoodShipping];
                _AssignedFoodShippings.Remove(servingBatch.FoodShipping);



                return await SerializeTaskScheduler.AddTask(async () =>
                {
                    int tries = 30;
                    while (tries > 0)
                    {
                        try
                        {
                            //any desk 270 237 378
                            this.Courier.DeAssignFoodShipping(servingBatch.FoodShipping);
                            return true;
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
                return true;
            }

            return false;
        }


        public async void PhoneCall(string foodShippingIdentity)
        {
            var foodShipping = AssignedFoodShippings.Where(x => x.Identity == foodShippingIdentity).FirstOrDefault();
            if (foodShipping == null)
                foodShipping = FoodShippings.Where(x => x.Identity == foodShippingIdentity).FirstOrDefault();

            if (foodShipping != null)
            {
#if DeviceDotNet
                PhoneDialer.Open(foodShipping.PhoneNumber);
#endif
            }
        }

        public async void Navigate(string foodShippingIdentity)
        {
            var foodShipping = AssignedFoodShippings.Where(x => x.Identity == foodShippingIdentity).FirstOrDefault();
            if (foodShipping == null)
                foodShipping = FoodShippings.Where(x => x.Identity == foodShippingIdentity).FirstOrDefault();

            if (foodShipping != null)
            {
                try
                {
#if DeviceDotNet
                    await Xamarin.Essentials.Map.OpenAsync(foodShipping.Place.Location.Latitude, foodShipping.Place.Location.Longitude, new MapLaunchOptions() { Name = foodShipping.ClientFullName, NavigationMode = NavigationMode.Driving });
#endif
                }
                catch (Exception error)
                {


                }
            }


        }




        public static OOAdvantech.SerializeTaskScheduler SerializeTaskScheduler
        {
            get
            {
                return AppLifeTime.SerializeTaskScheduler;
            }
        }
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

        public bool CommitFoodShippings()
        {
            SerializeTaskScheduler.AddTask(async () =>
            {
                int tries = 30;
                while (tries > 0)
                {
                    try
                    {
                        Courier.CommitFoodShipings();
                        return true;
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
            return true;
        }

        public void PrintFoodShippingsReceipt(string foodShippingIdentity)
        {
            throw new NotImplementedException();
        }

        internal void FoodShippingUpdated(FoodShippingPresentation foodShippingPresentation)
        {
            if (foodShippingPresentation.ContextsOfPreparedItems == null || foodShippingPresentation.ContextsOfPreparedItems.Count == 0)
            {
                _FoodShippings.Remove(foodShippingPresentation.FoodShipping);
                foodShippingPresentation.Dispose();
                foodShippingPresentation.ObjectChangeState -= FoodShipping_ObjectChangeState;
            }
            var state = foodShippingPresentation.State;


            ObjectChangeState?.Invoke(this, nameof(FoodShippings));
        }




        string lan = "el";// OOAdvantech.CultureContext.CurrentNeutralCultureInfo.Name;

        public string Language { get { return lan; } }

        string deflan = "en";
        public string DefaultLanguage { get { return deflan; } }

        /// <MetaDataID>{78e3b85a-cefd-4a92-aef3-66ad79121d13}</MetaDataID>
        Dictionary<string, JObject> Translations = new Dictionary<string, JObject>();

        /// <MetaDataID>{daa58ef0-acd5-443f-8fde-ebd20b3d3ec0}</MetaDataID>
        public string GetTranslation(string langCountry)
        {
            if (Translations.ContainsKey(langCountry))
                return Translations[langCountry].ToString();
            string json = "{}";
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

#if DeviceDotNet
            string path = "CourierApp.i18n";
#else
            string path = "WaiterApp.WPF.i18n";
#endif

            string jsonName = assembly.GetManifestResourceNames().Where(x => x.Contains(path) && x.Contains(langCountry + ".json")).FirstOrDefault();

            //string jsonName = assembly.GetManifestResourceNames().Where(x => x.Contains("WaiterApp.WPF.i18n") && x.Contains(langCountry + ".json")).FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(jsonName))
            {
                using (var reader = new System.IO.StreamReader(assembly.GetManifestResourceStream(jsonName), System.Text.Encoding.UTF8))
                {
                    json = reader.ReadToEnd();
                    Translations[langCountry] = JObject.Parse(json);
                    // Do something with the value
                }
            }
            return json;

        }

        public string GetString(string langCountry, string key)
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
                    return;
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


        public void FoodShippingDelivered(string foodShippingIdentity)
        {
            var foodShippingResentation = AssignedFoodShippings.Where(x => x.Identity == foodShippingIdentity).FirstOrDefault();

            if (foodShippingResentation != null)
                foodShippingResentation.FoodShipping.Delivered();

        }


        public void FoodShippingReturn(string foodShippingIdentity, string returnReasonIdentity, string customReturnReasonDescription = null)
        {
            var foodShippingResentation = AssignedFoodShippings.Where(x => x.Identity == foodShippingIdentity).FirstOrDefault();

            if (foodShippingResentation!=null)
                foodShippingResentation.FoodShipping.FoodShippingReturn(returnReasonIdentity, customReturnReasonDescription);
        }


        public string AppIdentity => "com.microneme.courierapp";

        Dictionary<string, FontData> Fonts = new Dictionary<string, FontData>();

        public FontData GetFont(string fontUri)
        { 
            FontData fontData;
            if (string.IsNullOrEmpty(fontUri))
                return default(FontData);

            if (!Fonts.TryGetValue(fontUri, out fontData))
            {
                string fontUrl = string.Format("http://{0}:8090/api/MenuModel/Font/{1}", FlavourBusinessFacade.ComputingResources.EndPoint.Server, fontUri);
                using (System.Net.WebClient wc = new System.Net.WebClient())
                {
                    var json = wc.DownloadString(fontUrl);
                    fontData = OOAdvantech.Json.JsonConvert.DeserializeObject<FontData>(json);
                    Fonts[fontUri] = fontData;
                }
            }
            return fontData;
        }

    }

}
