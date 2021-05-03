using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MenuItemsEditor.ViewModel
{
    /// <MetaDataID>{5f2530de-7654-44c5-9950-ce7c8afb18ae}</MetaDataID>
    public class TreeViewOptionsTypesViewModel: MarshalByRefObject, IMenusTreeNode, INotifyPropertyChanged
    {

        MenuModel.IItemsCategory ItemsCategory;

        public event PropertyChangedEventHandler PropertyChanged;

        public IMenusTreeNode Parent { get; set; }

        public string Name
        {
            get
            {
                return "Item Types";   
            }

            set
            {
            }
        }

        public ImageSource TreeImage
        {
            get
            {
                return new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/type16.png"));
            }
        }

        public List<IMenusTreeNode> Members
        {
            get
            {
                return new List<IMenusTreeNode>(); 
            }
        }

        public bool Edit
        {
            get
            {
                return false;
            }

            set
            {
                
            }
        }

        bool _IsSelected;

        public bool IsSelected
        {
            get
            {
                return _IsSelected;
            }

            set
            {
                _IsSelected = true;
            }
        }

        //public ContextMenu ContextMenu
        //{
        //    get
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        public bool HasContextMenu
        {
            get
            {
                return true;
            }
        }

        public WPFUIElementObjectBind.RelayCommand EditCommand { get; protected set; }



        /// <exclude>Excluded</exclude>
        List<MenuComamnd> _ContextMenuItems;
        public List<MenuComamnd> ContextMenuItems
        {
            get
            {
                if (_ContextMenuItems == null)
                {

                    _ContextMenuItems = new List<MenuComamnd>();

                    var imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Empty.png"));
                    var emptyImage = new Image() { Source = imageSource, Width = 16, Height = 16 };

                    
                    MenuComamnd menuItem = new MenuComamnd();
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Edit16.png"));
                    menuItem.Header = Properties.Resources.EditObject;
                    menuItem.Icon = new Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = EditCommand;

                    _ContextMenuItems.Add(menuItem);


                }
                return _ContextMenuItems;
            }
        }


        public bool IsEditable
        {
            get
            {
                return false;
            }
        }

        public TreeViewOptionsTypesViewModel(MenuModel.IItemsCategory itemsCategory, IMenusTreeNode parent)
        {
            ItemsCategory = itemsCategory;
            Parent = parent;


            EditCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                System.Windows.Window win = System.Windows.Window.GetWindow(EditCommand.UserInterfaceObjectConnection.ContainerControl as System.Windows.DependencyObject);
                EditMenuItem(win);
            });

        }

        private void EditMenuItem(Window win)
        {
            
        }
    }
}
