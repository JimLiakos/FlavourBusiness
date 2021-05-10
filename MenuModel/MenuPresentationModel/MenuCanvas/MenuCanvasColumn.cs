using System;
using System.Windows;
using System.Windows.Media;
using Newtonsoft.Json;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using System.Linq;
namespace MenuPresentationModel.MenuCanvas
{
    /// <MetaDataID>{ba88bcfc-a3e4-47c5-bfee-8831fbd3a03e}</MetaDataID>
    public class MenuCanvasColumn : MarshalByRefObject, IMenuCanvasColumn
    {

        /// <MetaDataID>{d2bb546b-a1a3-4c99-a769-52fe62d7146e}</MetaDataID>
        public MenuCanvasColumn()
        {

        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<IMenuCanvasFoodItemsGroup> _FoodItemsGroup = new OOAdvantech.Collections.Generic.Set<IMenuCanvasFoodItemsGroup>();
        /// <MetaDataID>{8b0944d5-d7c4-463c-b816-c2918295fe81}</MetaDataID>
        [ImplementationMember(nameof(_FoodItemsGroup))]
        public System.Collections.Generic.IList<IMenuCanvasFoodItemsGroup> FoodItemsGroup
        {
            get
            {
                return _FoodItemsGroup.AsReadOnly();
            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.Member<IMenuPageCanvas> _Page = new OOAdvantech.Member<IMenuPageCanvas>();
        /// <MetaDataID>{8d2392df-8562-4795-9ed5-adfa98ba84e1}</MetaDataID>
        [PersistentMember(nameof(_Page))]
        [BackwardCompatibilityID("+2")]
        public MenuPresentationModel.MenuCanvas.IMenuPageCanvas Page
        {
            get
            {
                return _Page.Value;
            }
            set
            {
                if (_Page.Value != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Page.Value = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <MetaDataID>{7d2825e5-4dc8-4dee-8e16-bea254e3a40c}</MetaDataID>
        public double MaxHeight { get; set; }

        /// <MetaDataID>{ace5c1f1-7d45-40d0-b58c-e8ca246b571e}</MetaDataID>
        public double Width { get; set; }

        /// <MetaDataID>{b2147d6d-5a14-47d6-85a7-88cbeb6bdfd1}</MetaDataID>
        public double YPos { get; set; }

        /// <MetaDataID>{74b82075-e53b-4674-a110-905b9652bdcc}</MetaDataID>
        public double XPos { get; set; }

        /// <MetaDataID>{f5987660-fdff-416e-af80-525b509a6caa}</MetaDataID>
        public void RenderMenuCanvasItems(System.Collections.Generic.IList<IMenuCanvasItem> menuCanvasItems, MenuCanvas.IMenuCanvasItem previousMenuCanvasItem)
        {
            if (Page.Style != null)
            {
                _FoodItemsGroup.Clear();
                IMenuCanvasHeading lastFoodItemsHeading = null;
                var pageStyle = (Page.Style.Styles["page"] as MenuStyles.PageStyle);
                var layoutStyle = (Page.Style.Styles["layout"] as MenuStyles.LayoutStyle);
                double nextMenuCanvasItemY = YPos;
                double columnWidth = Width;

                if (previousMenuCanvasItem is IMenuCanvasHeading&& (previousMenuCanvasItem as IMenuCanvasHeading).HeadingType!=HeadingType.SubHeading)
                {
                    lastFoodItemsHeading = previousMenuCanvasItem as IMenuCanvasHeading;
                    lastFoodItemsHeading.HostingArea = null;
                }


                while (menuCanvasItems.Count > 0)
                {
                    var menuCanvasItem = menuCanvasItems[0];
                    menuCanvasItems.RemoveAt(0);

                    if (menuCanvasItem is IMenuCanvasHeading)
                    {
                        #region Render Heading

                        IMenuCanvasHeading menuCanvasHeading = menuCanvasItem as IMenuCanvasHeading;
                        nextMenuCanvasItemY += menuCanvasHeading.Style.BeforeSpacing + pageStyle.LineSpacing;
                        (menuCanvasItem as IMenuCanvasHeading).YPos = nextMenuCanvasItemY;
                        nextMenuCanvasItemY += menuCanvasHeading.Style.AfterSpacing + menuCanvasHeading.Height;

                        //if (menuCanvasHeading.Style.Accent != null)
                        //{
                        //    var accent = new MenuCanvasHeadingAccent(menuCanvasItem as IMenuCanvasHeading, menuCanvasHeading.Style.Accent);
                        //    menuCanvasHeading.Accent = accent;
                        //}
                        //else
                        //    menuCanvasHeading.Accent = null;

                        if (menuCanvasHeading.Accent != null)
                            menuCanvasHeading.Accent.FullRowWidth = columnWidth;

                        if (menuCanvasHeading.Style.Alignment == MenuStyles.Alignment.Center)
                            (menuCanvasItem as IMenuCanvasHeading).XPos = XPos+ (columnWidth / 2) - (menuCanvasHeading.Width / 2);
                        if (menuCanvasHeading.Style.Alignment == MenuStyles.Alignment.Left)
                            (menuCanvasItem as IMenuCanvasHeading).XPos = XPos;
                        if (menuCanvasHeading.Style.Alignment == MenuStyles.Alignment.Right)
                            (menuCanvasItem as IMenuCanvasHeading).XPos = XPos+Width - menuCanvasHeading.Width;

                        #endregion

                        if (menuCanvasItem is IMenuCanvasHeading&& (menuCanvasItem as IMenuCanvasHeading).HeadingType != HeadingType.SubHeading)
                        {
                            lastFoodItemsHeading = menuCanvasItem as IMenuCanvasHeading;
                            lastFoodItemsHeading.HostingArea = null;
                        }

                    }
                    else if (menuCanvasItem is IMenuCanvasFoodItem)
                    {

                        MenuCanvasFoodItemsGroup foodItemsGroop = null;
                        if (lastFoodItemsHeading != null)
                        {
                            if (lastFoodItemsHeading.HostingArea != null)
                                foodItemsGroop = lastFoodItemsHeading.HostingArea as MenuCanvasFoodItemsGroup;
                            else
                            {
                                foodItemsGroop = new MenuCanvas.MenuCanvasFoodItemsGroup(this);
                                _FoodItemsGroup.Add(foodItemsGroop);
                                lastFoodItemsHeading.HostingArea = foodItemsGroop;
                            }
                        }
                        else
                        {
                            foodItemsGroop = new MenuCanvas.MenuCanvasFoodItemsGroup(this);
                            _FoodItemsGroup.Add(foodItemsGroop);
                        }
                        foodItemsGroop.YPos = nextMenuCanvasItemY;
                        foodItemsGroop.XPos = XPos;
                        foodItemsGroop.Width = Width;
                        foodItemsGroop.MaxHeight = YPos + MaxHeight - foodItemsGroop.YPos;
                        foodItemsGroop.AddGroupedItem(menuCanvasItem as IMenuCanvasFoodItem);
                        foodItemsGroop.RenderMenuCanvasItems();
                        while (menuCanvasItems.Count > 0 && menuCanvasItems[0] is IMenuCanvasFoodItem)
                        {
                            menuCanvasItem = menuCanvasItems[0];
                            menuCanvasItems.RemoveAt(0);
                            foodItemsGroop.AddGroupedItem(menuCanvasItem as IMenuCanvasFoodItem);
                            foodItemsGroop.RenderMenuCanvasItems();
                        }

                        nextMenuCanvasItemY = foodItemsGroop.YPos + foodItemsGroop.Height;


                    }
                }
            }
        }



        ///// <MetaDataID>{e75559b1-c248-4a15-881f-e3ba85d59783}</MetaDataID>
        //public static Size MeasureText(string text, MenuStyles.FontData font)
        //{
        //    if (font.AllCaps)
        //        text = text.ToUpper();
        //    FontFamily fontFamily = MenuStyles.FontData.FontFamilies[font.FontFamilyName];
        //    FontStyle fontStyle = (FontStyle)new FontStyleConverter().ConvertFromString(font.FontStyle);
        //    FontWeight fontWeight = (FontWeight)new FontWeightConverter().ConvertFromString(font.FontWeight);
        //    FontStretch fontStretch = FontStretches.Normal;
        //    double fontSize = font.FontSize;
        //    Typeface typeface = new Typeface(fontFamily, fontStyle, fontWeight, fontStretch);
        //    GlyphTypeface glyphTypeface;

        //    if (!typeface.TryGetGlyphTypeface(out glyphTypeface))
        //    {
        //        return MeasureTextSize(text, fontFamily, fontStyle, fontWeight, fontStretch, fontSize);
        //    }

        //    double totalWidth = 0;
        //    double height = 0;

        //    for (int n = 0; n < text.Length; n++)
        //    {
        //        ushort glyphIndex = glyphTypeface.CharacterToGlyphMap[text[n]];

        //        double width = glyphTypeface.AdvanceWidths[glyphIndex] * fontSize;

        //        double glyphHeight = glyphTypeface.AdvanceHeights[glyphIndex] * fontSize;

        //        if (glyphHeight > height)
        //        {
        //            height = glyphHeight;
        //        }

        //        totalWidth = totalWidth + width + font.FontSpacing;
        //    }
        //    totalWidth -= font.FontSpacing;
        //    return new Size(totalWidth, height);
        //}
        ///// <MetaDataID>{8f4b31ad-41b8-405f-b4c3-d34b08d1e7dc}</MetaDataID>
        //public static Size MeasureTextSize(string text, FontFamily fontFamily, FontStyle fontStyle, FontWeight fontWeight, FontStretch fontStretch, double fontSize)
        //{
        //    FormattedText ft = new FormattedText(text,
        //                                       System.Globalization.CultureInfo.CurrentCulture,
        //                                         FlowDirection.LeftToRight,
        //                                         new Typeface(fontFamily, fontStyle, fontWeight, fontStretch),
        //                                         fontSize,
        //                                         Brushes.Black);
        //    return new Size(ft.Width, ft.Height);
        //}
    }
}