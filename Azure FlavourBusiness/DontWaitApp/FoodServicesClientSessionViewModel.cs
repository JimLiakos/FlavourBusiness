using FlavourBusinessFacade.EndUsers;
using FlavourBusinessFacade.RoomService;
using FlavourBusinessFacade.ServicesContextResources;
using FlavourBusinessManager.RoomService;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Remoting.RestApi;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DontWaitApp
{
    /// <MetaDataID>{5ce0ff0c-1c71-4161-85e4-12150431675e}</MetaDataID>
    public class FoodServicesClientSessionViewModel: IFoodServicesClientSessionViewModel
    {



        #region Messages

        /// <MetaDataID>{9714f9b4-1950-4bd5-9a10-a482c42d355e}</MetaDataID>
        object MessagesLock = new object();

    
        /// <MetaDataID>{ca16b127-1d5f-46e5-aac6-633a14ae9794}</MetaDataID>
        internal void GetMessages()
        {
            lock (MessagesLock)
            {
                if (FoodServicesClientSession != null)
                {
                    var message = FoodServicesClientSession.PeekMessage();

                    if (message != null && message.GetDataValue<ClientMessages>("ClientMessageType") == ClientMessages.PartOfMealRequest)
                    {
                      this.FlavoursOrderServer.PartOfMealRequestMessageForward(message);
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

        /// <MetaDataID>{24360209-3b94-4a93-8d33-364f5c406bae}</MetaDataID>
        private void ShareItemHasChangeMessageForward(Message message)
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
        }

        /// <MetaDataID>{837f1943-f15c-49d8-a50b-2832acab8771}</MetaDataID>
        private void MessageReceived(IMessageConsumer sender)
        {
            GetMessages();
        }
        #endregion


        #region Food services client session events consumers
        /// <MetaDataID>{2784806b-281e-44df-af8d-dd847bb7c8a9}</MetaDataID>
        private void FoodServicesClientSessionItemsStateChanged(Dictionary<string, ItemPreparationState> newItemsState)
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
        private async void FoodServicesClientSessionItemStateChanged(string itemUid, string itemOwnerSession, bool isShared, List<string> shareInSessions)
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

        /// <MetaDataID>{0dce1877-b96f-4558-98b6-66fb704a80f1}</MetaDataID>
        private void FoodServicesClientSessionChangeState(object _object, string member)
        {
            if (member == nameof(IFoodServiceClientSession.FlavourItems))
            {
                OrderItems.Clear();
                //foreach (var flavourItem in FoodServiceClientSession.FlavourItems)
                //    OrderItems[flavourItem.uid] = flavourItem as ItemPreparation;
                //foreach (var flavourItem in FoodServiceClientSession.SharedItems)
                //    OrderItems[flavourItem.uid] = flavourItem as ItemPreparation;


                OrderItems.AddRange(FoodServicesClientSession.FlavourItems.OfType<ItemPreparation>());
                OrderItems.AddRange(FoodServicesClientSession.SharedItems.OfType<ItemPreparation>());

                ///RefreshMessmates();
                //var menuData = MenuData;
                //menuData.OrderItems = OrderItems.Values.ToList();
                //MenuData= menuData;

                var menuData = this.MenuData;
                menuData.OrderItems = OrderItems.ToList();
                MenuData = menuData;

                _ObjectChangeState?.Invoke(this, null);


            }
            else if (member == nameof(IFoodServiceClientSession.MainSession))
            {
                RefreshMessmates();
            }
            else if (member == nameof(IFoodServiceClientSession.ServicePoint))
            {
                var foodServiceClientSessionUri = RemotingServices.GetComputingContextPersistentUri(FoodServicesClientSession);
                GetFoodServicesClientSessionData(foodServiceClientSessionUri);
            }
            else
                RefreshMessmates();



        }

        /// <MetaDataID>{c8a403e0-d804-40a6-8543-4bee48d1319f}</MetaDataID>
        private void FoodServicesClientSessionReconnected(object sender)
        {

            var foodServiceClientSession = RemotingServices.CastTransparentProxy<IFoodServiceClientSession>(sender);
            OrderItems.Clear();

            //foreach (var flavourItem in FoodServiceClientSession.FlavourItems)
            //    OrderItems[flavourItem.uid] = flavourItem as ItemPreparation;

            //foreach (var flavourItem in FoodServiceClientSession.SharedItems)
            //    OrderItems[flavourItem.uid] = flavourItem as ItemPreparation;

            OrderItems.AddRange(FoodServicesClientSession.FlavourItems.OfType<ItemPreparation>());
            OrderItems.AddRange(FoodServicesClientSession.SharedItems.OfType<ItemPreparation>());



            var menuData = this.MenuData;
            menuData.OrderItems = OrderItems.ToList();
            MenuData = menuData;


            //foreach (var messmate in Messmates)
            //    messmate.Refresh(OrderItems);
            _ObjectChangeState?.Invoke(this, null);
        }

        #endregion


        /// <MetaDataID>{73f366a8-2d78-456f-aacd-b70114704a4f}</MetaDataID>
        public MenuData MenuData { get => ApplicationSettings.Current.LastServicePoinMenuData; set => ApplicationSettings.Current.LastServicePoinMenuData = value; }

        /// <MetaDataID>{e31b5e23-a2de-4d2e-879b-727f7ff1dc41}</MetaDataID>
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
                    FoodServicesClientSession.MessageReceived -= MessageReceived;
                    FoodServicesClientSession.ObjectChangeState -= FoodServicesClientSessionChangeState;
                    FoodServicesClientSession.ItemStateChanged -= FoodServicesClientSessionItemStateChanged;
                    FoodServicesClientSession.ItemsStateChanged -= FoodServicesClientSessionItemsStateChanged;
                }

                if (_FoodServicesClientSession == null && value != null)
                    value.DeviceResume();


                if (_FoodServicesClientSession is ITransparentProxy)
                    (_FoodServicesClientSession as ITransparentProxy).Reconnected -= FoodServicesClientSessionReconnected;





                if (value == null)
                {
                    _FoodServicesClientSession = null;
                    //Clear cache  the last session has ended
                    OrderItems.Clear();
                    this.MenuData = null;
                    Path = "";
                    return;
                }

                ClientSessionData clientSessionData = value.ClientSessionData;
                string path = Path;
                if (!string.IsNullOrWhiteSpace(this.MenuData.ServicePointIdentity) && this.MenuData.ServicePointIdentity != clientSessionData.ServicePointIdentity)
                {
                    //The session has  moved to other service point
                    if (path != null && path.IndexOf(this.MenuData.ServicePointIdentity) != -1)
                        path = path.Replace(this.MenuData.ServicePointIdentity, clientSessionData.ServicePointIdentity);
                    Path = path;
                }

                _FoodServicesClientSession = value;

                if (_FoodServicesClientSession is ITransparentProxy)
                    (_FoodServicesClientSession as ITransparentProxy).Reconnected += FoodServicesClientSessionReconnected;

                //## ### ####
                SessionID = FoodServicesClientSession.SessionID;
                ApplicationSettings.Current.LastClientSessionID = SessionID;

                #region synchronize cached order items
                foreach (var flavourItem in FoodServicesClientSession.FlavourItems)
                {
                    var cachedOrderItem = OrderItems.Where(x => x.uid == flavourItem.uid).FirstOrDefault();
                    if (cachedOrderItem != null)
                        OrderItems.Remove(cachedOrderItem);
                    OrderItems.Add(flavourItem as ItemPreparation);
                }

                foreach (var flavourItem in FoodServicesClientSession.SharedItems)
                {
                    var cachedOrderItem = OrderItems.Where(x => x.uid == flavourItem.uid).FirstOrDefault();
                    if (cachedOrderItem != null)
                        OrderItems.Remove(cachedOrderItem);
                    OrderItems.Add(flavourItem as ItemPreparation);
                }
                #endregion
                RefreshMessmates();

                FoodServicesClientSession.MessageReceived += MessageReceived;
                FoodServicesClientSession.ObjectChangeState += FoodServicesClientSessionChangeState;
                FoodServicesClientSession.ItemStateChanged += FoodServicesClientSessionItemStateChanged;
                FoodServicesClientSession.ItemsStateChanged += FoodServicesClientSessionItemsStateChanged;

                var storeRef = FoodServicesClientSession.Menu;
#if !DeviceDotNet
                storeRef.StorageUrl = "https://dev-localhost/" + storeRef.StorageUrl.Substring(storeRef.StorageUrl.IndexOf("devstoreaccount1"));
#endif



                MenuData menuData = new MenuData()
                {
                    MenuName = storeRef.Name,
                    MenuRoot = storeRef.StorageUrl.Substring(0, storeRef.StorageUrl.LastIndexOf("/") + 1),
                    MenuFile = storeRef.StorageUrl.Substring(storeRef.StorageUrl.LastIndexOf("/") + 1),
                    ClientSessionID = FoodServicesClientSession.SessionID,
                    MainSessionID = FoodServicesClientSession.MainSession?.SessionID,
                    FoodServiceClientSessionUri = RemotingServices.GetComputingContextPersistentUri(FoodServicesClientSession),
                    ServicePointIdentity = clientSessionData.ServicePointIdentity,
                    ServicesPointName = clientSessionData.ServicesPointName,
                    ServicesContextLogo = clientSessionData.ServicesContextLogo,
                    DefaultMealTypeUri = clientSessionData.DefaultMealTypeUri,
                    ServedMealTypesUris = clientSessionData.ServedMealTypesUris,
                    SessionType = clientSessionData.SessionType

                };
                menuData.OrderItems = OrderItems.ToList();
                MenuData = menuData;
                ApplicationSettings.Current.LastServicePoinMenuData = menuData;
                _ServicePointState = clientSessionData.ServicePointState;
                _ObjectChangeState?.Invoke(this, nameof(MenuData));
                GetMessages();
            }
        }

        ServicePointState _ServicePointState = ServicePointState.Laying;

        /// <summary>
        /// Defines the state of service point  
        /// </summary>
        public ServicePointState ServicePointState
        {
            get
            {
                return _ServicePointState;
            }
        }

        /// <MetaDataID>{2cb552e5-2926-44aa-a051-93f9d81d4045}</MetaDataID>
        string ClientSessionToken;


        #region Messmates

        /// <MetaDataID>{9e5492a1-32f6-4fb9-929d-f0d7a65fd727}</MetaDataID>
        IList<Messmate> CandidateMessmates = new List<Messmate>();
        /// <MetaDataID>{50126694-0421-469a-980e-029ead5df9e4}</MetaDataID>
        internal bool MessmatesLoaded;

        /// <MetaDataID>{7872a80f-383a-4e7e-a7c0-c81c99c9573a}</MetaDataID>
        public IList<Messmate> GetCandidateMessmates()
        {
            if (!MessmatesLoaded)
                GetMessmatesFromServer().Wait(2);
            return CandidateMessmates;

        }


        /// <MetaDataID>{d1b44dee-1753-48a2-89ba-fa7df5d31ecc}</MetaDataID>
        IList<Messmate> Messmates = new List<Messmate>();
        /// <MetaDataID>{af6e8091-4784-46b6-82af-70336ac8b8fd}</MetaDataID>
        public IList<Messmate> GetMessmates()
        {
            return Messmates;
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
                    if (FoodServicesClientSession != null)
                    {
                        //if (WaiterView)
                        //{
                        //    var messmates = (from clientSession in FoodServiceClientSession?.GetServicePointParticipants()
                        //                     where clientSession != this._FoodServiceClientSession
                        //                     select new Messmate(clientSession, OrderItems)).ToList();
                        //    Messmates = messmates;
                        //    MessmatesLoaded = true;

                        //}
                        //else
                        {
                            var messmates = (from clientSession in FoodServicesClientSession.GetMealParticipants()
                                             select new Messmate(clientSession, OrderItems)).ToList();
                            var candidateMessmates = (from clientSession in FoodServicesClientSession?.GetPeopleNearMe()
                                                      select new Messmate(clientSession, OrderItems)).ToList();
                            Messmates = messmates;
                            CandidateMessmates = candidateMessmates;

                            MessmatesLoaded = true;
                            GetMessages();
                        }
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
        public IList<ItemPreparation> PreparationItems => OrderItems.ToList();


        /// <MetaDataID>{8e9519da-bc4b-4953-8bde-7f95bea4a1f5}</MetaDataID>
        public async Task<bool> SendItemsForPreparation()
        {


            var itemsNewState = this.FoodServicesClientSession.Commit(OrderItems.OfType<IItemPreparation>().ToList());

            foreach (var itemNewState in itemsNewState)
            {
                var item = this.OrderItems.Where(x => x.uid == itemNewState.Key).FirstOrDefault();
                item.State = item.State;
            }
            return true;
        }

        /// <MetaDataID>{67a87990-9414-48bc-8d1d-bc20335eb6ea}</MetaDataID>
        List<ItemPreparation> OrderItems = new List<ItemPreparation>();


        /// <exclude>Excluded</exclude>
        [HttpInVisible]
        event SharedItemChangedHandle _SharedItemChanged;

        [HttpInVisible]
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


        /// <MetaDataID>{9748ebb1-5f4e-4afc-839d-40a2b3667c25}</MetaDataID>
        public void AddItem(ItemPreparation item)
        {

            OrderItems.Add(item);
            var menuData = MenuData;
            menuData.OrderItems = OrderItems.ToList();
            MenuData = menuData;
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
                        int cou = this.FoodServicesClientSession.FlavourItems.Count;
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
        public void ItemChanged(FlavourBusinessManager.RoomService.ItemPreparation item)
        {
            bool hasChanges = OrderItemsDictionary[item.uid].Update(item);
            if (hasChanges)
            {
                var menuData = MenuData;
                menuData.OrderItems = OrderItems.ToList();
                MenuData = menuData;
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
                            FoodServicesClientSession.RemoveMessage(message.MessageID);
                        return;
                    }
                    string menuItemUri = message.GetDataValue<string>("MenuItemUri");

                    _MenuItemProposal?.Invoke(this, messmate, menuItemUri, message.MessageID);
                }
            }
            else
                MenuItemProposalMessage = message;
        }


        /// <MetaDataID>{50033e95-3a4d-492d-98ba-f14f0e359418}</MetaDataID>
        public void EndOfMenuItemProposal(Messmate messmate, string messageID)
        {
            FoodServicesClientSession.RemoveMessage(messageID);
            GetMessages();
        }

        /// <MetaDataID>{c36bf954-e18c-4f82-aef4-ea94b18a4747}</MetaDataID>
        public void SuggestMenuItem(Messmate messmate, string menuItemUri)
        {
            var clientSession = (from theMessmate in this.Messmates
                                 where theMessmate.ClientSessionID == messmate.ClientSessionID
                                 select theMessmate.ClientSession).FirstOrDefault();

            clientSession.MenuItemProposal(FoodServicesClientSession, menuItemUri);

        }

           #endregion

    

        public IFoodServiceSession MainSession => this.FoodServicesClientSession?.MainSession;


   



        /// <MetaDataID>{d048d779-11d6-4c80-b308-24e58a0359f8}</MetaDataID>
        Message MenuItemProposalMessage;
        /// <MetaDataID>{3e58cac8-fc04-4ff0-9ec9-68df0268646b}</MetaDataID>
        private string SessionID;
        private ClientSessionData clientSessionData;
        private MenuData lastServicePoinMenuData;
        private FlavoursOrderServer FlavoursOrderServer;

        public FoodServicesClientSessionViewModel(ClientSessionData clientSessionData, FlavoursOrderServer flavoursOrderServer)
        {
            FlavoursOrderServer = flavoursOrderServer;
            FoodServicesClientSession = clientSessionData.FoodServiceClientSession;
            ClientSessionToken = clientSessionData.Token;
            
        }

        public FoodServicesClientSessionViewModel(MenuData menuData, FlavoursOrderServer flavoursOrderServer)
        {
            MenuData = menuData;
            FlavoursOrderServer = flavoursOrderServer;
            if (MenuData.OrderItems != null)
                OrderItems = MenuData.OrderItems.ToList();

        }
    }
}
