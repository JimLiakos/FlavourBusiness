using FlavourBusinessFacade.EndUsers;
using FlavourBusinessFacade.HumanResources;
using FlavourBusinessFacade.RoomService;
using FlavourBusinessFacade.ServicesContextResources;
using OOAdvantech;
using OOAdvantech.MetaDataRepository;
using System.Collections.Generic;
using System;
using System.Linq;
using FlavourBusinessManager.RoomService;
using OOAdvantech.Transactions;
using FlavourBusinessManager.EndUsers;

namespace FlavourBusinessManager.Shipping
{
    /// <MetaDataID>{c36022d2-142c-4f52-8ca3-debd1124b925}</MetaDataID>
    [BackwardCompatibilityID("{c36022d2-142c-4f52-8ca3-debd1124b925}")]
    [Persistent()]
    public class FoodShipping : MarshalByRefObject, OOAdvantech.Remoting.IExtMarshalByRefObject, FlavourBusinessFacade.Shipping.IFoodShipping, FinanceFacade.IPaymentGateway
    {

        /// <exclude>Excluded</exclude>
        DateTime? _DeliveryTime;
        /// <MetaDataID>{b3e9e858-b3da-4308-ae57-b8ab33179300}</MetaDataID>
        [PersistentMember(nameof(_DeliveryTime))]
        [BackwardCompatibilityID("+15")]
        public DateTime? DeliveryTime
        {
            get=>_DeliveryTime;
            set
            {
                if (_DeliveryTime!=value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _DeliveryTime=value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <exclude>Excluded</exclude>
        string _DistributionIdentity;
        /// <MetaDataID>{227bd8d0-f43a-4d3b-b9c3-906d1a0f9f79}</MetaDataID>
        [PersistentMember(nameof(_DistributionIdentity))]
        [BackwardCompatibilityID("+14")]
        public string DistributionIdentity
        {
            get => _DistributionIdentity;
            set
            {
                if (_DistributionIdentity != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _DistributionIdentity = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        string _ReturnReason;

        /// <MetaDataID>{64b865f4-304e-4e10-908e-23d6b22f6c9a}</MetaDataID>
        [PersistentMember(nameof(_ReturnReason))]
        [BackwardCompatibilityID("+13")]
        public string ReturnReason
        {
            get => _ReturnReason;
            set
            {
                if (_ReturnReason != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ReturnReason = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        string _ReturnReasonID;
        /// <MetaDataID>{77047fd4-36e9-479e-97e7-df10bf2756f7}</MetaDataID>
        [PersistentMember(nameof(_ReturnReasonID))]
        [BackwardCompatibilityID("+12")]
        public string ReturnReasonID
        {
            get => _ReturnReasonID;
            set
            {
                if (_ReturnReasonID != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ReturnReasonID = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <summary>
        /// Creates a pay order with assigned payment gateway 
        /// In case where payment can be completed from refunds amount, the payment completed without pay order.  
        /// </summary>
        /// <param name="payment">
        /// Payment has all data that needed for the creation of pay order   
        /// </param>
        /// <param name="tipAmount">
        /// Defines the extra amount for tipping
        /// </param>
        /// <param name="paramsJson">
        /// Defines some extra parameters which are necessary for payment gateway
        /// </param>
        /// <MetaDataID>{8cbfdfe4-c18e-407c-863f-074e5268a9c8}</MetaDataID>
        public void CreatePaymentOrder(FinanceFacade.IPayment payment, decimal tipAmount, string paramsJson)
        {
            if (payment.State != FinanceFacade.PaymentState.Completed)
            {
                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {
                    if (!(payment as FinanceFacade.Payment).TryToCompletePaymentWithRefundAmount(tipAmount))
                    {
                        PaymentProviders.VivaWallet.CreatePaymentOrder(payment as FinanceFacade.Payment, tipAmount, paramsJson);
                        (payment as FinanceFacade.Payment).TipsAmount = tipAmount;
                    }

                    stateTransition.Consistent = true;
                }

            }
        }


        /// <exclude>Excluded</exclude>
        string _Identity;

        /// <MetaDataID>{c3f2b2b1-64f8-4f3c-b7c6-1d0156e6958b}</MetaDataID>
        [PersistentMember(nameof(_Identity))]
        [BackwardCompatibilityID("+7")]
        public string Identity => _Identity;


        /// <exclude>Excluded</exclude>
        DateTime? _CreationTime;

        /// <MetaDataID>{836f9e8f-edf4-4c9a-8fdf-bb7a4c4418b6}</MetaDataID>
        [PersistentMember(nameof(_CreationTime))]
        [BackwardCompatibilityID("+5")]
        public DateTime? CreationTime
        {
            get => _CreationTime;
            set
            {
                if (_CreationTime != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _CreationTime = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<IItemPreparation> _PreparedItems = new OOAdvantech.Collections.Generic.Set<IItemPreparation>();

        /// <MetaDataID>{f439725a-bc72-4521-8ca4-44418cfeaad1}</MetaDataID>
        [PersistentMember(nameof(_PreparedItems))]
        [BackwardCompatibilityID("+3")]
        public List<IItemPreparation> PreparedItems => _PreparedItems.ToThreadSafeList();

        /// <exclude>Excluded</exclude>
        OOAdvantech.Member<FlavourBusinessFacade.HumanResources.IServingShiftWork> _ShiftWork = new Member<FlavourBusinessFacade.HumanResources.IServingShiftWork>();


        /// <MetaDataID>{9f5189cc-ab0f-41db-a31c-9e81926d4412}</MetaDataID>
        [PersistentMember(nameof(_ShiftWork))]
        [BackwardCompatibilityID("+4")]
        [CachingOnlyReferenceOnClientSide]
        public FlavourBusinessFacade.HumanResources.IServingShiftWork ShiftWork => _ShiftWork.Value;


        /// <exclude>Excluded</exclude>
        ServicePointType? _ServicePointType;

        /// <MetaDataID>{1494396b-fb28-4280-83ad-ee1c39c18f1b}</MetaDataID>
        [PersistentMember(nameof(_ServicePointType))]
        [BackwardCompatibilityID("+10")]
        [CachingDataOnClientSide]
        public ServicePointType ServicePointType
        {
            get
            {
                if (_ServicePointType.HasValue)
                    return _ServicePointType.Value;
                else
                {
                    ServicePointType = MealCourse.Meal.Session.ServicePoint.ServicePointType;
                    return _ServicePointType.Value;
                }
            }

            set
            {
                if (_ServicePointType != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ServicePointType = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }



        /// <MetaDataID>{0d8d1c78-9a94-491b-9bf3-0b88e4b4c183}</MetaDataID>
        public FoodShipping()
        {

        }

        /// <MetaDataID>{dc5a7887-71e5-4ba2-8b4e-db106a05d8bc}</MetaDataID>
        [BeforeCommitObjectStateInStorageCall]
        public void BeforeCommitObjectState()
        {
            if (_CreationTime != null)
            {
                _CreationTime = DateTime.UtcNow;
            }
            if (string.IsNullOrWhiteSpace(_Identity))
            {
                var ticks = new DateTime(2022, 1, 1).Ticks;
                var uniqueId = (DateTime.Now.Ticks - ticks).ToString("x");
                _Identity = uniqueId;
            }
            lock (CaregiversLock)
            {
                Caregiversjson = OOAdvantech.Json.JsonConvert.SerializeObject(_Caregivers);
            }
        }

        /// <MetaDataID>{62b80657-2b59-46c6-8b79-9a871b5c9bcd}</MetaDataID>
        [ObjectActivationCall]
        public void ObjectActivation()
        {

            lock (CaregiversLock)
            {
                if (!string.IsNullOrWhiteSpace(Caregiversjson))
                    _Caregivers = OOAdvantech.Json.JsonConvert.DeserializeObject<List<Caregiver>>(Caregiversjson);


            }

        }

        /// <MetaDataID>{124c5529-dd6b-429c-8772-17858121bb7a}</MetaDataID>
        public FoodShipping(IMealCourse mealCourse, IList<ItemsPreparationContext> preparedItems, IList<ItemsPreparationContext> underPreparationItems)
        {
            if (mealCourse.ServingBatches.Count > 0)
            {
                System.Diagnostics.Debug.Assert(false, "Meal course without serving batches allowed");
                throw new ArgumentException("Meal course without serving batches allowed", nameof(mealCourse));
            }

            MealCourseUri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(mealCourse)?.GetPersistentObjectUri(mealCourse);
            _MealCourse.Value = mealCourse;

            ContextsOfPreparedItems = preparedItems;
            ContextsOfUnderPreparationItems = underPreparationItems;
            ServicePoint = mealCourse.Meal.Session.ServicePoint;
            ServicesPointIdentity = ServicePoint.ServicesPointIdentity;



            Description = mealCourse.Meal.Session.Description + " - " + mealCourse.Name;

            if (MealCourse != null)
                MealCourse.ObjectChangeState += MealCourseChangeState;


            //Description = mealCourse.Name + " " + ServicePoint.ServiceArea.Description + " / " + ServicePoint.Description;
        }

        /// <MetaDataID>{f0183d98-c51d-466e-8538-e5653b30bad2}</MetaDataID>
        public ItemPreparationState State
        {
            get
            {
                if (this.PreparedItems.Where(x => x.State == ItemPreparationState.Serving).Count() == this.PreparedItems.Count && this.PreparedItems.Count > 0)
                    return ItemPreparationState.Serving;
                if (this.PreparedItems.Where(x => x.State == ItemPreparationState.OnRoad).Count() == this.PreparedItems.Count && this.PreparedItems.Count > 0)
                    return ItemPreparationState.OnRoad;

                if (this.PreparedItems.OfType<ItemPreparation>().All(x => x.IsInFollowingState(ItemPreparationState.Served))&& this.PreparedItems.Count > 0)
                    return ItemPreparationState.Served;

                if (this.PreparedItems.OfType<ItemPreparation>().All(x => x.IsInFollowingState(ItemPreparationState.Canceled))&& this.PreparedItems.Count > 0)
                    return ItemPreparationState.Canceled;

                return ItemPreparationState.Serving;
            }
        }
        /// <MetaDataID>{f593f136-b694-4dfb-a0cd-de10527a806d}</MetaDataID>
        internal void OnItemsChangeState(Dictionary<string, ItemPreparationState> newItemsState)
        {
            ItemsStateChanged?.Invoke(newItemsState);
        }

        /// <MetaDataID>{a2f2759f-6f41-4df8-94e4-2e4d8c4b5d03}</MetaDataID>
        internal void OnTheRoad()
        {
            var states = PreparedItems.ToDictionary(x => x.uid, x => x.State);
            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                foreach (var itemPreparation in PreparedItems.OfType<ItemPreparation>())
                {
                    itemPreparation.State = ItemPreparationState.OnRoad;
                    itemPreparation.StateTimestamp = DateTime.UtcNow;
                }

                states = PreparedItems.ToDictionary(x => x.uid, x => x.State);
                stateTransition.Consistent = true;
            }


            Transaction.RunOnTransactionCompleted(() =>
            {
                var newItemsState = PreparedItems.ToDictionary(x => x.uid, x => x.State);
                ItemsStateChanged?.Invoke(newItemsState);
                (MealCourse as MealCourse).RaiseItemsStateChanged(newItemsState);

            });

        }


        /// <MetaDataID>{3cd2eb19-4ee2-47f6-b6c6-8c687a775797}</MetaDataID>
        private void MealCourseChangeState(object _object, string member)
        {


            IList<ItemsPreparationContext> preparedItems = (from itemsPreparationContext in MealCourse.FoodItemsInProgress
                                                            where itemsPreparationContext.PreparationItems.OfType<ItemPreparation>().All(x => x.IsIntheSameOrFollowingState(ItemPreparationState.Serving))
                                                            select itemsPreparationContext).ToList();

            IList<ItemsPreparationContext> underPreparationItems = (from itemsPreparationContext in MealCourse.FoodItemsInProgress
                                                                    where itemsPreparationContext.PreparationItems.Any(x => x.State == ItemPreparationState.PendingPreparation ||
                                                                    x.State == ItemPreparationState.InPreparation ||
                                                                    x.State == ItemPreparationState.IsRoasting ||
                                                                    x.State == ItemPreparationState.IsPrepared)
                                                                    select itemsPreparationContext).ToList();
            var newPreparationItems = (from itemsPreparationContext in preparedItems
                                       from itemPreparation in itemsPreparationContext.PreparationItems
                                       select itemPreparation).ToList();

            var existingPreparationItems = (from itemsPreparationContext in ContextsOfPreparedItems
                                            from itemPreparation in itemsPreparationContext.PreparationItems
                                            select itemPreparation).ToList();

            bool servingBatchChanged = false;

            if (newPreparationItems.Where(x => !existingPreparationItems.Contains(x)).Count() != 0)
                servingBatchChanged = true;
            else if (existingPreparationItems.Where(x => !newPreparationItems.Contains(x)).Count() != 0)
                servingBatchChanged = true;

            if (!servingBatchChanged)
            {
                newPreparationItems = (from itemsPreparationContext in underPreparationItems
                                       from itemPreparation in itemsPreparationContext.PreparationItems
                                       select itemPreparation).ToList();

                existingPreparationItems = (from itemsPreparationContext in ContextsOfUnderPreparationItems
                                            from itemPreparation in itemsPreparationContext.PreparationItems
                                            select itemPreparation).ToList();
                if (newPreparationItems.Where(x => !existingPreparationItems.Contains(x)).Count() != 0)
                    servingBatchChanged = true;
                else if (existingPreparationItems.Where(x => !newPreparationItems.Contains(x)).Count() != 0)
                    servingBatchChanged = true;
            }
            if (nameof(ServicesContextResources.FoodServiceSession.ServicePoint) == member || nameof(RoomService.MealCourse.Meal) == member)
            {
                this.ServicePoint = MealCourse.Meal.Session.ServicePoint;
                Description = MealCourse.Meal.Session.Description + " - " + MealCourse.Name;
                this.ServicesPointIdentity = this.ServicePoint.ServicesPointIdentity;


            }
            if (servingBatchChanged || nameof(ServicesContextResources.FoodServiceSession.ServicePoint) == member)
            {
                ContextsOfPreparedItems = preparedItems;
                ContextsOfUnderPreparationItems = underPreparationItems;
                ObjectChangeState?.Invoke(this, null);
            }
            if (servingBatchChanged || nameof(RoomService.MealCourse.Meal) == member)
            {

                ContextsOfPreparedItems = preparedItems;
                ContextsOfUnderPreparationItems = underPreparationItems;
                ObjectChangeState?.Invoke(this, null);
            }


        }
        /// <exclude>Excluded</exclude>
        Member<IMealCourse> _MealCourse = new Member<IMealCourse>();

        /// <MetaDataID>{ebff5c16-abe5-4d72-bcd2-dcca20ca7fa7}</MetaDataID>
        [PersistentMember(nameof(_MealCourse))]
        [BackwardCompatibilityID("+1")]
        public IMealCourse MealCourse => _MealCourse.Value;



        /// <exclude>Excluded</exclude>
        bool? _IsAssigned;
        /// <MetaDataID>{d8834cae-2fde-4861-81b0-6be2a5c26f12}</MetaDataID>
        [PersistentMember(nameof(_IsAssigned))]
        [BackwardCompatibilityID("+11")]
        [CachingDataOnClientSide]
        public bool IsAssigned
        {
            get
            {
                if (!_ShiftWork.UnInitialized)
                    IsAssigned = ShiftWork != null;

                if (_IsAssigned == null)
                    IsAssigned = ShiftWork != null;
                return _IsAssigned.Value;
            }
            set
            {

                if (_IsAssigned != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _IsAssigned = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <MetaDataID>{b9ced204-54cb-4c4b-8eb7-5d854a577b56}</MetaDataID>
        [ObjectsLinkCall]
        void ObjectsLink(object linkedObject, AssociationEnd associationEnd, bool added)
        {
            if (associationEnd.Name == nameof(ShiftWork))
            {
                if (ShiftWork != null)
                    IsAssigned = true;
                else
                    IsAssigned = false;
            }
        }


        /// <MetaDataID>{a6d410ca-433e-4bc2-a60d-2902bbf42126}</MetaDataID>
        [CachingDataOnClientSide]
        public int SortID { get; internal set; }

        /// <MetaDataID>{f1770ae5-6716-476d-80cb-1542934f27e6}</MetaDataID>
        public string MealCourseUri { get; private set; }


        /// <exclude>Excluded</exclude>
        IServicePoint _ServicePoint;

        /// <MetaDataID>{c31ace5f-8e90-4682-a38c-e374a519580d}</MetaDataID>
        [OnDemandCachingDataOnClientSide]
        public IServicePoint ServicePoint
        {
            get
            {
                if (_ServicePoint == null)
                    _ServicePoint=MealCourse?.Meal?.Session?.ServicePoint;
                return _ServicePoint;
            }
            set { _ServicePoint = value; }
        }


        /// <exclude>Excluded</exclude>
        private IList<ItemsPreparationContext> _ContextsOfUnderPreparationItems;

        /// <MetaDataID>{81c8d3f1-bdf1-422b-9c79-baf0371f329a}</MetaDataID>
        [CachingDataOnClientSide]
        public IList<ItemsPreparationContext> ContextsOfUnderPreparationItems
        {
            get
            {
                if (_ContextsOfUnderPreparationItems == null)
                {
                    if (_MealCourse != null)
                    {
                        _ContextsOfUnderPreparationItems = (from itemsPreparationContext in MealCourse.FoodItemsInProgress
                                                            where itemsPreparationContext.PreparationItems.Any(x => x.State == ItemPreparationState.PendingPreparation ||
                                                            x.State == ItemPreparationState.InPreparation ||
                                                            x.State == ItemPreparationState.IsRoasting ||
                                                            x.State == ItemPreparationState.IsPrepared)
                                                            select itemsPreparationContext).ToList();

                    }
                    else
                        _ContextsOfUnderPreparationItems = new List<ItemsPreparationContext>();
                }

                return _ContextsOfUnderPreparationItems;
            }
            set
            {
                _ContextsOfUnderPreparationItems = value;
            }
        }
        /// <exclude>Excluded</exclude>
        IList<ItemsPreparationContext> _ContextsOfPreparedItems = null;

        /// <MetaDataID>{510a7954-2c31-4a10-a898-e2e1a004818d}</MetaDataID>
        [CachingDataOnClientSide]
        public IList<ItemsPreparationContext> ContextsOfPreparedItems
        {
            get
            {
                if (_ContextsOfPreparedItems == null)
                {
                    if (_MealCourse != null)
                        _ContextsOfPreparedItems = _MealCourse.Value.FoodItemsInProgress.Where(x => x.PreparationItems.All(y => y.ServedInTheBatch == this)).ToList();
                    else
                        _ContextsOfPreparedItems = new List<ItemsPreparationContext>();
                }
                return _ContextsOfPreparedItems;
            }
            set
            {
                _ContextsOfPreparedItems = value;
            }
        }

        /// <exclude>Excluded</exclude>
        string _Description;

        /// <MetaDataID>{c83578bb-5b53-459b-949d-362b78db48fd}</MetaDataID>
        [PersistentMember(nameof(_Description))]
        [BackwardCompatibilityID("+9")]
        [CachingDataOnClientSide]
        public string Description
        {
            get => _Description;
            set
            {
                if (_Description != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Description = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        string _ServicesPointIdentity;
        /// <MetaDataID>{effa589d-8157-44af-951c-4ca5c212dff6}</MetaDataID>
        [PersistentMember(nameof(_ServicesPointIdentity))]
        [BackwardCompatibilityID("+8")]
        [OnDemandCachingDataOnClientSide]
        public string ServicesPointIdentity
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_ServicesPointIdentity))
                {
                    ServicesPointIdentity = MealCourse.Meal.Session.ServicePoint.ServicesPointIdentity;

                }
                return _ServicesPointIdentity;
            }
            set
            {
                if (_ServicesPointIdentity != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ServicesPointIdentity = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <MetaDataID>{063af7ed-744e-447a-8a4b-96f75450d11a}</MetaDataID>
        [CachingDataOnClientSide]
        public string ServicesContextIdentity { get => ServicePointRunTime.ServicesContextRunTime.Current.ServicesContextIdentity; }



        /// <MetaDataID>{8b04d3b5-3066-4ac7-b863-258938fa98b8}</MetaDataID>
        [CachingDataOnClientSide]
        public IPlace Place
        {
            get
            {
                return MealCourse.Meal.Session.DeliveryPlace;
            }
            set
            {
            }
        }
        /// <MetaDataID>{f7552ffa-c40a-44f4-96d5-4196cc0361df}</MetaDataID>
        public string ClientFullName
        {
            get
            {
                string clientFullName = MealCourse.Meal.Session.PartialClientSessions.Where(x => x.SessionType == SessionType.HomeDelivery).FirstOrDefault()?.Client?.FullName;
                if (string.IsNullOrEmpty(clientFullName))
                    clientFullName = MealCourse.Meal.Session.PartialClientSessions.Where(x => x.SessionType == SessionType.HomeDelivery).FirstOrDefault()?.Client?.FriendlyName;
                return clientFullName;
            }
            set
            {
            }
        }
        /// <MetaDataID>{1809fd16-ec0f-44e6-b1d5-336f64e9af0d}</MetaDataID>
        public string PhoneNumber
        {
            get
            {
                return MealCourse.Meal.Session.PartialClientSessions.Where(x => x.SessionType == SessionType.HomeDelivery).FirstOrDefault()?.Client?.PhoneNumber;
            }
            set
            {
            }
        }

        /// <MetaDataID>{e07fd78f-f3f3-4bfe-ad3a-74bea31fc193}</MetaDataID>
        public string DeliveryRemark
        {
            get
            {
                return MealCourse.Meal.Session.PartialClientSessions.Where(x => x.SessionType == SessionType.HomeDelivery).FirstOrDefault()?.DeliveryComment;
            }
            set
            {
            }
        }
        /// <MetaDataID>{af2a9407-0611-4b59-bc64-801c3182a6aa}</MetaDataID>
        public string NotesForClient
        {
            get
            {
                return MealCourse.Meal.Session.PartialClientSessions.Where(x => x.SessionType == SessionType.HomeDelivery).FirstOrDefault()?.Client?.NotesForClient;
            }
            set
            {
            }
        }

        public event ObjectChangeStateHandle ObjectChangeState;
        public event ItemsStateChangedHandle ItemsStateChanged;

        /// <MetaDataID>{aafc5519-34b2-4c81-9379-a37e19b52226}</MetaDataID>
        [CommitObjectStateInStorageCall]
        void CommitObjectState()
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                foreach (var item in (from itemsPreprationContext in ContextsOfPreparedItems
                                      from itemPreparation in itemsPreprationContext.PreparationItems
                                      select itemPreparation))
                {
                    _PreparedItems.Add(item);
                }
                stateTransition.Consistent = true;
            }
        }

        /// <MetaDataID>{36db5a26-d4bc-4f1c-af8f-13cfc237c9f5}</MetaDataID>
        [DeleteObjectCall]
        void ObjectDeleting()
        {
            if (MealCourse != null)
                MealCourse.ObjectChangeState -= MealCourseChangeState;

        }




        /// <MetaDataID>{cee036f2-fe3c-4668-ac0b-b3250d86ea33}</MetaDataID>
        public void PrintReceiptAgain()
        {
            var transactionUri = PreparedItems.OfType<ItemPreparation>().Where(x => !string.IsNullOrWhiteSpace(x.TransactionUri)).FirstOrDefault()?.TransactionUri;
            if (!string.IsNullOrWhiteSpace(transactionUri))
            {
                var transaction = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri<FinanceFacade.Transaction>(transactionUri);
                transaction.PrintAgain = true;

            }
        }
        /// <MetaDataID>{a50c2381-6c9e-4359-a493-36c7640550bb}</MetaDataID>
        internal void Update(IMealCourse mealCourse, IList<ItemsPreparationContext> preparedItems, IList<ItemsPreparationContext> underPreparationItems)
        {
            var mealCourseUri = (mealCourse as MealCourse).MealCourseTypeUri;
            MealCourseUri = mealCourseUri;
            //if (mealCourse != MealCourse)
            //    throw new Exception("Meal course mismatch");

            if (MealCourse != null)
                MealCourse.ObjectChangeState -= MealCourseChangeState;

            _MealCourse.Value = mealCourse;

            if (MealCourse != null)
                MealCourse.ObjectChangeState += MealCourseChangeState;

            ContextsOfPreparedItems = preparedItems;
            ContextsOfUnderPreparationItems = underPreparationItems;
            ServicePoint = mealCourse.Meal.Session.ServicePoint;
            ServicesPointIdentity = ServicePoint.ServicesPointIdentity;
            //ServicesContextIdentity = ServicePoint.ServicesContextIdentity;

            Description = mealCourse.Meal.Session.Description + " - " + mealCourse.Name;

        }

        /// <MetaDataID>{2a2716f4-4dac-44f5-b406-1cdac06ec91f}</MetaDataID>
        object CaregiversLock = new object();


        /// <MetaDataID>{e088a5b2-9d6f-4522-806e-8826b1d800ba}</MetaDataID>
        [PersistentMember()]
        [BackwardCompatibilityID("+6")]
        private string Caregiversjson;

        /// <MetaDataID>{8e1cc494-16a6-4da4-8bfa-2ecb3f202bef}</MetaDataID>
        public void AddCaregiver(IServicesContextWorker caregiver, Caregiver.CareGivingType caregivingType)
        {
            lock (CaregiversLock)
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _Caregivers.Add(new Caregiver() { Worker = caregiver, CareGiving = caregivingType, WillTakeCareTimestamp = DateTime.UtcNow });
                    stateTransition.Consistent = true;
                }
            }
        }

        /// <MetaDataID>{e4d85c11-64ac-40f3-be7a-b7ee523d75a0}</MetaDataID>
        internal void AtTheCounter()
        {
            var states = PreparedItems.ToDictionary(x => x.uid, x => x.State);
            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                foreach (var itemPreparation in PreparedItems)
                    itemPreparation.State = ItemPreparationState.Serving;

                states = PreparedItems.ToDictionary(x => x.uid, x => x.State);
                stateTransition.Consistent = true;
            }


            Transaction.RunOnTransactionCompleted(() =>
            {
                var newItemsState = PreparedItems.ToDictionary(x => x.uid, x => x.State);
                ItemsStateChanged?.Invoke(newItemsState);
                (MealCourse as MealCourse).RaiseItemsStateChanged(newItemsState);
            });
        }

        /// <exclude>Excluded</exclude>
        List<Caregiver> _Caregivers = new List<Caregiver>();



        /// <MetaDataID>{1302b250-4bf5-4870-99b2-7b3dc9423f74}</MetaDataID>
        public List<Caregiver> Caregivers
        {
            get
            {
                lock (CaregiversLock)
                {
                    return _Caregivers.ToList();
                }
            }
        }

        ///// <MetaDataID>{3d10eb2d-c183-4c96-b6af-fdfe4e8e789f}</MetaDataID>
        //public void FoodShippingReturn(string returnReasonIdentity, string customReturnReasonDescription = null)
        //{

        //    var returnReason = (ServicePoint as IHomeDeliveryServicePoint).ReturnReasons.Where(x => x.Identity == returnReasonIdentity).FirstOrDefault();

     
        //    var states = PreparedItems.ToDictionary(x => x.uid, x => x.State);
        //    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
        //    {
        //        foreach (var itemPreparation in PreparedItems)
        //            itemPreparation.State = ItemPreparationState.Canceled;

        //        states = PreparedItems.ToDictionary(x => x.uid, x => x.State);


        //        if (returnReason != null)
        //        {
        //            this.ReturnReasonID = returnReasonIdentity;
        //            if (string.IsNullOrWhiteSpace(customReturnReasonDescription))
        //                this.ReturnReason = returnReason.Description;
        //            else
        //                this.ReturnReason = customReturnReasonDescription;
        //        }
        //        else
        //        {
        //            this.ReturnReasonID = "CSTMRTNR";
        //            this.ReturnReason = customReturnReasonDescription;
        //        }

        //        stateTransition.Consistent = true;
        //    }


        //    Transaction.RunOnTransactionCompleted(() =>
        //    {
        //        var newItemsState = PreparedItems.ToDictionary(x => x.uid, x => x.State);
        //        ItemsStateChanged?.Invoke(newItemsState);
        //        (MealCourse as MealCourse).RaiseItemsStateChanged(newItemsState);
        //    });
        //}

        ///// <MetaDataID>{173e97c3-1e9f-4dbf-a89e-97b1fe858338}</MetaDataID>
        //public void Delivered()
        //{

        //    var states = PreparedItems.ToDictionary(x => x.uid, x => x.State);
        //    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
        //    {
        //        foreach (var itemPreparation in PreparedItems)
        //            itemPreparation.State = ItemPreparationState.Served;

        //        states = PreparedItems.ToDictionary(x => x.uid, x => x.State);



        //        stateTransition.Consistent = true;
        //    }


        //    Transaction.RunOnTransactionCompleted(() =>
        //    {
        //        var newItemsState = PreparedItems.ToDictionary(x => x.uid, x => x.State);
        //        ItemsStateChanged?.Invoke(newItemsState);
        //        (MealCourse as MealCourse).RaiseItemsStateChanged(newItemsState);
        //    });

        //}

        
    }
}