using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FlavourBusinessFacade.ServicesContextResources;
using MenuItemsEditor.ViewModel;
using WPFUIElementObjectBind;

namespace FLBManager.ViewModel.Infrastructure
{
    /// <MetaDataID>{ca225e58-da85-4932-95f3-2a75a72839cc}</MetaDataID>
    public class InfrastructureTreeNode : FBResourceTreeNode, INotifyPropertyChanged, IDragDropTarget
    {
        public override void RemoveChild(FBResourceTreeNode treeNode)
        {
            throw new NotImplementedException();
        }
        public readonly FlavoursServicesContextPresentation ServicesContextPresentation;
        CallerIDServerTreeNode CallerIDServerTreeNode;
        public RelayCommand LaunchCallerIDServerCommand { get; protected set; }

        TreasuryTreeNode TreasuryTreeNode;
        Preparation.PreparationSationsTreeNode PreparationSattionsTreeNode;
        public InfrastructureTreeNode(FlavoursServicesContextPresentation servicesContextPresentation) : base(servicesContextPresentation)
        {
            ServicesContextPresentation = servicesContextPresentation;

            ServiceContextResources = ServicesContextPresentation.ServiceContextResources;

            var callerIDServer = ServiceContextResources.CallerIDServer;// ServicesContextPresentation.ServicesContext.CallerIDServer;

            if (callerIDServer != null)
                CallerIDServerTreeNode = new CallerIDServerTreeNode(this, callerIDServer);

            LaunchCallerIDServerCommand = new RelayCommand((object sender) =>
            {
                LaunchCallerIDServer();
            });

            LaunchHomeDeliveryCommand = new RelayCommand((object sender) =>
            {
                LaunchHomeDeliveryService();
            });

            TreasuryTreeNode = new TreasuryTreeNode(this);
            PreparationSattionsTreeNode = new Preparation.PreparationSationsTreeNode(this);

        }

        private void LaunchHomeDeliveryService()
        {

            if (HomeDeliveryServiceTreeNode == null)
            {
                ServicesContextPresentation.ServicesContext.LaunchHomeDeliveryService();
                var homeDeliveryService = ServicesContextPresentation.ServicesContext.DeliveryServicePoint;
                if (homeDeliveryService != null)
                    HomeDeliveryServiceTreeNode = new HomeDeliveryServiceTreeNode(this, homeDeliveryService);

                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
            }
        }
        private void LaunchCallerIDServer()
        {
            if (CallerIDServerTreeNode == null)
            {
                ServicesContextPresentation.ServicesContext.LaunchCallerIDServer();
                var callerIDServer = ServicesContextPresentation.ServicesContext.CallerIDServer;
                if (callerIDServer != null)
                    CallerIDServerTreeNode = new CallerIDServerTreeNode(this, callerIDServer);

                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));


            }
        }

        public RelayCommand LaunchHomeDeliveryCommand { get; protected set; }

        

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

                    //MenuComamnd menuItem = new MenuComamnd();
                    //imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Rename16.png"));
                    //menuItem.Header = MenuItemsEditor.Properties.Resources.TreeNodeRenameMenuItemHeader;
                    //menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    //menuItem.Command = RenameCommand;
                    //_ContextMenuItems.Add(menuItem);


                    //_ContextMenuItems.Add(null);

                    //menuItem = new MenuComamnd(); ;
                    //imageSource = new BitmapImage(new Uri(@"pack://application:,,,/FLBManager;Component/Resources/Images/Metro/ServiceArea.png"));
                    //menuItem.Header = Properties.Resources.AddServiceAreaHeader;
                    //menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    //menuItem.Command = AddServiceAreaCommand;

                    //_ContextMenuItems.Add(menuItem);

                    MenuCommand menuItem = new MenuCommand(); ;
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/FLBManager;Component/Resources/Images/Metro/CallerID16.png"));
                    menuItem.Header = Properties.Resources.LaunchCallerIDServer;
                    menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = LaunchCallerIDServerCommand;
                    _ContextMenuItems.Add(menuItem);

                    menuItem = new MenuCommand(); ;
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/FLBManager;Component/Resources/Images/Metro/delivery-bike16.png"));
                    menuItem.Header = Properties.Resources.LaunchCallerIDServer;
                    menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = LaunchHomeDeliveryCommand;
                    _ContextMenuItems.Add(menuItem);

                    //_ContextMenuItems.Add(null);

                    //menuItem = new MenuComamnd();
                    //imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/delete.png"));
                    //menuItem.Header = Properties.Resources.RemoveCallerIDServer;
                    //menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    //menuItem.Command = DeleteCommand;

                    //_ContextMenuItems.Add(menuItem);



                }

                return _ContextMenuItems;
            }
        }

        public override List<FBResourceTreeNode> Members
        {
            get
            {
                List<FBResourceTreeNode> members = new List<FBResourceTreeNode>();
                members.Add(PreparationSattionsTreeNode);
                members.Add(TreasuryTreeNode);
                if (CallerIDServerTreeNode != null)
                    members.Add(CallerIDServerTreeNode);

                return members;
            }
        }

        public override string Name
        {
            get
            {
                return "Infrastructure";
            }

            set
            {
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
                var imageSource = new BitmapImage(new Uri(@"pack://application:,,,/FLBManager;Component/Resources/Images/Metro/infrastructure.png"));
                return imageSource;
            }
        }

        internal void RemoveCallerIDServer()
        {
            if (CallerIDServerTreeNode != null)
            {
                ServicesContextPresentation.ServicesContext.RemoveCallerIDServer();
                CallerIDServerTreeNode = null;

                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));

            }
        }

        public override void SelectionChange()
        {
        }

        DateTime DragEnterStartTime;
        public void DragEnter(object sender, DragEventArgs e)
        {
            DragItemsCategory dragItemsCategory = e.Data.GetData(typeof(DragItemsCategory)) as DragItemsCategory;
            if (dragItemsCategory != null)
            {
                e.Effects = DragDropEffects.Copy;
                DragEnterStartTime = DateTime.Now;
            }
            else
                e.Effects = DragDropEffects.None;

        }

        public void DragLeave(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;
        }

        public void DragOver(object sender, DragEventArgs e)
        {
            DragItemsCategory dragItemsCategory = e.Data.GetData(typeof(DragItemsCategory)) as DragItemsCategory;
            if (dragItemsCategory != null)
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

        public void Drop(object sender, DragEventArgs e)
        {
        }

        public override bool HasContextMenu
        {
            get
            {
                return true;
            }
        }

        public ServiceContextResources ServiceContextResources { get; private set; }
        public HomeDeliveryServiceTreeNode HomeDeliveryServiceTreeNode { get; private set; }
    }
}
