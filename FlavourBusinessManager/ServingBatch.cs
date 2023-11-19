using FlavourBusinessFacade.HumanResources;
using FlavourBusinessFacade.RoomService;
using FlavourBusinessFacade.ServicesContextResources;
using FlavourBusinessManager.EndUsers;
using OOAdvantech;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlavourBusinessManager.RoomService
{
    /// <MetaDataID>{9fb9b6d1-dca2-47f1-8275-9c769636dbd1}</MetaDataID>
    [BackwardCompatibilityID("{9fb9b6d1-dca2-47f1-8275-9c769636dbd1}")]
    [Persistent()]
    public class ServingBatch : MarshalByRefObject, OOAdvantech.Remoting.IExtMarshalByRefObject, IServingBatch
    {


        /// <exclude>Excluded</exclude>
        DateTime? _CreationTime;

        /// <MetaDataID>{3ae30a45-2c6d-42bb-96e3-28a51bf72b87}</MetaDataID>
        [PersistentMember(nameof(_CreationTime))]

        [BackwardCompatibilityID("+6")]
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
        /// <MetaDataID>{023fdd54-ca3f-4571-9f25-70296b395aa6}</MetaDataID>
        [BeforeCommitObjectStateInStorageCall]
        public void BeforeCommitObjectState()
        {
            if (_CreationTime == null)
            {
                _CreationTime = DateTime.UtcNow;
            }
            lock (CaregiversLock)
            {
                Caregiversjson = OOAdvantech.Json.JsonConvert.SerializeObject(_Caregivers);
            }
        }
        /// <MetaDataID>{5f13e7f9-1392-4b1e-8f7d-a2c494d9ad00}</MetaDataID>
        [ObjectActivationCall]
        public void ObjectActivation()
        {

            lock (CaregiversLock)
            {
                if (!string.IsNullOrWhiteSpace(Caregiversjson))
                    _Caregivers = OOAdvantech.Json.JsonConvert.DeserializeObject<List<Caregiver>>(Caregiversjson);
            }

        }

        /// <MetaDataID>{f7e6bbea-e018-4ed1-83a3-5814ef9c1eb7}</MetaDataID>
        public void PrintReceiptAgain()
        {
            var transactionUri = PreparedItems.OfType<ItemPreparation>().Where(x => !string.IsNullOrWhiteSpace(x.TransactionUri)).FirstOrDefault()?.TransactionUri;
            if (!string.IsNullOrWhiteSpace(transactionUri))
            {
                var transaction = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri<FinanceFacade.Transaction>(transactionUri);
                transaction.PrintAgain = true;

            }
        }
        /// <MetaDataID>{0892df45-97cf-424c-9294-05700287fe9c}</MetaDataID>
        public string MealCourseUri { get; private set; }

        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<IItemPreparation> _PreparedItems = new OOAdvantech.Collections.Generic.Set<IItemPreparation>();

        /// <MetaDataID>{2411b3a6-0864-4dcf-b4c3-7ead1eb055c2}</MetaDataID>
        [PersistentMember(nameof(_PreparedItems))]
        [BackwardCompatibilityID("+1")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        public List<IItemPreparation> PreparedItems
        {
            get => _PreparedItems.ToThreadSafeList();

        }
        public event ObjectChangeStateHandle ObjectChangeState;

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


        /// <MetaDataID>{a3dfa875-e9b1-4fae-9656-c09968bd587f}</MetaDataID>
        [ObjectsLinkCall]
        void ObjectsLink(object linkedObject, AssociationEnd associationEnd, bool added)
        {
            if (associationEnd.Name == nameof(ShiftWork))
            {

            }
        }

        public event FlavourBusinessFacade.EndUsers.ItemsStateChangedHandle ItemsStateChanged;


        /// <exclude>Excluded</exclude>
        OOAdvantech.Member<FlavourBusinessFacade.HumanResources.IServingShiftWork> _ShiftWork = new Member<FlavourBusinessFacade.HumanResources.IServingShiftWork>();

        /// <MetaDataID>{afbee022-ff6f-45ad-a5e5-7f0ac055a7df}</MetaDataID>
        [PersistentMember(nameof(_ShiftWork))]
        public FlavourBusinessFacade.HumanResources.IServingShiftWork ShiftWork => _ShiftWork.Value;

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <exclude>Excluded</exclude>
        string _Description;



        /// <MetaDataID>{eb3120b1-beb4-4c33-bd96-5b861484b691}</MetaDataID>
        [PersistentMember(nameof(_Description))]
        [BackwardCompatibilityID("+11")]
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
        /// <MetaDataID>{46f36229-fd4d-4d54-b0c2-751685130bdc}</MetaDataID>
        [CachingDataOnClientSide]
        [PersistentMember(nameof(_ServicesPointIdentity))]
        [BackwardCompatibilityID("+10")]
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


        /// <exclude>Excluded</exclude>
        ServicePointType? _ServicePointType;

        /// <MetaDataID>{7a3cacaf-eaba-4fcd-a84b-e8bc312e54f5}</MetaDataID>
        [PersistentMember(nameof(_ServicePointType))]
        [BackwardCompatibilityID("+8")]
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

        /// <exclude>Excluded</exclude>
        bool? _IsAssigned;

        /// <MetaDataID>{0ac6e347-b768-4b8a-b22a-34094c85357d}</MetaDataID>
        [PersistentMember(nameof(_IsAssigned))]
        [BackwardCompatibilityID("+9")]
        [CachingDataOnClientSide]
        public bool IsAssigned
        {
            get
            {
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

        /// <MetaDataID>{4fd40b73-6ad1-4efb-a156-4e7c585155db}</MetaDataID>
        [CachingDataOnClientSide]
        public int SortID { get; internal set; }


        /// <MetaDataID>{1603a1ea-fc7a-4b83-ac7a-24c143fe7d31}</MetaDataID>
        public IServicePoint ServicePoint { get; set; }

        /// <exclude>Excluded</exclude>
        OOAdvantech.Member<IMealCourse> _MealCourse = new OOAdvantech.Member<IMealCourse>();

        /// <MetaDataID>{bb994edc-6942-44d7-ab3b-88e6ede82264}</MetaDataID>
        [PersistentMember(nameof(_MealCourse))]
        [BackwardCompatibilityID("+4")]
        [CachingDataOnClientSide]
        public IMealCourse MealCourse
        {
            get => _MealCourse.Value;
            private set
            {
                if (_MealCourse != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _MealCourse.Value = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <MetaDataID>{229605f9-a461-4742-b21c-003791d9f578}</MetaDataID>
        [CachingDataOnClientSide]
        public IList<ItemsPreparationContext> ContextsOfUnderPreparationItems { get; set; }

        /// <MetaDataID>{1abcf246-6696-41e7-82ae-19bd863348a1}</MetaDataID>
        [CachingDataOnClientSide]
        public IList<ItemsPreparationContext> ContextsOfPreparedItems { get; set; }
        /// <MetaDataID>{6bf00296-5438-4802-bbba-52b4263092e0}</MetaDataID>
        public ItemPreparationState State
        {
            get
            {
                if (this.PreparedItems.Where(x => x.State == ItemPreparationState.Serving).Count() == this.PreparedItems.Count && this.PreparedItems.Count > 0)
                    return ItemPreparationState.Serving;
                if (this.PreparedItems.Where(x => x.State == ItemPreparationState.OnRoad).Count() == this.PreparedItems.Count && this.PreparedItems.Count > 0)
                    return ItemPreparationState.OnRoad;

                if (this.PreparedItems.OfType<ItemPreparation>().Where(x => x.IsInFollowingState(ItemPreparationState.Served)).Count() == this.PreparedItems.Count && this.PreparedItems.Count > 0)
                    return ItemPreparationState.Served;

                return ItemPreparationState.Serving;
            }
        }



        /// <MetaDataID>{2bf04da7-e667-4ae3-8268-339da1a42253}</MetaDataID>
        protected ServingBatch()
        {
        }




        /// <MetaDataID>{35670efb-8e94-47ae-af59-82be8a3e6bec}</MetaDataID>
        public ServingBatch(IMealCourse mealCourse, IList<ItemsPreparationContext> preparedItems, IList<ItemsPreparationContext> underPreparationItems)
        {

            MealCourseUri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(mealCourse)?.GetPersistentObjectUri(mealCourse);
            MealCourse = mealCourse;

            ContextsOfPreparedItems = preparedItems;
            ContextsOfUnderPreparationItems = underPreparationItems;
            ServicePoint = mealCourse.Meal.Session.ServicePoint;
            ServicesPointIdentity = ServicePoint.ServicesPointIdentity;
            ServicesContextIdentity = ServicePoint.ServicesContextIdentity;



            Description = mealCourse.Meal.Session.Description + " - " + mealCourse.Name;

            if (MealCourse != null)
                MealCourse.ObjectChangeState += MealCourseChangeState;


            //Description = mealCourse.Name + " " + ServicePoint.ServiceArea.Description + " / " + ServicePoint.Description;
        }

        /// <MetaDataID>{1dfeec9c-13a0-4b97-bd97-31914813ab66}</MetaDataID>
        internal void Update(IMealCourse mealCourse, IList<ItemsPreparationContext> preparedItems, IList<ItemsPreparationContext> underPreparationItems)
        {
            var mealCourseUri = (mealCourse as MealCourse).MealCourseTypeUri;
            MealCourseUri = mealCourseUri;
            //if (mealCourse != MealCourse)
            //    throw new Exception("Meal course mismatch");

            if (MealCourse != null)
                MealCourse.ObjectChangeState -= MealCourseChangeState;

            MealCourse = mealCourse;

            if (MealCourse != null)
                MealCourse.ObjectChangeState += MealCourseChangeState;

            ContextsOfPreparedItems = preparedItems;
            ContextsOfUnderPreparationItems = underPreparationItems;
            ServicePoint = mealCourse.Meal.Session.ServicePoint;
            ServicesPointIdentity = ServicePoint.ServicesPointIdentity;



            Description = mealCourse.Meal.Session.Description + " - " + mealCourse.Name;

        }

        /// <MetaDataID>{a7f2b9ce-f952-472b-89c3-f1487c5a619c}</MetaDataID>
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
        /// <MetaDataID>{2fde4c2e-76b1-4b7d-a574-a6a7a59f4fcf}</MetaDataID>
        object CaregiversLock = new object();


        /// <MetaDataID>{e088a5b2-9d6f-4522-806e-8826b1d800ba}</MetaDataID>
        [PersistentMember()]
        [BackwardCompatibilityID("+7")]
        string Caregiversjson;

        /// <MetaDataID>{4ea761d6-3e1a-4eec-9c9a-112d792472f3}</MetaDataID>
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

        /// <exclude>Excluded</exclude>
        List<Caregiver> _Caregivers = new List<Caregiver>();


        /// <MetaDataID>{a1502d67-0540-4bb8-8760-2b80a0586d40}</MetaDataID>
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

        /// <MetaDataID>{723968b1-9961-4f29-9212-2e61db181cfd}</MetaDataID>
        [CachingDataOnClientSide]
        public string ServicesContextIdentity { get; set; }


        /// <MetaDataID>{7ddeada1-2762-4aea-812a-6486231344be}</MetaDataID>
        internal void OnTheRoad()
        {

            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                foreach (var itemPreparation in PreparedItems)
                    itemPreparation.State = ItemPreparationState.OnRoad;
                stateTransition.Consistent = true;
            }


            Transaction.RunOnTransactionCompleted(() =>
            {

                var status = Transaction.Current?.Status;
                if (Transaction.Current?.Status == TransactionStatus.Committed)
                    ItemsStateChanged?.Invoke(PreparedItems.ToDictionary(x => x.uid, x => x.State));
            });

        }
    }
}