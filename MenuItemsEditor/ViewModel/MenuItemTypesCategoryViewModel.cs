using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;

namespace MenuItemsEditor.ViewModel
{
    /// <MetaDataID>{5f2530de-7654-44c5-9950-ce7c8afb18ae}</MetaDataID>
    public class MenuItemTypesCategoryViewModel: MarshalByRefObject, IMenusTreeNode, INotifyPropertyChanged
    {

        MenuModel.IItemsCategory ItemsCategory;

        public event PropertyChangedEventHandler PropertyChanged;

        public IMenusTreeNode Parent { get; set; }

        public string Name
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public ImageSource TreeImage
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public List<IMenusTreeNode> Members
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool Edit
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public bool IsSelected
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public ContextMenu ContextMenu
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public List<MenuComamnd> ContextMenuItems
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsEditable
        {
            get
            {
                return false;
            }
        }

        public MenuItemTypesCategoryViewModel(MenuModel.IItemsCategory itemsCategory, IMenusTreeNode parent)
        {
            ItemsCategory = itemsCategory;
            Parent = parent;
        }

    }
}
