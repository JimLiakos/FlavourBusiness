using FlavourBusinessFacade.HumanResources;
using FlavourBusinessFacade.RoomService;
using FlavourBusinessManager.ServicePointRunTime;
using FlavourBusinessManager.ServicesContextResources;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Remoting;
using System.Collections.Generic;
using System.Linq;

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
                var mealCourses = (from openSession in ServicesContextRunTime.OpenSessions
                                   where openSession.Meal != null
                                   from mealCource in openSession.Meal.Courses
                                   orderby mealCource.Meal.Session.ServicePoint.Description, (mealCource as MealCourse).MealCourseTypeOrder//.Courses.IndexOf(mealCource)
                                   select mealCource).ToList();
                //you have to  filter mealcourses by state. 



                //(from foodServiceSession in ServicesContextRunTime.OpenSessions
                // from ss in foodServiceSession.PartialClientSessions
                return mealCourses;
            }
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

        /// <MetaDataID>{b6588e83-9deb-4b52-b259-b8aa89fbdc4c}</MetaDataID>
        public void AutoMealParticipation(EndUsers.FoodServiceClientSession referencClientSession)
        {
            FoodServiceSession foodServiceSession = referencClientSession.ServicePoint.ActiveFoodServiceClientSessions.OfType<FoodServiceSession>().OrderBy(x => x.SessionStarts).Last();


            if (foodServiceSession != null && foodServiceSession.Progresses < ServicePointRunTime.ServicesContextRunTime.Current.Settings.AutoAssignMaxMealProgress)
                foodServiceSession = null;

            if (foodServiceSession == null)
                foodServiceSession = referencClientSession.ServicePoint.NewFoodServiceSession() as FoodServiceSession;

            foodServiceSession.AddPartialSession(referencClientSession);
            //referencClientSession.
            if (referencClientSession.Menu != null)
                foodServiceSession.MenuStorageIdentity = referencClientSession.Menu.StorageIdentity;

            ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(foodServiceSession);
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



    }


}