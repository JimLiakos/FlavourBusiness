using System;
using System.Collections.Generic;
using System.Linq;

namespace MenuPresentationModel.MenuCanvas
{
    /// <MetaDataID>{b88924fa-424f-40bf-875c-0706ae52cf4a}</MetaDataID>
    public class FoodItemsGroupColumn : IFoodItemsGroupColumn
    {
        /// <exclude>Excluded</exclude>
        OOAdvantech.Member<IMenuCanvasFoodItemsGroup> _FoodItemGroup = new OOAdvantech.Member<IMenuCanvasFoodItemsGroup>();

        /// <MetaDataID>{492f5527-5f9a-458f-96a5-166aab7e0276}</MetaDataID>
        [OOAdvantech.MetaDataRepository.ImplementationMember(nameof(_FoodItemGroup))]
        public IMenuCanvasFoodItemsGroup FoodItemGroup
        {
            get
            {
                return _FoodItemGroup.Value;
            }
            set
            {
                _FoodItemGroup.Value = value;
            }
        }

        /// <exclude>Excluded</exclude>
        double _MaxHeight;
        /// <MetaDataID>{69df2857-79d5-4489-8b35-1f8952d1b51a}</MetaDataID>
        public double MaxHeight
        {
            get
            {
                return _MaxHeight;
            }
            set
            {
                _MaxHeight = value;
            }
        }

        /// <exclude>Excluded</exclude>
        double _Width;
        /// <MetaDataID>{0db548b6-f324-488d-ba66-cda4ebb1135e}</MetaDataID>
        public double Width
        {
            get
            {
                return _Width;
            }
            set
            {
                _Width = value;
            }
        }

        /// <exclude>Excluded</exclude>
        double _XPos;
        /// <MetaDataID>{5da03ce5-013e-4550-894c-4aaf15039575}</MetaDataID>
        public double XPos
        {
            get
            {
                return _XPos;
            }
            set
            {
                _XPos = value;
            }
        }

        /// <exclude>Excluded</exclude>
        double _YPos;
        /// <MetaDataID>{664c80de-384b-4397-90df-5aa22fa0f163}</MetaDataID>
        public double YPos
        {
            get
            {
                return _YPos;
            }
            set
            {
                _YPos = value;
            }
        }


        public bool DisplayMultiPriceHeadings
        {
            get
            {
                var priceStyle = Page.Style.Styles["price-options"] as MenuStyles.IPriceStyle;
                var menuItemStyle = Page.Style.Styles["menu-item"] as MenuStyles.IMenuItemStyle;

                if  (priceStyle.ShowMultiplePrices && priceStyle.Layout == MenuStyles.PriceLayout.Normal && menuItemStyle.Alignment == MenuStyles.Alignment.Left)
                    return true;
                else if (priceStyle.ShowMultiplePrices && priceStyle.Layout == MenuStyles.PriceLayout.FollowDescription && menuItemStyle.Alignment == MenuStyles.Alignment.Left)
                    return true;
                return false;

            }
        }
        /// <MetaDataID>{24f9afa2-2671-4a31-a61f-c646c5332d98}</MetaDataID>
        public void RenderMenuCanvasItems(IList<IMenuCanvasItem> menuCanvasItems, IList<IMenuCanvasItem> allItemMultiPriceHeadings)
        {
            ItemsMultiPriceHeading itemMultiPriceHeading = null;
            List<ItemsMultiPriceHeading> usedItemsMultiPriceHeadings = new List<ItemsMultiPriceHeading>();
            double nextMenuCanvasItemY = YPos;//+= menuItemStyle.BeforeSpacing;

            foreach (var item in menuCanvasItems.ToList())
            {
                if (item is MenuCanvasFoodItem)
                {
                    MenuCanvasFoodItem foodItem = item as MenuCanvasFoodItem;
                    var pageStyle = (foodItem.Page.Style.Styles["page"] as MenuStyles.PageStyle);

                    if (DisplayMultiPriceHeadings && foodItem.MenuItem != null && foodItem.Prices.Count > 1 && foodItem.Prices[0].ItemSelection != null)
                    {

                        var itemSelectorOptionsGroup = foodItem.Prices[0].ItemSelection.OptionGroup as MenuModel.ItemSelectorOptionsGroup;
                        if (itemMultiPriceHeading == null || itemMultiPriceHeading.Source != itemSelectorOptionsGroup)
                        {
                            #region Creates Multi Price Heading 

                            itemMultiPriceHeading = (from multiPriceHeading in allItemMultiPriceHeadings.OfType<ItemsMultiPriceHeading>()
                                                     where multiPriceHeading.Source == itemSelectorOptionsGroup
                                                     select multiPriceHeading).FirstOrDefault();
                            if (itemMultiPriceHeading == null)
                            {
                                itemMultiPriceHeading = (from multiPriceHeading in allItemMultiPriceHeadings.OfType<ItemsMultiPriceHeading>()
                                                         select multiPriceHeading).FirstOrDefault();
                                if (itemMultiPriceHeading != null)
                                {
                                    allItemMultiPriceHeadings.Remove(itemMultiPriceHeading);
                                    itemMultiPriceHeading.Source = itemSelectorOptionsGroup;
                                }
                                else
                                {
                                    if (foodItem.MultiPriceHeading != null &&
                                        foodItem.MultiPriceHeading.Source == itemSelectorOptionsGroup &&
                                        !usedItemsMultiPriceHeadings.Contains(foodItem.MultiPriceHeading as ItemsMultiPriceHeading))
                                    {
                                        itemMultiPriceHeading = foodItem.MultiPriceHeading as ItemsMultiPriceHeading;
                                    }
                                    else
                                    {
                                        itemMultiPriceHeading = new ItemsMultiPriceHeading(itemSelectorOptionsGroup);
                                        OOAdvantech.PersistenceLayer.ObjectStorage objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(item);
                                        objectStorage.CommitTransientObjectState(itemMultiPriceHeading);
                                    }
                                }
                            }
                            else
                                allItemMultiPriceHeadings.Remove(itemMultiPriceHeading);

                            #endregion

                            foodItem.MultiPriceHeading = itemMultiPriceHeading;
                            var remainingFoodtItems = (from menuCanvasItem in menuCanvasItems
                                                       where menuCanvasItems.IndexOf(menuCanvasItem) >= menuCanvasItems.IndexOf(item)
                                                       select menuCanvasItem).OfType<MenuCanvas.MenuCanvasFoodItem>().ToList();

                            itemMultiPriceHeading.YPos = nextMenuCanvasItemY;
                            itemMultiPriceHeading.CalculatePricesArea(remainingFoodtItems, this);
                            usedItemsMultiPriceHeadings.Add(itemMultiPriceHeading);
                            nextMenuCanvasItemY = itemMultiPriceHeading.YPos + itemMultiPriceHeading.Height + itemMultiPriceHeading.PriceHeadingsBottomMargin;
                            nextMenuCanvasItemY = itemMultiPriceHeading.Baseline + itemMultiPriceHeading.PriceHeadingsBottomMargin;
                        }
                        else
                        {
                            if (itemMultiPriceHeading.Source == itemSelectorOptionsGroup)
                                foodItem.MultiPriceHeading = itemMultiPriceHeading;
                            nextMenuCanvasItemY += foodItem.Style.BeforeSpacing + pageStyle.LineSpacing;
                        }
                    }
                    else
                    {
                        itemMultiPriceHeading = null;
                        foodItem.MultiPriceHeading = null;
                        nextMenuCanvasItemY += foodItem.Style.BeforeSpacing + pageStyle.LineSpacing;
                    }

                    foodItem.XPos = XPos;
                    foodItem.YPos = nextMenuCanvasItemY;
                    foodItem.Width = Width;
                    foodItem.MaxHeight = MaxHeight - (foodItem.YPos - YPos);
                    foodItem.RenderMenuCanvasItems(itemMultiPriceHeading);

                    nextMenuCanvasItemY = foodItem.YPos + foodItem.Height;
                }
            }
            Height = nextMenuCanvasItemY - YPos;

        }
        /// <MetaDataID>{99dd6f64-e2be-43fe-947c-17d8464389d2}</MetaDataID>
        public double Height { get; set; }

        /// <MetaDataID>{02a6855e-cb81-43de-b4fa-4da2e5e030f4}</MetaDataID>
        public IMenuPageCanvas Page
        {
            get
            {
                return FoodItemGroup.PageColumn.Page;
            }
        }


    }
}