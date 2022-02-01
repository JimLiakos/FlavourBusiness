using System;
using System.Collections.Generic;
using System.Linq;
using FlavourBusinessFacade;
using FlavourBusinessFacade.EndUsers;
using FlavourBusinessFacade.HumanResources;
using FlavourBusinessFacade.RoomService;
using FlavourBusinessFacade.ServicesContextResources;
using OOAdvantech;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using RestaurantHallLayoutModel;

namespace FlavourBusinessManager.HumanResources
{
    /// <MetaDataID>{87975a3c-4a6e-499d-9f6e-426fa4e9510d}</MetaDataID>
    [BackwardCompatibilityID("{87975a3c-4a6e-499d-9f6e-426fa4e9510d}")]
    [Persistent()]
    public class Waiter : MarshalByRefObject, IWaiter, OOAdvantech.Remoting.IExtMarshalByRefObject
    {



        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<IAccountability> _Responsibilities = new OOAdvantech.Collections.Generic.Set<IAccountability>();

        /// <MetaDataID>{e1fe7b0c-8831-4a47-8268-ffece5ae1a3c}</MetaDataID>
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+20")]
        [PersistentMember(nameof(_Responsibilities))]
        public List<IAccountability> Responsibilities => _Responsibilities.ToThreadSafeList();

        /// <exclude>Excluded</exclude>
        string _PhotoUrl;


        /// <MetaDataID>{c9221896-e02e-402a-ad3f-c0b4cc2bba95}</MetaDataID>
        [PersistentMember(nameof(_PhotoUrl))]
        [BackwardCompatibilityID("+18")]
        public string PhotoUrl
        {
            get => _PhotoUrl;
            set
            {
                if (_PhotoUrl != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _PhotoUrl = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <exclude>Excluded</exclude>
        System.Collections.Generic.List<FlavourBusinessFacade.UserData.UserRole> _Roles;
        /// <MetaDataID>{92e38a83-4b5d-4fab-9250-b22e4590c223}</MetaDataID>
        public System.Collections.Generic.List<FlavourBusinessFacade.UserData.UserRole> Roles
        {
            get
            {
                if (_Roles == null)
                {
                    FlavourBusinessFacade.UserData.UserRole role = new FlavourBusinessFacade.UserData.UserRole() { User = this, RoleType = FlavourBusinessFacade.UserData.UserRole.GetRoleType(GetType().FullName) };
                    _Roles = new List<FlavourBusinessFacade.UserData.UserRole>() { role };
                }
                return _Roles;
            }
        }


        /// <exclude>Excluded</exclude>
        string _DeviceFirebaseToken;

        /// <MetaDataID>{d43f85ac-54e1-4edd-8bc9-a64ad26fe3ed}</MetaDataID>
        /// <summary>This token is the identity of device for push notification mechanism</summary>
        [PersistentMember(nameof(_DeviceFirebaseToken))]
        [BackwardCompatibilityID("+17")]
        public string DeviceFirebaseToken
        {
            get => _DeviceFirebaseToken;
            set
            {

                if (_DeviceFirebaseToken != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _DeviceFirebaseToken = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<Message> _Messages;


        /// <MetaDataID>{b2984f92-63e0-4be2-9d7c-810c2e5ad124}</MetaDataID>
        [PersistentMember(nameof(_Messages))]
        [BackwardCompatibilityID("+16")]
        [AssociationEndBehavior(PersistencyFlag.CascadeDelete)]
        public System.Collections.Generic.IList<FlavourBusinessFacade.EndUsers.Message> Messages => _Messages.ToThreadSafeList();



        /// <MetaDataID>{f8b89ddb-af69-46d9-8b2f-7a21cee43bec}</MetaDataID>
        public Message PeekMessage()
        {
            var message = Messages.OrderBy(x => x.MessageTimestamp).FirstOrDefault();
            if (message != null)
                message.MessageReaded = true;
            return message;
        }

        /// <MetaDataID>{cb391dfc-97a9-476f-972d-ab5eda1249ad}</MetaDataID>
        public Message PopMessage()
        {
            var message = Messages.OrderBy(x => x.MessageTimestamp).FirstOrDefault();
            if (message != null)
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _Messages.Remove(message);
                    stateTransition.Consistent = true;
                }
            }
            return message;
        }

        /// <MetaDataID>{56b74493-26c4-49fb-a015-cc3d8eaf3a49}</MetaDataID>
        public Message GetMessage(string messageId)
        {

            var message = Messages.Where(x => x.MessageID == messageId).FirstOrDefault();
            return message;
        }

        /// <MetaDataID>{71a4bb26-e916-4b25-b5cf-0f6ce3d7f662}</MetaDataID>
        public void RemoveMessage(string messageId)
        {
            var message = Messages.Where(x => x.MessageID == messageId).FirstOrDefault();
            if (message != null)
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _Messages.Remove(message);
                    stateTransition.Consistent = true;
                }
            }
        }

        /// <MetaDataID>{ef00f23c-5bfb-4499-a386-5d5e761ba66f}</MetaDataID>
        public void PushMessage(Message message)
        {

            if (message != null)
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this, TransactionOption.RequiresNew))
                {

                    message.MessageTimestamp = DateTime.UtcNow;
                    if (!_Messages.Contains(message))
                    {
                        _Messages.Add(message);
                        OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(message);
                    }

                    stateTransition.Consistent = true;
                }
            }
            _MessageReceived?.Invoke(this);

        }

        event MessageReceivedHandle _MessageReceived;
        public event MessageReceivedHandle MessageReceived
        {
            add
            {

                System.Diagnostics.Debug.WriteLine("******************************* add MessageReceived **************************************");
                System.Diagnostics.Debug.WriteLine("******************************* add MessageReceived **************************************");
                System.Diagnostics.Debug.WriteLine("******************************* add MessageReceived **************************************");

                _MessageReceived += value;
            }
            remove
            {
                System.Diagnostics.Debug.WriteLine("******************************* remove MessageReceived **************************************");
                System.Diagnostics.Debug.WriteLine("******************************* remove MessageReceived **************************************");
                System.Diagnostics.Debug.WriteLine("******************************* remove MessageReceived **************************************");

                _MessageReceived -= value;
            }
        }



        /// <MetaDataID>{cf9b15ec-6496-4bb1-bc81-5ccce5198d44}</MetaDataID>
        public System.Collections.Generic.IList<FlavourBusinessFacade.ServicesContextResources.IHallLayout> GetServiceHalls()
        {
            List<IHallLayout> halls = new System.Collections.Generic.List<FlavourBusinessFacade.ServicesContextResources.IHallLayout>();

            //OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this));

            //var servicesContextRunTime = (from theServicePointRun in storage.GetObjectCollection<ServicePointRunTime.ServicesContextRunTime>()
            //                              where theServicePointRun.ServicesContextIdentity == ServicesContextIdentity
            //                              select theServicePointRun).FirstOrDefault();


            foreach (var serviceArea in ServicePointRunTime.ServicesContextRunTime.Current.ServiceAreas.OfType<ServicesContextResources.ServiceArea>())
            {
                if (!string.IsNullOrWhiteSpace(serviceArea.HallLayoutUri))
                {
                    HallLayout hallLayout = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(serviceArea.HallLayoutUri) as HallLayout;


                    if (hallLayout == null)
                        continue;

                    hallLayout.ServiceArea = serviceArea;
                    foreach (var servicePointShape in hallLayout.Shapes.Where(x => !string.IsNullOrWhiteSpace(x.ServicesPointIdentity)))
                    {
                        var servicePoint = hallLayout.ServiceArea.ServicePoints.Where(x => x.ServicesPointIdentity == servicePointShape.ServicesPointIdentity).FirstOrDefault();
                        if (servicePoint != null)
                        {
                            servicePointShape.ServicesPointState = servicePoint.State;

                        }
                    }

                    halls.Add(hallLayout);
                }
            }
            return halls;
        }


        public Dictionary<string, ServicePointState> HallsServicePointsState
        {
            get
            {
                Dictionary<string, ServicePointState> hallsServicePointsStates = new Dictionary<string, ServicePointState>();

                foreach (var servicePoint in (from serviceArea in ServicePointRunTime.ServicesContextRunTime.Current.ServiceAreas
                                              from servicePoint in serviceArea.ServicePoints
                                              select servicePoint))
                {
                    hallsServicePointsStates[servicePoint.ServicesPointIdentity] = servicePoint.State;
                }

                return hallsServicePointsStates;
            }
        }

        /// <MetaDataID>{ee04afcf-db5b-40db-ba95-b5a0b416e255}</MetaDataID>
        public void AddClientSession(IFoodServiceClientSession clientSession)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _ClientSessions.Add(clientSession);
                stateTransition.Consistent = true;
            }


        }

        /// <MetaDataID>{88a3084c-5da2-4d5d-bf50-5a4d3604d60c}</MetaDataID>
        public void RemoveClientSession(IFoodServiceClientSession clientSession)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _ClientSessions.Remove(clientSession);
                stateTransition.Consistent = true;
            }
        }


        /// <exclude>Excluded</exclude>
        string _Identity;
        /// <MetaDataID>{68c436a3-8692-468d-9033-8e2918cde12a}</MetaDataID>
        [PersistentMember(nameof(_Identity))]
        [BackwardCompatibilityID("")]
        public string Identity
        {
            get
            {
                if (_Identity == null)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Identity = Guid.NewGuid().ToString("N");
                        stateTransition.Consistent = true;
                    }
                }
                return _Identity;
            }
        }

        /// <exclude>Excluded</exclude>
        bool _Suspended;

        /// <MetaDataID>{4a8a88cd-d90a-4845-acef-646d4775b0e0}</MetaDataID>
        [PersistentMember(nameof(_Suspended))]
        [BackwardCompatibilityID("+11")]
        public bool Suspended
        {
            get
            {
                return _Suspended;
            }
            set
            {
                if (_Suspended != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Suspended = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <exclude>Excluded</exclude>
        string _PhoneNumber;

        /// <MetaDataID>{701fdee0-553b-45ea-8b1c-e419e34b544f}</MetaDataID>
        [PersistentMember(nameof(_PhoneNumber))]
        [BackwardCompatibilityID("+10")]
        public string PhoneNumber
        {
            get => _PhoneNumber;
            set
            {
                if (_PhoneNumber != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _PhoneNumber = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <exclude>Excluded</exclude> 
        string _Email;
        /// <MetaDataID>{a6f6229b-7e85-4053-8a51-a05fb1beb90e}</MetaDataID>
        [PersistentMember(nameof(_Email))]
        [BackwardCompatibilityID("+9")]
        public string Email
        {
            get => _Email;
            set
            {
                if (_Email != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Email = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <MetaDataID>{356c3213-064d-48dd-bae6-02de9936bce0}</MetaDataID>
        [BackwardCompatibilityID("+8")]
        public string FullName
        {
            get => Name;
            set
            {
                Name = value;
            }
        }



        /// <exclude>Excluded</exclude>
        string _SignUpUserIdentity;
        /// <MetaDataID>{bc6a1bf7-4cea-4ce5-8b1b-eb338eb47fd3}</MetaDataID>
        [PersistentMember(nameof(_SignUpUserIdentity))]
        [BackwardCompatibilityID("+6")]
        public string SignUpUserIdentity
        {
            get => _SignUpUserIdentity;
            set
            {
                if (_SignUpUserIdentity != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _SignUpUserIdentity = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{2f0a1110-26d6-4c9d-975d-16ba26dd1bb2}</MetaDataID>
        string _WaiterAssignKey;

        /// <MetaDataID>{07b0929c-0a6c-4db4-bba0-d36b0b087714}</MetaDataID>
        [PersistentMember(nameof(_WaiterAssignKey))]
        [BackwardCompatibilityID("+5")]
        public string WaiterAssignKey
        {
            get => _WaiterAssignKey;
            set
            {
                if (_WaiterAssignKey != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _WaiterAssignKey = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        string _ServicesContextIdentity;

        /// <MetaDataID>{f6815bf9-b78e-45ee-b984-a6fb601c8210}</MetaDataID>
        [PersistentMember(nameof(_ServicesContextIdentity))]
        [BackwardCompatibilityID("+4")]
        public string ServicesContextIdentity
        {
            get => _ServicesContextIdentity;
            set
            {

                if (_ServicesContextIdentity != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ServicesContextIdentity = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }







        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <exclude>Excluded</exclude>
        string _Password;

        /// <MetaDataID>{7b6e9d9f-92ba-45bf-81e2-73f39ad00475}</MetaDataID>
        [PersistentMember(nameof(_Password))]
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+15")]
        public string Password
        {
            get => _Password;
            set
            {
                if (_Password != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Password = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }
        /// <exclude>Excluded</exclude>
        string _UserName;

        /// <MetaDataID>{0b2ce92c-5816-43af-a583-b4cac5623346}</MetaDataID>
        [PersistentMember(nameof(_UserName))]
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+2")]
        public string UserName
        {
            get => _UserName;
            set
            {
                if (_UserName != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _UserName = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        string _Name;

        /// <MetaDataID>{d5861bfd-b2c6-4476-8de2-c7d5ec63db95}</MetaDataID>
        [PersistentMember(nameof(_Name))]
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+1")]
        public string Name
        {
            get => _Name;
            set
            {

                if (_Name != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Name = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<IFoodServiceClientSession> _ClientSessions = new OOAdvantech.Collections.Generic.Set<IFoodServiceClientSession>();

        /// <MetaDataID>{a9a4b5ac-788b-41c0-90ba-de4ede06f133}</MetaDataID>
        [BackwardCompatibilityID("+13")]
        public System.Collections.Generic.List<FlavourBusinessFacade.EndUsers.IFoodServiceClientSession> ClientSessions
        {
            get
            {
                return _ClientSessions.ToThreadSafeList();
            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<IShiftWork> _ShiftWorks = new OOAdvantech.Collections.Generic.Set<IShiftWork>();

        /// <MetaDataID>{b1ab72ed-7ba8-4d95-b8b5-8f1c8d5e6a03}</MetaDataID>
        [PersistentMember(nameof(_ShiftWorks))]
        [BackwardCompatibilityID("+14")]
        public System.Collections.Generic.IList<FlavourBusinessFacade.HumanResources.IShiftWork> ShiftWorks => _ShiftWorks.ToThreadSafeList();



        List<ShiftWork> RecentlyShiftWorks;

        /// <MetaDataID>{7943daf2-7520-4f77-a801-ce925d7f689b}</MetaDataID>
        public IShiftWork ActiveShiftWork
        {
            get
            {
                var mileStoneDate = System.DateTime.UtcNow - TimeSpan.FromDays(1);
                var objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this);
                if (objectStorage != null)
                {
                    if (RecentlyShiftWorks == null)
                    {
                        OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(objectStorage);
                        RecentlyShiftWorks = (from shiftWork in storage.GetObjectCollection<ShiftWork>()
                                              where shiftWork.StartsAt > mileStoneDate && shiftWork.Worker == this
                                              select shiftWork).ToList();
                    }

                    if (RecentlyShiftWorks.Count > 0)
                    {

                    }


                    return RecentlyShiftWorks.OrderBy(x => x.StartsAt).LastOrDefault();
                }
                else
                    return _ShiftWorks.ToThreadSafeList().Where(x => x.StartsAt > mileStoneDate).Last();
            }
        }

        /// <MetaDataID>{94047cd9-7a47-475c-a1bb-f949eee63d45}</MetaDataID>
        internal void AddShiftWork(ShiftWork shiftWork)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _ShiftWorks.Add(shiftWork);

                stateTransition.Consistent = true;
            }
            RecentlyShiftWorks = null;

        }


        /// <exclude>Excluded</exclude>
        ServicePointRunTime.ServicesContextRunTime _ServicesContextRunTime;

        public event ObjectChangeStateHandle ObjectChangeState;

        /// <MetaDataID>{cb7ba774-f6be-425a-9a14-c2bacf826387}</MetaDataID>
        public ServicePointRunTime.ServicesContextRunTime ServicesContextRunTime
        {
            get
            {
                if (_ServicesContextRunTime == null)
                {
                    var objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this);
                    if (objectStorage != null)
                    {
                        OOAdvantech.Linq.Storage linqStorage = new OOAdvantech.Linq.Storage(objectStorage);
                        _ServicesContextRunTime = (from serviceContext in linqStorage.GetObjectCollection<ServicePointRunTime.ServicesContextRunTime>()
                                                   select serviceContext).FirstOrDefault();
                    }
                }
                return _ServicesContextRunTime;
            }
        }

        /// <MetaDataID>{2e5555c3-9db4-4389-a567-4e653cee8291}</MetaDataID>
        [BackwardCompatibilityID("+21")]
        public List<IAccountability> Commissions => new List<IAccountability>();





        /// <MetaDataID>{9a28bc22-a7e4-4f96-8a1b-7fb80055c3f4}</MetaDataID>
        public void RemoveShiftWork(IShiftWork shiftWork)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _ShiftWorks.Remove(shiftWork);
                stateTransition.Consistent = true;
            }
            RecentlyShiftWorks = null;

        }


        /// <MetaDataID>{b6f48ee9-e125-4363-ac9f-a248a0f71f8f}</MetaDataID>
        public IShiftWork NewShiftWork(DateTime startedAt, double timespanInHours)
        {
            ShiftWork shiftWork = null;
            using (SystemStateTransition stateTransition = new SystemStateTransition())
            {
                shiftWork = new ServingShiftWork(Name);
                OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(shiftWork);
                shiftWork.StartsAt = startedAt;
                shiftWork.PeriodInHours = timespanInHours;
                AddShiftWork(shiftWork);
                stateTransition.Consistent = true;
            }
            ObjectChangeState?.Invoke(this, nameof(ActiveShiftWork));


            ServicePointRunTime.ServicesContextRunTime.Current.WaiterSiftWorkUpdated(this);

            return shiftWork;
            //return this.ServicesContextRunTime.NewShifWork(this, startedAt, timespanInHours);
        }

        /// <MetaDataID>{968412d0-8e06-4695-bd7d-3b1b29168195}</MetaDataID>
        public void ChangeSiftWork(IShiftWork shiftWork, DateTime startedAt, double timespanInHours)
        {
            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                (shiftWork as ShiftWork).StartsAt = startedAt;
                (shiftWork as ShiftWork).PeriodInHours = timespanInHours;

                stateTransition.Consistent = true;
            }
            ObjectChangeState?.Invoke(this, nameof(ActiveShiftWork));
            ServicePointRunTime.ServicesContextRunTime.Current.WaiterSiftWorkUpdated(this);
        }

        /// <MetaDataID>{974d10d9-2579-492d-b939-10fe4244c319}</MetaDataID>
        public IList<IServingBatch> GetServingBatches()
        {
            return (ServicesContextRunTime.MealsController as RoomService.MealsController).GetServingBatches(this).OfType<IServingBatch>().ToList();
        }

        /// <MetaDataID>{f5de8f49-f437-4d88-8daa-403e6aefce51}</MetaDataID>
        public ServingBatchUpdates GetServingUpdates(List<ItemPreparationAbbreviation> servingItemsOnDevice)
        {

            List<RoomService.ServingBatch> servingBatches = (ServicesContextRunTime.MealsController as RoomService.MealsController).GetServingBatches(this).ToList();

            var itemsToServe = (from servingBatch in servingBatches
                                from itemsContext in servingBatch.ContextsOfPreparedItems
                                from itemPreparation in itemsContext.PreparationItems
                                select new { servingBatch, itemsContext, itemPreparation }).ToList();
            itemsToServe.AddRange((from servingBatch in servingBatches
                                   from itemsContext in servingBatch.ContextsOfUnderPreparationItems
                                   from itemPreparation in itemsContext.PreparationItems
                                   select new { servingBatch, itemsContext, itemPreparation }).ToList());
            foreach (var itemToServe in itemsToServe.ToList())
            {
                var servingItemOnDevice = servingItemsOnDevice.Where(x => x.uid == itemToServe.itemPreparation.uid).FirstOrDefault();
                if (servingItemOnDevice != null)
                {
                    itemsToServe.Remove(itemToServe);
                    servingItemsOnDevice.Remove(servingItemOnDevice);
                }
            }
            servingBatches = itemsToServe.Select(x => x.servingBatch).Distinct().ToList();

            return new ServingBatchUpdates(servingBatches.OfType<IServingBatch>().ToList(), servingItemsOnDevice);
        }


        /// <MetaDataID>{16d6364d-018c-425d-808a-b663578c95b8}</MetaDataID>
        public void AssignServingBatch(IServingBatch servingBatch)
        {
            if (ActiveShiftWork is ServingShiftWork)
            {
                lock (servingBatch)
                {
                    if (!servingBatch.IsAssigned)
                        (ActiveShiftWork as ServingShiftWork).AddServingBatch(servingBatch);
                }
                (ServicesContextRunTime.MealsController as RoomService.MealsController).ServingBatchAssigned(this, servingBatch);
            }


        }


        /// <MetaDataID>{e75ecf5b-3567-448e-8424-6ee02d26f10f}</MetaDataID>
        public void DeassignServingBatch(IServingBatch servingBatch)
        {
            var mealCourse = servingBatch.MealCourse;
            var preparedItems = servingBatch.ContextsOfPreparedItems;
            var underPreparationItems = servingBatch.ContextsOfUnderPreparationItems;
            OOAdvantech.PersistenceLayer.ObjectStorage.DeleteObject(servingBatch);

            (servingBatch as RoomService.ServingBatch).Update(mealCourse, preparedItems, underPreparationItems);
            (ServicesContextRunTime.MealsController as RoomService.MealsController).ServingBatchDeassigned(this, servingBatch);





        }
        public event ServingBatchesChangedHandler ServingBatchesChanged;


        /// <MetaDataID>{63737ce6-b491-4fe4-a179-4fc82c6063f6}</MetaDataID>
        internal void RaiseServingBatchesChangedEvent()
        {
            ServingBatchesChanged?.Invoke();
        }


        /// <MetaDataID>{13795273-fe1b-4d00-b840-bb8bdcfda520}</MetaDataID>
        public void ServingBatchesCommit()
        {

        }

        /// <MetaDataID>{db723eb2-7265-4f9c-b34e-e3a4c41d49c7}</MetaDataID>
        public void CommitServingBatches()
        {
            if (ActiveShiftWork is ServingShiftWork)
            {
                lock (this)
                {

                    try
                    {
                        using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                        {
                            foreach (var servingBatch in GetServingBatches().OfType<RoomService.ServingBatch>().Where(x => x.State == ItemPreparationState.Serving && x.IsAssigned && x.ShiftWork.Worker == this))
                                servingBatch.OnTheRoad();


                            //foreach (var servingBatch in (ActiveShiftWork as ServingShiftWork).ServingBatches.Where(x => x.State == ItemPreparationState.Serving))
                            //    servingBatch.OnTheRoad();

                            stateTransition.Consistent = true;
                        }
                    }
                    catch (Exception error)
                    {

                        throw;
                    }
                }
            }
        }

        /// <MetaDataID>{47d8cdac-cb47-4f92-8fc1-79816021480c}</MetaDataID>
        public void TransferItems(IFoodServiceSession foodServiceSession, List<string> itemsPreparationsIDs, string targetServicePointIdentity)
        {



            //ObjectChangeState?.Invoke(this, nameof(HallsServicePointsState));
        }

        /// <MetaDataID>{7adedeba-6042-4f69-99c9-bf6718e17f60}</MetaDataID>
        public void TransferSession(IFoodServiceSession foodServiceSession, string targetServicePointIdentity)
        {
            if (foodServiceSession.ServicePoint.ServicesPointIdentity == targetServicePointIdentity)
                return;


            var targetServicePoint = (from serviceArea in ServicePointRunTime.ServicesContextRunTime.Current.ServiceAreas
                                      from servicePoint in serviceArea.ServicePoints
                                      where servicePoint.ServicesPointIdentity == targetServicePointIdentity
                                      select servicePoint).OfType<ServicesContextResources.ServicePoint>().FirstOrDefault();

            if (targetServicePoint == null)
                throw new ArgumentException("There is no service with identity, the value of 'targetServicePointIdentity' parameter");
            else
            {
                (foodServiceSession as ServicesContextResources.FoodServiceSession).ServicePoint = targetServicePoint;

                targetServicePoint.UpdateState();
                
                if(foodServiceSession.Meal!=null)
                    (ServicePointRunTime.ServicesContextRunTime.Current.MealsController as RoomService.MealsController).ReadyToServeMealcoursesCheck(foodServiceSession.Meal.Courses);


            }



            //ObjectChangeState?.Invoke(this, nameof(HallsServicePointsState));

        }

        //public IShifWork NewShifWork(System.DateTime startedAt, double timespanInHours)
        //{
        //    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
        //    {
        //        ShifWork shifWork = new ShifWork(this._Name);
        //        OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(shifWork);
        //        shifWork.StartsAt = startedAt.ToUniversalTime();
        //        shifWork.PeriodInHours = timespanInHours;
        //        _ShifWorks.Add(shifWork);
        //        stateTransition.Consistent = true;
        //        return shifWork;
        //    }
        //}
    }
}