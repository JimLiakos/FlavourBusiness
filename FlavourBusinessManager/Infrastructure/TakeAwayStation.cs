using FlavourBusinessFacade;
using FlavourBusinessFacade.EndUsers;
using FlavourBusinessFacade.ServicesContextResources;
using FlavourBusinessManager.HumanResources;
using FlavourBusinessManager.ServicePointRunTime;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using System;
using System.Linq;
using FlavourBusinessManager.RoomService;
using FlavourBusinessFacade.RoomService;
using System.Collections.Generic;
using FlavourBusinessManager.EndUsers;

namespace FlavourBusinessManager.ServicesContextResources
{
    /// <MetaDataID>{99f0adb9-6986-412c-9f26-aa956ec96f18}</MetaDataID>
    [BackwardCompatibilityID("{99f0adb9-6986-412c-9f26-aa956ec96f18}")]
    [Persistent()]
    public class TakeAwayStation : ServicePoint, OOAdvantech.Remoting.IExtMarshalByRefObject, ITakeAwayStation
    {

        /// <exclude>Excluded</exclude>
        string _PaymentTerminalIdentity;

        /// <MetaDataID>{3497ea83-2577-404c-a6c4-c4ea9939bd5c}</MetaDataID>
        [PersistentMember(nameof(_PaymentTerminalIdentity))]
        [BackwardCompatibilityID("+5")]
        public string PaymentTerminalIdentity
        {
            get => _PaymentTerminalIdentity;
            set
            {
                if (_PaymentTerminalIdentity != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _PaymentTerminalIdentity = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        string _GraphicMenuStorageIdentity;
        /// <MetaDataID>{0301f3d5-d132-499d-aef0-c04561c4dc80}</MetaDataID>
        [PersistentMember(nameof(_GraphicMenuStorageIdentity))]
        [BackwardCompatibilityID("+4")]
        public string GraphicMenuStorageIdentity
        {
            get => _GraphicMenuStorageIdentity;
            set
            {
                if (_GraphicMenuStorageIdentity != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _GraphicMenuStorageIdentity = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }



        public override ServicePointType ServicePointType
        {
            get
            {
                base.ServicePointType = ServicePointType.TakeAway;
                return ServicePointType.TakeAway;
            }
            protected set
            {
            }
        }


        ///// <exclude>Excluded</exclude>
        //OOAdvantech.ObjectStateManagerLink StateManagerLink;
        ///// <exclude>Excluded</exclude>
        //string _Description;

        ///// <MetaDataID>{ddd8d4f1-14bf-4a64-935d-0d097b9cf192}</MetaDataID>
        //[PersistentMember(nameof(_Description))]
        //[BackwardCompatibilityID("+3")]
        //public string Description
        //{
        //    get => _Description;
        //    set
        //    {
        //        if (_Description!=value)
        //        {
        //            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
        //            {
        //                _Description=value;
        //                stateTransition.Consistent = true;
        //            }
        //        }

        //    }
        //}


        ///// <exclude>Excluded</exclude>
        //string _ServicesContextIdentity;


        ///// <MetaDataID>{f5ed2bd1-4318-494e-b105-be918048d738}</MetaDataID>
        //[PersistentMember(nameof(_ServicesContextIdentity))]
        //[BackwardCompatibilityID("+2")]
        //public string ServicesContextIdentity
        //{
        //    get => _ServicesContextIdentity;
        //    set
        //    {

        //        if (_ServicesContextIdentity!=value)
        //        {
        //            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
        //            {
        //                _ServicesContextIdentity=value;
        //                stateTransition.Consistent = true;
        //            }
        //        }

        //    }
        //}

        /// <exclude>Excluded</exclude>
        string _TakeAwayStationIdentity;

        /// <MetaDataID>{bb90f771-ef43-4bfa-94a5-6bb976474110}</MetaDataID>
        [PersistentMember(nameof(_TakeAwayStationIdentity))]
        [BackwardCompatibilityID("+1")]
        public string TakeAwayStationIdentity
        {
            get => _TakeAwayStationIdentity;
            set
            {

                if (_TakeAwayStationIdentity != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _TakeAwayStationIdentity = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <MetaDataID>{57170463-54e1-47ae-bec8-bf45b89aee41}</MetaDataID>
        public TakeAwayStation()
        {

        }

        /// <MetaDataID>{e010fb08-4247-46e9-a957-d9788f3bff6a}</MetaDataID>
        public TakeAwayStation(ServicesContextRunTime servicesContextRunTime)
        {
            _TakeAwayStationIdentity = servicesContextRunTime.ServicesContextIdentity + "_" + Guid.NewGuid().ToString("N");
            ServicesContextIdentity = servicesContextRunTime.ServicesContextIdentity;
        }

        /// <MetaDataID>{8820a755-040e-4924-a0f4-4b295b3cfd34}</MetaDataID>
        public override IFoodServiceClientSession NewFoodServiceClientSession(string clientName, string clientDeviceID, DeviceType deviceType, string deviceFirebaseToken)
        {
            AuthUserRef authUserRef = AuthUserRef.GetCallContextAuthUserRef(false);
            FlavourBusinessFacade.IUser user = null;

            if (authUserRef != null)
                user = authUserRef.GetContextRoleObject<TakeawayCashier>();
            var objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this);

            try
            {
                EndUsers.FoodServiceClientSession fsClientSession = null;
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
https://play.google.com/store/apps/details?id=com.arion.deliveries;sp=servicepointid_Mega 

//fsClientSession = ServicesContextRunTime.NewFoodServiceClientSession(fsClientSession);
                    fsClientSession = new EndUsers.FoodServiceClientSession();

                    fsClientSession.ClientName = clientName;
                    fsClientSession.ClientDeviceID = clientDeviceID;
                    fsClientSession.DeviceFirebaseToken = deviceFirebaseToken;
                    fsClientSession.SessionStarts = DateTime.UtcNow;
                    fsClientSession.ModificationTime = DateTime.UtcNow;
                    fsClientSession.PreviousYouMustDecideMessageTime = DateTime.UtcNow;

                    fsClientSession.ClientDeviceType = deviceType;
                    fsClientSession.SessionType = SessionType.Takeaway;


                    if (user != null && fsClientSession != null && user.Identity != fsClientSession.UserIdentity)
                        fsClientSession.UserIdentity = user.Identity;


                    ((user as TakeawayCashier).ShiftWork as ServingShiftWork).AddClientSession(fsClientSession);


                    fsClientSession.DateTimeOfLastRequest = DateTime.UtcNow;// DateTime.MinValue + TimeSpan.FromDays(28);
                    objectStorage.CommitTransientObjectState(fsClientSession);
                    fsClientSession.ServicePoint = this;

                    stateTransition.Consistent = true;
                }

                ServicesContextRunTime.Current.AddOpenServiceClientSession(fsClientSession);
                return fsClientSession;
            }
            catch (OOAdvantech.Transactions.TransactionException error)
            {
                throw;
            }
            catch (System.Exception error)
            {
                throw;
            }

        }

        /// <MetaDataID>{091ac110-e485-4b1f-ae86-89245ef6a966}</MetaDataID>
        public IList<UserData> GetNativeUsers()
        {
            return ServicePointRunTime.ServicesContextRunTime.Current.GetNativeUsers(RoleType.TakeAwayCashier);
        }
        /// <MetaDataID>{98f55080-0ecf-4ba3-91c4-bcce991dcd11}</MetaDataID>
        public UserData SignInNativeUser(string userName, string password)
        {

            return ServicePointRunTime.ServicesContextRunTime.Current.SignInNativeUser(userName, password);
        }

        /// <MetaDataID>{ae5013ad-1152-478f-8267-3f16c4aef816}</MetaDataID>
        public IFoodServiceClientSession GetUncommittedFoodServiceClientSession(string clientName, string clientDeviceID, DeviceType deviceType, string deviceFirebaseToken)
        {
            AuthUserRef authUserRef = AuthUserRef.GetCallContextAuthUserRef(false);
            IUser user = null;

            if (authUserRef != null)
                user = authUserRef.GetContextRoleObject<TakeawayCashier>();


            var uncommittedFoodServiceClientSession = this.OpenClientSessions.Where(x => x.ServicePoint == this && x.ClientDeviceID == clientDeviceID && ((int)x.FlavourItems.GetMinimumCommonItemPreparationState()) < (int)ItemPreparationState.Committed).OrderBy(x => x.SessionStarts).LastOrDefault();

            if (!((user as TakeawayCashier).ShiftWork is ServingShiftWork))
                throw new Exception("there is not active shift work");

            if (uncommittedFoodServiceClientSession != null && uncommittedFoodServiceClientSession.SessionCreator == ((user as TakeawayCashier).ShiftWork as ServingShiftWork))
            {
                uncommittedFoodServiceClientSession.ClientDeviceType = deviceType;
                return uncommittedFoodServiceClientSession;
            }

            else
                return NewFoodServiceClientSession(clientName, clientDeviceID, deviceType, deviceFirebaseToken);

        }

        public StationWatchingOrders GetWatchingOrders(List<WatchingOrderAbbreviation> candidateToRemoveWatchingOrders = null)
        {

            StationWatchingOrders callCenterStationWatchingOrders = new StationWatchingOrders();
            //List<WatchingOrderAbbreviation> removedWatchingOrders = new OOAdvantech.Collections.Generic.List<WatchingOrderAbbreviation>();
            if (candidateToRemoveWatchingOrders != null)
            {
                List<WatchingOrder> watchingOrders = WatchingOrders.ToList();
                callCenterStationWatchingOrders.WatchingOrders = watchingOrders.Where(x => !candidateToRemoveWatchingOrders.Any(y => y.SessionID == x.SessionID && y.TimeStamp == x.TimeStamp)).ToList();
                callCenterStationWatchingOrders.MissingWatchingOrders = candidateToRemoveWatchingOrders.Where(x => !watchingOrders.Any(y => y.SessionID == x.SessionID)).ToList();
                return callCenterStationWatchingOrders;
            }
            else
            {

                callCenterStationWatchingOrders.WatchingOrders = WatchingOrders.ToList();
                return callCenterStationWatchingOrders;
            }

        }


        public List<WatchingOrder> WatchingOrders
        {
            get
            {

                var foodServicesSessions = this.ActiveFoodServiceClientSessions.Where(x => x.SessionType == SessionType.Takeaway && x.MainSession != null && x.MainSession.Meal != null).Select(x => x.MainSession).Distinct().ToList();
                
                return (from foodServicesSession in foodServicesSessions
                        select new WatchingOrder()
                        {
                            SessionID = foodServicesSession.SessionID,
                            ClientPhone = foodServicesSession.PartialClientSessions.Where(x => x.SessionType == SessionType.HomeDelivery)?.FirstOrDefault()?.Client?.PhoneNumber,
                            ClientIdentity = foodServicesSession.PartialClientSessions.Where(x => x.SessionType == SessionType.HomeDelivery)?.FirstOrDefault()?.Client?.Identity,
                            SessionType = foodServicesSession.SessionType,
                            DeliveryPlace = foodServicesSession.DeliveryPlace,
                            EntryDateTime = foodServicesSession.SessionStarts,
                            ServicePoint = new ServicePointAbbreviation() { Description = Description,  ServicesContextIdentity = ServicesContextIdentity, ServicesPointIdentity = ServicesPointIdentity  },
                            MealCourses = foodServicesSession.Meal.Courses,
                            TimeStamp = (foodServicesSession.PartialClientSessions.OrderByDescending(x => x.ModificationTime).FirstOrDefault()?.ModificationTime.Ticks - new DateTime(2022, 1, 1).Ticks)?.ToString("x"),
                            State = WatchingOrderState.InProggres,
                            OrderTotal = Bill.GetTotal(foodServicesSession)

                        }).ToList();
            }
        }

    }
}