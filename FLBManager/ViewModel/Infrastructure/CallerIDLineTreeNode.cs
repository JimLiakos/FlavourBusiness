using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FlavourBusinessFacade.ServicesContextResources;
using WPFUIElementObjectBind;

namespace FLBManager.ViewModel.Infrastructure
{
    /// <MetaDataID>{d32b953d-a5d1-4367-b26b-dccf0837fc10}</MetaDataID>
    public class CallerIDLineTreeNode : FBResourceTreeNode, INotifyPropertyChanged
    {
        public override void RemoveChild(FBResourceTreeNode treeNode)
        {
            throw new NotImplementedException();
        }
        internal ICallerIDLine CallerIDLine;
        internal CallerIDServerTreeNode CallerIDServerTreeNode;
        public CallerIDLineTreeNode(CallerIDServerTreeNode parent, ICallerIDLine callerIDLine) : base(parent)
        {
            CallerIDServerTreeNode = parent;
            this.CallerIDLine = callerIDLine;

            RenameCommand = new RelayCommand((object sender) =>
            {
                EditMode();
            });
            DeleteCommand = new RelayCommand((object sender) =>
            {
                Delete();
            });
        }

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

        private void Delete()
        {
            CallerIDServerTreeNode.RemoveCallerIDLine(this);

            // (Company as CompanyPresentation).RemoveServicesContext(this);
        }

        public RelayCommand RenameCommand { get; protected set; }
        public RelayCommand DeleteCommand { get; protected set; }



        List<MenuCommand> _ContextMenuItems;

        public override List<MenuCommand> ContextMenuItems
        {
            get
            {
                if (_ContextMenuItems == null)
                {

                    _ContextMenuItems = new List<MenuCommand>();

                    var imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Empty.png"));
                    var emptyImage = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };

                    MenuCommand menuItem = new MenuCommand();
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Rename16.png"));
                    menuItem.Header = MenuItemsEditor.Properties.Resources.TreeNodeRenameMenuItemHeader;
                    menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = RenameCommand;
                    _ContextMenuItems.Add(menuItem);


                    //_ContextMenuItems.Add(null);

                    //menuItem = new MenuComamnd(); ;
                    //imageSource = new BitmapImage(new Uri(@"pack://application:,,,/FLBManager;Component/Resources/Images/Metro/ServiceArea.png"));
                    //menuItem.Header = Properties.Resources.AddServiceAreaHeader;
                    //menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    //menuItem.Command = AddServiceAreaCommand;

                    //_ContextMenuItems.Add(menuItem);

                    //MenuComamnd menuItem = new MenuComamnd(); ;
                    //imageSource = new BitmapImage(new Uri(@"pack://application:,,,/FLBManager;Component/Resources/Images/Metro/CallerIDLine16.png"));
                    //menuItem.Header = Properties.Resources.AddCallerIDLine;
                    //menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    //menuItem.Command = AddCallerIDLineCommand;
                    //_ContextMenuItems.Add(menuItem);

                    _ContextMenuItems.Add(null);

                    menuItem = new MenuCommand();
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/delete.png"));
                    menuItem.Header = Properties.Resources.RemoveCallerIDServer;
                    menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = DeleteCommand;

                    _ContextMenuItems.Add(menuItem);



                }
                //if (_ContextMenuItems == null)
                //{
                //    _ContextMenuItems = new List<MenuComamnd>();
                //}
                return _ContextMenuItems;
            }
        }

        List<FBResourceTreeNode> _Members = new List<FBResourceTreeNode>();
        public override List<FBResourceTreeNode> Members
        {
            get
            {
                return _Members;
            }
        }


        public override string Name
        {
            get
            {
                return CallerIDLine.LineDescription;
            }

            set
            {
                CallerIDLine.LineDescription = value;
            }
        }
        public override bool HasContextMenu
        {
            get
            {
                return true;
            }
        }
        public override bool IsEditable
        {
            get
            {
                return true;
            }
        }

        public override List<MenuCommand> SelectedItemContextMenuItems
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


        public override ImageSource TreeImage
        {
            get
            {
                return new BitmapImage(new Uri(@"pack://application:,,,/FLBManager;Component/Resources/Images/Metro/CallerIDLine16.png"));
            }
        }

        public override void SelectionChange()
        {
        }
    }
}
