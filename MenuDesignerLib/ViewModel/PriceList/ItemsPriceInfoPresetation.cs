using FlavourBusinessFacade.ServicesContextResources;
using FLBManager.ViewModel;

using MenuModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using System.Windows.Media.Imaging;
using System.Windows.Media;
using UIBaseEx;
using WPFUIElementObjectBind;
using FlavourBusinessFacade.PriceList;
using System.Windows;

namespace MenuDesigner.ViewModel.PriceList
{

    public class ItemsPriceInfoPresentation : FBResourceTreeNode, INotifyPropertyChanged
    {
        public override void RemoveChild(FBResourceTreeNode treeNode)
        {
            throw new NotImplementedException();
        }
        /// <MetaDataID>{017f504b-513b-4fc4-af42-c0c2ae4641b9}</MetaDataID>
        public readonly IItemsPreparationInfo ItemsPreparationInfo;
        /// <MetaDataID>{b369fbe0-d83b-467f-ba71-050afe49452e}</MetaDataID>
        public readonly PriceListPresentation PriceListPresentation;

        /// <summary>
        /// Preparation infos concern menu item
        /// </summary>
        /// <MetaDataID>{664db8b4-7bf9-46f2-b0e8-a18af5e23430}</MetaDataID>
        // <MetaDataID>{4e50a2ba-e4a7-498e-a316-aa888e4692bd}</MetaDataID>
        IMenuItem MenuItem;

        /// <summary>
        ///  Preparation infos concern menu items of category
        /// </summary>
        /// <MetaDataID>{b341b6ed-cd19-4c47-b3a2-9f175b91d05b}</MetaDataID>
        // <MetaDataID>{a777298f-cf21-4ccb-848d-38596df908f3}</MetaDataID>
        internal IItemsCategory ItemsCategory;




        /// <summary>
        /// When EditMode is true you can include item in preparation station, exclude or edit item preparation info.
        /// </summary>
        /// <MetaDataID>{27d5229c-bc53-42bd-9d47-6ceea9487d1f}</MetaDataID>
        readonly bool EditMode;


        /// <MetaDataID>{636b39c3-93bd-4987-988e-ebb01317fc4d}</MetaDataID>
        public readonly bool IsRootCategory;

        /// <summary>
        /// Defines constructor for root items category
        /// </summary>
        /// <param name="preparationStationPresentation">
        /// Defines the preparation station presentation of itemsPreparationInfo
        /// </param>
        /// <param name="menu">
        /// Defines the items menu
        /// </param>
        /// <param name="editMode"></param>
        /// <MetaDataID>{36657d09-72be-40de-bc82-5213ba950d4b}</MetaDataID>
        // <MetaDataID>{6d5912d3-dbd6-431c-b216-d6a4732da003}</MetaDataID>
        public ItemsPriceInfoPresentation(PriceListPresentation priceListPresentation, IMenu menu, bool editMode) : base(priceListPresentation)
        {
            IsRootCategory = true;
            EditMode = editMode;
            ItemsPreparationInfo = new FlavourBusinessManager.ServicesContextResources.ItemsPreparationInfo(menu.RootCategory);
            PriceListPresentation = priceListPresentation;
            ItemsCategory = menu.RootCategory;


            DeleteCommand = new RelayCommand((object sender) =>
            {
                Delete();
            });
            RemoveItemsPreparationInfoCommand = new RelayCommand((object sender) =>
            {
                RemoveItemsPreparationInfo();
            });




            //System.Windows.Input.RoutedCommand((object sender) =>
            //{
            //    System.Diagnostics.Debug.WriteLine("");
            //});


        }


        /// <MetaDataID>{81bc990f-0fb8-46d4-9096-885f628cdae6}</MetaDataID>
        public ItemsPriceInfoPresentation(ItemsPriceInfoPresentation parent, PriceListPresentation priceListPresentation, MenuModel.IItemsCategory itemsCategory, bool editMode) : base(parent)
        {
            EditMode = editMode;
            PriceListPresentation = priceListPresentation;

            ItemsCategory = itemsCategory;

            DeleteCommand = new RelayCommand((object sender) =>
            {
                Delete();
            });

            RemoveItemsPreparationInfoCommand = new RelayCommand((object sender) =>
            {
                RemoveItemsPreparationInfo();
            });


            CheckBoxVisibility = System.Windows.Visibility.Visible;
        }

        private void RemoveItemsPreparationInfo()
        {
            if (EditMode)
            {
                if (ItemsCategory != null)
                    PriceListPresentation.ClearItemsPriceInfo(ItemsCategory);

                if (MenuItem != null)
                    PriceListPresentation.ClearItemsPriceInfo(MenuItem);

                UpdatePresentationItems();


                foreach (var member in Members.OfType<ItemsPriceInfoPresentation>())
                {
                    member.Refresh();
                }

            }
        }

        /// <MetaDataID>{f76f2725-c4bc-48db-b680-cf20f594f218}</MetaDataID>
        public ItemsPriceInfoPresentation(ItemsPriceInfoPresentation parent, PriceListPresentation priceListPresentation, IMenuItem menuItem, bool editMode) : base(parent)
        {
            EditMode = editMode;
            PriceListPresentation = priceListPresentation;
            MenuItem = menuItem;

            DeleteCommand = new RelayCommand((object sender) =>
            {
                Delete();
            });
            RemoveItemsPreparationInfoCommand = new RelayCommand((object sender) =>
            {
                RemoveItemsPreparationInfo();
            });




        }




        /// <MetaDataID>{2a48dd97-5dfd-4f46-aabb-a776aa182436}</MetaDataID>
        ViewModelWrappers<MenuModel.IItemsCategory, ItemsPriceInfoPresentation> SubCategories = new ViewModelWrappers<MenuModel.IItemsCategory, ItemsPriceInfoPresentation>();
        /// <MetaDataID>{760c0787-688d-4974-8c42-a58092d27c33}</MetaDataID>
        ViewModelWrappers<MenuModel.IMenuItem, ItemsPriceInfoPresentation> MenuItems = new ViewModelWrappers<MenuModel.IMenuItem, ItemsPriceInfoPresentation>();



        /// <summary>
        /// Defines when ItemsPreparationInfoPresentation node has items which can prepared from preparation station 
        /// In view mode displayed, only items category node and items which prepared from preparation station
        /// </summary>
        /// <MetaDataID>{2f994810-395c-48aa-8eb6-d44938a06d5c}</MetaDataID>
        // <MetaDataID>{5166e975-649f-47b1-b98e-5f1abf42f3ad}</MetaDataID>
        public bool HasItemsWhichCanPrepared
        {
            get
            {

                if (ItemsCategory != null && PriceListPresentation.PriceList.HasOverriddenPrice(this.ItemsCategory))
                    return true;
                else if (this.MenuItem != null && PriceListPresentation.PriceList.HasOverriddenPrice(this.MenuItem))
                    return true;
                else
                {
                    if (ItemsCategory != null)
                    {
                        var itemsPreparationInfosPresentations = (from subCategory in ItemsCategory.ClassifiedItems.OfType<MenuModel.IItemsCategory>()
                                                                  select SubCategories.GetViewModelFor(subCategory, this, PriceListPresentation, subCategory, EditMode)).Union(
                             (from menuItem in ItemsCategory.ClassifiedItems.OfType<MenuModel.MenuItem>()
                              select MenuItems.GetViewModelFor(menuItem, this, PriceListPresentation, menuItem, EditMode)));

                        var members = (from itemsPreparationInfoPresentation in itemsPreparationInfosPresentations
                                       select itemsPreparationInfoPresentation).OfType<FBResourceTreeNode>().ToList();

                        foreach (var member in members.OfType<ItemsPriceInfoPresentation>())
                        {
                            if (member.HasItemsWhichCanPrepared)
                                return true;
                        }
                    }
                    return false;
                }
            }
        }









        //public System.Windows.FontWeight CookingTimeFontWeight
        //{
        //    get
        //    {
        //        if (this.ItemsCategory != null && this.PriceListPresentation.CookingTimeSpanInMinIsDefinedFor(this.ItemsCategory))
        //            return System.Windows.FontWeights.SemiBold;
        //        if (this.MenuItem != null && this.PriceListPresentation.CookingTimeSpanInMinIsDefinedFor(this.MenuItem))
        //            return System.Windows.FontWeights.SemiBold;
        //        return System.Windows.FontWeights.Normal;
        //    }
        //}





        public override bool Edit
        {
            get => base.Edit;

            set
            {

                base.Edit = value;
            }
        }









        /// <MetaDataID>{a558dc92-8072-47f3-b669-0d4c1b763396}</MetaDataID>
        public bool PreparationTimeIsVisible
        {
            get
            {
                return DefinesNewPrice;
            }
        }

        /// <MetaDataID>{c0a45032-38e7-4142-ba1e-47015ff8c0dd}</MetaDataID>
        public bool DefinesNewPrice
        {
            get
            {
                if (this.ItemsCategory != null && this.PriceListPresentation.PriceList.HasOverriddenPrice(this.ItemsCategory))
                    return true;
                else if (this.MenuItem != null && this.PriceListPresentation.PriceList.HasOverriddenPrice(this.MenuItem))
                    return true;
                else
                    return false;
            }
            set
            {
                if (value && ItemsCategory != null)
                    PriceListPresentation.IncludeItems(ItemsCategory);

                if (!value && ItemsCategory != null)
                    PriceListPresentation.ExcludeItems(ItemsCategory);


                if (value && MenuItem != null)
                    PriceListPresentation.IncludeItem(MenuItem);

                if (!value && MenuItem != null)
                    this.PriceListPresentation.ExcludeItem(MenuItem);


                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(PreparationTimeIsVisible)));

                UpdatePresentationItems();

                //RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(PreparationTimeSpanInMin)));
                //RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(CookingTimeSpanInMin)));
                //RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(TotalPreparationTimeSpanInMin)));
                //RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(IsCooked)));
                //RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(HasPreparationTimeSpanValue)));
                //RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Tags)));
                //RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(HasTags)));
                //RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(HasAppearanceOrderValue)));
                //RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(AppearanceOrder)));
                //RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(AppearanceOrderText)));




            }
        }

        public Visibility PriceVisibility
        {
            get
            {
                if (MenuItem != null)
                    return Visibility.Visible;
                else
                    return Visibility.Hidden;


            }
            set { }
        }
        public string PriceText
        {
            get
            {

                return MenuItem?.MenuItemPrice?.Price.ToString("C");
            }
            set { }
        }

        /// <summary>
        /// Some times a change in items preparation info affect nodes in preparation station infos hierarchy
        /// this method update the  hierarchy if needed
        /// </summary>
        /// <MetaDataID>{eabde86b-dbcc-439f-9118-3958c36c0e92}</MetaDataID>
        private void UpdatePresentationItems()
        {

            if (ItemsCategory != null)
            {
                Root.Refresh(ItemsCategory as ItemsCategory);
                Root.PriceListPresentation.Refresh(ItemsCategory as ItemsCategory);
            }


            if (MenuItem != null)
            {
                Root.Refresh(MenuItem as MenuItem);
                Root.PriceListPresentation.Refresh(MenuItem as MenuItem);
            }


            //Refresh();
        }
        /// <MetaDataID>{7e6b7299-dd58-402d-9094-9c51b82cb177}</MetaDataID>
        ItemsPriceInfoPresentation Root
        {
            get
            {
                ItemsPriceInfoPresentation mainPreparationStationPresentation = null;
                FBResourceTreeNode parent = this;
                if (parent is ItemsPriceInfoPresentation)
                    mainPreparationStationPresentation = parent as ItemsPriceInfoPresentation;
                while (parent.Parent != null)
                {
                    parent = parent.Parent;
                    if (parent is ItemsPriceInfoPresentation)
                        mainPreparationStationPresentation = parent as ItemsPriceInfoPresentation;
                }

                return mainPreparationStationPresentation;
            }
        }
        /// <MetaDataID>{fad3761a-9049-4cf1-8c5e-ffe25141ee69}</MetaDataID>
        public System.Windows.Visibility CheckBoxVisibility
        {
            get; set;
        }



        /// <MetaDataID>{469e6645-b25e-49b6-8eaa-e3ce412bbf93}</MetaDataID>
        private void Delete()
        {
            if (ItemsCategory != null)
                PriceListPresentation.ExcludeItems(ItemsCategory);
            if (this.MenuItem != null)
                PriceListPresentation.ExcludeItem(MenuItem);
        }


        /// <MetaDataID>{99fd2858-5fca-4b23-8d02-7eca8f7106c5}</MetaDataID>
        public override string Name
        {
            get
            {
                if (ItemsCategory != null)
                    return ItemsCategory.Name;
                else if (this.MenuItem != null)
                    return this.MenuItem.Name;
                else
                    return "";
            }
            set
            {
            }
        }








        /// <MetaDataID>{5b29545c-fe12-4953-ae37-58f532044a8c}</MetaDataID>
        public override ImageSource TreeImage
        {
            get
            {
                if (ItemsCategory != null)
                    return new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/category16.png"));
                else if (this.MenuItem != null)
                    return new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/MenuItem16.png"));
                else
                    return null;
            }
        }



        /// <exclude>Excluded</exclude>
        List<FBResourceTreeNode> _Members;
        /// <MetaDataID>{d2401e62-1e30-4557-9b75-142a3f9c4690}</MetaDataID>
        public override List<FBResourceTreeNode> Members
        {
            get
            {

                if (_Members != null)
                    return _Members;



                List<FBResourceTreeNode> members = new List<FBResourceTreeNode>();

                if (ItemsCategory != null)
                {




                    if (EditMode)
                    {
                        var itemsPriceInfosPresentations = (from subCategory in ItemsCategory.ClassifiedItems.OfType<MenuModel.IItemsCategory>()
                                                            select SubCategories.GetViewModelFor(subCategory, this, PriceListPresentation, subCategory, EditMode)).Union(
                           (from menuItem in ItemsCategory.ClassifiedItems.OfType<MenuModel.MenuItem>()
                            select MenuItems.GetViewModelFor(menuItem, this, PriceListPresentation, menuItem, EditMode)));

                        members.AddRange((from itemsPriceInfoPresentation in itemsPriceInfosPresentations
                                          select itemsPriceInfoPresentation).OfType<FBResourceTreeNode>().ToList());
                        _Members = members;
                        return members;
                    }
                    else
                    {
                        var itemsPriceInfosPresentations = (from subCategory in ItemsCategory.ClassifiedItems.OfType<MenuModel.IItemsCategory>()
                                                            select SubCategories.GetViewModelFor(subCategory, this, PriceListPresentation, subCategory, EditMode)).Union(
                           (from menuItem in ItemsCategory.ClassifiedItems.OfType<MenuModel.MenuItem>()
                            select MenuItems.GetViewModelFor(menuItem, this, PriceListPresentation, menuItem, EditMode)));

                        members.AddRange((from itemsPreparationInfoPresentation in itemsPriceInfosPresentations
                                          where itemsPreparationInfoPresentation.HasItemsWhichCanPrepared
                                          select itemsPreparationInfoPresentation).OfType<FBResourceTreeNode>().ToList());
                        _Members = members;
                        return members;

                    }
                }
                else
                {
                    if(MenuItem!=null)
                    {
                        _Members = MenuItem.Prices.OfType<MenuItemPrice>().Where(x => x.ItemSelector != null).Select(x => new ItemSelectorPriceInfo(this, MenuItem, x)).OfType<FBResourceTreeNode>().ToList();
                        return _Members;
                    }

                    return members;
                }
            }
        }


        /// <MetaDataID>{ff9d9100-88c5-453d-ae84-6583239e8501}</MetaDataID>
        public RelayCommand DeleteCommand { get; protected set; }
        public RelayCommand RemoveItemsPreparationInfoCommand { get; protected set; }




        /// <MetaDataID>{261115fe-509b-4c99-bb24-709235bb932f}</MetaDataID>



        /// <exclude>Excluded</exclude>
        List<MenuCommand> _ContextMenuItems;

        /// <MetaDataID>{83dcd867-12dc-4c2f-97d7-2a9d26a10471}</MetaDataID>
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





                    if (EditMode)
                    {


                        menuItem = new MenuCommand();
                        imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/delete.png"));
                        menuItem.Header = Properties.Resources.ClearItemsPriceInfoMenuHeader;
                        menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                        menuItem.Command = RemoveItemsPreparationInfoCommand;
                        _ContextMenuItems.Add(menuItem);

                    }

                    if (this.Parent is PriceListPresentation)
                    {
                        _ContextMenuItems.Add(null);

                        menuItem = new MenuCommand();
                        imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/delete.png"));
                        menuItem.Header = Properties.Resources.RemoveCallerIDServer;
                        menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                        menuItem.Command = DeleteCommand;

                        _ContextMenuItems.Add(menuItem);
                    }



                }

                return _ContextMenuItems;
            }
        }
        /// <MetaDataID>{64de1643-b464-40c2-b4cf-ab00dea48227}</MetaDataID>
        public override bool HasContextMenu
        {
            get
            {
                return true;
            }
        }

        /// <MetaDataID>{1229ec0a-0597-483d-93c5-828187c7cd0e}</MetaDataID>
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

        /// <MetaDataID>{abae4496-c9a2-4ad7-a3bf-a508792fa0af}</MetaDataID>
        public override void SelectionChange()
        {
        }

        /// <MetaDataID>{cae2c912-49d0-45d1-894b-57dce6c77bdd}</MetaDataID>
        public bool AllInHierarchyDefinesNewPrice
        {
            get
            {
                if (!DefinesNewPrice)
                    return false;
                foreach (var itemsPreparationInfoPresentation in Members.OfType<ItemsPriceInfoPresentation>())
                {
                    if (!itemsPreparationInfoPresentation.DefinesNewPrice)
                        return false;
                }
                return true;
            }
        }



        //AppearanceOrder
        /// <MetaDataID>{68294fd3-55bc-4e63-ac2d-453faacd02d9}</MetaDataID>
        public void Refresh(MenuItem menuItemWithChanges)
        {

            if (ItemsCategory is ItemsCategory && menuItemWithChanges.IsAncestor(ItemsCategory as ItemsCategory))
            {
                ItemsCategory.GetAllMenuItems();
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(DefinesNewPrice)));
                _Members = null;

                foreach (var itemsPreparationInfoPresentation in Members.OfType<ItemsPriceInfoPresentation>())
                    itemsPreparationInfoPresentation.Refresh(menuItemWithChanges);






                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));

                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(PreparationTimeIsVisible)));
            }

            if (MenuItem == menuItemWithChanges)
                Refresh();

        }
        /// <MetaDataID>{919c4b1c-7f8e-40e3-8ad7-fdcab01e6dbb}</MetaDataID>
        public void Refresh(ItemsCategory itemsCategoryWithChanges)
        {

            if (MenuItem is MenuItem)
            {
                if ((MenuItem as MenuItem).IsAncestor(itemsCategoryWithChanges))
                    Refresh();
            }

            if (ItemsCategory is ItemsCategory)
            {
                if ((ItemsCategory as ItemsCategory).IsAncestor(itemsCategoryWithChanges) || ItemsCategory == itemsCategoryWithChanges)
                    Refresh();

                if (itemsCategoryWithChanges.IsAncestor(ItemsCategory as ItemsCategory))
                {
                    RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(DefinesNewPrice)));
                    _Members = null;

                    foreach (var itemsPreparationInfoPresentation in Members.OfType<ItemsPriceInfoPresentation>())
                        itemsPreparationInfoPresentation.Refresh(itemsCategoryWithChanges);

                    foreach (var preparationSubStation in Members.OfType<ItemsPriceInfoPresentation>())
                        preparationSubStation.Refresh(itemsCategoryWithChanges);

                    RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
                    RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(PreparationTimeIsVisible)));
                }

            }
        }

        /// <MetaDataID>{9bbe65bf-458f-4f70-9218-4a3e24f9823d}</MetaDataID>
        public void Refresh()
        {
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(DefinesNewPrice)));
            //RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(IsCooked)));
            //RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(PreparationTimeSpanInMin)));
            //RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(TotalPreparationTimeSpanInMin)));
            //RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(HasTags)));
            //RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Tags)));
            //RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(HasAppearanceOrderValue)));
            //RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(AppearanceOrder)));
            //RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(AppearanceOrderText)));
            //RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(HasPreparationTimeSpanValue)));



            foreach (var itemsPreparationInfoPresentation in Members.OfType<ItemsPriceInfoPresentation>())
                itemsPreparationInfoPresentation.Refresh();
            _Members = null;
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));

            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(PreparationTimeIsVisible)));


        }
    }

}
