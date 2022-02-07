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

        public readonly ServicePointRunTime.ServicesContextRunTime ServicesContextRunTime;

        public event NewMealCoursesInrogressHandel NewMealCoursesInrogress;
        public event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;

        public MealsController(ServicePointRunTime.ServicesContextRunTime servicesContextRunTime)
        {
            ServicesContextRunTime = servicesContextRunTime;
        }
        ~MealsController()
        {
            System.Diagnostics.Debug.WriteLine("MealsController");
        }

        internal void OnNewMealCoursesInrogress(List<IMealCourse> mealCourses)
        {
            NewMealCoursesInrogress?.Invoke(mealCourses);

            (ServicesContextRunTime.MealsController as MealsController).ReadyToServeMealcoursesCheck(mealCourses);
            //you have to  filter mealcourses by state.
        }

        internal void OnRemoveMealCoursesInrogress(List<IMealCourse> mealCourses)
        {
            ObjectChangeState?.Invoke(this, nameof(MealCoursesInProgress));

            //you have to  filter mealcourses by state.
        }

        public void AutoMealParticipation(EndUsers.FoodServiceClientSession referencClientSession)
        {

        }



        Dictionary<IMealCourse, ServingBatch> UnassignedMealCourses = new Dictionary<IMealCourse, ServingBatch>();

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
        internal void ReadyToServeMealcoursesCheck(List<IMealCourse> mealCourses, HumanResources.Waiter excludeWaiter=null)
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