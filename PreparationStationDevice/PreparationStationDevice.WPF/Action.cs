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
        public Action(List<PartialAction> subActions)
        {
            SubActions = subActions;
        }
        List<PartialAction> SubActions;
    }

    /// <MetaDataID>{0294b45b-f0c7-4af8-9938-6d827dbecc83}</MetaDataID>
    public class PartialAction
    {

        public double Duration;

        public ProductionLine ProductionLine;

        public List<ActionSlot> Slots;

    }
    /// <MetaDataID>{2db71939-c9b3-4834-b4b0-64606f034697}</MetaDataID>
    public class ActionSlot
    {
        public string Name;
        public double Duration;
    }

    /// <MetaDataID>{af9d458c-141a-40ca-9a4f-693deb6676bd}</MetaDataID>
    public class ProductionLine
    {
        public ProductionLine(string name)
        {
            Name = name;
            //List<PartialAction> actions
            //Actions = actions;
        }
        public List<PartialAction> Actions = new List<PartialAction>();

        public string Name { get; }
    }

    /// <MetaDataID>{b2c2a203-e4e6-4a0a-aec6-5106d04618dd}</MetaDataID>
    public class Simulator
    {
        public Simulator()
        {
            Action action = new Action(new List<PartialAction>() { new PartialAction() {
                ProductionLine = ProductionLines[1],
                Duration = 15,
                Slots=new List<ActionSlot>(){new ActionSlot(){Name="Pizza margarita",Duration=7 },new ActionSlot(){Name="Burger Americana",Duration=8 }}
            }});
            Actions.Add(action);

            action = new Action(new List<PartialAction>()
            {
                new PartialAction()
                { ProductionLine = ProductionLines[0],
                    Duration = 5,
                    Slots=new List<ActionSlot>(){new ActionSlot(){Name="Πίτα γύρο χειρινο",Duration=2.5 },new ActionSlot(){Name= "Πίτα γύρο κοτοπουλ", Duration=2.5 }}
                } });
            Actions.Add(action);

            action = new Action(new List<PartialAction>() { new PartialAction() { ProductionLine = ProductionLines[1], Duration = 30,
                Slots=new List<ActionSlot>(){
                    new ActionSlot(){Name="Pizza D'adrea",Duration=7 },
                    new ActionSlot(){Name="Chiken Burger",Duration=8 },
                    new ActionSlot(){Name="Boloneze",Duration=8 },
                    new ActionSlot(){Name="Amatritsian",Duration=7 }
                }
            } });
            Actions.Add(action);

            action = new Action(new List<PartialAction>()
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

            action = new Action(new List<PartialAction>()
            {
                new PartialAction() {
                    ProductionLine = ProductionLines[0],
                    Duration = 4,
                    Slots=new List<ActionSlot>(){new ActionSlot(){Name="Κοτομπεικον",Duration=4 } }
                }
            });
            Actions.Add(action);

            action = new Action(new List<PartialAction>()
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

            action = new Action(new List<PartialAction>()
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
        List<Action> Actions = new List<Action>();
        List<ProductionLine> ProductionLines = new List<ProductionLine>() { new ProductionLine("A"), new ProductionLine("B"), new ProductionLine("C") };

        public void start()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    var _this = this;
                    System.Threading.Thread.Sleep(2000);

                }
            });
        }


    }

}
