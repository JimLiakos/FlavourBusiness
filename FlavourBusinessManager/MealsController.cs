using FlavourBusinessFacade.EndUsers;
using FlavourBusinessFacade.HumanResources;
using FlavourBusinessFacade.RoomService;
using FlavourBusinessFacade.ServicesContextResources;
using FlavourBusinessFacade.Shipping;
using FlavourBusinessManager.HumanResources;
using FlavourBusinessManager.ServicePointRunTime;
using FlavourBusinessManager.ServicesContextResources;
using FlavourBusinessManager.Shipping;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Remoting;
using OOAdvantech.Transactions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using static System.Windows.Forms.AxHost;

namespace FlavourBusinessManager.RoomService
{
    /// <MetaDataID>{c31eb292-1b39-4081-9516-0e0e9e97b10a}</MetaDataID>
    public class MealsController : System.MarshalByRefObject, IExtMarshalByRefObject, IMealsController
    {
        static public System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
        /// <MetaDataID>{0a7aeeab-712e-4d32-b84b-4de237e3192c}</MetaDataID>
        public List<IMealCourse> MealCoursesInProgress
        {
            get
            {





                DateTime timeStamp = DateTime.UtcNow;
                var openSessions = ServicesContextRunTime.OpenSessions;
                TimeSpan timeSpan = (DateTime.UtcNow - timeStamp);

                var mealCourses = (from openSession in ServicesContextRunTime.OpenSessions
                                   where openSession.Meal != null
                                   from mealCource in openSession.Meal.Courses
                                   orderby mealCource.Meal.Session.ServicePoint.Description, (mealCource as MealCourse).MealCourseTypeOrder//.Courses.IndexOf(mealCource)
                                   select mealCource).ToList();
                TimeSpan timeSpan2 = (DateTime.UtcNow - timeStamp);

                var ids = mealCourses.Select(x => x.Identity).ToList();

                if (timer.IsRunning)
                {
                    timer.Stop();

                    var sdsd = timer.ElapsedMilliseconds;
                    //  System.IO.File.AppendAllText(@"f:\log.txt", Environment.NewLine + "elapsed time in millisecond :" + timer.ElapsedMilliseconds.ToString());
                }


                return mealCourses;
            }
        }

        public System.Collections.Generic.List<IMealCourse> GetMealCoursesInProgress(List<MealCourseAbbreviation> mealCoursesAtClientSide, string userLanguageCode)
        {
            var mealCourseInProgress = MealCoursesInProgress.Where(x => !mealCoursesAtClientSide.Any(y => y.Identity == x.Identity && y.TimeStamp == x.StateTimestamp)).ToList();

            if (!string.IsNullOrWhiteSpace(userLanguageCode))
            {
                foreach (var item in mealCourseInProgress.SelectMany(x => x.FoodItems).OfType<ItemPreparation>())
                    item.EnsurePresentationFor(CultureInfo.GetCultureInfo(userLanguageCode));
            }

            return mealCourseInProgress;
        }




        /// <MetaDataID>{db77bf43-40ea-48ca-830f-f713e88938aa}</MetaDataID>
        object buildPreparationPlanLock = new object();


        /// <summary>
        /// 
        /// </summary>
        /// <param name="preparationPlan"></param>
        /// <MetaDataID>{dfd3500e-b5f0-47da-b3b8-5c161f224ec3}</MetaDataID>
        private void RebuildPreparationPlan(PreparationPlan preparationPlan)
        {

            //return;
            //actionContext.PreparationStationspreParationPlanStartTime.Clear();
            lock (buildPreparationPlanLock)
            {
                try
                {
                    preparationPlan.RemoveOutOfPlanPreparationItems();
                    DateTime timeStamp = DateTime.UtcNow;
                    preparationPlan.PreparationPlanIsDoubleChecked = false;
                    bool stirTheSequence = true;



                    foreach (var preparationStation in ActivePreparationStations)
                        preparationStation.GetPreparationSections(preparationPlan);

                    while (!preparationPlan.PreparationPlanIsDoubleChecked)
                    {
                        preparationPlan.PreparationPlanIsDoubleChecked = true;

                        foreach (var preparationStation in ActivePreparationStations)
                        {
                            preparationStation.OptimizePreparationPlan(preparationPlan, stirTheSequence);
                            preparationStation.GetPredictions(preparationPlan, stirTheSequence);
                        }

                        TimeSpan timeSpan = (DateTime.UtcNow - timeStamp);
                        if (timeSpan.TotalSeconds > 2)
                            ComputationalResources.LogMessage.WriteLog("Load sessions time span : " + timeSpan.TotalSeconds.ToString());
                        timeStamp = DateTime.UtcNow;

                        stirTheSequence = false;
                    }
                    preparationPlan.LastPlanItemPreparationsStartsAt = new Dictionary<ItemPreparation, ItemPreparationStartsFor>(preparationPlan.ItemPreparationsStartsAt);
                    preparationPlan.PositionInterchanges.Clear();


                    #region Clear PreparationPlanStartTime in case preparationState is inactive and there are not items to prepare

                    foreach (var preparationStation in InActivePreparationStations)
                    {
                        if (preparationStation.FoodItemsInProgress.Count == 0)
                            preparationStation.PreparationPlanStartTime = null;
                    }
                    #endregion

                    foreach (var preparationStation in ActivePreparationStations)
                    {
                        preparationStation.ActionsOrderCommitted(preparationPlan);

                        #region clear PreparationPlanStartTime in case where there are not items in state pending to prepare 

                        if (preparationPlan.PreparationSections.ContainsKey(preparationStation))
                        {
                            DateTime? preparationPlanStartTime = preparationStation.PreparationPlanStartTime;

                            List<ItemPreparation> itemsToPrepare = (from thePartialAction in preparationPlan.PreparationSections[preparationStation]
                                                                    from itemPreparation in thePartialAction.GetItemsToPrepare()
                                                                    select itemPreparation).ToList();
                            if (itemsToPrepare.All(x => x.State.IsInPreviousState(ItemPreparationState.PendingPreparation)))
                            {
                                //in case where there aren't items pending to prepare clear PreparationPlanStartTime
                                preparationStation.PreparationPlanStartTime = null;
                            }
                        }
                        #endregion
                    }

                    foreach (var preparationStation in ActivePreparationStations)
                    {
                        var strings = preparationStation.GetActionsToStrings(preparationPlan);
                        if (strings.Count > 1)
                        {

                        }
                    }


                    RebuildPreparationPlanLastTime = DateTime.UtcNow;
                }
                catch (Exception error)
                {
                    var ss = error.StackTrace;

                    throw;
                }
            }
        }

        List<PreparationStation> InActivePreparationStations
        {
            get
            {
                List<PreparationStation> inActivePreparationStations = new OOAdvantech.Collections.Generic.List<PreparationStation>();
                foreach (var preparationStation in ServicesContextRunTime.Current.PreparationStations.OfType<PreparationStation>())
                {
                    if (!preparationStation.IsActive)
                        inActivePreparationStations.Add(preparationStation);
                    foreach (var subStation in preparationStation.SubStations.OfType<PreparationStation>())
                    {
                        if (!subStation.IsActive)
                            inActivePreparationStations.Add(subStation);
                    }
                }
                return inActivePreparationStations;
            }
        }

        /// <MetaDataID>{1c8b7c3a-e3a4-4ea4-9c77-3d61ed8d47f1}</MetaDataID>
        List<PreparationStation> ActivePreparationStations
        {
            get
            {

                DateTime timeStamp = DateTime.UtcNow;
                try
                {

                    return (from mealCourse in MealCoursesInProgress
                            from foodItemsInProgress in mealCourse.FoodItemsInProgress
                            from foodItem in foodItemsInProgress.PreparationItems
                            where foodItem.ActivePreparationStation != null
                            select foodItem.ActivePreparationStation).OfType<PreparationStation>().Distinct().ToList();
                }
                finally
                {
                    TimeSpan timeSpan2 = DateTime.UtcNow - timeStamp;
                }
            }
        }

        /// <exclude>Excluded</exclude>  
        DateTime _RebuildPreparationPlanLastTime;

        /// <MetaDataID>{9c0a13ae-a2d2-4371-b944-60605d818dcd}</MetaDataID>
        public DateTime RebuildPreparationPlanLastTime
        {
            get
            {
                lock (buildPreparationPlanLock)
                    return _RebuildPreparationPlanLastTime;
            }
            private set
            {
                lock (buildPreparationPlanLock)
                    _RebuildPreparationPlanLastTime = value;
            }
        }


        /// <MetaDataID>{05e0750f-3947-4c50-b0b7-28b7ecc6f434}</MetaDataID>
        internal void RecalculateServedAtForecast()
        {

            var mealCoursesUnderPreparation = (from openSession in ServicesContextRunTime.OpenSessions
                                               where openSession.Meal != null
                                               from mealCource in openSession.Meal.Courses
                                               where mealCource.PreparationState.IsInPreviousState(ItemPreparationState.Serving)
                                               orderby mealCource.SortID
                                               select mealCource).ToList();

        }

        /// <MetaDataID>{2545475d-7f8d-4eb5-b7ec-12ca0dc59bfa}</MetaDataID>
        public readonly ServicePointRunTime.ServicesContextRunTime ServicesContextRunTime;

        public event NewMealCoursesInProgressHandler NewMealCoursesInProgress;
        public event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;

        public event MealCourseChangeStateHandler MealCourseChangeState;
        public event MealCourseItemsStateChangedHandler MealCourseItemsStateChanged;


        internal void MealCourseStateChanged(MealCourse mealCourse, string member)
        {
            MealCourseChangeState?.Invoke(mealCourse, member);

        }
        internal void MealCourseItemsChangeState(MealCourse mealCourse, Dictionary<string, ItemPreparationState> newItemsState)
        {
            MealCourseItemsStateChanged?.Invoke(mealCourse, newItemsState);


            var foodShippings = (from foodShipping in mealCourse.ServingBatches.OfType<FoodShipping>()
                                 from preparationItem in foodShipping.PreparedItems
                                 where newItemsState.ContainsKey(preparationItem.uid)
                                 select foodShipping).Distinct().ToList();

            foreach (var foodShipping in foodShippings)
            {
                var foodShippingNewItemsState = foodShipping.PreparedItems.Where(x => newItemsState.ContainsKey(x.uid)).ToDictionary(x => x.uid, x => x.State);

                foodShipping.OnItemsChangeState(foodShippingNewItemsState);
            }

        }

        /// <MetaDataID>{678886b6-415d-4058-8264-0847b0872a3a}</MetaDataID>
        public MealsController(ServicePointRunTime.ServicesContextRunTime servicesContextRunTime)
        {
            ServicesContextRunTime = servicesContextRunTime;

        }
        /// <MetaDataID>{6570680d-a627-47c1-b385-2919e07bb359}</MetaDataID>
        ~MealsController()
        {
            System.Diagnostics.Debug.WriteLine("MealsController");
        }

        /// <MetaDataID>{287105f2-4956-4515-b8f8-a1b06d4d5092}</MetaDataID>
        internal void OnNewMealCoursesInProgress(List<IMealCourse> mealCourses)
        {
            NewMealCoursesInProgress?.Invoke(mealCourses);

            //(ServicesContextRunTime.MealsController as MealsController).ReadyToServeMealcoursesCheck(mealCourses);
            //you have to  filter mealcourses by state.
        }

        /// <MetaDataID>{31cd58c7-882d-4902-9d94-8b63c8ee71bc}</MetaDataID>
        internal void OnRemoveMealCoursesInrogress(List<IMealCourse> mealCourses)
        {
            ObjectChangeState?.Invoke(this, nameof(MealCoursesInProgress));

            //you have to  filter mealcourses by state.
        }
        /// <summary>
        /// In case where the prerequisites fulfilled, assigns all unassigned partial sessions to a meal session 
        /// </summary>
        /// <param name="referenceClientSession">
        /// Defines a partial session as reference to identify service point meal
        /// </param>
        /// <MetaDataID>{b6588e83-9deb-4b52-b259-b8aa89fbdc4c}</MetaDataID>
        public void AutoMealParticipation(EndUsers.FoodServiceClientSession referenceClientSession)
        {
            if (referenceClientSession.MainSession != null)
                return;
            FoodServiceSession foodServiceSession = (referenceClientSession.ServicePoint as HallServicePoint)?.ActiveFoodServiceClientSessions.Where(x => x.MainSession != null && (x.MainSession as FoodServiceSession).CanIncludeAsPart(referenceClientSession)).Select(x => x.MainSession).OfType<FoodServiceSession>().OrderBy(x => x.SessionStarts).LastOrDefault();

            if (foodServiceSession == null)
            {

                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {
                    foodServiceSession = referenceClientSession.ServicePoint.NewFoodServiceSession() as FoodServiceSession;
                    ObjectStorage.GetStorageOfObject(ServicesContextRunTime.Current).CommitTransientObjectState(foodServiceSession);
                    if (referenceClientSession.Menu != null)
                        foodServiceSession.MenuStorageIdentity = referenceClientSession.Menu.StorageIdentity;

                    foodServiceSession.SessionType = referenceClientSession.SessionType;
                    if (foodServiceSession.SessionType == FlavourBusinessFacade.EndUsers.SessionType.HomeDeliveryGuest)
                        foodServiceSession.SessionType = FlavourBusinessFacade.EndUsers.SessionType.HomeDelivery;

                    foodServiceSession.AddPartialSession(referenceClientSession);
                    referenceClientSession.ImplicitMealParticipation = true;


                    //Add all individual active sessions to service point session
                    foreach (var unAssignedClientSession in (referenceClientSession.ServicePoint as ServicePoint).OpenClientSessions.OfType<EndUsers.FoodServiceClientSession>().Where(x => x.MainSession == null && !x.Forgotten))
                    {
                        foodServiceSession.AddPartialSession(unAssignedClientSession);
                        unAssignedClientSession.ImplicitMealParticipation = true;
                    }

                    stateTransition.Consistent = true;
                }


            }
            else
            {

                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {
                    foodServiceSession.AddPartialSession(referenceClientSession);
                    referenceClientSession.ImplicitMealParticipation = true;
                    stateTransition.Consistent = true;
                }
            }

            //referencClientSession.


        }

        public List<DelayedServingBatchAbbreviation> GetDelayedServingBatchesAtTheCounter(double delayInMins)
        {

            var servingBatches = GetServingBatchesAtTheCounter();
            var delayedServiceBatches = servingBatches.Where(x => x.CreationTime != null && (DateTime.UtcNow - x.CreationTime.Value.ToUniversalTime()).TotalMinutes > delayInMins)
                .Select(x => new DelayedServingBatchAbbreviation(x)).ToList();

            return delayedServiceBatches;
        }

        public IFoodShipping GetMealCourseFoodShipping(string mealCourseUri)
        {
            var mealCourse = ObjectStorage.GetObjectFromUri(mealCourseUri) as MealCourse;


            var foodShiping = mealCourse.ServingBatches.OfType<FoodShipping>().FirstOrDefault();
            if (foodShiping == null)
                foodShiping = GetServingBatchesAtTheCounter().OfType<FoodShipping>().Where(x => x.MealCourse == mealCourse).FirstOrDefault();
            return foodShiping;
        }





        internal IList<IServingBatch> GetServingBatchesAtTheCounter()
        {

            var mealCoursesForHallServing = (from mealCourse in MealCoursesInProgress
                                             from itemsPreparationContext in mealCourse.FoodItemsInProgress
                                             from itemPreparation in itemsPreparationContext.PreparationItems
                                             where (itemPreparation.State == ItemPreparationState.Serving) &&
                                             (mealCourse.Meal.Session.ServicePoint is HallServicePoint)
                                             select mealCourse).Distinct().ToList();

            List<ServingBatch> servingBatches = new List<ServingBatch>();
            foreach (var mealCourse in mealCoursesForHallServing)
            {

                IList<ItemsPreparationContext> preparedItems = (from itemsPreparationContext in mealCourse.FoodItemsInProgress
                                                                where itemsPreparationContext.PreparationItems.All(x => (x.State == ItemPreparationState.Serving || x.State == ItemPreparationState.OnRoad))
                                                                select itemsPreparationContext).ToList();

                IList<ItemsPreparationContext> underPreparationItems = (from itemsPreparationContext in mealCourse.FoodItemsInProgress
                                                                        where itemsPreparationContext.PreparationItems.Any(x => x.State == ItemPreparationState.PendingPreparation ||
                                                                        x.State == ItemPreparationState.InPreparation ||
                                                                        x.State == ItemPreparationState.IsRoasting ||
                                                                        x.State == ItemPreparationState.IsPrepared)
                                                                        select itemsPreparationContext).ToList();

                var mealCourseUri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(mealCourse)?.GetPersistentObjectUri(mealCourse);

                var servingBatch = (from itemsPreparationContext in preparedItems
                                    from itemPreparation in itemsPreparationContext.PreparationItems
                                    where itemPreparation.ServedInTheBatch != null && itemPreparation.ServedInTheBatch.MealCourse == mealCourse
                                    select itemPreparation.ServedInTheBatch).OfType<ServingBatch>().FirstOrDefault();
                if (servingBatch == null)
                {
                    servingBatch = mealCourse.ServingBatches.OfType<ServingBatch>().ToList().Where(x => !x.IsAssigned).FirstOrDefault();
                    if (servingBatch == null)
                    {
                        using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.RequiresNew))
                        {
                            servingBatch = new ServingBatch(mealCourse, preparedItems, underPreparationItems);
                            ObjectStorage.GetStorageOfObject(mealCourse).CommitTransientObjectState(servingBatch);
                            servingBatches.Add(servingBatch);// new ServingBatch(mealCourse, preparedItems, underPreparationItems));
                            stateTransition.Consistent = true;
                        }
                    }
                }
                servingBatch.Update(mealCourse, preparedItems, underPreparationItems);
                servingBatches.Add(servingBatch);



            }


            int i = 0;
            foreach (var servingBatch in servingBatches)
                servingBatch.SortID = i++;



            List<FoodShipping> foodShipings = new List<FoodShipping>();

            var mealCoursesForShippins = (from mealCourse in MealCoursesInProgress
                                          from itemsPreparationContext in mealCourse.FoodItemsInProgress
                                          from itemPreparation in itemsPreparationContext.PreparationItems
                                          where (itemPreparation.State == ItemPreparationState.Serving) && (mealCourse.Meal.Session.ServicePoint is HomeDeliveryServicePoint)
                                          select mealCourse).Distinct().ToList();


            foreach (var mealCourse in mealCoursesForShippins)
            {


                IList<ItemsPreparationContext> preparedItems = (from itemsPreparationContext in mealCourse.FoodItemsInProgress
                                                                where itemsPreparationContext.PreparationItems.All(x => (x.State == ItemPreparationState.Serving || x.State == ItemPreparationState.OnRoad))
                                                                select itemsPreparationContext).ToList();

                IList<ItemsPreparationContext> underPreparationItems = (from itemsPreparationContext in mealCourse.FoodItemsInProgress
                                                                        where itemsPreparationContext.PreparationItems.Any(x => x.State == ItemPreparationState.PendingPreparation ||
                                                                        x.State == ItemPreparationState.InPreparation ||
                                                                        x.State == ItemPreparationState.IsRoasting ||
                                                                        x.State == ItemPreparationState.IsPrepared)
                                                                        select itemsPreparationContext).ToList();

                if (underPreparationItems.Count == 0)
                {

                    var mealCourseUri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(mealCourse)?.GetPersistentObjectUri(mealCourse);

                    var foodShipping = (from itemsPreparationContext in preparedItems
                                        from itemPreparation in itemsPreparationContext.PreparationItems
                                        where itemPreparation.ServedInTheBatch != null && itemPreparation.ServedInTheBatch.MealCourse == mealCourse
                                        select itemPreparation.ServedInTheBatch).OfType<FoodShipping>().FirstOrDefault();
                    if (foodShipping == null)
                    {
                        foodShipping = mealCourse.ServingBatches.OfType<FoodShipping>().ToList().Where(x => !x.IsAssigned).FirstOrDefault();
                        if (foodShipping == null)
                        {
                            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.RequiresNew))
                            {
                                foodShipping = new FoodShipping(mealCourse, preparedItems, underPreparationItems);
                                ObjectStorage.GetStorageOfObject(mealCourse).CommitTransientObjectState(foodShipping);

                                foodShipings.Add(foodShipping);

                                stateTransition.Consistent = true;
                            }
                        }
                    }
                    foodShipping.Update(mealCourse, preparedItems, underPreparationItems);
                    foodShipings.Add(foodShipping);
                }

            }


            i = 0;
            foreach (var foodShiping in foodShipings)
                foodShiping.SortID = i++;

            var allServingBatches = servingBatches.OfType<IServingBatch>().Union(foodShipings.OfType<IServingBatch>()).ToList();


            return allServingBatches;


        }

        ///// <MetaDataID>{cd97524c-d96b-4469-8ad1-dbc709729bb4}</MetaDataID>
        //Dictionary<IMealCourse, ServingBatch> UnassignedMealCourses = new Dictionary<IMealCourse, ServingBatch>();

        /// <MetaDataID>{e7cf8250-6b70-4095-bf94-bf087a79d362}</MetaDataID>
        internal IList<ServingBatch> GetServingBatches(HumanResources.Waiter waiter)
        {

            var activeShiftWork = ServicesContextRunTime.Current.GetActiveShiftWorks();

            List<ServingBatch> servingBatches = new List<ServingBatch>();
            if (waiter.ShiftWork != null)
            {

                //var sdsd = (from mealCourse in MealCoursesInProgress
                //            from itemsPreparationContext in mealCourse.FoodItemsInProgress
                //            from itemPreparation in itemsPreparationContext.PreparationItems
                //            select new { itemPreparation.Name, itemPreparation.State, itemPreparation }).ToList();

                var mealCoursesToServe = (from mealCourse in MealCoursesInProgress
                                          from itemsPreparationContext in mealCourse.FoodItemsInProgress
                                          from itemPreparation in itemsPreparationContext.PreparationItems
                                          where (itemPreparation.State == ItemPreparationState.Serving || itemPreparation.State == ItemPreparationState.OnRoad) &&
                                          (mealCourse.Meal.Session.ServicePoint is HallServicePoint) && (mealCourse.Meal.Session.ServicePoint as HallServicePoint).CanBeAssignedTo(waiter, waiter.ShiftWork)
                                          select mealCourse).Distinct().ToList();

                foreach (var mealCourse in mealCoursesToServe)
                {


                    IList<ItemsPreparationContext> preparedItems = (from itemsPreparationContext in mealCourse.FoodItemsInProgress
                                                                    where itemsPreparationContext.PreparationItems.All(x => (x.State == ItemPreparationState.Serving || x.State == ItemPreparationState.OnRoad))
                                                                    select itemsPreparationContext).ToList();

                    IList<ItemsPreparationContext> underPreparationItems = (from itemsPreparationContext in mealCourse.FoodItemsInProgress
                                                                            where itemsPreparationContext.PreparationItems.Any(x => x.State == ItemPreparationState.PendingPreparation ||
                                                                            x.State == ItemPreparationState.InPreparation ||
                                                                            x.State == ItemPreparationState.IsRoasting ||
                                                                            x.State == ItemPreparationState.IsPrepared)
                                                                            select itemsPreparationContext).ToList();

                    var mealCourseUri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(mealCourse)?.GetPersistentObjectUri(mealCourse);

                    var servingBatch = (from itemsPreparationContext in preparedItems
                                        from itemPreparation in itemsPreparationContext.PreparationItems
                                        where itemPreparation.ServedInTheBatch != null && itemPreparation.ServedInTheBatch.MealCourse == mealCourse
                                        select itemPreparation.ServedInTheBatch).OfType<ServingBatch>().FirstOrDefault();
                    if (servingBatch == null)
                    {
                        servingBatch = mealCourse.ServingBatches.OfType<ServingBatch>().ToList().Where(x => !x.IsAssigned).FirstOrDefault();
                        if (servingBatch == null)
                        {
                            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.RequiresNew))
                            {
                                servingBatch = new ServingBatch(mealCourse, preparedItems, underPreparationItems);
                                ObjectStorage.GetStorageOfObject(mealCourse).CommitTransientObjectState(servingBatch);
                                servingBatches.Add(servingBatch);// new ServingBatch(mealCourse, preparedItems, underPreparationItems));
                                stateTransition.Consistent = true;
                            }
                        }
                    }
                    servingBatch.Update(mealCourse, preparedItems, underPreparationItems);
                    servingBatches.Add(servingBatch);

                }
            }

            int i = 0;
            foreach (var servingBatch in servingBatches)
                servingBatch.SortID = i++;

            return servingBatches;
        }



        /// <MetaDataID>{8b512a24-5f97-4489-81dc-f0c6360e1a75}</MetaDataID>
        internal IList<FoodShipping> GetFoodShippings(Courier courier)
        {

            var activeShiftWork = ServicesContextRunTime.Current.GetActiveShiftWorks();

            List<FoodShipping> foodShippings = new List<FoodShipping>();
            if (courier.ShiftWork != null)
            {

                //var sdsd = (from mealCourse in MealCoursesInProgress
                //            from itemsPreparationContext in mealCourse.FoodItemsInProgress
                //            from itemPreparation in itemsPreparationContext.PreparationItems
                //            select new { itemPreparation.Name, itemPreparation.State, itemPreparation }).ToList();
                List<IMealCourse> mealCoursesToServe = null;

                var courierState = courier.State;
                if (courierState == CourierState.OnTheRoad || courierState == CourierState.NearDeliveryServicePoint)
                {
                    mealCoursesToServe = (from mealCourse in MealCoursesInProgress
                                          from itemsPreparationContext in mealCourse.FoodItemsInProgress
                                          from itemPreparation in itemsPreparationContext.PreparationItems
                                          where (itemPreparation.State == ItemPreparationState.OnRoad) &&
                                          (mealCourse.Meal.Session.ServicePoint is HomeDeliveryServicePoint) && (mealCourse.Meal.Session.ServicePoint as HomeDeliveryServicePoint).CanBeAssignedTo(courier, courier.ShiftWork)
                                          select mealCourse).Distinct().ToList();

                }
                else
                {
                    mealCoursesToServe = (from mealCourse in MealCoursesInProgress
                                          from itemsPreparationContext in mealCourse.FoodItemsInProgress
                                          from itemPreparation in itemsPreparationContext.PreparationItems
                                          where (itemPreparation.State == ItemPreparationState.Serving || itemPreparation.State == ItemPreparationState.OnRoad) &&
                                          (mealCourse.Meal.Session.ServicePoint is HomeDeliveryServicePoint) && (mealCourse.Meal.Session.ServicePoint as HomeDeliveryServicePoint).CanBeAssignedTo(courier, courier.ShiftWork)
                                          select mealCourse).Distinct().ToList();

                }

                foreach (var mealCourse in mealCoursesToServe)
                {


                    IList<ItemsPreparationContext> preparedItems = (from itemsPreparationContext in mealCourse.FoodItemsInProgress
                                                                    where itemsPreparationContext.PreparationItems.All(x => (x.State == ItemPreparationState.Serving || x.State == ItemPreparationState.OnRoad))
                                                                    select itemsPreparationContext).ToList();

                    IList<ItemsPreparationContext> underPreparationItems = (from itemsPreparationContext in mealCourse.FoodItemsInProgress
                                                                            where itemsPreparationContext.PreparationItems.Any(x => x.State == ItemPreparationState.PendingPreparation ||
                                                                            x.State == ItemPreparationState.InPreparation ||
                                                                            x.State == ItemPreparationState.IsRoasting ||
                                                                            x.State == ItemPreparationState.IsPrepared)
                                                                            select itemsPreparationContext).ToList();

                    if (underPreparationItems.Count == 0)
                    {
                        var mealCourseUri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(mealCourse)?.GetPersistentObjectUri(mealCourse);

                        var foodShipping = (from itemsPreparationContext in preparedItems
                                            from itemPreparation in itemsPreparationContext.PreparationItems
                                            where itemPreparation.ServedInTheBatch != null && itemPreparation.ServedInTheBatch.MealCourse == mealCourse
                                            select itemPreparation.ServedInTheBatch).OfType<FoodShipping>().FirstOrDefault();
                        if (foodShipping == null)
                        {
                            foodShipping = mealCourse.ServingBatches.OfType<FoodShipping>().ToList().FirstOrDefault();
                            if (foodShipping == null)
                            {

                                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.RequiresNew))
                                {
                                    foodShipping = new FoodShipping(mealCourse, preparedItems, underPreparationItems);
                                    ObjectStorage.GetStorageOfObject(mealCourse).CommitTransientObjectState(foodShipping);

                                    foodShippings.Add(foodShipping);

                                    stateTransition.Consistent = true;
                                }
                            }
                        }
                        foodShipping.Update(mealCourse, preparedItems, underPreparationItems);
                        if (foodShipping.IsAssigned)
                        {
                            if (foodShipping.ShiftWork.Worker == courier)
                                foodShippings.Add(foodShipping);
                        }
                        else
                            foodShippings.Add(foodShipping);

                    }

                }
            }

            int i = 0;
            foreach (var foodShiping in foodShippings)
                foodShiping.SortID = i++;

            return foodShippings;
        }


        /// <MetaDataID>{cad5c370-7874-46bf-a785-93efc6d68ed7}</MetaDataID>
        internal void ServingBatchDeAssigned(HumanResources.Waiter waiter, IServingBatch servingBatch)
        {

            var servicePoint = servingBatch.MealCourse.Meal.Session.ServicePoint as HallServicePoint;// ServicesContextRunTime.Current.OpenSessions.Select(x => x.ServicePoint).OfType<HallServicePoint>().Where(x => x.ServicesPointIdentity == servingBatch.ServicesPointIdentity).FirstOrDefault();

            var activeWaiters = (from shiftWork in ServicesContextRunTime.Current.GetActiveShiftWorks()
                                 where shiftWork.Worker is IWaiter && servicePoint.CanBeAssignedTo(shiftWork.Worker as IWaiter, shiftWork) && shiftWork.Worker != waiter
                                 select shiftWork.Worker).OfType<HumanResources.Waiter>().ToList();

            foreach (var a_Waiter in activeWaiters)
                a_Waiter.FindServingBatchesChanged();
        }

        internal void FoodShippingDeAssigned(Courier courier, IFoodShipping foodShipping)
        {

            var servicePoint = ServicesContextRunTime.Current.OpenSessions.Select(x => x.ServicePoint).OfType<HomeDeliveryServicePoint>().Where(x => x.ServicesPointIdentity == foodShipping.ServicesPointIdentity).FirstOrDefault();

            var activeCouriers = (from shiftWork in ServicesContextRunTime.Current.GetActiveShiftWorks()
                                  where shiftWork.Worker is ICourier && servicePoint.CanBeAssignedTo(shiftWork.Worker as ICourier, shiftWork) && shiftWork.Worker != courier
                                  select shiftWork.Worker).OfType<HumanResources.Courier>().ToList();

            foreach (var a_Courier in activeCouriers)
                a_Courier.FindFoodShippingsChanges();
        }

        /// <MetaDataID>{2cdcccbe-599b-4ebd-8c71-a5e7db5a1bed}</MetaDataID>
        object MealsControllerLock = new object();
        /// <MetaDataID>{4786b94c-efed-46cf-93d2-307a2343de57}</MetaDataID>
        internal int GetNextSortingID()
        {
            lock (MealsControllerLock)
            {
                MealCourseSortID++;
            }
            return MealCourseSortID;
        }

        /// <MetaDataID>{8dd7566f-8b5f-4107-b93f-913139929d09}</MetaDataID>
        int MealCourseSortID;
        /// <MetaDataID>{b5895f92-f271-4868-aaf6-dbbd158d4558}</MetaDataID>
        internal void Init()
        {
            lock (MealsControllerLock)
            {
                var lastMealCourse = (from openSession in ServicesContextRunTime.OpenSessions
                                      where openSession.Meal != null
                                      from mealCource in openSession.Meal.Courses
                                      orderby mealCource.SortID
                                      select mealCource).LastOrDefault();

                if (lastMealCourse == null)
                    MealCourseSortID = 0;
                else
                    MealCourseSortID = lastMealCourse.SortID + 1;


                RunMonitoring();

                RecalculateServedAtForecast();

            }
        }

        /// <MetaDataID>{fa568071-4178-4e14-a25c-51f4f8498bef}</MetaDataID>
        PreparationPlan ActionContext = new PreparationPlan();

        /// <MetaDataID>{dfff57b8-a93a-48b8-a2f0-b87345e77f04}</MetaDataID>
        Task MonitoringTask;


        /// <MetaDataID>{34c5fae2-3f2b-4531-8db6-3752ec0ba8c8}</MetaDataID>
        internal void RunMonitoring()
        {
            lock (MealsControllerLock)
            {
                if (MonitoringTask != null && !MonitoringTask.IsCompleted)
                    return;

                MonitoringTask = Task.Run(() =>
                {

                    while (true)
                    {
                        if ((DateTime.UtcNow - RebuildPreparationPlanLastTime).TotalSeconds >= 15)
                            RebuildPreparationPlan(ActionContext);
                        System.Threading.Thread.Sleep(10000);
                    }
                });

            }

        }




        /// <MetaDataID>{eb9a6c0c-3a5e-4115-a6e2-c1aaee2b0fb9}</MetaDataID>
        internal void ServingBatchAssigned(HumanResources.Waiter waiter, IServingBatch servingBatch)
        {

            var servicePoint = ServicesContextRunTime.Current.OpenSessions.Select(x => x.ServicePoint).OfType<HallServicePoint>().Where(x => x.ServicesPointIdentity == servingBatch.ServicesPointIdentity).FirstOrDefault();

            var activeWaiters = (from shiftWork in ServicesContextRunTime.Current.GetActiveShiftWorks()
                                 where shiftWork.Worker is IWaiter && servicePoint.CanBeAssignedTo(shiftWork.Worker as IWaiter, shiftWork) && shiftWork.Worker != waiter
                                 select shiftWork.Worker).OfType<HumanResources.Waiter>().ToList();

            foreach (var a_Waiter in activeWaiters)
                a_Waiter.FindServingBatchesChanged();

        }


        internal void FoodShippingAssigned(HumanResources.Courier courier, IFoodShipping foodShipping)
        {

            var servicePoint = ServicesContextRunTime.Current.OpenSessions.Select(x => x.ServicePoint).OfType<HomeDeliveryServicePoint>().Where(x => x.ServicesPointIdentity == foodShipping.ServicesPointIdentity).FirstOrDefault();

            var activeCouriers = (from shiftWork in ServicesContextRunTime.Current.GetActiveShiftWorks()
                                  where shiftWork.Worker is ICourier && servicePoint.CanBeAssignedTo(shiftWork.Worker as ICourier, shiftWork) && shiftWork.Worker != courier
                                  select shiftWork.Worker).OfType<HumanResources.Courier>().ToList();

            foreach (var a_Courier in activeCouriers)
                a_Courier.FindFoodShippingsChanges();

        }


        /// <MetaDataID>{c79fa8ae-e4b6-452e-96d8-9f590b3e5e04}</MetaDataID>
        public void MoveCourseBefore(string mealCourseAsReferenceUri, string movedMealCourseUri)
        {

        }

        /// <MetaDataID>{1f3b96de-e184-4669-897b-17c69b3c260b}</MetaDataID>
        public void MoveCourseAfter(string mealCourseAsReferenceUri, string movedMealCourseUri)
        {

        }

        /// <MetaDataID>{6bbb221b-cac1-435d-832a-e095be1d5070}</MetaDataID>
        internal PreparationPlan RebuildPreparationPlan()
        {
            RebuildPreparationPlan(ActionContext);
            return ActionContext;
        }

        /// <MetaDataID>{d718d244-0e50-4ac9-b66a-495fc9f91b43}</MetaDataID>
        internal void MealItemsReadyToServe(Meal meal)
        {
            if (meal.Session?.SessionType == SessionType.Hall)
            {
                var servicePoint = meal.Session.ServicePoint as HallServicePoint;

                var activeWaiters = (from shiftWork in ServicesContextRunTime.Current.GetActiveShiftWorks()
                                     where shiftWork.Worker is IWaiter && servicePoint.CanBeAssignedTo(shiftWork.Worker as IWaiter, shiftWork)
                                     select shiftWork.Worker).OfType<HumanResources.Waiter>().ToList();

                foreach (var waiter in activeWaiters)
                {
                    if (waiter.ShiftWork != null && DateTime.UtcNow > waiter.ShiftWork.StartsAt.ToUniversalTime() && DateTime.UtcNow < waiter.ShiftWork.EndsAt.ToUniversalTime())
                    {
                        var clientMessage = waiter.Messages.Where(x => x.GetDataValue<ClientMessages>("ClientMessageType") == ClientMessages.ItemsReadyToServe && !x.MessageReaded).OrderBy(x => x.MessageTimestamp).FirstOrDefault();
                        if (clientMessage == null)
                        {
                            clientMessage = new Message();
                            clientMessage.Data["ClientMessageType"] = ClientMessages.ItemsReadyToServe;
                            clientMessage.Data["ServicesPointIdentity"] = servicePoint.ServicesPointIdentity;
                            clientMessage.Notification = new Notification() { Title = "There are items read to serve", Body = "Check items ready to serve List" };
                        }
                        waiter.PushMessage(clientMessage);

                        if (!string.IsNullOrWhiteSpace(waiter.DeviceFirebaseToken))
                        {
                            CloudNotificationManager.SendMessage(clientMessage, waiter.DeviceFirebaseToken);
                            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                            {
                                foreach (var message in waiter.Messages.Where(x => x.GetDataValue<ClientMessages>("ClientMessageType") == ClientMessages.ItemsReadyToServe && !x.MessageReaded))
                                {
                                    message.NotificationsNum += 1;
                                    message.NotificationTimestamp = DateTime.UtcNow;
                                }

                                stateTransition.Consistent = true;
                            }
                        }
                    }
                }

                ServicesContextRunTime.Current.UpdateWaitersWithUnreadMessages();
            }

            if (meal.Session?.SessionType == SessionType.HomeDelivery && meal.Courses.All(x => x.PreparationState == ItemPreparationState.Serving))
            {

                var servicePoint = meal.Session.ServicePoint as HomeDeliveryServicePoint;


                List<HumanResources.Courier> activeCouriersForMealCoursesShipping = new List<HumanResources.Courier>();

                activeCouriersForMealCoursesShipping.AddRange((from shiftWork in ServicesContextRunTime.Current.GetActiveShiftWorks()
                                                               where shiftWork.Worker is ICourier && servicePoint.CanBeAssignedTo(shiftWork.Worker as ICourier, shiftWork)
                                                               select shiftWork.Worker).OfType<Courier>());

                activeCouriersForMealCoursesShipping = activeCouriersForMealCoursesShipping.Distinct().ToList();


                //foreach (var a_Courier in activeCouriersFormealCoursesShipping)
                //    a_Courier.FindNewFoodShippings();


                foreach (var courier in activeCouriersForMealCoursesShipping)
                {
                    if (courier.ShiftWork != null && DateTime.UtcNow > courier.ShiftWork.StartsAt.ToUniversalTime() && DateTime.UtcNow < courier.ShiftWork.EndsAt.ToUniversalTime())
                    {
                        var clientMessage = courier.Messages.Where(x => x.GetDataValue<ClientMessages>("ClientMessageType") == ClientMessages.ItemsReadyToServe && !x.MessageReaded).OrderBy(x => x.MessageTimestamp).FirstOrDefault();
                        if (clientMessage == null)
                        {
                            clientMessage = new Message();
                            clientMessage.Data["ClientMessageType"] = ClientMessages.ItemsReadyToServe;
                            clientMessage.Data["ServicesPointIdentity"] = servicePoint.ServicesPointIdentity;
                            clientMessage.Notification = new Notification() { Title = "There are items read to serve", Body = "Check items ready to serve List" };
                        }
                        courier.PushMessage(clientMessage);

                        if (!string.IsNullOrWhiteSpace(courier.DeviceFirebaseToken))
                        {
                            CloudNotificationManager.SendMessage(clientMessage, courier.DeviceFirebaseToken);
                            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                            {
                                foreach (var message in courier.Messages.Where(x => x.GetDataValue<ClientMessages>("ClientMessageType") == ClientMessages.ItemsReadyToServe && !x.MessageReaded))
                                {
                                    message.NotificationsNum += 1;
                                    message.NotificationTimestamp = DateTime.UtcNow;
                                }

                                stateTransition.Consistent = true;
                            }
                        }
                    }
                }


                ServicesContextRunTime.Current.UpdateCouriersWithUnreadMessages();
            }


        }

        /// <summary>
        /// Redistribute all the items to available preparation stations
        /// where its state is PendingPreparation
        /// </summary>
        ///<remarks>
        ///</remarks>
        internal void MealPrepStationsRedistribution()
        {
            foreach (MealCourse mealCourse in MealCoursesInProgress)
                mealCourse.MealPrepStationsRedistribution();

            RebuildPreparationPlan();
        }


        /// <summary>
        /// This method finds all waiter which can serve the meal courses and update them for changes
        /// </summary>
        /// <param name="mealCourses">
        /// Defines the meal courses which are ready for serving
        /// </param>

        /// <MetaDataID>{17792be3-f73c-4e93-b382-6848fcef9521}</MetaDataID>
        internal void ReadyToServeMealCoursesCheck(List<IMealCourse> mealCourses)
        {


            var mealCoursesToServe = (from mealCourse in mealCourses
                                      where mealCourse.Meal.Session.SessionType == FlavourBusinessFacade.EndUsers.SessionType.Hall
                                      from itemsPreparationContext in mealCourse.FoodItemsInProgress
                                      from itemPreparation in itemsPreparationContext.PreparationItems
                                      where (itemPreparation.State == ItemPreparationState.Serving || itemPreparation.State == ItemPreparationState.OnRoad)
                                      select mealCourse).Distinct().ToList();
            var mealCoursesServicesPoints = mealCoursesToServe.Select(x => x.Meal.Session.ServicePoint).OfType<ServicePoint>().ToList();


            List<HumanResources.Waiter> activeWaitersFormealCoursesServing = new List<HumanResources.Waiter>();
            foreach (var servicePoint in mealCoursesServicesPoints.OfType<HallServicePoint>())
            {
                activeWaitersFormealCoursesServing.AddRange((from shiftWork in ServicesContextRunTime.Current.GetActiveShiftWorks()
                                                             where shiftWork.Worker is IWaiter && servicePoint.CanBeAssignedTo(shiftWork.Worker as IWaiter, shiftWork)
                                                             select shiftWork.Worker).OfType<HumanResources.Waiter>());
            }
            activeWaitersFormealCoursesServing = activeWaitersFormealCoursesServing.Distinct().ToList();


            foreach (var a_Waiter in activeWaitersFormealCoursesServing)
                a_Waiter.FindServingBatchesChanged();




            List<HumanResources.Courier> activeCouriersFormealCoursesShipping = new List<HumanResources.Courier>();
            foreach (var servicePoint in mealCoursesServicesPoints.OfType<HomeDeliveryServicePoint>())
            {
                activeCouriersFormealCoursesShipping.AddRange((from shiftWork in ServicesContextRunTime.Current.GetActiveShiftWorks()
                                                               where shiftWork.Worker is ICourier && servicePoint.CanBeAssignedTo(shiftWork.Worker as ICourier, shiftWork)
                                                               select shiftWork.Worker).OfType<Courier>());
            }
            activeCouriersFormealCoursesShipping = activeCouriersFormealCoursesShipping.Distinct().ToList();


            foreach (var a_Courier in activeCouriersFormealCoursesShipping)
                a_Courier.FindFoodShippingsChanges();




            var mealCoursesReadyToHomeDelivery = (from mealCourse in mealCourses
                                                  where mealCourse.Meal.Session.SessionType == FlavourBusinessFacade.EndUsers.SessionType.HomeDelivery
                                                  from itemsPreparationContext in mealCourse.FoodItemsInProgress
                                                  from itemPreparation in itemsPreparationContext.PreparationItems
                                                  where (itemPreparation.State == ItemPreparationState.Serving || itemPreparation.State == ItemPreparationState.OnRoad)
                                                  select mealCourse).Distinct().ToList();



        }

    }

}