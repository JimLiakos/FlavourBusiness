using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FlavourBusinessFacade.HumanResources;
using WPFUIElementObjectBind;

namespace FLBManager.ViewModel.HumanResources
{
    /// <MetaDataID>{8092abdf-1b90-473d-bacb-137a8b579642}</MetaDataID>
    public class StaffTreeNode : FBResourceTreeNode, INotifyPropertyChanged
    {

        public override void RemoveChild(FBResourceTreeNode treeNode)
        {
            throw new NotImplementedException();
        }
        /// <MetaDataID>{89a93c06-3460-48f5-9793-ad2f037c791a}</MetaDataID>
        public readonly FlavoursServicesContextPresentation ServicesContextPresentation;
        /// <MetaDataID>{d01b4ea4-c363-40b8-9b65-a9c0f82f5ae9}</MetaDataID>
        Infrastructure.CallerIDServerTreeNode CallerIDServerTreeNode;

        /// <MetaDataID>{79316e75-87f2-44aa-bafe-9420bfd1a9a7}</MetaDataID>
        WaitersTreeNode WaitersTreeNode;

        /// <MetaDataID>{4cbad7b2-74a6-4b86-83d0-35dddb551f05}</MetaDataID>
        AdministrationTreeNode AdministrationTreeNode;

        /// <MetaDataID>{ff001114-444e-496d-a21c-fe606219f522}</MetaDataID>
        public StaffTreeNode(FlavoursServicesContextPresentation servicesContextPresentation) : base(servicesContextPresentation)
        {
            ServicesContextPresentation = servicesContextPresentation;

            ServiceContextHumanResources = ServicesContextPresentation.ServiceContextHumanResources;

            WaitersTreeNode = new WaitersTreeNode(this);
            AdministrationTreeNode = new AdministrationTreeNode(this);
        }

        /// <MetaDataID>{8c38f22e-e60c-4228-8121-56e02858e6e3}</MetaDataID>
        public void RefreshPresentation()
        {
            ServiceContextHumanResources = ServicesContextPresentation.ServiceContextHumanResources;
            AdministrationTreeNode.RefreshPresentation();
            WaitersTreeNode.RefreshPresentation();
        }



        /// <MetaDataID>{c2c4056f-5238-4336-b0aa-f4f64c924f91}</MetaDataID>
        public RelayCommand LaunchCallerIDServerCommand { get; protected set; }

        /// <MetaDataID>{1f33bb95-2a33-4301-a821-22345003de4e}</MetaDataID>
        List<MenuCommand> _ContextMenuItems;
        /// <MetaDataID>{bc156757-a5f1-4ba9-b825-84e1bc92c46e}</MetaDataID>
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

        /// <MetaDataID>{dba420a0-f33c-4a61-8b43-47b952e636f2}</MetaDataID>
        public override List<FBResourceTreeNode> Members
        {
            get
            {
                List<FBResourceTreeNode> members = new List<FBResourceTreeNode>();
                members.Add(AdministrationTreeNode);
                members.Add(WaitersTreeNode);
                return members;
            }
        }

        /// <MetaDataID>{6a586ea7-0623-426b-b85a-1eae3b0bb4ec}</MetaDataID>
        public override string Name
        {
            get
            {
                return "Human Resources";
            }

            set
            {
            }
        }

        /// <MetaDataID>{342d5e7d-71f1-4670-a45d-e2af579af25e}</MetaDataID>
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

        /// <MetaDataID>{ddc7f735-0fdb-45fe-a698-481aef24a370}</MetaDataID>
        public override ImageSource TreeImage
        {
            get
            {
                return new System.Windows.Media.Imaging.BitmapImage(new Uri(@"pack://application:,,,/FLBManager;Component/Resources/Images/Metro/human-resources16.png"));
            }
        }

        /// <MetaDataID>{52d8c910-7823-4179-bb6c-2284d7dc7429}</MetaDataID>
        internal void RemoveCallerIDServer()
        {
            if (CallerIDServerTreeNode != null)
            {
                ServicesContextPresentation.ServicesContext.RemoveCallerIDServer();
                CallerIDServerTreeNode = null;

                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));

            }
        }

        /// <MetaDataID>{1d449ad9-37cb-40a9-9846-700b43c8ff69}</MetaDataID>
        public override void SelectionChange()
        {
        }





        /// <MetaDataID>{958e0b74-0381-477c-b55e-1e2135267dd6}</MetaDataID>
        public override bool HasContextMenu
        {
            get
            {
                return true;
            }
        }

        /// <MetaDataID>{ada949f2-3d0f-4d39-bb60-2ce545211a34}</MetaDataID>
        public ServiceContextHumanResources ServiceContextHumanResources { get; private set; }
    }

}
