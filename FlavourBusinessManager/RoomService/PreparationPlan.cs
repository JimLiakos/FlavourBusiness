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


        internal List<PositionInterchange> PositionInterchanges = new List<PositionInterchange>();

        public DateTime GetPreparationStartsAt(ItemPreparation itemPreparation)
        {
            DateTime dateTime;
            if (ItemPreparationsStartsAt.TryGetValue(itemPreparation, out dateTime))
                return dateTime;

            return DateTime.UtcNow;
        }

        public void SetPreparationStartsAt(ItemPreparation itemPreparation, DateTime dateTime)
        {

            if (ItemPreparationsStartsAt.ContainsKey(itemPreparation) && Math.Abs((ItemPreparationsStartsAt[itemPreparation] - dateTime).TotalMinutes) > 1.2)
            {

            }

            ItemPreparationsStartsAt[itemPreparation] = dateTime;

            if ((GetPreparationEndsAt(itemPreparation) - DateTime.UtcNow).TotalSeconds < -20)
            {

            }

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

        internal void RemoveOutOfPlanPreparationItems()
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

                if (ItemsInPreparation[outOfPlanItemEntry.PreparationStation].IndexOf(outOfPlanItemEntry.itemInPreparation) == 0)//
                    outOfPlanItemEntry.PreparationStation.PreparationPlanStartTime = null;// the first item in list of ItemsInPreparation defines the preparation plan startTime  

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
            var pendingPreparationSessions = mealCourse.FoodItemsInProgress.Where(x => x.GetPreparationStation() != null && x.PreparationItems.
                    Any(y => y.State.IsIntheSameOrFollowingState(ItemPreparationState.PreparationDelay) && y.State.IsInTheSameOrPreviousState(ItemPreparationState.ΙnPreparation))).ToList();

            var allPreparationSessions = mealCourse.FoodItemsInProgress.Where(x => x.GetPreparationStation() != null).ToList();
            if (allPreparationSessions.Count != pendingPreparationSessions.Count)
            {

            }

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="preparationSection">
        /// Defines the preparation section where extension method referred 
        /// </param>
        /// <param name="actionContext">
        /// Defines the actions context
        /// </param>
        /// <returns>
        /// Returns the forecast time when all the products in the section will be prepared.
        /// In caSe where all items of preparation section are prepared return utc now 
        /// </returns>
        public static DateTime GetPreparationForecast(this ItemsPreparationContext preparationSection, ActionContext actionContext)
        {
            if (actionContext.PreparationSections.ContainsKey(preparationSection.GetPreparationStation()))
            {
                var lastItemToPrepare = preparationSection.PreparationItems.
                    Where(x => x.State.IsIntheSameOrFollowingState(ItemPreparationState.PreparationDelay) && x.State.IsInTheSameOrPreviousState(ItemPreparationState.ΙnPreparation)).
                    OfType<ItemPreparation>().OrderBy(x => actionContext.GetPreparationEndsAt(x)).LastOrDefault();

                if (lastItemToPrepare == null)//All items prepared
                    return DateTime.UtcNow;
                else
                    return actionContext.GetPreparationEndsAt(lastItemToPrepare);
            }
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

        public static void GetPredictions(this PreparationStation preparationStation, ActionContext actionContext, bool stirTheSequence)
        {

            if (actionContext.PreparationSections.ContainsKey(preparationStation))
            {

                DateTime? preparationPlanStartTime = preparationStation.PreparationPlanStartTime;

                var itemsToPrepare = (from thePartialAction in actionContext.PreparationSections[preparationStation]
                                      where thePartialAction.GetPreparationStation() == preparationStation
                                      from slot in thePartialAction.GetItemsToPrepare()
                                      select slot).ToList();

                #region Debug code 
                List<ItemsPreparationContext> preparationSections = preparationStation.PreparationSessions.ToList();
                var preparationStationItems = (from serviceSession in preparationSections
                                               from preparationItem in serviceSession.PreparationItems
                                               select preparationItem).ToList();
                var items = preparationStationItems.Where(x => !itemsToPrepare.Contains(x)).ToList();
                if (items.Count > 0)
                {

                }
                #endregion

                if (preparationPlanStartTime == null)
                {
                    preparationPlanStartTime = DateTime.UtcNow;
                    preparationStation.PreparationPlanStartTime = preparationPlanStartTime;
                }
                else
                    preparationPlanStartTime = preparationStation.PreparationPlanStartTime;




                #region Gets items in preparation predictions
                var lastPredictionItemsInPreparation = preparationStation.GetLastPredictionItemsInPreparation(actionContext);


                List<ItemPreparation> itemsInPreparation = itemsToPrepare.Where(x => x.State == ItemPreparationState.ΙnPreparation && actionContext.ItemPreparationsStartsAt.ContainsKey(x)).OrderBy(x => actionContext.GetPreparationStartsAt(x)).ToList();
                itemsInPreparation.AddRange(itemsToPrepare.Where(x => x.State == ItemPreparationState.ΙnPreparation && !actionContext.ItemPreparationsStartsAt.ContainsKey(x)));

                if (itemsInPreparation.Count == 1 && (lastPredictionItemsInPreparation == null || !lastPredictionItemsInPreparation.Contains(itemsInPreparation[0])))
                {
                    preparationPlanStartTime = DateTime.UtcNow;
                    preparationStation.PreparationPlanStartTime = preparationPlanStartTime;
                }

                DateTime previousePreparationEndsAt = preparationPlanStartTime.Value;
                //if (lastPredictionItemsInPreparation != null && lastPredictionItemsInPreparation.Count > 0 && itemsInPreparation.Count == 0)
                //{

                //}

                //if (lastPredictionItemsInPreparation == null)
                //{
                //    preparationStation.SetItemsInPreparation(actionContext, itemsInPreparation);
                //    lastPredictionItemsInPreparation = itemsInPreparation;
                //}
                //else
                //{
                //    #region Remove the items in preparation to recalculate preparation time
                //    var removedItem = lastPredictionItemsInPreparation.Where(x => !itemsInPreparation.Contains(x)).FirstOrDefault();
                //    if (removedItem != null && actionContext.ItemPreparationsStartsAt.ContainsKey(removedItem) && actionContext.ItemPreparationsStartsAt[removedItem] > DateTime.UtcNow)
                //    {
                //        for (int i = lastPredictionItemsInPreparation.IndexOf(removedItem); i < lastPredictionItemsInPreparation.Count; i++)
                //        {
                //            if (actionContext.ItemPreparationsStartsAt.ContainsKey(lastPredictionItemsInPreparation[i]))
                //                actionContext.ItemPreparationsStartsAt.Remove(lastPredictionItemsInPreparation[i]);
                //        }
                //    }
                //    #endregion
                //}
                //foreach (var itemInPreparation in itemsInPreparation.Where(x => lastPredictionItemsInPreparation.Contains(x) && actionContext.ItemPreparationsStartsAt.ContainsKey(x)))
                //{
                //    if (actionContext.GetPreparationEndsAt(itemInPreparation) > previousePreparationEndsAt)
                //        previousePreparationEndsAt = actionContext.GetPreparationEndsAt(itemInPreparation);
                //}


                foreach (var itemInPreparation in itemsInPreparation)//.Where(x => !lastPredictionItemsInPreparation.Contains(x) || !actionContext.ItemPreparationsStartsAt.ContainsKey(x)))
                {

                    //if (previousePreparationEndsAt > DateTime.UtcNow)
                    actionContext.SetPreparationStartsAt(itemInPreparation, previousePreparationEndsAt);
                    //else
                    //    actionContext.SetPreparationStartsAt(itemInPreparation, DateTime.UtcNow);

                    previousePreparationEndsAt = actionContext.GetPreparationEndsAt(itemInPreparation);
                }
                preparationStation.SetItemsInPreparation(actionContext, itemsInPreparation);
                #endregion


                var itemsPendingToPrepare = itemsToPrepare.Where(x => x.State == ItemPreparationState.PreparationDelay || x.State == ItemPreparationState.PendingPreparation).ToList();

                foreach (var positionInterchange in actionContext.PositionInterchanges)
                {
                    int nPosFirstItem = itemsPendingToPrepare.IndexOf(positionInterchange.FirstItemPreparation);
                    int nPosSecondItem = itemsPendingToPrepare.IndexOf(positionInterchange.SecondItemPreparation);
                    if (nPosFirstItem != -1 && nPosSecondItem != -1 && nPosFirstItem + 1 == nPosSecondItem)
                    {
                        itemsPendingToPrepare.Remove(positionInterchange.SecondItemPreparation);
                        itemsPendingToPrepare.Insert(nPosFirstItem, positionInterchange.SecondItemPreparation);
                    }
                }

                ItemsPreparationContext partialAction = null;
                double packingTime = 0;
                bool pendingItemsRerange = false;
                int pendingItemsRearrangements = 0;
                DateTime itemsPendingToPrepareStartTime = previousePreparationEndsAt;
                do
                {
                    previousePreparationEndsAt = itemsPendingToPrepareStartTime;
                    pendingItemsRerange = false;
                    foreach (var itemToPrepare in itemsPendingToPrepare.ToList())
                    {

                        if (itemToPrepare.FindItemsPreparationContext() != partialAction)
                        {
                            if (packingTime != 0)
                                previousePreparationEndsAt = previousePreparationEndsAt + TimeSpanEx.FromMinutes(packingTime);
                            packingTime = 0;
                            partialAction = itemToPrepare.FindItemsPreparationContext();
                        }



                        if (actionContext.GetPreparationStartsAt(itemToPrepare) == null || actionContext.GetPreparationStartsAt(itemToPrepare) != previousePreparationEndsAt)
                        {
                            var oldpreviousePreparationEndsAt = actionContext.GetPreparationStartsAt(itemToPrepare);
                            actionContext.PreparationPlanIsDoubleChecked = false;
                        }
                        actionContext.SetPreparationStartsAt(itemToPrepare, previousePreparationEndsAt);
                        previousePreparationEndsAt = previousePreparationEndsAt + TimeSpanEx.FromMinutes(preparationStation.GetPreparationTimeInMin(itemToPrepare));

                        //if (!stirTheSequence)       //The rearrangements doesn't allowed when we stir preparations sequence
                        {                           //The rearrangements  produce wrong plan when the re planning is in first state when the PreparationPlanStartTime defined for preparation stations.
                            var itemToPreparePreparationSection = itemToPrepare.FindItemsPreparationContext();
                            if (!itemToPrepare.IsInReferencePreparationSection(actionContext))
                            {

                                if (itemToPrepare != itemsPendingToPrepare.Last())
                                {
                                    ItemPreparation nextItemToPrepare = itemsPendingToPrepare[itemsPendingToPrepare.IndexOf(itemToPrepare) + 1];
                                    if (nextItemToPrepare.FindItemsPreparationContext() != itemToPreparePreparationSection)
                                    {
                                        if (nextItemToPrepare.IsInReferencePreparationSection(actionContext))
                                        {
                                            TimeSpan timeSpan = itemToPrepare.MealCourse.GetPreparationForecast(actionContext) - actionContext.GetPreparationEndsAt(itemToPrepare);
                                            if (timeSpan.TotalMinutes >= preparationStation.GetPreparationTimeInMin(nextItemToPrepare) * 0.9)
                                            {
                                                pendingItemsRerange = true;
                                                pendingItemsRearrangements++;
                                                itemsPendingToPrepare.Remove(nextItemToPrepare);
                                                itemsPendingToPrepare.Insert(itemsPendingToPrepare.IndexOf(itemToPrepare), nextItemToPrepare);
                                                actionContext.PositionInterchanges.Add(new PositionInterchange() { FirstItemPreparation = itemToPrepare, SecondItemPreparation = nextItemToPrepare });
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                    }

                }
                while (pendingItemsRerange && pendingItemsRearrangements < itemsPendingToPrepare.Count);

                //var strings = preparationStation.GetActionsToStrings(actionContext);
            }

            //if (preparationStation.GetActionsToDo(actionContext).Count == 0)
            //    preparationStation.PreparationPlanStartTime = DateTime.UtcNow;
        }
        internal static bool IsInReferencePreparationSection(this ItemPreparation itemPreparation, ActionContext actionContext)
        {
            return itemPreparation.MealCourse.GetReferencePreparationSection(actionContext) == itemPreparation.FindItemsPreparationContext();
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
                foreach (var partialAction in preparationStation.GetToDoPreparationSections(actionContext))
                {
                    partialAction.PreparationOrderCommitted = true;

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
        public static List<ItemsPreparationContext> GetToDoPreparationSections(this PreparationStation preparationStation, ActionContext actionContext)
        {

            List<ItemsPreparationContext> preparationSections = new List<ItemsPreparationContext>();//.Where(x=>(x.Action.PreparationForecast-TimeSpanEx.FromMinutes(x.Duration+5))<DateTime.UtcNow).ToList();
            if (actionContext.PreparationSections.ContainsKey(preparationStation))
                preparationSections = actionContext.PreparationSections[preparationStation];
            List<ItemsPreparationContext> filteredPartialActions = new List<ItemsPreparationContext>();

            foreach (var preparationSection in preparationSections)
            {
                if (preparationSection.PreparationOrderCommitted)
                {
                    filteredPartialActions.Add(preparationSection);
                }
                else if ((preparationSection.MealCourse.GetPreparationForecast(actionContext) - preparationSection.GetPreparationForecast(actionContext)).GetTotalMinutes() < 1.5 * Simulator.Velocity || ((preparationSection.MealCourse.GetPreparationForecast(actionContext) - TimeSpanEx.FromMinutes(preparationSection.GetDuration() + (2 * Simulator.Velocity))) < DateTime.UtcNow))
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
                    break;
                }
            }




            return filteredPartialActions;
        }

        internal static double GetDuration(this ItemsPreparationContext itemsPreparationContext)
        {
            return itemsPreparationContext.PreparationItems.OfType<ItemPreparation>().Sum(x => (x.PreparationStation as PreparationStation).GetPreparationTimeInMin(x));
        }

        internal static void GetPreparationSections(this PreparationStation preparationStation, ActionContext actionContext)
        {
            actionContext.PreparationSections[preparationStation] = preparationStation.PreparationSessions;
        }
        internal static void OptimizePreparationPlan(this PreparationStation preparationStation, ActionContext actionContext, bool stirTheSequence)
        {


            List<ItemsPreparationContext> preparationSectionsForOptimazation = null;
            var preparationSections = actionContext.PreparationSections[preparationStation];
            List<ItemsPreparationContext> orderCommittedPreparationSections = preparationSections.Where(x => x.PreparationOrderCommitted).OrderBy(x => x.PreparatioOrder).ToList();
            List<ItemsPreparationContext> preparationSectionsToCommit = new List<ItemsPreparationContext>();
            if (stirTheSequence)
            {

                // first takes the uncommitted  items preparation contexts where the meal course has all items preparation contexts uncommitted 
                preparationSectionsForOptimazation = preparationSections.Where(x => !x.PreparationOrderCommitted).OrderBy(x => (x.MealCourse as MealCourse).GetPreparationForecast(actionContext)).Where(x => x.MealCourse.FoodItemsInProgress.All(y => !y.PreparationOrderCommitted)).ToList();

                #region Debug
#if DEBUG
                var a_count = preparationSectionsForOptimazation.Count;
#endif 
                #endregion


                //in the sequel takes the uncommitted  items preparation contexts where the meal course has at least one items preparation contexts committed 
                preparationSectionsForOptimazation.AddRange(preparationSections.Where(x => !x.PreparationOrderCommitted).OrderBy(x => x.MealCourse.GetPreparationForecast(actionContext)).Where(x => x.MealCourse.FoodItemsInProgress.Any(y => y.PreparationOrderCommitted)).ToList());

                #region Debug
#if DEBUG
                if (preparationSectionsForOptimazation.Count > 1)
                {

                }

                //preparation contexts order by the preparation forecast time of meal course where this belongs   
                var b_count = preparationSectionsForOptimazation.Count;
                if (a_count != b_count)
                {

                }
#endif 
                #endregion

            }
            else
            {


                //preparation contexts order by the preparation forecast time of meal course where this belongs
                preparationSectionsForOptimazation = preparationSections.Where(x => !x.PreparationOrderCommitted).OrderBy(x => x.MealCourse.GetPreparationForecast(actionContext)).ToList();

                #region Debug

                if (preparationSectionsForOptimazation.Count > 1)
                {

                }

                #endregion

                if (actionContext.LastPlanItemPreparationsStartsAt != null)
                {
                    foreach (var preparationSection in preparationSectionsForOptimazation.ToList())
                    {
                        if (preparationSection.MealCourse.GetLastPlanReferencePreparationSection(actionContext) != preparationSection.MealCourse.GetReferencePreparationSection(actionContext) && // the reference preparation section of meal course has change
                            preparationSection.MealCourse.GetLastPlanReferencePreparationSection(actionContext).PreparationOrderCommitted) // there is reference committed preparation section
                        {
                            if (preparationSection.MealCourse.GetLastPlanPreparationForecast(actionContext) < preparationSection.MealCourse.GetPreparationForecast(actionContext))// the meal course preparation forecast is worse from the last plan
                            {
                                if (preparationSectionsForOptimazation.Count > 1)
                                {
                                    int oneUpPos = preparationSectionsForOptimazation.IndexOf(preparationSection) - 1; //moves preparation section one position up 
                                    if (oneUpPos >= 0)
                                    {
                                        preparationSectionsForOptimazation.Remove(preparationSection);
                                        preparationSectionsForOptimazation.Insert(oneUpPos, preparationSection);
                                        preparationSectionsToCommit.Add(preparationSection);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            List<ItemsPreparationContext> optimizedPreparationSections = orderCommittedPreparationSections.ToList();

            //preparation order committed PreparationSessions


            //Adds the re planed preparation contexts 
            optimizedPreparationSections.AddRange(preparationSectionsForOptimazation);



            int i = 0;

            //using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            //{
            //    foreach (var preparationSection in optimizedPreparationSections)
            //    {
            //        preparationSection.PreparatioOrder = i++;

            //        if (preparationSectionsToCommit.Contains(preparationSection))
            //            preparationSection.PreparationOrderCommitted = true;
            //    }

            //    stateTransition.Consistent = true;
            //}


            #region Update preparationOrder

            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                foreach (var preparationSection in optimizedPreparationSections)
                {
                    if (!preparationSection.PreparationOrderCommitted)
                        preparationSection.PreparatioOrder = i++;
                    else
                        i = preparationSection.PreparatioOrder + 1;

                    if (preparationSectionsToCommit.Contains(preparationSection))
                        preparationSection.PreparationOrderCommitted = true;
                }
                stateTransition.Consistent = true;
            }
            #endregion

            #region Debug

            var previous = preparationStation.PreparationSessions;
            if (actionContext.PreparationSections.ContainsKey(preparationStation))
                previous = actionContext.PreparationSections[preparationStation];

            #endregion

            actionContext.PreparationSections[preparationStation] = optimizedPreparationSections;

            #region Debug

            if (preparationSections.Count != optimizedPreparationSections.Count)
            {

            }

            var actionHash = optimizedPreparationSections.Select(x => x.GetHashCode()).ToArray();
            var m_actionsHash = previous.Select(x => x.GetHashCode()).ToArray();
            for (i = 0; i < optimizedPreparationSections.Count; i++)
            {
                if (actionHash.Length == m_actionsHash.Length)
                {
                    if (actionHash[i] != m_actionsHash[i])
                    {

                    }
                }
            }
            #endregion

        }

    }



    /// <MetaDataID>{ca707c10-e87c-4481-9046-f6adae30554b}</MetaDataID>
    class PositionInterchange
    {
        public ItemPreparation FirstItemPreparation;

        public ItemPreparation SecondItemPreparation;
    }
}
