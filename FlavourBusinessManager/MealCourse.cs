using FlavourBusinessFacade.RoomService;
using MenuModel;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Transactions;
using System;
using System.Linq;
using System.Collections.Generic;

namespace FlavourBusinessManager.RoomService
{



    /// <summary>
    ///Defines a service section with time constraints and includes the preparation of the food-coffee items, the serving - takeaway - home delivery.
    /// </summary>
    ///<MetaDataID>{986d1f45-2bef-4302-9d5c-b98141c24555}</MetaDataID>
    [BackwardCompatibilityID("{986d1f45-2bef-4302-9d5c-b98141c24555}")]
    [Persistent()]
    public class MealCourse : System.MarshalByRefObject, IMealCourse
    {

        /// <exclude>Excluded</exclude>
        double _DurationInMinutes;

        /// <MetaDataID>{73175278-0c8f-45cd-a78f-3c49557633cb}</MetaDataID>
        [PersistentMember(nameof(_DurationInMinutes))]
        [BackwardCompatibilityID("+7")]
        public double DurationInMinutes
        {
            get => _DurationInMinutes;
            set
            {
                if (_DurationInMinutes != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _DurationInMinutes = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }


        /// <exclude>Excluded</exclude>
        string _MealCourseTypeUri;

        /// <MetaDataID>{dbf7beba-2251-433e-80cc-9052b24a0189}</MetaDataID>
        [PersistentMember(nameof(_MealCourseTypeUri))]
        [BackwardCompatibilityID("+6")]
        public string MealCourseTypeUri
        {
            get => _MealCourseTypeUri;
            set
            {

                if (_MealCourseTypeUri != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _MealCourseTypeUri = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        string _Name;

        /// <MetaDataID>{8b2a7043-450a-4ff3-825a-f505a07ffd2d}</MetaDataID>
        [PersistentMember(nameof(_Name))]
        [BackwardCompatibilityID("+5")]
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

        /// <MetaDataID>{22f12ba9-4c26-4197-ad51-92b7981d601a}</MetaDataID>
        protected MealCourse()
        {

        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<IItemPreparation> _FoodItems = new OOAdvantech.Collections.Generic.Set<IItemPreparation>();

        /// <MetaDataID>{1f941000-ce6f-4ec6-85f6-736ad57cf9a6}</MetaDataID>
        [PersistentMember(nameof(_FoodItems))]
        [BackwardCompatibilityID("+1")]
        [CachingDataOnClientSide]
        public IList<IItemPreparation> FoodItems => _FoodItems.ToThreadSafeList();


        /// <exclude>Excluded</exclude>
        DateTime? _StartsAt;

        /// <MetaDataID>{963bb0ae-3234-4bf5-b7e4-9a313dcb9531}</MetaDataID>
        [PersistentMember(nameof(_StartsAt))]
        [BackwardCompatibilityID("+2")]
        public System.DateTime? StartsAt
        {
            get => _StartsAt;
            set
            {
                if (_StartsAt != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _StartsAt = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <exclude>Excluded</exclude>
        System.DateTime? _ServedAtForecast;

        /// <MetaDataID>{19242092-38cd-4f7d-b7dc-77abc4dfb56e}</MetaDataID>
        [PersistentMember(nameof(_ServedAtForecast))]
        [BackwardCompatibilityID("+8")]
        public System.DateTime? ServedAtForecast
        {
            get => _ServedAtForecast;
            set
            {
                if (_ServedAtForecast != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ServedAtForecast = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{2b29e1c8-9c66-4412-96f3-cf276f528e44}</MetaDataID>
        DateTime? _PreparedAt;

        /// <MetaDataID>{f5802cfc-5c51-4da8-891f-c48fa3abd3b3}</MetaDataID>
        [PersistentMember(nameof(_PreparedAt))]
        [BackwardCompatibilityID("+3")]
        public System.DateTime? ServedAt
        {
            get => _PreparedAt;
            set => throw new NotImplementedException();
        }



        /// <exclude>Excluded</exclude>
        ItemPreparationState _PreparationState;

        /// <MetaDataID>{fdf1dea6-88ed-41fa-8302-6d5392ea0359}</MetaDataID>
        [PersistentMember(nameof(_PreparationState))]
        [BackwardCompatibilityID("+4")]
        public ItemPreparationState PreparationState
        {
            get => _PreparationState;
            set => throw new NotImplementedException();
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.Member<IMeal> _Meal = new OOAdvantech.Member<IMeal>();


        /// <MetaDataID>{65995fe0-c0a3-47e3-90a1-58523ea31b41}</MetaDataID>
        [PersistentMember(nameof(_Meal))]
        [CachingDataOnClientSide]
        [BackwardCompatibilityID("+9")]
        public FlavourBusinessFacade.RoomService.IMeal Meal => _Meal.Value;


        [CachingDataOnClientSide]
        public IList<ItemsPreparationContext> FoodItemsInProgress
        {
            get
            {

                List<ItemsPreparationContext> foodItemsInProgress = (from itemPreparation in FoodItems
                                                                     where itemPreparation.State == ItemPreparationState.PendingPreparation ||
                                                                     itemPreparation.State == ItemPreparationState.PreparationDelay ||
                                                                     itemPreparation.State == ItemPreparationState.ÉnPreparation
                                                                     group itemPreparation by itemPreparation.PreparationStation into itemsUnderPreparation
                                                                     select new ItemsPreparationContext(this, itemsUnderPreparation.Key, itemsUnderPreparation.ToList())).ToList();
                return foodItemsInProgress;

            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;
        /// <MetaDataID>{7b6700b2-c8a4-4548-be19-657614416518}</MetaDataID>
        private MealCourseType mealCourseType;

        /// <MetaDataID>{e6899983-8b85-4e28-84f4-8a8199511318}</MetaDataID>
        public MealCourse(MealCourseType mealCourseType, List<ItemPreparation> itemPreparations)
        {
            this.mealCourseType = mealCourseType;
            _MealCourseTypeUri = ObjectStorage.GetStorageOfObject(mealCourseType).GetPersistentObjectUri(mealCourseType);
            _DurationInMinutes = mealCourseType.DurationInMinutes;
            _Name = mealCourseType.Name;
            foreach (var flavourItem in itemPreparations)
                AddItem(flavourItem);

        }

        /// <MetaDataID>{ed457de3-cf46-443e-a9cc-73340c1a1294}</MetaDataID>
        private void FlavourItem_ObjectChangeState(object _object, string member)
        {

        }

        /// <MetaDataID>{fa1a0f37-108e-478c-83d4-4f095498cef6}</MetaDataID>
        public void AddItem(IItemPreparation itemPreparation)
        {
            ItemPreparation flavourItem = itemPreparation as ItemPreparation;
            if (!_FoodItems.Contains(flavourItem))
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _FoodItems.Add(flavourItem);
                    stateTransition.Consistent = true;
                }
                flavourItem.ObjectChangeState += FlavourItem_ObjectChangeState;
                if (flavourItem.State == ItemPreparationState.Committed)
                {
                    if (flavourItem.MenuItem == null)
                        flavourItem.LoadMenuItem();

                    var preparationData = ServicesContextResources.PreparationStation.GetPreparationData(flavourItem);
                    flavourItem.State = ItemPreparationState.PreparationDelay;
                    (preparationData.PreparationStationRuntime as ServicesContextResources.PreparationStation).AssignItemPreparation(flavourItem);
                }
            }
        }

        /// <MetaDataID>{9b5fa430-11c3-4ad7-98c5-749b4d06c186}</MetaDataID>
        public void RemoveItem(IItemPreparation itemPreparation)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _FoodItems.Remove(itemPreparation);
                (itemPreparation as ItemPreparation).ObjectChangeState -= FlavourItem_ObjectChangeState;
                stateTransition.Consistent = true;
            }
        }
        /// <MetaDataID>{ce36f3b9-7c45-48df-b766-2f622eb00589}</MetaDataID>
        [ObjectActivationCall]
        void ObjectActivation()
        {

            foreach (var foodItem in _FoodItems.OfType<ItemPreparation>())
            {
                foodItem.ObjectChangeState += FlavourItem_ObjectChangeState;
            }

        }



        /// <MetaDataID>{72860a38-cffd-4a68-807b-4c2ece8cddc5}</MetaDataID>
        internal static void AssignMealCourseToItem(ItemPreparation flavourItem)
        {
            //flavourItem.ClientSession.ServicePoint.
        }
    }
}