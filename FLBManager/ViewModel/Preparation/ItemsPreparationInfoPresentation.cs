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
        PreparationStationPresentation PreparationStationPresentation;
        /// <MetaDataID>{20c119a0-5431-48fd-a824-18518c222158}</MetaDataID>
        ItemsPreparationInfoPresentation ItemsPreparationInfoTreeNode;
        /// <MetaDataID>{4e50a2ba-e4a7-498e-a316-aa888e4692bd}</MetaDataID>
        IMenuItem MenuItem;


        /// <MetaDataID>{6d5912d3-dbd6-431c-b216-d6a4732da003}</MetaDataID>
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

            NewSubPreparationStationCommand = new RelayCommand((object sender) =>
            {
                AddSubPreparationStation();
            });


        }

        private void AddSubPreparationStation()
        {
            if (PreparationStationPresentation != null)
            {
                var preparationStation = PreparationStationPresentation.PreparationStation.NewSubStation();




            }
        }

        /// <MetaDataID>{27d5229c-bc53-42bd-9d47-6ceea9487d1f}</MetaDataID>
        bool SelectionCheckBox;
        /// <MetaDataID>{81bc990f-0fb8-46d4-9096-885f628cdae6}</MetaDataID>
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
            NewSubPreparationStationCommand = new RelayCommand((object sender) =>
            {
                AddSubPreparationStation();
            });
            CheckBoxVisibility = System.Windows.Visibility.Visible;
        }

        /// <MetaDataID>{f76f2725-c4bc-48db-b680-cf20f594f218}</MetaDataID>
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


        /// <MetaDataID>{2a48dd97-5dfd-4f46-aabb-a776aa182436}</MetaDataID>
        ViewModelWrappers<MenuModel.IItemsCategory, ItemsPreparationInfoPresentation> SubCategories = new ViewModelWrappers<MenuModel.IItemsCategory, ItemsPreparationInfoPresentation>();
        /// <MetaDataID>{760c0787-688d-4974-8c42-a58092d27c33}</MetaDataID>
        ViewModelWrappers<MenuModel.IMenuItem, ItemsPreparationInfoPresentation> MenuItems = new ViewModelWrappers<MenuModel.IMenuItem, ItemsPreparationInfoPresentation>();


        /// <MetaDataID>{1ca3d486-6b70-49d2-bfaf-b311b29f1f5d}</MetaDataID>
        ViewModelWrappers<MenuModel.IItemsCategory, ItemsPreparationInfoPresentation> MultiSelectSubCategories = new ViewModelWrappers<MenuModel.IItemsCategory, ItemsPreparationInfoPresentation>();
        /// <MetaDataID>{a9216788-5f5a-4f64-b84b-b9b1ccfe46df}</MetaDataID>
        ViewModelWrappers<MenuModel.IMenuItem, ItemsPreparationInfoPresentation> MultiSelectMenuItems = new ViewModelWrappers<MenuModel.IMenuItem, ItemsPreparationInfoPresentation>();



        /// <MetaDataID>{5166e975-649f-47b1-b98e-5f1abf42f3ad}</MetaDataID>
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
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(PreparationTimeIsVisible)));

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

        /// <MetaDataID>{a777298f-cf21-4ccb-848d-38596df908f3}</MetaDataID>
        internal MenuModel.IItemsCategory ItemsCategory;

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

        /// <MetaDataID>{d2401e62-1e30-4557-9b75-142a3f9c4690}</MetaDataID>
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
        /// <MetaDataID>{05929148-020e-4a78-b9d4-9e153c7929bd}</MetaDataID>
        public override void SelectionChange()
        {
        }


        /// <MetaDataID>{9bbe65bf-458f-4f70-9218-4a3e24f9823d}</MetaDataID>
        public void Refresh()
        {
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(CanPrepared)));
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(PreparationTimeSpanInMin)));


            foreach (var itemsPreparationInfoPresentation in Members.OfType<ItemsPreparationInfoPresentation>())
                itemsPreparationInfoPresentation.Refresh();

            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(PreparationTimeIsVisible)));

        }
    }
}
