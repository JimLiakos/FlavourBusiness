using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using OOAdvantech.Transactions;
using WPFUIElementObjectBind;

namespace MenuDesigner.ViewModel.MenuCanvas
{
    /// <MetaDataID>{d93bc200-90ad-4a16-abc1-e4b8ed7ccaa7}</MetaDataID>
    public class TreeBlankItemViewModel : MarshalByRefObject, MenuItemsEditor.IMenusTreeNode, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public MenuPresentationModel.MenuCanvas.MenuCanvasFoodItem MenuCanvasFoodItem;


        MenuPresentationModel.RestaurantMenu _GraphicMenu;
        internal MenuPresentationModel.RestaurantMenu GraphicMenu
        {
            get
            {
                return _GraphicMenu;
            }
            set
            {
                _GraphicMenu = value;
            }
        }


        public TreeBlankItemViewModel(MenuItemsEditor.IMenusTreeNode parent, MenuPresentationModel.RestaurantMenu graphicMenu)
        {
            _GraphicMenu = graphicMenu;



            MenuCanvasFoodItem = new MenuPresentationModel.MenuCanvas.MenuCanvasFoodItem();

            MenuCanvasFoodItem.ObjectChangeState += MenuCanvasFoodItem_ObjectChangeState;
            // MenuCanvasFoodItem.Description = menuItem.Name;
            Parent = parent;

            //if (menuItem.CustomPrices.Count > 0&& MenuCanvasFoodItem.Prices.Count==0)
            //{
            //    MenuPresentationModel.MenuCanvas.MenuCanvasFoodItemPrice foodItemPrice = new MenuPresentationModel.MenuCanvas.MenuCanvasFoodItemPrice();
            //    foodItemPrice.Price = menuItem.CustomPrices[0].Price;
            //    MenuCanvasFoodItem.AddFoodItemPrice(foodItemPrice);
            //}

        }

        private void MenuCanvasFoodItem_ObjectChangeState(object _object, string member)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (member == nameof(MenuPresentationModel.MenuCanvas.IMenuCanvasHeading.Page))
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TreeImage)));
            }));
        }






        /// <exclude>Excluded</exclude>
        bool _Edit;
        public bool Edit
        {
            get
            {
                return _Edit;
            }
            set
            {
                _Edit = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Edit)));
            }
        }



        /// <exclude>Excluded</exclude>
        bool _IsSelected;
        public bool IsSelected
        {
            get
            {
                return _IsSelected;
            }
            set
            {
                _IsSelected = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));
            }
        }

        public List<MenuItemsEditor.IMenusTreeNode> Members
        {
            get
            {
                return new List<MenuItemsEditor.IMenusTreeNode>();
            }
        }

        public string Name
        {
            get
            {
                return Properties.Resources.BlankItemName;
            }

            set
            {

            }
        }
        //UnTranslated
        public bool UnTranslated
        {
            get
            {
                return false;

                //return true;
            }
        }


        WPFUIElementObjectBind.ITranslator _Translator;
        public WPFUIElementObjectBind.ITranslator Translator
        {
            get
            {
                if (_Translator == null)
                    _Translator = new MenuItemsEditor.Translator();

                return _Translator;
            }
        }

        public MenuItemsEditor.IMenusTreeNode Parent { get; set; }

        public ImageSource TreeImage
        {
            get
            {

                return new BitmapImage(new Uri(@"pack://application:,,/MenuDesignerLib;Component/Resources/Images/Metro/BlankSign16.png"));

            }
        }






        public bool HasContextMenu
        {
            get
            {
                return false;
            }
        }


        public List<MenuCommand> ContextMenuItems
        {
            get
            {

                return null;
            }
        }






        public string CondextMenuResourceKey
        {
            get
            {
                return "MenuItemTreeNodeMenu";
            }
        }

        public bool IsEditable
        {
            get
            {
                return true;
            }
        }

        public bool IsNodeExpanded
        {
            get; set;
        }

        public List<MenuCommand> SelectedItemContextMenuItems
        {
            get
            {
                if (IsSelected)
                    return this.ContextMenuItems;

                else
                {
                    foreach (var subItem in this.Members)
                    {
                        var contextMenuItems = subItem.ContextMenuItems;
                        if (contextMenuItems != null)
                            return contextMenuItems;
                    }
                }
                return null;
            }
        }
    }
}
