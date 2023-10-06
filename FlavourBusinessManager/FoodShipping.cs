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

namespace FlavourBusinessManager.Shipping
{
    /// <MetaDataID>{c36022d2-142c-4f52-8ca3-debd1124b925}</MetaDataID>
    [BackwardCompatibilityID("{c36022d2-142c-4f52-8ca3-debd1124b925}")]
    [Persistent()]
    public class FoodShipping : MarshalByRefObject, OOAdvantech.Remoting.IExtMarshalByRefObject, FlavourBusinessFacade.Shipping.IFoodShipping
    {

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
        public FlavourBusinessFacade.HumanResources.IServingShiftWork ShiftWork => _ShiftWork.Value;

        /// <MetaDataID>{1494396b-fb28-4280-83ad-ee1c39c18f1b}</MetaDataID>
        [CachingDataOnClientSide]
        public ServicePointType ServicePointType { get; set; }


        /// <exclude>Excluded</exclude>
        IMealCourse _MealCourse;

       

        public FoodShipping(IMealCourse mealCourse, IList<ItemsPreparationContext> preparedItems, IList<ItemsPreparationContext> underPreparationItems)
        {

            MealCourseUri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(mealCourse)?.GetPersistentObjectUri(mealCourse);
            _MealCourse = mealCourse;

            ContextsOfPreparedItems = preparedItems;
            ContextsOfUnderPreparationItems = underPreparationItems;
            ServicePoint = mealCourse.Meal.Session.ServicePoint;
            ServicesPointIdentity = ServicePoint.ServicesPointIdentity;



            Description = mealCourse.Meal.Session.Description + " - " + mealCourse.Name;

            if (MealCourse != null)
                MealCourse.ObjectChangeState += MealCourseChangeState;


            //Description = mealCourse.Name + " " + ServicePoint.ServiceArea.Description + " / " + ServicePoint.Description;
        }

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

        /// <MetaDataID>{ebff5c16-abe5-4d72-bcd2-dcca20ca7fa7}</MetaDataID>
        [PersistentMember(nameof(_MealCourse))]
        [BackwardCompatibilityID("+1")]
        public FlavourBusinessFacade.RoomService.IMealCourse MealCourse => _MealCourse;

        /// <MetaDataID>{d8834cae-2fde-4861-81b0-6be2a5c26f12}</MetaDataID>
        [CachingDataOnClientSide]
        public bool IsAssigned
        {
            get
            {
                return ShiftWork != null;
            }
        }

        /// <MetaDataID>{a6d410ca-433e-4bc2-a60d-2902bbf42126}</MetaDataID>
        [CachingDataOnClientSide]
        public int SortID { get; internal set; }

        /// <MetaDataID>{f1770ae5-6716-476d-80cb-1542934f27e6}</MetaDataID>
        public string MealCourseUri { get; private set; }

        /// <MetaDataID>{c31ace5f-8e90-4682-a38c-e374a519580d}</MetaDataID>
        public IServicePoint ServicePoint { get; set; }


        /// <MetaDataID>{81c8d3f1-bdf1-422b-9c79-baf0371f329a}</MetaDataID>
        [CachingDataOnClientSide]
        public IList<ItemsPreparationContext> ContextsOfUnderPreparationItems { get; set; }

        /// <MetaDataID>{510a7954-2c31-4a10-a898-e2e1a004818d}</MetaDataID>
        [CachingDataOnClientSide]
        public IList<ItemsPreparationContext> ContextsOfPreparedItems { get; set; }

        /// <MetaDataID>{c83578bb-5b53-459b-949d-362b78db48fd}</MetaDataID>
        [CachingDataOnClientSide]
        public string Description { get; set; }


        /// <MetaDataID>{effa589d-8157-44af-951c-4ca5c212dff6}</MetaDataID>
        [CachingDataOnClientSide]
        public string ServicesPointIdentity { get; set; }


        public event ObjectChangeStateHandle ObjectChangeState;
        public event ItemsStateChangedHandle ItemsStateChanged;

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
        internal void Update(IMealCourse mealCourse, IList<ItemsPreparationContext> preparedItems, IList<ItemsPreparationContext> underPreparationItems)
        {
            var mealCourseUri = (mealCourse as MealCourse).MealCourseTypeUri;
            MealCourseUri = mealCourseUri;
            //if (mealCourse != MealCourse)
            //    throw new Exception("Meal course mismatch");

            if (MealCourse != null)
                MealCourse.ObjectChangeState -= MealCourseChangeState;

            _MealCourse = mealCourse;

            if (MealCourse != null)
                MealCourse.ObjectChangeState += MealCourseChangeState;

            ContextsOfPreparedItems = preparedItems;
            ContextsOfUnderPreparationItems = underPreparationItems;
            ServicePoint = mealCourse.Meal.Session.ServicePoint;
            ServicesPointIdentity = ServicePoint.ServicesPointIdentity;



            Description = mealCourse.Meal.Session.Description + " - " + mealCourse.Name;

        }

    }
}