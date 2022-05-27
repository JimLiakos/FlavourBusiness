using OOAdvantech.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreparationStationDevice.WPF
{

    /// <summary>
    /// A meal consisting of multiple dishes (meal courses)
    /// Most Western-world multicourse meals follow a standard sequence.
    /// MealCourse class defines the food items where belongs to the same course 
    /// for instance hors d'oeuvre or appetizer,main dish , dessert
    /// </summary>
    /// <MetaDataID>{c268b82b-e7ec-4f75-aa0b-1923a81e934a}</MetaDataID>
    public class MealCourse
    {
        public override string ToString()
        {
            return this.GetDesription();
            //string subActions = " ";
            //foreach (var productionLine in Simulator.ProductionLines)
            //{
            //    if (this.ToDoSubActions.Any(x => x.ProductionLine == productionLine))
            //        subActions += productionLine.Name;
            //    else
            //        subActions += "-";

            //    subActions += "    ";
            //}


            //return string.Format("{0} {1} : {2}", Name, subActions, string.Format("{0:h:mm:ss tt}", PreparationForecast));
        }

        [JsonIgnore]
        public Simulator Simulator { get; internal set; }

        public readonly string Name;
        public MealCourse(string name, List<ItemsPreparationSession> preparationSessions, Simulator simulator)
        {
            Simulator = simulator;
            Name = name;
            _PreparationSessions = preparationSessions;

            foreach (var preparationSession in preparationSessions)
                preparationSession.MealCourse = this;
        }

        /// <exclude>Excluded</exclude>
        List<ItemsPreparationSession> _PreparationSessions;

        public List<ItemsPreparationSession> PreparationSessions
        {
            get
            {
                return _PreparationSessions.Where(x => x.PreparationItems.Count > 0).ToList();
            }
        }

        /// <summary>
        /// Preparation session to be done
        /// </summary>
        public List<ItemsPreparationSession> PendingPreparationSessions
        {
            get
            {
                //pending 
                return _PreparationSessions.Where(x => x.ItemsToPrepare.Count > 0).ToList();
            }
        }

        public DateTime? CompletionTime;



        public DateTime PreparationForecast
        {
            get
            {
                return PendingPreparationSessions.OrderBy(x => x.PreparationForecast).Last().PreparationForecast;
            }
        }
        public DateTime GetPreparationForecast(ActionContext context)
        {
            if (PendingPreparationSessions.Count != 0)
                return PendingPreparationSessions.OrderBy(x => x.GetPreparationForecast(context)).Last().GetPreparationForecast(context);
            else
            {
                if (PreparationSessions.Count == 0)
                    return DateTime.Now;
                return PreparationSessions.OrderBy(x => x.GetPreparationForecast(context)).Last().GetPreparationForecast(context);
            }
        }

        public DateTime GetPreparationStartForecast(ActionContext context)
        {
            return PendingPreparationSessions.OrderBy(x => x.GetPreparationForecast(context)).Last().GetPreparationForecast(context) - TimeSpanEx.FromMinutes(PendingPreparationSessions.OrderBy(x => x.GetPreparationForecast(context)).Last().Duration);
        }

        internal string ToString(ActionContext actionContext)
        {
            string subActions = " ";
            foreach (var productionLine in Simulator.ProductionLines)
            {
                if (this.PendingPreparationSessions.Any(x => x.ProductionLine == productionLine))
                    subActions += productionLine.Name;
                else
                    subActions += "-";

                subActions += "    ";
            }
            return string.Format("{0} {1} : {2}", Name, subActions, string.Format("{0:h:mm:ss tt}", GetPreparationForecast(actionContext)));
        }
    }

    /// <MetaDataID>{0294b45b-f0c7-4af8-9938-6d827dbecc83}</MetaDataID>
    public class ItemsPreparationSession
    {
        public ItemsPreparationSession()
        {

        }


        /// <summary>
        /// When PreparationOrderCommited is true the optimizer can't change the position of 
        /// this action in the preparation  sequence 
        /// </summary>
        public bool PreparationOrderCommited;


        public override string ToString()
        {
            return this.GetDesription();
            //return ProductionLine.Name + " " + MainAction.Name + " " + PreparationForecast.ToShortTimeString() + " " + MainAction.PreparationForecast.ToShortTimeString();
        }

        //public double Duration;
        [OOAdvantech.Json.JsonIgnore]
        public PreparationStation ProductionLine;

        public int ProductionLineIndex;

        List<ItemPreparation> _PreparationItems;

        public List<ItemPreparation> ItemsToPrepare
        {
            get
            {
                if (_PreparationItems != null)
                    return _PreparationItems.Where(x => x.State < FlavourBusinessFacade.RoomService.ItemPreparationState.IsRoasting).ToList();
                return _PreparationItems;
            }
        }
        public List<ItemPreparation> PreparationItems
        {
            get
            {
                if (_PreparationItems != null)
                    return _PreparationItems.ToList();
                return _PreparationItems;
            }
            set
            {
                if (value != null)
                {
                    foreach (var actionSlot in value)
                        actionSlot.PreparationSession = this;
                }
                _PreparationItems = value;
            }
        }

        public DateTime PreparationForecast
        {
            get
            {
                return PreparationItems.OrderBy(x => x.PreparationEnds).Last().PreparationEnds;
            }
        }
        [JsonIgnore]
        public MealCourse MealCourse { get; set; }
        public double Duration
        {
            get
            {
                return PreparationItems.Sum(x => x.ActiveDuration);
            }
        }
        public DateTime? CompletionTime;

        internal int PreparatioOrder { get; set; } = -1;

        public string ToString(ActionContext actionContext)
        {

            var preparationForecast = GetPreparationForecast(actionContext);

            var preparationStartsAt = GetPreparationStartsAt(actionContext);

            return ProductionLine.Name + " " + MealCourse.Name + " " + string.Format("{0:h:mm:ss tt}", preparationStartsAt) + " " + " " + string.Format("{0:h:mm:ss tt}", preparationForecast) + " " + string.Format("{0:h:mm:ss tt}", MealCourse.GetPreparationForecast(actionContext)) + " slots : " + PreparationItems.Count.ToString() + " v:" + ProductionLine?.Velocity;
        }

        public DateTime GetPreparationForecast(ActionContext actionContext)
        {
            if (actionContext.ProductionLineActions.ContainsKey(this.ProductionLine))
                return actionContext.GetPreparationEndsAt(PreparationItems.OrderBy(x => actionContext.GetPreparationEndsAt(x)).Last()).Value;
            else
                return PreparationForecast;
        }
        public DateTime GetPreparationStartsAt(ActionContext actionContext)
        {
            if (actionContext.ProductionLineActions.ContainsKey(this.ProductionLine) && ItemsToPrepare.Count > 0)
                return actionContext.GetPreparationStartsAt(ItemsToPrepare.OrderBy(x => actionContext.GetPreparationEndsAt(x)).First()).Value;
            else

                return PreparationItems.OrderBy(x => x.PreparationStart).First().PreparationStart;
        }
    }
    /// <MetaDataID>{2db71939-c9b3-4834-b4b0-64606f034697}</MetaDataID>
    public class ItemPreparation
    {

        public string Name;
        double _Duration;
        public double Duration
        {
            get => _Duration;
            set
            {
                _Duration = value * Simulator.Velocity;
            }
        }

        double _PackingTime;
        public double PackingTime
        {
            get => _PackingTime;
            set
            {
                _PackingTime = value * Simulator.Velocity;
            }
        }
        public double ActiveDuration
        {
            get
            {
                if (ProductionLine != null)
                    return this.ProductionLine.GetDuration(this);
                return Duration;
            }
        }
        public DateTime PreparationStart { get; internal set; }

        public DateTime PreparationEnds
        {
            get
            {
                return PreparationStart + TimeSpanEx.FromMinutes(Duration);
            }
        }
        public FlavourBusinessFacade.RoomService.ItemPreparationState State { get; set; }
        [JsonIgnore]
        public ItemsPreparationSession PreparationSession { get; internal set; }

        [JsonIgnore]
        public PreparationStation ProductionLine { get; private set; }

        internal ItemPreparation CopyFor(PreparationStation productionLine)
        {
            return new ItemPreparation() { Name = Name, Duration = Duration, PackingTime = PackingTime, ProductionLine = productionLine, State = FlavourBusinessFacade.RoomService.ItemPreparationState.PreparationDelay };
        }

    }

    /// <MetaDataID>{af9d458c-141a-40ca-9a4f-693deb6676bd}</MetaDataID>
    public class PreparationStation
    {
        public PreparationStation(string name, Simulator simulator)
        {
            Name = name;
            Simulator = simulator;
        }
        Simulator Simulator;
        public List<ItemsPreparationSession> PreparationSessions
        {
            get
            {
                return (from mealCourse in this.Simulator.MealCourses
                        from partialAction in mealCourse.PendingPreparationSessions
                        where partialAction.ProductionLine == this
                        select partialAction).ToList();
            }
        }

        public string Name { get; }

        internal void GetPredictions()
        {

            if (PreviousePreparationEndsAt == null)
                PreviousePreparationEndsAt = DateTime.UtcNow;
            var previousePreparationEndsAt = PreviousePreparationEndsAt.Value;
            var slots = (from action in this.Simulator.MealCourses
                         from partialAction in action.PendingPreparationSessions
                         where partialAction.ProductionLine == this
                         from slot in partialAction.ItemsToPrepare
                         select slot).ToList();

            foreach (var slot in slots)
            {
                slot.PreparationStart = previousePreparationEndsAt;
                previousePreparationEndsAt = slot.PreparationStart + TimeSpanEx.FromMinutes(slot.ActiveDuration);
            }

        }
        DateTime? PreviousePreparationEndsAt;
        internal void GetPredictions(ActionContext actionContext)
        {

            if (PreviousePreparationEndsAt == null)
                PreviousePreparationEndsAt = DateTime.UtcNow;
            var previousePreparationEndsAt = PreviousePreparationEndsAt.Value;
            if (actionContext.ProductionLineActions.ContainsKey(this))
            {
                var slots = (from thePartialAction in actionContext.ProductionLineActions[this]
                             where thePartialAction.ProductionLine == this
                             from slot in thePartialAction.ItemsToPrepare
                             select slot).ToList();
                ItemsPreparationSession partialAction = null;
                double packingTime = 0;
                foreach (var slot in slots)
                {
                    if (slot.PreparationSession != partialAction)
                    {
                        if (packingTime != 0)
                            previousePreparationEndsAt = previousePreparationEndsAt + TimeSpanEx.FromMinutes(packingTime);
                        packingTime = 0;
                        partialAction = slot.PreparationSession;
                    }

                    packingTime += slot.PackingTime;

                    if (actionContext.GetPreparationStartsAt(slot) == null || actionContext.GetPreparationStartsAt(slot).Value != previousePreparationEndsAt)
                        actionContext.PreparationPlanIsDoubleChecked = false;
                    actionContext.SetPreparationStartsAt(slot, previousePreparationEndsAt);
                    previousePreparationEndsAt = previousePreparationEndsAt + TimeSpanEx.FromMinutes(slot.ActiveDuration);
                }
            }

            if (GetActionsToDo(actionContext).Count == 0)
                PreviousePreparationEndsAt = DateTime.UtcNow;

        }
        public List<string> GetActionsToStrings(ActionContext actionContext)
        {
            if (!actionContext.ProductionLineActions.ContainsKey(this))
                return (from partialAction in this.PreparationSessions
                        select partialAction.ToString()).ToList();

            List<string> strings =
            (from partialAction in actionContext.ProductionLineActions[this]
             select partialAction.ToString(actionContext)).ToList();

            return strings;
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="firstTime"></param>
        internal void OptimizePreparationPlan(ActionContext actionContext, bool stirTheSequence)
        {


            List<ItemsPreparationSession> PreparationSessionsForOptimazation = null;
            if (stirTheSequence)
            {
                // first takes the uncommitted  items preparation contexts where the meal course has all items preparation contexts uncommitted 
                PreparationSessionsForOptimazation = PreparationSessions.Where(x => !x.PreparationOrderCommited).OrderBy(x => x.MealCourse.GetPreparationForecast(actionContext)).Where(x => x.MealCourse.PreparationSessions.All(y => !y.PreparationOrderCommited)).ToList();

                var a_count = PreparationSessionsForOptimazation.Count;

                //in the sequel takes the uncommitted  items preparation contexts where the meal course has at least one items preparation contexts committed 
                PreparationSessionsForOptimazation.AddRange(PreparationSessions.Where(x => !x.PreparationOrderCommited).OrderBy(x => x.MealCourse.GetPreparationForecast(actionContext)).Where(x => x.MealCourse.PreparationSessions.Any(y => y.PreparationOrderCommited)).ToList());

                //preparation contexts order by the preparation forecast time of meal course where this belongs   
                var b_count = PreparationSessionsForOptimazation.Count;
                if (a_count != b_count)
                {

                }
            }
            else
            {
                //preparation contexts order by the preparation forecast time of meal course where this belongs
                PreparationSessionsForOptimazation = PreparationSessions.Where(x => !x.PreparationOrderCommited).OrderBy(x => x.MealCourse.GetPreparationForecast(actionContext)).ToList();
            }



            //preparation order committed PreparationSessions
            List<ItemsPreparationSession> orderCommittedreparationContexts = PreparationSessions.Where(x => x.PreparationOrderCommited).OrderBy(x => x.PreparatioOrder).ToList();

            //Adds the re planed preparation contexts 
            orderCommittedreparationContexts.AddRange(PreparationSessionsForOptimazation);
            List<ItemsPreparationSession> actions = orderCommittedreparationContexts;


            //List<PartialAction> actions = Actions.OrderBy(x => x.MainAction.GetPreparationForecast(actionContext)).ToList();

            int i = 0;
            foreach (var partialAction in actions)
                partialAction.PreparatioOrder = i++;


#if DEBUG
            List<ItemsPreparationSession> contextActions = new List<ItemsPreparationSession>();
            if (actionContext.ProductionLineActions.ContainsKey(this))
                contextActions = actionContext.ProductionLineActions[this];
            if (!Simulator.CompareActionsSets(actions, contextActions))
            {
                var actionStrings = Simulator.GetActionsToStrings(actionContext, actions);
                var actionContextActionStrings = Simulator.GetActionsToStrings(actionContext, contextActions);
            }
#endif

            actionContext.ProductionLineActions[this] = actions;

        }


        DateTime? LastChangeDateTime;

        internal void Run(ActionContext actionContext)
        {
            List<ItemsPreparationSession> filteredPartialActions = GetActionsToDo(actionContext);
            var strings = GetActionsToStrings(actionContext);
            var slots = (from partialAction in filteredPartialActions
                         from slot in partialAction.ItemsToPrepare
                         select slot).ToList();

            if (slots.Count > 3)
                Velocity = 0.2;
            else
                Velocity = 0;



            if (LastChangeDateTime == null && slots.Count > 0)
            {
                LastChangeDateTime = DateTime.UtcNow;
                slots[0].State = FlavourBusinessFacade.RoomService.ItemPreparationState.ΙnPreparation;
                var date = actionContext.GetPreparationEndsAt(slots[0]);
                var action = PreparationSessions.Where(x => x.ItemsToPrepare.Any(y => y == slots[0])).First();
                var tt = action.GetPreparationForecast(actionContext);
                var astrings = Simulator.MealCourses.OrderBy(x => x.GetPreparationForecast(actionContext)).Select(x => x.GetDesription(actionContext)).ToList();
            }
            else if (slots.Count > 0)
            {
                while (slots.Count > 0 && actionContext.GetPreparationEndsAt(slots[0]) < DateTime.UtcNow)
                {
                    PreviousePreparationEndsAt = DateTime.UtcNow;
                    slots[0].State = FlavourBusinessFacade.RoomService.ItemPreparationState.IsPrepared;

                    if (slots[0].PreparationSession != null && slots[0].PreparationSession.ItemsToPrepare.Count == 0)
                    {
                        slots[0].PreparationSession.CompletionTime = DateTime.UtcNow;
                        if (slots[0].PreparationSession.MealCourse.PreparationSessions.All(x => x.CompletionTime != null))
                            slots[0].PreparationSession.MealCourse.CompletionTime = slots[0].PreparationSession.CompletionTime;

                        var packingTime = slots[0].PreparationSession.PreparationItems.Sum(x => x.PackingTime);
                        PreviousePreparationEndsAt += TimeSpanEx.FromMinutes(packingTime);
                    }
                    slots.RemoveAt(0);
                }

                //var astrings = Simulator.Actions.OrderBy(x => x.GetPreparationForecast(actionContext)).Select(x => x.ToString(actionContext)).ToList();
            }
        }

        public List<string> GetActionsToDoStrings(ActionContext actionContext)
        {
            List<String> actionStrings = new List<string>();
            var actions = PreparationSessions;//.Where(x=>(x.Action.PreparationForecast-TimeSpanEx.FromMinutes(x.Duration+5))<DateTime.UtcNow).ToList();
            if (actionContext.ProductionLineActions.ContainsKey(this))
                actions = actionContext.ProductionLineActions[this];

            List<ItemsPreparationSession> filteredPartialActions = GetActionsToDo(actionContext);
            foreach (var partialAction in actions)
            {
                if (filteredPartialActions.Contains(partialAction))
                    actionStrings.Add(partialAction.ToString(actionContext));
                else if (partialAction.PreparationItems.Any(x => x.State > FlavourBusinessFacade.RoomService.ItemPreparationState.ΙnPreparation))
                    actionStrings.Add(partialAction.ToString(actionContext));
                else
                {
                    if (partialAction.PreparationOrderCommited)
                    {

                    }
                    if (partialAction == partialAction.MealCourse.PreparationSessions.OrderBy(x => x.GetPreparationForecast(actionContext)).Last())
                    {

                    }

                    actionStrings.Add("X-" + partialAction.MealCourse.PreparationSessions.OrderBy(x => x.GetPreparationForecast(actionContext)).Last().ProductionLine.Name + "-" + partialAction.ToString(actionContext));
                }

            }

            return actionStrings;
        }


        public void ActionsOrderCommited(ActionContext actionContext)
        {
            foreach (var partialAction in GetActionsToDo(actionContext))
            {
                partialAction.PreparationOrderCommited = true;

                foreach (var preparationItem in partialAction.PreparationItems)
                {
                    if (preparationItem.State == FlavourBusinessFacade.RoomService.ItemPreparationState.PreparationDelay)
                        preparationItem.State = FlavourBusinessFacade.RoomService.ItemPreparationState.PendingPreparation;
                }
            }

        }

        public List<ItemsPreparationSession> GetActionsToDo(ActionContext actionContext)
        {


            var actions = PreparationSessions;//.Where(x=>(x.Action.PreparationForecast-TimeSpanEx.FromMinutes(x.Duration+5))<DateTime.UtcNow).ToList();
            if (actionContext.ProductionLineActions.ContainsKey(this))
                actions = actionContext.ProductionLineActions[this];
            List<ItemsPreparationSession> filteredPartialActions = new List<ItemsPreparationSession>();
            foreach (var partialAction in actions)
            {
                if ((partialAction.MealCourse.GetPreparationForecast(actionContext) - partialAction.GetPreparationForecast(actionContext)).GetTotalMinutes() < 1.5 * Simulator.Velocity || ((partialAction.MealCourse.GetPreparationForecast(actionContext) - TimeSpanEx.FromMinutes(partialAction.Duration + (2 * Simulator.Velocity))) < DateTime.UtcNow))
                {

                    if (partialAction.MealCourse.Name == "a11")
                    {

                    }
                    if ((partialAction.MealCourse.GetPreparationForecast(actionContext) - partialAction.GetPreparationForecast(actionContext)).GetTotalMinutes() < 1.5 * Simulator.Velocity)
                    {

                    }
                    //foreach (var actionSlot in partialAction.PreparationItems)
                    //{
                    //    if (actionSlot.State == FlavourBusinessFacade.RoomService.ItemPreparationState.PreparationDelay)
                    //        actionSlot.State = FlavourBusinessFacade.RoomService.ItemPreparationState.PendingPreparation;
                    //}

                    filteredPartialActions.Add(partialAction);
                }
                else if (partialAction.PreparationItems.Any(x => x.State > FlavourBusinessFacade.RoomService.ItemPreparationState.ΙnPreparation))
                {

                    filteredPartialActions.Add(partialAction);
                }
                else
                {
                    if (partialAction.PreparationItems.All(x => x.State > FlavourBusinessFacade.RoomService.ItemPreparationState.PreparationDelay))
                        filteredPartialActions.Add(partialAction);
                    else
                    {
                        if (partialAction.PreparationOrderCommited)
                        {

                        }
                    }

                    break;
                }
            }




            return filteredPartialActions;
        }

        public double Velocity = 0;

        internal double GetDuration(ItemPreparation actionSlot)
        {
            if (Velocity != 0)
            {

            }
            return actionSlot.Duration * (1 + Velocity);
        }
    }



    /// <MetaDataID>{b2c2a203-e4e6-4a0a-aec6-5106d04618dd}</MetaDataID>
    public class Simulator
    {
        public static double Velocity = 0.33;

        static List<ItemPreparation> B_ProductionLineActionSlots = new List<ItemPreparation>(){
            new ItemPreparation() { Name = "Pizza margarita",Duration = 7,PackingTime=0.8 },
            new ItemPreparation() { Name = "Burger Americana",Duration = 8, PackingTime=0.2 },
            new ItemPreparation(){Name="Pizza D'adrea",Duration=7, PackingTime=0.8},
            new ItemPreparation(){Name="Chiken Burger",Duration=8 , PackingTime=0.2 },
            new ItemPreparation(){Name="Boloneze",Duration=8 , PackingTime=0.2 },
            new ItemPreparation(){Name="Amatritsian",Duration=7 , PackingTime=0.2 },
            new ItemPreparation(){Name="Pizza special",Duration=7, PackingTime=0.8},
            new ItemPreparation() { Name = "Carbonara", Duration = 8 , PackingTime=0.2 }
        };

        static List<ItemPreparation> A_ProductionLineActionSlots = new List<ItemPreparation>(){
            new ItemPreparation(){Name="Πίτα γύρο χειρινο",Duration=0.5 , PackingTime=0.08 },
            new ItemPreparation(){Name= "Πίτα γύρο κοτοπουλ", Duration=0.5 , PackingTime=0.08 },
            new ItemPreparation(){Name="Κοτομπεικον",Duration=4 , PackingTime=0.2 },
            new ItemPreparation(){Name= "Μεριδα γύρο κοτοπουλ", Duration=1 , PackingTime=0.15 },
            new ItemPreparation(){Name= "Μεριδα γύρο χειρινο", Duration=1 , PackingTime=0.15 },
        };
        static List<ItemPreparation> C_ProductionLineActionSlots = new List<ItemPreparation>()
        {
            new ItemPreparation(){Name= "Ριζοτο Alfredo ", Duration=6 , PackingTime=0.1 },
            new ItemPreparation() { Name = "Ριζοτο Μιλανέζε ", Duration = 6 , PackingTime=0.1 },
             new ItemPreparation() { Name = "Roast Beef ", Duration = 9 , PackingTime=0.1 }
        };

        static List<List<int>> ProductionLinesSlots = new List<List<int>>() {
            new List<int> {1, 2, 0 },
            new List<int> { 0, 0, 2 },
            new List<int> { 2, 1, 0 },
            new List<int> { 0, 3, 0 },
            new List<int> { 2, 1, 1 },
            new List<int> { 0, 2, 1 },
            new List<int> { 2, 1, 1 },
            new List<int> { 2, 0, 1 },
            new List<int> { 3, 0, 1 },
            new List<int> { 5, 0, 0 },
            new List<int> { 2, 0, 0 },
            new List<int> { 1, 0, 2 }
        };
        static Random _R = new Random();



        string Name;
        public Simulator(string name)
        {
            Name = name;
            ProductionLines = new List<PreparationStation>() { new PreparationStation("A", this), new PreparationStation("B", this), new PreparationStation("C", this) };
        }
        List<MealCourse> SavedCourses;

        public void start()
        {
            //SavedCourses = OOAdvantech.Json.JsonConvert.DeserializeObject<List<MealCourse>>(System.IO.File.ReadAllText(@"C:\Projects\simulator.json"));

            //foreach (var preparationSession in (from course in SavedCourses
            //                                    from session in course.PreparationSessions
            //                                    select session))
            //{
            //    preparationSession.ProductionLine = ProductionLines[preparationSession.ProductionLineIndex];
            //}
            Task.Run(() =>
            {
                int count = 1;
                List<List<string>> theAStrings = new List<List<string>>();

                List<Snapshot> productionLinesStrings = new List<Snapshot>();
                ActionContext actionContext = new ActionContext();
                try
                {
                    while (true)
                    {
                        if (count < 15)
                        {
                            if (AddAction(count))
                                count++;
                        }
                        else
                        {

                        }
                        //if (ActionsRepository.Count > 0)
                        //{
                        //    _Actions.Add(ActionsRepository[0]);
                        //    ActionsRepository.RemoveAt(0);
                        //}
                        //else
                        //{
                        //}



                        var _this = this;


                        

                        Dictionary<PreparationStation, List<ItemsPreparationSession>> productionLineActions = new Dictionary<PreparationStation, List<ItemsPreparationSession>>();
                        foreach (var productionLine in actionContext.ProductionLineActions.Keys.ToArray())
                            productionLineActions[productionLine] = actionContext.ProductionLineActions[productionLine].ToList();

                        RebuildPreparationPlan(actionContext);

                        foreach (var productionLine in ProductionLines)
                        {

                            if (productionLineActions.ContainsKey(productionLine) && !Simulator.CompareActionsSets(productionLineActions[productionLine], actionContext.ProductionLineActions[productionLine]))
                            {
                                var actionStrings = Simulator.GetActionsToStrings(actionContext, actionContext.ProductionLineActions[productionLine]);
                                var actionContextActionStrings = Simulator.GetActionsToStrings(actionContext, productionLineActions[productionLine]);
                            }
                        }


                        List<List<string>> currentProductionLinesActionStrings = new List<List<string>>();


                        foreach (var productionLine in ProductionLines)
                            currentProductionLinesActionStrings.Add(productionLine.GetActionsToDoStrings(actionContext));


                        if (productionLinesStrings.Count > 0)
                        {

                            Snapshot snapshot = new Snapshot() { Entry = currentProductionLinesActionStrings, TimeSpan = string.Format("{0:h:mm:ss tt}", DateTime.UtcNow) };

                            string k_json = OOAdvantech.Json.JsonConvert.SerializeObject(snapshot.Entry);
                            if (OOAdvantech.Json.JsonConvert.SerializeObject(productionLinesStrings.Last().Entry) != k_json)
                                productionLinesStrings.Add(snapshot);
                            else
                            {

                            }


                        }
                        else
                        {
                            Snapshot snapshot = new Snapshot() { Entry = currentProductionLinesActionStrings, TimeSpan = string.Format("{0:h:mm:ss tt}", DateTime.UtcNow) };
                            productionLinesStrings.Add(snapshot);
                        }

                        var astrings = MealCourses.OrderBy(x => x.GetPreparationForecast(actionContext)).Select(x => x.GetDesription(actionContext)).ToList();
                        theAStrings.Add(astrings);
                        if (count == 7)
                        {

                        }
                        foreach (var inProductionLine in ProductionLines)
                        {
                            var strings = inProductionLine.GetActionsToStrings(actionContext);
                        }

                        foreach (var productionLine in ProductionLines)
                        {
                            productionLine.Run(actionContext);
                        }

                        string json = OOAdvantech.Json.JsonConvert.SerializeObject(productionLinesStrings);

                        string json_a = OOAdvantech.Json.JsonConvert.SerializeObject((from action in MealCourses
                                                                                      where action.CompletionTime != null
                                                                                      orderby action.CompletionTime
                                                                                      select new ActionCompleted
                                                                                      {
                                                                                          ActionName = action.Name,
                                                                                          SubActionCompletionTime = action.PreparationSessions.Select(x => x.ProductionLine.Name + " " + string.Format("{0:h:mm:ss tt}", x.CompletionTime)).ToList()
                                                                                      }).ToList());

                        string mealCoursesCopyJson = OOAdvantech.Json.JsonConvert.SerializeObject(MealCoursesCopy);

                        if (productionLinesStrings.LastOrDefault() != null)
                        {
                            if (count >= 15 && productionLinesStrings.Last().Entry[0].Count == 0 && productionLinesStrings.Last().Entry[1].Count == 0 && productionLinesStrings.Last().Entry[2].Count == 0)
                            {

                                if (!System.IO.File.Exists(@"F:\PreparationData\" + Name + "_" + GetHashCode() + "_summary.json"))
                                    System.IO.File.WriteAllText(@"F:\PreparationData\" + Name + "_" + GetHashCode() + "_summary.json", json_a);

                                if (!System.IO.File.Exists(@"F:\PreparationData\" + Name +"_"+ GetHashCode() + "_data.json"))
                                    System.IO.File.WriteAllText(@"F:\PreparationData\" +  Name + "_" + GetHashCode() + "_data.json", json);

                            }
                        }

                        System.Threading.Thread.Sleep((int)TimeSpanEx.FromMinutes(0.2 * Velocity).TotalMilliseconds);

                    }

                }
                catch (Exception error)
                {

                    throw;
                }

            });
        }

        public static bool CompareActionsSets(List<ItemsPreparationSession> actions, List<ItemsPreparationSession> contextActions)
        {
            if (actions == contextActions)
            {

            }
            if (contextActions.Count == actions.Count)
            {
                foreach (var action in actions)
                {
                    if (contextActions.Contains(action))
                    {
                        if (actions.IndexOf(action) != contextActions.IndexOf(action))
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        public static List<string> GetActionsToStrings(ActionContext actionContext, List<ItemsPreparationSession> actions)
        {

            List<string> strings =
            (from partialAction in actions
             select partialAction.ToString(actionContext)).ToList();

            return strings;
        }

        DateTime LastActionAdded = DateTime.UtcNow;
        private bool AddAction(int actionID)
        {

            while (true)
            {

                //if (actionID < 5 && (DateTime.UtcNow - LastActionAdded).GetTotalMinutes() < 1.5)
                //    return false;


                if (actionID < 5 && (DateTime.UtcNow - LastActionAdded).GetTotalMinutes() < (2.5 * Simulator.Velocity))
                    return false;

                if (actionID >= 5 && actionID < 7 && (DateTime.UtcNow - LastActionAdded).GetTotalMinutes() < (1.5 * Simulator.Velocity))
                    return false;

                if (actionID >= 7 && actionID < 12 && (DateTime.UtcNow - LastActionAdded).GetTotalMinutes() < (0.8 * Simulator.Velocity))
                    return false;

                if (actionID >= 12 && (DateTime.UtcNow - LastActionAdded).GetTotalMinutes() < (1.2 * Simulator.Velocity))
                    return false;



                var patern = ProductionLinesSlots[_R.Next(ProductionLinesSlots.Count - 1)].ToList();

                List<ItemPreparation> a_productionLineActionSlots = new List<ItemPreparation>();
                while (patern[0] > 0)
                {
                    a_productionLineActionSlots.Add(A_ProductionLineActionSlots[_R.Next(A_ProductionLineActionSlots.Count - 1)].CopyFor(ProductionLines[0]));
                    patern[0] = patern[0] - 1;
                }

                List<ItemPreparation> b_productionLineActionSlots = new List<ItemPreparation>();
                while (patern[1] > 0)
                {
                    b_productionLineActionSlots.Add(B_ProductionLineActionSlots[_R.Next(B_ProductionLineActionSlots.Count - 1)].CopyFor(ProductionLines[1]));
                    patern[1] = patern[1] - 1;
                }
                List<ItemPreparation> c_productionLineActionSlots = new List<ItemPreparation>();
                while (patern[2] > 0)
                {
                    c_productionLineActionSlots.Add(C_ProductionLineActionSlots[_R.Next(C_ProductionLineActionSlots.Count - 1)].CopyFor(ProductionLines[1]));
                    patern[2] = patern[2] - 1;
                }

                List<ItemsPreparationSession> partialActions = new List<ItemsPreparationSession>();


                if (a_productionLineActionSlots.Count > 0)
                    partialActions.Add(new ItemsPreparationSession() { ProductionLine = ProductionLines[0], ProductionLineIndex = 0, PreparationItems = a_productionLineActionSlots }); ;

                if (b_productionLineActionSlots.Count > 0)
                    partialActions.Add(new ItemsPreparationSession() { ProductionLine = ProductionLines[1], ProductionLineIndex = 1, PreparationItems = b_productionLineActionSlots });

                if (c_productionLineActionSlots.Count > 0)
                    partialActions.Add(new ItemsPreparationSession() { ProductionLine = ProductionLines[2], ProductionLineIndex = 2, PreparationItems = c_productionLineActionSlots });
                if (partialActions.Count != 0)
                {



                    MealCourse action = new MealCourse("a" + actionID, partialActions, this);
                    //if (SavedCourses != null)
                    //{
                    //    action = SavedCourses[actionID - 1];
                    //    action.Simulator = this;
                    //}

                    _MealCourses.Add(action);

                    var actionCopy = OOAdvantech.Json.JsonConvert.DeserializeObject<MealCourse>(OOAdvantech.Json.JsonConvert.SerializeObject(action));

                    MealCoursesCopy.Add(actionCopy);
                    LastActionAdded = DateTime.UtcNow;
                    return true;
                }
            }
        }

        List<MealCourse> MealCoursesCopy = new List<MealCourse>();

        List<MealCourse> _MealCourses = new List<MealCourse>();
        public List<MealCourse> MealCourses
        {
            get
            {
                return _MealCourses.Where(x => x.PreparationSessions.Count > 0).ToList();
            }
        }
        public List<PreparationStation> ProductionLines;
        private void RebuildPreparationPlan(ActionContext actionContext)
        {
            actionContext.PreparationPlanIsDoubleChecked = false;
            bool stirTheSequence = true;
            while (!actionContext.PreparationPlanIsDoubleChecked)
            {
                actionContext.PreparationPlanIsDoubleChecked = true;
                foreach (var productionLine in ProductionLines)
                {
                    productionLine.OptimizePreparationPlan(actionContext, stirTheSequence);
                    productionLine.GetPredictions(actionContext);
                }
                stirTheSequence = false;
            }

            foreach (var productionLine in ProductionLines)
                productionLine.ActionsOrderCommited(actionContext);
        }

    }
    /// <MetaDataID>{7d5af78e-a3af-45b8-b6a4-48e9facded87}</MetaDataID>
    public class ActionContext
    {
        public Dictionary<PreparationStation, List<ItemsPreparationSession>> ProductionLineActions = new Dictionary<PreparationStation, List<ItemsPreparationSession>>();

        public DateTime? GetPreparationStartsAt(ItemPreparation actionSlot)
        {
            DateTime dateTime;
            if (SlotsPreparationStartsAt.TryGetValue(actionSlot, out dateTime))
                return dateTime;
            return actionSlot.PreparationStart;
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
            if (actionSlot.ActiveDuration != actionSlot.Duration)
            {

            }


            DateTime dateTime;
            if (SlotsPreparationStartsAt.TryGetValue(actionSlot, out dateTime))
                return dateTime + TimeSpanEx.FromMinutes(actionSlot.ActiveDuration);

            return actionSlot.PreparationStart + TimeSpanEx.FromMinutes(actionSlot.ActiveDuration);
        }
        Dictionary<ItemPreparation, DateTime> SlotsPreparationStartsAt = new Dictionary<ItemPreparation, DateTime>();

        //        Dictionary<ActionSlot, DateTime> SlotsPreparationEndsAt = new Dictionary<ActionSlot, DateTime>();

        public bool PreparationPlanIsDoubleChecked { get; internal set; }
    }

    class Snapshot
    {
        public string TimeSpan;
        public List<List<string>> Entry;
    }

    class ActionCompleted
    {
        public List<string> SubActionCompletionTime;

        public string ActionName;
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

    static class ActionsDescription
    {
        public static string GetDesription(this MealCourse mealCourse)
        {
            string subActions = " ";
            foreach (var productionLine in mealCourse.Simulator.ProductionLines)
            {
                if (mealCourse.PendingPreparationSessions.Any(x => x.ProductionLine == productionLine))
                    subActions += productionLine.Name;
                else
                    subActions += "-";
                subActions += "    ";
            }
            return string.Format("{0} {1} : {2}", mealCourse.Name, subActions, string.Format("{0:h:mm:ss tt}", mealCourse.PreparationForecast));
        }

        public static string GetDesription(this MealCourse mealCourse, ActionContext actionContext)
        {
            string subActions = " ";
            foreach (var productionLine in mealCourse.Simulator.ProductionLines)
            {
                if (mealCourse.PendingPreparationSessions.Any(x => x.ProductionLine == productionLine))
                    subActions += productionLine.Name;
                else
                    subActions += "-";

                subActions += "    ";
            }
            return string.Format("{0} {1} : {2}", mealCourse.Name, subActions, string.Format("{0:h:mm:ss tt}", mealCourse.GetPreparationForecast(actionContext)));
        }

        public static string GetDesription(this ItemsPreparationSession itemsPreparationSession)
        {

            return itemsPreparationSession.ProductionLine.Name + " " + itemsPreparationSession.MealCourse.Name + " " + itemsPreparationSession.PreparationForecast.ToShortTimeString() + " " + itemsPreparationSession.MealCourse.PreparationForecast.ToShortTimeString();

        }

        public static string GetDesription(this ItemsPreparationSession itemsPreparationSession, ActionContext actionContext)
        {
            var preparationForecast = itemsPreparationSession.GetPreparationForecast(actionContext);

            var preparationStartsAt = itemsPreparationSession.GetPreparationStartsAt(actionContext);

            return itemsPreparationSession.ProductionLine.Name + " " + itemsPreparationSession.MealCourse.Name + " " + string.Format("{0:h:mm:ss tt}", preparationStartsAt) + " " + " " + string.Format("{0:h:mm:ss tt}", preparationForecast) + " " + string.Format("{0:h:mm:ss tt}", itemsPreparationSession.MealCourse.GetPreparationForecast(actionContext)) + " slots : " + itemsPreparationSession.PreparationItems.Count.ToString() + " v:" + itemsPreparationSession.ProductionLine?.Velocity;

        }
    }

}
