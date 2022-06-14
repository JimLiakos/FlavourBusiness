using FlavourBusinessFacade.RoomService;
using FlavourBusinessManager.ServicesContextResources;
using OOAdvantech.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlavourBusinessManager.RoomService
{



    /// <MetaDataID>{16a0ba3c-c9f6-44a7-94cf-0598ffea5437}</MetaDataID>
    public class ActionContext
    {
        /// <summary>
        /// 
        /// </summary>
        internal Dictionary<PreparationStation, List<ItemsPreparationContext>> PreparationSections = new Dictionary<PreparationStation, List<ItemsPreparationContext>>();

        internal Dictionary<PreparationStation, List<ItemPreparation>> ItemsInPreparation = new Dictionary<PreparationStation, List<ItemPreparation>>();


        public DateTime GetPreparationStartsAt(ItemPreparation itemPreparation)
        {
            DateTime dateTime;
            if (ItemPreparationsStartsAt.TryGetValue(itemPreparation, out dateTime))
                return dateTime;

            return DateTime.UtcNow;
        }

        public void SetPreparationStartsAt(ItemPreparation itemPreparation, DateTime dateTime)
        {
            if (ItemPreparationsStartsAt.ContainsKey(itemPreparation) && ItemPreparationsStartsAt[itemPreparation] != dateTime)
            {

            }

            ItemPreparationsStartsAt[itemPreparation] = dateTime;

        }

        public DateTime GetPreparationEndsAt(ItemPreparation itemPreparation)
        {

            DateTime dateTime;
            if (ItemPreparationsStartsAt.TryGetValue(itemPreparation, out dateTime))
                return dateTime + TimeSpanEx.FromMinutes((itemPreparation.PreparationStation as PreparationStation).GetPreparationTimeInMin(itemPreparation));

            return DateTime.UtcNow + TimeSpanEx.FromMinutes((itemPreparation.PreparationStation as PreparationStation).GetPreparationTimeInMin(itemPreparation));
        }

        public DateTime GetLastPlanPreparationEndsAt(ItemPreparation itemPreparation)
        {
            DateTime dateTime;
            if (LastPlanItemPreparationsStartsAt.TryGetValue(itemPreparation, out dateTime))
                return dateTime + TimeSpanEx.FromMinutes((itemPreparation.PreparationStation as PreparationStation).GetPreparationTimeInMin(itemPreparation));

            return DateTime.MinValue;
        }


        internal Dictionary<ItemPreparation, DateTime> ItemPreparationsStartsAt = new Dictionary<ItemPreparation, DateTime>();

        internal Dictionary<ItemPreparation, DateTime> LastPlanItemPreparationsStartsAt;

        /// <summary>
        /// 
        /// </summary>
        public bool PreparationPlanIsDoubleChecked { get; internal set; }

        internal void RemoveOutOfPlamPreparationItems()
        {
            foreach (var itemOutOfPlan in ItemPreparationsStartsAt.Keys.Where(x => x.State.IsInFollowingState(ItemPreparationState.ΙnPreparation)).ToList())
                ItemPreparationsStartsAt.Remove(itemOutOfPlan);

            foreach (var outOfPlanItemEntry in (from itemsInPreparationEntry in ItemsInPreparation
                                                from itemInPreparation in itemsInPreparationEntry.Value
                                                where itemInPreparation.State.IsInFollowingState(ItemPreparationState.ΙnPreparation)
                                                select new
                                                {
                                                    PreparationStation = itemsInPreparationEntry.Key,
                                                    itemInPreparation
                                                }).ToList())
            {
                ItemsInPreparation[outOfPlanItemEntry.PreparationStation].Remove(outOfPlanItemEntry.itemInPreparation);
                if (ItemsInPreparation[outOfPlanItemEntry.PreparationStation].Count == 0)
                    ItemsInPreparation.Remove(outOfPlanItemEntry.PreparationStation);

            }


        }
    }


    /// <MetaDataID>{5384aed7-9476-46a3-9f4d-59644106a8c5}</MetaDataID>
    static class TimeSpanEx
    {
        public static TimeSpan FromMinutes(double value)
        {
            //return TimeSpan.FromSeconds(value);
            return TimeSpan.FromMinutes(value);
        }
        public static double GetTotalMinutes(this TimeSpan timeSpan)
        {
            //return timeSpan.TotalSeconds;
            return timeSpan.TotalMinutes;
        }
    }

    /// <MetaDataID>{8b5ea92a-eb91-4c86-9941-8f646b46e1ac}</MetaDataID>
    public static class PreparationPlanExMethods
    {

        public static DateTime GetPreparationForecast(this IMealCourse mealCourse, ActionContext context)
        {
            var pendingPreparationSessions = mealCourse.FoodItemsInProgress.Where(x => x.GetPreparationStation() != null).ToList();

            if (pendingPreparationSessions.Count != 0)
                return pendingPreparationSessions.OrderBy(x => x.GetPreparationForecast(context)).Last().GetPreparationForecast(context);
            else
            {
                if (pendingPreparationSessions.Count == 0)
                    return DateTime.Now;
                return pendingPreparationSessions.OrderBy(x => x.GetPreparationForecast(context)).Last().GetPreparationForecast(context);
            }
        }

        public static DateTime GetLastPlanPreparationForecast(this IMealCourse mealCourse, ActionContext context)
        {
            var pendingPreparationSessions = mealCourse.FoodItemsInProgress.Where(x => x.GetPreparationStation() != null).ToList();

            if (pendingPreparationSessions.Count != 0)
                return pendingPreparationSessions.OrderBy(x => x.GetLastPlanPreparationForecast(context)).Last().GetLastPlanPreparationForecast(context);
            else
                return DateTime.Now;
            
        }
        public static ItemsPreparationContext GetReferencePreparationSection(this IMealCourse mealCourse, ActionContext context)
        {
            var pendingPreparationSessions = mealCourse.FoodItemsInProgress.Where(x => x.GetPreparationStation() != null).ToList();

            return pendingPreparationSessions.OrderBy(x => x.GetPreparationForecast(context)).LastOrDefault();
        }

        public static ItemsPreparationContext GetLastPlanReferencePreparationSection(this IMealCourse mealCourse, ActionContext context)
        {
            var pendingPreparationSessions = mealCourse.FoodItemsInProgress.Where(x => x.GetPreparationStation() != null).ToList();

            return pendingPreparationSessions.OrderBy(x => x.GetLastPlanPreparationForecast(context)).LastOrDefault();
        }
        public static DateTime GetPreparationForecast(this ItemsPreparationContext preparationSection, ActionContext actionContext)
        {
            if (actionContext.PreparationSections.ContainsKey(preparationSection.GetPreparationStation()))
                return actionContext.GetPreparationEndsAt(preparationSection.PreparationItems.OfType<ItemPreparation>().OrderBy(x => actionContext.GetPreparationEndsAt(x)).Last());
            else
                return preparationSection.PreparedAtForecast;
        }

        public static DateTime GetLastPlanPreparationForecast(this ItemsPreparationContext preparationSection, ActionContext actionContext)
        {
            if (actionContext.PreparationSections.ContainsKey(preparationSection.GetPreparationStation()))
                return actionContext.GetLastPlanPreparationEndsAt(preparationSection.PreparationItems.OfType<ItemPreparation>().OrderBy(x => actionContext.GetLastPlanPreparationEndsAt(x)).Last());
            else
                return preparationSection.PreparedAtForecast;
        }

        public static DateTime GetPreparationStartsAt(this ItemsPreparationContext preparationSection, ActionContext actionContext)
        {
            if (actionContext.PreparationSections.ContainsKey(preparationSection.GetPreparationStation()))
                return actionContext.GetPreparationStartsAt(preparationSection.PreparationItems.OfType<ItemPreparation>().OrderBy(x => actionContext.GetPreparationEndsAt(x)).Last());
            else
                return DateTime.UtcNow;
        }

        static PreparationStation GetPreparationStation(this ItemsPreparationContext preparationSection)
        {
            return preparationSection.PreparationItems[0].PreparationStation as PreparationStation;
        }

        public static List<ItemPreparation> GetLastPredictionItemsInPreparation(this PreparationStation preparationStation, ActionContext actionContext)
        {
            List<ItemPreparation> itemsInPreparation = null;
            actionContext.ItemsInPreparation.TryGetValue(preparationStation, out itemsInPreparation);
            return itemsInPreparation;
        }
        public static void SetItemsInPreparation(this PreparationStation preparationStation, ActionContext actionContext, List<ItemPreparation> itemsInPreparation)
        {
            actionContext.ItemsInPreparation[preparationStation] = itemsInPreparation;
        }

        public static void GetPredictions(this PreparationStation preparationStation, ActionContext actionContext)
        {


            if (actionContext.PreparationSections.ContainsKey(preparationStation))
            {


                List<ItemsPreparationContext> preparationSections = null;

                preparationSections = preparationStation.PreparationSessions.ToList();

                var preparationStationItems = (from serviceSession in preparationSections
                                               from preparationItem in serviceSession.PreparationItems
                                               select preparationItem).ToList();



                DateTime? preparationPlanStartTime = preparationStation.PreparationPlanStartTime;

                var itemsToPrepare = (from thePartialAction in actionContext.PreparationSections[preparationStation]
                                      where thePartialAction.GetPreparationStation() == preparationStation
                                      from slot in thePartialAction.GetItemsToPrepare()
                                      select slot).ToList();

                var items = preparationStationItems.Where(x => !itemsToPrepare.Contains(x)).ToList();
                if (items.Count > 0)
                {

                }

                if (preparationPlanStartTime == null)
                {
                    preparationPlanStartTime = DateTime.UtcNow;
                    preparationStation.PreparationPlanStartTime = preparationPlanStartTime;
                }
                else
                    preparationPlanStartTime = preparationStation.PreparationPlanStartTime;


                DateTime previousePreparationEndsAt = preparationPlanStartTime.Value;
                var lastPredictionItemsInPreparation = preparationStation.GetLastPredictionItemsInPreparation(actionContext);
                List<ItemPreparation> itemsInPreparation = itemsToPrepare.Where(x => x.State == ItemPreparationState.ΙnPreparation).OrderBy(x => x.PreparationStartsAt).ToList();

                if (lastPredictionItemsInPreparation == null)
                {
                    preparationStation.SetItemsInPreparation(actionContext, itemsInPreparation);
                    lastPredictionItemsInPreparation = itemsInPreparation;
                }
                else
                {
                    #region Remove the items in preparation to recalculate preparation time
                    var removedItem = lastPredictionItemsInPreparation.Where(x => !itemsInPreparation.Contains(x)).FirstOrDefault();
                    if (removedItem != null && actionContext.ItemPreparationsStartsAt.ContainsKey(removedItem) && actionContext.ItemPreparationsStartsAt[removedItem] > DateTime.UtcNow)
                    {
                        for (int i = lastPredictionItemsInPreparation.IndexOf(removedItem); i < lastPredictionItemsInPreparation.Count; i++)
                        {
                            if (actionContext.ItemPreparationsStartsAt.ContainsKey(lastPredictionItemsInPreparation[i]))
                                actionContext.ItemPreparationsStartsAt.Remove(lastPredictionItemsInPreparation[i]);
                        }
                    }
                    #endregion
                }
                foreach (var itemInPreparation in itemsInPreparation.Where(x => lastPredictionItemsInPreparation.Contains(x) && actionContext.ItemPreparationsStartsAt.ContainsKey(x)))
                {
                    if (actionContext.GetPreparationEndsAt(itemInPreparation) > previousePreparationEndsAt)
                        previousePreparationEndsAt = actionContext.GetPreparationEndsAt(itemInPreparation);
                }
                foreach (var itemInPreparation in itemsInPreparation.Where(x => !lastPredictionItemsInPreparation.Contains(x) || !actionContext.ItemPreparationsStartsAt.ContainsKey(x)))
                {

                    if (previousePreparationEndsAt > DateTime.UtcNow)
                        actionContext.SetPreparationStartsAt(itemInPreparation, previousePreparationEndsAt);
                    else
                        actionContext.SetPreparationStartsAt(itemInPreparation, DateTime.UtcNow);

                    previousePreparationEndsAt = actionContext.GetPreparationEndsAt(itemInPreparation);
                }
                preparationStation.SetItemsInPreparation(actionContext, itemsInPreparation);


                var itemsPendingToPrepare = itemsToPrepare.Where(x => x.State == ItemPreparationState.PreparationDelay || x.State == ItemPreparationState.PendingPreparation).ToList();

                ItemsPreparationContext partialAction = null;
                double packingTime = 0;
                foreach (var itemToPrepare in itemsPendingToPrepare)
                {
                    if (itemToPrepare.FindItemsPreparationContext() != partialAction)
                    {
                        if (packingTime != 0)
                            previousePreparationEndsAt = previousePreparationEndsAt + TimeSpanEx.FromMinutes(packingTime);
                        packingTime = 0;
                        partialAction = itemToPrepare.FindItemsPreparationContext();
                    }

                    //packingTime += itemToPrepare.PackingTime;

                    if (actionContext.GetPreparationStartsAt(itemToPrepare) == null || actionContext.GetPreparationStartsAt(itemToPrepare) != previousePreparationEndsAt)
                    {
                        var oldpreviousePreparationEndsAt = actionContext.GetPreparationStartsAt(itemToPrepare);
                        actionContext.PreparationPlanIsDoubleChecked = false;
                    }
                    actionContext.SetPreparationStartsAt(itemToPrepare, previousePreparationEndsAt);
                    previousePreparationEndsAt = previousePreparationEndsAt + TimeSpanEx.FromMinutes(preparationStation.GetPreparationTimeInMin(itemToPrepare));
                }

                //var strings = preparationStation.GetActionsToStrings(actionContext);
            }

            //if (preparationStation.GetActionsToDo(actionContext).Count == 0)
            //    preparationStation.PreparationPlanStartTime = DateTime.UtcNow;
        }

        internal static List<string> GetActionsToStrings(this PreparationStation preparationStation, ActionContext actionContext)
        {
            if (!actionContext.PreparationSections.ContainsKey(preparationStation))
                return (from preparationSection in preparationStation.PreparationSessions
                        orderby preparationSection.MealCourseStartsAt
                        select preparationSection.ToString()).ToList();

            List<string> strings =
            (from preparationSection in actionContext.PreparationSections[preparationStation]
             orderby preparationSection.GetPreparationStartsAt(actionContext)
             select preparationSection.TotString(actionContext)).ToList();

            return strings;
        }
        static string TotString(this ItemsPreparationContext preparationSection, ActionContext actionContext)
        {
            var preparationForecast = preparationSection.GetPreparationForecast(actionContext);

            var preparationStartsAt = preparationSection.GetPreparationStartsAt(actionContext);

            var standby = !preparationSection.PreparationItems.Any(x => x.State.IsIntheSameOrFollowingState(ItemPreparationState.PendingPreparation));

            if (standby)
                return "X " + preparationSection.MealCourse.Meal.Session.ServicePoint.Description + " " + preparationSection.MealCourseStartsAt?.Day.ToString() + " " + preparationSection.MealCourseStartsAt?.ToShortTimeString() + " " + preparationSection.Description + " " + preparationSection.MealCourse.Name + " " + string.Format("{0:h:mm:ss tt}", preparationStartsAt) + " " + " " + string.Format("{0:h:mm:ss tt}", preparationForecast) + " " + string.Format("{0:h:mm:ss tt}", preparationSection.MealCourse.GetPreparationForecast(actionContext)) + " itemsToPrepare : " + preparationSection.PreparationItems.Count.ToString() + " v:" + preparationSection.GetPreparationStation()?.PreparationVelocity;
            else
                return preparationSection.MealCourse.Meal.Session.ServicePoint.Description + " " + preparationSection.MealCourseStartsAt?.Day.ToString() + " " + preparationSection.MealCourseStartsAt?.ToShortTimeString() + " " + preparationSection.Description + " " + preparationSection.MealCourse.Name + " " + string.Format("{0:h:mm:ss tt}", preparationStartsAt) + " " + " " + string.Format("{0:h:mm:ss tt}", preparationForecast) + " " + string.Format("{0:h:mm:ss tt}", preparationSection.MealCourse.GetPreparationForecast(actionContext)) + " itemsToPrepare : " + preparationSection.PreparationItems.Count.ToString() + " v:" + preparationSection.GetPreparationStation()?.PreparationVelocity;




        }
        internal static List<ItemPreparation> GetItemsToPrepare(this ItemsPreparationContext preparationSection)
        {
            var itemsToPrepare = (from preparationItem in preparationSection.PreparationItems
                                  where preparationItem.State.IsIntheSameOrFollowingState(ItemPreparationState.PreparationDelay) &&
                                  preparationItem.State.IsInPreviousState(ItemPreparationState.IsRoasting)
                                  select preparationItem).OfType<ItemPreparation>().ToList();

            return itemsToPrepare;
        }

        public static void ActionsOrderCommited(this PreparationStation preparationStation, ActionContext actionContext)
        {

            bool thereArenotPendingPreparationItems = false;
            if (actionContext.PreparationSections.ContainsKey(preparationStation))
            {
                List<ItemPreparation> itemsToPrepare = (from thePartialAction in actionContext.PreparationSections[preparationStation]
                                                        from itemPreparation in thePartialAction.GetItemsToPrepare()
                                                        select itemPreparation).ToList();
                thereArenotPendingPreparationItems = itemsToPrepare.All(x => x.State.IsInPreviousState(ItemPreparationState.PendingPreparation));
            }

            bool thereArePendingPreparationItems = false;
            string description = preparationStation.Description;
            string delayAction = actionContext.PreparationSections[preparationStation].Where(x => x.PreparationState == ItemPreparationState.PreparationDelay).FirstOrDefault()?.TotString(actionContext);
            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                foreach (var partialAction in preparationStation.GetActionsToDo(actionContext))
                {
                    partialAction.PreparationOrderCommited = true;

                    foreach (var preparationItem in partialAction.PreparationItems.OfType<ItemPreparation>())
                    {
                        if (preparationItem.State == FlavourBusinessFacade.RoomService.ItemPreparationState.PreparationDelay)
                        {
                            preparationItem.State = FlavourBusinessFacade.RoomService.ItemPreparationState.PendingPreparation;
                            preparationItem.StateTimestamp = DateTime.UtcNow;//If stateTimestamp has change the client side itemPreparation realize the this change   
                            preparationItem.PreparedAtForecast = actionContext.GetPreparationEndsAt(preparationItem as ItemPreparation);
                            thereArePendingPreparationItems = true;
                        }
                    }
                }
                if (thereArenotPendingPreparationItems && thereArePendingPreparationItems)
                {

                    preparationStation.OnPreparationItemsChangeState();

                }
                stateTransition.Consistent = true;
            }


        }
        public static List<ItemsPreparationContext> GetActionsToDo(this PreparationStation preparationStation, ActionContext actionContext)
        {

            List<ItemsPreparationContext> preparationSections = new List<ItemsPreparationContext>();//.Where(x=>(x.Action.PreparationForecast-TimeSpanEx.FromMinutes(x.Duration+5))<DateTime.UtcNow).ToList();
            if (actionContext.PreparationSections.ContainsKey(preparationStation))
                preparationSections = actionContext.PreparationSections[preparationStation];
            List<ItemsPreparationContext> filteredPartialActions = new List<ItemsPreparationContext>();

            foreach (var preparationSection in preparationSections)
            {
                if ((preparationSection.MealCourse.GetPreparationForecast(actionContext) - preparationSection.GetPreparationForecast(actionContext)).GetTotalMinutes() < 1.5 * Simulator.Velocity || ((preparationSection.MealCourse.GetPreparationForecast(actionContext) - TimeSpanEx.FromMinutes(preparationSection.GetDuration() + (2 * Simulator.Velocity))) < DateTime.UtcNow))
                {
                    if (preparationSection.MealCourse.Name == "a11")
                    {
                    }
                    if ((preparationSection.MealCourse.GetPreparationForecast(actionContext) - preparationSection.GetPreparationForecast(actionContext)).GetTotalMinutes() < 1.5 * Simulator.Velocity)
                    {
                    }

                    filteredPartialActions.Add(preparationSection);
                }
                else if (preparationSection.PreparationItems.Any(x => x.State > FlavourBusinessFacade.RoomService.ItemPreparationState.ΙnPreparation))
                {

                    filteredPartialActions.Add(preparationSection);
                }
                else
                {
                    if (preparationSection.PreparationItems.All(x => x.State > FlavourBusinessFacade.RoomService.ItemPreparationState.PreparationDelay))
                        filteredPartialActions.Add(preparationSection);
                    else
                    {
                        if (preparationSection.PreparationOrderCommited)
                        {

                        }
                    }

                    break;
                }
            }




            return filteredPartialActions;
        }

        internal static double GetDuration(this ItemsPreparationContext itemsPreparationContext)
        {
            return itemsPreparationContext.PreparationItems.OfType<ItemPreparation>().Sum(x => (x.PreparationStation as PreparationStation).GetPreparationTimeInMin(x));
        }

        internal static void OptimizePreparationPlan(this PreparationStation preparationStation, ActionContext actionContext, bool stirTheSequence)
        {


            List<ItemsPreparationContext> PreparationSessionsForOptimazation = null;
            var preparationSections = preparationStation.PreparationSessions.ToList();
            if (stirTheSequence)
            {
                // first takes the uncommitted  items preparation contexts where the meal course has all items preparation contexts uncommitted 
                PreparationSessionsForOptimazation = preparationSections.Where(x => !x.PreparationOrderCommited).OrderBy(x => (x.MealCourse as MealCourse).GetPreparationForecast(actionContext)).Where(x => x.MealCourse.FoodItemsInProgress.All(y => !y.PreparationOrderCommited)).ToList();

                var a_count = PreparationSessionsForOptimazation.Count;

                //in the sequel takes the uncommitted  items preparation contexts where the meal course has at least one items preparation contexts committed 
                PreparationSessionsForOptimazation.AddRange(preparationSections.Where(x => !x.PreparationOrderCommited).OrderBy(x => x.MealCourse.GetPreparationForecast(actionContext)).Where(x => x.MealCourse.FoodItemsInProgress.Any(y => y.PreparationOrderCommited)).ToList());

                //preparation contexts order by the preparation forecast time of meal course where this belongs   
                var b_count = PreparationSessionsForOptimazation.Count;
                if (a_count != b_count)
                {

                }
            }
            else
            {
                //preparation contexts order by the preparation forecast time of meal course where this belongs
                PreparationSessionsForOptimazation = preparationSections.Where(x => !x.PreparationOrderCommited).OrderBy(x => x.MealCourse.GetPreparationForecast(actionContext)).ToList();
                
                if(PreparationSessionsForOptimazation.Count>1)
                {

                }

                if (actionContext.LastPlanItemPreparationsStartsAt!= null)
                {
                    foreach (var preparationSection in PreparationSessionsForOptimazation.ToList())
                    {
                        if (preparationSection.MealCourse.GetLastPlanReferencePreparationSection(actionContext) != preparationSection.MealCourse.GetReferencePreparationSection(actionContext) && // the reference preparation section of meal course has change
                            preparationSection.MealCourse.GetLastPlanReferencePreparationSection(actionContext).PreparationOrderCommited) // there is reference committed preparation section
                        {
                            if (preparationSection.MealCourse.GetLastPlanPreparationForecast(actionContext) < preparationSection.MealCourse.GetPreparationForecast(actionContext))// the meal course preparation forecast is worse from the last plan
                            {
                                if (PreparationSessionsForOptimazation.Count > 1)
                                {
                                    int oneUpPos = PreparationSessionsForOptimazation.IndexOf(preparationSection) - 1; //moves preparation section one position up 
                                    if (oneUpPos >= 0)
                                    {
                                        PreparationSessionsForOptimazation.Remove(preparationSection);
                                        PreparationSessionsForOptimazation.Insert(oneUpPos, preparationSection);
                                        preparationSection.PreparationOrderCommited = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            
            //preparation order committed PreparationSessions
            List<ItemsPreparationContext> orderCommittedreparationContexts = preparationSections.Where(x => x.PreparationOrderCommited).OrderBy(x => x.PreparatioOrder).ToList();

            //Adds the re planed preparation contexts 
            orderCommittedreparationContexts.AddRange(PreparationSessionsForOptimazation);
            List<ItemsPreparationContext> actions = orderCommittedreparationContexts;

            //var preparationStationItems = (from serviceSession in preparationSections
            //                               from preparationItem in serviceSession.PreparationItems
            //                               select preparationItem).ToList();

            //List<PartialAction> actions = Actions.OrderBy(x => x.MainAction.GetPreparationForecast(actionContext)).ToList();

            int i = 0;

            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                foreach (var partialAction in actions)
                    partialAction.PreparatioOrder = i++;

                stateTransition.Consistent = true;
            }

            var previous = preparationStation.PreparationSessions;
            if (actionContext.PreparationSections.ContainsKey(preparationStation))
                previous = actionContext.PreparationSections[preparationStation];

            actionContext.PreparationSections[preparationStation] = actions;
            if(preparationSections.Count!=actions.Count)
            {

            }

            var actionHash = actions.Select(x => x.GetHashCode()).ToArray();
            var m_actionsHash = previous.Select(x => x.GetHashCode()).ToArray();
            for (i = 0; i < actions.Count; i++)
            {
                if (actionHash.Length == m_actionsHash.Length)
                {
                    if (actionHash[i] != m_actionsHash[i])
                    {

                    }
                }
            }

        }

    }

    /// <MetaDataID>{17f3f120-eb70-4c0e-ba73-7b5f7715633a}</MetaDataID>
    public class Simulator
    {
        public static double Velocity = 0.33;
    }
}
