using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FlavourBusinessFacade.ServicesContextResources;
using MenuItemsEditor.ViewModel;
using MenuModel;
using OOAdvantech.Remoting.RestApi.Serialization;
using OOAdvantech.Transactions;
using StyleableWindow;
using WPFUIElementObjectBind;

namespace FLBManager.ViewModel.Preparation
{
    /// <MetaDataID>{8c2727a3-6d5f-4ad3-b771-0cfd38a663a8}</MetaDataID>
    public class PreparationStationPresentation : FBResourceTreeNode, INotifyPropertyChanged, IDragDropTarget
    {


        public void SetPreparationTimeSpanInMin(MenuModel.IItemsCategory itemsCategory, double timeSpanInMinutes)
        {
            var itemsPreparationInfos = (from itemsInfo in ItemsPreparationInfos
                                         select new
                                         {
                                             @object = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfo.ItemsInfoObjectUri),
                                             ItemsPreparationInfo = itemsInfo
                                         }).ToList();

            var itemsPreparationInfo = (from itemsInfoEntry in itemsPreparationInfos
                                                where itemsInfoEntry.@object == itemsCategory && (itemsInfoEntry.ItemsPreparationInfo.Included() || itemsInfoEntry.ItemsPreparationInfo.PreparationTime())
                                                select itemsInfoEntry.ItemsPreparationInfo).FirstOrDefault();

            if (itemsPreparationInfo != null)
            {
                itemsPreparationInfo.ItemsPreparationInfoType |= ItemsPreparationInfoType.PreparationTime;
                itemsPreparationInfo.PreparationTimeSpanInMin = timeSpanInMinutes;
            }
            else
            {


                string uri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(itemsCategory).GetPersistentObjectUri(itemsCategory);
                itemsPreparationInfo = this.PreparationStation.NewPreparationInfo(uri, ItemsPreparationInfoType.PreparationTime);
                itemsPreparationInfo.PreparationTimeSpanInMin = timeSpanInMinutes;
                this.ItemsPreparationInfos.Add(itemsPreparationInfo);
            }
            foreach (var itemsPreparationInfoPresentation in ItemsToChoose.OfType<ItemsPreparationInfoPresentation>())
                itemsPreparationInfoPresentation.Refresh();

            // updates infrastacture vew 

            if (PreparationStationItems != null)
                PreparationStationItems.Refresh();
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));



        }
        public void SetPreparationTimeSpanInMin(MenuModel.IMenuItem menuItem, double timeSpanInMinutes)
        {

            var itemsPreparationInfos = (from itemsInfo in ItemsPreparationInfos
                                         select new
                                         {
                                             @object = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfo.ItemsInfoObjectUri),
                                             ItemsPreparationInfo = itemsInfo
                                         }).ToList();

            var itemsPreparationInfo = (from itemsInfoEntry in itemsPreparationInfos
                                                where itemsInfoEntry.@object == menuItem 
                                                select itemsInfoEntry.ItemsPreparationInfo).FirstOrDefault();

            if (itemsPreparationInfo != null)
            {
                itemsPreparationInfo.ItemsPreparationInfoType |= ItemsPreparationInfoType.PreparationTime;
                itemsPreparationInfo.PreparationTimeSpanInMin = timeSpanInMinutes;
            }
            else
            {
                string uri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(menuItem).GetPersistentObjectUri(menuItem);

                itemsPreparationInfo = this.PreparationStation.NewPreparationInfo(uri, ItemsPreparationInfoType.PreparationTime);
                itemsPreparationInfo.PreparationTimeSpanInMin = timeSpanInMinutes;
                this.ItemsPreparationInfos.Add(itemsPreparationInfo);
                //_Members.Add(new ItemsPreparationInfoPresentation(this, itemsPreparationInfo));
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));

                foreach (var itemsPreparationInfoPresentation in ItemsToChoose.OfType<ItemsPreparationInfoPresentation>())
                    itemsPreparationInfoPresentation.Refresh();
            }

            if (PreparationStationItems != null)
                PreparationStationItems.Refresh();
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));

        }

        internal double GetPreparationTimeSpanInMin(IMenuItem menuItem)
        {
            var itemsPreparationInfos = (from itemsInfo in ItemsPreparationInfos
                                         select new
                                         {
                                             @object = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfo.ItemsInfoObjectUri),
                                             ItemsPreparationInfo = itemsInfo
                                         }).ToList();

            foreach (var itemsPreparationInfoEntry in itemsPreparationInfos)
            {
                if (itemsPreparationInfoEntry.@object is MenuModel.IMenuItem && (itemsPreparationInfoEntry.@object as MenuModel.IMenuItem) == menuItem)
                    return itemsPreparationInfoEntry.ItemsPreparationInfo.PreparationTimeSpanInMin;

            }

            foreach (var itemsPreparationInfoEntry in itemsPreparationInfos)
            {
                if (itemsPreparationInfoEntry.@object is MenuModel.IItemsCategory)
                {
                    MenuModel.IItemsCategory itemsCategory = null;
                    var itemsPreparationInfoCategory = (itemsPreparationInfoEntry.@object as MenuModel.IItemsCategory);
                    if (menuItem is MenuModel.IClassified)
                    {
                        itemsCategory = (menuItem as MenuModel.IClassified).Class as MenuModel.ItemsCategory;
                        return GetPreparationTimeSpanInMin(itemsCategory);
                    }
                }
            }
            return 1;
        }

        internal double GetPreparationTimeSpanInMin(IItemsCategory itemsCategory)
        {

            var itemsPreparationInfos = (from itemsInfo in ItemsPreparationInfos
                                         select new
                                         {
                                             ItemsInfoObjectUri = itemsInfo.ItemsInfoObjectUri,
                                             @object = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfo.ItemsInfoObjectUri),
                                             ItemsPreparationInfo = itemsInfo
                                         }).ToList();

            foreach (var itemsPreparationInfoEntry in itemsPreparationInfos)
            {
                if (itemsPreparationInfoEntry.@object is MenuModel.IItemsCategory && (itemsPreparationInfoEntry.@object as MenuModel.IItemsCategory) == itemsCategory)
                {
                    if (itemsPreparationInfoEntry.ItemsPreparationInfo.PreparationTime())
                        return itemsPreparationInfoEntry.ItemsPreparationInfo.PreparationTimeSpanInMin;
                }
            }

            foreach (var itemsPreparationInfo in ItemsPreparationInfos)
            {
                MenuModel.IItemsCategory itemsCategoryOrParent = itemsCategory;
                var @object = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsPreparationInfo.ItemsInfoObjectUri);
                if (@object is MenuModel.IItemsCategory)
                {
                    var itemsPreparationInfoCategory = (@object as MenuModel.IItemsCategory);
                    while (itemsCategoryOrParent != null && itemsCategoryOrParent != itemsPreparationInfoCategory)
                        itemsCategoryOrParent = itemsCategoryOrParent.Class as MenuModel.IItemsCategory;
                    if (itemsCategoryOrParent == itemsPreparationInfoCategory && itemsPreparationInfo.PreparationTime())
                        return itemsPreparationInfo.PreparationTimeSpanInMin;
                }
            }
            return 1;
        }

        public void IncludeItems(MenuModel.IItemsCategory itemsCategory)
        {

            var itemsPreparationInfos = (from itemsInfo in ItemsPreparationInfos
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
                PreparationStation.RemovePreparationInfo(excludedItemsPreparationInfo);
                ItemsPreparationInfos.Remove(excludedItemsPreparationInfo);

            }

            if (!StationPrepareItems(itemsCategory))
            {
                string uri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(itemsCategory).GetPersistentObjectUri(itemsCategory);

                var itemsPreparationInfo = this.PreparationStation.NewPreparationInfo(uri, ItemsPreparationInfoType.Include);

                this.ItemsPreparationInfos = this.PreparationStation.ItemsPreparationInfos.ToList();
            }

            List<IItemsPreparationInfo> uselessDescendantItemsPreparationInfos = GetUselessDescendantItemsPreparationInfos(itemsCategory);
            if (uselessDescendantItemsPreparationInfos.Count > 0)
            {
                // the item preparation infos which refer to items or category which contained in included category and are useless must be removed
                PreparationStation.RemovePreparationInfos(uselessDescendantItemsPreparationInfos);
                ItemsPreparationInfos = PreparationStation.ItemsPreparationInfos.ToList();
            }
            foreach (var itemsPreparationInfoPresentation in ItemsToChoose.OfType<ItemsPreparationInfoPresentation>())
                itemsPreparationInfoPresentation.Refresh();

            // updates infrastacture vew 

            if (PreparationStationItems != null)
                PreparationStationItems.Refresh();
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
        }

        private List<IItemsPreparationInfo> GetUselessDescendantItemsPreparationInfos(IItemsCategory itemsCategory)
        {

            List<IItemsPreparationInfo> itemsPreparationInfos = new List<IItemsPreparationInfo>();

            var itemsPreparationInfosEntry = (from itemsInfo in ItemsPreparationInfos
                                              select new
                                              {
                                                  @object = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfo.ItemsInfoObjectUri),
                                                  ItemsPreparationInfo = itemsInfo
                                              }).ToList();

            foreach (var itemsPreparationInfo in itemsPreparationInfosEntry)
            {
                if (itemsPreparationInfo.ItemsPreparationInfo.Included())
                {
                    if (itemsPreparationInfo.@object is IItemsCategory)
                        if (IsDescendantOfCategory(itemsCategory, itemsPreparationInfo.@object as IItemsCategory))
                        {
                            itemsPreparationInfos.Add(itemsPreparationInfo.ItemsPreparationInfo);
                        }

                    if (itemsPreparationInfo.@object is IMenuItem)
                        if (((itemsPreparationInfo.@object as MenuItem).Class == itemsCategory ||
                            IsDescendantOfCategory(itemsCategory, (itemsPreparationInfo.@object as MenuItem).Class as IItemsCategory)))
                        {
                            itemsPreparationInfos.Add(itemsPreparationInfo.ItemsPreparationInfo);
                        }
                }

            }
            return itemsPreparationInfos;
        }


        private bool IsDescendantOfCategory(IItemsCategory itemsCategory, IItemsCategory descendantItemsCategory)
        {
            //if (itemsCategory != null && itemsCategory == descendantItemsCategory)
            //    return true;

            descendantItemsCategory = descendantItemsCategory.Class as ItemsCategory;
            while (descendantItemsCategory != null && descendantItemsCategory != itemsCategory)
            {
                descendantItemsCategory = descendantItemsCategory.Class as ItemsCategory;
                if (descendantItemsCategory == itemsCategory)
                    return true;
            }
            return false;
        }



        public void IncludeItem(MenuModel.IMenuItem menuItem)
        {

            var itemsPreparationInfos = (from itemsInfo in ItemsPreparationInfos
                                         select new
                                         {
                                             @object = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfo.ItemsInfoObjectUri),
                                             ItemsPreparationInfo = itemsInfo
                                         }).ToList();

            var excludedItemsPreparationInfo = (from itemsInfoEntry in itemsPreparationInfos
                                                where itemsInfoEntry.@object == menuItem && itemsInfoEntry.ItemsPreparationInfo.Excluded()
                                                select itemsInfoEntry.ItemsPreparationInfo).FirstOrDefault();

            if (excludedItemsPreparationInfo != null)
            {
                if (excludedItemsPreparationInfo.PreparationTime())
                {
                    excludedItemsPreparationInfo.ItemsPreparationInfoType = ItemsPreparationInfoType.PreparationTime;
                    if(!StationPrepareItem(menuItem))
                        excludedItemsPreparationInfo.ItemsPreparationInfoType = ItemsPreparationInfoType.Include | ItemsPreparationInfoType.PreparationTime;
                }
                else
                {
                    PreparationStation.RemovePreparationInfo(excludedItemsPreparationInfo);
                    ItemsPreparationInfos.Remove(excludedItemsPreparationInfo);
                }
            }


            if (!StationPrepareItem(menuItem))
            {
                string uri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(menuItem).GetPersistentObjectUri(menuItem);

                var itemsPreparationInfo = this.PreparationStation.NewPreparationInfo(uri, ItemsPreparationInfoType.Include);
                this.ItemsPreparationInfos.Add(itemsPreparationInfo);
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));

                foreach (var itemsPreparationInfoPresentation in ItemsToChoose.OfType<ItemsPreparationInfoPresentation>())
                    itemsPreparationInfoPresentation.Refresh();
            }

            if (PreparationStationItems != null)
                PreparationStationItems.Refresh();
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));


    
        }

        public void ExcludeItems(MenuModel.IItemsCategory itemsCategory)
        {


            if (StationPrepareItems(itemsCategory))
            {


                var itemsPreparationInfos = (from itemsInfo in ItemsPreparationInfos
                                             select new
                                             {
                                                 @object = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfo.ItemsInfoObjectUri),
                                                 ItemsPreparationInfo = itemsInfo
                                             }).ToList();

                var includedItemsPreparationInfo = (from itemsInfoEntry in itemsPreparationInfos
                                                    where itemsInfoEntry.@object == itemsCategory && itemsInfoEntry.ItemsPreparationInfo.Included()
                                                    select itemsInfoEntry.ItemsPreparationInfo).FirstOrDefault();

                if (includedItemsPreparationInfo != null)
                {
                    this.PreparationStation.RemovePreparationInfo(includedItemsPreparationInfo);
                    ItemsPreparationInfos.Remove(includedItemsPreparationInfo);
                }
                else
                {

                    string uri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(itemsCategory).GetPersistentObjectUri(itemsCategory);
                    var itemsPreparationInfo = this.PreparationStation.NewPreparationInfo(uri, ItemsPreparationInfoType.Exclude);

                    ItemsPreparationInfos.Add(itemsPreparationInfo);
                    //var sdd = itemsPreparationInfo.Exclude;
                }

                foreach (var itemsPreparationInfoPresentation in ItemsToChoose.OfType<ItemsPreparationInfoPresentation>())
                    itemsPreparationInfoPresentation.Refresh();

            }
            else
            {
                List<IItemsPreparationInfo> uselessDescendantItemsPreparationInfos = GetUselessDescendantItemsPreparationInfos(itemsCategory);
                if (uselessDescendantItemsPreparationInfos.Count > 0)
                {
                    // the item preparation infos which refer to items or category which contained in included category and are useless must be removed
                    PreparationStation.RemovePreparationInfos(uselessDescendantItemsPreparationInfos);
                    ItemsPreparationInfos = PreparationStation.ItemsPreparationInfos.ToList();
                }
                foreach (var itemsPreparationInfoPresentation in ItemsToChoose.OfType<ItemsPreparationInfoPresentation>())
                    itemsPreparationInfoPresentation.Refresh();


            }

            if (PreparationStationItems != null)
                PreparationStationItems.Refresh();
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
            //foreach (var member in _Members.OfType<ItemsPreparationInfoPresentation>())
            //    member.Refresh();
        }
        public void ExcludeItem(MenuModel.IMenuItem menuItem)
        {

            if (StationPrepareItem(menuItem))
            {


                var itemsPreparationInfos = (from itemsInfo in ItemsPreparationInfos
                                             select new
                                             {
                                                 @object = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfo.ItemsInfoObjectUri),
                                                 ItemsPreparationInfo = itemsInfo
                                             }).ToList();

                var includedItemsPreparationInfo = (from itemsInfoEntry in itemsPreparationInfos
                                                    where itemsInfoEntry.@object == menuItem 
                                                    select itemsInfoEntry.ItemsPreparationInfo).FirstOrDefault();

                if (includedItemsPreparationInfo != null)
                {
                    this.PreparationStation.RemovePreparationInfo(includedItemsPreparationInfo);
                    ItemsPreparationInfos.Remove(includedItemsPreparationInfo);
                }
                if(StationPrepareItem(menuItem))
                {

                    string uri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(menuItem).GetPersistentObjectUri(menuItem);
                    var itemsPreparationInfo = this.PreparationStation.NewPreparationInfo(uri, ItemsPreparationInfoType.Exclude);
                    this.ItemsPreparationInfos.Add(itemsPreparationInfo);
                }

                if (PreparationStationItems != null)
                    PreparationStationItems.Refresh();
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
            }
        }

        public bool StationPrepareItems(MenuModel.IItemsCategory itemsCategory)
        {


            var itemsPreparationInfos = (from itemsInfo in ItemsPreparationInfos
                                         select new
                                         {
                                             ItemsInfoObjectUri = itemsInfo.ItemsInfoObjectUri,
                                             @object = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfo.ItemsInfoObjectUri),
                                             ItemsPreparationInfo = itemsInfo
                                         }).ToList();

            foreach (var itemsPreparationInfoEntry in itemsPreparationInfos)
            {
                if (itemsPreparationInfoEntry.@object is MenuModel.IItemsCategory && (itemsPreparationInfoEntry.@object as MenuModel.IItemsCategory) == itemsCategory)
                {
                    if (itemsPreparationInfoEntry.ItemsPreparationInfo.Included())
                        return true;
                    else
                        return false;

                }
            }



            foreach (var itemsPreparationInfo in ItemsPreparationInfos)
            {
                MenuModel.IItemsCategory itemsCategoryOrParent = itemsCategory;
                var @object = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsPreparationInfo.ItemsInfoObjectUri);
                if (@object is MenuModel.IItemsCategory)
                {
                    var itemsPreparationInfoCategory = (@object as MenuModel.IItemsCategory);
                    while (itemsCategoryOrParent != null && itemsCategoryOrParent != itemsPreparationInfoCategory)
                        itemsCategoryOrParent = itemsCategoryOrParent.Class as MenuModel.IItemsCategory;

                    if (itemsCategoryOrParent == itemsPreparationInfoCategory)
                        return true;

                }
            }
            return false;

        }

        public bool StationPrepareItem(MenuModel.IMenuItem menuItem)
        {

            var itemsPreparationInfos = (from itemsInfo in ItemsPreparationInfos
                                         select new
                                         {
                                             @object = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfo.ItemsInfoObjectUri),
                                             ItemsPreparationInfo = itemsInfo
                                         }).ToList();

            foreach (var itemsPreparationInfoEntry in itemsPreparationInfos)
            {
                if (itemsPreparationInfoEntry.@object is MenuModel.IMenuItem && (itemsPreparationInfoEntry.@object as MenuModel.IMenuItem) == menuItem)
                {
                    if (itemsPreparationInfoEntry.ItemsPreparationInfo.Included())
                        return true;
                    if (itemsPreparationInfoEntry.ItemsPreparationInfo.Excluded())
                        return false;

                }
            }

            foreach (var itemsPreparationInfoEntry in itemsPreparationInfos)
            {
                if (itemsPreparationInfoEntry.@object is MenuModel.IItemsCategory)
                {
                    MenuModel.IItemsCategory itemsCategory = null;
                    var itemsPreparationInfoCategory = (itemsPreparationInfoEntry.@object as MenuModel.IItemsCategory);
                    if (menuItem is MenuModel.IClassified)
                    {
                        itemsCategory = (menuItem as MenuModel.IClassified).Class as MenuModel.ItemsCategory;

                        if (StationPrepareItems(itemsCategory))
                            return true;
                        else
                            return false;

                        //while (itemsCategory != null && itemsCategory != itemsPreparationInfoCategory)
                        //    itemsCategory = itemsCategory.Class as MenuModel.IItemsCategory;

                        //if (itemsCategory == itemsPreparationInfoCategory)
                        //    return true;
                    }
                }
            }
            return false;

        }



        public bool StationPrepareForServicePoint(MenuModel.IMenuItem menuItem)
        {

            var itemsPreparationInfos = (from itemsInfo in ItemsPreparationInfos
                                         select new
                                         {
                                             @object = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfo.ItemsInfoObjectUri),
                                             ItemsPreparationInfo = itemsInfo
                                         }).ToList();

            foreach (var itemsPreparationInfoEntry in itemsPreparationInfos)
            {
                if (itemsPreparationInfoEntry.@object is MenuModel.IMenuItem && (itemsPreparationInfoEntry.@object as MenuModel.IMenuItem) == menuItem)
                {
                    if (itemsPreparationInfoEntry.ItemsPreparationInfo.Included())
                        return true;
                    if (itemsPreparationInfoEntry.ItemsPreparationInfo.Excluded())
                        return false;

                }
            }

            foreach (var itemsPreparationInfoEntry in itemsPreparationInfos)
            {
                if (itemsPreparationInfoEntry.@object is MenuModel.IItemsCategory)
                {
                    MenuModel.IItemsCategory itemsCategory = null;
                    var itemsPreparationInfoCategory = (itemsPreparationInfoEntry.@object as MenuModel.IItemsCategory);
                    if (menuItem is MenuModel.IClassified)
                    {
                        itemsCategory = (menuItem as MenuModel.IClassified).Class as MenuModel.ItemsCategory;

                        if (StationPrepareItems(itemsCategory))
                            return true;
                        else
                            return false;

                        //while (itemsCategory != null && itemsCategory != itemsPreparationInfoCategory)
                        //    itemsCategory = itemsCategory.Class as MenuModel.IItemsCategory;

                        //if (itemsCategory == itemsPreparationInfoCategory)
                        //    return true;
                    }
                }
            }
            return false;

        }

        public bool StationPrepareForServicePoints(IServiceArea serviceArea)
        {


            var itemsPreparationInfos = (from itemsInfo in ItemsPreparationInfos
                                         select new
                                         {
                                             ItemsInfoObjectUri = itemsInfo.ItemsInfoObjectUri,
                                             @object = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfo.ItemsInfoObjectUri),
                                             ItemsPreparationInfo = itemsInfo
                                         }).ToList();

            foreach (var itemsPreparationInfoEntry in itemsPreparationInfos)
            {
                if (itemsPreparationInfoEntry.@object is MenuModel.IItemsCategory && (itemsPreparationInfoEntry.@object as MenuModel.IItemsCategory) == itemsCategory)
                {
                    if (itemsPreparationInfoEntry.ItemsPreparationInfo.Included())
                        return true;
                    else
                        return false;

                }
            }



            foreach (var itemsPreparationInfo in ItemsPreparationInfos)
            {
                MenuModel.IItemsCategory itemsCategoryOrParent = itemsCategory;
                var @object = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsPreparationInfo.ItemsInfoObjectUri);
                if (@object is MenuModel.IItemsCategory)
                {
                    var itemsPreparationInfoCategory = (@object as MenuModel.IItemsCategory);
                    while (itemsCategoryOrParent != null && itemsCategoryOrParent != itemsPreparationInfoCategory)
                        itemsCategoryOrParent = itemsCategoryOrParent.Class as MenuModel.IItemsCategory;

                    if (itemsCategoryOrParent == itemsPreparationInfoCategory)
                        return true;

                }
            }
            return false;

        }


        public readonly MenuViewModel MenuViewModel;
        public readonly IPreparationStation PreparationStation;
        PreparationSationsTreeNode PreparationSations;
        public PreparationStationPresentation(PreparationSationsTreeNode parent, IPreparationStation preparationStation, MenuViewModel menuViewModel) : base(parent)
        {
            MenuViewModel = menuViewModel;
            PreparationSations = parent;
            PreparationStation = preparationStation;
            ItemsPreparationInfos = preparationStation.ItemsPreparationInfos.ToList();
            


            RenameCommand = new RelayCommand((object sender) =>
            {
                EditMode();
            });
            DeleteCommand = new RelayCommand((object sender) =>
            {
                Delete();
            });

            EditCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                System.Windows.Window win = System.Windows.Window.GetWindow(EditCommand.UserInterfaceObjectConnection.ContainerControl as System.Windows.DependencyObject);
                EditMenuItem(win);
            });
        }

        public PreparationStationPresentation() : base(null)
        {

        }

        List<IItemsPreparationInfo> ItemsPreparationInfos;

        List<IPreparationForInfo> ServicePointsPreparationInfos;


        MenuModel.IMenu Menu;
        public PreparationStationPresentation(IPreparationStation preparationStation, MenuModel.IMenu menu, MenuViewModel menuViewModel) : base(null)
        {
            MenuViewModel = menuViewModel;
            PreparationStation = preparationStation;
            Menu = menu;
            ItemsPreparationInfos = preparationStation.ItemsPreparationInfos.ToList();
            CheckBoxVisibility = Visibility.Collapsed;
            this.IsNodeExpanded = true;
        }



        public System.Windows.Visibility CheckBoxVisibility
        {
            get; set;
        }


        private void EditMenuItem(System.Windows.Window owner)
        {
            //using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.RequiredNested))
            //{
            //    var SelectPreparationStationItemsWindow = new Views.Preparation.SelectPreparationStationItemsWindow();

            //    SelectPreparationStationItemsWindow.Owner = owner;

            //    SelectPreparationStationItemsWindow.GetObjectContext().RollbackOnExitWithoutAnswer = false;
            //    SelectPreparationStationItemsWindow.GetObjectContext().RollbackOnNegativeAnswer = false;

            //    SelectPreparationStationItemsWindow.GetObjectContext().SetContextInstance(this);

            //    SelectPreparationStationItemsWindow.ShowDialog();
            //    stateTransition.Consistent = true;
            //}


            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
            {

                //System.Windows.Window owner = System.Windows.Window.GetWindow(EditOptionsTypesCommand.UserInterfaceObjectConnection.ContainerControl as System.Windows.DependencyObject);
                //var menuItemWindow = new MenuItemsEditor.Views.CategoryMenuItemTypesWindow();
                //menuItemWindow.Owner = owner;
                //MenuItemsEditor.ViewModel.OptionsTypesViewModel optionsTypesViewModel = new MenuItemsEditor.ViewModel.OptionsTypesViewModel(ItemsCategory);
                //menuItemWindow.GetObjectContext().SetContextInstance(optionsTypesViewModel);
                //menuItemWindow.ShowDialog();

                var frame = PageDialogFrame.LoadedPageDialogFrames.FirstOrDefault();// WPFUIElementObjectBind.ObjectContext.FindChilds<PageDialogFrame>(win).Where(x => x.Name == "PageDialogHost").FirstOrDefault();
                Views.Preparation.PreparationStationItemsPage preparationStationItemsPage = new Views.Preparation.PreparationStationItemsPage();
                preparationStationItemsPage.GetObjectContext().SetContextInstance(this);
                
                frame.ShowDialogPage(preparationStationItemsPage);
                stateTransition.Consistent = true;
            }


            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Name)));
        }

        List<FBResourceTreeNode> _ItemsToChoose;
        public List<FBResourceTreeNode> ItemsToChoose
        {
            get
            {
                if (_ItemsToChoose == null)
                {
                    var rootCategory = new ItemsPreparationInfoPresentation(this, new FlavourBusinessManager.ServicesContextResources.ItemsPreparationInfo(MenuViewModel.Menu.RootCategory), true);
                    rootCategory.CheckBoxVisibility = Visibility.Collapsed;
                    rootCategory.IsNodeExpanded = true;
                    _ItemsToChoose = new List<FBResourceTreeNode>() { rootCategory };
                }
                return _ItemsToChoose;
            }
        }
        private void Delete()
        {
            PreparationSations.RemovePreparationStation(this);
        }

        public void EditMode()
        {
            if (_Edit == true)
            {
                _Edit = !_Edit;
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Edit)));
            }
            _Edit = true;
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Edit)));
        }



        public RelayCommand RenameCommand { get; protected set; }
        public RelayCommand DeleteCommand { get; protected set; }
        public RelayCommand EditCommand { get; protected set; }



        public override string Name
        {
            get
            {
                return PreparationStation.Description;
            }
            set
            {
                PreparationStation.Description = value;
            }
        }

        public string PreparationStationIdentity
        {
            get
            {
                return PreparationStation.PreparationStationIdentity;
            }
        }


        public override bool HasContextMenu
        {
            get
            {
                return true;
            }
        }

        public override bool IsEditable
        {
            get
            {
                return true;
            }
        }

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



                    _ContextMenuItems.Add(null);

                    menuItem = new MenuCommand();
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/delete.png"));
                    menuItem.Header = Properties.Resources.RemoveCallerIDServer;
                    menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = DeleteCommand;

                    _ContextMenuItems.Add(menuItem);

                    menuItem = new MenuCommand();
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Edit16.png"));
                    menuItem.Header = MenuItemsEditor.Properties.Resources.EditObject;
                    menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = EditCommand;

                    _ContextMenuItems.Add(menuItem);



                }
                //if (_ContextMenuItems == null)
                //{
                //    _ContextMenuItems = new List<MenuComamnd>();
                //}
                return _ContextMenuItems;
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


        public override ImageSource TreeImage
        {
            get
            {
                return new BitmapImage(new Uri(@"pack://application:,,,/FLBManager;Component/Resources/Images/Metro/chef16.png"));
            }
        }

        public override void SelectionChange()
        {

        }

        ItemsPreparationInfoPresentation PreparationStationItems;

        //List<FBResourceTreeNode> _Members;// = new List<FBResourceTreeNode>();
        public override List<FBResourceTreeNode> Members
        {
            get
            {

                if (PreparationStationItems == null)
                {
                    PreparationStationItems = new ItemsPreparationInfoPresentation(this, new FlavourBusinessManager.ServicesContextResources.ItemsPreparationInfo(MenuViewModel.Menu.RootCategory), false);
                    PreparationStationItems.IsNodeExpanded = true;
                    PreparationStationItems.CheckBoxVisibility = Visibility.Collapsed;
                    
                    
                }

                return PreparationStationItems.Members;
            }
        }

        bool _PreparationTimeVisible;
        public bool PreparationTimeIsVisible
        {
            get => _PreparationTimeVisible;
            set => _PreparationTimeVisible = value;
        }

        DateTime DragEnterStartTime;
        public void DragEnter(object sender, DragEventArgs e)
        {
            DragEnterStartTime = DateTime.Now;

            DragItemsCategory dragItemsCategory = e.Data.GetData(typeof(DragItemsCategory)) as DragItemsCategory;
            if (dragItemsCategory != null)
            {
                e.Effects = DragDropEffects.Copy;
                DragEnterStartTime = DateTime.Now;
            }
            else
                e.Effects = DragDropEffects.None;


        }

        public void DragLeave(object sender, DragEventArgs e)
        {

            if (SelectedOnDragOver)
            {
                IsSelected = false;
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(IsSelected)));

            }
            e.Effects = DragDropEffects.None;
        }

        bool SelectedOnDragOver = false;
        public void DragOver(object sender, DragEventArgs e)
        {

            DragItemsCategory dragItemsCategory = e.Data.GetData(typeof(DragItemsCategory)) as DragItemsCategory;
            if (dragItemsCategory != null)
            {
                if (!IsSelected)
                    SelectedOnDragOver = true;
                IsSelected = true;
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(IsSelected)));
                if ((DateTime.Now - DragEnterStartTime).TotalSeconds > 2)
                {
                    IsNodeExpanded = true;
                    RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(IsNodeExpanded)));
                }
            }
            else
                e.Effects = DragDropEffects.None;

            System.Diagnostics.Debug.WriteLine("DragOver InfrastructureTreeNode");
        }

        public void Drop(object sender, DragEventArgs e)
        {
            DragItemsCategory dragItemsCategory = e.Data.GetData(typeof(DragItemsCategory)) as DragItemsCategory;
            if (dragItemsCategory != null)
            {

                IncludeItems(dragItemsCategory.ItemsCategory);
                IsNodeExpanded = true;
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(IsNodeExpanded)));

                //string uri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(dragItemsCategory.ItemsCategory).GetPersistentObjectUri(dragItemsCategory.ItemsCategory);
                //var itemsPreparationInfo = this.PreparationStation.NewPreparationInfo(uri);
                //_Members.Add(new ItemsPreparationInfoPresentation(this, itemsPreparationInfo));
                //RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
                //IsNodeExpanded = true;
                //RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(IsNodeExpanded)));
            }
            //DragItemsCategory

        }

        //public List<FBResourceTreeNode> PreparationItemsInfoSelections
        //{
        //    get
        //    {
        //        return _Members.ToList();
        //    }
        //}

    }

    //Extension

    static class ItemsPreparationInfoTypeExtension
    {
        public static bool Included(this IItemsPreparationInfo itemsPreparationInfo)
        {
            return (itemsPreparationInfo.ItemsPreparationInfoType & ItemsPreparationInfoType.Include) == ItemsPreparationInfoType.Include;
        }

        public static bool Excluded(this IItemsPreparationInfo itemsPreparationInfo)
        {
            return (itemsPreparationInfo.ItemsPreparationInfoType & ItemsPreparationInfoType.Exclude) == ItemsPreparationInfoType.Exclude;
        }

        public static bool PreparationTime(this IItemsPreparationInfo itemsPreparationInfo)
        {
            return (itemsPreparationInfo.ItemsPreparationInfoType & ItemsPreparationInfoType.PreparationTime) == ItemsPreparationInfoType.PreparationTime;
        }


    }

    //class mms
    //{
    //    public void ExcludeServicePoints(IServiceArea serviceArea)
    //    {


    //        if (StationPrepareItems(itemsCategory))
    //        {


    //            var itemsPreparationInfos = (from itemsInfo in ItemsPreparationInfos
    //                                         select new
    //                                         {
    //                                             @object = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfo.ItemsInfoObjectUri),
    //                                             ItemsPreparationInfo = itemsInfo
    //                                         }).ToList();

    //            var includedItemsPreparationInfo = (from itemsInfoEntry in itemsPreparationInfos
    //                                                where itemsInfoEntry.@object == itemsCategory && itemsInfoEntry.ItemsPreparationInfo.Included()
    //                                                select itemsInfoEntry.ItemsPreparationInfo).FirstOrDefault();

    //            if (includedItemsPreparationInfo != null)
    //            {
    //                this.PreparationStation.RemovePreparationInfo(includedItemsPreparationInfo);
    //                ItemsPreparationInfos.Remove(includedItemsPreparationInfo);
    //            }
    //            else
    //            {

    //                string uri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(itemsCategory).GetPersistentObjectUri(itemsCategory);
    //                var itemsPreparationInfo = this.PreparationStation.NewPreparationInfo(uri, ItemsPreparationInfoType.Exclude);

    //                ItemsPreparationInfos.Add(itemsPreparationInfo);
    //                //var sdd = itemsPreparationInfo.Exclude;
    //            }

    //            foreach (var itemsPreparationInfoPresentation in ItemsToChoose.OfType<ItemsPreparationInfoPresentation>())
    //                itemsPreparationInfoPresentation.Refresh();

    //        }
    //        else
    //        {
    //            List<IItemsPreparationInfo> uselessDescendantItemsPreparationInfos = GetUselessDescendantItemsPreparationInfos(itemsCategory);
    //            if (uselessDescendantItemsPreparationInfos.Count > 0)
    //            {
    //                // the item preparation infos which refer to items or category which contained in included category and are useless must be removed
    //                PreparationStation.RemovePreparationInfos(uselessDescendantItemsPreparationInfos);
    //                ItemsPreparationInfos = PreparationStation.ItemsPreparationInfos.ToList();
    //            }
    //            foreach (var itemsPreparationInfoPresentation in ItemsToChoose.OfType<ItemsPreparationInfoPresentation>())
    //                itemsPreparationInfoPresentation.Refresh();


    //        }

    //        if (PreparationStationItems != null)
    //            PreparationStationItems.Refresh();
    //        RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
    //        //foreach (var member in _Members.OfType<ItemsPreparationInfoPresentation>())
    //        //    member.Refresh();
    //    }
    //    public void ExcludeServicePoint(IServicePoint servicePoint)
    //    {

    //        if (StationPrepareItem(menuItem))
    //        {


    //            var itemsPreparationInfos = (from itemsInfo in ItemsPreparationInfos
    //                                         select new
    //                                         {
    //                                             @object = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfo.ItemsInfoObjectUri),
    //                                             ItemsPreparationInfo = itemsInfo
    //                                         }).ToList();

    //            var includedItemsPreparationInfo = (from itemsInfoEntry in itemsPreparationInfos
    //                                                where itemsInfoEntry.@object == menuItem
    //                                                select itemsInfoEntry.ItemsPreparationInfo).FirstOrDefault();

    //            if (includedItemsPreparationInfo != null)
    //            {
    //                this.PreparationStation.RemovePreparationInfo(includedItemsPreparationInfo);
    //                ItemsPreparationInfos.Remove(includedItemsPreparationInfo);
    //            }
    //            if (StationPrepareItem(menuItem))
    //            {

    //                string uri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(menuItem).GetPersistentObjectUri(menuItem);
    //                var itemsPreparationInfo = this.PreparationStation.NewPreparationInfo(uri, ItemsPreparationInfoType.Exclude);
    //                this.ItemsPreparationInfos.Add(itemsPreparationInfo);
    //            }

    //            if (PreparationStationItems != null)
    //                PreparationStationItems.Refresh();
    //            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
    //        }
    //    }

    //    public bool StationPrepareForServicePonts(IServiceArea serviceArea)
    //    {


    //        var itemsPreparationInfos = (from itemsInfo in ItemsPreparationInfos
    //                                     select new
    //                                     {
    //                                         ItemsInfoObjectUri = itemsInfo.ItemsInfoObjectUri,
    //                                         @object = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfo.ItemsInfoObjectUri),
    //                                         ItemsPreparationInfo = itemsInfo
    //                                     }).ToList();

    //        foreach (var itemsPreparationInfoEntry in itemsPreparationInfos)
    //        {
    //            if (itemsPreparationInfoEntry.@object is MenuModel.IItemsCategory && (itemsPreparationInfoEntry.@object as MenuModel.IItemsCategory) == itemsCategory)
    //            {
    //                if (itemsPreparationInfoEntry.ItemsPreparationInfo.Included())
    //                    return true;
    //                else
    //                    return false;

    //            }
    //        }



    //        foreach (var itemsPreparationInfo in ItemsPreparationInfos)
    //        {
    //            MenuModel.IItemsCategory itemsCategoryOrParent = itemsCategory;
    //            var @object = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsPreparationInfo.ItemsInfoObjectUri);
    //            if (@object is MenuModel.IItemsCategory)
    //            {
    //                var itemsPreparationInfoCategory = (@object as MenuModel.IItemsCategory);
    //                while (itemsCategoryOrParent != null && itemsCategoryOrParent != itemsPreparationInfoCategory)
    //                    itemsCategoryOrParent = itemsCategoryOrParent.Class as MenuModel.IItemsCategory;

    //                if (itemsCategoryOrParent == itemsPreparationInfoCategory)
    //                    return true;

    //            }
    //        }
    //        return false;

    //    }

    //    public  bool StationPrepareForServicePont(IServicePoint  servicePoint)

    //    {

    //        var itemsPreparationInfos = (from itemsInfo in ItemsPreparationInfos
    //                                     select new
    //                                     {
    //                                         @object = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfo.ItemsInfoObjectUri),
    //                                         ItemsPreparationInfo = itemsInfo
    //                                     }).ToList();

    //        foreach (var itemsPreparationInfoEntry in itemsPreparationInfos)
    //        {
    //            if (itemsPreparationInfoEntry.@object is MenuModel.IMenuItem && (itemsPreparationInfoEntry.@object as MenuModel.IMenuItem) == menuItem)
    //            {
    //                if (itemsPreparationInfoEntry.ItemsPreparationInfo.Included())
    //                    return true;
    //                if (itemsPreparationInfoEntry.ItemsPreparationInfo.Excluded())
    //                    return false;

    //            }
    //        }

    //        foreach (var itemsPreparationInfoEntry in itemsPreparationInfos)
    //        {
    //            if (itemsPreparationInfoEntry.@object is MenuModel.IItemsCategory)
    //            {
    //                MenuModel.IItemsCategory itemsCategory = null;
    //                var itemsPreparationInfoCategory = (itemsPreparationInfoEntry.@object as MenuModel.IItemsCategory);
    //                if (menuItem is MenuModel.IClassified)
    //                {
    //                    itemsCategory = (menuItem as MenuModel.IClassified).Class as MenuModel.ItemsCategory;

    //                    if (StationPrepareItems(itemsCategory))
    //                        return true;
    //                    else
    //                        return false;

    //                    //while (itemsCategory != null && itemsCategory != itemsPreparationInfoCategory)
    //                    //    itemsCategory = itemsCategory.Class as MenuModel.IItemsCategory;

    //                    //if (itemsCategory == itemsPreparationInfoCategory)
    //                    //    return true;
    //                }
    //            }
    //        }
    //        return false;

    //    }

    //}
}