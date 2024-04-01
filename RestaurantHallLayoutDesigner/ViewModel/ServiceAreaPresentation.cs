using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using FlavourBusinessFacade;
using FlavourBusinessFacade.ServicesContextResources;
using FlavourBusinessToolKit;
using FLBManager.ViewModel;
using FloorLayoutDesigner.ViewModel;
using MenuModel;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Transactions;
using RestaurantHallLayoutModel;
using WPFUIElementObjectBind;
using MenuItemsEditor.ViewModel.PriceList;
using System.Windows.Controls;
using OOAdvantech.UserInterface.Runtime;
using MenuItemsEditor.ViewModel;
using FlavourBusinessFacade.PriceList;

namespace FloorLayoutDesigner.ViewModel
{
    /// <MetaDataID>{2346d8c4-3061-4757-9e0e-9c039aaf0e89}</MetaDataID>
    public class ServiceAreaPresentation : FBResourceTreeNode, INotifyPropertyChanged, IDragDropTarget, IServiceAreaViewModel 
    {
        public override void RemoveChild(FBResourceTreeNode treeNode)
        {
            if (treeNode is PriceListPresentation)
                SalesPointViewModel. RemovePriceList(treeNode as PriceListPresentation);
        }
        List<FBResourceTreeNode> _TreeItems;
        public List<FBResourceTreeNode> TreeItems
        {
            get
            {
                if (_TreeItems == null)
                    _TreeItems = new List<FBResourceTreeNode>() { new ServiceAreaPresentation(ServiceArea, MealTypesViewModel) };
                return _TreeItems;

            }
        }

        /// <MetaDataID>{29a52b78-bc3a-499f-ae69-8ad80e0a014a}</MetaDataID>
        public IServiceArea ServiceArea { get; private set; }

        public List<IServicePointViewModel> ServicePoints
        {
            get
            {
                return _ServicePoints.Values.OfType<IServicePointViewModel>().ToList();
            }
        }

        public ServiceAreaPresentation() : base(null)
        {
        }

        /// <MetaDataID>{5b2b393c-dfb3-4d1c-987c-4d493d07adec}</MetaDataID>
        Dictionary<IServicePoint, ServicePointPresentation> _ServicePoints = new Dictionary<IServicePoint, ServicePointPresentation>();

        /// <MetaDataID>{c224a420-b8da-4f4a-9a0c-64c056294897}</MetaDataID>
        IServiceAreaTreeNodeOwner FlavoursServicesContext;

        public SalesPointViewModel SalesPointViewModel { get; }
        public PriceListsTreeNode PriceListsTreeNode { get; }

        /// <MetaDataID>{b7fc965f-51b4-4c09-9b24-f808131ab602}</MetaDataID>
        public ServiceAreaPresentation(IServiceArea serviceArea, FBResourceTreeNode parent, IServiceAreaTreeNodeOwner flavoursServicesContext) : base(parent)
        {
            ServiceArea = serviceArea;
            FlavoursServicesContext = flavoursServicesContext;
            
            
            SalesPointViewModel = new SalesPointViewModel(serviceArea as ISalesPoint,this, (HeaderNode as IPriceListsOwner).Organization);
            PriceListsTreeNode = SalesPointViewModel.PriceListsTreeNode;
            SalesPointViewModel.AssignedPriceListHasChanged += (salesPointViewModel) =>
            {
                _ContextMenuItems = null;
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(ContextMenuItems)));
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
                
            };

            //_Name = Properties.Resources.LoadingPrompt;




            GetServiceAreName();

            RenameCommand = new RelayCommand((object sender) =>
            {
                EditMode();
            });
            DeleteCommand = new RelayCommand((object sender) =>
            {
                Delete();
            });

            AssignPricelistCommand = new RelayCommand((object sender) =>
            {
                var priceList = UIProxy.GetRealObject<PriceListPresentation>(sender);
              SalesPointViewModel. AssignPriceList(priceList);

            });

            AddServicePointCommand = new RelayCommand((object sender) =>
            {
                AddServicePoint();
            });

            DesigneCommand = new RelayCommand((object sender) =>
             {
                 ShowHallLayoutDesigner();
             });

            ServedMealsCommand = new RelayCommand((object sender) =>
            {
                ShowServedMealTypesPage();
            });
            _Members = new List<FBResourceTreeNode>();
            Task.Run(() =>
            {
                foreach (var servicePoint in ServiceArea.ServicePoints)
                    _ServicePoints.Add(servicePoint, new ServicePointPresentation(servicePoint, this));

                _Members.AddRange(_ServicePoints.Values.OrderBy(x => x.Name).OfType<FBResourceTreeNode>().ToList());
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));

            });
        }


        MealTypesTreeNode MealTypesTreeNode;
        public ServiceAreaPresentation(IServiceArea serviceArea, MenuItemsEditor.ViewModel.MealTypesViewModel mealTypesViewModel) : base(null)
        {
            _Name = serviceArea.Description;
            ServiceArea = serviceArea;
            MealTypesViewModel = mealTypesViewModel;
            MealTypesTreeNode = new MealTypesTreeNode(this);
            _Members = new List<FBResourceTreeNode>() { MealTypesTreeNode };
            IsNodeExpanded = true;
            Task.Run(() =>
            {
                foreach (var servicePoint in ServiceArea.ServicePoints)
                    _ServicePoints.Add(servicePoint, new ServicePointPresentation(servicePoint, this, MealTypesViewModel));
                _Members.AddRange(_ServicePoints.Values.OrderBy(x => x.Name).OfType<FBResourceTreeNode>().ToList());
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));

            });

            CheckBoxVisibility = Visibility.Collapsed;
        }

        internal void RefreshMealTypes()
        {
            MealTypesTreeNode.RefreshMealTypes();
            foreach (var servicePoint in ServicePoints)
                servicePoint.RefreshMealTypes();


        }

        public System.Windows.Visibility CheckBoxVisibility
        {
            get; set;
        }

        private void ShowServedMealTypesPage()
        {
            System.Windows.Window win = System.Windows.Window.GetWindow(ServedMealsCommand.UserInterfaceObjectConnection.ContainerControl as System.Windows.DependencyObject);
            var pageDialogFrame = WPFUIElementObjectBind.ObjectContext.FindChilds<StyleableWindow.PageDialogFrame>(win).Where(x => x.Name == "PageDialogHost").FirstOrDefault();


            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
            {
                MealTypesViewModel = new MenuItemsEditor.ViewModel.MealTypesViewModel(ObjectStorage.GetStorageOfObject(HallLayoutDesignerHost.Current.RestaurantMenus.Menus[0]));
                var hallLayoutDesignerPage = new Views.HallMealTypesPage();
                hallLayoutDesignerPage.GetObjectContext().SetContextInstance(this);
                pageDialogFrame.ShowDialogPage(hallLayoutDesignerPage);
                stateTransition.Consistent = true;
            }

        }

        public HallLayout RestaurantHallLayout { get; private set; }

        /// <MetaDataID>{0730b610-b04b-49cf-a180-77a6d3a63c42}</MetaDataID>
        private async void ShowHallLayoutDesigner()
        {
            if (HallLayoutDesignerHost.Current != null)
            {
                if (RestaurantHallLayout == null)
                {

                    OrganizationStorageRef hallLayoutStorageRef = FlavoursServicesContext.ServicesContext.GetHallLayoutStorageForServiceArea(ServiceArea);

                    string appDataPath = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Microneme";
                    if (!System.IO.Directory.Exists(appDataPath))
                        System.IO.Directory.CreateDirectory(appDataPath);
                    appDataPath += "\\DontWaitWater";
                    if (!System.IO.Directory.Exists(appDataPath))
                        System.IO.Directory.CreateDirectory(appDataPath);
                    string temporaryStorageLocation = appDataPath + string.Format("\\{0}.xml", hallLayoutStorageRef.StorageIdentity.Replace("-", "") + hallLayoutStorageRef.Name);
                    HttpClient httpClient = new HttpClient();
                    var transaction = OOAdvantech.Transactions.Transaction.Current;
                    var dataStream = await httpClient.GetStreamAsync(hallLayoutStorageRef.StorageUrl);
                    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.RequiredNested))
                    {

                        RawStorageData storageData = new RawStorageData(XDocument.Load(dataStream), temporaryStorageLocation, hallLayoutStorageRef, FlavoursServicesContext.ServicesContext.UploadService);
                        var hallLayoutStorage = OOAdvantech.PersistenceLayer.ObjectStorage.OpenStorage(hallLayoutStorageRef.Name, storageData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                        OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(hallLayoutStorage);

                        RestaurantHallLayout = (from hallLayout in storage.GetObjectCollection<HallLayout>()
                                                select hallLayout).FirstOrDefault();
                        if (RestaurantHallLayout == null)
                        {
                            RestaurantHallLayout = new HallLayout() { Name = ServiceArea.Description, ServicesContextIdentity = ServiceArea.ServicesContextIdentity };
                            hallLayoutStorage.CommitTransientObjectState(RestaurantHallLayout);
                        }
                        RestaurantHallLayout.ServiceArea = ServiceArea;

                        stateTransition.Consistent = true;
                    }
                }

                if (RestaurantHallLayout != null)
                    HallLayoutDesignerHost.Current.ShowHallLayout(this);



            }


        }

        private async void GetServiceAreName()
        {
            if (await Task<bool>.Run(() =>
             {
                 try
                 {
                     _Name = ServiceArea.Description;
                     return true;
                 }
                 catch (Exception error)
                 {
                     throw;
                 }

             }))
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Name)));
        }

        public override event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                base.PropertyChanged += value;
            }
            remove
            {
                base.PropertyChanged -= value;
            }
        }

        /// <MetaDataID>{a321cbd9-8658-40c1-b133-0d673566e68f}</MetaDataID>
        private void AddServicePoint()
        {
            Task.Run(() =>
            {
                var servicePoint = ServiceArea.NewServicePoint();
                servicePoint.Description = Properties.Resources.DefaultServicePointDescription;
                _ServicePoints.Add(servicePoint, new ServicePointPresentation(servicePoint, this));

                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Members)));
            });
        }

        public Task<IServicePointViewModel> NewServicePoint()
        {
            return Task<IServicePointViewModel>.Run(() =>
             {
                 var servicePoint = ServiceArea.NewServicePoint();
                 servicePoint.Description = Properties.Resources.DefaultServicePointDescription;
                 var newServicePointPresentation = new ServicePointPresentation(servicePoint, this);
                 foreach (var servicePointPresentation in _ServicePoints.Values)
                     servicePointPresentation.IsSelected = false;

                 _ServicePoints.Add(servicePoint, newServicePointPresentation);

                 RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
                 IsNodeExpanded = true;
                 RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(IsNodeExpanded)));
                 newServicePointPresentation.IsSelected = true;


                 return newServicePointPresentation as IServicePointViewModel;
             });

        }

        public void SetServicePointName(string servicesPointIdentity, string name)
        {
            ServicePointPresentation servicePointPresentation = this.ServicePoints.Where(x => x.ServicePoint.ServicesPointIdentity == servicesPointIdentity).FirstOrDefault() as ServicePointPresentation;
            servicePointPresentation.Name = name;
        }
        public void SetServicePointSeats(string servicesPointIdentity, int seats)
        {
            ServicePointPresentation servicePointPresentation = this.ServicePoints.Where(x => x.ServicePoint.ServicesPointIdentity == servicesPointIdentity).FirstOrDefault() as ServicePointPresentation;
            servicePointPresentation.ServicePoint.Seats = seats;
        }

        /// <MetaDataID>{bd6609c5-6fb5-4e32-b435-bd7933797d85}</MetaDataID>
        private void Delete()
        {
            FlavoursServicesContext.RemoveServiceArea(this);

        }
        /// <MetaDataID>{1f3b7abd-6b95-47e4-9482-280751d5fb29}</MetaDataID>
        public override bool IsEditable
        {
            get
            {
                return true;
            }
        }

        /// <MetaDataID>{3dfbb603-e9ba-40d7-ae94-897e57297988}</MetaDataID>
        public override bool HasContextMenu
        {
            get
            {
                return true;
            }
        }
        /// <MetaDataID>{dd215d47-c22c-46f7-9fe5-bfa2ccfba8bc}</MetaDataID>
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
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Edit)));
        }
        /// <MetaDataID>{78b9591d-727f-43d6-8cee-982f1afde428}</MetaDataID>
        public RelayCommand RenameCommand { get; protected set; }


        /// <MetaDataID>{73dabb7e-dceb-4677-9ce2-c585eb94a453}</MetaDataID>
        public RelayCommand DeleteCommand { get; protected set; }

        public RelayCommand AssignPricelistCommand { get; protected set; }

        /// <MetaDataID>{9178cc6b-09ae-49bd-8c66-94fa7fe5fa97}</MetaDataID>
        public RelayCommand AddServicePointCommand { get; protected set; }

        /// <MetaDataID>{b58058b5-272d-42b5-aa75-a0c0bb4698e9}</MetaDataID>
        public RelayCommand DesigneCommand { get; protected set; }
        public RelayCommand ServedMealsCommand { get; protected set; }

        /// <exclude>Excluded</exclude>
        List<MenuCommand> _ContextMenuItems;
        /// <MetaDataID>{abfe64a8-9a3a-4066-bd68-960d50ab1171}</MetaDataID>
        public override List<MenuCommand> ContextMenuItems
        {
            get
            {
                var priceListsNodeEntries = this.HeaderNode.FBResourceTreeNodesDictionary.Where(x => x.Value.All(y => y is PriceListPresentation)).Select(x => x.Value.OfType<PriceListPresentation>().FirstOrDefault()).ToList();

                if (_ContextMenuItems == null)
                {

                    _ContextMenuItems = new List<MenuCommand>();

                    var imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Empty.png"));
                    var emptyImage = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };

                    MenuCommand menuItem = new MenuCommand();
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Rename16.png"));
                    menuItem.Header = Properties.Resources.TreeNodeRenameMenuItemHeader;
                    menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = RenameCommand;
                    _ContextMenuItems.Add(menuItem);




                    menuItem = new MenuCommand(); ;
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/RestaurantHallLayoutDesigner;Component/Resources/Images/Metro/ServicePoint.png"));
                    menuItem.Header = Properties.Resources.AddServicePointHeader;
                    menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = AddServicePointCommand;

                    _ContextMenuItems.Add(menuItem);

                    menuItem = new MenuCommand();
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/RestaurantHallLayoutDesigner;Component/Resources/Images/Metro/sketch16.png"));
                    menuItem.Header = Properties.Resources.DesignServiceAreaMenuContextHeader;
                    menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = DesigneCommand;
                    _ContextMenuItems.Add(menuItem);

                    menuItem = new MenuCommand();
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/RestaurantHallLayoutDesigner;Component/Resources/Images/Metro/serve16.png"));
                    menuItem.Header = Properties.Resources.ServedMealTypesMenuContextHeader;
                    menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = ServedMealsCommand;
                    _ContextMenuItems.Add(menuItem);


                    _ContextMenuItems.Add(null);

                    menuItem = new MenuCommand();
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/delete.png"));
                    menuItem.Header = Properties.Resources.RemoveServiceAreaHeader;
                    menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = DeleteCommand;

                    _ContextMenuItems.Add(menuItem);
                    PriceListsMenuItem = new MenuCommand();
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/price-list16.png"));
                    PriceListsMenuItem.Header = Properties.Resources.PriceListsManuHeader; //$"{Properties.Resources.AddPrefix} {priceList.Name}";
                    PriceListsMenuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };

                }
                //priceListsNodeEntries = new List<PriceListPresentation>();
                foreach (var priceList in priceListsNodeEntries.Where(x => SalesPointViewModel.PriceLists.Where(y => y.PriceListStorageRef.StorageIdentity == y.PriceListStorageRef.StorageIdentity).Count() == 0))
                {

                    if (PriceListsMenuItem.SubMenuCommands.Count() == 0)
                    {
                        MenuCommand menuItem = new MenuCommand();
                        BitmapImage imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/price-list16.png"));
                        menuItem.Header = $"{Properties.Resources.AddPrefix} {priceList.Name}";
                        menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                        menuItem.Command = AssignPricelistCommand;
                        menuItem.Parameter = priceList;
                        menuItem.Tag = priceList;
                        PriceListsMenuItem.AddMenucommand(menuItem);
                    }
                }
                if (PriceListsMenuItem.SubMenuCommands.Count > 0 && !_ContextMenuItems.Contains(PriceListsMenuItem))
                {
                    if (_ContextMenuItems.IndexOf(null) != 1)
                        _ContextMenuItems.Insert(_ContextMenuItems.IndexOf(null), PriceListsMenuItem);
                    else
                        _ContextMenuItems.Add(PriceListsMenuItem);
                }

                if (PriceListsMenuItem.SubMenuCommands.Count == 0 && _ContextMenuItems.Contains(PriceListsMenuItem))
                    _ContextMenuItems.Remove(PriceListsMenuItem);

                if (PriceListsMenuItem != null)
                {
                    foreach (var priceListMenuItem in PriceListsMenuItem.SubMenuCommands)
                        priceListMenuItem.Header = (priceListMenuItem.Tag as PriceListPresentation).Name;
                }

                return _ContextMenuItems;
            }
        }



        /// <exclude>Excluded</exclude>
        List<FBResourceTreeNode> _Members;

        /// <MetaDataID>{3930a9a5-c95b-4528-a912-885e71b3686c}</MetaDataID>
        public override List<FBResourceTreeNode> Members
        {
            get
            {
                if (SalesPointViewModel.PriceLists.Count > 0)
                    return new List<FBResourceTreeNode>() { PriceListsTreeNode }.Union(_Members).ToList();
                else
                    return _Members;

            }
        }
        /// <exclude>Excluded</exclude>
        string _Name;
        /// <MetaDataID>{c0092266-3058-4798-a172-a8135e5193a4}</MetaDataID>
        public override string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
                ServiceArea.Description = value;
            }
        }

        /// <MetaDataID>{39d35c94-d253-4246-86cd-31fd0f786aff}</MetaDataID>
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

        /// <MetaDataID>{4415b955-7763-404b-ac3d-7423718d3d06}</MetaDataID>
        public override ImageSource TreeImage
        {
            get
            {
                return new System.Windows.Media.Imaging.BitmapImage(new Uri(@"pack://application:,,,/RestaurantHallLayoutDesigner;Component/Resources/Images/Metro/ServiceArea.png"));
            }
        }

        public MenuItemsEditor.ViewModel.MealTypesViewModel MealTypesViewModel { get; set; }
        public MenuCommand PriceListsMenuItem { get; private set; }






        /// <MetaDataID>{32c8cf5b-2b92-43f8-b76f-e4a23e31b26c}</MetaDataID>
        public void DragEnter(object sender, DragEventArgs e)
        {
            //MenuItemsEditor.ViewModel.pr
            PriceListPresentation priceList = e.Data.GetData(typeof(PriceListPresentation)) as PriceListPresentation;
            if (priceList != null && SalesPointViewModel.CanAssignPriceList(priceList))
            {
                e.Effects = DragDropEffects.Copy;
            }
            else
                e.Effects = DragDropEffects.None;
        }

        /// <MetaDataID>{8ba9d096-ca95-4104-9e2e-fbe41ef1f9f3}</MetaDataID>
        public void DragLeave(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;
        }

        /// <MetaDataID>{7a97e4ff-2055-44ac-b3c1-7409db2ae413}</MetaDataID>
        public void DragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;
        }

        /// <MetaDataID>{f1b4f096-23f6-4b4a-b5c1-a12052a610f0}</MetaDataID>
        public void Drop(object sender, DragEventArgs e)
        {
            //ViewModel.GraphicMenuTreeNode graphicMenuTreeNode = e.Data.GetData(typeof(ViewModel.GraphicMenuTreeNode)) as ViewModel.GraphicMenuTreeNode;
            //if (graphicMenuTreeNode == null)
            //    e.Effects = DragDropEffects.None;
            //else
            //{
            //    var servicePointRunTime = ServicesContext.GetRunTime();
            //}

        }

        /// <MetaDataID>{b0f65f54-bf51-4ee5-9f40-c4b29211e6ee}</MetaDataID>
        public override void SelectionChange()
        {

        }

        public IServicePointViewModel GetServicePoint(string servicesPointIdentity)
        {
            return this.ServicePoints.Where(x => x.ServicePoint.ServicesPointIdentity == servicesPointIdentity).FirstOrDefault() as ServicePointPresentation;
        }

        public List<AssignedMealTypeViewMode> GetMealTypes(string servicesPointIdentity)
        {

            if (string.IsNullOrWhiteSpace(servicesPointIdentity))
                return (from mealTypeViewModel in MealTypesViewModel.MealTypes
                        select new AssignedMealTypeViewMode(mealTypeViewModel.MealType, this)).ToList();

            ServicePointPresentation servicePointPresentation = this.ServicePoints.Where(x => x.ServicePoint.ServicesPointIdentity == servicesPointIdentity).FirstOrDefault() as ServicePointPresentation;
            if (servicePointPresentation != null)
            {
                return (from mealTypeViewModel in MealTypesViewModel.MealTypes
                        select new AssignedMealTypeViewMode(mealTypeViewModel.MealType, servicePointPresentation)).ToList();
            }
            else
            {
                return (from mealTypeViewModel in MealTypesViewModel.MealTypes
                        select new AssignedMealTypeViewMode(mealTypeViewModel.MealType, this)).ToList();
            }

        }

        //#region IPriceListsOwner implementation

        //PriceListsTreeNode PriceListsTreeNode;

        //public MenuCommand PriceListsMenuItem { get; private set; }
        //public List<OrganizationStorageRef> PriceListStorageRefs { get; private set; }

        ///// <exclude>Excluded</exclude>
        //List<PriceListPresentation> _PriceList = new List<PriceListPresentation>();

        //public List<PriceListPresentation> PriceLists
        //{
        //    get
        //    {
        //        if (PriceListStorageRefs == null)
        //            PriceListStorageRefs = (ServiceArea as ISalesPoint).PriceLists;

        //        #region Sync serviceArea price list nodes
        //        foreach (var storageRef in PriceListStorageRefs)
        //        {
        //            if (_PriceList.Where(x => x.PriceListStorageRef.StorageIdentity == storageRef.StorageIdentity).Count() == 0)
        //            {
        //                var priceListsNodeEntries = this.HeaderNode.FBResourceTreeNodesDictionary.Where(x => x.Value.All(y => y is PriceListPresentation)).Select(x => x.Value.OfType<PriceListPresentation>().FirstOrDefault()).ToList();
        //                var priceListNode = priceListsNodeEntries.Where(x => x.PriceListStorageRef.StorageIdentity == storageRef.StorageIdentity).FirstOrDefault();
        //                _PriceList.Add(new PriceListPresentation(PriceListsTreeNode, priceListNode.PriceListStorageRef, priceListNode.MenuViewModel));
        //            }
        //        }
        //        foreach (var removedPriceList in _PriceList.Where(x => !PriceListStorageRefs.Any(y => y.StorageIdentity == x.PriceListStorageRef.StorageIdentity)).ToList())
        //        {
        //            _PriceList.Remove(removedPriceList);
        //        }

        //        #endregion

        //        return _PriceList.ToList();
        //    }
        //}





        //public void AssignPriceList(PriceListPresentation priceList)
        //{

        //    (ServiceArea as ISalesPoint).AssignPriceList(priceList.PriceListStorageRef);
        //    PriceListStorageRefs.Add(priceList.PriceListStorageRef);

        //    _ContextMenuItems = null;
        //    RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(ContextMenuItems)));
        //    RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
        //    PriceListsTreeNode.Refresh();

        //}

        //public bool CanAssignPriceList(PriceListPresentation PriceListTreeNode)
        //{
        //    return false;
        //}

        //public bool RemovePriceList(PriceListPresentation priceListTreeNode)
        //{
        //    var priceListStorageRef = PriceListStorageRefs.Where(x => x.StorageIdentity == priceListTreeNode.PriceListStorageRef.StorageIdentity).FirstOrDefault();
        //    if (priceListStorageRef != null)
        //    {
        //        (ServiceArea as ISalesPoint).RemovePriceList(priceListStorageRef);
        //        PriceListStorageRefs.Remove(priceListStorageRef);

        //    }
        //    _ContextMenuItems = null;
        //    RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
        //    RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(ContextMenuItems)));
        //    return true;
        //}

        //public bool NewPriceListAllowed => false;

        //public void NewPriceList()
        //{

        //}
        //#endregion


    }

    public delegate void AssignedPriceListHasChangedHandler(SalesPointViewModel salesPointViewModel);
    public class SalesPointViewModel: IPriceListsOwner
    {
        ISalesPoint SalesPoint;

        FBResourceTreeNode SalesPointNode;

        public event AssignedPriceListHasChangedHandler AssignedPriceListHasChanged;


        public SalesPointViewModel(ISalesPoint salesPoint, FBResourceTreeNode salesPointNode, IOrganization organization)
        {
            SalesPoint = salesPoint;
            SalesPointNode = salesPointNode;
            Organization = organization;
            PriceListsTreeNode =  new PriceListsTreeNode(MenuItemsEditor.Properties.Resources.PriceListsNodeName, salesPointNode, this); 
        }




        #region IPriceListsOwner implementation


       public readonly PriceListsTreeNode PriceListsTreeNode;

        public MenuCommand PriceListsMenuItem { get; private set; }
        public List<OrganizationStorageRef> PriceListStorageRefs { get; private set; }

        /// <exclude>Excluded</exclude>
        List<PriceListPresentation> _PriceList = new List<PriceListPresentation>();

        public List<PriceListPresentation> PriceLists
        {
            get
            {
                if (PriceListStorageRefs == null)
                    PriceListStorageRefs = SalesPoint.PriceLists;

                #region Sync serviceArea price list nodes
                foreach (var storageRef in PriceListStorageRefs)
                {
                    if (_PriceList.Where(x => x.PriceListStorageRef.StorageIdentity == storageRef.StorageIdentity).Count() == 0)
                    {
                        var priceListsNodeEntries = SalesPointNode.HeaderNode.FBResourceTreeNodesDictionary.Where(x => x.Value.All(y => y is PriceListPresentation)).Select(x => x.Value.OfType<PriceListPresentation>().FirstOrDefault()).ToList();
                        var priceListNode = priceListsNodeEntries.Where(x => x.PriceListStorageRef.StorageIdentity == storageRef.StorageIdentity).FirstOrDefault();
                        _PriceList.Add(new PriceListPresentation(PriceListsTreeNode, priceListNode.PriceListStorageRef, priceListNode.MenuViewModel, Organization));
                    }
                }
                foreach (var removedPriceList in _PriceList.Where(x => !PriceListStorageRefs.Any(y => y.StorageIdentity == x.PriceListStorageRef.StorageIdentity)).ToList())
                {
                    _PriceList.Remove(removedPriceList);
                }

                #endregion

                return _PriceList.ToList();
            }
        }





        public void AssignPriceList(PriceListPresentation priceList)
        {

            SalesPoint.AssignPriceList(priceList.PriceListStorageRef);
            PriceListStorageRefs.Add(priceList.PriceListStorageRef);

            AssignedPriceListHasChanged?.Invoke(this);

            PriceListsTreeNode.Refresh();

        }

        public bool CanAssignPriceList(PriceListPresentation PriceListTreeNode)
        {
            return false;
        }

        public bool RemovePriceList(PriceListPresentation priceListTreeNode)
        {
            var priceListStorageRef = PriceListStorageRefs.Where(x => x.StorageIdentity == priceListTreeNode.PriceListStorageRef.StorageIdentity).FirstOrDefault();
            if (priceListStorageRef != null)
            {
                SalesPoint.RemovePriceList(priceListStorageRef);
                PriceListStorageRefs.Remove(priceListStorageRef);
            }

            AssignedPriceListHasChanged?.Invoke(this);

                 return true;
        }

        public bool NewPriceListAllowed => false;

        public IOrganization Organization { get; }

        public void NewPriceList()
        {

        }
        #endregion
    }



    /// <MetaDataID>{572fa887-0e6d-440c-88c3-82122fd188bf}</MetaDataID>
    public interface IServiceAreaTreeNodeOwner
    {
        /// <MetaDataID>{80b6ff61-8819-4a73-bc49-a3887a134c81}</MetaDataID>
        IFlavoursServicesContext ServicesContext { get; }

        /// <MetaDataID>{a9fa9c29-6842-49ca-926e-c98004d7a206}</MetaDataID>
        void RemoveServiceArea(ServiceAreaPresentation serviceAreaPresentation);
    }





}

