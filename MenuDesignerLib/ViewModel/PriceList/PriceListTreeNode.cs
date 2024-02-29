using FlavourBusinessFacade;
using FlavourBusinessFacade.PriceList;
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

namespace MenuDesigner.ViewModel.PriceList
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
                return new BitmapImage(new Uri(@"pack://application:,,,/MenuDesignerLib;Component/Resources/Images/Metro/price-list16.png"));
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
                    menuItem.Header = Properties.Resources.RemoveCallerIDServer;
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
            throw new NotImplementedException();
        }

        internal void ClearItemsPriceInfo(IMenuItem menuItem)
        {
            throw new NotImplementedException();
        }

        internal void IncludeItems(IItemsCategory itemsCategory)
        {
            var itemsPreparationInfos = (from itemsInfo in PriceList.ItemsPrices
                                         select new
                                         {
                                             @object = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfo.ItemsInfoObjectUri),
                                             ItemsPreparationInfo = itemsInfo
                                         }).ToList();

            var excludedItemsPreparationInfo = (from itemsInfoEntry in itemsPreparationInfos
                                                where itemsInfoEntry.@object == itemsCategory && itemsInfoEntry.ItemsPreparationInfo.Excluded()
                                                select itemsInfoEntry.ItemsPreparationInfo).FirstOrDefault();
            if (excludedItemsPreparationInfo != null)
            {
                PriceList.RemoveItemsPriceInfos(excludedItemsPreparationInfo);
                //RemoveItemsPreparationInfos(excludedItemsPreparationInfo);

            }

            if (!PriceList.HasOverriddenPrice(itemsCategory))
            {
                string uri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(itemsCategory).GetPersistentObjectUri(itemsCategory);

                var itemsPreparationInfo = this.PriceList.NewPriceInfo(uri, ItemsPriceInfoType.Include);

                //this.ItemsPreparationInfos = this.PreparationStation.ItemsPreparationInfos.ToList();
            }

            List<IItemsPriceInfo> uselessDescendantItemsPreparationInfos = GetUselessDescendantItemsPriceInfos(itemsCategory);
            if (uselessDescendantItemsPreparationInfos.Count > 0)
            {
                // the item preparation infos which refer to items or category which contained in included category and are useless must be removed
                PriceList.RemoveItemsPriceInfos(uselessDescendantItemsPreparationInfos);
                //ItemsPreparationInfos = PreparationStation.ItemsPreparationInfos.ToList();
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
                                                  ItemsPreparationInfo = itemsInfo
                                              }).ToList();

            return itemsPreparationInfos;
        }

        internal void ExcludeItems(IItemsCategory itemsCategory)
        {
            throw new NotImplementedException();
        }

        internal void IncludeItem(IMenuItem menuItem)
        {
            throw new NotImplementedException();
        }

        internal void ExcludeItem(IMenuItem menuItem)
        {
            throw new NotImplementedException();
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
}