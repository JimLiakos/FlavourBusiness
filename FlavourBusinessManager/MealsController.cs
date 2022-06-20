using FlavourBusinessFacade.HumanResources;
using FlavourBusinessFacade.RoomService;
using FlavourBusinessFacade.ServicesContextResources;
using FlavourBusinessManager.ServicePointRunTime;
using FlavourBusinessManager.ServicesContextResources;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Remoting;
using OOAdvantech.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlavourBusinessManager.RoomService
{
    /// <MetaDataID>{c31eb292-1b39-4081-9516-0e0e9e97b10a}</MetaDataID>
    public class MealsController : System.MarshalByRefObject, IExtMarshalByRefObject, IMealsController
    {
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

                return mealCourses;
            }
        }


        /// <MetaDataID>{db77bf43-40ea-48ca-830f-f713e88938aa}</MetaDataID>
        object buildPreparationPlanLock = new object();


        /// <MetaDataID>{dfd3500e-b5f0-47da-b3b8-5c161f224ec3}</MetaDataID>
        private void RebuildPreparationPlan(ActionContext actionContext)
        {

            //return;
            //actionContext.PreparationStationspreParationPlanStartTime.Clear();
            lock (buildPreparationPlanLock)
            {
                try
                {
                    actionContext.RemoveOutOfPlanPreparationItems();
                    DateTime timeStamp = DateTime.UtcNow;
                    actionContext.PreparationPlanIsDoubleChecked = false;
                    bool stirTheSequence = true;
                    
                    foreach (var preparationStation in ActivePreparationStations)
                        preparationStation.GetPreparationSections(actionContext);

                    while (!actionContext.PreparationPlanIsDoubleChecked)
                    {
                        actionContext.PreparationPlanIsDoubleChecked = true;

                        foreach (var preparationStation in ActivePreparationStations)
                        {
                            preparationStation.OptimizePreparationPlan(actionContext, stirTheSequence);
                            preparationStation.GetPredictions(actionContext, stirTheSequence);
                        }

                        TimeSpan timeSpan = (DateTime.UtcNow - timeStamp);
                        if (timeSpan.TotalSeconds > 2)
                            ComputationalResources.LogMessage.WriteLog("Load sessions time span : " + timeSpan.TotalSeconds.ToString());
                        timeStamp = DateTime.UtcNow;

                        stirTheSequence = false;
                    }
                    actionContext.LastPlanItemPreparationsStartsAt = new Dictionary<ItemPreparation, DateTime>(actionContext.ItemPreparationsStartsAt);
                    actionContext.PositionInterchanges.Clear();


                   

                    foreach (var preparationStation in ActivePreparationStations)
                    {
                        preparationStation.ActionsOrderCommited(actionContext);

                        #region clear PreparationPlanStartTime in case where there are not items in state pendings to prepare 

                        if (actionContext.PreparationSections.ContainsKey(preparationStation))
                        {
                            DateTime? preparationPlanStartTime = preparationStation.PreparationPlanStartTime;

                            List<ItemPreparation> itemsToPrepare = (from thePartialAction in actionContext.PreparationSections[preparationStation]
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
                        var strings = preparationStation.GetActionsToStrings(actionContext);
                        if(strings.Count>1)
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
                            where foodItem.PreparationStation != null
                            select foodItem.PreparationStation).OfType<PreparationStation>().Distinct().ToList();
                }
                finally
                {
                    TimeSpan timeSpan2 = DateTime.UtcNow - timeStamp;

                }
            }
        }

        /// <MetaDataID>{a42810ef-fbbe-49d5-8733-04986c3d7030}</MetaDataID>
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

        public event NewMealCoursesInrogressHandel NewMealCoursesInrogress;
        public event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;

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
        internal void OnNewMealCoursesInrogress(List<IMealCourse> mealCourses)
        {
            NewMealCoursesInrogress?.Invoke(mealCourses);

            (ServicesContextRunTime.MealsController as MealsController).ReadyToServeMealcoursesCheck(mealCourses);
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
        /// <param name="referencClientSession">
        /// Defines a partial session as reference to identify service point meal
        /// </param>
        /// <MetaDataID>{b6588e83-9deb-4b52-b259-b8aa89fbdc4c}</MetaDataID>
        public void AutoMealParticipation(EndUsers.FoodServiceClientSession referencClientSession)
        {
            if (referencClientSession.MainSession != null)
                return;
            FoodServiceSession foodServiceSession = referencClientSession.ServicePoint.ActiveFoodServiceClientSessions.Where(x => x.MainSession != null && (x.MainSession as FoodServiceSession).CanIncludeAsPart(referencClientSession)).Select(x => x.MainSession).OfType<FoodServiceSession>().OrderBy(x => x.SessionStarts).LastOrDefault();

            if (foodServiceSession == null)
            {

                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {
                    foodServiceSession = referencClientSession.ServicePoint.NewFoodServiceSession() as FoodServiceSession;
                    ObjectStorage.GetStorageOfObject(ServicesContextRunTime.Current).CommitTransientObjectState(foodServiceSession);
                    if (referencClientSession.Menu != null)
                        foodServiceSession.MenuStorageIdentity = referencClientSession.Menu.StorageIdentity;


                    foodServiceSession.AddPartialSession(referencClientSession);
                    referencClientSession.ImplicitMealParticipation = true;


                    //Add all individual active sessions to service point session
                    foreach (var unAssignedClientSession in (referencClientSession.ServicePoint as ServicePoint).OpenClientSessions.OfType<EndUsers.FoodServiceClientSession>().Where(x => x.MainSession == null && !x.Forgotten))
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
                    foodServiceSession.AddPartialSession(referencClientSession);
                    referencClientSession.ImplicitMealParticipation = true;
                    stateTransition.Consistent = true;
                }
            }

            //referencClientSession.


        }



        /// <MetaDataID>{cd97524c-d96b-4469-8ad1-dbc709729bb4}</MetaDataID>
        Dictionary<IMealCourse, ServingBatch> UnassignedMealCourses = new Dictionary<IMealCourse, ServingBatch>();

        /// <MetaDataID>{e7cf8250-6b70-4095-bf94-bf087a79d362}</MetaDataID>
        internal IList<ServingBatch> GetServingBatches(HumanResources.Waiter waiter)
        {

            var activeShiftWork = ServicesContextRunTime.Current.GetActiveShiftWorks();

            List<ServingBatch> servingBatches = new List<ServingBatch>();
            if (waiter.ActiveShiftWork != null)
            {

                var sdsd = (from mealCourse in MealCoursesInProgress
                            from itemsPreparationContext in mealCourse.FoodItemsInProgress
                            from itemPreparation in itemsPreparationContext.PreparationItems
                            select new { itemPreparation.Name, itemPreparation.State, itemPreparation }).ToList();

                var mealCoursesToServe = (from mealCourse in MealCoursesInProgress
                                          from itemsPreparationContext in mealCourse.FoodItemsInProgress
                                          from itemPreparation in itemsPreparationContext.PreparationItems
                                          where (itemPreparation.State == ItemPreparationState.Serving || itemPreparation.State == ItemPreparationState.OnRoad) &&
                                          (mealCourse.Meal.Session.ServicePoint as ServicePoint).IsAssignedTo(waiter, waiter.ActiveShiftWork)
                                          select mealCourse).Distinct().ToList();

                foreach (var mealCourse in mealCoursesToServe)
                {


                    IList<ItemsPreparationContext> preparedItems = (from itemsPreparationContext in mealCourse.FoodItemsInProgress
                                                                    where itemsPreparationContext.PreparationItems.All(x => (x.State == ItemPreparationState.Serving || x.State == ItemPreparationState.OnRoad))
                                                                    select itemsPreparationContext).ToList();

                    IList<ItemsPreparationContext> underPreparationItems = (from itemsPreparationContext in mealCourse.FoodItemsInProgress
                                                                            where itemsPreparationContext.PreparationItems.Any(x => x.State == ItemPreparationState.PendingPreparation ||
                                                                            x.State == ItemPreparationState.ÉnPreparation ||
                                                                            x.State == ItemPreparationState.IsRoasting ||
                                                                            x.State == ItemPreparationState.IsPrepared)
                                                                            select itemsPreparationContext).ToList();

                    var mealCourseUri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(mealCourse)?.GetPersistentObjectUri(mealCourse);

                    var serviceBatch = (from itemsPreparationContext in preparedItems
                                        from itemPreparation in itemsPreparationContext.PreparationItems
                                        where itemPreparation.ServedInTheBatch != null && itemPreparation.ServedInTheBatch.MealCourse == mealCourse
                                        select itemPreparation.ServedInTheBatch).OfType<ServingBatch>().FirstOrDefault();
                    if (serviceBatch != null)
                    {

                        if (serviceBatch.ShiftWork?.Worker == waiter)
                        {
                            serviceBatch.Update(mealCourse, preparedItems, underPreparationItems);
                            servingBatches.Add(serviceBatch);
                        }
                    }
                    else
                    {
                        UnassignedMealCourses.TryGetValue(mealCourse, out serviceBatch);
                        if (serviceBatch == null)
                            servingBatches.Add(new ServingBatch(mealCourse, preparedItems, underPreparationItems));
                        else
                        {
                            serviceBatch.Update(mealCourse, preparedItems, underPreparationItems);
                            servingBatches.Add(serviceBatch);
                        }
                    }
                }
            }

            int i = 0;
            foreach (var servingBatch in servingBatches)
                servingBatch.SortID = i++;

            return servingBatches;
        }

        /// <MetaDataID>{cad5c370-7874-46bf-a785-93efc6d68ed7}</MetaDataID>
        internal void ServingBatchDeassigned(HumanResources.Waiter waiter, IServingBatch servingBatch)
        {

            var servicePoint = ServicesContextRunTime.Current.OpenSessions.Select(x => x.ServicePoint).OfType<ServicePoint>().Where(x => x.ServicesPointIdentity == servingBatch.ServicesPointIdentity).FirstOrDefault();

            var activeWaiters = (from shiftWork in ServicesContextRunTime.Current.GetActiveShiftWorks()
                                 where shiftWork.Worker is IWaiter && servicePoint.IsAssignedTo(shiftWork.Worker as IWaiter, shiftWork) && shiftWork.Worker != waiter
                                 select shiftWork.Worker).OfType<HumanResources.Waiter>().ToList();

            foreach (var a_Waiter in activeWaiters)
                a_Waiter.FindServingBatchesChanged();
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
        ActionContext ActionContext = new ActionContext();

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


        /// <summary>
        /// This method finds all waiter which can serve the meal courses and update them for changes
        /// </summary>
        /// <param name="mealCourses">
        /// Defines the meal courses which are ready for serving
        /// </param>
        /// <param name="excludeWaiter">
        /// This parameter defines the waiter where exclude from waiters list
        /// </param>
        /// <MetaDataID>{17792be3-f73c-4e93-b382-6848fcef9521}</MetaDataID>
        internal void ReadyToServeMealcoursesCheck(List<IMealCourse> mealCourses, HumanResources.Waiter excludeWaiter = null)
        {


            var mealCoursesToServe = (from mealCourse in mealCourses
                                      from itemsPreparationContext in mealCourse.FoodItemsInProgress
                                      from itemPreparation in itemsPreparationContext.PreparationItems
                                      where (itemPreparation.State == ItemPreparationState.Serving || itemPreparation.State == ItemPreparationState.OnRoad)
                                      select mealCourse).Distinct().ToList();
            var mealCoursesServicesPoints = mealCoursesToServe.Select(x => x.Meal.Session.ServicePoint).OfType<ServicePoint>().ToList();
            List<HumanResources.Waiter> activeWaitersFormealCoursesServing = new List<HumanResources.Waiter>();

            foreach (var servicePoint in mealCoursesServicesPoints)
            {
                activeWaitersFormealCoursesServing.AddRange((from shiftWork in ServicesContextRunTime.Current.GetActiveShiftWorks()
                                                             where shiftWork.Worker is IWaiter && servicePoint.IsAssignedTo(shiftWork.Worker as IWaiter, shiftWork)
                                                             select shiftWork.Worker).OfType<HumanResources.Waiter>());
            }

            if (excludeWaiter != null && activeWaitersFormealCoursesServing.Contains(excludeWaiter))
                activeWaitersFormealCoursesServing.Remove(excludeWaiter);

            activeWaitersFormealCoursesServing = activeWaitersFormealCoursesServing.Distinct().ToList();

            foreach (var a_Waiter in activeWaitersFormealCoursesServing)
                a_Waiter.FindServingBatchesChanged();
        }

        /// <MetaDataID>{eb9a6c0c-3a5e-4115-a6e2-c1aaee2b0fb9}</MetaDataID>
        internal void ServingBatchAssigned(HumanResources.Waiter waiter, IServingBatch servingBatch)
        {

            var servicePoint = ServicesContextRunTime.Current.OpenSessions.Select(x => x.ServicePoint).OfType<ServicePoint>().Where(x => x.ServicesPointIdentity == servingBatch.ServicesPointIdentity).FirstOrDefault();

            var activeWaiters = (from shiftWork in ServicesContextRunTime.Current.GetActiveShiftWorks()
                                 where shiftWork.Worker is IWaiter && servicePoint.IsAssignedTo(shiftWork.Worker as IWaiter, shiftWork) && shiftWork.Worker != waiter
                                 select shiftWork.Worker).OfType<HumanResources.Waiter>().ToList();

            foreach (var a_Waiter in activeWaiters)
                a_Waiter.FindServingBatchesChanged();

        }

        /// <MetaDataID>{c79fa8ae-e4b6-452e-96d8-9f590b3e5e04}</MetaDataID>
        public void MoveCourseBefore(string mealCourseAsReferenceUri, string movedMealCourseUri)
        {

        }

        /// <MetaDataID>{1f3b96de-e184-4669-897b-17c69b3c260b}</MetaDataID>
        public void MoveCourseAfter(string mealCourseAsReferenceUri, string movedMealCourseUri)
        {

        }

        /// <MetaDataID>{3e5166da-a888-46e4-9778-4c4acfe31411}</MetaDataID>
        internal Dictionary<string, ItemPreparationPlan> GetItemToServingTimespanPredictions(List<ItemPreparation> preparationStationItems)
        {
            DateTime startTime = DateTime.UtcNow;

            RebuildPreparationPlan(ActionContext);
            Dictionary<string, ItemPreparationPlan> predictions = new Dictionary<string, ItemPreparationPlan>();

   
            foreach (var itemPreparation in preparationStationItems)
            {
                if (!ActionContext.ItemPreparationsStartsAt.ContainsKey(itemPreparation))
                {
                    ItemPreparationPlan itemPreparationPlan = new ItemPreparationPlan()
                    {
                        PreparationStart = DateTime.UtcNow,
                        Duration = 0
                    };

                    predictions[itemPreparation.uid] = itemPreparationPlan;

                }
                else
                {
                    ItemPreparationPlan itemPreparationPlan = new ItemPreparationPlan()
                    {
                        PreparationStart = ActionContext.ItemPreparationsStartsAt[itemPreparation],
                        Duration = TimeSpanEx.FromMinutes((itemPreparation.PreparationStation as PreparationStation).GetPreparationTimeInMin(itemPreparation)).TotalMinutes
                    };

                    predictions[itemPreparation.uid] = itemPreparationPlan;
                }
            }
            var timeSpan = DateTime.UtcNow - startTime;
            return predictions;
        }
    }

}