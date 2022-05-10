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
            string subActions = null;
            foreach (var subAction in SubActions)
                subActions += subAction.ProductionLine.Name + "  ";

            return string.Format("{0} {1} : {2}", Name, subActions,PreparationForecast.ToShortTimeString());
        }
        public readonly string Name;
        public Action(string name, List<PartialAction> subActions)
        {
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
    }

    /// <MetaDataID>{0294b45b-f0c7-4af8-9938-6d827dbecc83}</MetaDataID>
    public class PartialAction
    {
        public PartialAction()
        {

        }
        public override string ToString()
        {
            return ProductionLine.Name + " " + Action.Name+" "+PreparationForecast.ToShortTimeString()+" "+Action.PreparationForecast.ToShortTimeString();
        }

        public double Duration;

        public ProductionLine ProductionLine;

        public List<ActionSlot> Slots;

        public DateTime PreparationForecast
        {
            get
            {
                return Slots.OrderBy(x => x.PreparationEnds).Last().PreparationEnds;
            }
        }

        public Action Action { get; set; }
    }
    /// <MetaDataID>{2db71939-c9b3-4834-b4b0-64606f034697}</MetaDataID>
    public class ActionSlot
    {
        public string Name;
        public double Duration;

        public DateTime PreparationStart { get; internal set; }

        public DateTime PreparationEnds
        {
            get
            {
                return PreparationStart + TimeSpan.FromMinutes(Duration);
            }
        }
    }

    /// <MetaDataID>{af9d458c-141a-40ca-9a4f-693deb6676bd}</MetaDataID>
    public class ProductionLine
    {
        public ProductionLine(string name, Simulator simulator)
        {
            Name = name;
            Simulator = simulator;
            //List<PartialAction> actions
            //Actions = actions;
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

            DateTime previousePreparationEndsAt = DateTime.UtcNow;
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
        internal void GetPredictions(ActionContext actionContext)
        {
            DateTime previousePreparationEndsAt = DateTime.UtcNow;

            if (actionContext.ProductionLineActions.ContainsKey(this))
            {
                var slots = (from partialAction in actionContext.ProductionLineActions[this]
                             where partialAction.ProductionLine == this
                             from slot in partialAction.Slots
                             select slot).ToList();
                
                foreach (var slot in slots)
                {
                    actionContext.SlotsPreparationStart[slot]= previousePreparationEndsAt;
                    slot.PreparationStart = previousePreparationEndsAt;
                    previousePreparationEndsAt = slot.PreparationStart + TimeSpan.FromMinutes(slot.Duration);
                }
            }
        }


        internal List<PartialAction> OptimizeActions()
        {
            var actions = Actions.OrderBy(x=>x.Action.PreparationForecast).ToList();


            return actions;

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
            }});
            Actions.Add(action);

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
            } });
            Actions.Add(action);

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
            });
            Actions.Add(action);

            action = new Action("a4", new List<PartialAction>()
            {
                new PartialAction() {
                    ProductionLine = ProductionLines[0],
                    Duration = 4,
                    Slots=new List<ActionSlot>(){new ActionSlot(){Name="Κοτομπεικον",Duration=4 } }
                }
            });
            Actions.Add(action);

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
            });
            Actions.Add(action);

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
            });
            Actions.Add(action);



        }
        public List<Action> Actions = new List<Action>();
        List<ProductionLine> ProductionLines;

        public void start()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    var _this = this;

                    foreach (var productionLine in ProductionLines)
                        productionLine.GetPredictions();

                    foreach (var action in Actions)
                    {
                        
                    }
                    ActionContext actionContext = new ActionContext();
                    foreach (var productionLine in ProductionLines)
                    {

                       var actions= productionLine.OptimizeActions();
                        actionContext.ProductionLineActions[productionLine] = actions;

                        foreach (var inProductionLine in ProductionLines)
                        {
                            productionLine.GetPredictions(actionContext);
                        }

                    }


                    System.Threading.Thread.Sleep(20000);

                }
            });
        }


    }
    public class ActionContext
    {
        public Dictionary<ProductionLine, List<PartialAction>> ProductionLineActions = new Dictionary<ProductionLine, List<PartialAction>>();

        public Dictionary<ActionSlot, DateTime> SlotsPreparationStart = new Dictionary<ActionSlot, DateTime>();
    }

}
