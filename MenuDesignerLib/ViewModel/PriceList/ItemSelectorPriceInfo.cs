using FLBManager.ViewModel;
using MenuModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using WPFUIElementObjectBind;

namespace MenuDesigner.ViewModel.PriceList
{
    /// <MetaDataID>{330d4ee9-3ed3-4fef-ab04-5778b4b571ab}</MetaDataID>
    public class ItemSelectorPriceInfo : FBResourceTreeNode, INotifyPropertyChanged
    {
        public ItemSelectorPriceInfo(FBResourceTreeNode parent, IMenuItem menuItem, MenuItemPrice itemPrice) : base(parent)
        {
            ItemPrice=itemPrice;
        }

        public override string Name { get => ItemPrice.ItemSelector.Name; set { } }

        public override ImageSource TreeImage => null;

        public override List<FBResourceTreeNode> Members => new List<FBResourceTreeNode>();

        public override List<MenuCommand> ContextMenuItems => new List<MenuCommand>();

        public override List<MenuCommand> SelectedItemContextMenuItems => ContextMenuItems;

        public MenuItemPrice ItemPrice { get; }

        public override void RemoveChild(FBResourceTreeNode treeNode)
        {
            throw new NotImplementedException();
        }

        public override void SelectionChange()
        {
            throw new NotImplementedException();
        }

        public Visibility PriceVisibility
        {
            get
            {

                return Visibility.Visible;



            }
            set { }
        }
        public string PriceText
        {
            get
            {

                return ItemPrice?.Price.ToString("C");
            }
            set { }
        }
    }
}
