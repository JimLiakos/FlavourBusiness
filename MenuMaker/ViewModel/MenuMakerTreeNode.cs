using FlavourBusinessFacade.HumanResources;
using FLBManager.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPFUIElementObjectBind;

namespace MenuMaker.ViewModel
{
    /// <MetaDataID>{08b02dd6-61e5-496c-b2df-4ad7c6bab4e1}</MetaDataID>
    public class MenuMakerTreeNode : FBResourceTreeNode, INotifyPropertyChanged
    {
        public override void RemoveChild(FBResourceTreeNode treeNode)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{c3eaea5b-8aa7-417f-b532-6b12547c7380}</MetaDataID>
        public readonly IMenuMaker MenuMaker;



      
        /// <MetaDataID>{b5619724-da6a-42c4-a1bf-89b44f3db7cb}</MetaDataID>
        public MenuMakerTreeNode(IMenuMaker menuMaker) : base(null)
        {
            MenuMaker = menuMaker;

            RenameCommand = new RelayCommand((object sender) =>
            {
                EditMode();
            });


            ManuMakingActivitiesTreeNode manuMakingActivitiesTreeNode = new ManuMakingActivitiesTreeNode(this);
            _Members.Add(manuMakingActivitiesTreeNode);
            IsNodeExpanded = true;
        }

   

      

        /// <MetaDataID>{0825e2b9-9383-4958-88f4-2f8659710987}</MetaDataID>
        public WPFUIElementObjectBind.RelayCommand RenameCommand { get; protected set; }

        /// <MetaDataID>{f4667af8-6d71-4ffb-86f4-b75e00506496}</MetaDataID>


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
                return new BitmapImage(new Uri(@"pack://application:,,,/MenuMaker;Component/Resources/Images/Metro/graphic-designer16.png"));
            }
        }

        /// <MetaDataID>{da46b7f1-1ef7-48ba-92f7-104535b39a6e}</MetaDataID>
        public override void SelectionChange()
        {

        }
    }
}
