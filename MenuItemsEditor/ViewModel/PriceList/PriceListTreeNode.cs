using FlavourBusinessFacade;
using FlavourBusinessFacade.PriceList;
using FlavourBusinessFacade.ServicesContextResources;
using FlavourBusinessManager.PriceList;
using FlavourBusinessToolKit;
using FLBManager.ViewModel;
using MenuItemsEditor.ViewModel;
using MenuModel;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using StyleableWindow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPFUIElementObjectBind;

namespace MenuItemsEditor.ViewModel.PriceList
{
    public class PriceListPresentation : FBResourceTreeNode, INotifyPropertyChanged
    {
        public PriceListPresentation() : base(null)
        {

        }
        public PriceListPresentation(FBResourceTreeNode parent, OrganizationStorageRef priceListStorageRef, MenuViewModel menuViewModel) : base(parent)
        {
            PriceListStorageRef = priceListStorageRef;
            MenuViewModel = menuViewModel;


            //string localFileName = $"{System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\\Microneme\\DontWaitWater\\{priceListStorageRef.Name}.xml";
            //var rawStorageData = new RawStorageData(localFileName, priceListStorageRef, priceListStorageRef.UploadService);

            //var priceListObjectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.OpenStorage("PriceList", rawStorageData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
            //OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(priceListObjectStorage);

            //var priceList = (from m_priceList in storage.GetObjectCollection<IPriceList>()
            //                 select m_priceList).FirstOrDefault();


            EditCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                System.Windows.Window win = System.Windows.Window.GetWindow(EditCommand.UserInterfaceObjectConnection.ContainerControl as System.Windows.DependencyObject);
                EditMenuItem(win);
            });
        }


        IPriceList _PriceList = null;


        public IPriceList PriceList
        {
            get
            {
                if (_PriceList == null)
                {
                    string localFileName = $"{System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\\Microneme\\DontWaitWater\\{PriceListStorageRef.Name}.xml";
                    var rawStorageData = new RawStorageData(localFileName, PriceListStorageRef, PriceListStorageRef.UploadService);

                    var priceListObjectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.OpenStorage("PriceList", rawStorageData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                    OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(priceListObjectStorage);

                    _PriceList = (from m_priceList in storage.GetObjectCollection<IPriceList>()
                                  select m_priceList).FirstOrDefault();
                }

                return _PriceList;
            }
        }


        /// <MetaDataID>{25d5dfb4-441e-425c-bfb6-e3c610ba2b76}</MetaDataID>
        List<FBResourceTreeNode> _ItemsToChoose;
        /// <MetaDataID>{81287596-6671-46ba-80fa-133bd488c72c}</MetaDataID>
        public List<FBResourceTreeNode> ItemsToChoose
        {
            get
            {
                if (_ItemsToChoose == null)
                {

                    var rootCategory = new ItemsPriceInfoPresentation(this, MenuViewModel.Menu, true);
                    rootCategory.CheckBoxVisibility = Visibility.Collapsed;
                    rootCategory.IsNodeExpanded = true;
                    _ItemsToChoose = new List<FBResourceTreeNode>() { rootCategory };
                }
                return _ItemsToChoose;
            }
        }
        private void EditMenuItem(System.Windows.Window owner)
        {
            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
            {
                var frame = PageDialogFrame.LoadedPageDialogFrames.FirstOrDefault();// WPFUIElementObjectBind.ObjectContext.FindChilds<PageDialogFrame>(win).Where(x => x.Name == "PageDialogHost").FirstOrDefault();
                Views.PriceList.PriceListPage priceListPage = new Views.PriceList.PriceListPage();
                priceListPage.GetObjectContext().SetContextInstance(this);

                frame.ShowDialogPage(priceListPage);
                stateTransition.Consistent = true;
            }
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Name)));
        }

        bool Editable = true;

        OrganizationStorageRef PriceListStorageRef = null;

        public MenuViewModel MenuViewModel { get; }

        public override string Name
        {
            get => PriceListStorageRef.Name;
            set
            {
                if (PriceListStorageRef.Name != value)
                {
                    string oldName = PriceListStorageRef.Name;
                    if (!string.IsNullOrWhiteSpace(value) && System.IO.Path.GetInvalidFileNameChars().Where(x => value.IndexOf(x) != -1).Count() > 0)
                    {
                        var messageBoxResult = StyleableWindow.WpfMessageBox.Show("Graphic menu name", "The graphic name has invalid characters !", MessageBoxButton.OK, StyleableWindow.MessageBoxImage.Error);



                        Task.Run(() =>
                        {
                            SetOtherPriceListTreeNodesName(oldName);
                            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Name)));

                        });
                        return;
                    }
                    PriceListStorageRef.Name = value;
                    SetOtherPriceListTreeNodesName(value);
                    if (Editable)
                    {
                        OrganizationStorageRef storageRef = null;
                        try
                        {
                            storageRef = (FlavourBusinessManager.Organization.CurrentOrganization as FlavourBusinessFacade.IResourceManager).UpdateStorage(PriceListStorageRef.Name, PriceListStorageRef.Description, PriceListStorageRef.StorageIdentity);
                        }
                        catch (Exception error)
                        {
                            Task.Run(() =>
                            {
                                PriceListStorageRef.Name = oldName;
                                SetOtherPriceListTreeNodesName(oldName);
                                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Name)));
                            });
                            return;
                        }

                        PriceListStorageRef.StorageUrl = storageRef.StorageUrl;
                        PriceListStorageRef.Name = storageRef.Name;
                        PriceListStorageRef.TimeStamp = storageRef.TimeStamp;
                        PriceListStorageRef.StorageIdentity = storageRef.StorageIdentity;
                        PriceListStorageRef.Name = storageRef.Name;
                    }
                    RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Name)));

                }

            }
        }
        private void SetOtherPriceListTreeNodesName(string name)
        {
            List<FBResourceTreeNode> priceListStorageReferences = null;
            if ((HeaderNode).FBResourceTreeNodesDictionary.TryGetValue(PriceListStorageRef.StorageIdentity, out priceListStorageReferences))
            {
                foreach (PriceListPresentation graphicMenuTreeNode in priceListStorageReferences)
                {
                    if (graphicMenuTreeNode != this)
                        graphicMenuTreeNode.Name = name;
                }
            }
        }
        public bool HasContextMenu
        {
            get => true;
        }

        public override ImageSource TreeImage
        {
            get
            {
                return new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/price-list16.png"));
            }
        }

        public List<ItemsPriceInfoPresentation> ItemsPriceInfoPresentations
        {
            get
            {
                return null;
            }
        }

        public override List<FBResourceTreeNode> Members => new List<FBResourceTreeNode>();


        public RelayCommand RenameCommand { get; protected set; }

        public RelayCommand DeleteCommand { get; protected set; }
        public RelayCommand EditCommand { get; protected set; }



        /// <exclude>Excluded</exclude>
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



                    menuItem = new MenuCommand();
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Edit16.png"));
                    menuItem.Header = MenuItemsEditor.Properties.Resources.EditObject;
                    menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = EditCommand;

                    _ContextMenuItems.Add(menuItem);


                    _ContextMenuItems.Add(null);

                    menuItem = new MenuCommand();
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/delete.png"));
                    menuItem.Header = Properties.Resources.DeleteMenuItemHeader;
                    menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = DeleteCommand;

                    _ContextMenuItems.Add(menuItem);



                }

                return _ContextMenuItems;

            }
        }


        public override List<MenuCommand> SelectedItemContextMenuItems
        {
            get
            {
                if (IsSelected)
                    return null;
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


        public override void RemoveChild(FBResourceTreeNode treeNode)
        {
            Parent.RemoveChild(treeNode);
        }

        public override void SelectionChange()
        {
        }

        internal void ClearItemsPriceInfo(IItemsCategory itemsCategory)
        {
            var itemsPriceInfos = (from itemsInfo in PriceList.ItemsPrices
                                         select new
                                         {
                                             @object = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfo.ItemsInfoObjectUri),
                                             ItemsPreparationInfo = itemsInfo
                                         }).ToList();

            var includedItemsPriceInfo = (from itemsInfoEntry in itemsPriceInfos
                                                where itemsInfoEntry.@object == itemsCategory
                                                select itemsInfoEntry.ItemsPreparationInfo).FirstOrDefault();

            if (includedItemsPriceInfo != null)
            {
                this.PriceList.RemoveItemsPriceInfos(includedItemsPriceInfo);

                ClearSubCategoryItemsPriceInfo(itemsCategory);

            }
        }

        private void ClearSubCategoryItemsPriceInfo(IItemsCategory itemsCategory)
        {
            foreach (var menuitem in itemsCategory.MenuItems)
            {
                if (!PriceList.HasOverriddenPrice(menuitem))
                    ClearItemsPriceInfo(menuitem);
            }


            foreach (var subCategory in itemsCategory.SubCategories)
            {
                if (!PriceList.HasOverriddenPrice(subCategory))
                    ClearSubCategoryItemsPriceInfo(subCategory);
            }
        }


      

        internal void ClearItemsPriceInfo(IMenuItem menuItem)
        {
            var itemsPriceInfos = (from itemsInfo in PriceList.ItemsPrices
                                   select new
                                   {
                                       @object = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfo.ItemsInfoObjectUri),
                                       ItemsPreparationInfo = itemsInfo
                                   }).ToList();

            var includedItemsPriceInfo = (from itemsInfoEntry in itemsPriceInfos
                                          where itemsInfoEntry.@object == menuItem
                                          select itemsInfoEntry.ItemsPreparationInfo).FirstOrDefault();

            if (includedItemsPriceInfo != null)
            {
                this.PriceList.RemoveItemsPriceInfos(includedItemsPriceInfo);

            }
        }

        internal void IncludeItems(IItemsCategory itemsCategory)
        {
            var itemsPreparationInfos = (from itemsInfo in PriceList.ItemsPrices
                                         select new
                                         {
                                             @object = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfo.ItemsInfoObjectUri),
                                             ItemsPriceInfo = itemsInfo
                                         }).ToList();

            var excludedItemsPreparationInfo = (from itemsInfoEntry in itemsPreparationInfos
                                                where itemsInfoEntry.@object == itemsCategory && itemsInfoEntry.ItemsPriceInfo.Excluded()
                                                select itemsInfoEntry.ItemsPriceInfo).FirstOrDefault();
            if (excludedItemsPreparationInfo != null)
            {
                PriceList.RemoveItemsPriceInfos(excludedItemsPreparationInfo);
                //RemoveItemsPreparationInfos(excludedItemsPreparationInfo);

            }

            if (!PriceList.HasOverriddenPrice(itemsCategory))
            {
                string uri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(itemsCategory).GetPersistentObjectUri(itemsCategory);

                var itemsPreparationInfo = this.PriceList.NewPriceInfo(uri, ItemsPriceInfoType.Include);

                //this.ItemsPreparationInfos = this.PriceList.ItemsPreparationInfos.ToList();
            }

            List<IItemsPriceInfo> uselessDescendantItemsPreparationInfos = GetUselessDescendantItemsPriceInfos(itemsCategory);
            if (uselessDescendantItemsPreparationInfos.Count > 0)
            {
                // the item preparation infos which refer to items or category which contained in included category and are useless must be removed
                PriceList.RemoveItemsPriceInfos(uselessDescendantItemsPreparationInfos);
                //ItemsPreparationInfos = PriceList.ItemsPreparationInfos.ToList();
            }


            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
        }




        private List<IItemsPriceInfo> GetUselessDescendantItemsPriceInfos(IItemsCategory itemsCategory)
        {

            List<IItemsPriceInfo> itemsPreparationInfos = new List<IItemsPriceInfo>();

            //_Members.Add(new ItemsPreparationInfoPresentation(this, itemsPreparationInfo));
            var itemsPreparationInfosEntry = (from itemsInfo in PriceList.ItemsPrices
                                              select new
                                              {
                                                  @object = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfo.ItemsInfoObjectUri),
                                                  ItemsPriceInfo = itemsInfo
                                              }).ToList();

            return itemsPreparationInfos;
        }


        ItemsPriceInfoPresentation PreparationStationItems;

        public void IncludeItem(MenuModel.IMenuItem menuItem)
        {

            var itemsPreparationInfos = (from itemsInfo in PriceList.ItemsPrices
                                         select new
                                         {
                                             @object = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfo.ItemsInfoObjectUri),
                                             ItemsPriceInfo = itemsInfo
                                         }).ToList();

            var excludedItemsPriceInfo = (from itemsInfoEntry in itemsPreparationInfos
                                          where itemsInfoEntry.@object == menuItem && itemsInfoEntry.ItemsPriceInfo.Excluded()
                                          select itemsInfoEntry.ItemsPriceInfo).FirstOrDefault();

            if (excludedItemsPriceInfo != null)
                PriceList.RemoveItemsPriceInfos(excludedItemsPriceInfo);



            if (!PriceList.HasOverriddenPrice(menuItem))
            {
                string uri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(menuItem).GetPersistentObjectUri(menuItem);
                var dd = this.PriceList.ItemsPrices;
                var itemsPreparationInfo = this.PriceList.NewPriceInfo(uri, ItemsPriceInfoType.Include);

                //this.AddItemsPreparationInfos(itemsPreparationInfo);
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));

                foreach (var itemsPreparationInfoPresentation in ItemsToChoose.OfType<ItemsPriceInfoPresentation>())
                    itemsPreparationInfoPresentation.Refresh();
            }

            if (PreparationStationItems != null)
                PreparationStationItems.Refresh();
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));



        }



        public void ExcludeItems(MenuModel.IItemsCategory itemsCategory)
        {


            if (PriceList.HasOverriddenPrice(itemsCategory))
            {


                var itemsPreparationInfos = (from itemsInfo in PriceList.ItemsPrices
                                             select new
                                             {
                                                 @object = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfo.ItemsInfoObjectUri),
                                                 ItemsPriceInfo = itemsInfo
                                             }).ToList();

                var includedItemsPreparationInfo = (from itemsInfoEntry in itemsPreparationInfos
                                                    where itemsInfoEntry.@object == itemsCategory && itemsInfoEntry.ItemsPriceInfo.Included()
                                                    select itemsInfoEntry.ItemsPriceInfo).FirstOrDefault();

                if (includedItemsPreparationInfo != null)
                {
                    this.PriceList.RemoveItemsPriceInfos(includedItemsPreparationInfo);
                    //RemoveItemsPreparationInfos(includedItemsPreparationInfo);
                }
                else
                {

                    string uri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(itemsCategory).GetPersistentObjectUri(itemsCategory);
                    var itemsPreparationInfo = this.PriceList.NewPriceInfo(uri, ItemsPriceInfoType.Exclude);

                    //AddItemsPreparationInfos(itemsPreparationInfo);
                    //var sdd = itemsPreparationInfo.Exclude;
                }

                //foreach (var itemsPreparationInfoPresentation in ItemsToChoose.OfType<ItemsPreparationInfoPresentation>())
                //    itemsPreparationInfoPresentation.Refresh();

            }
            else
            {
                List<IItemsPriceInfo> uselessDescendantItemsPriceInfos = GetUselessDescendantItemsPriceInfos(itemsCategory);
                if (uselessDescendantItemsPriceInfos.Count > 0)
                {
                    // the item preparation infos which refer to items or category which contained in included category and are useless must be removed
                    PriceList.RemoveItemsPriceInfos(uselessDescendantItemsPriceInfos);
                    //ItemsPreparationInfos = PriceList.ItemsPreparationInfos.ToList();
                }
                //foreach (var itemsPreparationInfoPresentation in ItemsToChoose.OfType<ItemsPreparationInfoPresentation>())
                //    itemsPreparationInfoPresentation.Refresh();


            }

            //if (PreparationStationItems != null)
            //    PreparationStationItems.Refresh();
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
            //foreach (var member in _Members.OfType<ItemsPreparationInfoPresentation>())
            //    member.Refresh();
        }


        public void ExcludeItem(MenuModel.IMenuItem menuItem)
        {

            if (PriceList.HasOverriddenPrice(menuItem))
            {


                var itemsPreparationInfos = (from itemsInfo in PriceList.ItemsPrices
                                             select new
                                             {
                                                 @object = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfo.ItemsInfoObjectUri),
                                                 ItemsPriceInfo = itemsInfo
                                             }).ToList();

                var includedItemsPreparationInfo = (from itemsInfoEntry in itemsPreparationInfos
                                                    where itemsInfoEntry.@object == menuItem
                                                    select itemsInfoEntry.ItemsPriceInfo).FirstOrDefault();

                if (includedItemsPreparationInfo != null)
                {
                    this.PriceList.RemoveItemsPriceInfos(includedItemsPreparationInfo);
                    // RemoveItemsPreparationInfos(includedItemsPreparationInfo);
                }
                if (PriceList.HasOverriddenPrice(menuItem))
                {

                    string uri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(menuItem).GetPersistentObjectUri(menuItem);
                    var itemsPreparationInfo = this.PriceList.NewPriceInfo(uri, ItemsPriceInfoType.Exclude);
                    //this.AddItemsPreparationInfos(itemsPreparationInfo);
                }

                if (PreparationStationItems != null)
                    PreparationStationItems.Refresh();
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
            }
        }



        public IItemsPriceInfo GetItemsPriceInfo(MenuModel.IItemsCategory itemsCategory)
        {
            var itemsPriceInfos = (from itemsInfo in PriceList.ItemsPrices
                                   select new
                                   {
                                       @object = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfo.ItemsInfoObjectUri),
                                       ItemsPriceInfo = itemsInfo
                                   }).ToList();

            var itemsPriceInfo = (from itemsInfoEntry in itemsPriceInfos
                                  where itemsInfoEntry.@object == itemsCategory && (itemsInfoEntry.ItemsPriceInfo.Included())
                                  select itemsInfoEntry.ItemsPriceInfo).FirstOrDefault();

            if (itemsPriceInfo != null)
                return itemsPriceInfo;
            else
                return null;
        }



        public IItemsPriceInfo GetOrCreateItemsPriceInfo(MenuModel.IItemsCategory itemsCategory)
        {
            var itemsPriceInfos = (from itemsInfo in PriceList.ItemsPrices
                                   select new
                                   {
                                       @object = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfo.ItemsInfoObjectUri),
                                       ItemsPriceInfo = itemsInfo
                                   }).ToList();

            var itemsPriceInfo = (from itemsInfoEntry in itemsPriceInfos
                                  where itemsInfoEntry.@object == itemsCategory && (itemsInfoEntry.ItemsPriceInfo.Included())
                                  select itemsInfoEntry.ItemsPriceInfo).FirstOrDefault();

            if (itemsPriceInfo != null)
            {

                return itemsPriceInfo;
            }
            else
            {
                string uri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(itemsCategory).GetPersistentObjectUri(itemsCategory);
                itemsPriceInfo = PriceList.NewPriceInfo(uri, ItemsPriceInfoType.Include);
                return itemsPriceInfo;
            }


            //foreach (var itemsPreparationInfoPresentation in ItemsToChoose.OfType<ItemsPreparationInfoPresentation>())
            //    itemsPreparationInfoPresentation.Refresh();


            //if (PreparationStationItems != null)
            //    PreparationStationItems.Refresh();
            //RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));

        }


        public IItemsPriceInfo GetItemsPriceInfo(MenuModel.IMenuItem menuItem)
        {

            var itemsPreparationInfos = (from itemsInfo in PriceList.ItemsPrices
                                         select new
                                         {
                                             @object = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfo.ItemsInfoObjectUri),
                                             ItemsPriceInfo = itemsInfo
                                         }).ToList();

            var itemsPreparationInfo = (from itemsInfoEntry in itemsPreparationInfos
                                        where itemsInfoEntry.@object == menuItem
                                        select itemsInfoEntry.ItemsPriceInfo).FirstOrDefault();

            if (itemsPreparationInfo != null)
            {
                return itemsPreparationInfo;
            }
            else
            {
                return null;
            }
        }


        public IItemsPriceInfo GetOrCreateItemsPriceInfo(MenuModel.IMenuItem menuItem)
        {

            var itemsPreparationInfos = (from itemsInfo in PriceList.ItemsPrices
                                         select new
                                         {
                                             @object = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfo.ItemsInfoObjectUri),
                                             ItemsPriceInfo = itemsInfo
                                         }).ToList();

            var itemsPreparationInfo = (from itemsInfoEntry in itemsPreparationInfos
                                        where itemsInfoEntry.@object == menuItem
                                        select itemsInfoEntry.ItemsPriceInfo).FirstOrDefault();

            if (itemsPreparationInfo != null)
            {
                return itemsPreparationInfo;
            }
            else
            {
                string uri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(menuItem).GetPersistentObjectUri(menuItem);
                itemsPreparationInfo = this.PriceList.NewPriceInfo(uri, ItemsPriceInfoType.Include);
                return itemsPreparationInfo;
            }
        }

        internal double? GetPercentageDiscount(IItemsCategory itemsCategory)
        {
            var itemsPriceInfos = PriceList.GetItemsPriceInfo(itemsCategory);

            foreach (var itemsPriceInfo in itemsPriceInfos)
            {

                if (itemsPriceInfo.PercentageDiscount != null)
                    return itemsPriceInfo.PercentageDiscount.Value;
            }
            return PriceList.PriceListMainItemsPriceInfo.PercentageDiscount;
        }
        internal double? GetPercentageDiscount(IMenuItem menuItem)
        {
            var itemsPriceInfos = PriceList.GetItemsPriceInfo(menuItem);

            foreach (var itemsPriceInfo in itemsPriceInfos)
            {

                if (itemsPriceInfo.PercentageDiscount != null)
                    return itemsPriceInfo.PercentageDiscount.Value;
            }

            return PriceList.PriceListMainItemsPriceInfo.PercentageDiscount;

        }

        internal void SetPercentageDiscount(IItemsCategory itemsCategory, double? percentageDiscount)
        {
            if (percentageDiscount==null)
            {
                var itemsPriceInfo = GetItemsPriceInfo(itemsCategory);
                if (itemsPriceInfo!=null)
                    itemsPriceInfo.PercentageDiscount=null;
                return;
            }
            GetOrCreateItemsPriceInfo(itemsCategory).PercentageDiscount = percentageDiscount;
        }

        internal void SetPercentageDiscount(IMenuItem menuItem, double? percentageDiscount)
        {
            if (percentageDiscount==null)
            {
                var itemsPriceInfo = GetItemsPriceInfo(menuItem);
                if (itemsPriceInfo!=null)
                    itemsPriceInfo.PercentageDiscount=null;
                return;
            }
            GetOrCreateItemsPriceInfo(menuItem).PercentageDiscount = percentageDiscount;
        }


        internal double? GetPricerounding(IItemsCategory itemsCategory)
        {
            var itemsPriceInfos = PriceList.GetItemsPriceInfo(itemsCategory);

            foreach (var itemsPriceInfo in itemsPriceInfos)
            {

                if (itemsPriceInfo.PercentageDiscount != null)
                    return itemsPriceInfo.Pricerounding;
            }
            if (PriceList.PriceListMainItemsPriceInfo.PercentageDiscount != null && PriceList.PriceListMainItemsPriceInfo.Pricerounding != null)
                return PriceList.PriceListMainItemsPriceInfo.Pricerounding;
            return 0;
        }
        internal double? GetPricerounding(IMenuItem menuItem)
        {
            var itemsPriceInfos = PriceList.GetItemsPriceInfo(menuItem);

            foreach (var itemsPriceInfo in itemsPriceInfos)
            {

                if (itemsPriceInfo.Pricerounding != null)
                    return itemsPriceInfo.Pricerounding.Value;
            }
            if (PriceList.PriceListMainItemsPriceInfo.PercentageDiscount != null && PriceList.PriceListMainItemsPriceInfo.Pricerounding != null)
                return PriceList.PriceListMainItemsPriceInfo.Pricerounding;

            return 0;

        }

        internal void SetPricerounding(IItemsCategory itemsCategory, double Pricerounding)
        {
            GetOrCreateItemsPriceInfo(itemsCategory).Pricerounding = Pricerounding;
        }

        internal void SetPricerounding(IMenuItem menuItem, double Pricerounding)
        {
            GetOrCreateItemsPriceInfo(menuItem).Pricerounding = Pricerounding;
        }



        internal double? GetAmountDiscount(IItemsCategory itemsCategory)
        {
            var itemsPriceInfos = PriceList.GetItemsPriceInfo(itemsCategory);

            foreach (var itemsPriceInfo in itemsPriceInfos)
            {

                if (itemsPriceInfo.AmountDiscount != null)
                    return itemsPriceInfo.AmountDiscount.Value;
            }
            return PriceList.PriceListMainItemsPriceInfo.AmountDiscount;
        }
        internal double? GetAmountDiscount(IMenuItem menuItem)
        {
            var itemsPriceInfos = PriceList.GetItemsPriceInfo(menuItem);

            foreach (var itemsPriceInfo in itemsPriceInfos)
            {

                if (itemsPriceInfo.AmountDiscount != null)
                    return itemsPriceInfo.AmountDiscount.Value;
            }
            return PriceList.PriceListMainItemsPriceInfo.AmountDiscount;

        }
        internal void SetAmountDiscount(double amountDiscount)
        {
            if (amountDiscount == 0)
                PriceList.PriceListMainItemsPriceInfo.AmountDiscount = null;
            else
                PriceList.PriceListMainItemsPriceInfo.AmountDiscount = amountDiscount;

        }

        internal void SetAmountDiscount(IItemsCategory itemsCategory, double AmountDiscount)
        {
            if (AmountDiscount==0)
            {
                var itemsPriceInfo = GetItemsPriceInfo(itemsCategory);
                if (itemsPriceInfo!=null)
                    itemsPriceInfo.AmountDiscount=null;
            }

            GetOrCreateItemsPriceInfo(itemsCategory).AmountDiscount = AmountDiscount;
        }

        internal void SetAmountDiscount(IMenuItem menuItem, double AmountDiscount)
        {
            if (AmountDiscount==0)
            {
                var itemsPriceInfo = GetItemsPriceInfo(menuItem);
                if (itemsPriceInfo!=null)
                    itemsPriceInfo.AmountDiscount=null;
                return;
            }


            GetOrCreateItemsPriceInfo(menuItem).AmountDiscount = AmountDiscount;
        }




        internal decimal? GetOverridenPrice(IItemsCategory itemsCategory)
        {
            var itemsPriceInfos = PriceList.GetItemsPriceInfo(itemsCategory);

            foreach (var itemsPriceInfo in itemsPriceInfos)
            {

                if (itemsPriceInfo.OverridenPrice != null)
                    return itemsPriceInfo.OverridenPrice.Value;
            }
            return null; 
        }
        internal decimal? GetOverridenPrice(IMenuItem menuItem)
        {
            var itemsPriceInfos = PriceList.GetItemsPriceInfo(menuItem);

            foreach (var itemsPriceInfo in itemsPriceInfos)
            {

                if (itemsPriceInfo.OverridenPrice != null)
                    return itemsPriceInfo.OverridenPrice.Value;
            }
            return null;

        }

        internal void SetOverridenPrice(IItemsCategory itemsCategory, decimal? OverridenPrice)
        {
            if(OverridenPrice==null)
            {
                var itemsPriceInfo = GetItemsPriceInfo(itemsCategory);
                if (itemsPriceInfo!=null)
                    itemsPriceInfo.OverridenPrice=null;
                return;
            }
                
            GetOrCreateItemsPriceInfo(itemsCategory).OverridenPrice = OverridenPrice;
        }

        internal void SetOverridenPrice(IMenuItem menuItem, decimal? OverridenPrice)
        {
            if (OverridenPrice==null)
            {
                var itemsPriceInfo = GetItemsPriceInfo(menuItem);
                if (itemsPriceInfo!=null)
                    itemsPriceInfo.OverridenPrice=null;
                return;
            }
            GetOrCreateItemsPriceInfo(menuItem).OverridenPrice = OverridenPrice;
        }

        internal void SetPercentageDiscount(double percentageDiscount)
        {
            if (percentageDiscount == 0)
                PriceList.PriceListMainItemsPriceInfo.PercentageDiscount = null;
            else
                PriceList.PriceListMainItemsPriceInfo.PercentageDiscount = percentageDiscount;
        }

        internal void SetPricerounding(double pricerounding)
        {
            if (pricerounding == 0)
                PriceList.PriceListMainItemsPriceInfo.Pricerounding = null;
            else
                PriceList.PriceListMainItemsPriceInfo.Pricerounding = pricerounding;
        }

        internal void ClearItemsPriceInfo()
        {
            PriceList.PriceListMainItemsPriceInfo.PercentageDiscount = null;
            PriceList.PriceListMainItemsPriceInfo.Pricerounding = null;
        }
    }
}
static class ItemsCategoryExtention
{
    /// <MetaDataID>{e78b2a9d-d9b4-4f90-8a52-e5bb03227c45}</MetaDataID>
    public static List<MenuItem> GetAllMenuItems(this IItemsCategory itemsCategory)
    {
        var menuItems = itemsCategory.ClassifiedItems.OfType<MenuItem>().ToList();

        menuItems.AddRange((from subCutegory in itemsCategory.ClassifiedItems.OfType<IItemsCategory>()
                            from subCutegoriesItem in subCutegory.GetAllMenuItems()
                            select subCutegoriesItem));
        return menuItems;

    }

    /// <MetaDataID>{97965af5-b8c5-433e-bdf7-6d6c6206c72a}</MetaDataID>
    public static bool IsAncestor(this MenuItem menuItem, ItemsCategory itemsCategory)
    {
        var category = menuItem.Category;
        while (category != null)
        {
            if (category == itemsCategory)
                return true;
            category = category.Parent;
        }
        return false;

    }

    /// <MetaDataID>{792215bf-6d4b-4804-9378-197f51d0aea9}</MetaDataID>
    public static bool IsAncestor(this ItemsCategory thisItemsCategory, ItemsCategory itemsCategory)
    {
        var category = thisItemsCategory.Parent;
        if (category == itemsCategory)
            return true;
        while (category != null)
        {
            if (category == itemsCategory)
                return true;
            category = category.Parent;
        }
        return false;

    }


}
