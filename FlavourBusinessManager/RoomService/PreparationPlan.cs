using FlavourBusinessFacade.RoomService;
using FlavourBusinessManager.HumanResources;
using FlavourBusinessManager.ServicesContextResources;
using Microsoft.Azure.Documents;
using OOAdvantech.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace FlavourBusinessManager.RoomService
{


    /// <summary>
    /// Preparation plan has keeps infos about the item preparation start time and the preparation duration.
    /// Also has temporary info, useful for the plan build and plan update, like the order in which each preparation section begins  
    /// </summary>
    /// <MetaDataID>{16a0ba3c-c9f6-44a7-94cf-0598ffea5437}</MetaDataID>
    public class PreparationPlan
    {
        /// <summary>
        /// Defines the order in which each preparation section begins 
        /// </summary>
        internal Dictionary<PreparationStation, List<ItemsPreparationContext>> PreparationSections = new Dictionary<PreparationStation, List<ItemsPreparationContext>>();


        /// <summary>
        /// Defines the items where are in preparation state.
        /// </summary>
        internal Dictionary<PreparationStation, List<ItemPreparation>> ItemsInPreparation = new Dictionary<PreparationStation, List<ItemPreparation>>();

        /// <summary>
        /// Position interchange are necessary in case where there is preparation gap.
        /// In case where planner delays the start of item preparation for meal course synchronization reason, 
        /// one item from the preparation section can fills the gap if it meets certain conditions 
        /// </summary>
        internal List<PositionInterchange> PositionInterchanges = new List<PositionInterchange>();


        /// <summary>
        /// Gets the planned item preparation start time
        /// </summary>
        /// <param name="itemPreparation">
        /// Defines the item where looking for start preparation time
        /// </param>
        /// <returns>
        /// return preparation start time
        /// </returns>
        public DateTime GetPreparationStartsAt(ItemPreparation itemPreparation)
        {

            ItemPreparationStartsFor itemPreparationStartsFor = null;
            if (ItemPreparationsStartsAt.TryGetValue(itemPreparation, out itemPreparationStartsFor))
            {
                if (itemPreparationStartsFor.PreparationStation != itemPreparation.ActivePreparationStation)
                    ItemPreparationsStartsAt.Remove(itemPreparation);
                else
                    return itemPreparationStartsFor.Starts;
            }

            return DateTime.UtcNow;
        }

        public void SetPreparationStartsAt(ItemPreparation itemPreparation, DateTime dateTime)
        {

            #region debug code
            if (ItemPreparationsStartsAt.ContainsKey(itemPreparation) && Math.Abs((ItemPreparationsStartsAt[itemPreparation].Starts - dateTime).TotalMinutes) > 1.2)
            {
            }
            #endregion

            ItemPreparationStartsFor itemPreparationStartsFor = null;
            if (!ItemPreparationsStartsAt.TryGetValue(itemPreparation, out itemPreparationStartsFor))
            {
                itemPreparationStartsFor = new ItemPreparationStartsFor(itemPreparation.ActivePreparationStation as PreparationStation);
                ItemPreparationsStartsAt.Add(itemPreparation, itemPreparationStartsFor);
            }

            itemPreparationStartsFor.Starts = dateTime;


            #region debug code
            if ((GetPreparationEndsAt(itemPreparation) - DateTime.UtcNow).TotalSeconds < -20)
            {

            }
            #endregion

        }

        /// <summary>
        /// Gets the planned item preparation end time
        /// </summary>
        /// <param name="itemPreparation">
        /// Defines the item where looking for   preparation end time
        /// </param>
        /// <returns>
        /// return preparation end time
        /// </returns>
        public DateTime GetPreparationEndsAt(ItemPreparation itemPreparation)
        {

            ItemPreparationStartsFor itemPreparationStartsFor = null;
            if (ItemPreparationsStartsAt.TryGetValue(itemPreparation, out itemPreparationStartsFor))
            {
                if (itemPreparationStartsFor.PreparationStation == itemPreparation.ActivePreparationStation)
                    return itemPreparationStartsFor.Starts + TimeSpanEx.FromMinutes((itemPreparation.PreparationStation as PreparationStation).GetPreparationTimeSpanInMin(itemPreparation));
                else
                    ItemPreparationsStartsAt.Remove(itemPreparation);

            }

            return DateTime.UtcNow + TimeSpanEx.FromMinutes((itemPreparation.PreparationStation as PreparationStation).GetPreparationTimeSpanInMin(itemPreparation));
        }


        /// <summary>
        /// Gets the  item preparation end time in previous plan.
        /// In case where there isn't previous plan return DateTime.MinValue 
        /// </summary>
        /// <param name="itemPreparation">
        /// Defines the item where looking for preparation end time
        /// </param>
        /// <returns>
        /// return preparation end time
        /// </returns>
        public DateTime GetLastPlanPreparationEndsAt(ItemPreparation itemPreparation)
        {
            ItemPreparationStartsFor itemPreparationStartsFor = null;
            if (LastPlanItemPreparationsStartsAt.TryGetValue(itemPreparation, out itemPreparationStartsFor))
            {
                if (itemPreparationStartsFor.PreparationStation == itemPreparation.ActivePreparationStation)
                    return itemPreparationStartsFor.Starts + TimeSpanEx.FromMinutes((itemPreparation.PreparationStation as PreparationStation).GetPreparationTimeSpanInMin(itemPreparation));
                else
                    LastPlanItemPreparationsStartsAt.Remove(itemPreparation);
            }
            return DateTime.MinValue;
        }

        /// <summary>
        /// Defines the dictionary with items preparation start time
        /// </summary>
        internal Dictionary<ItemPreparation, ItemPreparationStartsFor> ItemPreparationsStartsAt = new Dictionary<ItemPreparation, ItemPreparationStartsFor>();

        /// <summary>
        /// Defines the dictionary with items preparation start time in previous plan
        /// </summary>
        internal Dictionary<ItemPreparation, ItemPreparationStartsFor> LastPlanItemPreparationsStartsAt;

        /// <summary>
        /// The planning mechanism planning item preparation for each preparation station in cycles 
        /// In case where to cycles produce the same plan the flag PreparationPlanIsDoubleChecked is true and the plan is valid otherwise
        /// the flag PreparationPlanIsDoubleChecked is false
        /// </summary>
        internal bool PreparationPlanIsDoubleChecked { get; set; }


        /// <summary>
        /// Before the system starts the re planning  procedure removes all item where are in state of prepared 
        /// </summary>
        internal void RemoveOutOfPlanPreparationItems()
        {
            foreach (var itemOutOfPlan in ItemPreparationsStartsAt.Keys.Where(x => x.State.IsInFollowingState(ItemPreparationState.InPreparation)).ToList())
                ItemPreparationsStartsAt.Remove(itemOutOfPlan);

            foreach (var outOfPlanItemEntry in (from itemsInPreparationEntry in ItemsInPreparation
                                                from itemInPreparation in itemsInPreparationEntry.Value
                                                where itemInPreparation.State.IsInFollowingState(ItemPreparationState.InPreparation)
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

    /// <summary>
    /// Defines the extension methods for the classes MealCourse, PreparationSection,PreparationStation necessary for preparation planing
    /// </summary>
    /// <MetaDataID>{3fad3b58-8af7-4061-814a-61c69c4d658e}</MetaDataID>
    // <MetaDataID>{8b5ea92a-eb91-4c86-9941-8f646b46e1ac}</MetaDataID>
    public static class PreparationPlanExMethods
    {
        /// <summary>
        /// Gets  a forecast about the end time of meal course preparation 
        /// </summary>
        /// <param name="mealCourse">Extension class this object </param>
        /// <param name="preparationPlan">
        /// Defines preparation plan.
        /// </param>
        /// <returns></returns>
        public static DateTime GetPreparationForecast(this IMealCourse mealCourse, PreparationPlan preparationPlan)
        {
            var pendingPreparationSessions = mealCourse.FoodItemsInProgress.Where(x => x.GetPreparationStation() != null && x.PreparationItems.
                    Any(y => y.State.IsIntheSameOrFollowingState(ItemPreparationState.PreparationDelay) && y.State.IsInTheSameOrPreviousState(ItemPreparationState.InPreparation))).ToList();

            var allPreparationSessions = mealCourse.FoodItemsInProgress.Where(x => x.GetPreparationStation() != null).ToList();
            if (allPreparationSessions.Count != pendingPreparationSessions.Count)
            {

            }

            if (pendingPreparationSessions.Count != 0)
                return pendingPreparationSessions.OrderBy(x => x.GetPreparationForecast(preparationPlan)).Last().GetPreparationForecast(preparationPlan);
            else
            {
                if (pendingPreparationSessions.Count == 0)
                    return DateTime.Now;
                return pendingPreparationSessions.OrderBy(x => x.GetPreparationForecast(preparationPlan)).Last().GetPreparationForecast(preparationPlan);
            }
        }

        public static DateTime GetLastPlanPreparationForecast(this IMealCourse mealCourse, PreparationPlan context)
        {
            var pendingPreparationSessions = mealCourse.FoodItemsInProgress.Where(x => x.GetPreparationStation() != null).ToList();

            if (pendingPreparationSessions.Count != 0)
                return pendingPreparationSessions.OrderBy(x => x.GetLastPlanPreparationForecast(context)).Last().GetLastPlanPreparationForecast(context);
            else
                return DateTime.Now;

        }

        /// <summary>
        /// MealCourse with many ItemsPreparationContext one from that is the reference ItemsPreparationContext. Is th ItemsPreparationContext which will prepared at latest time.  
        /// </summary>
        /// <param name="mealCourse"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static ItemsPreparationContext GetReferenceItemsPreparationContext(this IMealCourse mealCourse, PreparationPlan context)
        {
            var pendingItemsPreparationContexts = mealCourse.FoodItemsInProgress.Where(x => x.GetPreparationStation() != null).ToList();

            return pendingItemsPreparationContexts.OrderBy(x => x.GetPreparationForecast(context)).LastOrDefault();
        }

        public static ItemsPreparationContext GetLastPlanReferencePreparationSection(this IMealCourse mealCourse, PreparationPlan context)
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
        public static DateTime GetPreparationForecast(this ItemsPreparationContext preparationSection, PreparationPlan actionContext)
        {
            if (actionContext.PreparationSections.ContainsKey(preparationSection.GetPreparationStation()))
            {
                var lastItemToPrepare = preparationSection.PreparationItems.
                    Where(x => x.State.IsIntheSameOrFollowingState(ItemPreparationState.PreparationDelay) && x.State.IsInTheSameOrPreviousState(ItemPreparationState.InPreparation)).
                    OfType<ItemPreparation>().OrderBy(x => actionContext.GetPreparationEndsAt(x) + TimeSpan.FromMinutes(x.CookingTimeSpanInMin)).LastOrDefault();


                if (lastItemToPrepare == null)//All items prepared
                    return DateTime.UtcNow;
                else
                    return actionContext.GetPreparationEndsAt(lastItemToPrepare);
            }
            else
                return preparationSection.PreparedAtForecast;
        }

        public static DateTime GetLastPlanPreparationForecast(this ItemsPreparationContext preparationSection, PreparationPlan actionContext)
        {
            if (actionContext.PreparationSections.ContainsKey(preparationSection.GetPreparationStation()))
                return actionContext.GetLastPlanPreparationEndsAt(preparationSection.PreparationItems.OfType<ItemPreparation>().OrderBy(x => actionContext.GetLastPlanPreparationEndsAt(x)).Last());
            else
                return preparationSection.PreparedAtForecast;
        }

        public static DateTime GetPreparationStartsAt(this ItemsPreparationContext itemsPreparationContext, PreparationPlan actionContext)
        {
            if (actionContext.PreparationSections.ContainsKey(itemsPreparationContext.GetPreparationStation()))
                return actionContext.GetPreparationStartsAt(itemsPreparationContext.PreparationItems.OfType<ItemPreparation>().OrderBy(x => actionContext.GetPreparationEndsAt(x)).Last());
            else
                return DateTime.UtcNow;
        }

        static PreparationStation GetPreparationStation(this ItemsPreparationContext itemsPreparationContext)
        {
            var preparationStation = ServicePointRunTime.ServicesContextRunTime.Current.PreparationStations.OfType<PreparationStation>().Where(x => x.PreparationStationIdentity == itemsPreparationContext.PreparationStationIdentity).FirstOrDefault();

            if (preparationStation == null)
            {
                preparationStation = ServicePointRunTime.ServicesContextRunTime.Current.PreparationStations.SelectMany(x => x.SubStations).OfType<PreparationStation>().Where(x => x.PreparationStationIdentity == itemsPreparationContext.PreparationStationIdentity).FirstOrDefault();
            }
            return preparationStation;
        }

        public static List<ItemPreparation> GetLastPredictionItemsInPreparation(this PreparationStation preparationStation, PreparationPlan actionContext)
        {
            List<ItemPreparation> itemsInPreparation = null;
            actionContext.ItemsInPreparation.TryGetValue(preparationStation, out itemsInPreparation);


            return itemsInPreparation;
        }
        public static void SetItemsInPreparation(this PreparationStation preparationStation, PreparationPlan actionContext, List<ItemPreparation> itemsInPreparation)
        {
            actionContext.ItemsInPreparation[preparationStation] = itemsInPreparation;
        }

        public static void GetPredictions(this PreparationStation preparationStation, PreparationPlan actionContext, bool stirTheSequence)
        {

            if (actionContext.PreparationSections.ContainsKey(preparationStation))
            {

                DateTime? preparationPlanStartTime = preparationStation.PreparationPlanStartTime;

                var itemsToPrepare = (from thePartialAction in actionContext.PreparationSections[preparationStation]
                                      where thePartialAction.GetPreparationStation() == preparationStation
                                      from slot in thePartialAction.GetItemsToPrepare()
                                      select slot).ToList();

                #region Debug code 
                List<ItemsPreparationContext> preparationSections = preparationStation.FoodItemsInProgress.ToList();
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


                List<ItemPreparation> itemsInPreparation = itemsToPrepare.Where(x => x.State == ItemPreparationState.InPreparation && actionContext.ItemPreparationsStartsAt.ContainsKey(x)).OrderBy(x => actionContext.GetPreparationStartsAt(x)).ToList();
                itemsInPreparation.AddRange(itemsToPrepare.Where(x => x.State == ItemPreparationState.InPreparation && !actionContext.ItemPreparationsStartsAt.ContainsKey(x)));

                if (itemsInPreparation.Count > 0 && (lastPredictionItemsInPreparation == null || lastPredictionItemsInPreparation.Count == 0))//!itemsInPreparation.Any(x=>lastPredictionItemsInPreparation.Contains(x)))
                {
                    preparationPlanStartTime = DateTime.UtcNow;
                    preparationStation.PreparationPlanStartTime = preparationPlanStartTime;
                }



                DateTime previousPreparationEndsAt = preparationPlanStartTime.Value;


                foreach (var itemInPreparation in itemsInPreparation)//.Where(x => !lastPredictionItemsInPreparation.Contains(x) || !actionContext.ItemPreparationsStartsAt.ContainsKey(x)))
                {


                    actionContext.SetPreparationStartsAt(itemInPreparation, previousPreparationEndsAt);


                    previousPreparationEndsAt = actionContext.GetPreparationEndsAt(itemInPreparation);
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
                bool pendingItemsRearrange = false;
                int pendingItemsRearrangements = 0;
                DateTime itemsPendingToPrepareStartTime = previousPreparationEndsAt;
                do
                {
                    previousPreparationEndsAt = itemsPendingToPrepareStartTime;
                    pendingItemsRearrange = false;
                    foreach (var itemToPrepare in itemsPendingToPrepare.ToList())
                    {

                        if (itemToPrepare.FindItemsPreparationContext() != partialAction)
                        {
                            if (packingTime != 0)
                                previousPreparationEndsAt = previousPreparationEndsAt + TimeSpanEx.FromMinutes(packingTime);
                            packingTime = 0;
                            partialAction = itemToPrepare.FindItemsPreparationContext();
                        }



                        if (actionContext.GetPreparationStartsAt(itemToPrepare) == null || actionContext.GetPreparationStartsAt(itemToPrepare) != previousPreparationEndsAt)
                        {
                            var oldPreviousPreparationEndsAt = actionContext.GetPreparationStartsAt(itemToPrepare);
                            actionContext.PreparationPlanIsDoubleChecked = false;
                        }
                        actionContext.SetPreparationStartsAt(itemToPrepare, previousPreparationEndsAt);
                        previousPreparationEndsAt = previousPreparationEndsAt + TimeSpanEx.FromMinutes(preparationStation.GetPreparationTimeSpanInMin(itemToPrepare));


                        //The rearrangements produce wrong plan when the re planning is in first state when the PreparationPlanStartTime defined for preparation stations.

                        #region Creates position interchange between two items "PositionInterchange"


                        #region More info
                        //Σε ένα παρασκευαστήριο κάποιες ενότητες παρασκευής(κάθε ενότητα αντιστοιχεί σε μια παραγγελία) ανταγωνίζονται για την σειρά εκτέλεσης.
                        //Η πρώτη Α ενότητα είναι εξαρτημένη από μια ενότητα αναφοράς σε άλλο παρασκευαστήριο.
                        //Η Δεύτερη Β δεν έχει χρονικές δεσμεύσεις.Εάν ο ελεγκτής επιλέξει να αλλάξει την σειρά(πρώτη η Β και ύστερα η Α)
                        //η καινούρια σειρά θα είναι βέλτιστη για το παρασκευαστήριο αλλά έχει χαθεί ο συγχρονισμός της Α με την ενότητα αναφοράς στο άλλο παρασκευαστήριο.
                        //Για να επιτευχθούν οι δύο στόχοι πρέπει να δημιουργηθεί ένα πλάνο που μόνο μερικά είδη της ενότητας Β να προηγηθούν της ενότητας A.
                        //Αυτό γίνεται δημιουργώντας μια εντολή ανταλλαγή θέσης μεταξύ ειδών «PositionInterchange»
                        //Invariants
                        //Αυτή η διαδικασία μπορεί να λάβει χώρα μόνο όταν όλα τα είδη της Α ενότητας είναι σε κατάσταση αναμονής για προετοιμασίας ή προηγούμενη.
                        //Αυτή η διαδικασία δε μπορεί να οδηγήσει την Α ενότητα να είναι έτοιμη για σερβίρισμα σε χρόνο μεταγενέστερο από ότι πριν.

                        #endregion

                        var itemToPrepareItemsPreparationContext = itemToPrepare.FindItemsPreparationContext();

                        if (!itemToPrepare.IsInReferenceItemsPreparationContext(actionContext) && itemToPrepareItemsPreparationContext.PreparationItems.All(x => x.State.IsInTheSameOrPreviousState(ItemPreparationState.PendingPreparation)))
                        {
                            //there isn't reason to interchange position of item that belong to the reference items preparation context
                            //All items of itemsPreparationContext must be is in the same or previousState of PendingPreparation

                            if (itemToPrepare != itemsPendingToPrepare.Last())
                            {
                                ItemPreparation nextItemToPrepare = itemsPendingToPrepare[itemsPendingToPrepare.IndexOf(itemToPrepare) + 1];
                                if (nextItemToPrepare.FindItemsPreparationContext() != itemToPrepareItemsPreparationContext)
                                {
                                    if (nextItemToPrepare.IsInReferenceItemsPreparationContext(actionContext))
                                    {
                                        var itemMealCoursePreparationEndTimeForecast = itemToPrepare.MealCourse.GetPreparationForecast(actionContext);
                                        //Defines the time where the meal course will be ready to serve. Depends from the reference ItemsPreparationContext.

                                        DateTime? lastItemPreparationEndTime = itemToPrepare.FindItemsPreparationContext().PreparationItems.Select(x => actionContext.GetPreparationEndsAt(x as ItemPreparation)).OrderBy(x => x).LastOrDefault();
                                        //Defines the time where the last item of ItemsPreparationContext (which belong the itemToPrepare), can be prepared

                                        if (lastItemPreparationEndTime.HasValue)
                                        {
                                            bool movingItemPreparationEndsAfterTheLast = actionContext.GetPreparationEndsAt(nextItemToPrepare) > lastItemPreparationEndTime.Value;
                                            TimeSpan timeSpan = TimeSpan.FromSeconds(30);
                                            if (movingItemPreparationEndsAfterTheLast)
                                                timeSpan -= TimeSpan.FromMinutes(preparationStation.GetPreparationTimeSpanInMin(nextItemToPrepare) * 0.9); //timeSpan with item moving up

                                            //The time that the mealCourse will be ready to serve should not be longer after the item moving up


                                            if (timeSpan.TotalMinutes > 0)
                                            {
                                                //Creates a position change command.
                                                pendingItemsRearrange = true;
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
                        #endregion

                    }

                }
                while (pendingItemsRearrange && pendingItemsRearrangements < itemsPendingToPrepare.Count);

                //var strings = preparationStation.GetActionsToStrings(actionContext);
            }


        }
        internal static bool IsInReferenceItemsPreparationContext(this ItemPreparation itemPreparation, PreparationPlan actionContext)
        {
            return itemPreparation.MealCourse.GetReferenceItemsPreparationContext(actionContext) == itemPreparation.FindItemsPreparationContext();
        }
        internal static List<string> GetActionsToStrings(this PreparationStation preparationStation, PreparationPlan actionContext)
        {
            if (!actionContext.PreparationSections.ContainsKey(preparationStation))
                return (from preparationSection in preparationStation.FoodItemsInProgress
                        orderby preparationSection.MealCourseStartsAt
                        select preparationSection.ToString()).ToList();

            List<string> strings =
            (from preparationSection in actionContext.PreparationSections[preparationStation]
             orderby preparationSection.GetPreparationStartsAt(actionContext)
             select preparationSection.TotString(actionContext)).ToList();

            return strings;
        }
        static string TotString(this ItemsPreparationContext preparationSection, PreparationPlan actionContext)
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
            var itemsToPrepare = (from preparationItem in preparationSection.PreparationItems.OrderBy(x => x.AppearanceOrder)
                                  where preparationItem.State.IsIntheSameOrFollowingState(ItemPreparationState.PreparationDelay) &&
                                  preparationItem.State.IsInPreviousState(ItemPreparationState.IsRoasting)
                                  select preparationItem).OfType<ItemPreparation>().ToList();

            return itemsToPrepare;
        }

        public static void ActionsOrderCommitted(this PreparationStation preparationStation, PreparationPlan actionContext)
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
        public static List<ItemsPreparationContext> GetToDoPreparationSections(this PreparationStation preparationStation, PreparationPlan actionContext)
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

                    if ((preparationSection.MealCourse.GetPreparationForecast(actionContext) - preparationSection.GetPreparationForecast(actionContext)).GetTotalMinutes() < 1.5 * Simulator.Velocity)
                    {
                    }

                    filteredPartialActions.Add(preparationSection);
                }
                else if (preparationSection.PreparationItems.Any(x => x.State > FlavourBusinessFacade.RoomService.ItemPreparationState.InPreparation))
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
            return itemsPreparationContext.PreparationItems.OfType<ItemPreparation>().Sum(x => (x.PreparationStation as PreparationStation).GetPreparationTimeSpanInMin(x));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="preparationStation"></param>
        /// <param name="actionContext"></param>
        internal static void GetPreparationSections(this PreparationStation preparationStation, PreparationPlan actionContext)
        {

            if (preparationStation.FoodItemsInProgress.Count == 0)
                preparationStation.PreparationPlanStartTime = null;

            actionContext.PreparationSections[preparationStation] = preparationStation.FoodItemsInProgress;
        }
        internal static void OptimizePreparationPlan(this PreparationStation preparationStation, PreparationPlan actionContext, bool stirTheSequence)
        {


            List<ItemsPreparationContext> preparationSectionsForOptimazation = null;
            var preparationSections = actionContext.PreparationSections[preparationStation];
            List<ItemsPreparationContext> orderCommittedPreparationSections = preparationSections.Where(x => x.PreparationOrderCommitted).OrderBy(x => x.PreparationOrder).ToList();
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
                        if (preparationSection.MealCourse.GetLastPlanReferencePreparationSection(actionContext) != preparationSection.MealCourse.GetReferenceItemsPreparationContext(actionContext) && // the reference preparation section of meal course has change
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
            //        preparationSection.PreparationOrder = i++;

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
                        preparationSection.PreparationOrder = i++;
                    else
                        i = preparationSection.PreparationOrder + 1;

                    if (preparationSectionsToCommit.Contains(preparationSection))
                        preparationSection.PreparationOrderCommitted = true;
                }
                stateTransition.Consistent = true;
            }
            #endregion

            #region Debug

            var previous = preparationStation.FoodItemsInProgress;
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

    /// <MetaDataID>{fafd0874-5846-48b8-97f1-a98bc41d350a}</MetaDataID>
    class ItemPreparationStartsFor
    {
        public readonly PreparationStation PreparationStation;
        public DateTime Starts;

        public ItemPreparationStartsFor(PreparationStation preparationStation)
        {
            PreparationStation = preparationStation;
        }
    }



}
