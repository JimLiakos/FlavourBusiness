using FlavourBusinessFacade.ServicesContextResources;
using FLBManager.ViewModel.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using WPFUIElementObjectBind;
using MenuItemsEditor.ViewModel;
using System.Windows;
using MenuDesigner.ViewModel;

namespace FLBManager.ViewModel.TakeAway
{
    public class TakeAwayStationTreeNode : FBResourceTreeNode, IGraphicMenusOwner, INotifyPropertyChanged, IDragDropTarget
    {

        public ITakeAwayStation TakeAwayStation;


        public override void RemoveChild(FBResourceTreeNode treeNode)
        {
            throw new NotImplementedException();
        }

        internal TakeAwayStationsTreeNode TakeAwayStationsTreeNode;
        public TakeAwayStationTreeNode(TakeAwayStationsTreeNode parent, ITakeAwayStation takeAwayStation) : base(parent)
        {




            TakeAwayStationsTreeNode = parent;
            this.TakeAwayStation = takeAwayStation;



            RenameCommand = new RelayCommand((object sender) =>
            {
                EditMode();
            });
            DeleteCommand = new RelayCommand((object sender) =>
            {
                Delete();
            });

            string graphicMenuStorageIdentity = takeAwayStation?.GraphicMenuStorageIdentity;
            if (!string.IsNullOrWhiteSpace(graphicMenuStorageIdentity))
            {
                var graphicMenuTreeNode = parent.ServiceContextInfrastructure.ServicesContextPresentation.Company.GraphicMenus.Where(x => x.GraphicMenuStorageRef.StorageIdentity==takeAwayStation.GraphicMenuStorageIdentity).FirstOrDefault();
                GraphicMenuTreeNode=    new GraphicMenuTreeNode(graphicMenuTreeNode.GraphicMenuStorageRef?.Clone(), graphicMenuTreeNode.MenuItemsStorageRef?.Clone(), this, this, false);
            }
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
            TakeAwayStationsTreeNode.RemoveTakeAwayStation(this);

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
        private DateTime DragEnterStartTime;

        public override List<FBResourceTreeNode> Members
        {
            get
            {
                var member = _Members.ToList();
                if (GraphicMenuTreeNode!=null)
                    member.Add(GraphicMenuTreeNode);
                return member;
            }
        }


        public override string Name
        {
            get
            {
                return TakeAwayStation.Description;
            }

            set
            {
                TakeAwayStation.Description = value;
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
                return new BitmapImage(new Uri(@"pack://application:,,,/FLBManager;Component/Resources/Images/Metro/pos-terminal16.png"));
            }
        }

        public List<GraphicMenuTreeNode> GraphicMenus => throw new NotImplementedException();

        public bool NewGraphicMenuAllowed => throw new NotImplementedException();

        public GraphicMenuTreeNode GraphicMenuTreeNode { get; private set; }

        public override void SelectionChange()
        {
        }

        public void DragEnter(object sender, DragEventArgs e)
        {
            DragEnterStartTime = DateTime.Now;

            MenuDesigner.ViewModel.GraphicMenuTreeNode graphicMenu = e.Data.GetData(typeof(MenuDesigner.ViewModel.GraphicMenuTreeNode)) as MenuDesigner.ViewModel.GraphicMenuTreeNode;
            if (graphicMenu != null)
            {
                e.Effects = DragDropEffects.Copy;
                DragEnterStartTime = DateTime.Now;
            }
            else
                e.Effects = DragDropEffects.None;
        }

        /// <MetaDataID>{7685fc3f-422b-4f0f-bbc2-ce73147fc26b}</MetaDataID>
        public void DragLeave(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;
        }

        /// <MetaDataID>{50554c50-c0c4-4324-bc16-4e2494ee9e78}</MetaDataID>
        public void DragOver(object sender, DragEventArgs e)
        {

            MenuDesigner.ViewModel.GraphicMenuTreeNode graphicMenu = e.Data.GetData(typeof(MenuDesigner.ViewModel.GraphicMenuTreeNode)) as MenuDesigner.ViewModel.GraphicMenuTreeNode;
            if (graphicMenu != null)
            {
                if ((DateTime.Now - DragEnterStartTime).TotalSeconds > 2)
                {
                    IsNodeExpanded = true;
                    RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(IsNodeExpanded)));
                }
            }
            else
                e.Effects = DragDropEffects.None;





            System.Diagnostics.Debug.WriteLine("DragOver InfrastructureTreeNode");
        }

        /// <MetaDataID>{6d4862ac-5434-4fc0-8e58-f8ec7710fec4}</MetaDataID>
        public void Drop(object sender, DragEventArgs e)
        {
            MenuDesigner.ViewModel.GraphicMenuTreeNode graphicMenu = e.Data.GetData(typeof(MenuDesigner.ViewModel.GraphicMenuTreeNode)) as MenuDesigner.ViewModel.GraphicMenuTreeNode;
            if (graphicMenu != null)
            {
                this.TakeAwayStation.GraphicMenuStorageIdentity=graphicMenu.GraphicMenuStorageRef.StorageIdentity;
                //string graphicMenuUri = MenuPresentationModel.RestaurantMenu.GetGraphicMenuUri(graphicMenu.GraphicMenuStorageRef);
                //graphicMenu.string GraphicMenuUri
            }
        }

        public void AssignGraphicMenu(GraphicMenuTreeNode graphicMenuTreeNode)
        {
            //throw new NotImplementedException();
        }

        public bool CanAssignGraphicMenu(GraphicMenuTreeNode graphicMenuTreeNode)
        {
            return false;
        }

        public bool RemoveGraphicMenu(GraphicMenuTreeNode graphicMenuTreeNode)
        {
            this.TakeAwayStation.GraphicMenuStorageIdentity=null;
            GraphicMenuTreeNode=null;
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
            return true;
        }

        public void NewGraphicMenu()
        {

        }
    }

}
