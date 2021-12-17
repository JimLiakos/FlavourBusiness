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

        [DeleteObjectCall]
        void ObjectDeleting()
        {
            if (MealCourse != null)
                MealCourse.ObjectChangeState -= MealCourseChangeState;

        }


        [ObjectsLinkCall]
        void ObjectsLink(object linkedObject, AssociationEnd associationEnd, bool added)
        {
            if (associationEnd.Name == nameof(ShiftWork))
            {

            }
        }

        public event FlavourBusinessFacade.EndUsers.ItemsStateChangedHandle ItemsStateChanged;


        /// <exclude>Excluded</exclude>
        OOAdvantech.Member<HumanResources.ServingShiftWork> _ShiftWork = new OOAdvantech.Member<HumanResources.ServingShiftWork>();

        [PersistentMember(nameof(_ShiftWork))]
        [Association("ServingBatchInShiftWork", Roles.RoleB, "5b49aba4-a3de-46da-9a52-6436a3823d6f")]
        public HumanResources.ServingShiftWork ShiftWork => _ShiftWork.Value;

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

        private void MealCourseChangeState(object _object, string member)
        {

            IList<ItemsPreparationContext> preparedItems = (from itemsPreparationContext in MealCourse.FoodItemsInProgress
                                                            where itemsPreparationContext.PreparationItems.All(x => x.State == ItemPreparationState.Serving)
                                                            select itemsPreparationContext).ToList();

            IList<ItemsPreparationContext> underPreparationItems = (from itemsPreparationContext in MealCourse.FoodItemsInProgress
                                                                    where itemsPreparationContext.PreparationItems.Any(x => x.State == ItemPreparationState.PendingPreparation ||
                                                                    x.State == ItemPreparationState.ÉnPreparation ||
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
            if (servingBatchChanged)
            {
                ContextsOfPreparedItems = preparedItems;
                ContextsOfUnderPreparationItems = underPreparationItems;
                ObjectChangeState?.Invoke(this, null);
            }
        }

        internal void OnTheRoad()
        {

            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                foreach (var itemPreparation in PreparedItems)
                    itemPreparation.State = ItemPreparationState.OnRoad;
                stateTransition.Consistent = true;
            }

            ItemsStateChanged?.Invoke()
        }
    }
}