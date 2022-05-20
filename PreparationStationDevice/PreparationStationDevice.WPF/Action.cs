using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreparationStationDevice.WPF
{
    /// <MetaDataID>{c268b82b-e7ec-4f75-aa0b-1923a81e934a}</MetaDataID>
    public class Action
    {
        public override string ToString()
        {
            string subActions = " ";
            foreach (var productionLine in Simulator.ProductionLines)
            {
                if (this.ToDoSubActions.Any(x => x.ProductionLine == productionLine))
                    subActions += productionLine.Name;
                else
                    subActions += "-";

                subActions += "    ";
            }


            return string.Format("{0} {1} : {2}", Name, subActions, string.Format("{0:h:mm:ss tt}", PreparationForecast));
        }

        public Simulator Simulator { get; private set; }

        public readonly string Name;
        public Action(string name, List<PartialAction> subActions, Simulator simulator)
        {
            Simulator = simulator;
            Name = name;
            _SubActions = subActions;

            foreach (var subAction in subActions)
                subAction.MainAction = this;
        }
        List<PartialAction> _SubActions;
        public List<PartialAction> ToDoSubActions
        {
            get
            {
                return _SubActions.Where(x => x.ToDoSlots.Count > 0).ToList();
            }
        }

        public DateTime? CompletionTime;

        public List<PartialAction> SubActions
        {
            get
            {
                return _SubActions.Where(x => x.Slots.Count > 0).ToList();
            }
        }

        public DateTime PreparationForecast
        {
            get
            {
                return ToDoSubActions.OrderBy(x => x.PreparationForecast).Last().PreparationForecast;
            }
        }
        public DateTime GetPreparationForecast(ActionContext context)
        {
            if (ToDoSubActions.Count != 0)
                return ToDoSubActions.OrderBy(x => x.GetPreparationForecast(context)).Last().GetPreparationForecast(context);
            else
            {
                if (SubActions.Count == 0)
                    return DateTime.Now;
                return SubActions.OrderBy(x => x.GetPreparationForecast(context)).Last().GetPreparationForecast(context);
            }
        }

        public DateTime GetPreparationStartForecast(ActionContext context)
        {
            return ToDoSubActions.OrderBy(x => x.GetPreparationForecast(context)).Last().GetPreparationForecast(context) - TimeSpanEx.FromMinutes(ToDoSubActions.OrderBy(x => x.GetPreparationForecast(context)).Last().Duration);
        }

        internal string ToString(ActionContext actionContext)
        {
            string subActions = " ";
            foreach (var productionLine in Simulator.ProductionLines)
            {
                if (this.ToDoSubActions.Any(x => x.ProductionLine == productionLine))
                    subActions += productionLine.Name;
                else
                    subActions += "-";

                subActions += "    ";
            }
            return string.Format("{0} {1} : {2}", Name, subActions, string.Format("{0:h:mm:ss tt}", GetPreparationForecast(actionContext)));
        }
    }

    /// <MetaDataID>{0294b45b-f0c7-4af8-9938-6d827dbecc83}</MetaDataID>
    public class PartialAction
    {
        public PartialAction()
        {

        }

        public bool ActionOrderCommited;


        public override string ToString()
        {
            return ProductionLine.Name + " " + MainAction.Name + " " + PreparationForecast.ToShortTimeString() + " " + MainAction.PreparationForecast.ToShortTimeString();
        }

        //public double Duration;

        public ProductionLine ProductionLine;

        List<ActionSlot> _Slots;

        public List<ActionSlot> ToDoSlots
        {
            get
            {
                if (_Slots != null)
                    return _Slots.Where(x => x.State < FlavourBusinessFacade.RoomService.ItemPreparationState.IsRoasting).ToList();
                return _Slots;
            }
        }
        public List<ActionSlot> Slots
        {
            get
            {
                if (_Slots != null)
                    return _Slots.ToList();
                return _Slots;
            }
            set
            {
                if (value != null)
                {
                    foreach (var actionSlot in value)
                        actionSlot.PartialAction = this;
                }
                _Slots = value;
            }
        }

        public DateTime PreparationForecast
        {
            get
            {
                return Slots.OrderBy(x => x.PreparationEnds).Last().PreparationEnds;
            }
        }

        public Action MainAction { get; set; }
        public double Duration
        {
            get
            {
                return Slots.Sum(x => x.ActiveDuration);
            }
        }
        public DateTime? CompletionTime;

        public int ActionOrder { get; internal set; } = -1;

        public string ToString(ActionContext actionContext)
        {

            var preparationForecast = GetPreparationForecast(actionContext);

            var preparationStartsAt = GetPreparationStartsAt(actionContext);

            return ProductionLine.Name + " " + MainAction.Name + " " + string.Format("{0:h:mm:ss tt}", preparationStartsAt) + " " + " " + string.Format("{0:h:mm:ss tt}", preparationForecast) + " " + string.Format("{0:h:mm:ss tt}", MainAction.GetPreparationForecast(actionContext)) + " slots : " + Slots.Count.ToString() + " v:" + ProductionLine?.Velocity;
        }

        public DateTime GetPreparationForecast(ActionContext actionContext)
        {
            if (actionContext.ProductionLineActions.ContainsKey(this.ProductionLine))
                return actionContext.GetPreparationEndsAt(Slots.OrderBy(x => actionContext.GetPreparationEndsAt(x)).Last()).Value;
            else
                return PreparationForecast;
        }
        public DateTime GetPreparationStartsAt(ActionContext actionContext)
        {
            if (actionContext.ProductionLineActions.ContainsKey(this.ProductionLine) && ToDoSlots.Count > 0)
                return actionContext.GetPreparationStartsAt(ToDoSlots.OrderBy(x => actionContext.GetPreparationEndsAt(x)).First()).Value;
            else

                return Slots.OrderBy(x => x.PreparationStart).First().PreparationStart;
        }
    }
    /// <MetaDataID>{2db71939-c9b3-4834-b4b0-64606f034697}</MetaDataID>
    public class ActionSlot
    {

        public string Name;
        double _Duration;
        public double Duration
        {
            get => _Duration;
            set
            {
                _Duration = value*Simulator.Velocity;
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
        public PartialAction PartialAction { get; internal set; }
        public ProductionLine ProductionLine { get; private set; }

        internal ActionSlot CopyFor(ProductionLine productionLine)
        {
            return new ActionSlot() { Name = Name, Duration = Duration, PackingTime = PackingTime, ProductionLine = productionLine, State = FlavourBusinessFacade.RoomService.ItemPreparationState.PreparationDelay };
        }

    }

    /// <MetaDataID>{af9d458c-141a-40ca-9a4f-693deb6676bd}</MetaDataID>
    public class ProductionLine
    {
        public ProductionLine(string name, Simulator simulator)
        {
            Name = name;
            Simulator = simulator;
        }
        Simulator Simulator;
        public List<PartialAction> Actions
        {
            get
            {
                return (from action in this.Simulator.Actions
                        from partialAction in action.ToDoSubActions
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
            var slots = (from action in this.Simulator.Actions
                         from partialAction in action.ToDoSubActions
                         where partialAction.ProductionLine == this
                         from slot in partialAction.ToDoSlots
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
                             from slot in thePartialAction.ToDoSlots
                             select slot).ToList();
                PartialAction partialAction = null;
                double packingTime = 0;
                foreach (var slot in slots)
                {
                    if (slot.PartialAction != partialAction)
                    {
                        if (packingTime != 0)
                            previousePreparationEndsAt = previousePreparationEndsAt + TimeSpanEx.FromMinutes(packingTime);
                        packingTime = 0;
                        partialAction = slot.PartialAction;
                    }

                    packingTime += slot.PackingTime;

                    if (actionContext.GetPreparationStartsAt(slot) == null || actionContext.GetPreparationStartsAt(slot).Value != previousePreparationEndsAt)
                        actionContext.DoubleCheckOptimazation = false;
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
                return (from partialAction in this.Actions
                        select partialAction.ToString()).ToList();

            List<string> strings =
            (from partialAction in actionContext.ProductionLineActions[this]
             select partialAction.ToString(actionContext)).ToList();

            return strings;
        }





        internal void OptimizeActions(ActionContext actionContext, bool firstTime)
        {


            List<PartialAction> actionsForOptimazation = null;
            if (firstTime)
            {
                actionsForOptimazation = Actions.Where(x => !x.ActionOrderCommited).OrderBy(x => x.MainAction.GetPreparationForecast(actionContext)).Where(x => x.MainAction.SubActions.All(y => !y.ActionOrderCommited)).ToList();
                var a_count = actionsForOptimazation.Count;
                actionsForOptimazation.AddRange(Actions.Where(x => !x.ActionOrderCommited).OrderBy(x => x.MainAction.GetPreparationForecast(actionContext)).Where(x => x.MainAction.SubActions.Any(y => y.ActionOrderCommited)).ToList());
                var b_count = actionsForOptimazation.Count;
                if (a_count != b_count)
                {

                }
            }
            else
                actionsForOptimazation = Actions.Where(x => !x.ActionOrderCommited).OrderBy(x => x.MainAction.GetPreparationForecast(actionContext)).ToList();

            
            List<PartialAction> optimazedActions = Actions.Where(x => x.ActionOrderCommited).OrderBy(x=>x.ActionOrder).ToList();


            optimazedActions.AddRange(actionsForOptimazation);
            List<PartialAction> actions = optimazedActions;


            //List<PartialAction> actions = Actions.OrderBy(x => x.MainAction.GetPreparationForecast(actionContext)).ToList();


            List<PartialAction> contextActions = new List<PartialAction>();
            if (actionContext.ProductionLineActions.ContainsKey(this))
                contextActions = actionContext.ProductionLineActions[this];
            if (!Simulator.CompareActionsSets(actions, contextActions))
            {
                var actionStrings = Simulator.GetActionsToStrings(actionContext, actions);
                var actionContextActionStrings = Simulator.GetActionsToStrings(actionContext, contextActions);
            }

            int i = 0;
            foreach(var partialAction  in actions)
            {
                partialAction.ActionOrder = i++;
            }

            actionContext.ProductionLineActions[this] = actions;

        }


        DateTime? LastChangeDateTime;

        internal void Run(ActionContext actionContext)
        {
            List<PartialAction> filteredPartialActions = GetActionsToDo(actionContext);
            var strings = GetActionsToStrings(actionContext);
            var slots = (from partialAction in filteredPartialActions
                         from slot in partialAction.ToDoSlots
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
                var action = Actions.Where(x => x.ToDoSlots.Any(y => y == slots[0])).First();
                var tt = action.GetPreparationForecast(actionContext);
                var astrings = Simulator.Actions.OrderBy(x => x.GetPreparationForecast(actionContext)).Select(x => x.ToString(actionContext)).ToList();
            }
            else if (slots.Count > 0)
            {
                while (slots.Count > 0 && actionContext.GetPreparationEndsAt(slots[0]) < DateTime.UtcNow)
                {
                    PreviousePreparationEndsAt = DateTime.UtcNow;
                    slots[0].State = FlavourBusinessFacade.RoomService.ItemPreparationState.IsPrepared;

                    var actions = Actions;//.Where(x=>(x.Action.PreparationForecast-TimeSpanEx.FromMinutes(x.Duration+5))<DateTime.UtcNow).ToList();
                    if (actionContext.ProductionLineActions.ContainsKey(this))
                        actions = actionContext.ProductionLineActions[this];
                    actions = actions.Where(x => x.ToDoSlots.Count > 0).ToList();

                    if (slots[0].PartialAction != null && slots[0].PartialAction.ToDoSlots.Count == 0)
                    {
                        slots[0].PartialAction.CompletionTime = DateTime.UtcNow;
                        if (slots[0].PartialAction.MainAction.SubActions.All(x => x.CompletionTime != null))
                            slots[0].PartialAction.MainAction.CompletionTime = slots[0].PartialAction.CompletionTime;

                        var packingTime = slots[0].PartialAction.Slots.Sum(x => x.PackingTime);
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
            var actions = Actions;//.Where(x=>(x.Action.PreparationForecast-TimeSpanEx.FromMinutes(x.Duration+5))<DateTime.UtcNow).ToList();
            if (actionContext.ProductionLineActions.ContainsKey(this))
                actions = actionContext.ProductionLineActions[this];

            List<PartialAction> filteredPartialActions = GetActionsToDo(actionContext);
            foreach (var partialAction in actions)
            {
                if (filteredPartialActions.Contains(partialAction))
                    actionStrings.Add(partialAction.ToString(actionContext));
                else if (partialAction.Slots.Any(x => x.State > FlavourBusinessFacade.RoomService.ItemPreparationState.ΙnPreparation))
                    actionStrings.Add(partialAction.ToString(actionContext));
                else
                {
                    if (partialAction.ActionOrderCommited)
                    {

                    }
                    actionStrings.Add("X-" + partialAction.ToString(actionContext));
                }

            }

            return actionStrings;
        }


        public void ActionsOrderCommited(ActionContext actionContext)
        {
            foreach (var partialAction in GetActionsToDo(actionContext))
                partialAction.ActionOrderCommited = true;

        }

        public List<PartialAction> GetActionsToDo(ActionContext actionContext)
        {


            var actions = Actions;//.Where(x=>(x.Action.PreparationForecast-TimeSpanEx.FromMinutes(x.Duration+5))<DateTime.UtcNow).ToList();
            if (actionContext.ProductionLineActions.ContainsKey(this))
                actions = actionContext.ProductionLineActions[this];
            List<PartialAction> filteredPartialActions = new List<PartialAction>();
            foreach (var partialAction in actions)
            {
                if ((partialAction.MainAction.GetPreparationForecast(actionContext) - partialAction.GetPreparationForecast(actionContext)).GetTotalMinutes() < 1.5 *Simulator.Velocity || ((partialAction.MainAction.GetPreparationForecast(actionContext) - TimeSpanEx.FromMinutes(partialAction.Duration + (2 * Simulator.Velocity))) < DateTime.UtcNow))
                {
                    foreach (var actionSlot in partialAction.Slots)
                    {
                        if (actionSlot.State == FlavourBusinessFacade.RoomService.ItemPreparationState.PreparationDelay)
                            actionSlot.State = FlavourBusinessFacade.RoomService.ItemPreparationState.PendingPreparation;
                    }

                    filteredPartialActions.Add(partialAction);
                }
                else if (partialAction.Slots.Any(x => x.State > FlavourBusinessFacade.RoomService.ItemPreparationState.ΙnPreparation))
                {

                    filteredPartialActions.Add(partialAction);
                }
                else
                {
                    if (partialAction.Slots.All(x => x.State > FlavourBusinessFacade.RoomService.ItemPreparationState.PreparationDelay))
                        filteredPartialActions.Add(partialAction);
                    else
                    {
                        if(partialAction.ActionOrderCommited)
                        {

                        }
                    }

                    break;
                }
            }


            //List<PartialAction> filteredPartialActions = new List<PartialAction>();

            //var partialActions = actionContext.ProductionLineActions[this];
            //foreach (var partialAction in partialActions)
            //{
            //    if ((partialAction.Action.GetPreparationStartForecast(actionContext) - DateTime.UtcNow).TotalMinutes <= 2.5)//
            //    {
            //        filteredPartialActions.Add(partialAction);
            //    }
            //    else
            //        break;
            //}

            return filteredPartialActions;
        }

        public double Velocity = 0;

        internal double GetDuration(ActionSlot actionSlot)
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

        static List<ActionSlot> B_ProductionLineActionSlots = new List<ActionSlot>(){
            new ActionSlot() { Name = "Pizza margarita",Duration = 7,PackingTime=0.8 },
            new ActionSlot() { Name = "Burger Americana",Duration = 8, PackingTime=0.2 },
            new ActionSlot(){Name="Pizza D'adrea",Duration=7, PackingTime=0.8},
            new ActionSlot(){Name="Chiken Burger",Duration=8 , PackingTime=0.2 },
            new ActionSlot(){Name="Boloneze",Duration=8 , PackingTime=0.2 },
            new ActionSlot(){Name="Amatritsian",Duration=7 , PackingTime=0.2 },
            new ActionSlot(){Name="Pizza special",Duration=7, PackingTime=0.8},
            new ActionSlot() { Name = "Carbonara", Duration = 8 , PackingTime=0.2 }
        };

        static List<ActionSlot> A_ProductionLineActionSlots = new List<ActionSlot>(){
            new ActionSlot(){Name="Πίτα γύρο χειρινο",Duration=0.5 , PackingTime=0.08 },
            new ActionSlot(){Name= "Πίτα γύρο κοτοπουλ", Duration=0.5 , PackingTime=0.08 },
            new ActionSlot(){Name="Κοτομπεικον",Duration=4 , PackingTime=0.2 },
            new ActionSlot(){Name= "Μεριδα γύρο κοτοπουλ", Duration=1 , PackingTime=0.15 },
            new ActionSlot(){Name= "Μεριδα γύρο χειρινο", Duration=1 , PackingTime=0.15 },
        };
        static List<ActionSlot> C_ProductionLineActionSlots = new List<ActionSlot>()
        {
            new ActionSlot(){Name= "Ριζοτο Alfredo ", Duration=6 , PackingTime=0.1 },
            new ActionSlot() { Name = "Ριζοτο Μιλανέζε ", Duration = 6 , PackingTime=0.1 },
             new ActionSlot() { Name = "Roast Beef ", Duration = 9 , PackingTime=0.1 }
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

        public Simulator()
        {

            if (true)
            {

            }

            ProductionLines = new List<ProductionLine>() { new ProductionLine("A", this), new ProductionLine("B", this), new ProductionLine("C", this) };

            Action action = new Action("a0", new List<PartialAction>() { new PartialAction() {
                ProductionLine = ProductionLines[1],
                //Duration = 15,
                Slots=new List<ActionSlot>(){new ActionSlot(){Name="Pizza margarita",Duration=7 },new ActionSlot(){Name="Burger Americana",Duration=8 }}
            }}, this);
            ActionsRepository.Add(action);

            //action = new Action("a1", new List<PartialAction>()
            //{
            //    new PartialAction()
            //    { ProductionLine = ProductionLines[0],
            //        Duration = 5,
            //        Slots=new List<ActionSlot>(){new ActionSlot(){Name="Πίτα γύρο χειρινο",Duration=2.5 },new ActionSlot(){Name= "Πίτα γύρο κοτοπουλ", Duration=2.5 }}
            //    } });
            //Actions.Add(action);

            action = new Action("a2",
                new List<PartialAction>() {
                     new PartialAction() { ProductionLine = ProductionLines[0],
                    //Duration = 5,
                    Slots=new List<ActionSlot>(){new ActionSlot(){Name="Πίτα γύρο χειρινο",Duration=2.5 },new ActionSlot(){Name= "Πίτα γύρο κοτοπουλ", Duration=2.5 }}
                },
                    new PartialAction() { ProductionLine = ProductionLines[1], //Duration = 30,
                Slots=new List<ActionSlot>(){
                    new ActionSlot(){Name="Pizza D'adrea",Duration=7 },
                    new ActionSlot(){Name="Chiken Burger",Duration=8 },
                    new ActionSlot(){Name="Boloneze",Duration=8 },
                    new ActionSlot(){Name="Amatritsian",Duration=7 }
                }
            } }, this);
            ActionsRepository.Add(action);

            action = new Action("a3", new List<PartialAction>()
            {
                new PartialAction() {
                    ProductionLine = ProductionLines[0],
                    //Duration = 4,
                    Slots=new List<ActionSlot>(){new ActionSlot(){Name="Κοτομπεικον",Duration=4 } }
                } ,
                new PartialAction() {
                    ProductionLine = ProductionLines[2],
                    //Duration = 12,
                    Slots=new List<ActionSlot>(){new ActionSlot(){Name= "Ριζοτο Alfredo ", Duration=6 }, new ActionSlot() { Name = "Ριζοτο Μιλανέζε ", Duration = 6 } }
                }
            }, this);
            ActionsRepository.Add(action);

            action = new Action("a4", new List<PartialAction>()
            {
                new PartialAction() {
                    ProductionLine = ProductionLines[0],
                    //Duration = 4,
                    Slots=new List<ActionSlot>(){new ActionSlot(){Name="Κοτομπεικον",Duration=4 } }
                }
            }, this);
            ActionsRepository.Add(action);

            action = new Action("a5", new List<PartialAction>()
            {
                new PartialAction() {
                    ProductionLine = ProductionLines[1],
                    //Duration = 10,
                Slots=new List<ActionSlot>(){new ActionSlot(){Name="Pizza special",Duration=7 }, new ActionSlot() { Name = "Carbonara", Duration = 8 } } } ,
                new PartialAction() {
                    ProductionLine = ProductionLines[2],
                    //Duration = 15,
                    Slots=new List<ActionSlot>(){new ActionSlot(){Name= "Ριζοτο Alfredo ", Duration=6 }, new ActionSlot() { Name = "Roast Beef ", Duration = 9 } }
                }
            }, this);
            ActionsRepository.Add(action);

            action = new Action("a6", new List<PartialAction>()
            {
                new PartialAction()
                {
                    ProductionLine = ProductionLines[0],
                    //Duration = 10,
                Slots=new List<ActionSlot>(){new ActionSlot(){Name="2 Πίτα γύρο χειρινο",Duration=5 },new ActionSlot(){Name= "2 Πίτα γύρο κοτοπουλ", Duration=5 }}
                } ,
                new PartialAction() { ProductionLine = ProductionLines[2], 
                    //Duration = 6,
                Slots=new List<ActionSlot>(){new ActionSlot(){Name= "Ριζοτο Alfredo ", Duration=6 } } }
            }, this);
            ActionsRepository.Add(action);
        }

        public List<Action> ActionsRepository = new List<Action>();
        List<Action> _Actions = new List<Action>();
        public List<Action> Actions
        {
            get
            {
                return _Actions.Where(x => x.SubActions.Count > 0).ToList();
            }
        }
        public List<ProductionLine> ProductionLines;

        public void start()
        {

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
                        if (count < 10)
                        {
                           if(AddAction(count))
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

                        foreach (var productionLine in ProductionLines)
                            productionLine.GetPredictions();

                        actionContext.DoubleCheckOptimazation = false;

                        Dictionary<ProductionLine, List<PartialAction>> productionLineActions = new Dictionary<ProductionLine, List<PartialAction>>();

                        foreach (var productionLine in actionContext.ProductionLineActions.Keys.ToArray())
                            productionLineActions[productionLine] = actionContext.ProductionLineActions[productionLine].ToList();

                        bool firstTime = true;
                        while (!actionContext.DoubleCheckOptimazation)
                        {
                            actionContext.DoubleCheckOptimazation = true;
                            foreach (var productionLine in ProductionLines)
                            {
                                productionLine.OptimizeActions(actionContext, firstTime);
                                productionLine.GetPredictions(actionContext);
                            }
                            firstTime = false;
                            //var astrings = Actions.OrderBy(x => x.GetPreparationForecast(actionContext)).Select(x => x.ToString(actionContext)).ToList();
                            //theAStrings.Add(astrings);

                            //List<List<string>> currentProductionLinesStrings = new List<List<string>>();
                            //productionLinesStrings.Add(currentProductionLinesStrings);

                            //foreach (var productionLine in ProductionLines)
                            //    currentProductionLinesStrings.Add(productionLine.GetNextActions(actionContext).Select(x=>x.ToString(actionContext)).ToList());

                        }
                        foreach (var productionLine in ProductionLines)
                        {
                            productionLine.ActionsOrderCommited(actionContext);
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

                        var astrings = Actions.OrderBy(x => x.GetPreparationForecast(actionContext)).Select(x => x.ToString(actionContext)).ToList();
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
                        if (productionLinesStrings.LastOrDefault() != null)
                        {
                            if (productionLinesStrings.Last().Entry[0].Count == 0 && productionLinesStrings.Last().Entry[1].Count == 0 && productionLinesStrings.Last().Entry[2].Count == 0)
                            {

                            }
                        }
                        string json = OOAdvantech.Json.JsonConvert.SerializeObject(productionLinesStrings);

                        string json_a= OOAdvantech.Json.JsonConvert.SerializeObject((from action in Actions
                                                                      where action.CompletionTime != null
                                                                      orderby action.CompletionTime
                                                                                     select new ActionCompleted
                                                                      {
                                                                          ActionName= action.Name,
                                                                          SubActionCompletionTime = action.SubActions.Select(x => x.ProductionLine.Name+" "+string.Format("{0:h:mm:ss tt}", x.CompletionTime)).ToList()
                                                                      }).ToList());

                        System.Threading.Thread.Sleep((int)TimeSpanEx.FromMinutes(0.2*Velocity).TotalMilliseconds);

                    }

                }
                catch (Exception error)
                {

                    throw;
                }

            });
        }
        public static bool CompareActionsSets(List<PartialAction> actions, List<PartialAction> contextActions)
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

        public static List<string> GetActionsToStrings(ActionContext actionContext, List<PartialAction> actions)
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
                if ((DateTime.UtcNow - LastActionAdded).GetTotalMinutes() > 1.5)
                    return false;
                var patern = ProductionLinesSlots[_R.Next(ProductionLinesSlots.Count - 1)];

                List<ActionSlot> a_productionLineActionSlots = new List<ActionSlot>();
                while (patern[0] > 0)
                {
                    a_productionLineActionSlots.Add(A_ProductionLineActionSlots[_R.Next(A_ProductionLineActionSlots.Count - 1)].CopyFor(ProductionLines[0]));
                    patern[0] = patern[0] - 1;
                }

                List<ActionSlot> b_productionLineActionSlots = new List<ActionSlot>();
                while (patern[1] > 0)
                {
                    b_productionLineActionSlots.Add(B_ProductionLineActionSlots[_R.Next(B_ProductionLineActionSlots.Count - 1)].CopyFor(ProductionLines[1]));
                    patern[1] = patern[1] - 1;
                }
                List<ActionSlot> c_productionLineActionSlots = new List<ActionSlot>();
                while (patern[2] > 0)
                {
                    c_productionLineActionSlots.Add(C_ProductionLineActionSlots[_R.Next(C_ProductionLineActionSlots.Count - 1)].CopyFor(ProductionLines[1]));
                    patern[2] = patern[2] - 1;
                }

                List<PartialAction> partialActions = new List<PartialAction>();

                if (a_productionLineActionSlots.Count > 0)
                    partialActions.Add(new PartialAction() { ProductionLine = ProductionLines[0], Slots = a_productionLineActionSlots });

                if (b_productionLineActionSlots.Count > 0)
                    partialActions.Add(new PartialAction() { ProductionLine = ProductionLines[1], Slots = b_productionLineActionSlots });

                if (c_productionLineActionSlots.Count > 0)
                    partialActions.Add(new PartialAction() { ProductionLine = ProductionLines[2], Slots = c_productionLineActionSlots });
                if (partialActions.Count != 0)
                {

                    var action = new Action("a" + actionID, partialActions, this);
                    _Actions.Add(action);
                    LastActionAdded = DateTime.UtcNow;
                    return true;
                }
            }
        }
    }
    /// <MetaDataID>{7d5af78e-a3af-45b8-b6a4-48e9facded87}</MetaDataID>
    public class ActionContext
    {
        public Dictionary<ProductionLine, List<PartialAction>> ProductionLineActions = new Dictionary<ProductionLine, List<PartialAction>>();

        public DateTime? GetPreparationStartsAt(ActionSlot actionSlot)
        {
            DateTime dateTime;
            if (SlotsPreparationStartsAt.TryGetValue(actionSlot, out dateTime))
                return dateTime;
            return actionSlot.PreparationStart;
        }
        public void SetPreparationStartsAt(ActionSlot actionSlot, DateTime dateTime)
        {
            if (SlotsPreparationStartsAt.ContainsKey(actionSlot) && SlotsPreparationStartsAt[actionSlot] != dateTime)
            {

            }

            SlotsPreparationStartsAt[actionSlot] = dateTime;

        }

        public DateTime? GetPreparationEndsAt(ActionSlot actionSlot)
        {
            if (actionSlot.ActiveDuration != actionSlot.Duration)
            {

            }


            DateTime dateTime;
            if (SlotsPreparationStartsAt.TryGetValue(actionSlot, out dateTime))
                return dateTime + TimeSpanEx.FromMinutes(actionSlot.ActiveDuration);

            return actionSlot.PreparationStart + TimeSpanEx.FromMinutes(actionSlot.ActiveDuration);
        }
        Dictionary<ActionSlot, DateTime> SlotsPreparationStartsAt = new Dictionary<ActionSlot, DateTime>();

        //        Dictionary<ActionSlot, DateTime> SlotsPreparationEndsAt = new Dictionary<ActionSlot, DateTime>();

        public bool DoubleCheckOptimazation { get; internal set; }
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

}
