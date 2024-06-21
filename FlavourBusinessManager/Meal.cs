using ComputationalResources;
using FlavourBusinessFacade.HumanResources;
using FlavourBusinessFacade.RoomService;
using FlavourBusinessFacade.ServicesContextResources;
using MenuModel;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FlavourBusinessManager.RoomService
{
    /// <MetaDataID>{2a2adf89-8607-46c7-844d-d075fcf5d18b}</MetaDataID>
    [BackwardCompatibilityID("{2a2adf89-8607-46c7-844d-d075fcf5d18b}")]
    [Persistent()]
    public class Meal : MarshalByRefObject, OOAdvantech.Remoting.IExtMarshalByRefObject, IMeal
    {
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{9dfc917f-c600-497d-a324-d1722058e811}</MetaDataID>
        string _MealTypeUri;

        /// <MetaDataID>{64183d95-2f41-4306-92fe-f877796dc446}</MetaDataID>
        [PersistentMember(nameof(_MealTypeUri))]
        [BackwardCompatibilityID("+4")]
        public string MealTypeUri
        {
            get => _MealTypeUri;
            set
            {

                if (_MealTypeUri != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _MealTypeUri = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        string _Name;

        /// <MetaDataID>{48e9e0d2-baf3-4f5a-8d87-b93c25ba690a}</MetaDataID>
        [PersistentMember(nameof(_Name))]
        [BackwardCompatibilityID("+3")]
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

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// </remarks>
        internal List<IMealCourse> MealCoursesSequences
        {
            get
            {
                return Courses.Select(x => x.HeaderCourse).Distinct().ToList();
            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<IMealCourse> _Courses = new OOAdvantech.Collections.Generic.Set<IMealCourse>();

        /// <MetaDataID>{5f5f90f7-bfae-4b34-8dc6-c6aa57297db5}</MetaDataID>
        [PersistentMember(nameof(_Courses))]
        [BackwardCompatibilityID("+2")]
        [AssociationEndBehavior(PersistencyFlag.CascadeDelete | PersistencyFlag.OnConstruction)]
        public List<IMealCourse> Courses => _Courses.ToThreadSafeList();

        /// <exclude>Excluded</exclude>
        OOAdvantech.Member<IFoodServiceSession> _Session = new OOAdvantech.Member<IFoodServiceSession>();

        /// <MetaDataID>{df1bef38-aa51-450a-a6c2-bdb6b6f960a5}</MetaDataID>
        [PersistentMember(nameof(_Session))]
        [BackwardCompatibilityID("+1")]
        [CachingDataOnClientSide]
        public IFoodServiceSession Session => _Session.Value;

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;




        /// <MetaDataID>{41cb3ae5-a95b-4809-bf04-5e4738fc3756}</MetaDataID>
        protected Meal()
        {

        }
        /// <exclude>Excluded</exclude>
        [OOAdvantech.Json.JsonIgnore]
        MenuModel.MealType _MealType;
        /// <MetaDataID>{5f6ff876-551a-47e2-ba91-7456465a3b99}</MetaDataID>
        [OOAdvantech.Json.JsonIgnore]
        public MenuModel.MealType MealType
        {
            get
            {
                if (_MealType == null)
                    _MealType = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(_MealTypeUri) as MenuModel.MealType;
                return _MealType;
            }
        }

        /// <MetaDataID>{1d0598fe-a193-451d-a1f4-38a330388d26}</MetaDataID>
        internal Meal(MenuModel.MealType mealType, List<ItemPreparation> mealItems, ServicesContextResources.FoodServiceSession foodServiceSession)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _Session.Value = foodServiceSession;
                _Name = mealType.Name;
                _MealTypeUri = ObjectStorage.GetStorageOfObject(mealType).GetPersistentObjectUri(mealType);
                foreach (var mealCourseItems in (from mealItem in mealItems
                                                 group mealItem by mealItem.SelectedMealCourseTypeUri into mealCourseItems
                                                 select mealCourseItems))
                {
                    var mealCourseType = mealType.Courses.OfType<MenuModel.MealCourseType>().Where(x => ObjectStorage.GetStorageOfObject(x)?.GetPersistentObjectUri(x) == mealCourseItems.Key).FirstOrDefault();
                    if (mealCourseType == null)
                        mealCourseType = mealType.Courses.OfType<MenuModel.MealCourseType>().Where(x => x.IsDefault).FirstOrDefault();
                    //if (mealCourseType == null)
                    //    mealCourseType = mealType.Courses.OfType<MenuModel.MealCourseType>().First();

                    if (mealCourseType == null)
                        throw new Exception("Invalid meal course type");
                    MealCourse mealCourse = _Courses.OfType<MealCourse>().Where(x => x.MealCourseType == mealCourseType).FirstOrDefault();
                    if (mealCourse == null)
                    {
                        mealCourse = new MealCourse(mealCourseType, mealCourseItems.ToList(), this);
                        mealCourse.StartsAt = DateTime.UtcNow;
                        mealCourse.Previous = _Courses.LastOrDefault();
                        _Courses.Add(mealCourse);
                    }
                    else
                    {
                        foreach (var mealCourseItem in mealCourseItems)
                            mealCourse.AddItem(mealCourseItem);
                    }



                }

                stateTransition.Consistent = true;
            }
        }
        /// <MetaDataID>{529127a8-8386-46da-a81b-1539bed5530b}</MetaDataID>
        object MealLock = new object();

        ManualResetEvent StopMealMonitoring = new ManualResetEvent(false);

        /// <MetaDataID>{340bccf9-c1ba-4185-851e-370a3286f0ee}</MetaDataID>
        Task MonitoringTask;
        /// <MetaDataID>{16c25ab9-e42f-4e19-b0de-7f0ea44de07c}</MetaDataID>
        internal void MonitoringRun()
        {
            lock (MealLock)
            {
                if (MonitoringTask != null && !MonitoringTask.IsCompleted)
                    return;
                MonitoringTask = Task.Run(() =>
                {
                    var sessionState = Session?.SessionState;
                    while (sessionState == SessionState.MealMonitoring)
                    {
                        try
                        {
                            //if (Courses.Count > 0 && Courses[0].ServedAtForecast == null)
                            //    BuildMealTimePlan();


                            //using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                            //{
                            //    foreach (var preparationItem in (from course in Courses
                            //                                     from foodItem in course.FoodItems.OfType<ItemPreparation>()
                            //                                     where foodItem.State == ItemPreparationState.PreparationDelay
                            //                                     select foodItem))
                            //    {
                            //        var preparationData = ServicesContextResources.PreparationStation.GetPreparationData(preparationItem);
                            //        if ((DateTime.UtcNow + preparationData.Duration).ToUniversalTime() > preparationItem.MealCourse.ServedAtForecast?.ToUniversalTime())
                            //            preparationItem.State = ItemPreparationState.PendingPreparation;
                            //    }



                            //    stateTransition.Consistent = true;
                            //}

                            try
                            {
                                CheckForNewItems();

                                List<IMealCourse> removedMealCourses = new List<IMealCourse>();

                                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                                {
                                    foreach (var mealCourse in Courses.Where(x => x.FoodItems.Count == 0).ToList())
                                    {
                                        _Courses.Remove(mealCourse);
                                        removedMealCourses.Add(mealCourse);
                                    }
                                    stateTransition.Consistent = true;
                                }

                                if (removedMealCourses.Count > 0)
                                    (ServicePointRunTime.ServicesContextRunTime.Current.MealsController as MealsController).OnRemoveMealCoursesInrogress(removedMealCourses);





                                foreach (var mealCourse in Courses.ToList())
                                    (mealCourse as MealCourse).Monitoring();

                            }
                            catch (Exception error)
                            {
                                ComputingCluster.WriteOnEventLog("MealMonitoring", error.Message + Environment.NewLine + error.StackTrace, System.Diagnostics.EventLogEntryType.Error);
                            }

                            (Session?.ServicePoint as ServicesContextResources.ServicePoint)?.UpdateState();

                            if (StopMealMonitoring.WaitOne(1000))
                            {

                                StopMealMonitoring.Reset();
                                return;
                            }

                            sessionState = Session.SessionState;
                        }
                        catch (Exception error)
                        {
                            throw;
                        }
                    }
                });


            }

        }

        internal void StopMonitoring()
        {
            if (MonitoringTask != null && MonitoringTask.Status == TaskStatus.Running)
            {
                StopMealMonitoring.Set();
                MonitoringTask.Wait(2000);
            }
        }

        internal List<ItemsMealCourseAssignment> GetUncommittedItemsMealCourseAssignment()
        {

            List<ItemsMealCourseAssignment> itemsMealCourseAssignments = new List<ItemsMealCourseAssignment>();

            List<IMealCourse> newMealCourses = new List<IMealCourse>();
            var mealItems = (from foodServiceClientSession in Session.PartialClientSessions
                             from itemPreparation in foodServiceClientSession.FlavourItems.OfType<ItemPreparation>()
                             where itemPreparation.State.IsInTheSameOrPreviousState(ItemPreparationState.Committed)
                             select itemPreparation).ToList();

            if (mealItems.Count > 0)
            {
                MenuModel.MealType mealType = ObjectStorage.GetObjectFromUri<MenuModel.MealType>(_MealTypeUri);

                foreach (var mealCourseTypesItems in (from mealItem in mealItems
                                                      group mealItem by mealItem.SelectedMealCourseTypeUri into mealCourseItems
                                                      select mealCourseItems))
                {
                    var mealCourseType = mealType.Courses.OfType<MenuModel.MealCourseType>().Where(x => ObjectStorage.GetStorageOfObject(x)?.GetPersistentObjectUri(x) == mealCourseTypesItems.Key).First();


                    MealCourse mealCourse = _Courses.OfType<MealCourse>().Where(x => x.MealCourseTypeUri == mealCourseTypesItems.Key).FirstOrDefault();
                    if (mealCourse == null || mealCourseTypesItems.Any(x => mealCourse.ItsTooLateForChange(x)))
                    {
                        ItemsMealCourseAssignment itemsMealCourseAssignment = new ItemsMealCourseAssignment(mealCourseType, mealCourseTypesItems.ToList(), null);
                        itemsMealCourseAssignments.Add(itemsMealCourseAssignment);
                    }
                    else
                    {
                        ItemsMealCourseAssignment itemsMealCourseAssignment = new ItemsMealCourseAssignment(mealCourseType, mealCourseTypesItems.ToList(), mealCourse);
                        itemsMealCourseAssignments.Add(itemsMealCourseAssignment);
                    }
                }
            }
            return itemsMealCourseAssignments;
        }

        /// <MetaDataID>{09ccf99d-b9a6-4d3a-b26a-3b1c931b682c}</MetaDataID>
        private void CheckForNewItems()
        {
            List<IMealCourse> newMealCourses = new List<IMealCourse>();
            var mealItems = (from foodServiceClientSession in Session.PartialClientSessions
                             from itemPreparation in foodServiceClientSession.FlavourItems.OfType<ItemPreparation>()
                             where itemPreparation.State == ItemPreparationState.Committed
                             select itemPreparation).ToList();

            if (mealItems.Count > 0)
            {
                MenuModel.MealType mealType = ObjectStorage.GetObjectFromUri<MenuModel.MealType>(_MealTypeUri);

                foreach (var mealCourseTypesItems in (from mealItem in mealItems
                                                      group mealItem by mealItem.SelectedMealCourseTypeUri into mealCourseItems
                                                      select mealCourseItems))
                {
                    var mealCourseType = mealType.Courses.OfType<MenuModel.MealCourseType>().Where(x => ObjectStorage.GetStorageOfObject(x)?.GetPersistentObjectUri(x) == mealCourseTypesItems.Key).First();


                    MealCourse mealCourse = _Courses.OfType<MealCourse>().Where(x => x.MealCourseTypeUri == mealCourseTypesItems.Key).FirstOrDefault();
                    if (mealCourse == null || mealCourseTypesItems.Any(x => mealCourse.ItsTooLateForChange(x)))
                    {
                        using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                        {
                            mealCourse = new MealCourse(mealCourseType, mealCourseTypesItems.ToList(), this);
                            mealCourse.StartsAt = DateTime.UtcNow;

                            newMealCourses.Add(mealCourse);
                            _Courses.Add(mealCourse);
                            stateTransition.Consistent = true;
                        }
                    }
                    else
                    {
                        using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                        {
                            foreach (var mealCourseItem in mealCourseTypesItems)
                                mealCourse.AddItem(mealCourseItem);

                            stateTransition.Consistent = true;
                        }
                    }
                }
            }

            if (newMealCourses.Count > 0)
                (ServicePointRunTime.ServicesContextRunTime.Current.MealsController as MealsController).OnNewMealCoursesInProgress(newMealCourses);

            MakeTradeItemsAvailableForServing();
        }



        /// <summary>
        /// Search for trade item that are by default prepared available for serving
        /// </summary>
        private void MakeTradeItemsAvailableForServing()
        {
            var tradeProductsItemsContexts = _Courses.ToThreadSafeList().SelectMany(x => x.FoodItemsInProgress).Where(x => x.PreparationStationIdentity == ItemsPreparationContext.TradeProductsStationIdentity).ToList();

            var tradeItemsUris = tradeProductsItemsContexts.SelectMany(x => x.PreparationItems).Where(x => x.State == ItemPreparationState.IsPrepared).Select(x => x.uid).ToList();

            var clientSessionsItems = (from servicePointPreparationItems in tradeProductsItemsContexts
                                       from itemPreparation in servicePointPreparationItems.PreparationItems
                                       where tradeItemsUris.Contains(itemPreparation.uid)
                                       group itemPreparation by itemPreparation.ClientSession into ClientSessionItems
                                       select new { clientSession = ClientSessionItems.Key, ClientSessionItems = ClientSessionItems.ToList() }).ToList();
            foreach (var clientSessionItems in clientSessionsItems)
                clientSessionItems.clientSession.ItemsServing(clientSessionItems.ClientSessionItems);
        }

        ///// <MetaDataID>{d71ac0eb-ed43-410f-80d8-ab8cce78f64d}</MetaDataID>
        //private void BuildMealTimePlan()
        //{


        //    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
        //    {
        //        MealCourse headCourse = Courses.FirstOrDefault()?.HeaderCourse as MealCourse;
        //        if (headCourse.ServedAtForecast == null)
        //        {
        //            var foodItemsPreparatioData = headCourse.FoodItems.OfType<ItemPreparation>().Select(x => new { foodItem = x, duration = ServicesContextResources.PreparationStation.GetPreparationData(x).Duration }).OrderByDescending(x => x.duration).ToList();
        //            headCourse.ServedAtForecast = DateTime.UtcNow + foodItemsPreparatioData[0].duration;
        //            foreach (var foodITem in foodItemsPreparatioData.Select(x => x.foodItem))
        //            {
        //                foodITem.State = ItemPreparationState.PreparationDelay;
        //                foodITem.PreparedAtForecast = headCourse.ServedAtForecast;
        //            }
        //        }

        //        foreach (MealCourse course in Courses)
        //        {
        //            if (course == headCourse || course.ServedAtForecast != null)
        //                continue;

        //            var foodItemsPreparatioData = course.FoodItems.OfType<ItemPreparation>().Select(x => new { foodItem = x, duration = ServicesContextResources.PreparationStation.GetPreparationData(x).Duration }).OrderByDescending(x => x.duration).ToList();

        //            DateTime shouldnotServedBefore = (Courses[Courses.IndexOf(course) - 1] as MealCourse).ServedAtForecast.Value + TimeSpan.FromMinutes((Courses[Courses.IndexOf(course) - 1] as MealCourse).DurationInMinutes);



        //            course.ServedAtForecast = System.DateTime.UtcNow + foodItemsPreparatioData[0].duration;
        //            if (course.ServedAtForecast < shouldnotServedBefore)
        //                course.ServedAtForecast = shouldnotServedBefore;

        //            foreach (var foodITem in foodItemsPreparatioData.Select(x => x.foodItem))
        //            {
        //                if (foodITem.State == ItemPreparationState.Committed)
        //                {
        //                    foodITem.State = ItemPreparationState.PreparationDelay;
        //                    foodITem.PreparedAtForecast = course.ServedAtForecast;
        //                }
        //            }
        //        }
        //        stateTransition.Consistent = true;
        //    }


        //}


        /// <MetaDataID>{4a767f54-cf13-46d7-8efd-7763ffcd80af}</MetaDataID>
        [BeforeCommitObjectStateInStorageCall]
        protected void BeforeCommitObjectState()
        {
            foreach (var course in Courses)
            {
                if (ObjectStorage.GetStorageOfObject(course) == null)
                    OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(course);
            }
        }

        internal void Merge(Meal meal)
        {
            if (meal == null)
                return;
            lock (MealLock)
            {

                foreach (var mealCourse in meal.Courses.OfType<MealCourse>())
                {
                    mealCourse.ChangeMeal(this);
                    var courses = meal.Courses.ToArray();
                }
            }
            ///// <MetaDataID>{df1bef38-aa51-450a-a6c2-bdb6b6f960a5}</MetaDataID>
            //public IFoodServiceSession Session => throw new System.NotImplementedException();
        }
    }

    public class ItemsMealCourseAssignment
    {
        public MealCourseType MealCourseType { get; }

        public MealCourse MealCourse { get; }

        public List<ItemPreparation> ItemsPreparations { get; }

        public ItemsMealCourseAssignment(MealCourseType mealCourseType, List<ItemPreparation> itemsPreparations, MealCourse mealCourse)
        {
            MealCourseType=mealCourseType;
            ItemsPreparations=itemsPreparations;
            MealCourse=mealCourse;
        }



    }
}