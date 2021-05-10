using System;
using System.Collections.Generic;
using System.Windows;
using MenuModel;
using OOAdvantech;
using OOAdvantech.Transactions;
using OOAdvantech.MetaDataRepository;
using System.Linq;
namespace MenuPresentationModel.MenuCanvas
{
    /// <MetaDataID>{7df6122e-f9e9-468f-b827-364e167c805f}</MetaDataID>
    [BackwardCompatibilityID("{7df6122e-f9e9-468f-b827-364e167c805f}")]
    [Persistent()]
    public class ItemsMultiPriceHeading : MarshalByRefObject, IMenuCanvasItem, IItemMultiPriceHeading
    {



        OOAdvantech.ObjectStateManagerLink StateManagerLink;
        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<IPriceHeading> _PriceHeadings = new OOAdvantech.Collections.Generic.Set<IPriceHeading>();

        /// <MetaDataID>{33c02cb6-4b5e-4f6a-8e22-68409c30d5b8}</MetaDataID>
        [PersistentMember(nameof(_PriceHeadings))]
        [BackwardCompatibilityID("+7")]
        public IList<IPriceHeading> PriceHeadings
        {
            get
            {
                return _PriceHeadings.AsReadOnly();
            }
        }



        private static System.Windows.Rect RotateRect(System.Windows.Rect sourceRect, double angle, Point rotationPoint)
        {
            rotationPoint = new Point(sourceRect.X + rotationPoint.X, sourceRect.Y + rotationPoint.Y);
            var corners = new[]
                {new Point(sourceRect.Left-rotationPoint.X, sourceRect.Top-rotationPoint.Y), new Point(sourceRect.Width+(sourceRect.Left-rotationPoint.X), sourceRect.Top-rotationPoint.Y), new Point(sourceRect.Left-rotationPoint.X, sourceRect.Height+(sourceRect.Top-rotationPoint.Y)), new Point(sourceRect.Width+(sourceRect.Left-rotationPoint.X), sourceRect.Height+(sourceRect.Top-rotationPoint.Y))};

            var xc = corners.Select(p => Rotate(p, angle).X);
            var yc = corners.Select(p => Rotate(p, angle).Y);

            //create a new empty bitmap to hold rotated image
            Size returnBitmap = new Size(Math.Abs(xc.Max() - xc.Min()), Math.Abs(yc.Max() - yc.Min()));
            System.Windows.Rect rect = new System.Windows.Rect(new Point(xc.Min() + rotationPoint.X, yc.Min() + rotationPoint.Y), new Point(xc.Max() + rotationPoint.X, yc.Max() + rotationPoint.Y));
            return rect;
        }

        /// <summary>
        /// Rotates a point around the origin (0,0)
        /// </summary>
        private static Point Rotate(Point p, double angle)
        {
            // convert from angle to radians
            var theta = Math.PI * angle / 180;
            return new Point(
                (double)(Math.Cos(theta) * (p.X) - Math.Sin(theta) * (p.Y)),
                (double)(Math.Sin(theta) * (p.X) + Math.Cos(theta) * (p.Y)));
        }


        ///// <exclude>Excluded</exclude>
        //IFoodItemsGroupColumn _MenuItemsGroupColumn;

        ///// <MetaDataID>{f8102ebf-4a87-4001-8a92-87c2907e9a11}</MetaDataID>
        //[ImplementationMember(nameof(_MenuItemsGroupColumn))]
        //public IFoodItemsGroupColumn MenuItemsGroupColumn
        //{
        //    get
        //    {
        //        return _MenuItemsGroupColumn;
        //    }
        //}

        /// <MetaDataID>{0b643cc4-92e0-45db-a7e1-0d108c975f24}</MetaDataID>
        internal MenuStyles.IPriceStyle Style
        {
            get
            {
                return Page.Style.Styles["price-options"] as MenuStyles.IPriceStyle;
            }
        }
        /// <exclude>Excluded</exclude>
        string _Description;
        /// <MetaDataID>{8dc0a41f-a72a-4260-9a90-b0d436d6af27}</MetaDataID>
        [PersistentMember(nameof(_Description))]
        [BackwardCompatibilityID("+1")]
        public string Description
        {
            get
            {
                return _Description;
            }

            set
            {
                if (_Description != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Description = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        MenuStyles.FontData? _Font;

        /// <MetaDataID>{5d555c16-9096-47a5-b9d7-ec614c5ccad1}</MetaDataID>
        public MenuPresentationModel.MenuStyles.FontData Font
        {
            get
            {
                if (!_Font.HasValue)
                {
                    if (Style.Layout == MenuStyles.PriceLayout.WithDescription || Style.Layout == MenuStyles.PriceLayout.FollowDescription)
                        return (Page.Style.Styles["menu-item"] as MenuStyles.IMenuItemStyle).DescriptionFont;
                    else
                        return Style.Font;
                }
                return _Font.Value;
            }
            set
            {
                _Font = value;
            }
        }

        /// <exclude>Excluded</exclude>
        double _Height;

        /// <MetaDataID>{376aa80b-6af5-46c4-87fd-618a3d09c672}</MetaDataID>
        [PersistentMember(nameof(_Height))]
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+2")]
        public double Height
        {
            get
            {
                return _Height;
            }
            set
            {
                if (_Height != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Height = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <MetaDataID>{0a2d327b-0563-4d35-aacb-90b0e7387b58}</MetaDataID>
        public MenuPresentationModel.MenuCanvas.IMenuPageCanvas Page
        {
            get
            {

                if (FoodItems.Count > 0)
                    return FoodItems[0].Page;
                else
                    return null;

            }
        }

        /// <MetaDataID>{05e29f06-b260-452f-88e9-c5caeab07ad4}</MetaDataID>
        internal void CalculatePricesArea(List<MenuCanvasFoodItem> menuCanvasFoodtItems, FoodItemsGroupColumn foodItemsGroupColumn)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                double yDif = 0;

                double spaceWidth = 0;
                int i = 0;
                foreach (var menuCanvasFoodtItem in menuCanvasFoodtItems)
                {
                    if (menuCanvasFoodtItem.MenuItem != null &&
                        menuCanvasFoodtItem.Prices.Count > 1 &&
                        menuCanvasFoodtItem.Prices[0].ItemSelection != null &&
                        menuCanvasFoodtItem.Prices[0].ItemSelection.OptionGroup == Source)
                    {
                        i = 0;
                        foreach (MenuCanvasFoodItemPrice price in menuCanvasFoodtItem.Prices)
                        {
                            spaceWidth = price.Font.MeasureText(" ").Width;

                            double wi = price.Width;
                            if (_PriceHeadings.Count > i)
                                _PriceHeadings[i].Source = price.ItemSelection;
                            else
                            {
                                MenuCanvasTextPriceHeading priceHeading = new MenuCanvasTextPriceHeading();
                                priceHeading.Source = price.ItemSelection;
                                _PriceHeadings.Add(priceHeading);
                            }

                            if ((_PriceHeadings[i] as IMenuCanvasItem).Width < wi)
                                (_PriceHeadings[i] as IMenuCanvasItem).Width = wi;

                            _PriceHeadings[i].YPos = YPos;
                            i++;
                        }
                        while (_PriceHeadings.Count > i)
                            _PriceHeadings.RemoveAt(i);
                    }
                    else
                        break;
                }

                i = 0;
                _Width = 0;
                foreach (var priceHeading in _PriceHeadings)
                {
                    if (i == 0)
                        _Width += (priceHeading as IMenuCanvasItem).Width;
                    else
                        _Width += spaceWidth + (priceHeading as IMenuCanvasItem).Width;
                    i++;
                }
                i = 0;
                double xPos = foodItemsGroupColumn.XPos + foodItemsGroupColumn.Width - _Width;
                foreach (var priceHeading in _PriceHeadings)
                {
                    priceHeading.XPos = xPos;

                    priceHeading.PriceHeadinTextXPos = xPos;
                    xPos += (priceHeading as IMenuCanvasItem).Width + spaceWidth;
                    var angle = priceHeading.Angle;
                    var transformOrigin = priceHeading.TransformOrigin;
                    string description = priceHeading.Description;
                    if (description != null)
                    {
                        var size = priceHeading.Font.MeasureText(description);
                        priceHeading.PriceHeadingTextWitdh = size.Width;
                       Baseline = YPos + size.Height;
                        double x = 0;
                        double y = 0;

                        if (transformOrigin.xAxis == "center")
                            x = size.Width / 2;
                        if (transformOrigin.xAxis == "left" || transformOrigin.xAxis == "start")
                        {
                            x = 0;
                            priceHeading.PriceHeadinTextXPos = priceHeading.PriceHeadinTextXPos + priceHeading.Width / 2;
                        }
                        if (transformOrigin.xAxis == "right" || transformOrigin.xAxis == "end")
                        {
                            x = size.Width;
                            priceHeading.PriceHeadinTextXPos = priceHeading.PriceHeadinTextXPos- priceHeading.PriceHeadingTextWitdh+ priceHeading.Width/2;
                        }
                        if (transformOrigin.yAxis == "center")
                            y = size.Height / 2;
                        if (transformOrigin.yAxis == "top" || transformOrigin.yAxis == "start")
                            y = 0;
                        if (transformOrigin.yAxis == "bottom" || transformOrigin.yAxis == "end")
                            y = size.Height;

                        double baseLine = priceHeading.Font.GetTextBaseLine(description);
                        (priceHeading as IMenuCanvasItem).Height = size.Height;

                        System.Windows.Rect orgPriceHeadingRect = new System.Windows.Rect(priceHeading.XPos, priceHeading.YPos, size.Width, size.Height);
                        var rotatedRect = RotateRect(orgPriceHeadingRect, angle, new Point(x, y));

                       double dfd=  (rotatedRect.Width/orgPriceHeadingRect.Width) *100;

                        if (Math.Abs(yDif) < Math.Abs(orgPriceHeadingRect.Y - rotatedRect.Y))
                            yDif = orgPriceHeadingRect.Y - rotatedRect.Y;

                        if (_Height < rotatedRect.Height)
                            _Height = rotatedRect.Height;
                    }
                }

                foreach (var priceHeading in _PriceHeadings)
                    priceHeading.YPos += yDif;

                _YPos += yDif;
                Baseline += yDif;

                stateTransition.Consistent = true;
            }

        }




        /// <exclude>Excluded</exclude>
        ItemSelectorOptionsGroup _Source;

        /// <MetaDataID>{3f40bf0d-fb6e-4a17-8ffb-78fc0eea7b2e}</MetaDataID>
        [PersistentMember(nameof(_Source))]
        [BackwardCompatibilityID("+6")]
        public MenuModel.ItemSelectorOptionsGroup Source
        {
            get
            {
                return _Source;
            }

            set
            {
                if (_Source != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Source = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        double _Width;

        /// <MetaDataID>{079f1ac7-6414-4885-bbcb-61ba829096a8}</MetaDataID>
        [PersistentMember(nameof(_Width))]
        [BackwardCompatibilityID("+3")]
        public double Width
        {
            get
            {
                return _Width;
            }

            set
            {
                if (_Width != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Width = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        double _XPos;
        /// <MetaDataID>{3205ab31-bfa5-402a-b1ce-6bc424ea1752}</MetaDataID>
        [PersistentMember(nameof(_XPos))]
        [BackwardCompatibilityID("+4")]
        public double XPos
        {
            get
            {
                return _XPos;
            }

            set
            {
                if (_XPos != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _XPos = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }



        /// <MetaDataID>{7937e0ea-51fa-47e5-9cc2-4169bc91b1e1}</MetaDataID>
        public ItemsMultiPriceHeading(ItemSelectorOptionsGroup itemSelectorOptionsGroup)
        {
            _Source = itemSelectorOptionsGroup;
        }

        /// <MetaDataID>{4092bdf7-e4f8-42eb-8338-5edc9529fb9a}</MetaDataID>
        protected ItemsMultiPriceHeading()
        {

        }
        /// <exclude>Excluded</exclude>
        double _YPos;


        /// <MetaDataID>{0c9fc561-b801-4c84-94c3-64317482cb20}</MetaDataID>
        [PersistentMember(nameof(_YPos))]
        [BackwardCompatibilityID("+5")]
        public double YPos
        {
            get
            {
                return _YPos;
            }
            set
            {
                if (_YPos != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _YPos = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        OOAdvantech.Collections.Generic.Set<IMenuCanvasFoodItem> _FoodItems = new OOAdvantech.Collections.Generic.Set<IMenuCanvasFoodItem>();
        /// <MetaDataID>{32b9ac99-3ace-4d10-ac60-636d94390864}</MetaDataID>
        [PersistentMember(nameof(_FoodItems))]
        [BackwardCompatibilityID("+8")]
        public IList<IMenuCanvasFoodItem> FoodItems
        {
            get
            {
                return _FoodItems.AsReadOnly();
            }
        }

        public double Baseline { get; internal set; }

        public event ObjectChangeStateHandle ObjectChangeState;

        /// <MetaDataID>{75102711-f2b7-4c5e-ae0b-6e653176ffea}</MetaDataID>
        public void AlignOnBaseline(IMenuCanvasItem menuCanvasItemLineText)
        {
            _YPos = menuCanvasItemLineText.YPos + menuCanvasItemLineText.Font.GetTextBaseLine(Description) - Font.GetTextBaseLine(Description);
        }

        /// <MetaDataID>{d6026b37-eff7-44f4-8dfa-36cf5c62a0f1}</MetaDataID>
        public ItemRelativePos GetRelativePos(Point point)
        {
            throw new NotImplementedException();
        }
    }
}