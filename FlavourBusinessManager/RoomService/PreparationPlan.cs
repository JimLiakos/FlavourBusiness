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

        public DateTime? GetPreparationStartsAt(ItemPreparation actionSlot)
        {
            DateTime dateTime;
            if (SlotsPreparationStartsAt.TryGetValue(actionSlot, out dateTime))
                return dateTime;

            return null;
        }

        public void SetPreparationStartsAt(ItemPreparation actionSlot, DateTime dateTime)
        {
            if (SlotsPreparationStartsAt.ContainsKey(actionSlot) && SlotsPreparationStartsAt[actionSlot] != dateTime)
            {

            }

            SlotsPreparationStartsAt[actionSlot] = dateTime;

        }

        public DateTime? GetPreparationEndsAt(ItemPreparation actionSlot)
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
                return actionContext.GetPreparationEndsAt(preparationSession.PreparationItems.OfType<ItemPreparation>().OrderBy(x => actionContext.GetPreparationEndsAt(x)).Last()).Value;
            else
                return preparationSession.PreparedAtForecast;
        }

        static PreparationStation GetPreparationStation(this ItemsPreparationContext preparationSession)
        {
            return preparationSession.PreparationItems[0].PreparationStation as PreparationStation;
        }

        public static void GetPredictions(this PreparationStation preparationStation, ActionContext actionContext)
        {
            //if (PreviousePreparationEndsAt == null)
            //    PreviousePreparationEndsAt = DateTime.UtcNow;
            //var previousePreparationEndsAt = PreviousePreparationEndsAt.Value;
            //if (actionContext.ProductionLineActions.ContainsKey(preparationStation))
            //{
            //    var slots = (from thePartialAction in actionContext.ProductionLineActions[preparationStation]
            //                 where thePartialAction.GetPreparationStation() == preparationStation
            //                 from slot in thePartialAction.ItemsToPrepare
            //                 select slot).ToList();
            //    ItemsPreparationContext partialAction = null;
            //    double packingTime = 0;
            //    foreach (var slot in slots)
            //    {
            //        if (slot.PreparationSession != partialAction)
            //        {
            //            if (packingTime != 0)
            //                previousePreparationEndsAt = previousePreparationEndsAt + TimeSpanEx.FromMinutes(packingTime);
            //            packingTime = 0;
            //            partialAction = slot.PreparationSession;
            //        }

            //        packingTime += slot.PackingTime;

            //        if (actionContext.GetPreparationStartsAt(slot) == null || actionContext.GetPreparationStartsAt(slot).Value != previousePreparationEndsAt)
            //            actionContext.PreparationPlanIsDoubleChecked = false;
            //        actionContext.SetPreparationStartsAt(slot, previousePreparationEndsAt);
            //        previousePreparationEndsAt = previousePreparationEndsAt + TimeSpanEx.FromMinutes(slot.ActiveDuration);
            //    }
            //}

            //if (GetActionsToDo(actionContext).Count == 0)
            //    PreviousePreparationEndsAt = DateTime.UtcNow;
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

            var preparationSessions = preparationStation.PreparationSessions;//.Where(x=>(x.Action.PreparationForecast-TimeSpanEx.FromMinutes(x.Duration+5))<DateTime.UtcNow).ToList();
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

        internal static void OptimizePreparationPlan(this PreparationStation preparationStation,ActionContext actionContext, bool stirTheSequence)
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
        }

    }

    public class Simulator
    {
        public static double Velocity = 0.33;
    }
}
