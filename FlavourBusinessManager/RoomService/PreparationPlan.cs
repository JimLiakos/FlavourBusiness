using FlavourBusinessFacade.RoomService;
using FlavourBusinessManager.ServicesContextResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlavourBusinessManager.RoomService
{



    public class ActionContext
    {
        /// <summary>
        /// 
        /// </summary>
        internal Dictionary<PreparationStation, List<ItemsPreparationContext>> PreparationSections = new Dictionary<PreparationStation, List<ItemsPreparationContext>>();

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
                return dateTime + TimeSpanEx.FromMinutes((itemPreparation.PreparationStation as PreparationStation).GetPreparationTimeSpanInMin(itemPreparation.MenuItem));

            return DateTime.UtcNow + TimeSpanEx.FromMinutes((itemPreparation.PreparationStation as PreparationStation).GetPreparationTimeSpanInMin(itemPreparation.MenuItem));
        }


       internal Dictionary<ItemPreparation, DateTime> ItemPreparationsStartsAt = new Dictionary<ItemPreparation, DateTime>();

        

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

        public static DateTime GetPreparationForecast(this ItemsPreparationContext preparationSection, ActionContext actionContext)
        {
            if (actionContext.PreparationSections.ContainsKey(preparationSection.GetPreparationStation()))
                return actionContext.GetPreparationEndsAt(preparationSection.PreparationItems.OfType<ItemPreparation>().OrderBy(x => actionContext.GetPreparationEndsAt(x)).Last());
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

        public static void GetPredictions(this PreparationStation preparationStation, ActionContext actionContext)
        {
            if (preparationStation.PreparationPlanStartTime == null)
                preparationStation.PreparationPlanStartTime = DateTime.UtcNow;
            DateTime previousePreparationEndsAt = DateTime.UtcNow;

            if (preparationStation.PreparationPlanStartTime != null)
                previousePreparationEndsAt = preparationStation.PreparationPlanStartTime.Value;

            var preparationPlanStartTime = previousePreparationEndsAt;

            if (actionContext.PreparationSections.ContainsKey(preparationStation))
            {
                var itemsToPrepare = (from thePartialAction in actionContext.PreparationSections[preparationStation]
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
                    {
                        var oldpreviousePreparationEndsAt = actionContext.GetPreparationStartsAt(itemToPrepare);
                        actionContext.PreparationPlanIsDoubleChecked = false;
                    }
                    actionContext.SetPreparationStartsAt(itemToPrepare, previousePreparationEndsAt);
                    previousePreparationEndsAt = previousePreparationEndsAt + TimeSpanEx.FromMinutes(preparationStation.GetPreparationTimeSpanInMin(itemToPrepare.MenuItem));
                }

                var strings = preparationStation.GetActionsToStrings(actionContext);
            }

            if (preparationStation.GetActionsToDo(actionContext).Count == 0)
                preparationStation.PreparationPlanStartTime = DateTime.UtcNow;
        }

        internal static List<string> GetActionsToStrings(this PreparationStation preparationStation, ActionContext actionContext)
        {
            if (!actionContext.PreparationSections.ContainsKey(preparationStation))
                return (from preparationSection in preparationStation.PreparationSessions
                        orderby preparationSection.MealCourseStartsAt
                        select preparationSection.ToString()).ToList();

            List<string> strings =
            (from preparationSection in actionContext.PreparationSections[preparationStation]
             orderby preparationSection.GetPreparationStartsAt( actionContext)
             select preparationSection.TotString(actionContext)).ToList();

            return strings;
        }
        static string TotString(this ItemsPreparationContext preparationSection, ActionContext actionContext)
        {
            var preparationForecast = preparationSection.GetPreparationForecast(actionContext);

            var preparationStartsAt = preparationSection.GetPreparationStartsAt(actionContext);

            var standby = !preparationSection.PreparationItems.Any(x => x.State.IsIntheSameOrFollowingState(ItemPreparationState.PendingPreparation));

            if(standby )
                return "X "+ preparationSection.MealCourse.Meal.Session.ServicePoint.Description + " " + preparationSection.MealCourseStartsAt?.Day.ToString()+" "+ preparationSection.MealCourseStartsAt?.ToShortTimeString() + " " + preparationSection.Description + " " + preparationSection.MealCourse.Name + " " + string.Format("{0:h:mm:ss tt}", preparationStartsAt) + " " + " " + string.Format("{0:h:mm:ss tt}", preparationForecast) + " " + string.Format("{0:h:mm:ss tt}", preparationSection.MealCourse.GetPreparationForecast(actionContext)) + " itemsToPrepare : " + preparationSection.PreparationItems.Count.ToString() + " v:" + preparationSection.GetPreparationStation()?.PreparationVelocity;
            else
                return preparationSection.MealCourse.Meal.Session.ServicePoint.Description + " " + preparationSection.MealCourseStartsAt?.Day.ToString() + " " + preparationSection.MealCourseStartsAt?.ToShortTimeString() + " " + preparationSection.Description + " " + preparationSection.MealCourse.Name + " " + string.Format("{0:h:mm:ss tt}", preparationStartsAt) + " " + " " + string.Format("{0:h:mm:ss tt}", preparationForecast) + " " + string.Format("{0:h:mm:ss tt}", preparationSection.MealCourse.GetPreparationForecast(actionContext)) + " itemsToPrepare : " + preparationSection.PreparationItems.Count.ToString() + " v:" + preparationSection.GetPreparationStation()?.PreparationVelocity;




        }
        static List<ItemPreparation> GetItemsToPrepare(this ItemsPreparationContext preparationSection)
        {
            var itemsToPrepare = (from preparationItem in preparationSection.PreparationItems
                                  where preparationItem.State.IsIntheSameOrFollowingState(ItemPreparationState.PreparationDelay) &&
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

            var previous = preparationStation.PreparationSessions;
            if (actionContext.PreparationSections.ContainsKey(preparationStation))
                previous = actionContext.PreparationSections[preparationStation];

            actionContext.PreparationSections[preparationStation] = actions;
            var actionHash = actions.Select(x => x.GetHashCode()).ToArray();
            var m_actionsHash = previous.Select(x => x.GetHashCode()).ToArray();
            for (i = 0;i < actions.Count;i++)
            {
                if(actionHash[i]!= m_actionsHash[i])
                {

                }
            }

        }

    }

    public class Simulator
    {
        public static double Velocity = 0.33;
    }
}
