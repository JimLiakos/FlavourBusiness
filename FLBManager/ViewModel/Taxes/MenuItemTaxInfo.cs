using Finance.ViewModel;
using MenuModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using UIBaseEx;
using WPFUIElementObjectBind;

namespace FLBManager.ViewModel.Taxes
{
    /// <MetaDataID>{feab2210-beff-4bf7-a39f-e459740c9a37}</MetaDataID>
    public class MenuItemTaxInfo : FBResourceTreeNode, INotifyPropertyChanged
    {
        public override void RemoveChild(FBResourceTreeNode treeNode)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Preparation infos concern menu item
        /// </summary>
        MenuItem MenuItem;

        /// <summary>
        ///  Preparation infos concern menu items of category
        /// </summary>
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
      
        public MenuItemTaxInfo(FBResourceTreeNode parent, IMenu menu, bool editMode) : base(parent)
        {
            IsRootCategory = true;
            EditMode = editMode;

            ItemsCategory = menu.RootCategory;


         

         

        }
        TaxableTypeViewModel _TaxableType;
        public TaxableTypeViewModel TaxableType
        {
            get
            {
                return _TaxableType;
            }
            set
            {
                var oldIncludeAllItemsAllowed = IncludeAllItemsAllowed;
                _TaxableType = value;
                foreach (var menuItemTaxInfo in Members.OfType<MenuItemTaxInfo>())
                    menuItemTaxInfo.TaxableType = _TaxableType;

                if (IncludeAllItemsAllowed != oldIncludeAllItemsAllowed)
                {
                    RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(IncludeAllItemsAllowed)));
                    RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(TaxTypeAssigned)));
                }
            }
        }

        /// <MetaDataID>{81bc990f-0fb8-46d4-9096-885f628cdae6}</MetaDataID>
        public MenuItemTaxInfo(FBResourceTreeNode parent, MenuModel.IItemsCategory itemsCategory, bool editMode) : base(parent)
        {
            EditMode = editMode;
            ItemsCategory = itemsCategory;

            CheckBoxVisibility = System.Windows.Visibility.Visible;
        }

        /// <MetaDataID>{f76f2725-c4bc-48db-b680-cf20f594f218}</MetaDataID>
        public MenuItemTaxInfo(FBResourceTreeNode parent, MenuItem menuItem, bool editMode) : base(parent)
        {
            EditMode = editMode;

            MenuItem = menuItem;
        }


        /// <summary>
        /// Defines when ItemsPreparationInfoPresentation node has items which can prepared from preparation station 
        /// In view mode displayed, only items category node and items which prepared from preparation station
        /// </summary>
        /// <MetaDataID>{2f994810-395c-48aa-8eb6-d44938a06d5c}</MetaDataID>
        // <MetaDataID>{5166e975-649f-47b1-b98e-5f1abf42f3ad}</MetaDataID>
        public bool HasItemsToAssignTaxableType
        {
            get
            {
              
                if (this.TaxableType == null)
                    return false;
                if (Members.OfType<MenuItemTaxInfo>().Where(x => !x.HasItemsToAssignTaxableType).FirstOrDefault() != null)
                    return false;
                if (MenuItem?.TaxableType == null || TaxableType.TaxableType == MenuItem?.TaxableType)
                    return true;
                return false;
            }
        }






        /// <MetaDataID>{c0a45032-38e7-4142-ba1e-47015ff8c0dd}</MetaDataID>
        public bool TaxTypeAssigned
        {
            get
            {
                if(ItemsCategory!=null)
                {
                    if (Members.OfType<MenuItemTaxInfo>().Where(x => !x.TaxTypeAssigned).FirstOrDefault() != null)
                        return false;
                    else
                        return true;
                }
                if (MenuItem == null || TaxableType == null)
                    return false;
                if (MenuItem.TaxableType != null && MenuItem.TaxableType == TaxableType.TaxableType)
                    return true;

                return false;
            }
            set
            {
                if (value)
                    SetMenuItemTaxableType();
                else
                    ResetMenuItemTaxableType();
            }
        }

        private void ResetMenuItemTaxableType()
        {
            foreach (var menuItemTaxInfo in Members.OfType<MenuItemTaxInfo>())
                menuItemTaxInfo.ResetMenuItemTaxableType();
            if (MenuItem != null && MenuItem.TaxableType != null && TaxableType?.TaxableType == MenuItem.TaxableType)
                MenuItem.TaxableType = null;
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(IncludeAllItemsAllowed)));
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(TaxTypeAssigned)));
        }

        private void SetMenuItemTaxableType()
        {
            foreach (var menuItemTaxInfo in Members.OfType<MenuItemTaxInfo>())
                menuItemTaxInfo.SetMenuItemTaxableType();
            

            if (MenuItem != null && MenuItem.TaxableType == null && TaxableType != null)
                MenuItem.TaxableType = TaxableType.TaxableType;
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(IncludeAllItemsAllowed)));
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(TaxTypeAssigned)));

        }



        /// <MetaDataID>{fad3761a-9049-4cf1-8c5e-ffe25141ee69}</MetaDataID>
        public System.Windows.Visibility CheckBoxVisibility
        {
            get; set;
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


        ViewModelWrappers<IItemsCategory, MenuItemTaxInfo> SubCategories = new ViewModelWrappers<IItemsCategory, MenuItemTaxInfo>();

        ViewModelWrappers<IMenuItem, MenuItemTaxInfo> MenuItems = new ViewModelWrappers<IMenuItem, MenuItemTaxInfo>();


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
                        var itemsPreparationInfosPresentations = (from subCategory in ItemsCategory.ClassifiedItems.OfType<MenuModel.IItemsCategory>()

                                                                  select SubCategories.GetViewModelFor(subCategory, this, subCategory, EditMode)).Union(
                                       (from menuItem in ItemsCategory.ClassifiedItems.OfType<MenuModel.MenuItem>()
                                        select MenuItems.GetViewModelFor(menuItem, this, menuItem, EditMode)));

                        members.AddRange((from itemsPreparationInfoPresentation in itemsPreparationInfosPresentations
                                          select itemsPreparationInfoPresentation).OfType<FBResourceTreeNode>().ToList());
                        _Members = members;
                        return members;
                    }
                    else
                    {
                        var itemsPreparationInfosPresentations = (from subCategory in ItemsCategory.ClassifiedItems.OfType<MenuModel.IItemsCategory>()
                                                                  select SubCategories.GetViewModelFor(subCategory, this, subCategory, EditMode)).Union(
                           (from menuItem in ItemsCategory.ClassifiedItems.OfType<MenuModel.MenuItem>()
                            select MenuItems.GetViewModelFor(menuItem, this, menuItem, EditMode)));

                        members.AddRange((from itemsPreparationInfoPresentation in itemsPreparationInfosPresentations
                                          where itemsPreparationInfoPresentation.HasItemsToAssignTaxableType
                                          select itemsPreparationInfoPresentation).OfType<FBResourceTreeNode>().ToList());
                        _Members = members;
                        return members;

                    }
                }
                else
                    return new List<FBResourceTreeNode>();
            }
        }






        public override List<MenuCommand> ContextMenuItems
        {
            get
            {
                return null;
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

        public bool IncludeAllItemsAllowed
        {
            get
            {
                if (ItemsCategory != null)
                {
                    if (HasItemsToAssignTaxableType)
                        return true;
                    else
                        return false;
                }
                if (MenuItem == null || TaxableType == null)
                    return false;
                if (MenuItem.TaxableType != null && MenuItem.TaxableType != TaxableType.TaxableType)
                    return false;
                if (TaxableType == null)
                    return false;
                return true;
            }
        }

       
    }

}
