using FlavourBusinessFacade.RoomService;
using FlavourBusinessManager.ServicesContextResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlavourBusinessManager.RoomService
{
    ///// <MetaDataID>{02762bb6-cd92-4b15-ada5-c8c09c003bd6}</MetaDataID>
    //public class PreparationPlan
    //{


    //}



    public class ActionContext
    {
        public Dictionary<PreparationStation, List<ItemsPreparationContext>> ProductionLineActions = new Dictionary<PreparationStation, List<ItemsPreparationContext>>();

        public DateTime GetPreparationStartsAt(ItemPreparation actionSlot)
        {
            DateTime dateTime;
            if (SlotsPreparationStartsAt.TryGetValue(actionSlot, out dateTime))
                return dateTime;

            return DateTime.UtcNow;
        }

        public void SetPreparationStartsAt(ItemPreparation actionSlot, DateTime dateTime)
        {
            if (SlotsPreparationStartsAt.ContainsKey(actionSlot) && SlotsPreparationStartsAt[actionSlot] != dateTime)
            {

            }

            SlotsPreparationStartsAt[actionSlot] = dateTime;

        }

        public DateTime GetPreparationEndsAt(ItemPreparation actionSlot)
        {

            DateTime dateTime;
            if (SlotsPreparationStartsAt.TryGetValue(actionSlot, out dateTime))
                return dateTime + TimeSpanEx.FromMinutes((actionSlot.PreparationStation as PreparationStation).GetPreparationTimeSpanInMin(actionSlot.MenuItem));

            return DateTime.UtcNow + TimeSpanEx.FromMinutes((actionSlot.PreparationStation as PreparationStation).GetPreparationTimeSpanInMin(actionSlot.MenuItem));
        }
        Dictionary<ItemPreparation, DateTime> SlotsPreparationStartsAt = new Dictionary<ItemPreparation, DateTime>();

        //        Dictionary<ActionSlot, DateTime> SlotsPreparationEndsAt = new Dictionary<ActionSlot, DateTime>();

        public bool PreparationPlanIsDoubleChecked { get; internal set; }
    }


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

    public static class PreparationPlanExMethods
    {

        public static DateTime GetPreparationForecast(this IMealCourse mealCourse, ActionContext context)
        {
            var pendingPreparationSessions = mealCourse.FoodItemsInProgress;

            if (pendingPreparationSessions.Count != 0)
                return pendingPreparationSessions.OrderBy(x => x.GetPreparationForecast(context)).Last().GetPreparationForecast(context);
            else
            {
                if (pendingPreparationSessions.Count == 0)
                    return DateTime.Now;
                return pendingPreparationSessions.OrderBy(x => x.GetPreparationForecast(context)).Last().GetPreparationForecast(context);
            }
        }

        public static DateTime GetPreparationForecast(this ItemsPreparationContext preparationSession, ActionContext actionContext)
        {
            if (actionContext.ProductionLineActions.ContainsKey(preparationSession.GetPreparationStation()))
                return actionContext.GetPreparationEndsAt(preparationSession.PreparationItems.OfType<ItemPreparation>().OrderBy(x => actionContext.GetPreparationEndsAt(x)).Last());
            else
                return preparationSession.PreparedAtForecast;
        }

        public static DateTime GetPreparationStartsAt(this ItemsPreparationContext preparationSession, ActionContext actionContext)
        {
            if (actionContext.ProductionLineActions.ContainsKey(preparationSession.GetPreparationStation()))
                return actionContext.GetPreparationStartsAt(preparationSession.PreparationItems.OfType<ItemPreparation>().OrderBy(x => actionContext.GetPreparationEndsAt(x)).Last());
            else
                return DateTime.UtcNow;
        }

        static PreparationStation GetPreparationStation(this ItemsPreparationContext preparationSession)
        {
            return preparationSession.PreparationItems[0].PreparationStation as PreparationStation;
        }

        public static void GetPredictions(this PreparationStation preparationStation, ActionContext actionContext)
        {
            if (preparationStation.PreparationPlanStartTime == null)
                preparationStation.PreparationPlanStartTime = DateTime.UtcNow;
            DateTime previousePreparationEndsAt = DateTime.UtcNow;

            if (preparationStation.PreparationPlanStartTime != null)
                previousePreparationEndsAt = preparationStation.PreparationPlanStartTime.Value;

            if (actionContext.ProductionLineActions.ContainsKey(preparationStation))
            {
                var itemsToPrepare = (from thePartialAction in actionContext.ProductionLineActions[preparationStation]
                                      where thePartialAction.GetPreparationStation() == preparationStation
                                      from slot in thePartialAction.GetItemsToPrepare()
                                      select slot).ToList();
                ItemsPreparationContext partialAction = null;
                double packingTime = 0;
                foreach (var itemToPrepare in itemsToPrepare)
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
                        actionContext.PreparationPlanIsDoubleChecked = false;
                    actionContext.SetPreparationStartsAt(itemToPrepare, previousePreparationEndsAt);
                    previousePreparationEndsAt = previousePreparationEndsAt + TimeSpanEx.FromMinutes(preparationStation.GetPreparationTimeSpanInMin(itemToPrepare.MenuItem));
                }
            }

            if (preparationStation.GetActionsToDo(actionContext).Count == 0)
                preparationStation.PreparationPlanStartTime = DateTime.UtcNow;
        }

        internal static List<string> GetActionsToStrings(this PreparationStation preparationStation, ActionContext actionContext)
        {
            if (!actionContext.ProductionLineActions.ContainsKey(preparationStation))
                return (from partialAction in preparationStation.PreparationSessions
                        select partialAction.ToString()).ToList();

            List<string> strings =
            (from partialAction in actionContext.ProductionLineActions[preparationStation]
             select partialAction.TotString(actionContext)).ToList();

            return strings;
        }
        static string TotString(this ItemsPreparationContext preparationSession, ActionContext actionContext)
        {
            var preparationForecast = preparationSession.GetPreparationForecast(actionContext);

            var preparationStartsAt = preparationSession.GetPreparationStartsAt(actionContext);

            return preparationSession.MealCourse.Meal.Session.ServicePoint.Description + " " + preparationSession.Description + " " + preparationSession.MealCourse.Name + " " + string.Format("{0:h:mm:ss tt}", preparationStartsAt) + " " + " " + string.Format("{0:h:mm:ss tt}", preparationForecast) + " " + string.Format("{0:h:mm:ss tt}", preparationSession.MealCourse.GetPreparationForecast(actionContext)) + " itemsToPrepare : " + preparationSession.PreparationItems.Count.ToString() + " v:" + preparationSession.GetPreparationStation()?.PreparationVelocity;


            return "";
        }
        static List<ItemPreparation> GetItemsToPrepare(this ItemsPreparationContext preparationSession)
        {
            var itemsToPrepare = (from preparationItem in preparationSession.PreparationItems
                                  where preparationItem.State.IsIntheSameOrFollowingState(ItemPreparationState.PendingPreparation) &&
                                  preparationItem.State.IsInPreviousState(ItemPreparationState.IsRoasting)
                                  select preparationItem).OfType<ItemPreparation>().ToList();

            return itemsToPrepare;
        }

        public static void ActionsOrderCommited(this PreparationStation preparationStation, ActionContext actionContext)
        {
            foreach (var partialAction in preparationStation.GetActionsToDo(actionContext))
            {
                partialAction.PreparationOrderCommited = true;

                foreach (var preparationItem in partialAction.PreparationItems)
                {
                    if (preparationItem.State == FlavourBusinessFacade.RoomService.ItemPreparationState.PreparationDelay)
                        preparationItem.State = FlavourBusinessFacade.RoomService.ItemPreparationState.PendingPreparation;
                }
            }
        }
        public static List<ItemsPreparationContext> GetActionsToDo(this PreparationStation preparationStation, ActionContext actionContext)
        {

            List<ItemsPreparationContext> preparationSessions = new List<ItemsPreparationContext>();//.Where(x=>(x.Action.PreparationForecast-TimeSpanEx.FromMinutes(x.Duration+5))<DateTime.UtcNow).ToList();
            if (actionContext.ProductionLineActions.ContainsKey(preparationStation))
                preparationSessions = actionContext.ProductionLineActions[preparationStation];
            List<ItemsPreparationContext> filteredPartialActions = new List<ItemsPreparationContext>();

            foreach (var preparationSession in preparationSessions)
            {
                if ((preparationSession.MealCourse.GetPreparationForecast(actionContext) - preparationSession.GetPreparationForecast(actionContext)).GetTotalMinutes() < 1.5 * Simulator.Velocity || ((preparationSession.MealCourse.GetPreparationForecast(actionContext) - TimeSpanEx.FromMinutes(preparationSession.GetDuration() + (2 * Simulator.Velocity))) < DateTime.UtcNow))
                {
                    if (preparationSession.MealCourse.Name == "a11")
                    {
                    }
                    if ((preparationSession.MealCourse.GetPreparationForecast(actionContext) - preparationSession.GetPreparationForecast(actionContext)).GetTotalMinutes() < 1.5 * Simulator.Velocity)
                    {
                    }

                    filteredPartialActions.Add(preparationSession);
                }
                else if (preparationSession.PreparationItems.Any(x => x.State > FlavourBusinessFacade.RoomService.ItemPreparationState.ΙnPreparation))
                {

                    filteredPartialActions.Add(preparationSession);
                }
                else
                {
                    if (preparationSession.PreparationItems.All(x => x.State > FlavourBusinessFacade.RoomService.ItemPreparationState.PreparationDelay))
                        filteredPartialActions.Add(preparationSession);
                    else
                    {
                        if (preparationSession.PreparationOrderCommited)
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
            return itemsPreparationContext.PreparationItems.OfType<ItemPreparation>().Sum(x => (x.PreparationStation as PreparationStation).GetPreparationTimeSpanInMin(x.MenuItem));
        }

        internal static void OptimizePreparationPlan(this PreparationStation preparationStation, ActionContext actionContext, bool stirTheSequence)
        {
            List<ItemsPreparationContext> PreparationSessionsForOptimazation = null;
            if (stirTheSequence)
            {
                // first takes the uncommitted  items preparation contexts where the meal course has all items preparation contexts uncommitted 
                PreparationSessionsForOptimazation = preparationStation.PreparationSessions.Where(x => !x.PreparationOrderCommited).OrderBy(x => (x.MealCourse as MealCourse).GetPreparationForecast(actionContext)).Where(x => x.MealCourse.FoodItemsInProgress.All(y => !y.PreparationOrderCommited)).ToList();

                var a_count = PreparationSessionsForOptimazation.Count;

                //in the sequel takes the uncommitted  items preparation contexts where the meal course has at least one items preparation contexts committed 
                PreparationSessionsForOptimazation.AddRange(preparationStation.PreparationSessions.Where(x => !x.PreparationOrderCommited).OrderBy(x => x.MealCourse.GetPreparationForecast(actionContext)).Where(x => x.MealCourse.FoodItemsInProgress.Any(y => y.PreparationOrderCommited)).ToList());

                //preparation contexts order by the preparation forecast time of meal course where this belongs   
                var b_count = PreparationSessionsForOptimazation.Count;
                if (a_count != b_count)
                {

                }
            }
            else
            {
                //preparation contexts order by the preparation forecast time of meal course where this belongs
                PreparationSessionsForOptimazation = preparationStation.PreparationSessions.Where(x => !x.PreparationOrderCommited).OrderBy(x => x.MealCourse.GetPreparationForecast(actionContext)).ToList();
            }


            //preparation order committed PreparationSessions
            List<ItemsPreparationContext> orderCommittedreparationContexts = preparationStation.PreparationSessions.Where(x => x.PreparationOrderCommited).OrderBy(x => x.PreparatioOrder).ToList();

            //Adds the re planed preparation contexts 
            orderCommittedreparationContexts.AddRange(PreparationSessionsForOptimazation);
            List<ItemsPreparationContext> actions = orderCommittedreparationContexts;


            //List<PartialAction> actions = Actions.OrderBy(x => x.MainAction.GetPreparationForecast(actionContext)).ToList();

            int i = 0;
            foreach (var partialAction in actions)
                partialAction.PreparatioOrder = i++;

            actionContext.ProductionLineActions[preparationStation] = actions;
        }

    }

    public class Simulator
    {
        public static double Velocity = 0.33;
    }
}
