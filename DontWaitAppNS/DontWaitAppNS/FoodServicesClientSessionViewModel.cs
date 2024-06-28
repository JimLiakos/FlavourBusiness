﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlavourBusinessFacade.EndUsers;
using OOAdvantech.MetaDataRepository;

using OOAdvantech.Remoting.RestApi;
using FlavourBusinessFacade.RoomService;
using FlavourBusinessManager.RoomService;
using OOAdvantech.Transactions;




#if DeviceDotNet
using Xamarin.Forms;
using Xamarin.Essentials;
using ZXing.Net.Mobile.Forms;
using ZXing;
using ZXing.QrCode;
using FlavourBusinessFacade;
using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;
using FlavourBusinessFacade.ServicesContextResources;
using OOAdvantech;
using OOAdvantech.Web;
using OOAdvantech.Pay;
using FinanceFacade;



#else
using FlavourBusinessFacade.ServicesContextResources;
using FlavourBusinessManager.EndUsers;
using FlavourBusinessFacade;




#endif

#if FlavourBusinessDevice
using RestaurantHallLayoutModel;
#else
using HallLayout=System.Object;
#endif 
namespace DontWaitApp
{
    /// <MetaDataID>{5ce0ff0c-1c71-4161-85e4-12150431675e}</MetaDataID>
    [BackwardCompatibilityID("{5ce0ff0c-1c71-4161-85e4-12150431675e}")]
    [Persistent()]
    public class FoodServicesClientSessionViewModel : MarshalByRefObject, IFoodServicesClientSessionViewModel, OOAdvantech.Remoting.IExtMarshalByRefObject
    {


        /// <exclude>Excluded</exclude>
        string _PriceListUrl;

        /// <MetaDataID>{004f98c9-bbe8-46e8-9d33-0b6e95beb073}</MetaDataID>
        [PersistentMember(nameof(_PriceListUrl))]
        [BackwardCompatibilityID("+20")]
        public string PriceListUrl
        {
            get => _PriceListUrl;
            set
            {
                if (_PriceListUrl != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _PriceListUrl = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <exclude>Excluded</exclude>
        DateTime? _ServiceTime;

        /// <MetaDataID>{61c7b65a-a2b4-45ef-adb0-4b9fdcac8f1e}</MetaDataID>
        [PersistentMember(nameof(_ServiceTime))]
        [BackwardCompatibilityID("+15")]
        public DateTime? ServiceTime
        {
            get => _ServiceTime;
            set
            {
                if (_ServiceTime != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        if (value != null)
                            _ServiceTime = value.Value.ToUniversalTime();
                        else
                            _ServiceTime = value;

                        stateTransition.Consistent = true;
                    }

                }
                //var menuData = MenuData;
                //menuData.OrderItems = OrderItems.ToList();
                //MenuData = menuData;

                FlavoursOrderServer.SerializeTaskScheduler.AddTask(async () =>
                {
                    var datetime = DateTime.Now;
                    string timestamp = DateTime.Now.ToLongTimeString() + ":" + datetime.Millisecond.ToString();

                    int tries = 30;
                    while (tries > 0)
                    {
                        if (this._FoodServicesClientSession == null)
                            await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(3));
                        else
                        {

                            try
                            {
                                this._FoodServicesClientSession.SetSessionServiceTime(_ServiceTime);
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
                    }
                    return true;
                });
            }
        }

        /// <exclude>Excluded</exclude>
        PayOptions? _PayOption = PayOptions.PayOnCheckout;


        /// <MetaDataID>{fff2c173-978b-4738-ae9d-6d510622ebc7}</MetaDataID>
        [PersistentMember(nameof(_PayOption))]
        [BackwardCompatibilityID("+16")]
        public PayOptions? PayOption
        {
            get => _PayOption;
            set
            {
                if (_PayOption != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _PayOption = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <MetaDataID>{a1bf2f75-d92f-4180-b98d-61cf23697119}</MetaDataID>
        public List<TipOption> TipOptions
        {
            get
            {
                var isoCurrencySymbol = OrderItems?.FirstOrDefault()?.ISOCurrencySymbol;
                if (string.IsNullOrWhiteSpace(isoCurrencySymbol))
                    isoCurrencySymbol = FlavoursOrderServer.ISOCurrencySymbol;
                return new List<TipOption>() {
                new TipOption() { Amount=0, ISOCurrencySymbol= isoCurrencySymbol },
                new TipOption() { Amount=0.5M, ISOCurrencySymbol= isoCurrencySymbol},
                new TipOption() { Amount=1, ISOCurrencySymbol= isoCurrencySymbol },
                new TipOption() { Amount=1.5M, ISOCurrencySymbol= isoCurrencySymbol },
                new TipOption() { Amount=2, ISOCurrencySymbol= isoCurrencySymbol },
                new TipOption() { Amount=2.5M, ISOCurrencySymbol= isoCurrencySymbol },
                new TipOption() { Amount=3, ISOCurrencySymbol= isoCurrencySymbol },
                new TipOption() { Amount=3.5M, ISOCurrencySymbol= isoCurrencySymbol },
                new TipOption() { Amount=4M, ISOCurrencySymbol= isoCurrencySymbol }};
            }
        }


        /// <MetaDataID>{333a5215-f8ae-4fbe-9c0f-a30a3ea4ac30}</MetaDataID>
        public List<CurrencyOption> CurrencyOptions
        {
            get
            {
                var isoCurrencySymbol = OrderItems?.FirstOrDefault()?.ISOCurrencySymbol;
                if (string.IsNullOrWhiteSpace(isoCurrencySymbol))
                    isoCurrencySymbol = FlavoursOrderServer.ISOCurrencySymbol;
                return new List<CurrencyOption>() {
                new CurrencyOption() { Amount=0.05M, ISOCurrencySymbol= isoCurrencySymbol },
                new CurrencyOption() { Amount=0.1M, ISOCurrencySymbol= isoCurrencySymbol },
                new CurrencyOption() { Amount=0.2M, ISOCurrencySymbol= isoCurrencySymbol },
                new CurrencyOption() { Amount=0.5M, ISOCurrencySymbol= isoCurrencySymbol },                new CurrencyOption() { Amount=1, ISOCurrencySymbol= isoCurrencySymbol },
                new CurrencyOption() { Amount=2, ISOCurrencySymbol= isoCurrencySymbol},
                new CurrencyOption() { Amount=5, ISOCurrencySymbol= isoCurrencySymbol },
                new CurrencyOption() { Amount=10, ISOCurrencySymbol= isoCurrencySymbol },
                new CurrencyOption() { Amount=20, ISOCurrencySymbol= isoCurrencySymbol },
                new CurrencyOption() { Amount=50, ISOCurrencySymbol= isoCurrencySymbol },
                new CurrencyOption() { Amount=100, ISOCurrencySymbol= isoCurrencySymbol },
                new CurrencyOption() { Amount=200, ISOCurrencySymbol= isoCurrencySymbol },
                new CurrencyOption() { Amount=500, ISOCurrencySymbol= isoCurrencySymbol }};
            }
        }

        /// <MetaDataID>{ccaeca74-9ca0-4cbe-aebc-f9d785696fdf}</MetaDataID>
        [PersistentMember]
        [BackwardCompatibilityID("+14")]
        private string DeliveryPlaceJson;


        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;


        /// <MetaDataID>{1d430442-9e08-4282-aab1-0a78cffa06f7}</MetaDataID>
        internal FoodServicesClientSessionViewModel()
        {
        }

        /// <MetaDataID>{2316c59f-16f2-4faf-942b-b0079959f338}</MetaDataID>
        public static FoodServicesClientSessionViewModel GetFoodServicesClientSessionViewModel(IFoodServiceClientSession foodServiceClientSession, IFlavoursOrderServer flavoursOrderServer)
        {
            FoodServicesClientSessionViewModel foodServicesClientSessionViewModel = ApplicationSettings.Current.ActiveSessions.Where(x => x.ClientSessionID == foodServiceClientSession.SessionID).FirstOrDefault();
            var clientSessionData = foodServiceClientSession.ClientSessionData;
            if (foodServicesClientSessionViewModel == null)
            {

                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {
                    foodServicesClientSessionViewModel = new FoodServicesClientSessionViewModel();
                    OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(ApplicationSettings.Current).CommitTransientObjectState(foodServicesClientSessionViewModel);
                    foodServicesClientSessionViewModel.Init(clientSessionData, flavoursOrderServer as FlavoursOrderServer);
                    ApplicationSettings.Current.AddClientSession(foodServicesClientSessionViewModel);
                    stateTransition.Consistent = true;
                }
                ApplicationSettings.Current.DisplayedFoodServicesClientSession = foodServicesClientSessionViewModel;
                return foodServicesClientSessionViewModel;
            }
            else
            {
                ApplicationSettings.Current.DisplayedFoodServicesClientSession = foodServicesClientSessionViewModel;
                foodServicesClientSessionViewModel.Init(clientSessionData, flavoursOrderServer as FlavoursOrderServer);
                return foodServicesClientSessionViewModel;
            }

        }


        /// <MetaDataID>{a5f046d9-c7fa-43f3-8c49-24c32be142f1}</MetaDataID>
        internal void Init(FlavourBusinessFacade.EndUsers.ClientSessionData clientSessionData, DontWaitApp.FlavoursOrderServer flavoursOrderServer)
        {
            FlavoursOrderServer = flavoursOrderServer;
            FoodServicesClientSession = clientSessionData.FoodServiceClientSession;
            ClientSessionToken = clientSessionData.Token;
            if (FoodServicesClientSession.SessionType == SessionType.HomeDelivery)
                _DeliveryPlace = ApplicationSettings.Current.ClientAsGuest.DeliveryPlaces.OfType<FlavourBusinessManager.EndUsers.Place>().Where(x => x.Default).FirstOrDefault();

        }

        /// <MetaDataID>{5428ae4b-3c26-4a24-b187-43f089d168f8}</MetaDataID>
        internal void Init(OrganizationStorageRef menu, FlavoursOrderServer flavoursOrderServer)
        {
            FlavoursOrderServer = flavoursOrderServer;
            MenuName = menu.Name;
            MenuRoot = menu.StorageUrl.Substring(0, menu.StorageUrl.LastIndexOf("/") + 1);
            MenuFile = menu.StorageUrl.Substring(menu.StorageUrl.LastIndexOf("/") + 1);

        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<ItemPreparation> _OrderItems = new OOAdvantech.Collections.Generic.Set<ItemPreparation>();

        /// <MetaDataID>{67a87990-9414-48bc-8d1d-bc20335eb6ea}</MetaDataID>
        [PersistentMember(nameof(_OrderItems))]
        [BackwardCompatibilityID("+13")]
        public List<FlavourBusinessManager.RoomService.ItemPreparation> OrderItems => _OrderItems.ToThreadSafeList();


        /// <exclude>Excluded</exclude>
        SessionType _SessionType;

        /// <MetaDataID>{44cbee97-c7e1-40e1-bcd0-af06664d1732}</MetaDataID>
        [PersistentMember(nameof(_SessionType))]
        [BackwardCompatibilityID("+12")]
        public SessionType SessionType
        {
            get => _SessionType;
            set
            {
                if (_SessionType != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _SessionType = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <MetaDataID>{4ea3afdd-49f4-4466-a7f0-f8af9c15732f}</MetaDataID>
        [PersistentMember]
        [BackwardCompatibilityID("+11")]
        string ServedMealTypesUrisStream;

        /// <MetaDataID>{3141a8af-e238-4b7b-96a0-b9e4f102974d}</MetaDataID>
        public List<string> ServedMealTypesUris
        {
            get
            {
                if (ServedMealTypesUrisStream == null)
                    return new List<string>();

                return ServedMealTypesUrisStream.Split(';').ToList();
            }
            internal set
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    ServedMealTypesUrisStream = "";
                    foreach (var uri in value)
                    {
                        if (string.IsNullOrWhiteSpace(ServedMealTypesUrisStream))
                            ServedMealTypesUrisStream = uri;
                        else
                            ServedMealTypesUrisStream += ";" + uri;

                    }

                    stateTransition.Consistent = true;
                }

            }
        }

        /// <exclude>Excluded</exclude>
        string _DefaultMealTypeUri;

        /// <MetaDataID>{3f2b9ae9-82ad-4b3e-90ed-a9c417051c22}</MetaDataID>
        [PersistentMember(nameof(_DefaultMealTypeUri))]
        [BackwardCompatibilityID("+10")]
        public string DefaultMealTypeUri
        {
            get => _DefaultMealTypeUri;
            set
            {
                if (_DefaultMealTypeUri != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _DefaultMealTypeUri = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        string _FoodServiceClientSessionUri;


        /// <MetaDataID>{884da2a5-7653-4335-86a0-d68ba6f3e5b3}</MetaDataID>
        [PersistentMember(nameof(_FoodServiceClientSessionUri))]
        [BackwardCompatibilityID("+9")]
        public string FoodServiceClientSessionUri
        {
            get => _FoodServiceClientSessionUri;
            set
            {
                if (_FoodServiceClientSessionUri != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _FoodServiceClientSessionUri = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        string _MainSessionID;

        /// <MetaDataID>{d9f5d9ac-a2a5-4dbc-b592-b042d696a42b}</MetaDataID>
        [PersistentMember(nameof(_MainSessionID))]
        [BackwardCompatibilityID("+8")]
        public string MainSessionID
        {
            get => _MainSessionID;
            set
            {
                if (_MainSessionID != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _MainSessionID = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        string _ServicePointIdentity;
        /// <MetaDataID>{af736c3a-101a-4861-8d08-5c7aebc3240c}</MetaDataID>
        [PersistentMember(nameof(_ServicePointIdentity))]
        [BackwardCompatibilityID("+1")]
        public string ServicePointIdentity
        {
            get => _ServicePointIdentity;
            set
            {

                if (_ServicePointIdentity != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ServicePointIdentity = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        string _MenuName;

        /// <MetaDataID>{c2a37f04-cc75-4af0-8cff-e716cc7b733d}</MetaDataID>
        [PersistentMember(nameof(_MenuName))]
        [BackwardCompatibilityID("+2")]
        public string MenuName
        {
            get => _MenuName;
            set
            {
                if (_MenuName != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _MenuName = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <exclude>Excluded</exclude>
        string _MenuFile;


        /// <MetaDataID>{ae56ec83-fb03-4330-bb36-afd078191be7}</MetaDataID>
        [PersistentMember(nameof(_MenuFile))]
        [BackwardCompatibilityID("+3")]
        public string MenuFile
        {
            get => _MenuFile;
            set
            {
                if (_MenuFile != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _MenuFile = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }




        /// <exclude>Excluded</exclude>
        string _MenuRoot;

        /// <MetaDataID>{54a19a1a-6f07-4e80-be81-352a12d88b9c}</MetaDataID>
        [PersistentMember(nameof(_MenuRoot))]
        [BackwardCompatibilityID("+4")]
        public string MenuRoot
        {
            get => _MenuRoot;
            set
            {
                if (_MenuRoot != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _MenuRoot = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }


        /// <exclude>Excluded</exclude>
        string _ClientSessionID;
        /// <MetaDataID>{9f1c50f4-2a52-4c89-a3d6-4c3acecf7a74}</MetaDataID>
        [PersistentMember(nameof(_ClientSessionID))]
        [BackwardCompatibilityID("+5")]
        public string ClientSessionID
        {
            get => _ClientSessionID;
            set
            {
                if (_ClientSessionID != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ClientSessionID = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }



        /// <exclude>Excluded</exclude>
        string _ServicesPointName;
        /// <MetaDataID>{489084e0-abfa-4bc7-970d-59cd80ca09e7}</MetaDataID>
        [PersistentMember(nameof(_ServicesPointName))]
        [BackwardCompatibilityID("+6")]
        public string ServicesPointName
        {
            get => _ServicesPointName;
            set
            {
                if (_ServicesPointName != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ServicesPointName = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

#if DeviceDotNet
        internal void Device_MessageReceived(IRemoteMessage remoteMessage)
        {
            if (remoteMessage.Data.ContainsKey("MessageID") && FoodServicesClientSession != null)
                MessageReceived(FoodServicesClientSession);
        }
#endif


        /// <exclude>Excluded</exclude>
        string _ServicesContextLogo;


        /// <MetaDataID>{5406671a-d0c3-4f26-926c-1901a904bf65}</MetaDataID>
        [PersistentMember(nameof(_ServicesContextLogo))]
        [BackwardCompatibilityID("+7")]
        public string ServicesContextLogo
        {
            get => _ServicesContextLogo;
            set
            {
                if (_ServicesContextLogo != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ServicesContextLogo = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }





        #region Messages

        /// <MetaDataID>{9714f9b4-1950-4bd5-9a10-a482c42d355e}</MetaDataID>
        object MessagesLock = new object();

        Message YouMustDecideMessage;

        /// <MetaDataID>{ca16b127-1d5f-46e5-aac6-633a14ae9794}</MetaDataID>
        internal void GetMessages()
        {  
            lock (MessagesLock)
            {

#if DeviceDotNet
                OOAdvantech.DeviceApplication.Current.Log(new System.Collections.Generic.List<string>() { "In GetMessage" });
#endif
                if (FoodServicesClientSession != null)
                {
#if DeviceDotNet
                    OOAdvantech.DeviceApplication.Current.Log(new System.Collections.Generic.List<string>() { "PeekMessage" });
#endif
                    var message = FoodServicesClientSession.PeekMessage();
                    if (message == null)
                    {
#if DeviceDotNet
                        OOAdvantech.DeviceApplication.Current.Log(new System.Collections.Generic.List<string>() { "there is not message" });
#endif


                    }
                    if (message != null && message.GetDataValue<ClientMessages>("ClientMessageType") == ClientMessages.PartOfMealRequest)
                    {
                        string clientSessionID = message.GetDataValue("ClientSessionID") as string;

#if DeviceDotNet
                        OOAdvantech.DeviceApplication.Current.Log(new System.Collections.Generic.List<string>() { "ClientMessages.PartOfMealRequest" });
#endif

                        var messmate = this.GetCandidateMessmates().Union(this.GetMessmates()).Where(x => x.ClientSessionID == "clientSessionID").FirstOrDefault();
                        if (messmate == null)
                        {
                            GetMessmatesFromServer().Wait();
                            messmate = this.GetCandidateMessmates().Union(this.GetMessmates()).Where(x => x.ClientSessionID == clientSessionID).FirstOrDefault();
                        }
                        this.FlavoursOrderServer.PartOfMealRequestMessageForward(message);
                        return;
                    }

                    if (message != null && message.GetDataValue<ClientMessages>("ClientMessageType") == ClientMessages.MenuItemProposal)
                    {
                        MenuItemProposalMessageForward(message);
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
                    //if(YouMustDecideMessage==null&& _MessmatesWaitForYouToDecide!=null)
                    //{
                    //    YouMustDecideMessage= new Message() { MessageID = Guid.NewGuid().ToString("N") };
                    //    YouMustDecideMessageForward(YouMustDecideMessage);
                    //}


                }

#if DeviceDotNet
                OOAdvantech.DeviceApplication.Current.Log(new System.Collections.Generic.List<string>() { "out GetMessage" });
#endif

            }
        }

        /// <MetaDataID>{4a4ab174-50dd-4220-88c1-cb6e1d925628}</MetaDataID>
        private void YouMustDecideMessageForward(FlavourBusinessFacade.EndUsers.Message message)
        {
            if (message.Notification != null)
            {
                FoodServicesClientSession.RemoveMessage(message.MessageID);
                Task.Run(() =>
                {
                    try
                    {
                        var messmates = (from clientSession in FoodServicesClientSession.GetMealParticipants()
                                         select new Messmate(clientSession, OrderItems)).ToList();

                        messmates = (from messmate in messmates
                                     where !messmate.WaiterSession
                                     from preparationItem in messmate.PreparationItems
                                     where preparationItem.State == ItemPreparationState.Committed
                                     select messmate).Distinct().ToList();

                        while (_MessmatesWaitForYouToDecide == null)
                            System.Threading.Thread.Sleep(1000);

                        _MessmatesWaitForYouToDecide?.Invoke(this, messmates, message.MessageID);
                    }
                    catch (Exception error)
                    {
                    }
                });
            }
        }
         

        FlavourBusinessFacade.EndUsers.Message ShareItemHasChangeMessage;
        /// <MetaDataID>{24360209-3b94-4a93-8d33-364f5c406bae}</MetaDataID>
        private void ShareItemHasChangeMessageForward(FlavourBusinessFacade.EndUsers.Message message)
        {
            if (_SharedItemChanged!=null)
            {
                string itemUid = message.GetDataValue<string>("SharedItemUid");
                string itemOwningSession = message.GetDataValue<string>("ItemOwningSession");
                string itemChangeSession = message.GetDataValue<string>("itemChangeSession");
                Messmate changeItemMessmate = Messmates.Where(x => x.ClientSessionID == itemChangeSession).FirstOrDefault();


                bool isShared = message.GetDataValue<bool>("IsShared");
                List<string> shareInSessions = message.GetDataValue<List<string>>("ShareInSessions");
                FoodServicesClientSessionItemStateChanged(itemUid, itemOwningSession, isShared, shareInSessions);
                _SharedItemChanged?.Invoke(this, changeItemMessmate, itemUid, message.MessageID);
                FoodServicesClientSession.RemoveMessage(message.MessageID);
                GetMessages();
            }
            else
                ShareItemHasChangeMessage = message;
        }

        /// <MetaDataID>{837f1943-f15c-49d8-a50b-2832acab8771}</MetaDataID>
        private void MessageReceived(IMessageConsumer sender)
        {
            GetMessages();
        }
        #endregion


        #region Food services client session events consumers
        /// <MetaDataID>{2784806b-281e-44df-af8d-dd847bb7c8a9}</MetaDataID>
        private void FoodServicesClientSessionItemsStateChanged(System.Collections.Generic.Dictionary<string, FlavourBusinessFacade.RoomService.ItemPreparationState> newItemsState)
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
        /// <MetaDataID>{eaba9b2e-24d1-40c9-a56c-245a47e9c3cb}</MetaDataID>
        private async void FoodServicesClientSessionItemStateChanged(string itemUid, string itemOwnerSession, bool isShared, System.Collections.Generic.List<string> shareInSessions)
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
                ItemPreparation preparationItem = await GetItemFromOwner(itemUid, itemOwnerSession);
                if (preparationItem == null)
                    throw new NullReferenceException("There isn't item to update");
                //updates order items

                if (shareInSessions.Contains(FoodServicesClientSession.SessionID))
                {

                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        var orderItem = _OrderItems.Where(x => x.uid == itemUid).FirstOrDefault();
                        if (orderItem != null)
                            _OrderItems.Remove(orderItem);
                        _OrderItems.Add(preparationItem);

                        if (!OOAdvantech.PersistenceLayer.ObjectStorage.IsPersistent(preparationItem))
                        {
                            var objectStarage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this);
                            objectStarage.CommitTransientObjectState(preparationItem);
                        }


                        stateTransition.Consistent = true;
                    }

                }

                //Updates the messmates which have sessionID among the item shareInSessions.
                foreach (var messmate in (from theMessmate in this.Messmates
                                          where shareInSessions.Contains(theMessmate.ClientSessionID)
                                          select theMessmate))
                {

                    var oldPreparationItem = messmate.PreparationItems.Where(x => x.uid == itemUid).FirstOrDefault();
                    messmate.UpdateMealConversationTimeout();
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



            foreach (var messmate in (from theMessmate in this.Messmates
                                      where theMessmate.HasItemWithUid(itemUid) && !shareInSessions.Contains(theMessmate.ClientSessionID)
                                      select theMessmate))
            {
                messmate.RemovePreparationItem(messmate.PreparationItems.Where(x => x.uid == itemUid).First());
            }
            #endregion

            //var menuData = this.MenuData;
            //menuData.OrderItems = OrderItems.ToList();
            //MenuData = menuData;

            _ObjectChangeState?.Invoke(this, null);



        }

        /// <MetaDataID>{0dce1877-b96f-4558-98b6-66fb704a80f1}</MetaDataID>
        private void FoodServicesClientSessionChangeState(object _object, string member)
        {
            if (member == nameof(IFoodServiceClientSession.FlavourItems))
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _OrderItems.Clear();
                    //foreach (var flavourItem in FoodServiceClientSession.FlavourItems)
                    //    OrderItems[flavourItem.uid] = flavourItem as ItemPreparation;
                    //foreach (var flavourItem in FoodServiceClientSession.SharedItems)
                    //    OrderItems[flavourItem.uid] = flavourItem as ItemPreparation;


                    _OrderItems.AddRange(FoodServicesClientSession.FlavourItems.OfType<ItemPreparation>());
                    _OrderItems.AddRange(FoodServicesClientSession.SharedItems.OfType<ItemPreparation>());

                    foreach (var preparationItem in _OrderItems)
                    {
                        if (!OOAdvantech.PersistenceLayer.ObjectStorage.IsPersistent(preparationItem))
                        {
                            var objectStarage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this);
                            if (objectStarage != null)
                                objectStarage.CommitTransientObjectState(preparationItem);
                        }

                    }
                    stateTransition.Consistent = true;
                }



                //var menuData = this.MenuData;
                //menuData.OrderItems = OrderItems.ToList();
                //MenuData = menuData;

                _ObjectChangeState?.Invoke(this, null);


            }
            else if (member == nameof(IFoodServiceClientSession.MainSession))
            {
                RefreshMessmates();
            }
            else if (member == nameof(IFoodServiceClientSession.ServicePoint))
            {
                var foodServiceClientSessionUri = RemotingServices.GetComputingContextPersistentUri(FoodServicesClientSession);
                FoodServiceClientSessionUri = foodServiceClientSessionUri;
                //var menuData = MenuData;
                //menuData.FoodServiceClientSessionUri = foodServiceClientSessionUri;
                //MenuData = menuData;
                GetFoodServicesClientSessionData(foodServiceClientSessionUri);
            }
            else
                RefreshMessmates();



        }

        /// <MetaDataID>{b2483c93-a9f2-41f9-b223-ea85797d1490}</MetaDataID>
        object ClientSessionLock = new object();
        /// <MetaDataID>{1019ceeb-b902-4e3a-af00-af2b159e0a96}</MetaDataID>
        System.Collections.Generic.Dictionary<string, System.Threading.Tasks.Task<bool>> GetServicePointDataTasks = new Dictionary<string, Task<bool>>();

        /// <summary>
        /// this method gets food services client session  data and synchronize caching data 
        /// </summary>
        /// <param name="foodServicesClientSessionUri">
        /// Defines the Uri of food services client session necessary to access the  FoodServicesClientSession from server
        /// </param>
        /// <returns>
        /// true when device connected to server successfully 
        /// otherwise return false
        /// </returns>
        /// <MetaDataID>{72f72a6c-225e-4775-a059-9aab2871f785}</MetaDataID>
        Task<bool> GetFoodServicesClientSessionData(string foodServicesClientSessionUri)
        {

            lock (ClientSessionLock)
            {
                if (foodServicesClientSessionUri != FoodServiceClientSessionUri)
                {

                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        //ApplicationSettings.Current.LastServicePoinMenuData = default(MenuData);
                        _OrderItems.Clear();
                        this.FoodServicesClientSession = null;

                        stateTransition.Consistent = true;
                    }
                }

                Task<bool> getServicePointDataTask = null;
                GetServicePointDataTasks.TryGetValue(foodServicesClientSessionUri, out getServicePointDataTask);
                if (getServicePointDataTask != null && !getServicePointDataTask.IsCompleted)
                    return getServicePointDataTask; // returns the active task to get service point data

                //There isn't active task.
                //Starts task to get service point data
                getServicePointDataTask = Task<bool>.Run(async () =>
                {

                    try
                    {
                        DateTime start = DateTime.UtcNow;
                        IFoodServiceClientSession foodServiceClientSession = null;
                        bool retry = false;
                        do
                        {

                            try
                            {
                                foodServiceClientSession = RemotingServices.GetPersistentObject<IFoodServiceClientSession>(FlavoursOrderServer.AzureServerUrl, foodServicesClientSessionUri);
                            }
                            catch (System.Net.WebException connectionError)
                            {
                                if (connectionError.Status != System.Net.WebExceptionStatus.ConnectFailure)
                                    throw connectionError;
                                retry = true;
                            }
                            catch (TimeoutException timeoutError)
                            {
                                retry = true;
                            }
                        } while (retry);


                        if (foodServiceClientSession != null && foodServiceClientSession.SessionState != ClientSessionState.Closed)
                        {
                            var clientSessionData = foodServiceClientSession.ClientSessionData;
                            FoodServicesClientSession = clientSessionData.FoodServiceClientSession;
                            ClientSessionToken = clientSessionData.Token;

                        }
                        else
                        {
                            if (ApplicationSettings.Current.DisplayedFoodServicesClientSession == this)
                                this.FlavoursOrderServer.SessionIsNoLongerActive(this);

                            FoodServicesClientSession = null;
                            return true;
                        }
                    }
                    catch (Exception error)
                    {

                        return false;
                    }
                    return true;

                });
                GetServicePointDataTasks[foodServicesClientSessionUri] = getServicePointDataTask;
                return getServicePointDataTask;
            }


        }

        /// <MetaDataID>{64cca5c3-be6b-43ce-a7dd-d3bc2486a79e}</MetaDataID>
        public async Task<bool?> IsActive()
        {

            if (FoodServicesClientSession != null)
                return (bool?)true;
            else
            {
                if (!string.IsNullOrWhiteSpace(FoodServiceClientSessionUri))
                {

                    if (await GetFoodServicesClientSessionData(FoodServiceClientSessionUri))
                    {
                        if (FoodServicesClientSession != null && FoodServicesClientSession.SessionState != ClientSessionState.Closed)
                            return (bool?)true;
                    }
                    else
                        return null;
                }
                return false;
            }
        }




        /// <MetaDataID>{c8a403e0-d804-40a6-8543-4bee48d1319f}</MetaDataID>
        private void FoodServicesClientSessionReconnected(object sender)
        {
            try
            {
                sender = RemotingServices.RefreshCacheData(RemotingServices.CastTransparentProxy<MarshalByRefObject>(sender));
            }
            catch (OOAdvantech.Remoting.MissingServerObjectException)
            {

#if DeviceDotNet
                OOAdvantech.DeviceApplication.Current.Log(new List<string> { "MissingServerObjectException" });
#endif

                FoodServicesClientSession = null;
                FlavoursOrderServer.SessionIsNoLongerActive(this);
                return;
            }

            var foodServiceClientSession = RemotingServices.CastTransparentProxy<IFoodServiceClientSession>(sender);


#if DeviceDotNet
            OOAdvantech.DeviceApplication.Current.Log(new List<string> { "befor order items" });
#endif

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _OrderItems.Clear();

                //foreach (var flavourItem in FoodServiceClientSession.FlavourItems)
                //    OrderItems[flavourItem.uid] = flavourItem as ItemPreparation;

                //foreach (var flavourItem in FoodServiceClientSession.SharedItems)
                //    OrderItems[flavourItem.uid] = flavourItem as ItemPreparation;

                _OrderItems.AddRange(FoodServicesClientSession.FlavourItems.OfType<ItemPreparation>());
                _OrderItems.AddRange(FoodServicesClientSession.SharedItems.OfType<ItemPreparation>());
                foreach (var preparationItem in _OrderItems)
                {
                    if (!OOAdvantech.PersistenceLayer.ObjectStorage.IsPersistent(preparationItem))
                    {
                        var objectStarage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this);
                        objectStarage.CommitTransientObjectState(preparationItem);
                    }

                }

                stateTransition.Consistent = true;
            }

#if DeviceDotNet
            OOAdvantech.DeviceApplication.Current.Log(new List<string> { "get ClientSessionData" });
#endif
            var clientSessionData = FoodServicesClientSession.ClientSessionData;
            FoodServicesClientSession = clientSessionData.FoodServiceClientSession;
            ClientSessionToken = clientSessionData.Token;

            //var menuData = this.MenuData;
            //menuData.OrderItems = OrderItems.ToList();
            //MenuData = menuData;


            //foreach (var messmate in Messmates)
            //    messmate.Refresh(OrderItems);
            _ObjectChangeState?.Invoke(this, null);
        }

        #endregion


        /// <MetaDataID>{73f366a8-2d78-456f-aacd-b70114704a4f}</MetaDataID>
        public DontWaitApp.MenuData MenuData
        {
            get
            {
#if DeviceDotNet
                DeviceApplication.Current.Log(new List<string>() { "get_MenuData" });
#endif
                MenuData menuData = new MenuData()
                {
                    MenuName = MenuName,
                    MenuRoot = MenuRoot,
                    MenuFile = MenuFile,
                    PriceListUrl = PriceListUrl,
                    ClientSessionID = ClientSessionID,
                    MainSessionID = MainSessionID,
                    FoodServiceClientSessionUri = FoodServiceClientSessionUri,
                    ServicePointIdentity = ServicePointIdentity,
                    ServicesPointName = ServicesPointName,
                    ServicesContextLogo = ServicesContextLogo,
                    DefaultMealTypeUri = DefaultMealTypeUri,
                    ServedMealTypesUris = ServedMealTypesUris,
                    SessionType = SessionType,
                    OrderItems = OrderItems,
                    DeliveryPlace = DeliveryPlace,
                    EndUser = FlavoursOrderServer.EndUser,
                    ServiceTime = ServiceTime,
                    PayOption = PayOption,
                    FontsLink = this.FlavoursOrderServer.FontsLink


                };

                return menuData;

            }
        }// => ApplicationSettings.Current.LastServicePoinMenuData; set => ApplicationSettings.Current.LastServicePoinMenuData = value; }

        /// <exclude>Excluded</exclude>
        FlavourBusinessFacade.ViewModel.ISecureUser _EndUser;
        /// <MetaDataID>{e7648f67-8aba-4332-8173-27b6c422daa5}</MetaDataID>
        public FlavourBusinessFacade.ViewModel.ISecureUser EndUser
        {
            get
            {
                if (_EndUser == null)
                    return FlavoursOrderServer.EndUser;
                else
                    return _EndUser;
            }
            set
            {
                _EndUser = value;

                _ObjectChangeState?.Invoke(this, nameof(EndUser));
            }

        }


        /// <exclude>Excluded</exclude>
        IFoodServiceClientSession _FoodServicesClientSession;

        /// <MetaDataID>{be42123c-b940-44e6-91d1-d2a9e2b2ca7a}</MetaDataID>
        public IFoodServiceClientSession FoodServicesClientSession
        {
            get
            {
                return _FoodServicesClientSession;
            }
            set
            {
                if (_FoodServicesClientSession != null)
                {
                    try
                    {
                        _FoodServicesClientSession.MessageReceived -= MessageReceived;
                        _FoodServicesClientSession.ObjectChangeState -= FoodServicesClientSessionChangeState;
                        _FoodServicesClientSession.ItemStateChanged -= FoodServicesClientSessionItemStateChanged;
                        _FoodServicesClientSession.ItemsStateChanged -= FoodServicesClientSessionItemsStateChanged;
                        if (_FoodServicesClientSession is ITransparentProxy)
                            (_FoodServicesClientSession as ITransparentProxy).Reconnected -= FoodServicesClientSessionReconnected;


                    }
                    catch (Exception error)
                    {
                    }
                }

                if(FlavoursOrderServer==null)
                {
                    if(!ApplicationSettings.Current.ActiveSessions.Contains(this))
                    {

                    }
                }

                if (_FoodServicesClientSession == null && value != null && FlavoursOrderServer.AppType != AppType.OrderTaking)
                    value.DeviceResume();






                if (value == null)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _FoodServicesClientSession = null;
                        //Clear cache  the last session has ended
                        _OrderItems.Clear();
                        //this.MenuData = default(MenuData);
                        FlavoursOrderServer.Path = "";
                        MenuName = "";
                        MenuRoot = "";
                        MenuFile = "";
                        ClientSessionID = "";
                        MainSessionID = ""; ;
                        FoodServiceClientSessionUri = "";
                        ServicePointIdentity = "";
                        ServicesPointName = "";
                        ServicesContextLogo = "";
                        DefaultMealTypeUri = "";
                        ServedMealTypesUris = new List<string>();

                        stateTransition.Consistent = true;
                    }
                    return;
                }

                ClientSessionData clientSessionData = value.ClientSessionData;

                string path = FlavoursOrderServer.Path;
                if (!string.IsNullOrWhiteSpace(this.MenuData.ServicePointIdentity) && this.MenuData.ServicePointIdentity != clientSessionData.ServicePointIdentity)
                {
                    //The session has  moved to other service point
                    if (path != null && path.IndexOf(this.MenuData.ServicePointIdentity) != -1)
                        path = path.Replace(this.MenuData.ServicePointIdentity, clientSessionData.ServicePointIdentity);
                    FlavoursOrderServer.Path = path;
                }

                _FoodServicesClientSession = value;
                if (_FoodServicesClientSession != null && FlavoursOrderServer != null && clientSessionData.UserLanguageCode != FlavoursOrderServer.Language)
                    _FoodServicesClientSession.UpdateSessionUser(FlavoursOrderServer.Language);
                if (_FoodServicesClientSession?.SessionType == SessionType.HomeDelivery)
                {
                    if (DeliveryPlace == null)
                        _DeliveryPlace = clientSessionData.DeliveryPlace as FlavourBusinessManager.EndUsers.Place;
                    else
                    {
                        if (!FlavourBusinessManager.EndUsers.Place.AreSame(clientSessionData.DeliveryPlace, DeliveryPlace))
                            _FoodServicesClientSession.SetSessionDeliveryPlace(DeliveryPlace);
                    }
                    _DeliveryComment = _FoodServicesClientSession.DeliveryComment;
                }
                if (_FoodServicesClientSession?.SessionType == SessionType.HomeDeliveryGuest)
                {
                    if (_FoodServicesClientSession.MainSession != null)
                    {

                        using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                        {
                            _DeliveryPlace = _FoodServicesClientSession.MainSession.DeliveryPlace as FlavourBusinessManager.EndUsers.Place;
                            _ServiceTime = _FoodServicesClientSession.MainSession.ServiceTime;

                            stateTransition.Consistent = true;
                        }
                    }
                    _DeliveryComment = _FoodServicesClientSession.DeliveryComment;
                }
                if (_FoodServicesClientSession is ITransparentProxy)
                    (_FoodServicesClientSession as ITransparentProxy).Reconnected += FoodServicesClientSessionReconnected;

                //## ### ####
                SessionID = FoodServicesClientSession.SessionID;





                var storeRef = clientSessionData.Menu;
#if !DeviceDotNet
                storeRef.StorageUrl = "https://dev-localhost/devstoreaccount1/" + storeRef.StorageUrl.Substring(storeRef.StorageUrl.IndexOf("usersfolder"));
#endif
                //storeRef.StorageUrl = "https://angularhost.z16.web.core.windows.net/" + storeRef.StorageUrl.Substring(storeRef.StorageUrl.IndexOf("usersfolder"));


                var priceListStorageRef = clientSessionData.PriceList;
#if !DeviceDotNet
                if (priceListStorageRef != null)
                    priceListStorageRef.StorageUrl = "https://dev-localhost/devstoreaccount1/" + priceListStorageRef.StorageUrl.Substring(priceListStorageRef.StorageUrl.IndexOf("usersfolder"));

#endif



                MenuName = storeRef.Name;
                MenuRoot = storeRef.StorageUrl.Substring(0, storeRef.StorageUrl.LastIndexOf("/") + 1);
                MenuFile = storeRef.StorageUrl.Substring(storeRef.StorageUrl.LastIndexOf("/") + 1);
                PriceListUrl = priceListStorageRef?.StorageUrl;
                ClientSessionID = FoodServicesClientSession.SessionID;
                MainSessionID = FoodServicesClientSession.MainSession?.SessionID;
                FoodServiceClientSessionUri = RemotingServices.GetComputingContextPersistentUri(FoodServicesClientSession);
                ServicePointIdentity = clientSessionData.ServicePointIdentity;
                ServicesPointName = clientSessionData.ServicesPointName;
                ServicesContextLogo = clientSessionData.ServicesContextLogo;
                DefaultMealTypeUri = clientSessionData.DefaultMealTypeUri;
                ServedMealTypesUris = clientSessionData.ServedMealTypesUris;
                SessionType = clientSessionData.SessionType;

                FoodServicesClientSession.MessageReceived += MessageReceived;
                FoodServicesClientSession.ObjectChangeState += FoodServicesClientSessionChangeState;
                FoodServicesClientSession.ItemStateChanged += FoodServicesClientSessionItemStateChanged;
                FoodServicesClientSession.ItemsStateChanged += FoodServicesClientSessionItemsStateChanged;





                #region synchronize cached order items

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    var objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(ApplicationSettings.Current);

                    var orderItems = OrderItems.ToList();
                    foreach (var flavourItem in FoodServicesClientSession.FlavourItems)
                    {
                        var cachedOrderItem = OrderItems.Where(x => x.uid == flavourItem.uid).FirstOrDefault();
                        if (cachedOrderItem != null)
                        {
                            _OrderItems.Remove(cachedOrderItem);
                            orderItems.Remove(cachedOrderItem);
                        }
                        _OrderItems.Add(flavourItem as ItemPreparation);
                        objectStorage.CommitTransientObjectState(flavourItem);
                    }

                    foreach (var flavourItem in FoodServicesClientSession.SharedItems)
                    {
                        var cachedOrderItem = OrderItems.Where(x => x.uid == flavourItem.uid).FirstOrDefault();
                        if (cachedOrderItem != null)
                        {
                            _OrderItems.Remove(cachedOrderItem);
                            orderItems.Remove(cachedOrderItem);
                        }
                        _OrderItems.Add(flavourItem as ItemPreparation);
                        objectStorage.CommitTransientObjectState(flavourItem);
                    }

                    foreach (var item in orderItems)
                    {
                        if (item.SessionID == SessionID || item.SessionID == null)
                            FoodServicesClientSession.AddItem(item);
                    }

                    stateTransition.Consistent = true;
                }
                #endregion


                if (FlavoursOrderServer.AppType != AppType.OrderTaking)
                    RefreshMessmates();

                //MenuData menuData = new MenuData()
                //{
                //    MenuName = storeRef.Name,
                //    MenuRoot = storeRef.StorageUrl.Substring(0, storeRef.StorageUrl.LastIndexOf("/") + 1),
                //    MenuFile = storeRef.StorageUrl.Substring(storeRef.StorageUrl.LastIndexOf("/") + 1),
                //    ClientSessionID = FoodServicesClientSession.SessionID,
                //    MainSessionID = FoodServicesClientSession.MainSession?.SessionID,
                //    FoodServiceClientSessionUri = RemotingServices.GetComputingContextPersistentUri(FoodServicesClientSession),
                //    ServicePointIdentity = clientSessionData.ServicePointIdentity,
                //    ServicesPointName = clientSessionData.ServicesPointName,
                //    ServicesContextLogo = clientSessionData.ServicesContextLogo,
                //    DefaultMealTypeUri = clientSessionData.DefaultMealTypeUri,
                //    ServedMealTypesUris = clientSessionData.ServedMealTypesUris,
                //    SessionType = clientSessionData.SessionType

                //};
                //menuData.OrderItems = OrderItems.ToList();
                //MenuData = menuData;
                //ApplicationSettings.Current.LastServicePoinMenuData = menuData;
                _ServicePointState = clientSessionData.ServicePointState;

#if DeviceDotNet

                //MainThread.BeginInvokeOnMainThread(() =>
                //{

                //    if (Application.Current?.MainPage != null)
                //        Application.Current.MainPage.DisplayAlert("MenuData ", "ObjectChangeState MenuData", "OK");
                //});
#endif
#if DeviceDotNet
                OOAdvantech.DeviceApplication.Current.Log(new System.Collections.Generic.List<string>() { "_ObjectChangeState?.Invoke(this, nameof(MenuData))" });
#endif
                _ObjectChangeState?.Invoke(this, nameof(MenuData));
                GetMessages();
            }
        }

        /// <exclude>Excluded</exclude>
        ServicePointState _ServicePointState = ServicePointState.Laying;

        /// <summary>
        /// Defines the state of service point  
        /// </summary>
        /// <MetaDataID>{f4b050ec-01e9-4aab-9067-55c68e958c75}</MetaDataID>
        public FlavourBusinessFacade.ServicesContextResources.ServicePointState ServicePointState
        {
            get
            {
                return _ServicePointState;
            }
        }

        /// <MetaDataID>{2cb552e5-2926-44aa-a051-93f9d81d4045}</MetaDataID>
        internal string ClientSessionToken;


        #region Messmates

        /// <MetaDataID>{9e5492a1-32f6-4fb9-929d-f0d7a65fd727}</MetaDataID>
        IList<Messmate> CandidateMessmates = new List<Messmate>();
        /// <MetaDataID>{50126694-0421-469a-980e-029ead5df9e4}</MetaDataID>
        internal bool MessmatesLoaded;

        /// <MetaDataID>{7872a80f-383a-4e7e-a7c0-c81c99c9573a}</MetaDataID>
        public System.Collections.Generic.IList<DontWaitApp.Messmate> GetCandidateMessmates()
        {
            if (!MessmatesLoaded && FlavoursOrderServer.AppType != AppType.OrderTaking)
                GetMessmatesFromServer().Wait();
            return CandidateMessmates;

        }


        /// <MetaDataID>{d1b44dee-1753-48a2-89ba-fa7df5d31ecc}</MetaDataID>
        IList<Messmate> Messmates = new List<Messmate>();
        /// <MetaDataID>{af6e8091-4784-46b6-82af-70336ac8b8fd}</MetaDataID>
        public System.Collections.Generic.IList<DontWaitApp.Messmate> GetMessmates()
        {

            return Messmates;
        }




        /// <MetaDataID>{4ad45d89-6e83-4805-8f54-19858baaf192}</MetaDataID>
        IBill Bill;
        /// <MetaDataID>{dfe09297-df83-49c4-8767-8c4627cccaf4}</MetaDataID>
        public async Task<IBill> GetBill()
        {

            Bill = FoodServicesClientSession?.GetBill();
            var ss = Bill.Payments;

            foreach (var payment in ss)
            {
                var dsd = payment.Items;
            }

            var items = ss.FirstOrDefault()?.Items;
            return Bill;

            //var state = Payment.State;

            //var providerJson = Payment.PaymentProviderJson;
            //if (!string.IsNullOrWhiteSpace(providerJson))
            //{
            //    var orderCode = OOAdvantech.Json.JsonConvert.DeserializeObject<PaymentOrderResponse>(providerJson)?.orderCode;

            //    var webView = ((App.Current.MainPage as Xamarin.Forms.NavigationPage).CurrentPage as HybridWebViewPage).hybridWebView;
            //    webView.NativeWebBrowser.Navigate($"https://demo.vivapayments.com/web/checkout?ref={orderCode}");

            //}
            //this.FlavoursOrderServer.Pay(Payment);


        }
        /// <MetaDataID>{fa575847-542b-44f2-96c2-51398a146e95}</MetaDataID>
        public async Task<FlavourBusinessFacade.RoomService.IBill> GetBill(List<SessionItemPreparationAbbreviation> itemPreparations)
        {
            Bill = FoodServicesClientSession?.GetBill(itemPreparations);
            var ss = Bill.Payments;

            foreach (var payment in ss)
            {
                var dsd = payment.Items;
            }

            var items = ss.FirstOrDefault()?.Items;
            return Bill;

        }

        /// <MetaDataID>{da16b8ff-3972-47e6-b92c-f1f169f54c3a}</MetaDataID>
        internal async Task<bool> Pay(FinanceFacade.IPayment payment, decimal tipAmount)
        {
            if (payment.State == FinanceFacade.PaymentState.Completed)
                return true;
#if DeviceDotNet


            var paymentService = new PaymentService();
            return await paymentService.Pay(payment, tipAmount, FlavourBusinessFacade.ComputingResources.EndPoint.Server, Device.RuntimePlatform == "iOS");
#else
            return true;
#endif
        }


        /// <MetaDataID>{08af9f7f-89c9-40a9-9aab-e60d1661e7c5}</MetaDataID>
        public async Task Pay(FinanceFacade.IPayment payment, FinanceFacade.PaymentMethod paymentMethod, decimal tipAmount)
        {


            await this.FlavoursOrderServer.Pay(payment, paymentMethod, tipAmount);


        }


        //        /// <MetaDataID>{08af9f7f-89c9-40a9-9aab-e60d1661e7c5}</MetaDataID>
        //        public async Task<bool> PayAndCommit(FinanceFacade.IPayment payment, PaymentMethod paymentMethod, decimal tipAmount)
        //        {


        //#if DeviceDotNet

        //            try
        //            {
        //                FoodServicesClientSession.CreatePaymentGatewayOrder(payment, tipAmount, @"{""color"": ""607d8b""}");
        //            }
        //            catch (Exception error)
        //            {

        //                if(error.HResult==5001)
        //                {

        //                    var itemsState = this.FoodServicesClientSession.FlavourItemsPreparationState;
        //                    foreach (var item in OrderItems)
        //                    {
        //                        ItemPreparationState itemState;
        //                        if (itemsState.TryGetValue(item.uid, out itemState))
        //                            item.State = itemState;
        //                        var allCommited = this._OrderItems.Where(x => x.SessionID==SessionID).All(x => x.State.IsIntheSameOrFollowingState(ItemPreparationState.Committed));
        //                        if (allCommited)
        //                            return true;
        //                    }
        //                }
        //                throw;
        //            }
        //            RemotingServices.InvalidateCacheData(payment as MarshalByRefObject);
        //            if (await this.FlavoursOrderServer.Pay(payment))
        //            {
        //                RemotingServices.InvalidateCacheData(payment as MarshalByRefObject);
        //                var state = payment.State;
        //                if (state==FinanceFacade.PaymentState.Completed)
        //                {

        //                    var itemsState = this.FoodServicesClientSession.FlavourItemsPreparationState;
        //                    foreach (var item in OrderItems)
        //                    {
        //                        ItemPreparationState itemState;
        //                        if (itemsState.TryGetValue(item.uid, out itemState))
        //                            item.State = itemState;
        //                    }

        //                    System.Diagnostics.Debug.WriteLine("FinanceFacade.PaymentState.Completed");
        //                    var sessionOrderItems = this._OrderItems.Where(x => x.SessionID==SessionID).ToList();
        //                    var allCommited = this._OrderItems.Where(x => x.SessionID==SessionID).All(x => x.State.IsIntheSameOrFollowingState(ItemPreparationState.Committed));

        //                    return allCommited;
        //                }
        //                else
        //                {
        //                    return false;
        //                }
        //            }
        //            return false;
        //#else
        //            payment.CashPaymentCompleted(tipAmount);
        //            var paymenta = payment.State;
        //            this.SendItemsForPreparation();

        //            return true;
        //#endif
        //        }


        /// <MetaDataID>{1a978c97-bdb0-4ad9-8de6-358bf86d2fa4}</MetaDataID>
        public async Task RefreshMessmates()
        {
            await GetMessmatesFromServer();
        }
        /// <MetaDataID>{4f182b25-801d-44a9-bb30-ca0320c4c7d5}</MetaDataID>
        public Task GetMessmatesFromServer()
        {

            return Task.Run(() =>
            {
                try
                {
                    if (FoodServicesClientSession != null)
                    {

                        var messmates = (from clientSession in FoodServicesClientSession.GetMealParticipants()
                                         select new Messmate(clientSession, OrderItems)).ToList();
                        Messmates = messmates;
                        if (SessionType == SessionType.Hall)
                        {
                            var candidateMessmates = (from clientSession in FoodServicesClientSession?.GetPeopleNearMe()
                                                      select new Messmate(clientSession, OrderItems)).ToList();
                            CandidateMessmates = candidateMessmates;
                        }

                        MessmatesLoaded = true;
                        //GetMessages();
                        _ObjectChangeState?.Invoke(this, null);
                    }
                }
                catch (Exception error)
                {
                }
            });
        }


        [OOAdvantech.MetaDataRepository.HttpInVisible]
        event MessmatesWaitForYouToDecideHandle _MessmatesWaitForYouToDecide;

        [HttpInVisible]
        public event MessmatesWaitForYouToDecideHandle MessmatesWaitForYouToDecide
        {
            add
            {
                if (_MessmatesWaitForYouToDecide == null)
                {
                    _MessmatesWaitForYouToDecide += value;
                    GetMessages();
                }
                else
                    _MessmatesWaitForYouToDecide += value;

            }
            remove
            {
                _MessmatesWaitForYouToDecide -= value;
            }
        }



        #endregion


        #region Items to prepare


        /// <MetaDataID>{63add039-3bee-4d8b-8d15-002e6e67aa63}</MetaDataID>
        public System.Collections.Generic.IList<FlavourBusinessManager.RoomService.ItemPreparation> PreparationItems => OrderItems.ToList();


        /// <MetaDataID>{8e9519da-bc4b-4953-8bde-7f95bea4a1f5}</MetaDataID>
        public async System.Threading.Tasks.Task<bool> SendItemsForPreparation()
        {

            //if (this.SessionType==SessionType.HomeDelivery||this.SessionType==SessionType.HomeDeliveryGuest&&PayOption==PayOptions.PayOnCheckout)
            //{
            //    Bill = FoodServicesClientSession?.GetBill();
            //    var payment = Bill.Payments.Where(x => x.State!=FinanceFacade.PaymentState.Completed).FirstOrDefault();
            //    if (payment != null)
            //    {
            //        //Pay()
            //    }
            //    return false;

            //}
            //else
             
            var itemsNewState = this.FoodServicesClientSession.Commit(OrderItems.OfType<IItemPreparation>().ToList());
            foreach (var itemNewState in itemsNewState)
            {
                var item = this.OrderItems.Where(x => x.uid == itemNewState.Key).FirstOrDefault();
                item.State = itemNewState.Value;
            }
            return true;

        }



        public async Task<bool> CommitNewSessionType(SessionType sessionType)
        {
            if (this.SessionType != sessionType)
            {
                var itemsNewState = FoodServicesClientSession.CommitNewSessionType(sessionType, OrderItems.OfType<IItemPreparation>().ToList());
                FoodServicesClientSession = FoodServicesClientSession;
            }
            return true;
        }


        /// <exclude>Excluded</exclude>
        [HttpInVisible]
        event SharedItemChangedHandle _SharedItemChanged;

        [HttpInVisible]
        public event SharedItemChangedHandle SharedItemChanged
        {
            add
            {
                if (_SharedItemChanged == null)
                {
                    _SharedItemChanged += value;
                    if (ShareItemHasChangeMessage != null)
                    {
                        Task.Run(() =>
                        {
                            System.Threading.Thread.Sleep(2000);
                            var shareItemHasChangeMessage = ShareItemHasChangeMessage;
                            ShareItemHasChangeMessage = null;
                            ShareItemHasChangeMessageForward(shareItemHasChangeMessage);

                        });
                    }
                }
                else
                    _SharedItemChanged += value;

            }
            remove
            {
                _SharedItemChanged -= value;
            }
        }


        /// <MetaDataID>{9748ebb1-5f4e-4afc-839d-40a2b3667c25}</MetaDataID>
        public void AddItem(ItemPreparation item)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _OrderItems.Add(item);
                var objectStarage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this);
                if (objectStarage != null)
                    objectStarage.CommitTransientObjectState(item);
                stateTransition.Consistent = true;
            }


            //var menuData = MenuData;
            //menuData.OrderItems = OrderItems.ToList();
            //MenuData = menuData;

            FlavoursOrderServer.SerializeTaskScheduler.AddTask(async () =>
            {

                var datetime = DateTime.Now;
                string timestamp = DateTime.Now.ToLongTimeString() + ":" + datetime.Millisecond.ToString();

                int tries = 30;
                while (tries > 0)
                {
                    try
                    {
                        this.FoodServicesClientSession.AddItem(item);
                        //int cou = this.FoodServicesClientSession.FlavourItems.Count;
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

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _OrderItems.Add(item);
                var objectStarage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this);
                objectStarage.CommitTransientObjectState(item);
                stateTransition.Consistent = true;
            }

            //var menuData = MenuData;
            //menuData.OrderItems = OrderItems.ToList();
            //MenuData = menuData;

            FlavoursOrderServer.SerializeTaskScheduler.AddTask(async () =>
            {
                var datetime = DateTime.Now;
                string timestamp = DateTime.Now.ToLongTimeString() + ":" + datetime.Millisecond.ToString();

                int tries = 30;
                while (tries > 0)
                {
                    try
                    {
                        this.FoodServicesClientSession.AddSharingItem(item);
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
        public void ItemChanged(ItemPreparation item)
        {
            bool hasChanges = OrderItemsDictionary[item.uid].Update(item);

            item = OrderItemsDictionary[item.uid];
            if (hasChanges)
            {
                //var menuData = MenuData;
                //menuData.OrderItems = OrderItems.ToList();
                //MenuData = menuData;
                FlavoursOrderServer.SerializeTaskScheduler.AddTask(async () =>
                {
                    var datetime = DateTime.Now;
                    string timestamp = DateTime.Now.ToLongTimeString() + ":" + datetime.Millisecond.ToString();

                    int tries = 30;
                    while (tries > 0)
                    {
                        try
                        {
                            FoodServicesClientSession.ItemChanged(item);
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
        /// <MetaDataID>{b44c1f47-af25-4dcf-b59f-f818941c6397}</MetaDataID>
        System.Collections.Generic.Dictionary<string, FlavourBusinessManager.RoomService.ItemPreparation> OrderItemsDictionary
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


            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                var orderItem = _OrderItems.Where(x => x.uid == item.uid).FirstOrDefault();
                if (orderItem != null)
                    _OrderItems.Remove(orderItem);
                stateTransition.Consistent = true;
            }


            //var menuData = MenuData;
            //menuData.OrderItems = OrderItems.ToList();
            //MenuData = menuData;
            FlavoursOrderServer.SerializeTaskScheduler.AddTask(async () =>
            {

                var datetime = DateTime.Now;
                string timestamp = DateTime.Now.ToLongTimeString() + ":" + datetime.Millisecond.ToString();

                int tries = 30;
                while (tries > 0)
                {
                    try
                    {
                        if (this.FoodServicesClientSession != null)
                        {
                            if (item.SessionID == SessionID)
                            {
                                this.FoodServicesClientSession.RemoveItem(item);
                                int cou = this.FoodServicesClientSession.FlavourItems.Count;
                                break;
                            }
                            else
                            {
                                this.FoodServicesClientSession.RemoveSharingItem(item);
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

        /// <summary>
        /// Gets itemPreparation
        /// </summary>
        /// <param name="itemUid">
        /// Defines the item unique identity
        /// </param>
        /// <param name="itemOwnerSession">
        /// Defines the session where item created
        /// </param>
        /// <returns>
        /// returns the itemPreparation object
        /// </returns>
        /// <MetaDataID>{6a9b4e5b-be1a-40c7-bc79-3d3a86e8ad87}</MetaDataID>
        private async Task<ItemPreparation> GetItemFromOwner(string itemUid, string itemOwnerSession)
        {
            if (FoodServicesClientSession.SessionID == itemOwnerSession)
                return FoodServicesClientSession.GetItem(itemUid) as ItemPreparation;
            else
            {
                var messmate = (from theMessmate in this.Messmates
                                where theMessmate.ClientSessionID == itemOwnerSession
                                select theMessmate).FirstOrDefault();
                if (messmate != null)
                    return messmate.ClientSession.GetItem(itemUid) as ItemPreparation;
                else
                {
                    await GetMessmatesFromServer();
                    messmate = (from theMessmate in this.Messmates
                                where theMessmate.ClientSessionID == itemOwnerSession
                                select theMessmate).FirstOrDefault();
                    if (messmate != null)
                        return messmate.ClientSession.GetItem(itemUid) as ItemPreparation;

                    return null;
                }
            }
        }


        #endregion



        #region Menu item proposal

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
                                    FoodServicesClientSession.RemoveMessage(message.MessageID);

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




        /// <MetaDataID>{1ad75d89-e702-4d8e-8577-316b6b2a7977}</MetaDataID>
        private void MenuItemProposalMessageForward(FlavourBusinessFacade.EndUsers.Message message)
        {
            if (_MenuItemProposal != null)
            {
                if (MessmatesLoaded)
                {
                    var messmate = (from theMessmate in this.Messmates
                                    where theMessmate.ClientSessionID == message.GetDataValue("ClientSessionID") as string
                                    select theMessmate).FirstOrDefault();
                    if (messmate != null)
                    {
                        string menuItemUri = message.GetDataValue<string>("MenuItemUri");

                        _MenuItemProposal?.Invoke(this, messmate, menuItemUri, message.MessageID);
                    }

                    //try
                    //{
                    //    var notificationManager = DependencyService.Get<INotificationManager>();
                    //    notificationManager.SendNotification(message.Notification.Title, message.Notification.Body, DateTime.Now.AddSeconds(10));

                    //}
                    //catch (Exception error)
                    //{

                    //}                
                }
            }
            else
                MenuItemProposalMessage = message;
        }


        /// <MetaDataID>{50033e95-3a4d-492d-98ba-f14f0e359418}</MetaDataID>
        public void EndOfMenuItemProposal(DontWaitApp.Messmate messmate, string messageID)
        {
            FoodServicesClientSession.RemoveMessage(messageID);
            GetMessages();
        }

        /// <MetaDataID>{c36bf954-e18c-4f82-aef4-ea94b18a4747}</MetaDataID>
        public void SuggestMenuItem(DontWaitApp.Messmate messmate, string menuItemUri)
        {
            var clientSession = (from theMessmate in this.Messmates
                                 where theMessmate.ClientSessionID == messmate.ClientSessionID
                                 select theMessmate.ClientSession).FirstOrDefault();

            clientSession.MenuItemProposal(FoodServicesClientSession, menuItemUri);

        }



        #endregion



        /// <MetaDataID>{0575115b-ff76-471d-b365-a880011b0050}</MetaDataID>
        public IFoodServiceSession MainSession => this.FoodServicesClientSession?.MainSession;



        /// <exclude>Excluded</exclude>
        FlavourBusinessManager.EndUsers.Place _DeliveryPlace;
        /// <MetaDataID>{ec5a44a6-bb30-4a35-aa38-67b6ab70a130}</MetaDataID>
        [CachingDataOnClientSide]
        public IPlace DeliveryPlace
        {
            get => _DeliveryPlace;
            set
            {

                if (_DeliveryPlace != value)
                {
                    if (value != null && CanChangeDeliveryPlace(value) == ChangeDeliveryPlaceResponse.OK)
                    {
                        using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                        {
                            _DeliveryPlace = value as FlavourBusinessManager.EndUsers.Place;
                            if (this.FoodServicesClientSession != null)
                                this.FoodServicesClientSession.SetSessionDeliveryPlace(value);

                            FlavourBusinessManager.EndUsers.Place existingPlace = (EndUser as IGeocodingPlaces).Places.Where(x => x.PlaceID == value.PlaceID).FirstOrDefault() as FlavourBusinessManager.EndUsers.Place;
                            if (existingPlace != null)
                            {
                                existingPlace.Update(value);
                                (EndUser as IGeocodingPlaces).SavePlace(existingPlace);
                            }
                            else
                                (EndUser as IGeocodingPlaces).SavePlace(value);

                            stateTransition.Consistent = true;
                        }
                    }
                }
            }
        }




        /// <exclude>Excluded</exclude>
        string _DeliveryComment;
        /// <MetaDataID>{a3dff6d9-b898-4414-8766-45fabf3c4aec}</MetaDataID>
        [PersistentMember(nameof(_DeliveryComment))]
        [BackwardCompatibilityID("+17")]
        public string DeliveryComment
        {
            get => _DeliveryComment;
            set
            {
                if (_DeliveryComment != value)
                {
                    if (this.FoodServicesClientSession != null)
                        this.FoodServicesClientSession.DeliveryComment = value;

                    _DeliveryComment = value;

                }
            }
        }

        /// <exclude>Excluded</exclude>
        string _OrderComment;

        /// <MetaDataID>{0a248fc2-7e80-4304-8590-3eb598558067}</MetaDataID>
        [PersistentMember(nameof(_OrderComment))]
        [BackwardCompatibilityID("+18")]
        public string OrderComment
        {
            get => _OrderComment; set
            {
                if (_OrderComment != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        if (this.FoodServicesClientSession != null)
                            this.FoodServicesClientSession.OrderComment = value;
                        _OrderComment = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        string _NotesForClient;

        /// <MetaDataID>{45654077-d907-4760-9c9a-a256352ae795}</MetaDataID>
        [PersistentMember(nameof(_NotesForClient))]
        [BackwardCompatibilityID("+19")]
        public string NotesForClient
        {
            get => _NotesForClient; set
            {
                if (_NotesForClient != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {

                        _NotesForClient = value;
                        if ((this.EndUser as FoodServiceClientVM)?.FoodServiceClient != null)
                            (this.EndUser as FoodServiceClientVM).FoodServiceClient.NotesForClient = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }






        /// <MetaDataID>{9302cac6-d078-4956-a5d6-b190b3a0700c}</MetaDataID>
        public ChangeDeliveryPlaceResponse CanChangeDeliveryPlace(FlavourBusinessFacade.EndUsers.IPlace newDeliveryPlace)
        {
            if (_FoodServicesClientSession == null)
                return ChangeDeliveryPlaceResponse.OK;

            return _FoodServicesClientSession.CanChangeDeliveryPlace(newDeliveryPlace.Location);
        }
        /// <MetaDataID>{ebc255f7-6774-4f15-b12a-d8a9cb85c1c3}</MetaDataID>
        [ObjectActivationCall]
        public void ObjectActivation()
        {
            if (DeliveryPlaceJson != null)
                _DeliveryPlace = OOAdvantech.Json.JsonConvert.DeserializeObject<FlavourBusinessManager.EndUsers.Place>(DeliveryPlaceJson);
        }

        /// <MetaDataID>{4c3d8994-d157-4a4c-94ba-a3d6d645331b}</MetaDataID>
        [BeforeCommitObjectStateInStorageCall]
        void BeforeCommitObjectState()
        {
            DeliveryPlaceJson = OOAdvantech.Json.JsonConvert.SerializeObject(_DeliveryPlace);
        }



        /// <MetaDataID>{732cc3b9-e3cc-4b53-88ed-a1d05cf332e5}</MetaDataID>
        FlavourBusinessFacade.EndUsers.Message MenuItemProposalMessage;
        /// <MetaDataID>{3e58cac8-fc04-4ff0-9ec9-68df0268646b}</MetaDataID>
        private string SessionID;
        /// <MetaDataID>{356d9786-15a7-41f6-a44c-19ff475abbe8}</MetaDataID>
        private FlavourBusinessFacade.EndUsers.ClientSessionData clientSessionData;
        /// <MetaDataID>{20bd8838-4798-4082-a3e4-c40299bce52d}</MetaDataID>
        private MenuData lastServicePoinMenuData;
        /// <MetaDataID>{b57c2c39-f423-4c81-b4d4-e6459daed045}</MetaDataID>
        public DontWaitApp.FlavoursOrderServer FlavoursOrderServer { get; internal set; }

    }




}
