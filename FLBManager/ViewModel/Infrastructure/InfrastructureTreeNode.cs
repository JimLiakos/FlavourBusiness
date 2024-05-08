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
using FLBManager.ViewModel.Delivery;
using FLBManager.ViewModel.TakeAway;
using MenuItemsEditor.ViewModel;
using WPFUIElementObjectBind;

namespace FLBManager.ViewModel.Infrastructure
{
    /// <MetaDataID>{ca225e58-da85-4932-95f3-2a75a72839cc}</MetaDataID>
    public class InfrastructureTreeNode : FBResourceTreeNode, INotifyPropertyChanged, IDragDropTarget
    {
        /// <MetaDataID>{ed252185-b4e1-4a2a-b2ab-b5b0c1b38ec4}</MetaDataID>
        public override void RemoveChild(FBResourceTreeNode treeNode)
        {
            throw new NotImplementedException();
        }
        /// <MetaDataID>{40f47d64-e939-459f-97b0-84e6d8a65e63}</MetaDataID>
        public readonly FlavoursServicesContextPresentation ServicesContextPresentation;
        /// <MetaDataID>{b39a2fb7-fabd-4d05-8d30-7da1b92041fb}</MetaDataID>
        CallerIDServerTreeNode CallerIDServerTreeNode;
        /// <MetaDataID>{9b6ad120-e0d7-4a28-92b0-855e9fe57f40}</MetaDataID>
        public RelayCommand LaunchCallerIDServerCommand { get; protected set; }

        /// <MetaDataID>{56da9d01-9f6c-49e0-947a-3ac54216aab1}</MetaDataID>
        TreasuryTreeNode TreasuryTreeNode;
        /// <MetaDataID>{79957b34-1702-40bf-96f2-ffd2008e5482}</MetaDataID>
        Preparation.PreparationSationsTreeNode PreparationSattionsTreeNode;

        /// <MetaDataID>{219c3fea-ee04-499e-8e09-4f07f9aa5435}</MetaDataID>
        internal TakeAwayStationsTreeNode TakeAwayStationsTreeNode;

        /// <MetaDataID>{b4f15201-9ba7-4640-bd3f-61452b2c7788}</MetaDataID>
        public DeliveryCallCenterStationsTreeNode DeliveryCallCenterStationsTreeNode { get; }

        /// <MetaDataID>{11f6cbbf-bfac-4fff-bd89-0478202df17a}</MetaDataID>
        internal PaymentTerminalsTreeNode PaymentTerminalsTreeNode { get; }

        /// <MetaDataID>{16bc257b-61d3-46d8-95d3-4ad15695ce7c}</MetaDataID>
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


            TreasuryTreeNode = new TreasuryTreeNode(this);
            PreparationSattionsTreeNode = new Preparation.PreparationSationsTreeNode(this);
            TakeAwayStationsTreeNode=new TakeAwayStationsTreeNode(this);
            DeliveryCallCenterStationsTreeNode=new DeliveryCallCenterStationsTreeNode(this);
            PaymentTerminalsTreeNode =new PaymentTerminalsTreeNode(this);
        }


        /// <MetaDataID>{a0803aaf-fc6f-4023-a9ab-624fbe090cd1}</MetaDataID>
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



        /// <MetaDataID>{37f6614d-d886-409e-9122-cbb25909e8f5}</MetaDataID>
        List<MenuCommand> _ContextMenuItems;
        /// <MetaDataID>{7a46f33a-ad15-48df-8631-08bae13134fd}</MetaDataID>
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

        /// <MetaDataID>{4b6639e0-f3f4-4369-8d8c-30eb91b63a67}</MetaDataID>
        public override List<FBResourceTreeNode> Members
        {
            get
            {
                List<FBResourceTreeNode> members = new List<FBResourceTreeNode>();
                members.Add(PreparationSattionsTreeNode);
                members.Add(TakeAwayStationsTreeNode);
                members.Add(DeliveryCallCenterStationsTreeNode);
                members.Add(TreasuryTreeNode);
                members.Add(PaymentTerminalsTreeNode);

                if (CallerIDServerTreeNode != null)
                    members.Add(CallerIDServerTreeNode);

                return members;
            }
        }

        /// <MetaDataID>{36ea56b2-691f-4eba-a65d-382a93d505c9}</MetaDataID>
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

        /// <MetaDataID>{088f3638-d79b-4646-bc86-b5d3dbbcdf3e}</MetaDataID>
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

        /// <MetaDataID>{aaedb066-476e-4c5a-96ce-32ec7924f4c7}</MetaDataID>
        public override ImageSource TreeImage
        {
            get
            {
                var imageSource = new BitmapImage(new Uri(@"pack://application:,,,/FLBManager;Component/Resources/Images/Metro/infrastructure.png"));
                return imageSource;
            }
        }

        /// <MetaDataID>{206f71d6-966e-4b32-a147-c5a112a08714}</MetaDataID>
        internal void RemoveCallerIDServer()
        {
            if (CallerIDServerTreeNode != null)
            {
                ServicesContextPresentation.ServicesContext.RemoveCallerIDServer();
                CallerIDServerTreeNode = null;

                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));

            }
        }

        /// <MetaDataID>{9a423363-fd61-4bf8-bc01-3bb2922b96a1}</MetaDataID>
        public override void SelectionChange()
        {
        }

        /// <MetaDataID>{142e57bf-be32-48aa-b24c-e6bf2e6055cb}</MetaDataID>
        DateTime DragEnterStartTime;
        /// <MetaDataID>{5668fc70-8a9e-42dd-8b88-a6eed1fe6b39}</MetaDataID>
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

        /// <MetaDataID>{fe731d50-38c6-4806-9bca-111dc0c30969}</MetaDataID>
        public void DragLeave(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;
        }

        /// <MetaDataID>{ad9681a2-b821-4be1-899a-96da68cee2a2}</MetaDataID>
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

        /// <MetaDataID>{a1bed398-1223-4fa1-90cc-46f9aa8a8df2}</MetaDataID>
        public void Drop(object sender, DragEventArgs e)
        {
        }

        /// <MetaDataID>{d3d8de6d-e7b4-4707-9b9a-ee8ad6f26460}</MetaDataID>
        public override bool HasContextMenu
        {
            get
            {
                return true;
            }
        }

        /// <MetaDataID>{9d0581f5-61ae-4dee-a327-7a1a582426b0}</MetaDataID>
        public ServiceContextResources ServiceContextResources { get; private set; }

    }
}
