using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media.Imaging;

using OOAdvantech.PersistenceLayer;
using OOAdvantech.Transactions;
using WPFUIElementObjectBind;
using MenuPresentationModel.MenuCanvas;
using OOAdvantech;

namespace MenuDesigner.ViewModel.MenuCanvas
{
    /// <MetaDataID>{9714df9f-8db5-4d27-8ed2-a3a0952a786a}</MetaDataID>
    public class MenuHeadingsPresentation : MarshalByRefObject, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;


        MenuPresentationModel.RestaurantMenu RestaurantMenu;

        public MenuHeadingsPresentation()
        {

        }
        public MenuHeadingsPresentation(MenuPresentationModel.RestaurantMenu restaurantMenu)
        {


            RestaurantMenu = restaurantMenu;
            var objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(RestaurantMenu);

            if (objectStorage != null)
            {
                OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(objectStorage);

                var headings = (from heading in storage.GetObjectCollection<MenuPresentationModel.MenuCanvas.IMenuCanvasHeading>()
                                select heading).ToList();

                foreach (var heading in headings)
                {
                    if (!RestaurantMenu.AvailableHeadings.Contains(heading))
                        RestaurantMenu.AddAvailableHeading(heading as MenuPresentationModel.MenuCanvas.FoodItemsHeading);
                }


                _Headings = (from heading in RestaurantMenu.AvailableHeadings
                             select new ListMenuHeadingPresentation(restaurantMenu, heading)).ToList();

            }
            else
                _Headings = new List<ListMenuHeadingPresentation>();
            //(from page in RestaurantMenu.Pages
            // from heading in page.MenuCanvasItems.OfType<MenuPresentationModel.MenuCanvas.IMenuCanvasHeading>()
            // select new MenuHeadingPresentation(restaurantMenu, heading)).ToList();

            DeleteMenuCommand = new RelayCommand((object sender) =>
            {
                if (_SelectedHeading.Heading != null && _SelectedHeading.Heading.Page == null)
                {

                    OOAdvantech.PersistenceLayer.ObjectStorage.DeleteObject(_SelectedHeading.Heading, DeleteOptions.TryToDelete);
                    RestaurantMenu.RemoveAvailableHeading(_SelectedHeading.Heading as MenuPresentationModel.MenuCanvas.FoodItemsHeading);
                    _Headings.Remove(_SelectedHeading);
                    _SelectedHeading.Remove();
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Headings)));
                }
                //Delete();
            }, (object sender) => CanDelete(sender));

            EditHeadingCommand = new RelayCommand((object sender) =>
            {
                System.Windows.Window win = System.Windows.Window.GetWindow(EditHeadingCommand.UserInterfaceObjectConnection.ContainerControl as System.Windows.DependencyObject);
                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.RequiredNested))
                {
                    var menuItemWindow = new Views.HeadingWindow();
                    menuItemWindow.Owner = win;
                    MenuHeadingViewModel menuHeadingViewModel = new MenuHeadingViewModel(SelectedHeading.Heading, RestaurantMenu);
                    menuItemWindow.GetObjectContext().SetContextInstance(menuHeadingViewModel);
                    if (menuItemWindow.ShowDialog().Value)
                    {
                        stateTransition.Consistent = true;
                    }
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Headings)));

            }, (object sender) => CanEdit(sender));

            NewHeadingCommand = new RelayCommand((object sender) =>
            {
                MenuPresentationModel.MenuCanvas.FoodItemsHeading foodItemsHeading = new MenuPresentationModel.MenuCanvas.FoodItemsHeading();
                OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(RestaurantMenu).CommitTransientObjectState(foodItemsHeading);
                RestaurantMenu.AddAvailableHeading(foodItemsHeading);
                var headingPresentation = new ListMenuHeadingPresentation(RestaurantMenu, foodItemsHeading);
                headingPresentation.EditMode();
                _Headings.Insert(0, headingPresentation);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Headings)));
            });

            RenameCommand = new RelayCommand((object sender) =>
            {
                SelectedHeading.EditMode();
                //Delete();
            }, (object sender) => CanRename(sender));


            CopyHeadingCommand = new RelayCommand((object sender) =>
            {

                MenuHeadingsPresentation.HeadingDescripionCopies= Headings.Where(x=>x.IsSelected).Select(x=>x.Heading).OfType<FoodItemsHeading>().Select(x=>x.MultilingualDescription).ToList();
                //System.Windows.Window win = System.Windows.Window.GetWindow(EditHeadingCommand.UserInterfaceObjectConnection.ContainerControl as System.Windows.DependencyObject);
                //using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.RequiredNested))
                //{
                //    var menuItemWindow = new Views.HeadingWindow();
                //    menuItemWindow.Owner = win;
                //    MenuHeadingViewModel menuHeadingViewModel = new MenuHeadingViewModel(SelectedHeading.Heading, RestaurantMenu);
                //    menuItemWindow.GetObjectContext().SetContextInstance(menuHeadingViewModel);
                //    if (menuItemWindow.ShowDialog().Value)
                //    {
                //        stateTransition.Consistent = true;
                //    }
                //}
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Headings)));

            }, (object sender) => CanCopy());

            PasteHeadingCommand = new RelayCommand((object sender) =>
            {
                //System.Windows.Window win = System.Windows.Window.GetWindow(EditHeadingCommand.UserInterfaceObjectConnection.ContainerControl as System.Windows.DependencyObject);
                //using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.RequiredNested))
                //{
                //    var menuItemWindow = new Views.HeadingWindow();
                //    menuItemWindow.Owner = win;
                //    MenuHeadingViewModel menuHeadingViewModel = new MenuHeadingViewModel(SelectedHeading.Heading, RestaurantMenu);
                //    menuItemWindow.GetObjectContext().SetContextInstance(menuHeadingViewModel);
                //    if (menuItemWindow.ShowDialog().Value)
                //    {
                //        stateTransition.Consistent = true;
                //    }
                //}
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Headings)));

            }, (object sender) => MenuHeadingsPresentation.HeadingDescripionCopies!=null&&MenuHeadingsPresentation.HeadingDescripionCopies.Count>0);
        }

        private bool CanCopy()
        {
            return Headings!=null&&Headings.Where(x => x.IsSelected).Count()>0;
        }

        private bool CanEdit(object sender)
        {
            if (SelectedHeading != null)
                return true;
            else
                return false;
        }

        private bool CanRename(object sender)
        {
            if (_SelectedHeading != null)
                return true;
            else
                return false;
        }
        private bool CanDelete(object sender)
        {
            if (SelectedHeading != null && SelectedHeading.Heading.Page == null)
                return true;
            return false;
        }

        public WPFUIElementObjectBind.RelayCommand RenameCommand { get; protected set; }
        public WPFUIElementObjectBind.RelayCommand DeleteMenuCommand { get; protected set; }
        public WPFUIElementObjectBind.RelayCommand NewHeadingCommand { get; protected set; }
        public WPFUIElementObjectBind.RelayCommand CopyHeadingCommand { get; protected set; }
        public WPFUIElementObjectBind.RelayCommand PasteHeadingCommand { get; protected set; }

        public WPFUIElementObjectBind.RelayCommand EditHeadingCommand { get; protected set; }

        /// <exclude>Excluded</exclude>
        ListMenuHeadingPresentation _SelectedHeading;
        public ListMenuHeadingPresentation SelectedHeading
        {
            get
            {
                return _SelectedHeading;
            }
            set
            {
                //if (_SelectedHeading != null&& _SelectedHeading!=value)
                //    _SelectedHeading.IsEditing = false;

                _SelectedHeading = value;
            }
        }


        /// <exclude>Excluded</exclude>
        List<WPFUIElementObjectBind.MenuCommand> _ContextMenuItems;
        public List<WPFUIElementObjectBind.MenuCommand> ContextMenuItems
        {
            get
            {
                //return null;
                if (_ContextMenuItems == null)
                {

                    _ContextMenuItems = new List<MenuCommand>();


                    var imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuDesignerLib;Component/Resources/Images/Metro/Delete.png"));
                    MenuCommand menuItem = new MenuCommand();
                    menuItem.Header = Properties.Resources.DeleteMenuItemHeader;
                    menuItem.Icon = new Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = DeleteMenuCommand;
                    _ContextMenuItems.Add(menuItem);

                    menuItem = new MenuCommand();
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuDesignerLib;Component/Resources/Images/Metro/Rename16.png"));
                    menuItem.Header = Properties.Resources.RenameMenuIHeadingHeader;
                    menuItem.Icon = new Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = RenameCommand;

                    _ContextMenuItems.Add(menuItem);

                    menuItem = new WPFUIElementObjectBind.MenuCommand();
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuDesignerLib;Component/Resources/Images/Metro/Edit16.png"));
                    menuItem.Header = Properties.Resources.EditMenuItemHeader;
                    menuItem.Icon = new Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = EditHeadingCommand;

                    _ContextMenuItems.Add(menuItem);

                    _ContextMenuItems.Add(null);



                    menuItem = new WPFUIElementObjectBind.MenuCommand();
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuDesignerLib;Component/Resources/Images/Metro/DocumentHeaderAdd.png"));
                    menuItem.Header = Properties.Resources.NewMenuIHeadingHeader;
                    menuItem.Icon = new Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = NewHeadingCommand;
                    _ContextMenuItems.Add(menuItem);


                    menuItem = new WPFUIElementObjectBind.MenuCommand(); ;
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuDesignerLib;Component/Resources/Images/Metro/Copy.png"));
                    menuItem.Header = Properties.Resources.CopyMenuItemHeader;
                    menuItem.Icon = new Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = CopyHeadingCommand;
                    _ContextMenuItems.Add(menuItem);

                    menuItem = new WPFUIElementObjectBind.MenuCommand(); ;
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuDesignerLib;Component/Resources/Images/Metro/Paste.png"));
                    menuItem.Header = Properties.Resources.PasteMenuItemHeader;
                    menuItem.Icon = new Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = PasteHeadingCommand;
                    _ContextMenuItems.Add(menuItem);





                }
                return _ContextMenuItems;
            }
        }

      


        /// <exclude>Excluded</exclude>
        List<ListMenuHeadingPresentation> _Headings;
        public List<ListMenuHeadingPresentation> Headings
        {
            get
            {
                return _Headings;
            }
        }

        public static List<MultilingualMember<string>> HeadingDescripionCopies { get; private set; }
    }
}
