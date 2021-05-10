using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using MenuPresentationModel;
using MenuPresentationModel.MenuCanvas;
using MenuPresentationModel.MenuStyles;
using OOAdvantech.Transactions;

namespace MenuDesigner.ViewModel.MenuCanvas
{
    /// <MetaDataID>{a0b00e06-a3ba-4803-9098-95d6f4382670}</MetaDataID>
    public class FoodItemViewModel : MarshalByRefObject, INotifyPropertyChanged
    {
        private IMenuCanvasFoodItem MenuCanvasFoodItem;
        private RestaurantMenu GraphicMenu;


        public event PropertyChangedEventHandler PropertyChanged;

        public WPFUIElementObjectBind.RelayCommand MenuItemDetailsCommand { get; protected set; }


        public string PageTitle
        {
            get
            {
                return string.Format("Graphic menu item '{0}'", Title);
            }
            set { }
        }

        List<AccentViewModel> _AccentImages;
        public List<AccentViewModel> AccentImages
        {
            get
            {
                return _AccentImages;
            }
        }


        AccentViewModel _SelectedAccent;
        public AccentViewModel SelectedAccent
        {
            get
            {
                return _SelectedAccent;
            }
            set
            {
                _SelectedAccent = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedAccent)));
                (MenuCanvasFoodItem as MenuCanvasFoodItem).AccentType = value.Accent;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AccentColorize)));


                //if ((MenuCanvasHeading as FoodItemsHeading).CustomHeadingAccent != null)
                //{
                //    (MenuCanvasHeading as FoodItemsHeading).CustomHeadingAccent.AccentColor = new ColorConverter().ConvertToString(_HeadingsAccentSelectedColor);
                //}
                //else
                //{
                //    _HeadingsAccentSelectedColor = Colors.LightGray;
                //    _HeadingsAccentColorize = false;
                //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HeadingsAccentColorize)));
                //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HeadingsAccentSelectedColor)));

                //}

                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CustomAccent)));
            }
        }


        bool _AccentColorize;
        public bool AccentColorize
        {
            get
            {
                return _SelectedAccent.AccentType == AccentViewModel.AccentViewModelType.Image;
            }
            set
            {
                _AccentColorize = value;
                if (!_AccentColorize)
                {
                    _AccentSelectedColor = Colors.LightGray;
                    SelectedAccent.Accent.AccentColor = null;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AccentSelectedColor)));
                }
            }
        }

        /// <exclude>Excluded</exclude>
        Color _AccentSelectedColor = Colors.LightGray;
        public Color AccentSelectedColor
        {
            get
            {
                return _AccentSelectedColor;
            }
            set
            {
                _AccentSelectedColor = value;
                SelectedAccent.Accent.AccentColor = new ColorConverter().ConvertToString(_AccentSelectedColor);

                (this.MenuCanvasFoodItem as MenuCanvasFoodItem).AccentType.AccentColor = SelectedAccent.Accent.AccentColor;
            }
        }




        public FoodItemViewModel(IMenuCanvasFoodItem menuCanvasFoodItem, RestaurantMenu graphicMenu)
        {
            MenuCanvasFoodItem = menuCanvasFoodItem;
            GraphicMenu = graphicMenu;

         if(MenuCanvasFoodItem!=null)
                MenuCanvasFoodItem.ObjectChangeState += MenuCanvasFoodItem_ObjectChangeState;

            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(DownloadStylesWindow.HeadingAccentStorage);
            var accents = (from accent in storage.GetObjectCollection<Accent>() where accent.MultipleItemsAccent select accent).ToList();

            //var prorotypeAccent = (from accent in storage.GetObjectCollection<HeadingAccent>() where accent.SelectionAccentImageUri == @"AccentImages\boxes\Box.svg" select accent).FirstOrDefault();
            //var prorotypeImage = prorotypeAccent.AccentImages[0];

            //using (SystemStateTransition suppressStateTransition = new SystemStateTransition(TransactionOption.Suppress))
            //{


            //    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            //    {
            //    foreach(var boxAccent in     (from accent in storage.GetObjectCollection<HeadingAccent>()
            //         where accent.MultipleItemsAccent==true select accent))
            //        {
            //            if (boxAccent.MarginLeft == 0)
            //                boxAccent.MarginLeft = boxAccent.MarginRight = boxAccent.MarginTop;
            //        }

            //        //System.IO.DirectoryInfo dirInfo = new System.IO.DirectoryInfo(@"C:\ProgramData\Microneme\DontWaitWater\AccentImages\JoinBoxes\tmp");
            //        //foreach (var file in dirInfo.GetFiles())
            //        //{

            //        //    Resource image = new Resource();
            //        //    image.Uri = @"AccentImages\" + file.Name;
            //        //    MenuImage menuImage = new MenuImage(image, prorotypeImage.Width, prorotypeImage.Height);

            //        //    var accentName = file.Name.Replace(file.Extension, "").Replace("_", " ");
            //        //    HeadingAccent headingAccent = new HeadingAccent();
            //        //    headingAccent.SelectionAccentImageUri = @"AccentImages\boxes\" + file.Name;
            //        //    headingAccent.AccentColor = prorotypeAccent.AccentColor;
            //        //    headingAccent.DoubleImage = prorotypeAccent.DoubleImage;
            //        //    headingAccent.FullRowImage = prorotypeAccent.FullRowImage;
            //        //    headingAccent.Height = prorotypeAccent.Height;
            //        //    headingAccent.MarginBottom = prorotypeAccent.MarginBottom;
            //        //    headingAccent.MarginLeft = prorotypeAccent.MarginLeft;
            //        //    headingAccent.MarginRight = prorotypeAccent.MarginRight;
            //        //    headingAccent.MarginTop = prorotypeAccent.MarginTop;
            //        //    headingAccent.MultipleItemsAccent = true;

            //        //    headingAccent.Name = accentName;
            //        //    headingAccent.TextBackgroundImage = prorotypeAccent.TextBackgroundImage;
            //        //    headingAccent.UnderlineImage = prorotypeAccent.UnderlineImage;
            //        //    headingAccent.AddAccentImage(menuImage);

            //        //    DownloadStylesWindow.HeadingAccentStorage.CommitTransientObjectState(menuImage);
            //        //    DownloadStylesWindow.HeadingAccentStorage.CommitTransientObjectState(headingAccent);
            //        //}
            //        stateTransition.Consistent = true;
            //    }

            //}



            //"SelectionAccentImageUri"
            //AccentImage
            _AccentImages = (from accent in accents select new AccentViewModel(accent)).ToList();
            //_AccentImages.Insert(0, new AccentViewModel(AccentViewModel.AccentViewModelType.StyleSheet));
            _AccentImages.Insert(0, new AccentViewModel(AccentViewModel.AccentViewModelType.None));
            _SelectedAccent = _AccentImages[0];

            if ((MenuCanvasFoodItem as MenuCanvasFoodItem).AccentType != null)
                _SelectedAccent = (from accent in _AccentImages where accent.Accent != null && accent.Accent.Name == (MenuCanvasFoodItem as MenuCanvasFoodItem).AccentType.Name select accent).FirstOrDefault();



            MenuItemDetailsCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {

                var foodItemPage = MenuItemDetailsCommand.UserInterfaceObjectConnection.ContainerControl as StyleableWindow.PageDialogViewEmulator;
                if (foodItemPage == null)
                    foodItemPage = WPFUIElementObjectBind.ObjectContext.FindParent<StyleableWindow.PageDialogViewEmulator>(MenuItemDetailsCommand.UserInterfaceObjectConnection.ContainerControl as DependencyObject);

                Window owner = System.Windows.Window.GetWindow(MenuItemDetailsCommand.UserInterfaceObjectConnection.ContainerControl as System.Windows.DependencyObject);

                using (SystemStateTransition suppressStateTransition = new SystemStateTransition(TransactionOption.Suppress))
                {
                    if (foodItemPage != null)
                    {
                        var menuItemPage = new MenuItemsEditor.Views.MenuItemPage();
                        MenuItemsEditor.ViewModel.MenuItemViewModel menuItemViewModel = new MenuItemsEditor.ViewModel.MenuItemViewModel(MenuCanvasFoodItem.MenuItem);
                        menuItemPage.GetObjectContext().SetContextInstance(menuItemViewModel);
                        foodItemPage.NavigationWindow.Navigate(menuItemPage);
                    
                    }
                    else
                    {
                        using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                        {
                            var menuItemWindow = new MenuItemsEditor.Views.MenuItemWindow();
                            menuItemWindow.Owner = owner;
                            MenuItemsEditor.ViewModel.MenuItemViewModel menuItemViewModel = new MenuItemsEditor.ViewModel.MenuItemViewModel(MenuCanvasFoodItem.MenuItem);
                            menuItemWindow.GetObjectContext().SetContextInstance(menuItemViewModel);
                            if (menuItemWindow.ShowDialog().Value)
                                stateTransition.Consistent = true;
                        }
                    }
                }

                //if (SelectedMenuType != MenuItem.DedicatedType)
                //    MenuItem.RemoveType(SelectedMenuType);
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MenuItemTypes)));
            });

        }

        private void MenuCanvasFoodItem_ObjectChangeState(object _object, string member)
        {
            if (member == nameof(MenuCanvasFoodItem.Description))
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Title)));
            if (member == nameof(MenuCanvasFoodItem.FullDescription))
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FullTitle)));

            if (member == nameof(MenuCanvasFoodItem.Extras))
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Description)));


        }

        public string Title
        {
            get
            {
                return MenuCanvasFoodItem.Description;
            }
            set
            {
                if (MenuCanvasFoodItem.Description != value)
                {
                    MenuCanvasFoodItem.Description = value;
                    CommitTransientMenuCanvasFoodItem();
                }
            }
        }

        public string FullTitle
        {
            get
            {
                return MenuCanvasFoodItem.FullDescription;
            }
            set
            {
                if (MenuCanvasFoodItem.FullDescription != value)
                {
                    MenuCanvasFoodItem.FullDescription = value;
                    CommitTransientMenuCanvasFoodItem();
                }
            }
        }

        private void CommitTransientMenuCanvasFoodItem()
        {
            var objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(MenuCanvasFoodItem);
            //if(objectStorage==null)
            //    objectStorage.CommitTransientObjectState(MenuCanvasFoodItem);
        }

        public string Description
        {
            get
            {
                return MenuCanvasFoodItem.ExtraDescription;
            }
            set
            {
                if (MenuCanvasFoodItem.ExtraDescription != value)
                {
                    MenuCanvasFoodItem.ExtraDescription = value;
                    CommitTransientMenuCanvasFoodItem();
                }
            }
        }

        public bool Span
        {
            get
            {
                return MenuCanvasFoodItem.Span;
            }
            set
            {
                MenuCanvasFoodItem.Span = value;
                CommitTransientMenuCanvasFoodItem();
            }
        }

        public bool PriceInvisible
        {
            get
            {
                return MenuCanvasFoodItem.PriceInvisible;
            }
            set
            {
                MenuCanvasFoodItem.PriceInvisible = value;
                CommitTransientMenuCanvasFoodItem();
            }
        }

        public string Extras
        {
            get
            {
                return MenuCanvasFoodItem.Extras;
            }
            set
            {
                if (MenuCanvasFoodItem.Extras != value)
                {
                    MenuCanvasFoodItem.Extras = value;
                    CommitTransientMenuCanvasFoodItem();
                }
            }
        }


        public bool CustomSpacing
        {
            get
            {
                return MenuCanvasFoodItem.CustomSpacing;
            }
            set
            {
                MenuCanvasFoodItem.CustomSpacing = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BeforeSpacing)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AfterSpacing)));
            }
        }
        public bool StyleSpacing
        {
            get
            {
                return !MenuCanvasFoodItem.CustomSpacing;
            }
        }
        public string SpacingUnit
        {
            get
            {
                return "mm";
            }
        }
        public double BeforeSpacing
        {
            get
            {
                return Math.Round(LayoutOptionsPresentation.PixelToMM(MenuCanvasFoodItem.BeforeSpacing), 2);

            }
            set
            {
                MenuCanvasFoodItem.BeforeSpacing = Math.Round(LayoutOptionsPresentation.mmToPixel(value), 2);
            }
        }
        public double AfterSpacing
        {
            get
            {
                return Math.Round(LayoutOptionsPresentation.PixelToMM(MenuCanvasFoodItem.AfterSpacing), 2);
            }
            set
            {
                MenuCanvasFoodItem.AfterSpacing = Math.Round(LayoutOptionsPresentation.mmToPixel(value), 2);
            }
        }

    }
}
