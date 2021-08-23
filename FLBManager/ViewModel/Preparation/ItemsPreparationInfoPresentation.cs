using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FlavourBusinessFacade.ServicesContextResources;
using MenuModel;
using WPFUIElementObjectBind;

namespace FLBManager.ViewModel.Preparation
{
    /// <MetaDataID>{6cb20dd9-745b-4737-82d2-f6b4f1bff1ce}</MetaDataID>
    public class ItemsPreparationInfoPresentation : FBResourceTreeNode, INotifyPropertyChanged
    {

        /// <MetaDataID>{017f504b-513b-4fc4-af42-c0c2ae4641b9}</MetaDataID>
        public readonly IItemsPreparationInfo ItemsPreparationInfo;
        /// <MetaDataID>{b369fbe0-d83b-467f-ba71-050afe49452e}</MetaDataID>
        public readonly PreparationStationPresentation PreparationStationPresentation;

        /// <summary>
        /// Preparation infos concern menu item
        /// </summary>
        // <MetaDataID>{4e50a2ba-e4a7-498e-a316-aa888e4692bd}</MetaDataID>
        IMenuItem MenuItem;

        /// <summary>
        ///  Preparation infos concern menu items of category
        /// </summary>
        // <MetaDataID>{a777298f-cf21-4ccb-848d-38596df908f3}</MetaDataID>
        internal IItemsCategory ItemsCategory;

   
        private void AddSubPreparationStation()
        {
            if (PreparationStationPresentation != null)
            {
                var preparationStation = PreparationStationPresentation.PreparationStation.NewSubStation();
            }
        }

        /// <summary>
        /// When EditMode is true you can include item in preparation station, exclude or edit item preparation info.
        /// </summary>
        /// <MetaDataID>{27d5229c-bc53-42bd-9d47-6ceea9487d1f}</MetaDataID>
       readonly bool EditMode;


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
        /// <param name="editMode">
        /// 
        /// </param>
        // <MetaDataID>{6d5912d3-dbd6-431c-b216-d6a4732da003}</MetaDataID>
        public ItemsPreparationInfoPresentation(PreparationStationPresentation preparationStationPresentation, IMenu menu, bool editMode) : base(preparationStationPresentation)
        {
            IsRootCategory = true;
            EditMode = editMode;
            ItemsPreparationInfo = new FlavourBusinessManager.ServicesContextResources.ItemsPreparationInfo(menu.RootCategory);
            PreparationStationPresentation = preparationStationPresentation;
            ItemsCategory = menu.RootCategory;

            foreach (var preparationSubStation in preparationStationPresentation.PreparationStation.SubStations)
                PreparationSubStations.GetViewModelFor(preparationSubStation, this, preparationSubStation, PreparationStationPresentation.MenuViewModel, editMode);

            DeleteCommand = new RelayCommand((object sender) =>
            {
                Delete();
            });

            NewSubPreparationStationCommand = new RelayCommand((object sender) =>
            {
                AddSubPreparationStation();
            });


        }

        /// <MetaDataID>{81bc990f-0fb8-46d4-9096-885f628cdae6}</MetaDataID>
        public ItemsPreparationInfoPresentation(ItemsPreparationInfoPresentation parent, PreparationStationPresentation preparationStationPresentation, MenuModel.IItemsCategory itemsCategory, bool editMode) : base(parent)
        {
            EditMode = editMode;
            PreparationStationPresentation = preparationStationPresentation;

            ItemsCategory = itemsCategory;

            DeleteCommand = new RelayCommand((object sender) =>
            {
                Delete();
            });
            NewSubPreparationStationCommand = new RelayCommand((object sender) =>
            {
                AddSubPreparationStation();
            });
            CheckBoxVisibility = System.Windows.Visibility.Visible;
        }

        /// <MetaDataID>{f76f2725-c4bc-48db-b680-cf20f594f218}</MetaDataID>
        public ItemsPreparationInfoPresentation(ItemsPreparationInfoPresentation parent, PreparationStationPresentation preparationStationPresentation, MenuModel.IMenuItem menuItem, bool editMode) : base(parent)
        {
            EditMode = editMode;
            PreparationStationPresentation = preparationStationPresentation;
            MenuItem = menuItem;

            DeleteCommand = new RelayCommand((object sender) =>
            {
                Delete();
            });

        }


        /// <MetaDataID>{2a48dd97-5dfd-4f46-aabb-a776aa182436}</MetaDataID>
        ViewModelWrappers<MenuModel.IItemsCategory, ItemsPreparationInfoPresentation> SubCategories = new ViewModelWrappers<MenuModel.IItemsCategory, ItemsPreparationInfoPresentation>();
        /// <MetaDataID>{760c0787-688d-4974-8c42-a58092d27c33}</MetaDataID>
        ViewModelWrappers<MenuModel.IMenuItem, ItemsPreparationInfoPresentation> MenuItems = new ViewModelWrappers<MenuModel.IMenuItem, ItemsPreparationInfoPresentation>();


        internal ViewModelWrappers<IPreparationStation, PreparationStationPresentation> PreparationSubStations = new ViewModelWrappers<IPreparationStation, PreparationStationPresentation>();

        /// <summary>
        /// Defines when ItemsPreparationInfoPresentation node has items which can prepared from preparation station 
        /// In view mode displayed, only items category node and items which prepared from preparation station
        /// </summary>
        // <MetaDataID>{5166e975-649f-47b1-b98e-5f1abf42f3ad}</MetaDataID>
        public bool HasItemsWhichCanPrepared
        {
            get
            {

                if (ItemsCategory != null && PreparationStationPresentation.StationPrepareItems(this.ItemsCategory))
                    return true;
                else if (this.MenuItem != null && PreparationStationPresentation.StationPrepareItem(this.MenuItem))
                    return true;
                else
                {
                    if (ItemsCategory != null)
                    {
                        var itemsPreparationInfosPresentations = (from subCategory in ItemsCategory.ClassifiedItems.OfType<MenuModel.IItemsCategory>()
                                                                  select SubCategories.GetViewModelFor(subCategory, this, PreparationStationPresentation, subCategory, EditMode)).Union(
                             (from menuItem in ItemsCategory.ClassifiedItems.OfType<MenuModel.MenuItem>()
                              select MenuItems.GetViewModelFor(menuItem, this, PreparationStationPresentation, menuItem, EditMode)));

                        var members = (from itemsPreparationInfoPresentation in itemsPreparationInfosPresentations
                                       select itemsPreparationInfoPresentation).OfType<FBResourceTreeNode>().ToList();

                        foreach (var member in members.OfType<ItemsPreparationInfoPresentation>())
                        {
                            if (member.HasItemsWhichCanPrepared)
                                return true;
                        }
                    }
                    return false;
                }
            }
        }

        /// <MetaDataID>{d37fedec-1f49-40c2-ad20-88b25bbaea54}</MetaDataID>
        public double PreparationTimeSpanInMin
        {
            get
            {

                if (this.ItemsCategory != null)
                    return this.PreparationStationPresentation.GetPreparationTimeSpanInMin(this.ItemsCategory);

                if (this.MenuItem != null)
                    return this.PreparationStationPresentation.GetPreparationTimeSpanInMin(this.MenuItem);

                return 1;

            }
            set
            {

                if (this.ItemsCategory != null)
                    this.PreparationStationPresentation.SetPreparationTimeSpanInMin(this.ItemsCategory, value);

                if (this.MenuItem != null)
                    this.PreparationStationPresentation.SetPreparationTimeSpanInMin(this.MenuItem, value);
            }
        }


        public bool PreparationTimeIsVisible
        {
            get
            {
                return CanPrepared;
            }
        }

        /// <MetaDataID>{c0a45032-38e7-4142-ba1e-47015ff8c0dd}</MetaDataID>
        public bool CanPrepared
        {
            get
            {
                if (this.ItemsCategory != null && this.PreparationStationPresentation.StationPrepareItems(this.ItemsCategory))
                    return true;
                else if (this.MenuItem != null && this.PreparationStationPresentation.StationPrepareItem(this.MenuItem))
                    return true;
                else
                    return false;
            }
            set
            {
                if (value && ItemsCategory != null)
                    PreparationStationPresentation.IncludeItems(ItemsCategory);

                if (!value && ItemsCategory != null)
                    PreparationStationPresentation.ExcludeItems(ItemsCategory);


                if (value && MenuItem != null)
                    PreparationStationPresentation.IncludeItem(MenuItem);

                if (!value && MenuItem != null)
                    this.PreparationStationPresentation.ExcludeItem(MenuItem);


                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(PreparationTimeIsVisible)));
                
                UpdatePresentationItems();

            }
        }
        /// <summary>
        /// Some times a change in items preparation info affect nodes in preparation station infos hierarchy
        /// this method update the  hierarchy if needed
        /// </summary>
        private void UpdatePresentationItems()
        {

            if (ItemsCategory != null)
            {
                Root.Refresh(ItemsCategory as ItemsCategory);
                Root.PreparationStationPresentation.Refresh(ItemsCategory as ItemsCategory);
            }
     

            if (MenuItem != null)
            {
                Root.Refresh(MenuItem as MenuItem);
                Root.PreparationStationPresentation.Refresh(MenuItem as MenuItem);
            }


            //Refresh();
        }
        ItemsPreparationInfoPresentation Root
        {
            get
            {
                ItemsPreparationInfoPresentation mainPreparationStationPresentation = null;
                FBResourceTreeNode parent = this;
                if (parent is ItemsPreparationInfoPresentation)
                    mainPreparationStationPresentation = parent as ItemsPreparationInfoPresentation;
                while (parent.Parent != null)
                {
                    parent = parent.Parent;
                    if (parent is ItemsPreparationInfoPresentation)
                        mainPreparationStationPresentation = parent as ItemsPreparationInfoPresentation;
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
                PreparationStationPresentation.ExcludeItems(ItemsCategory);
            if (this.MenuItem != null)
                PreparationStationPresentation.ExcludeItem(MenuItem);
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

                    members.AddRange(PreparationSubStations.Values);



                    if (EditMode)
                    {
                        var itemsPreparationInfosPresentations = (from subCategory in ItemsCategory.ClassifiedItems.OfType<MenuModel.IItemsCategory>()
                                                                  where PreparationStationPresentation.StationPrepareItems(subCategory) || (!PreparationStationPresentation.IsCategoryAssigned(subCategory))// equipmentPresentaion == null || !(equipmentPresentaion.Parent as ItemsPreparationInfoPresentation).IsCategoryAssigned(subCategory)
                                                                  select SubCategories.GetViewModelFor(subCategory, this, PreparationStationPresentation, subCategory, EditMode)).Union(
                           (from menuItem in ItemsCategory.ClassifiedItems.OfType<MenuModel.MenuItem>()

                            where PreparationStationPresentation.StationPrepareItem(menuItem) || (!PreparationStationPresentation.IsMenuItemAssigned(menuItem))//  equipmentPresentaion == null || !(equipmentPresentaion.Parent as ItemsPreparationInfoPresentation).IsMenuItemAssigned(menuItem)
                            select MenuItems.GetViewModelFor(menuItem, this, PreparationStationPresentation, menuItem, EditMode)));

                        members.AddRange((from itemsPreparationInfoPresentation in itemsPreparationInfosPresentations
                                          select itemsPreparationInfoPresentation).OfType<FBResourceTreeNode>().ToList());
                        _Members = members;
                        return members;
                    }
                    else
                    {
                        var itemsPreparationInfosPresentations = (from subCategory in ItemsCategory.ClassifiedItems.OfType<MenuModel.IItemsCategory>()
                                                                  select SubCategories.GetViewModelFor(subCategory, this, PreparationStationPresentation, subCategory, EditMode)).Union(
                           (from menuItem in ItemsCategory.ClassifiedItems.OfType<MenuModel.MenuItem>()
                            select MenuItems.GetViewModelFor(menuItem, this, PreparationStationPresentation, menuItem, EditMode)));

                        members.AddRange((from itemsPreparationInfoPresentation in itemsPreparationInfosPresentations
                                          where itemsPreparationInfoPresentation.HasItemsWhichCanPrepared
                                          select itemsPreparationInfoPresentation).OfType<FBResourceTreeNode>().ToList());
                        _Members = members;
                        return members;

                    }
                }
                else
                    return new List<FBResourceTreeNode>();
            }
        }

    
        /// <MetaDataID>{ff9d9100-88c5-453d-ae84-6583239e8501}</MetaDataID>
        public RelayCommand DeleteCommand { get; protected set; }

        public RelayCommand NewSubPreparationStationCommand { get; protected set; }



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



                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/FLBManager;Component/Resources/Images/Metro/bbq16.png"));
                    menuItem.Header = Properties.Resources.NewSubPreparationStationHeader;
                    menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = NewSubPreparationStationCommand;
                    _ContextMenuItems.Add(menuItem);

                    if (this.Parent is PreparationStationPresentation)
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

        public override void SelectionChange()
        {
        }

        public bool AllInHierarchyCanPrepared
        {
            get
            {
                if (!CanPrepared)
                    return false;
                foreach (var itemsPreparationInfoPresentation in Members.OfType<ItemsPreparationInfoPresentation>())
                {
                    if (!itemsPreparationInfoPresentation.AllInHierarchyCanPrepared)
                        return false;
                }
                return true;
            }
        }





       public bool IncludeAllItemsAllowed
        {
            get
            {

                if (ItemsCategory is ItemsCategory)
                {
                    List<PreparationStationPresentation> preparationStations = new List<PreparationStationPresentation>() { Root.PreparationStationPresentation };
                    preparationStations.AddRange(Root.PreparationStationPresentation.PreparationSubStations);

                    if (EditMode && preparationStations.Count > 1)
                    {


                        List<PreparationStationPresentation> categoryMenuItemsPreparationStations = new List<PreparationStationPresentation>() { this.PreparationStationPresentation };

                        foreach (var menuItem in ItemsCategory.GetAllMenuItems())
                        {
                            foreach (var preparationStationPresentation in preparationStations)
                            {
                                if (preparationStationPresentation.StationPrepareItem(menuItem))
                                {
                                    if (!categoryMenuItemsPreparationStations.Contains(preparationStationPresentation))
                                        categoryMenuItemsPreparationStations.Add(preparationStationPresentation);

                                    if (categoryMenuItemsPreparationStations.Count > 1)
                                        return false;
                                }
                            }
                        }
                    }
                }

                return true;
            }
        }

        public void Refresh(MenuItem menuItemWithChanges)
        {

            if (ItemsCategory is ItemsCategory && menuItemWithChanges.IsAncestor(ItemsCategory as ItemsCategory))
            {
                ItemsCategory.GetAllMenuItems();
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(CanPrepared)));
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(PreparationTimeSpanInMin)));
                _Members = null;

                foreach (var itemsPreparationInfoPresentation in Members.OfType<ItemsPreparationInfoPresentation>())
                    itemsPreparationInfoPresentation.Refresh(menuItemWithChanges);


                foreach (var preparationSubStation in Members.OfType<PreparationStationPresentation>())
                    preparationSubStation.Refresh(menuItemWithChanges);



                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(IncludeAllItemsAllowed)));
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(PreparationTimeIsVisible)));
            }

            if (MenuItem == menuItemWithChanges)
                Refresh();

        }
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
                    RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(CanPrepared)));
                    RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(PreparationTimeSpanInMin)));
                    _Members = null;

                    foreach (var itemsPreparationInfoPresentation in Members.OfType<ItemsPreparationInfoPresentation>())
                        itemsPreparationInfoPresentation.Refresh(itemsCategoryWithChanges);

                    foreach (var preparationSubStation in Members.OfType<PreparationStationPresentation>())
                        preparationSubStation.Refresh(itemsCategoryWithChanges);

                    RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
                    RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(IncludeAllItemsAllowed)));
                    RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(PreparationTimeIsVisible)));
                }

            }
        }

        /// <MetaDataID>{9bbe65bf-458f-4f70-9218-4a3e24f9823d}</MetaDataID>
        public void Refresh()
        {
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(CanPrepared)));
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(PreparationTimeSpanInMin)));


            foreach (var itemsPreparationInfoPresentation in Members.OfType<ItemsPreparationInfoPresentation>())
                itemsPreparationInfoPresentation.Refresh();
            _Members = null;
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(IncludeAllItemsAllowed)));
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(PreparationTimeIsVisible)));

        }
    }
}
