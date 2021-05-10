using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MenuDesigner.MenuPresentetion
{
    public delegate void PriceTextWidthChangedHandle(MenuItemPresentationViewModel sender);
    /// <MetaDataID>{944ddde3-19cb-4fc6-aa78-37645c880470}</MetaDataID>
    public class MenuItemPresentationViewModel : MarshalByRefObject, System.ComponentModel.INotifyPropertyChanged
    {

        public readonly MenuItemsPresentationViewModel PresentationArea;
        public readonly MenuModel.MenuItem MenuItem;
        public MenuItemPresentationViewModel(MenuItemsPresentationViewModel presentationArea, MenuModel.MenuItem menuItem)
        {
            MenuItem = menuItem;
            PresentationArea = presentationArea;
        }
        public MenuItemPresentationViewModel()
        {
        }
        public void Remove()
        {
            PresentationArea.RemoveMenuItem(this);
        }
        double _PriceTextWidth;
        public double PriceTextWidth
        {
            get
            {
                return _PriceTextWidth;
            }
            set
            {
                
                if (_PriceTextWidth != value)
                {
                    _PriceTextWidth = value;
                    PriceTextWidthChanged?.Invoke(this);
                }
            }

        }
        System.Windows.GridLength _PriceColumntWidth = System.Windows.GridLength.Auto;
        public System.Windows.GridLength PriceColumntWidth
        {
            get
            {
                return _PriceColumntWidth;
            }
            set
            {
                _PriceColumntWidth = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PriceColumntWidth"));
            }

        }

        public event PriceTextWidthChangedHandle PriceTextWidthChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        public string Name
        {
            get
            {
                return MenuItem.Name;
            }
            set
            {
                MenuItem.Name = value;
            }
        }

        public string Price
        {
            get
            {
                return string.Format("{0:C}", 12);
            }
            set
            {

            }
        }

        internal void MoveItemAfter(MenuItemPresentationViewModel menuItem)
        {
            if(menuItem!=this)
            {
                int pos = 0;
                if (menuItem != null)
                {
                    if(menuItem.PresentationArea.MenuItems.IndexOf(this)> menuItem.PresentationArea.MenuItems.IndexOf(menuItem))
                        pos = menuItem.PresentationArea.MenuItems.IndexOf(menuItem) + 1;
                    else
                        pos = menuItem.PresentationArea.MenuItems.IndexOf(menuItem) ;

                }
                PresentationArea.MoveMenuItem(this, pos);

            }
        }
    }
}
