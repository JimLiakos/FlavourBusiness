using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FlavourBusinessFacade.ServicesContextResources;
using OOAdvantech.Transactions;
using RestaurantHallLayoutModel;
using StyleableWindow;
using UIBaseEx;
using WPFUIElementObjectBind;

namespace FloorLayoutDesigner.ViewModel
{
    using RoutedCommand = WPFUIElementObjectBind.RoutedCommand;

    public delegate void HallLayoutSizeChangedHandle(HallLayout hallLayout, PaperSize newSize);
    /// <MetaDataID>{02fcfad8-6722-4e7b-ba63-033eeee228d9}</MetaDataID>
    [Transactional]
    public class HallLayoutViewModel : MarshalByRefObject, INotifyPropertyChanged, FlavourBusinessUI.ViewModel.IScaledArea
    {
        public event HallLayoutSizeChangedHandle HallLayoutSizeChanged;

        readonly HallLayout _HallLayout;
        public HallLayout HallLayout
        {
            get
            {
                var ss = FloorLayoutDesigner.Properties.Resources.HallLayoutLabelFontMenuItemHeader;
                return _HallLayout;
            }
        }

        AssignedMealTypeViewMode _DefaultMealType;
        public AssignedMealTypeViewMode DefaultMealType
        {
            get
            {
                if (_DefaultMealType == null)
                    _DefaultMealType = MealTypes.FirstOrDefault();
                return _DefaultMealType;

            }
            set
            {
                _DefaultMealType = value;
                if (_DefaultMealType != null)
                    _DefaultMealType.Assigned = true;
            }
        }


        bool _MealTypesViewExpanded;
        public bool MealTypesViewExpanded { 
            get=> _MealTypesViewExpanded; 
            set
            {
                _MealTypesViewExpanded = value;
                if(!value)
                {
                    if (_DefaultMealType != null)
                        _DefaultMealType.Assigned = true;
                }

            }
        }


        public RelayCommand MealTypeSelectCommand { get; set; }


        /// <exclude>Excluded</exclude>
        List<AssignedMealTypeViewMode> _MealTypes = new List<AssignedMealTypeViewMode>();
        public List<AssignedMealTypeViewMode> MealTypes
        {
            get
            {
                return ServiceAreaViewModel.GetMealTypes(null);
            }

        }

        public List<MenuCommand> MenuItems
        {
            get
            {
                return new List<MenuCommand>() { EditMenuItem, FontsMenu };
            }
        }

        /// <exclude>Excluded</exclude>
        MenuCommand _EditMenuItem;

        public MenuCommand EditMenuItem
        {
            get
            {

                if (_EditMenuItem == null)
                {
                    _EditMenuItem = new MenuCommand()
                    {
                        Header = Properties.Resources.EditMenuItemHeader,
                        SubMenuCommands = new List<MenuCommand>()
                        {
                            new MenuCommand()
                            {
                                Header=Properties.Resources.UndoMenuItemHeader,
                                Icon = new System.Windows.Controls.Image() { Source = new BitmapImage(new Uri(@"pack://application:,,,/RestaurantHallLayoutDesigner;Component/Resources/Images/Metro/undo16.png")), Width = 16, Height = 16 },
                                Command = UndoCommand
                            },
                        new MenuCommand()
                            {
                                Header=Properties.Resources.RedoMenuItemHeader,
                                Icon = new System.Windows.Controls.Image() { Source = new BitmapImage(new Uri(@"pack://application:,,,/RestaurantHallLayoutDesigner;Component/Resources/Images/Metro/redo16.png")), Width = 16, Height = 16 },
                                Command = RedoCommand
                            },
                             null,
                         new MenuCommand()
                            {
                                Header=Properties.Resources.CutMenuItemHeader,
                                Icon = new System.Windows.Controls.Image() { Source = new BitmapImage(new Uri(@"pack://application:,,,/RestaurantHallLayoutDesigner;Component/Resources/Images/Metro/Cut.png")), Width = 16, Height = 16 },
                                Command = CutCommand
                            },
                         new MenuCommand()
                            {
                                Header=Properties.Resources.CopyMenuItemHeader,
                                Icon = new System.Windows.Controls.Image() { Source = new BitmapImage(new Uri(@"pack://application:,,,/RestaurantHallLayoutDesigner;Component/Resources/Images/Metro/Copy.png")), Width = 16, Height = 16 },
                                Command = CopyCommand
                            },
                         new MenuCommand()
                            {
                                Header=Properties.Resources.PasteMenuItemHeader,
                                Icon = new System.Windows.Controls.Image() { Source = new BitmapImage(new Uri(@"pack://application:,,,/RestaurantHallLayoutDesigner;Component/Resources/Images/Metro/Paste.png")), Width = 16, Height = 16 },
                                Command = PasteCommand
                            },
                         null,
                         new MenuCommand()
                            {
                                Header=Properties.Resources.DeleteMenuItemHeader,
                                Icon = new System.Windows.Controls.Image() { Source = new BitmapImage(new Uri(@"pack://application:,,,/RestaurantHallLayoutDesigner;Component/Resources/Images/Metro/Delete.png")), Width = 16, Height = 16 },
                                Command = DeleteCommand
                            }
                         }
                    };
                }

                return _EditMenuItem;

            }
        }

        public bool EditPopupIsOpen { get; set; }

        /// <exclude>Excluded</exclude>
        MenuCommand _FontsMenu;
        public MenuCommand FontsMenu
        {
            get
            {
                if (_FontsMenu == null)
                {
                    _FontsMenu = new WPFUIElementObjectBind.MenuCommand()
                    {
                        Header = Properties.Resources.FontsMenuItemHeader,
                        SubMenuCommands = new List<WPFUIElementObjectBind.MenuCommand>()
                        {
                            new MenuCommand()
                            {
                                Header = Properties.Resources.HallLayoutLabelFontMenuItemHeader,
                                Command = HallLayoutShapeLabelFontCommand,
                                Icon = new System.Windows.Controls.Image() { Source = new BitmapImage(new Uri(@"pack://application:,,,/RestaurantHallLayoutDesigner;Component/Resources/Images/Metro/font16.png")), Width = 16, Height = 16 },
                            },
                            new MenuCommand()
                            {
                                Header = Properties.Resources.HallLayoutLabelBkMenuItemHeader,
                                Command = HallLayoutShapeLabelBackgroundCommand,
                                Icon = new System.Windows.Controls.Image() { Source = new BitmapImage(new Uri(@"pack://application:,,,/RestaurantHallLayoutDesigner;Component/Resources/Images/Metro/textBackground16.png")), Width = 16, Height = 16 },

                            }
                        }
                    };
                }
                return _FontsMenu;
            }
        }
        WPFUIElementObjectBind.MenuCommand _HallLayoutDesignerToolBar;
        public WPFUIElementObjectBind.MenuCommand HallLayoutDesignerToolBar
        {
            get
            {
                if (_HallLayoutDesignerToolBar == null)
                {

                    var sImageSource = new BitmapImage(new Uri(@"pack://application:,,,/RestaurantHallLayoutDesigner;Component/Resources/Images/Metro/AlignObjectsleft.png"));

                    _HallLayoutDesignerToolBar = new WPFUIElementObjectBind.MenuCommand()
                    {
                        SubMenuCommands = new List<WPFUIElementObjectBind.MenuCommand>()
                        {

                            new MenuCommand()
                            {
                                ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/RestaurantHallLayoutDesigner;Component/Resources/Images/Metro/AlignObjectsleft.png")),
                                Command = AlignLeftCommand,
                                ToolTipText = Properties.Resources.ToolTipAlignLefts
                            },
                            new MenuCommand()
                            {
                                ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuDesignerLib;Component/Resources/Images/Metro/AlignObjectsCenteredHorizontal.png")),
                                Command = AlignHorizontalCentersCommand,
                                ToolTipText = Properties.Resources.ToolTipCenterHorizontally
                            },
                            new MenuCommand()
                            {
                                ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuDesignerLib;Component/Resources/Images/Metro/AlignObjectsRight.png")),
                                Command = AlignRightCommand,
                                ToolTipText = Properties.Resources.ToolTipAlignRights
                            },
                            new MenuCommand()
                            {
                                ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuDesignerLib;Component/Resources/Images/Metro/AlignObjectsTop.png")),
                                Command = AlignTopCommand,
                                ToolTipText = Properties.Resources.ToolTipAlignTops
                            },
                            new MenuCommand()
                            {
                                ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuDesignerLib;Component/Resources/Images/Metro/AlignObjectsCenteredVertical.png")),
                                Command = AlignVerticalCentersCommand,
                                ToolTipText = Properties.Resources.ToolTipVerticalSpacingEqual
                            },
                            new MenuCommand()
                            {
                                ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuDesignerLib;Component/Resources/Images/Metro/AlignObjectsBottom.png")),
                                Command = AlignBottomCommand,
                                ToolTipText = Properties.Resources.ToolTipAlignBottoms
                            },
                            new MenuCommand()
                            {
                                ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuDesignerLib;Component/Resources/Images/Metro/DistributeObjectsVertical.png")),
                                Command = DistributeVerticalCommand,
                                ToolTipText = Properties.Resources.ToolTipVerticalSpacingEqual
                            },
                            new MenuCommand()
                            {
                                ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuDesignerLib;Component/Resources/Images/Metro/DistributeObjectsHorizontal.png")),
                                Command = DistributeHorizontalCommand,
                                ToolTipText = Properties.Resources.ToolTipHorizontalSpacingEqual
                            },
                            new MenuCommand()
                            {
                                ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuDesignerLib;Component/Resources/Images/Metro/same-width16.png")),
                                Command = new WPFUIElementObjectBind.RoutedCommand(new List<InputGesture>() { new KeyGesture(Key.N, ModifierKeys.Control) }, (object seneder) => { }),
                                ToolTipText = Properties.Resources.ToolTipMakeSameWidth
                            },
                            new MenuCommand()
                            {
                                ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuDesignerLib;Component/Resources/Images/Metro/same-height16.png")),
                                Command = SameHeightCommand,
                                ToolTipText = Properties.Resources.ToolTipMakeSameHeight
                            },
                            new MenuCommand()
                            {
                                ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuDesignerLib;Component/Resources/Images/Metro/same-size16.png")),
                                Command = SameSizeCommand,
                                ToolTipText = Properties.Resources.ToolTipMakeSameSize
                            },
                              new MenuCommand()
                            {
                                ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuDesignerLib;Component/Resources/Images/Metro/360-degrees16.png")),
                                Command = SameSizeCommand,
                                DataTemplateStaticResource="RotateButton",
                                ToolTipText = Properties.Resources.ToolTipMakeSameSize
                            }



                        }
                    };

                }
                return _HallLayoutDesignerToolBar;
            }
        }

        public HallLayoutViewModel()
        {

        }
        public string HallName
        {
            get
            {
                return ServiceAreaViewModel.ServiceArea.Description;
            }
        }
        //internal readonly MenuModel.IMenu RestaurantMenuData;
        internal IServiceAreaViewModel ServiceAreaViewModel { get; }
        public HallLayoutViewModel(IServiceAreaViewModel serviceAreaViewModel)//, MenuModel.IMenu menu)
        {
            //RestaurantMenuData = menu;
            CutCommand = new WPFUIElementObjectBind.RoutedCommand(new List<InputGesture>() { new KeyGesture(Key.X, ModifierKeys.Control) }, (object sender) => DesignerCanvas.Cut_Executed(sender, null), (object sender) => DesignerCanvas.CutEnabled());
            CopyCommand = new WPFUIElementObjectBind.RoutedCommand(new List<InputGesture>() { new KeyGesture(Key.C, ModifierKeys.Control) }, (object sender) => DesignerCanvas.Copy_Executed(sender, null), (object sender) => DesignerCanvas.CopyEnabled());
            PasteCommand = new WPFUIElementObjectBind.RoutedCommand(new List<InputGesture>() { new KeyGesture(Key.V, ModifierKeys.Control) }, (object sender) => DesignerCanvas.Paste_Executed(sender, null), (object sender) => DesignerCanvas.PasteEnabled());
            DeleteCommand = new WPFUIElementObjectBind.RoutedCommand(new List<InputGesture>() { new KeyGesture(Key.Delete) }, (object sender) => DesignerCanvas.Delete_Executed(sender, null), (object sender) => DesignerCanvas.DeleteEnabled());
            UndoCommand = new WPFUIElementObjectBind.RoutedCommand(new List<InputGesture>() { new KeyGesture(Key.Z, ModifierKeys.Control) }, (object sender) => DesignerCanvas.Undo_Executed(sender, null), (object sender) => DesignerCanvas.Undo_Enabled(sender));
            RedoCommand = new WPFUIElementObjectBind.RoutedCommand(new List<InputGesture>() { new KeyGesture(Key.Y, ModifierKeys.Control) }, (object sender) => DesignerCanvas.Redo_Executed(sender, null), (object sender) => DesignerCanvas.Redo_Enabled(sender));

            BringToFrontCommand = new WPFUIElementObjectBind.RoutedCommand((object sender) => DesignerCanvas.BringToFront_Executed(sender, null), (object sender) => DesignerCanvas.Order_Enabled());
            SendToBackCommand = new WPFUIElementObjectBind.RoutedCommand((object sender) => DesignerCanvas.SendToBack_Executed(sender, null), (object sender) => DesignerCanvas.Order_Enabled());
            GroupCommand = new WPFUIElementObjectBind.RoutedCommand((object sender) => DesignerCanvas.Group_Executed(sender, null), (object sender) => DesignerCanvas.Group_CanExecute(sender));
            UngroupCommand = new WPFUIElementObjectBind.RoutedCommand((object sender) => DesignerCanvas.Ungroup_Executed(sender, null), (object sender) => DesignerCanvas.Ungroup_CanExecute(sender));



            AlignLeftCommand = new RelayCommand((object sender) => DesignerCanvas?.AlignLeft_Executed(sender, null));
            AlignRightCommand = new RelayCommand((object sender) => DesignerCanvas.AlignRight_Executed(sender, null));
            AlignTopCommand = new RelayCommand((object sender) => DesignerCanvas.AlignTop_Executed(sender, null));
            AlignBottomCommand = new RelayCommand((object sender) => DesignerCanvas.AlignBottom_Executed(sender, null));
            AlignHorizontalCentersCommand = new RelayCommand((object sender) => DesignerCanvas.AlignHorizontalCenters_Executed(sender, null));
            AlignVerticalCentersCommand = new RelayCommand((object sender) => DesignerCanvas.AlignVerticalCenters_Executed(sender, null));
            DistributeHorizontalCommand = new RelayCommand((object sender) => DesignerCanvas.DistributeHorizontal_Executed(sender, null));
            DistributeVerticalCommand = new RelayCommand((object sender) => DesignerCanvas.DistributeVertical_Executed(sender, null));
            SameWidthCommand = new RelayCommand((object sender) => DesignerCanvas.SameWidth_Executed(sender, null));
            SameHeightCommand = new RelayCommand((object sender) => DesignerCanvas.SameHeight_Executed(sender, null));
            SameSizeCommand = new RelayCommand((object sender) => DesignerCanvas.SameSize_Executed(sender, null));
            RotateCommand = new RelayCommand((object sender) =>
            {

                RotatePopupIsOpen = true;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RotatePopupIsOpen)));

            }, (object sesnder) => DesignerCanvas.SelectedShape != null);



            ServiceAreaViewModel = serviceAreaViewModel;
            _HallLayout = serviceAreaViewModel.RestaurantHallLayout;

            SelectedPaperSize = PaperSizes.Where(x => x.PaperType == PaperType.A4).FirstOrDefault();
            Landscape = true;
            _FontPresantation = new FontPresantation() { Font = _HallLayout.Font, TitlebarText = Properties.Resources.ShapeLabelFontsTitlebarText };
            _FontPresantation.PropertyChanged += _FontPresantation_PropertyChanged;

            _LabelBkColor = (Color)ColorConverter.ConvertFromString(_HallLayout.LabelBkColor);

            UpdateCanvasItemValues();

            HallLayoutShapeLabelFontCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                DesignerCanvas designer = this.DesignerCanvas;
                if (designer != null)
                    designer.LabelFont_Executed(sender, null);
            });

            HallLayoutShapeLabelBackgroundCommand = new RelayCommand((object sender) =>
            {
                DesignerCanvas designer = this.DesignerCanvas;
                if (designer != null)
                    designer.LabelBackground_Executed(sender, null);
            });
            HallLayoutShapeServicepointToggleCommand = new RelayCommand((object sender) =>
            {

            });

            HallLayoutPageSetupCommand = new RelayCommand((object sender) =>
             {
                 DesignerCanvas designer = this.DesignerCanvas;
                 if (designer != null)
                     designer.PageSetup_Executed(sender, null);
             });
            HallLayoutEditCommand = new RelayCommand((object sender) =>
            {
                EditPopupIsOpen = true;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EditPopupIsOpen)));
                //DesignerCanvas designer = this.DesignerCanvas;
                //if (designer != null)
                //    designer.PageSetup_Executed(sender, null);
            });
            HallLayoutCutCommand = new RelayCommand((object sender) =>
            {
                DesignerCanvas designer = this.DesignerCanvas;
                if (designer != null)
                    designer.Cut_Executed(sender, null);

            }, (object sender) =>
            {
                DesignerCanvas designer = this.DesignerCanvas;
                if (designer != null)
                    return designer.CutEnabled();
                return false;

            });

            HallLayoutDeleteCommand = new RelayCommand((object sender) =>
            {
                DesignerCanvas designer = this.DesignerCanvas;
                if (designer != null)
                    designer.Delete_Executed(sender, null);

            }, (object sender) =>
            {
                DesignerCanvas designer = this.DesignerCanvas;
                if (designer != null)
                    return designer.DeleteEnabled();
                return false;

            });

            HallLayoutCopyCommand = new RelayCommand((object sender) =>
            {
                DesignerCanvas designer = this.DesignerCanvas;
                if (designer != null)
                    designer.Copy_Executed(sender, null);

            }, (object sender) =>
            {
                DesignerCanvas designer = this.DesignerCanvas;
                if (designer != null)
                    return designer.CopyEnabled();
                return false;

            });
            HallLayoutPasteCommand = new RelayCommand((object sender) =>
            {
                DesignerCanvas designer = this.DesignerCanvas;
                if (designer != null)
                    designer.Paste_Executed(sender, null);

            }, (object sender) =>
            {
                DesignerCanvas designer = this.DesignerCanvas;
                if (designer != null)
                    return designer.PasteEnabled();
                return false;

            });
            HallLayoutGroupCommand = new RelayCommand((object sender) =>
            {
                DesignerCanvas designer = this.DesignerCanvas;
                if (designer != null)
                    designer.Group_Executed(sender, null);

            }, (object sender) =>
            {
                DesignerCanvas designer = this.DesignerCanvas;
                if (designer != null)
                    return designer.Group_CanExecute(sender);
                return false;

            });
            HallLayoutUngroupCommand = new RelayCommand((object sender) =>
            {
                DesignerCanvas designer = this.DesignerCanvas;
                if (designer != null)
                    designer.Ungroup_Executed(sender, null);

            }, (object sender) =>
            {
                DesignerCanvas designer = this.DesignerCanvas;
                if (designer != null)
                    return designer.Ungroup_CanExecute(sender);
                return false;

            });

            MealTypeSelectCommand = new RelayCommand((object sender) =>
            {
                MealTypesViewExpanded = false;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MealTypesViewExpanded)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DefaultMealType)));

            });



        }





        /// <exclude>Excluded</exclude> 
        SolidColorBrush _LabelBackground;

        public Brush LabelBackground
        {
            get
            {
                if (_LabelBackground == null)
                    _LabelBackground = new SolidColorBrush(LabelBkColor);

                return _LabelBackground;
            }
        }

        /// <exclude>Excluded</exclude>
        Color _LabelBkColor;
        public Color LabelBkColor
        {
            get
            {
                return _LabelBkColor;
            }
            set
            {
                _LabelBkColor = value;
                HallLayout.LabelBkColor = new ColorConverter().ConvertToString(value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LabelBkColor)));
            }
        }


        public string OpacityValue
        {
            get => Opacity.ToString("N2");
        }




        public double Opacity
        {
            get
            {
                if (HallLayout != null)
                    return HallLayout.LabelBkOpacity;
                return 0;
            }
            set
            {

                HallLayout.LabelBkOpacity = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OpacityValue)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Opacity)));


            }
        }



        public double LabelBkCornerRadius
        {
            get
            {
                if (HallLayout != null)
                    return HallLayout.LabelBkCornerRadius;
                else
                    return 0;
            }
            set
            {
                if (HallLayout != null)
                {
                    HallLayout.LabelBkCornerRadius = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LabelBkCornerRadius)));
                }
            }
        }



        public Thickness LabelMargin
        {
            get
            {
                double top, left, bottom, right = 0;
                left = LabelPaddingLeft;
                top = LabelPaddingTop - this.HallLayout.Font.GetTextCapsLine(SampleLabel);

                if (LabelPaddingRight - this.HallLayout.Font.ShadowXOffset < 0)
                    right = this.HallLayout.Font.ShadowXOffset;
                else
                    right = LabelPaddingRight;

                if (LabelPaddingBottom - this.HallLayout.Font.ShadowYOffset < 0)
                    bottom = this.HallLayout.Font.ShadowYOffset;
                else
                    bottom = LabelPaddingBottom;

                return new Thickness(left, top, right, bottom);
            }
        }

        public string SampleLabel
        {
            get
            {
                if (HallLayout.Font.AllCaps)
                    return "SampleText".ToUpper();
                else
                    return "SampleText";

            }
        }



        public double LabelPaddingTop
        {
            get
            {
                if (HallLayout != null)
                    return HallLayout.Margin.MarginTop;
                return 0;
            }
            set
            {
                var margin = HallLayout.Margin;
                margin.MarginTop = value;
                HallLayout.Margin = margin;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LabelMargin)));
            }
        }

        public double LabelPaddingLeft
        {
            get
            {
                if (HallLayout != null)
                    return HallLayout.Margin.MarginLeft;
                return 0;
            }
            set
            {
                var margin = HallLayout.Margin;
                margin.MarginLeft = value;
                HallLayout.Margin = margin;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LabelMargin)));
            }
        }

        public double LabelPaddingBottom
        {
            get
            {
                if (HallLayout != null)
                    return HallLayout.Margin.MarginBottom;
                return 0;
            }
            set
            {
                var margin = HallLayout.Margin;
                margin.MarginBottom = value;
                HallLayout.Margin = margin;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LabelMargin)));
            }
        }

        public double LabelPaddingRight
        {
            get
            {
                if (HallLayout != null)
                    return HallLayout.Margin.MarginRight;
                return 0;
            }
            set
            {
                var margin = HallLayout.Margin;
                margin.MarginRight = value;
                HallLayout.Margin = margin;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LabelMargin)));
            }
        }

        public string MarginUnit
        {
            get => "px";
        }
        public string RadiusUnit
        {
            get => "px";
        }

        /// <exclude>Excluded</exclude>
        System.Windows.Media.FontFamily _FontFamily;
        public FontFamily FontFamily
        {
            get
            {
                return _FontFamily;

            }
        }

        /// <exclude>Excluded</exclude>
        FontStyle _FontStyle;
        public FontStyle FontStyle
        {
            get
            {
                return _FontStyle;
            }
        }

        /// <exclude>Excluded</exclude>
        FontWeight _FontWeight;
        public FontWeight FontWeight
        {
            get
            {
                return _FontWeight;
            }
        }
        /// <exclude>Excluded</exclude>
        Color _StrokeFill;
        Brush _StrokeFillBrush;
        public Brush StrokeFill
        {
            get
            {
                return _StrokeFillBrush;
            }
        }

        /// <exclude>Excluded</exclude>
        double _StrokeThickness;
        public double StrokeThickness
        {
            get
            {
                return _StrokeThickness;
            }
        }

        /// <exclude>Excluded</exclude>
        bool _AllCaps;
        public bool AllCaps
        {
            get
            {
                return _AllCaps;
            }
        }

        /// <exclude>Excluded</exclude>
        Color _Foreground;

        /// <exclude>Excluded</exclude>
        Brush _ForegroundBrush;
        public Brush Foreground
        {
            get
            {
                return _ForegroundBrush;
            }
        }

        /// <exclude>Excluded</exclude>
        System.Windows.Media.Effects.DropShadowEffect _DropShadowEffect;
        public System.Windows.Media.Effects.DropShadowEffect DropShadowEffect
        {
            get
            {
                return _DropShadowEffect;
            }
        }

        /// <exclude>Excluded</exclude>
        double _FontSize;
        /// <MetaDataID>{e6273d48-ed55-4313-9ee7-8803ba40ecd1}</MetaDataID>
        public double FontSize
        {
            get
            {
                return _FontSize;

            }
        }

        /// <exclude>Excluded</exclude>
        double _FontSpacing;
        public double FontSpacing
        {
            get
            {
                return _FontSpacing;
            }
        }



        FontData? LabelFont
        {
            get
            {
                return FontPresantation.Font;
            }
        }
        private void UpdateCanvasItemValues()
        {

            FontFamily fontFamily = null;
            if (LabelFont != null)
                fontFamily = FontData.FontFamilies[LabelFont.Value.FontFamilyName];
            else
                fontFamily = null;

            if (fontFamily != _FontFamily)
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _FontFamily = fontFamily;
                    stateTransition.Consistent = true;
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FontFamily)));
            }

            FontStyle fontStyle = default(FontStyle);

            if (LabelFont != null)
                fontStyle = (FontStyle)new FontStyleConverter().ConvertFromString(LabelFont.Value.FontStyle);
            else
                fontStyle = default(FontStyle);
            if (fontStyle != _FontStyle)
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _FontStyle = fontStyle;
                    stateTransition.Consistent = true;
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FontStyle)));
            }

            FontWeight fontWeight = default(FontWeight);
            if (LabelFont != null)
                fontWeight = (FontWeight)new FontWeightConverter().ConvertFromString(LabelFont.Value.FontWeight);
            else
                fontWeight = default(FontWeight);

            if (fontWeight != _FontWeight)
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _FontWeight = fontWeight;
                    stateTransition.Consistent = true;
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FontWeight)));

            }

            double strokeThickness = 0;
            if (LabelFont != null && LabelFont.Value.Stroke)
                strokeThickness = LabelFont.Value.StrokeThickness;
            else
                strokeThickness = 0;

            if (strokeThickness != _StrokeThickness)
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _StrokeThickness = strokeThickness;
                    stateTransition.Consistent = true;
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StrokeThickness)));
            }

            bool allCaps = false;
            if (LabelFont != null)
                allCaps = LabelFont.Value.AllCaps;
            else
                allCaps = false;

            if (allCaps != _AllCaps)
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _AllCaps = allCaps;
                    stateTransition.Consistent = true;
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllCaps)));
            }

            Color strokeFill = default(Color);

            if (LabelFont != null)
            {
                if (LabelFont.Value.StrokeFill != null && LabelFont.Value.Stroke)
                    strokeFill = (Color)ColorConverter.ConvertFromString(LabelFont.Value.StrokeFill);
                else
                    strokeFill = _Foreground;
            }
            else
                strokeFill = _Foreground;

            if (strokeFill != _StrokeFill)
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _StrokeFill = strokeFill;
                    stateTransition.Consistent = true;
                }

                _StrokeFillBrush = new SolidColorBrush(strokeFill);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StrokeFill)));
            }


            System.Windows.Media.Effects.DropShadowEffect dropShadowEffect = null;

            if (LabelFont != null)
            {
                double deltaX = LabelFont.Value.ShadowXOffset;
                double deltaY = LabelFont.Value.ShadowXOffset;

                if (LabelFont.Value.ShadowColor == null && deltaX == 0 && deltaY == 0)
                    dropShadowEffect = null;
                else
                {

                    deltaY = -deltaY;
                    var rad = Math.Atan2(deltaY, deltaX);

                    var deg = rad * (180 / Math.PI);

                    if (deg < 0)
                        deg = 360 + deg;

                    double a = deltaX;
                    double b = deltaY;
                    if (a < 0)
                        a = -a;
                    if (b < 0)
                        b = -b;
                    double depth = Math.Sqrt(a * a + b * b);

                    var shaddow = new System.Windows.Media.Effects.DropShadowEffect();
                    shaddow.Direction = deg;
                    shaddow.ShadowDepth = depth;

                    shaddow.Opacity = 1;
                    shaddow.BlurRadius = LabelFont.Value.BlurRadius;
                    shaddow.Color = (Color)ColorConverter.ConvertFromString(LabelFont.Value.ShadowColor);
                    dropShadowEffect = shaddow;
                }
            }
            else
                dropShadowEffect = null;

            if (dropShadowEffect != _DropShadowEffect)
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _DropShadowEffect = dropShadowEffect;
                    stateTransition.Consistent = true;
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DropShadowEffect)));
            }

            Color foreground = default(Color);

            if (LabelFont != null)
                foreground = (Color)ColorConverter.ConvertFromString(LabelFont.Value.Foreground);
            else
                foreground = default(Color);

            if (foreground != _Foreground)
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _Foreground = foreground;
                    _ForegroundBrush = new SolidColorBrush(foreground);

                    stateTransition.Consistent = true;
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Foreground)));
            }

            double fontSize = 0;
            if (LabelFont != null)
                fontSize = LabelFont.Value.FontSize;
            else
                fontSize = 0;

            if (fontSize != _FontSize)
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _FontSize = fontSize;
                    stateTransition.Consistent = true;
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FontSize)));
            }

            double fontSpacing = 0;
            if (LabelFont != null)
                fontSpacing = LabelFont.Value.FontSpacing;
            else
                fontSpacing = 0;

            if (fontSpacing != _FontSpacing)
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _FontSpacing = fontSpacing;
                    stateTransition.Consistent = true;
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FontSpacing)));
            }
        }


        private void _FontPresantation_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateCanvasItemValues();
            if (LabelFont != null)
                HallLayout.Font = LabelFont.Value;
            //WindowsBaseEx.FontData font = new WindowsBaseEx.FontData() { AllCaps = true, BlurRadius = 0, FontFamilyName = "Airbag Free", FontSize = 22, Shadow = true, ShadowColor = "#FFD3D3D3", ShadowXOffset = 2, ShadowYOffset = 2 };
        }

        // FontPresantation


        /// <exclude>Excluded</exclude>
        bool _Portrait = true;
        public bool Portrait
        {
            get
            {
                return _Portrait;
            }
            set
            {
                if (value)
                    Landscape = false;

                _Portrait = value;
                if (_Portrait)
                {
                    PaperSize paperSize = new PaperSize(PaperType.Custom, PaperType.Custom.ToString(), mmToPixel(_PageWidth), mmToPixel(PageHeight), "px");
                    _HallLayout.Width = paperSize.Width;
                    _HallLayout.Height = paperSize.Height;
                    HallLayoutSizeChanged?.Invoke(_HallLayout, paperSize);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HallLayoutWidth)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HallLayoutHeight)));

                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Portrait)));
            }
        }

        /// <exclude>Excluded</exclude>
        bool _Landscape;

        public bool Landscape
        {
            get
            {
                return _Landscape;
            }
            set
            {
                if (value)
                    Portrait = false;
                _Landscape = value;
                if (_Landscape)
                {
                    PaperSize paperSize = new PaperSize(PaperType.Custom, PaperType.Custom.ToString(), mmToPixel(PageHeight), mmToPixel(_PageWidth), "px");
                    _HallLayout.Width = paperSize.Width;
                    _HallLayout.Height = paperSize.Height;
                    HallLayoutSizeChanged?.Invoke(_HallLayout, paperSize);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HallLayoutWidth)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HallLayoutHeight)));

                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Landscape)));
            }
        }



        /// <exclude>Excluded</exclude>
        static System.Collections.Generic.List<PaperSize> _PaperSizes = null;
        /// <MetaDataID>{031f7ff7-7702-4bb2-98b3-c22c1e3aa55d}</MetaDataID>
        public List<PaperSize> PaperSizes
        {
            get
            {
                if (_PaperSizes == null)
                {
                    _PaperSizes = new System.Collections.Generic.List<PaperSize>()
                    {
                        new PaperSize(PaperType.Letter,"Letter (216mm x 280mm)",216,280,"mm"),
                        new PaperSize(PaperType.Legal,"Legal (216mm x 355.6mm)",216,355.6,"mm" ),
                        new PaperSize(PaperType.Tabloid,"Tabloid (280mm x 432mm)",280,432 ,"mm"),
                        new PaperSize(PaperType.Statement,"Statement (140mm x 216mm)",140,216 ,"mm"),
                        new PaperSize(PaperType.A3,"A3 (297mm x 420mm)",297,420 ,"mm"),
                        new PaperSize(PaperType.A4,"A4 (210mm x 297mm)",210,297 ,"mm"),
                        new PaperSize(PaperType.A5,"A5 (148mm x 210mm)",148,210 ,"mm"),
                        new PaperSize(PaperType.B4,"B4 (250mm x 353mm)" ,250,353,"mm"),
                        new PaperSize(PaperType.B5,"B5 (176mm x 250mm)" ,176,250,"mm"),
                        new PaperSize(PaperType.HDTV,"HDTV (286mm x 508mm)" ,286,508,"mm"),
                        new PaperSize(PaperType.Custom,"Custom" ,0,0,"mm")
                    };
                }
                return _PaperSizes;
            }
        }


        /// <exclude>Excluded</exclude>
        double _PageWidth;
        public double PageWidth
        {
            set
            {
                _PageWidth = value;
                if (_PageWidth > 0)
                {
                    PaperSize paperSize = new PaperSize(PaperType.Custom,
                                                                PaperType.Custom.ToString(), mmToPixel(_PageWidth), mmToPixel(PageHeight), "px");


                    //PageStyle.PageSize = paperSize;
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PageWidth)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HallLayoutWidth)));
            }
            get
            {
                return _PageWidth;
            }
        }



        /// <exclude>Excluded</exclude>
        double _PageHeight;

        public event PropertyChangedEventHandler PropertyChanged;

        public double PageHeight
        {
            set
            {
                _PageHeight = value;
                if (_PageHeight > 0)
                {
                    PaperSize paperSize = new PaperSize(PaperType.Custom,
                                                        PaperType.Custom.ToString(), mmToPixel(_PageWidth), mmToPixel(PageHeight), "px");



                    //PageStyle.PageSize = paperSize;
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PageHeight)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HallLayoutHeight)));
            }
            get
            {

                return _PageHeight;
            }
        }

        public double BorderThickness
        {
            get
            {
                return 10;
            }
        }

        public double HallLayoutHeight
        {
            get
            {
                if (Portrait)
                    return mmToPixel(PageHeight) + 2 * BorderThickness;
                else
                    return mmToPixel(PageWidth) + 2 * BorderThickness;
            }
        }

        public double HallLayoutWidth
        {
            get
            {
                if (Portrait)
                    return mmToPixel(PageWidth) + 2 * BorderThickness;
                else
                    return mmToPixel(PageHeight) + 2 * BorderThickness;
            }
        }


        PaperSize _SelectedPaperSize;
        public PaperSize SelectedPaperSize
        {
            get
            {
                return _SelectedPaperSize;
            }
            set
            {


                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this, TransactionOption.Supported))
                {
                    _SelectedPaperSize = value;
                    if (_SelectedPaperSize.Width != 0 && _SelectedPaperSize.Height != 0)
                    {
                        PageWidth = _SelectedPaperSize.Width;
                        PageHeight = _SelectedPaperSize.Height;

                        if (_Portrait)
                        {
                            PaperSize paperSize = new PaperSize(PaperType.Custom, PaperType.Custom.ToString(), mmToPixel(_PageWidth), mmToPixel(PageHeight), "px");
                            _HallLayout.Width = paperSize.Width;
                            _HallLayout.Height = paperSize.Height;
                            HallLayoutSizeChanged?.Invoke(_HallLayout, paperSize);
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HallLayoutWidth)));
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HallLayoutHeight)));


                        }
                        if (_Landscape)
                        {
                            PaperSize paperSize = new PaperSize(PaperType.Custom, PaperType.Custom.ToString(), mmToPixel(PageHeight), mmToPixel(_PageWidth), "px");
                            _HallLayout.Width = paperSize.Width;
                            _HallLayout.Height = paperSize.Height;
                            HallLayoutSizeChanged?.Invoke(_HallLayout, paperSize);
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HallLayoutWidth)));
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HallLayoutHeight)));

                        }

                    }


                    stateTransition.Consistent = true;
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsCustomPaperSize)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedPaperSize)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PortraitLandscapeIsEnabled)));

            }

        }

        public bool PortraitLandscapeIsEnabled
        {
            get
            {
                if (_SelectedPaperSize.PaperType != PaperType.Custom)
                    return true;
                else
                    return false;
            }
        }

        public bool IsCustomPaperSize
        {
            get
            {
                if (SelectedPaperSize.PaperType == PaperType.Custom)
                    return true;
                else
                    return false;

            }
        }



        /// <exclude>Excluded</exclude>
        string _Unit = "mm";
        public string Unit
        {
            get
            {
                return _Unit;
            }
            set
            {
                _Unit = value;
            }
        }

        /// <exclude>Excluded</exclude>
        double _ZoomPercentage;
        public double ZoomPercentage
        {
            get
            {
                return _ZoomPercentage;
            }
            set
            {
                _ZoomPercentage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ZoomPercentage)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ZoomPercentageLabel)));
            }
        }


        public string ZoomPercentageLabel
        {
            get
            {
                return ZoomPercentage.ToString("N2") + "%";
            }
        }

        FontPresantation _FontPresantation;
        DesignerCanvas _DesignerCanvas;
        public DesignerCanvas DesignerCanvas
        {
            get => _DesignerCanvas;
            internal set
            {
                _DesignerCanvas = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DesignerCanvas)));

            }
        }
        public RoutedCommand BringToFrontCommand { get; private set; }
        public RoutedCommand SendToBackCommand { get; private set; }
        public RoutedCommand GroupCommand { get; private set; }
        public RoutedCommand UngroupCommand { get; private set; }
        public RoutedCommand CutCommand { get; private set; }
        public RoutedCommand CopyCommand { get; private set; }
        public RoutedCommand PasteCommand { get; private set; }
        public RoutedCommand DeleteCommand { get; private set; }
        public RoutedCommand UndoCommand { get; private set; }
        public RoutedCommand RedoCommand { get; private set; }
        public RelayCommand AlignLeftCommand { get; private set; }
        public RelayCommand AlignRightCommand { get; private set; }
        public RelayCommand AlignTopCommand { get; private set; }
        public RelayCommand AlignBottomCommand { get; private set; }
        public RelayCommand AlignHorizontalCentersCommand { get; private set; }
        public RelayCommand AlignVerticalCentersCommand { get; private set; }
        public RelayCommand DistributeHorizontalCommand { get; private set; }
        public RelayCommand DistributeVerticalCommand { get; private set; }
        public RelayCommand SameWidthCommand { get; private set; }
        public RelayCommand SameHeightCommand { get; private set; }
        public RelayCommand SameSizeCommand { get; private set; }
        public RelayCommand RotateCommand { get; private set; }

        public RelayCommand HallLayoutShapeLabelFontCommand { get; protected set; }
        public FontPresantation FontPresantation
        {
            get
            {
                return _FontPresantation;
            }
        }
        public WPFUIElementObjectBind.RelayCommand HallLayoutShapeLabelBackgroundCommand { get; protected set; }
        public WPFUIElementObjectBind.RelayCommand HallLayoutShapeServicepointToggleCommand { get; protected set; }
        public WPFUIElementObjectBind.RelayCommand HallLayoutCutCommand { get; protected set; }
        public WPFUIElementObjectBind.RelayCommand HallLayoutPageSetupCommand { get; protected set; }
        public WPFUIElementObjectBind.RelayCommand HallLayoutEditCommand { get; protected set; }
        public WPFUIElementObjectBind.RelayCommand HallLayoutCopyCommand { get; protected set; }
        public WPFUIElementObjectBind.RelayCommand HallLayoutPasteCommand { get; protected set; }
        public WPFUIElementObjectBind.RelayCommand HallLayoutDeleteCommand { get; protected set; }
        public WPFUIElementObjectBind.RelayCommand HallLayoutGroupCommand { get; protected set; }
        public WPFUIElementObjectBind.RelayCommand HallLayoutUngroupCommand { get; protected set; }
        public bool RotatePopupIsOpen { get; private set; }

        public static double PixelToMM(double px)
        {
            var pixpermm = (double)new System.Windows.LengthConverter().ConvertFromString("1in") / 25.4;
            return px / pixpermm;

            //return (int)Math.Round(px / pixpermm, 1);
        }

        public static double mmToPixel(double mm)
        {
            var pixpermm = (double)new System.Windows.LengthConverter().ConvertFromString("1in") / 25.4;
            return mm * pixpermm;

            //return (int)Math.Round(mm * pixpermm, 1);
        }



    }

    /// <MetaDataID>{1ee2fb31-544d-4ee2-a357-312e599b2332}</MetaDataID>
    public interface IServiceAreaViewModel
    {
        List<IServicePointViewModel> ServicePoints { get; }

        IServiceArea ServiceArea { get; }
        HallLayout RestaurantHallLayout { get; }

        List<AssignedMealTypeViewMode> GetMealTypes(string servicesPointIdentity);
        IServicePointViewModel GetServicePoint(string servicesPointIdentity);
        Task<IServicePointViewModel> NewServicePoint();
        void SetServicePointName(string servicesPointIdentity, string label);
        void SetServicePointSeats(string servicesPointIdentity, int seats);
    }

    /// <MetaDataID>{ea5dbd7d-1978-4c77-bb99-24d90d745571}</MetaDataID>
    public interface IServicePointViewModel
    {
        IServicePoint ServicePoint { get; }
    }
}
