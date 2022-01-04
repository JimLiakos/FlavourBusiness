using FlavourBusinessFacade.HumanResources;
using OOAdvantech.Transactions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPFUIElementObjectBind;

namespace FLBManager.ViewModel.HumanResources
{
    /// <MetaDataID>{28230a56-a983-4da1-bf57-9a6a006645d3}</MetaDataID>
    public class MenuMakerTreeNode : FBResourceTreeNode, INotifyPropertyChanged
    {
        public override void RemoveChild(FBResourceTreeNode treeNode)
        {
            throw new NotImplementedException();
        }
        /// <MetaDataID>{55be189e-0c0c-40b5-949e-dff553578092}</MetaDataID>
        MenuMakersTreeNode MenuMakers;
        /// <MetaDataID>{c3eaea5b-8aa7-417f-b532-6b12547c7380}</MetaDataID>
        public readonly IMenuMaker MenuMaker;


        public readonly IAccountability MenuMakingAccountability;

        /// <MetaDataID>{b5619724-da6a-42c4-a1bf-89b44f3db7cb}</MetaDataID>
        public MenuMakerTreeNode(MenuMakersTreeNode parent, IAccountability menuMakingAccountability) : base(parent)
        {
            MenuMakers = parent;
            MenuMakingAccountability = menuMakingAccountability;
            MenuMaker = menuMakingAccountability.Responsible as IMenuMaker;
            EditCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                System.Windows.Window win = System.Windows.Window.GetWindow(EditCommand.UserInterfaceObjectConnection.ContainerControl as System.Windows.DependencyObject);
                EditMenuItem(win);
            });
            DeleteCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                MenuMakers.RemoveMenuMaker(this);
            });
        }

        /// <MetaDataID>{e27121fe-2d0c-48d3-8551-1ac4b1705ebd}</MetaDataID>
        private void EditMenuItem(Window win)
        {
            Views.HumanResources.MenuMakerWindow menuMakerWindow = new Views.HumanResources.MenuMakerWindow();
            menuMakerWindow.Owner = win;

            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.RequiresNew))
            {

                ViewModel.HumanResources.MenuMakerViewModel menuMakerViewModel = new HumanResources.MenuMakerViewModel(MenuMakers.Company, this.MenuMakingAccountability);
                menuMakerWindow.GetObjectContext().SetContextInstance(menuMakerViewModel);
                if (menuMakerWindow.ShowDialog().Value)
                {
                    menuMakerViewModel.Save();
                    stateTransition.Consistent = true;
                }
            }
        }

        /// <MetaDataID>{e5f7b48e-c994-4151-b6aa-ba6955e10749}</MetaDataID>
        public void EditMode()
        {
            if (_Edit == true)
            {
                _Edit = !_Edit;
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Edit)));
            }
            _Edit = true;
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Edit)));
        }

        /// <MetaDataID>{7041e614-aded-4427-bad6-4d98791b03d4}</MetaDataID>
        private void Delete()
        {
            MenuMakers.RemoveMenuMaker(this);

            // (Company as CompanyPresentation).RemoveServicesContext(this);
        }

        /// <MetaDataID>{0825e2b9-9383-4958-88f4-2f8659710987}</MetaDataID>
        public WPFUIElementObjectBind.RelayCommand RenameCommand { get; protected set; }
        /// <MetaDataID>{031c88f5-eeba-4caa-96ac-1d9e66ba6956}</MetaDataID>
        public WPFUIElementObjectBind.RelayCommand DeleteCommand { get; protected set; }
        /// <MetaDataID>{f4667af8-6d71-4ffb-86f4-b75e00506496}</MetaDataID>
        public WPFUIElementObjectBind.RelayCommand EditCommand { get; protected set; }




        /// <MetaDataID>{78ee1297-3fe4-49e4-b0b1-fb7b93385d47}</MetaDataID>
        List<WPFUIElementObjectBind.MenuCommand> _ContextMenuItems;

        /// <MetaDataID>{96ab5e40-6da5-4849-a728-7b5d67f25311}</MetaDataID>
        public override List<WPFUIElementObjectBind.MenuCommand> ContextMenuItems
        {
            get
            {
                if (_ContextMenuItems == null)
                {

                    _ContextMenuItems = new List<WPFUIElementObjectBind.MenuCommand>();

                    var imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Empty.png"));
                    var emptyImage = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };

                    WPFUIElementObjectBind.MenuCommand menuItem = new WPFUIElementObjectBind.MenuCommand();
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Rename16.png"));
                    menuItem.Header = MenuItemsEditor.Properties.Resources.TreeNodeRenameMenuItemHeader;
                    menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = RenameCommand;
                    _ContextMenuItems.Add(menuItem);




                    _ContextMenuItems.Add(null);

                    menuItem = new WPFUIElementObjectBind.MenuCommand();
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/delete.png"));
                    menuItem.Header = Properties.Resources.RemoveCallerIDServer;
                    menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = DeleteCommand;

                    _ContextMenuItems.Add(menuItem);
                    menuItem = new WPFUIElementObjectBind.MenuCommand();
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Edit16.png"));
                    menuItem.Header = MenuItemsEditor.Properties.Resources.EditObject;
                    menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = EditCommand;

                    _ContextMenuItems.Add(menuItem);

                }

                return _ContextMenuItems;
            }
        }

        /// <MetaDataID>{4326eecb-560f-45a8-8864-ef48d9f915fe}</MetaDataID>
        List<FBResourceTreeNode> _Members = new List<FBResourceTreeNode>();
        /// <MetaDataID>{fb4791e1-7b45-44fd-a784-4f2e75866e91}</MetaDataID>
        public override List<FBResourceTreeNode> Members
        {
            get
            {
                return _Members;
            }
        }


        /// <MetaDataID>{2e7ab156-8b2a-40fb-94e7-b08fbcd5063c}</MetaDataID>
        public override string Name
        {
            get
            {
                return MenuMaker.Name;
            }

            set
            {
                MenuMaker.Name = value;
            }
        }
        /// <MetaDataID>{72beb241-c151-4308-bda8-910a59a2e9d2}</MetaDataID>
        public override bool HasContextMenu
        {
            get
            {
                return true;
            }
        }
        /// <MetaDataID>{b26cb720-659e-4709-8788-d2653ffea09c}</MetaDataID>
        public override bool IsEditable
        {
            get
            {
                return true;
            }
        }

        /// <MetaDataID>{5039b146-9bb7-4f4c-b222-7965f7a61d92}</MetaDataID>
        public override List<WPFUIElementObjectBind.MenuCommand> SelectedItemContextMenuItems
        {
            get
            {
                if (IsSelected)
                    return ContextMenuItems;
                else
                    foreach (var treeNode in Members)
                    {
                        var contextMenuItems = treeNode.SelectedItemContextMenuItems;
                        if (contextMenuItems != null)
                            return contextMenuItems;
                    }

                return null;
            }
        }


        /// <MetaDataID>{78b0d5ff-8ff0-4b4b-b291-2961f9c6ae21}</MetaDataID>
        public override ImageSource TreeImage
        {
            get
            {
                return new BitmapImage(new Uri(@"pack://application:,,,/FLBManager;Component/Resources/Images/Metro/graphic-designer16.png"));
            }
        }

        /// <MetaDataID>{da46b7f1-1ef7-48ba-92f7-104535b39a6e}</MetaDataID>
        public override void SelectionChange()
        {

        }
    }
}
