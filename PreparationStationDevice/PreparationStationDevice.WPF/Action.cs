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
                if (this.SubActions.Any(x => x.ProductionLine == productionLine))
                    subActions += productionLine.Name;
                else
                    subActions += "-";

                subActions += "    ";
            }


            return string.Format("{0} {1} : {2}", Name, subActions, PreparationForecast.ToShortTimeString());
        }

        public Simulator Simulator { get; private set; }

        public readonly string Name;
        public Action(string name, List<PartialAction> subActions, Simulator simulator)
        {
            Simulator = simulator;
            Name = name;
            SubActions = subActions;

            foreach (var subAction in subActions)
                subAction.Action = this;
        }
        public List<PartialAction> SubActions;

        public DateTime PreparationForecast
        {
            get
            {
                return SubActions.OrderBy(x => x.PreparationForecast).Last().PreparationForecast;
            }
        }
        public DateTime GetPreparationForecast(ActionContext context)
        {
            return SubActions.OrderBy(x => x.GetPreparationForecast(context)).Last().GetPreparationForecast(context);
        }

        public DateTime GetPreparationStartForecast(ActionContext context)
        {
            return SubActions.OrderBy(x => x.GetPreparationForecast(context)).Last().GetPreparationForecast(context) - TimeSpan.FromMinutes(SubActions.OrderBy(x => x.GetPreparationForecast(context)).Last().Duration);
        }

        internal string ToString(ActionContext actionContext)
        {
            string subActions = " ";
            foreach (var productionLine in Simulator.ProductionLines)
            {
                if (this.SubActions.Any(x => x.ProductionLine == productionLine))
                    subActions += productionLine.Name;
                else
                    subActions += "-";

                subActions += "    ";
            }
            return string.Format("{0} {1} : {2}", Name, subActions, GetPreparationForecast(actionContext).ToShortTimeString());
        }
    }

    /// <MetaDataID>{0294b45b-f0c7-4af8-9938-6d827dbecc83}</MetaDataID>
    public class PartialAction
    {
        public PartialAction()
        {

        }
        public override string ToString()
        {
            return ProductionLine.Name + " " + Action.Name + " " + PreparationForecast.ToShortTimeString() + " " + Action.PreparationForecast.ToShortTimeString();
        }

        double _Duration;
        public double Duration
        {
            get => _Duration;
            set
            {
                _Duration = value / 2;
            }

        }

        public ProductionLine ProductionLine;

        List<ActionSlot> _Slots;
        public List<ActionSlot> Slots
        {
            get
            {
                if (_Slots != null)
                    return _Slots.Where(x => x.State < FlavourBusinessFacade.RoomService.ItemPreparationState.IsRoasting).ToList();
                return _Slots;
            }
            set
            {
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

        public Action Action { get; set; }

        public string ToString(ActionContext actionContext)
        {

            var preparationForecast = GetPreparationForecast(actionContext);

            return ProductionLine.Name + " " + Action.Name + " " + preparationForecast.ToShortTimeString() + " " + Action.GetPreparationForecast(actionContext).ToShortTimeString();
        }

        public DateTime GetPreparationForecast(ActionContext actionContext)
        {
            if (actionContext.ProductionLineActions.ContainsKey(this.ProductionLine))
                return actionContext.SlotsPreparationEndsAt[Slots.OrderBy(x => actionContext.SlotsPreparationEndsAt[x]).Last()];
            else
                return PreparationForecast;
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
                _Duration = value / 2;
            }

        }

        public DateTime PreparationStart { get; internal set; }

        public DateTime PreparationEnds
        {
            get
            {
                return PreparationStart + TimeSpan.FromMinutes(Duration);
            }
        }
        public FlavourBusinessFacade.RoomService.ItemPreparationState State { get; set; }
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
                        from partialAction in action.SubActions
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
                         from partialAction in action.SubActions
                         where partialAction.ProductionLine == this
                         from slot in partialAction.Slots
                         select slot).ToList();

            foreach (var slot in slots)
            {
                slot.PreparationStart = previousePreparationEndsAt;
                previousePreparationEndsAt = slot.PreparationStart + TimeSpan.FromMinutes(slot.Duration);
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
                var slots = (from partialAction in actionContext.ProductionLineActions[this]
                             where partialAction.ProductionLine == this
                             from slot in partialAction.Slots
                             select slot).ToList();

                foreach (var slot in slots)
                {

                    previousePreparationEndsAt = previousePreparationEndsAt + TimeSpan.FromMinutes(slot.Duration);
                    if (!actionContext.SlotsPreparationEndsAt.ContainsKey(slot) || actionContext.SlotsPreparationEndsAt[slot] != previousePreparationEndsAt)
                        actionContext.OptimazationDoubleCheck = false;
                    actionContext.SlotsPreparationEndsAt[slot] = previousePreparationEndsAt;
                }
            }
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


        internal void OptimizeActions(ActionContext actionContext)
        {
            var partialActions = Actions.OrderBy(x => x.Action.GetPreparationForecast(actionContext)).ToList();

            actionContext.ProductionLineActions[this] = partialActions;

        }


        DateTime? LastChangeDateTime;

        internal void Run(ActionContext actionContext)
        {



            List<PartialAction> filteredPartialActions = GetActionsToDo(actionContext);
            var strings = GetActionsToStrings(actionContext);
            var slots = (from partialAction in filteredPartialActions
                         from slot in partialAction.Slots
                         where slot.State < FlavourBusinessFacade.RoomService.ItemPreparationState.IsRoasting
                         select slot).ToList();


            if (LastChangeDateTime == null && slots.Count > 0)
            {
                LastChangeDateTime = DateTime.UtcNow;
                slots[0].State = FlavourBusinessFacade.RoomService.ItemPreparationState.ΙnPreparation;
                var date = actionContext.SlotsPreparationEndsAt[slots[0]];
            }
            else if (slots.Count > 0)
            {
                if (actionContext.SlotsPreparationEndsAt[slots[0]] < DateTime.UtcNow)
                {
                    PreviousePreparationEndsAt = DateTime.UtcNow;
                    slots[0].State = FlavourBusinessFacade.RoomService.ItemPreparationState.IsPrepared;
                }
            }




        }

        public List<PartialAction> GetActionsToDo(ActionContext actionContext)
        {
            List<PartialAction> filteredPartialActions = new List<PartialAction>();
            var partialActions = actionContext.ProductionLineActions[this];
            foreach (var partialAction in partialActions)
            {
                if ((partialAction.Action.GetPreparationStartForecast(actionContext) - DateTime.UtcNow).TotalMinutes <= 2.5 ||
                    ((partialAction.Action.GetPreparationForecast(actionContext) - partialAction.GetPreparationForecast(actionContext)).TotalMinutes < 1.5))
                {
                    filteredPartialActions.Add(partialAction);
                }
                else
                    break;
            }
            return filteredPartialActions;
        }
    }

    /// <MetaDataID>{b2c2a203-e4e6-4a0a-aec6-5106d04618dd}</MetaDataID>
    public class Simulator
    {


        public Simulator()
        {

            ProductionLines = new List<ProductionLine>() { new ProductionLine("A", this), new ProductionLine("B", this), new ProductionLine("C", this) };

            Action action = new Action("a0", new List<PartialAction>() { new PartialAction() {
                ProductionLine = ProductionLines[1],
                Duration = 15,
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
                    Duration = 5,
                    Slots=new List<ActionSlot>(){new ActionSlot(){Name="Πίτα γύρο χειρινο",Duration=2.5 },new ActionSlot(){Name= "Πίτα γύρο κοτοπουλ", Duration=2.5 }}
                },
                    new PartialAction() { ProductionLine = ProductionLines[1], Duration = 30,
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
                    Duration = 4,
                    Slots=new List<ActionSlot>(){new ActionSlot(){Name="Κοτομπεικον",Duration=4 } }
                } ,
                new PartialAction() {
                    ProductionLine = ProductionLines[2],
                    Duration = 12,
                    Slots=new List<ActionSlot>(){new ActionSlot(){Name= "Ριζοτο Alfredo ", Duration=6 }, new ActionSlot() { Name = "Ριζοτο Μιλανέζε ", Duration = 6 } }
                }
            }, this);
            ActionsRepository.Add(action);

            action = new Action("a4", new List<PartialAction>()
            {
                new PartialAction() {
                    ProductionLine = ProductionLines[0],
                    Duration = 4,
                    Slots=new List<ActionSlot>(){new ActionSlot(){Name="Κοτομπεικον",Duration=4 } }
                }
            }, this);
            ActionsRepository.Add(action);

            action = new Action("a5", new List<PartialAction>()
            {
                new PartialAction() {
                    ProductionLine = ProductionLines[1],
                    Duration = 10,
                Slots=new List<ActionSlot>(){new ActionSlot(){Name="Pizza special",Duration=7 }, new ActionSlot() { Name = "Carbonara", Duration = 8 } } } ,
                new PartialAction() {
                    ProductionLine = ProductionLines[2],
                    Duration = 15,
                    Slots=new List<ActionSlot>(){new ActionSlot(){Name= "Ριζοτο Alfredo ", Duration=6 }, new ActionSlot() { Name = "Roast Beef ", Duration = 9 } }
                }
            }, this);
            ActionsRepository.Add(action);

            action = new Action("a6", new List<PartialAction>()
            {
                new PartialAction()
                {
                    ProductionLine = ProductionLines[0],
                    Duration = 10,
                Slots=new List<ActionSlot>(){new ActionSlot(){Name="2 Πίτα γύρο χειρινο",Duration=5 },new ActionSlot(){Name= "2 Πίτα γύρο κοτοπουλ", Duration=5 }}
                } ,
                new PartialAction() { ProductionLine = ProductionLines[2], Duration = 6,
                Slots=new List<ActionSlot>(){new ActionSlot(){Name= "Ριζοτο Alfredo ", Duration=6 } } }
            }, this);
            ActionsRepository.Add(action);
        }

        public List<Action> ActionsRepository = new List<Action>();

        public List<Action> Actions = new List<Action>();
        public List<ProductionLine> ProductionLines;

        public void start()
        {
            Task.Run(() =>
            {
                List<List<string>> theAStrings = new List<List<string>>();
                List<List<List<string>>> theProductionLinesActionStrings = new List<List<List<string>>>();

                while (true)
                {

                    if (ActionsRepository.Count > 0)
                    {
                        Actions.Add(ActionsRepository[0]);
                        ActionsRepository.RemoveAt(0);
                    }
                    else
                    {

                    }

                    //List<List<string>> theAStrings = new List<List<string>>();
                    //List<List<List<string>>> theProductionLinesActionStrings = new List<List<List<string>>>();

                    var _this = this;

                    foreach (var productionLine in ProductionLines)
                        productionLine.GetPredictions();


                    ActionContext actionContext = new ActionContext();

                    while (!actionContext.OptimazationDoubleCheck)
                    {
                        actionContext.OptimazationDoubleCheck = true;
                        foreach (var productionLine in ProductionLines)
                        {
                            productionLine.OptimizeActions(actionContext);
                            productionLine.GetPredictions(actionContext);
                        }
                        //List<List<string>> currentProductionLinesActionStrings = new List<List<string>>();
                        //theProductionLinesActionStrings.Add(currentProductionLinesActionStrings);
                        //foreach (var productionLine in ProductionLines)
                        //    currentProductionLinesActionStrings.Add(productionLine.GetActionsToDo(actionContext).Select(x => x.ToString(actionContext)).ToList());

                        //var astrings = Actions.OrderBy(x => x.GetPreparationForecast(actionContext)).Select(x => x.ToString(actionContext)).ToList();
                        //theAStrings.Add(astrings);
                    }

                    List<List<string>> currentProductionLinesActionStrings = new List<List<string>>();
                    theProductionLinesActionStrings.Add(currentProductionLinesActionStrings);
                    foreach (var productionLine in ProductionLines)
                        currentProductionLinesActionStrings.Add(productionLine.GetActionsToDo(actionContext).Select(x => x.ToString(actionContext)).ToList());

                    var astrings = Actions.OrderBy(x => x.GetPreparationForecast(actionContext)).Select(x => x.ToString(actionContext)).ToList();
                    theAStrings.Add(astrings);


                    //foreach (var inProductionLine in ProductionLines)
                    //{
                    //    var strings = inProductionLine.GetActionsToStrings(actionContext);
                    //    var astrings = Actions.OrderBy(x => x.GetPreparationForecast(actionContext)).Select(x => x.ToString(actionContext)).ToList();
                    //    theAStrings.Add(astrings);
                    //}


                    foreach (var productionLine in ProductionLines)
                    {
                        productionLine.Run(actionContext);
                    }
                    System.Threading.Thread.Sleep(20000);

                }
            });
        }


    }
    /// <MetaDataID>{7d5af78e-a3af-45b8-b6a4-48e9facded87}</MetaDataID>
    public class ActionContext
    {
        public Dictionary<ProductionLine, List<PartialAction>> ProductionLineActions = new Dictionary<ProductionLine, List<PartialAction>>();

        Get
        public Dictionary<ActionSlot, DateTime> SlotsPreparationEndsAt = new Dictionary<ActionSlot, DateTime>();

        public bool OptimazationDoubleCheck { get; internal set; }
    }

}
