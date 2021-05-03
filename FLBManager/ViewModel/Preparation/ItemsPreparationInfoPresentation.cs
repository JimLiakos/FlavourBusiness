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

        public readonly IItemsPreparationInfo ItemsPreparationInfo;
        PreparationStationPresentation PreparationStationPresentation;
        ItemsPreparationInfoPresentation ItemsPreparationInfoTreeNode;
        IMenuItem MenuItem;


        public ItemsPreparationInfoPresentation(PreparationStationPresentation parent, IItemsPreparationInfo itemsPreparationInfo, bool selectionCheckBox) : base(parent)
        {
            SelectionCheckBox = selectionCheckBox;
            ItemsPreparationInfo = itemsPreparationInfo;
            PreparationStationPresentation = parent;
            var @object = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsPreparationInfo.ItemsInfoObjectUri);
            ItemsCategory = @object as MenuModel.IItemsCategory;

            DeleteCommand = new RelayCommand((object sender) =>
            {
                Delete();
            });


        }

        bool SelectionCheckBox;
        public ItemsPreparationInfoPresentation(ItemsPreparationInfoPresentation parent, PreparationStationPresentation preparationStationPresentation, MenuModel.IItemsCategory itemsCategory, bool selectionCheckBox) : base(parent)
        {
            SelectionCheckBox = selectionCheckBox;
            PreparationStationPresentation = preparationStationPresentation;

            ItemsPreparationInfoTreeNode = parent;
            ItemsCategory = itemsCategory;

            DeleteCommand = new RelayCommand((object sender) =>
            {
                Delete();
            });

            CheckBoxVisibility = System.Windows.Visibility.Visible;
        }

        public ItemsPreparationInfoPresentation(ItemsPreparationInfoPresentation parent, PreparationStationPresentation preparationStationPresentation, MenuModel.IMenuItem menuItem, bool selectionCheckBox) : base(parent)
        {
            SelectionCheckBox = selectionCheckBox;
            PreparationStationPresentation = preparationStationPresentation;
            ItemsPreparationInfoTreeNode = parent;
            MenuItem = menuItem;

            DeleteCommand = new RelayCommand((object sender) =>
            {
                Delete();
            });

        }


        ViewModelWrappers<MenuModel.IItemsCategory, ItemsPreparationInfoPresentation> SubCategories = new ViewModelWrappers<MenuModel.IItemsCategory, ItemsPreparationInfoPresentation>();
        ViewModelWrappers<MenuModel.IMenuItem, ItemsPreparationInfoPresentation> MenuItems = new ViewModelWrappers<MenuModel.IMenuItem, ItemsPreparationInfoPresentation>();


        ViewModelWrappers<MenuModel.IItemsCategory, ItemsPreparationInfoPresentation> MultiSelectSubCategories = new ViewModelWrappers<MenuModel.IItemsCategory, ItemsPreparationInfoPresentation>();
        ViewModelWrappers<MenuModel.IMenuItem, ItemsPreparationInfoPresentation> MultiSelectMenuItems = new ViewModelWrappers<MenuModel.IMenuItem, ItemsPreparationInfoPresentation>();



        public bool HasItemsToPrepared
        {
            get
            {
                if (this.ItemsCategory != null && this.PreparationStationPresentation.StationPrepareItems(this.ItemsCategory))
                    return true;
                else if (this.MenuItem != null && this.PreparationStationPresentation.StationPrepareItem(this.MenuItem))
                    return true;
                else
                {
                    if (ItemsCategory != null)
                    {
                        var itemsPreparationInfosPresentations = (from subCategory in ItemsCategory.ClassifiedItems.OfType<MenuModel.IItemsCategory>()
                                                                  select MultiSelectSubCategories.GetViewModelFor(subCategory, this, PreparationStationPresentation, subCategory, SelectionCheckBox)).Union(
                             (from menuItem in ItemsCategory.ClassifiedItems.OfType<MenuModel.MenuItem>()
                              select MultiSelectMenuItems.GetViewModelFor(menuItem, this, PreparationStationPresentation, menuItem, SelectionCheckBox)));

                        var members = (from itemsPreparationInfoPresentation in itemsPreparationInfosPresentations
                                       select itemsPreparationInfoPresentation).OfType<FBResourceTreeNode>().ToList();

                        foreach (var member in members.OfType<ItemsPreparationInfoPresentation>())
                        {
                            if (member.HasItemsToPrepared)
                                return true;
                        }
                    }
                    return false;
                }
            }
        }

        public double PreparationTimeSpanInMin
        {
            get
            {

                if (this.ItemsCategory !=null)
                    return this.PreparationStationPresentation.GetPreparationTimeSpanInMin(this.ItemsCategory) ;

                if (this.MenuItem != null)
                    return this.PreparationStationPresentation.GetPreparationTimeSpanInMin(this.MenuItem);

                return 1;

            }
            set 
            {

                if (this.ItemsCategory != null)
                    this.PreparationStationPresentation.SetPreparationTimeSpanInMin(this.ItemsCategory,value);

                if (this.MenuItem != null)
                    this.PreparationStationPresentation.SetPreparationTimeSpanInMin(this.MenuItem,value);
            }
        }

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
                    this.PreparationStationPresentation.IncludeItems(ItemsCategory);

                if (!value && ItemsCategory != null)
                    this.PreparationStationPresentation.ExcludeItems(ItemsCategory);


                if (value && MenuItem != null)
                {
                    this.PreparationStationPresentation.IncludeItem(MenuItem);
                    Refresh();
                }

                if (!value && MenuItem != null)
                    this.PreparationStationPresentation.ExcludeItem(MenuItem);


            }
        }
        public System.Windows.Visibility CheckBoxVisibility
        {
            get; set;
        }
        private void Delete()
        {
            if (ItemsCategory != null)
                PreparationStationPresentation.ExcludeItems(ItemsCategory);

            if (this.MenuItem != null)
                PreparationStationPresentation.ExcludeItem(MenuItem);

        }

        internal MenuModel.IItemsCategory ItemsCategory;

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

        public override List<FBResourceTreeNode> Members
        {
            get
            {

                if (ItemsCategory != null)
                {

                    if (SelectionCheckBox)
                    {
                        var itemsPreparationInfosPresentations = (from subCategory in ItemsCategory.ClassifiedItems.OfType<MenuModel.IItemsCategory>()
                                                                  select MultiSelectSubCategories.GetViewModelFor(subCategory, this, PreparationStationPresentation, subCategory, SelectionCheckBox)).Union(
                           (from menuItem in ItemsCategory.ClassifiedItems.OfType<MenuModel.MenuItem>()
                            select MultiSelectMenuItems.GetViewModelFor(menuItem, this, PreparationStationPresentation, menuItem, SelectionCheckBox)));

                        var members = (from itemsPreparationInfoPresentation in itemsPreparationInfosPresentations
                                       select itemsPreparationInfoPresentation).OfType<FBResourceTreeNode>().ToList();
                        return members;
                    }
                    else
                    {
                        var itemsPreparationInfosPresentations = (from subCategory in ItemsCategory.ClassifiedItems.OfType<MenuModel.IItemsCategory>()
                                                                  select SubCategories.GetViewModelFor(subCategory, this, PreparationStationPresentation, subCategory, SelectionCheckBox)).Union(
                           (from menuItem in ItemsCategory.ClassifiedItems.OfType<MenuModel.MenuItem>()
                            select MenuItems.GetViewModelFor(menuItem, this, PreparationStationPresentation, menuItem, SelectionCheckBox)));

                        var members = (from itemsPreparationInfoPresentation in itemsPreparationInfosPresentations
                                       where itemsPreparationInfoPresentation.HasItemsToPrepared
                                       select itemsPreparationInfoPresentation).OfType<FBResourceTreeNode>().ToList();
                        return members;

                    }
                }
                else
                    return new List<FBResourceTreeNode>();
            }
        }
        public RelayCommand DeleteCommand { get; protected set; }

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
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/delete.png"));
                    menuItem.Header = Properties.Resources.RemoveCallerIDServer;
                    menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = DeleteCommand;

                    _ContextMenuItems.Add(menuItem);



                }

                return _ContextMenuItems;
            }
        }
        public override bool HasContextMenu
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
        public override void SelectionChange()
        {
        }


        public void Refresh()
        {
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(CanPrepared)));
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(PreparationTimeSpanInMin)));


            foreach (var itemsPreparationInfoPresentation in Members.OfType<ItemsPreparationInfoPresentation>())
                itemsPreparationInfoPresentation.Refresh();

            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));

        }
    }
}
