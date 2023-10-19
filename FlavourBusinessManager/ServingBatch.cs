using FlavourBusinessFacade.RoomService;
using FlavourBusinessFacade.ServicesContextResources;
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
                if (_CreationTime!=value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _CreationTime=value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        [BeforeCommitObjectStateInStorageCall]
        public void BeforeCommitObjectState()
        {
            if (_CreationTime==null)
            { 
                _CreationTime=DateTime.UtcNow;
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
        /// <MetaDataID>{eb3120b1-beb4-4c33-bd96-5b861484b691}</MetaDataID>
        [CachingDataOnClientSide]
        public string Description { get; set; }

        /// <MetaDataID>{46f36229-fd4d-4d54-b0c2-751685130bdc}</MetaDataID>
        [CachingDataOnClientSide]
        public string ServicesPointIdentity { get; set; }

        /// <MetaDataID>{7a3cacaf-eaba-4fcd-a84b-e8bc312e54f5}</MetaDataID>
        [CachingDataOnClientSide]
        public ServicePointType ServicePointType { get; set; }

        /// <MetaDataID>{0ac6e347-b768-4b8a-b22a-34094c85357d}</MetaDataID>
        [CachingDataOnClientSide]
        public bool IsAssigned
        {
            get
            {
                return ShiftWork != null;
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
            if (nameof(ServicesContextResources.FoodServiceSession.ServicePoint) == member|| nameof(RoomService.MealCourse.Meal) == member)
            {
                this.ServicePoint = MealCourse.Meal.Session.ServicePoint;
                Description = MealCourse.Meal.Session.Description + " - " + MealCourse.Name;
                this.ServicesPointIdentity = this.ServicePoint.ServicesPointIdentity;

            }
            if (servingBatchChanged|| nameof(ServicesContextResources.FoodServiceSession.ServicePoint) == member)
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
                if (Transaction.Current?.Status==TransactionStatus.Committed)
                    ItemsStateChanged?.Invoke(PreparedItems.ToDictionary(x => x.uid, x => x.State));
            });

        }
    }
}