using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FlavourBusinessFacade;
using FlavourBusinessFacade.HumanResources;
using FlavourBusinessFacade.ServicesContextResources;
using FLBManager.ViewModel.HumanResources;
using FLBManager.ViewModel.Infrastructure;
using FloorLayoutDesigner.ViewModel;
using MenuDesigner.ViewModel;
using OOAdvantech.Remoting.RestApi;
using OOAdvantech.Transactions;
using StyleableWindow;
using WPFUIElementObjectBind;

namespace FLBManager.ViewModel
{
    /// <MetaDataID>{0d1d707a-9f9c-473c-a556-daedf99c71b2}</MetaDataID>
    public class FlavoursServicesContextPresentation : FBResourceTreeNode, INotifyPropertyChanged, IDragDropTarget, IServiceAreaTreeNodeOwner, IGraphicMenusOwner
    {
        /// <MetaDataID>{bbb2d3bf-b421-4173-a6ed-16cc837e6d9a}</MetaDataID>
        public override void RemoveChild(FBResourceTreeNode treeNode)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{a6c9d015-f393-40ed-88ab-8914d4d64c90}</MetaDataID>
        Dictionary<IServiceArea, ServiceAreaPresentation> ServiceAreas = new Dictionary<IServiceArea, ServiceAreaPresentation>();

        /// <MetaDataID>{a9bc3a17-19fb-46e4-b77f-ea6ceb25dabd}</MetaDataID>
        public IFlavoursServicesContextRuntime FlavoursServicesContextRuntime;


        /// <MetaDataID>{06fd7155-b404-42fd-aa4d-32a7f752a640}</MetaDataID>
        Dictionary<string, GraphicMenuTreeNode> _GraphicMenus = new Dictionary<string, GraphicMenuTreeNode>();



        /// <MetaDataID>{8ce87a1f-4913-48ae-b6e4-1825b468414d}</MetaDataID>
        internal CompanyPresentation Company;

        /// <MetaDataID>{4243ee6f-aaf7-42d5-897e-02f4d68dee48}</MetaDataID>
        internal void RemoveGraphicMenu(OrganizationStorageRef graphicMenuStorageRef)
        {


            Task.Run(() =>
            {
                FlavoursServicesContextRuntime.RemoveGraphicMenu(graphicMenuStorageRef);
                _GraphicMenus.Clear();


                var graphicMenus = FlavoursServicesContextRuntime.GraphicMenus;


                if (MenusTreeNode == null && graphicMenus.Count > 0)
                    MenusTreeNode = new MenusTreeNode(Name + " " + Properties.Resources.GraphicMenusTitle, this, this);

                foreach (var graphicMenu in FlavoursServicesContextRuntime.GraphicMenus)
                    _GraphicMenus[graphicMenu.StorageIdentity] = new GraphicMenuTreeNode(graphicMenu, null, Company, MenusTreeNode, this, false);


                if (GraphicMenus.Count == 0)
                    MenusTreeNode = null;

                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));

                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Members)));
            });

        }

        /// <MetaDataID>{a80ba1bd-ddfb-440a-a4d8-5726b18158e9}</MetaDataID>
        internal ServiceContextResources ServiceContextResources;

        /// <MetaDataID>{5ce994a6-332e-426d-9d87-2514de79d960}</MetaDataID>
        internal ServiceContextHumanResources ServiceContextHumanResources;


        /// <MetaDataID>{4f2c794f-9d06-4ef9-9621-437893031974}</MetaDataID>
        bool CompanySignedIn;

        /// <MetaDataID>{a123ee8e-9353-4c12-9c31-43a8f98f1232}</MetaDataID>
        InfrastructureTreeNode InfrastructureTreeNode;

        /// <MetaDataID>{fbd2b3b1-7f84-4082-9a8d-e99d5f26c2af}</MetaDataID>
        StaffTreeNode StaffTreeNode;

        /// <MetaDataID>{f334631c-078e-482e-af74-8f5894bb877c}</MetaDataID>
        public RelayCommand LaunchHomeDeliveryCommand { get; protected set; }


        /// <MetaDataID>{472640f9-e3eb-41d7-b257-7e5e8b9327ad}</MetaDataID>
        public readonly IFlavoursServicesContext ServicesContext;
        /// <MetaDataID>{48e74a4f-7d97-445b-90ae-1f5e5cfdcc83}</MetaDataID>
        public FlavoursServicesContextPresentation(IFlavoursServicesContext servicesContext, CompanyPresentation company) : base(company)
        {
            ServicesContext = servicesContext;

            if (FlavoursServicesContextRuntime == null)
                FlavoursServicesContextRuntime = ServicesContext.GetRunTime();

            FlavoursServicesContextRuntime.ObjectChangeState += ServicesContext_ObjectChangeState;

            IsNodeExpanded = true;

            Company = company;
            CompanySignedIn = true;
            (Company as CompanyPresentation).CompanySignedOut += FlavoursServicesContextPresentation_CompanySignedOut;



            if (GraphicMenus.Count > 0)
                MenusTreeNode = new MenusTreeNode(Name + " " + Properties.Resources.GraphicMenusTitle, this, this);



            RenameCommand = new RelayCommand((object sender) =>
            {
                EditMode();
            });
            DeleteCommand = new RelayCommand((object sender) =>
            {
                Delete();
            });
            AddServiceAreaCommand = new RelayCommand((object sender) =>
            {
                AddServiceArea();
            });
            LaunchHomeDeliveryCommand = new RelayCommand((object sender) =>
            {
                LaunchHomeDeliveryService();
            });



            Task.Run(() =>
            {

                try
                {

                    ServiceContextResources = FlavoursServicesContextRuntime.ServiceContextResources;
                    ServiceContextHumanResources = FlavoursServicesContextRuntime.ServiceContextHumanResources;

                    var homeDeliveryService = FlavoursServicesContextRuntime.DeliveryServicePoint;
                    if (homeDeliveryService != null)
                        HomeDeliveryServiceTreeNode = new HomeDeliveryServiceTreeNode(this, homeDeliveryService);


                    foreach (var serviceArea in ServiceContextResources.ServiceAreas)
                        ServiceAreas.Add(serviceArea, new ServiceAreaPresentation(serviceArea, this, this));
                    while (this.Company.RestaurantMenus == null)
                    {
                        System.Threading.Thread.Sleep(1000);
                    }
                    InfrastructureTreeNode = new InfrastructureTreeNode(this);
                    StaffTreeNode = new StaffTreeNode(this);

                    //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Members)));



                    //FlavoursServicesContextRuntime.ObjectChangeState += FlavoursServicesContextRuntime_ObjectChangeState;
                    FlavoursServicesContextRuntime.Description = FlavoursServicesContextRuntime.Description;
                    if (CompanySignedIn)
                    {
                        if (DeviceAuthentication.AuthUser == null)
                        {

                        }


                        var graphicMenus = FlavoursServicesContextRuntime.GraphicMenus;

                        
                        if (MenusTreeNode == null && graphicMenus.Count > 0)
                            MenusTreeNode = new MenusTreeNode(Name + " " + Properties.Resources.GraphicMenusTitle, this, this);
                        foreach (var graphicMenu in FlavoursServicesContextRuntime.GraphicMenus)
                            _GraphicMenus[graphicMenu.StorageIdentity] = new GraphicMenuTreeNode(graphicMenu, null, this.Company, MenusTreeNode, this, false);



                        //foreach (var graphicMenu in FlavoursServicesContextRuntime.GraphicMenus)
                        //    GraphicMenus[graphicMenu.StorageIdentity] = new GraphicMenuTreeNode(graphicMenu,null, MenusTreeNode, false);
                        //if (MenusTreeNode == null && GraphicMenus.Count > 0)
                        //    MenusTreeNode = new MenusTreeNode(Name + " " + Properties.Resources.GraphicMenusTitle, this, GraphicMenus);

                    }

                    //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Members)));
                    RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
                }
                catch (Exception error)
                {

                    throw;
                }

            });

        }
        /// <MetaDataID>{f55cff4f-a1bf-4085-94da-4c4220995107}</MetaDataID>
        public HomeDeliveryServiceTreeNode HomeDeliveryServiceTreeNode { get; private set; }
        /// <MetaDataID>{e793e50a-e5e3-44e8-a8af-446b8d97eebe}</MetaDataID>
        private void LaunchHomeDeliveryService()
        {

            if (HomeDeliveryServiceTreeNode == null)
            {
                ServicesContext.LaunchHomeDeliveryService();
                var homeDeliveryService = FlavoursServicesContextRuntime.DeliveryServicePoint;
                if (homeDeliveryService != null)
                    HomeDeliveryServiceTreeNode = new HomeDeliveryServiceTreeNode(this, homeDeliveryService);

                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
            }
        }


        /// <MetaDataID>{7c25119b-4d82-47ab-9cb0-4ac9de2c65f8}</MetaDataID>
        private void ServicesContext_ObjectChangeState(object _object, string member)
        {
            ServiceContextHumanResources = ServicesContext.ServiceContextHumanResources;
            StaffTreeNode.RefreshPresentation();
            //StaffTreeNode = new StaffTreeNode(this);
            //StaffTreeNode.IsNodeExpanded = true;
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
        }

        /// <MetaDataID>{a803dce7-28d5-423f-8578-b1d4dad75dd3}</MetaDataID>
        private void FlavoursServicesContextPresentation_CompanySignedOut(object sender, EventArgs e)
        {
            if (FlavoursServicesContextRuntime != null)
                FlavoursServicesContextRuntime.ObjectChangeState -= FlavoursServicesContextRuntime_ObjectChangeState;
            CompanySignedIn = false;
        }

        /// <MetaDataID>{56f48612-d65d-48d3-bcab-b111b4b35074}</MetaDataID>
        private void FlavoursServicesContextRuntime_ObjectChangeState(object _object, string member)
        {
            //IFlavoursServicesContextRuntime flavoursServicesContextRuntime = _object as IFlavoursServicesContextRuntime;
            //string mm = flavoursServicesContextRuntime.Description;
        }

        /// <MetaDataID>{540b332e-f725-4a07-b134-c712cc0b8c57}</MetaDataID>
        public void RemoveServiceArea(ServiceAreaPresentation serviceAreaPresentation)
        {
            Task.Run(() =>
            {
                ServicesContext.RemoveServiceArea(serviceAreaPresentation.ServiceArea);
                ServiceAreas.Remove(serviceAreaPresentation.ServiceArea);
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Members)));
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
            });
        }

        /// <MetaDataID>{1d143479-950c-4c40-8c4a-2d5bb04c26f9}</MetaDataID>
        private void AddServiceArea()
        {
            Task.Run(() =>
            {
                var serviceArea = ServicesContext.NewServiceArea();
                serviceArea.Description = Properties.Resources.DefaultServiceAreaDescription;
                ServiceAreas.Add(serviceArea, new ServiceAreaPresentation(serviceArea, this, this));
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Members)));
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));

            });


        }

        /// <MetaDataID>{21a71789-b037-462d-93a6-d1ad6c2fc9d3}</MetaDataID>
        private void Delete()
        {
            (Company as CompanyPresentation).RemoveServicesContext(this);
        }

        /// <MetaDataID>{fa35edd2-18b0-4be3-9218-a071bb7dda6a}</MetaDataID>
        public void EditMode()
        {
            if (_Edit == true)
            {
                _Edit = !_Edit;
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Edit)));
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Edit)));
            }
            _Edit = true;

            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Edit)));
        }


        /// <MetaDataID>{3b589712-a28d-4925-9895-31d1b8ca202f}</MetaDataID>
        public RelayCommand RenameCommand { get; protected set; }

        /// <MetaDataID>{b31d818b-772d-4d5e-bac0-49bf7146fd7a}</MetaDataID>
        public RelayCommand AddServiceAreaCommand { get; protected set; }






        /// <MetaDataID>{48a3b73c-d1b1-4a5e-ac05-f91f10b2d1d1}</MetaDataID>
        public RelayCommand DeleteCommand { get; protected set; }

        /// <exclude>Excluded</exclude>
        List<MenuCommand> _ContextMenuItems;


        /// <MetaDataID>{99e3859f-3648-4507-8a58-892674c2275b}</MetaDataID>
        MenusTreeNode MenusTreeNode;
        /// <MetaDataID>{a06d42ce-922c-4d18-ad6a-30d853d6d0dc}</MetaDataID>
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


                    _ContextMenuItems.Add(null);

                    menuItem = new MenuCommand(); ;
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/FLBManager;Component/Resources/Images/Metro/ServiceArea.png"));
                    menuItem.Header = Properties.Resources.AddServiceAreaHeader;
                    menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = AddServiceAreaCommand;

                    _ContextMenuItems.Add(menuItem);


                    menuItem = new MenuCommand();
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/delete.png"));
                    menuItem.Header = Properties.Resources.RemoveServicesContextHeader;
                    menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = DeleteCommand;

                    _ContextMenuItems.Add(menuItem);
                    if (HomeDeliveryServiceTreeNode == null)
                    {
                        menuItem = new MenuCommand();
                        imageSource = new BitmapImage(new Uri(@"pack://application:,,,/FLBManager;Component/Resources/Images/Metro/delivery-bike16.png"));
                        menuItem.Header = Properties.Resources.LaunchHomeDeliveryService;
                        menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                        menuItem.Command = LaunchHomeDeliveryCommand;
                        _ContextMenuItems.Add(menuItem);
                    }



                }
                //if (_ContextMenuItems == null)
                //{
                //    _ContextMenuItems = new List<MenuComamnd>();
                //}
                return _ContextMenuItems;
            }
        }





        /// <MetaDataID>{3567ae5b-96be-4568-ae5d-70db1ab4e3b1}</MetaDataID>
        public override bool IsEditable
        {
            get
            {
                return true;
            }
        }




        /// <MetaDataID>{cf2152ae-d4e4-4b06-b383-b577f0a0819a}</MetaDataID>
        public override List<FBResourceTreeNode> Members
        {
            get
            {
                var members = ServiceAreas.Values.OfType<FBResourceTreeNode>().ToList();

                if (HomeDeliveryServiceTreeNode != null)
                    members.Add(HomeDeliveryServiceTreeNode);

                if (MenusTreeNode != null)
                    members.Add(MenusTreeNode);


                if (StaffTreeNode != null)
                    members.Add(StaffTreeNode);


                if (InfrastructureTreeNode != null)
                    members.Add(InfrastructureTreeNode);


                return members;
            }
        }


        /// <MetaDataID>{49915216-7a5b-4b2b-9779-b283932d71e6}</MetaDataID>
        public override string Name
        {
            get
            {
                return this.ServicesContext.Description;
            }

            set
            {
                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {
                    this.ServicesContext.Description = value;
                    stateTransition.Consistent = true;
                }
            }
        }

        /// <MetaDataID>{7794aef2-cf4b-4e8e-8c37-f43f82de2aa8}</MetaDataID>
        public override FBResourceTreeNode Parent
        {
            get
            {
                return Company;
            }

            set
            {
            }
        }
        /// <MetaDataID>{afa30e1a-955e-43f5-bbb2-f8fb61258e9d}</MetaDataID>
        public virtual bool HasContextMenu
        {
            get
            {
                return true;
            }
        }
        /// <MetaDataID>{9ae7889b-f864-4123-ab1d-3583a7c34445}</MetaDataID>
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

        /// <MetaDataID>{b8576801-2e2c-4363-9b8e-eb574f6c0bb1}</MetaDataID>
        public override ImageSource TreeImage
        {
            get
            {
                return new System.Windows.Media.Imaging.BitmapImage(new Uri(@"pack://application:,,,/FLBManager;Component/Resources/Images/Metro/Restaurant16.png"));

            }
        }

        /// <MetaDataID>{bda35925-5665-4a91-925e-35c2c6692d65}</MetaDataID>
        IFlavoursServicesContext IServiceAreaTreeNodeOwner.ServicesContext => ServicesContext;

        /// <MetaDataID>{acbf8a60-507c-4e1f-b2d9-87819c9e7261}</MetaDataID>
        public List<GraphicMenuTreeNode> GraphicMenus => _GraphicMenus.Values.ToList();

        /// <MetaDataID>{5131d595-0967-4835-b469-8f2e03020aa8}</MetaDataID>
        public bool NewGraphicMenuAllowed => false;

        //public event PropertyChangedEventHandler PropertyChanged;

        /// <MetaDataID>{5b4e7ef2-b49a-4a0c-918c-f1560b24e5aa}</MetaDataID>
        public void DragEnter(object sender, DragEventArgs e)
        {

            GraphicMenuTreeNode graphicMenuTreeNode = e.Data.GetData(typeof(GraphicMenuTreeNode)) as GraphicMenuTreeNode;
            if (graphicMenuTreeNode == null)
                e.Effects = DragDropEffects.None;

            if (!CanAssignGraphicMenu(graphicMenuTreeNode))
                e.Effects = DragDropEffects.None;

        }

        /// <MetaDataID>{57c420f9-1928-45c6-9d77-546e492574fb}</MetaDataID>
        public void DragLeave(object sender, DragEventArgs e)
        {
            GraphicMenuTreeNode graphicMenuTreeNode = e.Data.GetData(typeof(GraphicMenuTreeNode)) as GraphicMenuTreeNode;
            if (graphicMenuTreeNode == null)
                e.Effects = DragDropEffects.None;



        }

        /// <MetaDataID>{475a0786-9795-489f-8dd0-96ce3002d55c}</MetaDataID>
        public void DragOver(object sender, DragEventArgs e)
        {
            GraphicMenuTreeNode graphicMenuTreeNode = e.Data.GetData(typeof(GraphicMenuTreeNode)) as GraphicMenuTreeNode;
            if (graphicMenuTreeNode == null)
                e.Effects = DragDropEffects.None;

            if (!CanAssignGraphicMenu(graphicMenuTreeNode))
                e.Effects = DragDropEffects.None;

        }

        /// <MetaDataID>{d4522845-7554-4c71-b910-220bc917f56a}</MetaDataID>
        public void Drop(object sender, DragEventArgs e)
        {
            GraphicMenuTreeNode graphicMenuTreeNode = e.Data.GetData(typeof(GraphicMenuTreeNode)) as GraphicMenuTreeNode;
            if (graphicMenuTreeNode == null)
                e.Effects = DragDropEffects.None;
            else
                AssignGraphicMenu(graphicMenuTreeNode);

        }

        /// <MetaDataID>{19131433-9439-4786-bbbc-475640005678}</MetaDataID>
        public override void SelectionChange()
        {
        }

        /// <MetaDataID>{ab8e0804-223c-4f55-9e13-1e3f5bfe640d}</MetaDataID>
        public bool RemoveGraphicMenu(GraphicMenuTreeNode graphicMenuTreeNode)
        {
            //FlavoursServicesContextRuntime.RemoveGraphicMenu(graphicMenuTreeNode.GraphicMenuStorageRef);
            return false;
        }

        /// <MetaDataID>{db846dcd-5953-472a-bce8-46dc700128a6}</MetaDataID>
        public void AssignGraphicMenu(GraphicMenuTreeNode graphicMenuTreeNode)
        {
            if (_GraphicMenus.ContainsKey(graphicMenuTreeNode.GraphicMenuStorageRef.StorageIdentity))
                return;

            if (FlavoursServicesContextRuntime == null)
                FlavoursServicesContextRuntime = ServicesContext.GetRunTime();
            FlavoursServicesContextRuntime.AssignGraphicMenu(graphicMenuTreeNode.GraphicMenuStorageRef);

            _GraphicMenus[graphicMenuTreeNode.GraphicMenuStorageRef.StorageIdentity] = new GraphicMenuTreeNode(graphicMenuTreeNode.GraphicMenuStorageRef.Clone(), null,this.Company, MenusTreeNode, this, false);

            if (MenusTreeNode == null)
                MenusTreeNode = new MenusTreeNode(Name + " " + Properties.Resources.GraphicMenusTitle, this, this);
            else
                MenusTreeNode.Refresh();



            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));

        }

        /// <MetaDataID>{108306aa-9045-4180-8949-3338df3349a9}</MetaDataID>
        public bool CanAssignGraphicMenu(GraphicMenuTreeNode graphicMenuTreeNode)
        {
            if (MenusTreeNode == null)
                return true;

            if (MenusTreeNode.Members.OfType<GraphicMenuTreeNode>().Where(x => x.GraphicMenuStorageRef.StorageIdentity == graphicMenuTreeNode.GraphicMenuStorageRef.StorageIdentity).Count() == 0)
                return true;

            return false;


        }

        /// <MetaDataID>{296ea51c-4ebf-496c-a4d8-19a4a2ebf6cb}</MetaDataID>
        public void NewGraphicMenu()
        {

        }
    }

}
