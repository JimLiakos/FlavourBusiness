using System;
using System.Collections.Generic;
using System.Windows;
using OOAdvantech.Transactions;
using System.Linq;
using OOAdvantech.MetaDataRepository;

namespace MenuPresentationModel.MenuCanvas
{
    /// <MetaDataID>{5b385e5a-ffb8-4ec4-b7d7-314d58f23e32}</MetaDataID>
    [BackwardCompatibilityID("{5b385e5a-ffb8-4ec4-b7d7-314d58f23e32}")]
    [Persistent()]

    public class MenuCanvasFoodItemsGroup : MarshalByRefObject, IMenuCanvasFoodItemsGroup
    {

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;
        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.MultilingualSet<IGroupedItem> _GroupedItems = new OOAdvantech.Collections.Generic.MultilingualSet<IGroupedItem>();

        /// <MetaDataID>{dbd25e1d-133a-442c-9c47-8ba13f77cbd5}</MetaDataID>
        [PersistentMember(nameof(_GroupedItems))]
        [BackwardCompatibilityID("+3")]
        public IList<IGroupedItem> GroupedItems
        {
            get
            {
                return _GroupedItems.AsReadOnly();
            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.MultilingualMember<IMenuCanvasHeading> _ItemsGroupHeading = new OOAdvantech.MultilingualMember<IMenuCanvasHeading>();

        /// <MetaDataID>{68cde684-beab-4b44-a9f7-5f2a79864863}</MetaDataID>
        [PersistentMember(nameof(_ItemsGroupHeading))]
        [BackwardCompatibilityID("+2")]
        public IMenuCanvasHeading ItemsGroupHeading
        {
            get => _ItemsGroupHeading.Value;
            internal set
            {

                if (_ItemsGroupHeading != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ItemsGroupHeading.Value = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }







        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<IFoodItemsGroupColumn> _Columns = new OOAdvantech.Collections.Generic.Set<IFoodItemsGroupColumn>();

        /// <MetaDataID>{e67935d8-8bb1-4a95-bb58-4b415a1b4b77}</MetaDataID>
        [PersistentMember(nameof(_Columns))]
        [BackwardCompatibilityID("+9")]
        public System.Collections.Generic.IList<MenuPresentationModel.MenuCanvas.IFoodItemsGroupColumn> Columns
        {
            get
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    while ((ItemsGroupHeading == null && _Columns.Count > 1) || (_Columns.Count > 0 && ItemsGroupHeading != null && _Columns.Count > ItemsGroupHeading.NumberOfFoodColumns))
                    {
                        var column = _Columns[_Columns.Count - 1];
                        OOAdvantech.PersistenceLayer.ObjectStorage.DeleteObject(column);
                        _Columns.RemoveAt(_Columns.Count - 1);
                    }

                    if (ItemsGroupHeading == null && _Columns.Count == 0)
                    {
                        MenuCanvas.FoodItemsGroupColumn column = new FoodItemsGroupColumn();
                        OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(column);
                        _Columns.Add(column);
                    }
                    else
                    {
                        while (ItemsGroupHeading != null && _Columns.Count < ItemsGroupHeading.NumberOfFoodColumns)
                        {
                            MenuCanvas.FoodItemsGroupColumn column = new FoodItemsGroupColumn();
                            OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this)?.CommitTransientObjectState(column);
                            _Columns.Add(column);
                        }
                    }

                    stateTransition.Consistent = true;
                }
                var layoutStyle = PageColumn.Page.Style.Styles["layout"] as MenuStyles.LayoutStyle;


                double width = (Width - (layoutStyle.SpaceBetweenColumns * (_Columns.Count - 1))) / _Columns.Count;

                double xPos = XPos;
                foreach (var column in _Columns)
                {
                    column.XPos = xPos;
                    column.YPos = YPos;
                    column.Width = width;
                    column.MaxHeight = MaxHeight;
                    xPos += width + layoutStyle.SpaceBetweenColumns;
                }
                return _Columns.AsReadOnly();
            }
        }



        /// <exclude>Excluded</exclude>
        double _Height;

        /// <MetaDataID>{ede94873-d2e6-4e37-8413-239c1fea20c0}</MetaDataID>
        [PersistentMember(nameof(_Height))]
        [BackwardCompatibilityID("+4")]
        public double Height
        {
            get => _Height;
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



        [DeleteObjectCall]
        void OnObjectDeleting()
        {
            foreach (var foodItemsGroupColumn in _Columns.ToList())
            {

                OOAdvantech.PersistenceLayer.ObjectStorage.DeleteObject(foodItemsGroupColumn);
            }
        }


        /// <MetaDataID>{8362c7c4-5ab3-4ea3-87bf-669bbc719e42}</MetaDataID>
        public MenuCanvasFoodItemsGroup(MenuCanvasColumn menuCanvasColumn)
        {

            _PageColumn.Value = menuCanvasColumn;
        }
        /// <MetaDataID>{b347de53-4f50-44d1-b16e-4c9c73fe7a43}</MetaDataID>
        protected MenuCanvasFoodItemsGroup()
        {

        }








        /// <exclude>Excluded</exclude>
        double _MaxHeight;

        /// <MetaDataID>{92293a64-904d-463c-aee2-b8664b3dfcc7}</MetaDataID>
        [PersistentMember(nameof(_MaxHeight))]
        [BackwardCompatibilityID("+5")]
        public double MaxHeight
        {
            get => _MaxHeight;
            set
            {
                if (_MaxHeight != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _MaxHeight = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }


        /// <exclude>Excluded</exclude>
        double _Width;


        /// <MetaDataID>{d78c2426-158c-4db6-8ff7-ee6e1085bc07}</MetaDataID>
        [PersistentMember(nameof(_Width))]
        [BackwardCompatibilityID("+6")]
        public double Width
        {
            get => _Width;
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
        double _YPos;

        /// <MetaDataID>{70b9dc96-f53c-4bf5-8f44-97f2990dac08}</MetaDataID>
        [PersistentMember(nameof(_YPos))]
        [BackwardCompatibilityID("+7")]
        public double YPos
        {
            get => _YPos;
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


        /// <exclude>Excluded</exclude>
        double _XPos;
        /// <MetaDataID>{7b193984-2c91-4dc7-887c-bb89a157c3b1}</MetaDataID>
        [PersistentMember(nameof(_XPos))]
        [BackwardCompatibilityID("+8")]
        public double XPos
        {
            get => _XPos;
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





        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<IMenuCanvasLine> _SeparationLines = new OOAdvantech.Collections.Generic.Set<IMenuCanvasLine>();
        /// <MetaDataID>{45e8292c-b322-4daa-bbb8-02982ead127b}</MetaDataID>
        [PersistentMember(nameof(_SeparationLines))]
        [AssociationEndBehavior(PersistencyFlag.CascadeDelete)]
        [BackwardCompatibilityID("+10")]
        public System.Collections.Generic.IList<MenuPresentationModel.MenuCanvas.IMenuCanvasLine> SeparationLines
        {
            get
            {
                return _SeparationLines.AsReadOnly();

            }
        }


        /// <exclude>Excluded</exclude>
        OOAdvantech.Member<IMenuCanvasPageColumn> _PageColumn = new OOAdvantech.Member<IMenuCanvasPageColumn>();

        /// <MetaDataID>{6795327b-8830-47de-9d85-6093a650f1fc}</MetaDataID>
        [PersistentMember(nameof(_PageColumn))]
        [BackwardCompatibilityID("+1")]
        public IMenuCanvasPageColumn PageColumn => _PageColumn.Value;



        /// <MetaDataID>{d390e137-8a73-4b74-b4fc-5c490d84d030}</MetaDataID>
        public void AddGroupedItem(IGroupedItem item)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                if (item.HostingArea != null && item.HostingArea != this)
                    throw new ArgumentOutOfRangeException("this item belongs to another group");
                _GroupedItems.Add(item);

                stateTransition.Consistent = true;
            }

        }

        /// <MetaDataID>{0dae3f4d-01da-4283-9a29-f5d0606504ab}</MetaDataID>
        public void RemoveGroupedItem(IGroupedItem item)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _GroupedItems.Remove(item);
                stateTransition.Consistent = true;
            }
        }

        /// <MetaDataID>{2405d6b8-4d8c-4cfb-8600-89896b26778d}</MetaDataID>
        internal void RenderMenuCanvasItems(List<IMenuCanvasItem> menuCanvasItems)
        {

            //List<IMenuCanvasItem> menuCanvasItems = _GroupedItems.OfType<IMenuCanvasItem>().ToList();
            IList<IMenuCanvasItem> allItemMultiPriceHeadings = menuCanvasItems.ToList();
            foreach (IMenuCanvasFoodItem menuCanvasItem in allItemMultiPriceHeadings.OfType<IMenuCanvasFoodItem>())
            {
                if (menuCanvasItem.MultiPriceHeading != null && menuCanvasItem.MultiPriceHeading.Column != null)
                    menuCanvasItem.MultiPriceHeading.Column.RemoveMenuCanvasItem(menuCanvasItem.MultiPriceHeading);
            }
            var groupedItems = menuCanvasItems.ToList();
            List<List<IMenuCanvasItem>> columnsItems = new List<List<IMenuCanvasItem>>();
            foreach (var column in Columns)
                columnsItems.Add(new List<IMenuCanvasItem>());
            var menuCanvasItemsCanFit = menuCanvasItems.ToList();

            IList<IMenuCanvasItem> columnsMenuCanvasItems = menuCanvasItemsCanFit.ToList();
            int i = 0;
            while (columnsMenuCanvasItems.Count > 0)
            {
                int numOfItems = (int)Math.Ceiling((decimal)columnsMenuCanvasItems.Count/(columnsItems.Count-i));
                columnsItems[i].AddRange(columnsMenuCanvasItems.Take(numOfItems).ToList());
                i++;
                foreach (var menuItem in columnsMenuCanvasItems.Take(numOfItems).ToList())
                    columnsMenuCanvasItems.Remove(menuItem);
            }

            Height = 0;
            i = 0;
            bool skipNextColumns = false;
            foreach (var column in Columns)
            {
                if (skipNextColumns)
                {
                    column.RenderMenuCanvasItems(new List<IMenuCanvasItem>(), allItemMultiPriceHeadings);
                }
                else
                {
                    int itemsToRender = columnsItems[i].Count;

                    column.RenderMenuCanvasItems(columnsItems[i], allItemMultiPriceHeadings);
                    //if (itemsToRender==columnsItems[i].Count)
                    //{
                    //    skipNextColumns=true;
                    //}

                    if (column.Height > Height)
                        Height = column.Height;
                }
                if (columnsItems[i].Count>0&&i<Columns.Count-1)
                {
                    foreach (var menuItem in columnsItems[i].ToArray().Reverse().ToList())
                    {
                        columnsItems[i].Remove(menuItem);
                        columnsItems[i+1].Insert(0,menuItem);
                    }

                }
                i++;

            }


            List<IMenuCanvasItem> remainingItems = new List<IMenuCanvasItem>();

            foreach (var columnItems in columnsItems)
                remainingItems.AddRange(columnItems);
            foreach (var menuCanvasItem in menuCanvasItems.ToList())
            {
                if (!remainingItems.Contains(menuCanvasItem))
                    menuCanvasItems.Remove(menuCanvasItem);
            }


            #region sort page menu canvas items
            if (Columns.Count > 0)
            {
                var page = PageColumn.Page;

                foreach (var goupedItem in groupedItems)
                {
                    if (!menuCanvasItems.Contains(goupedItem))
                        page.RemoveMenuItem(goupedItem);
                }
                foreach (var goupedItem in groupedItems)
                {
                    if (!menuCanvasItems.Contains(goupedItem))
                        page.AddMenuItem(goupedItem);
                }
            }
            #endregion


            foreach (var goupedItem in groupedItems)
            {
                if (!menuCanvasItems.Contains(goupedItem))
                    AddGroupedItem(goupedItem as IGroupedItem);
            }

            RenderSeparationLines();

            //double nextMenuCanvasItemY = YPos;//+= menuItemStyle.BeforeSpacing;


            //foreach (var item in _GroupedItems)
            //{
            //    if (item is MenuCanvasFoodItem)
            //    {
            //        MenuCanvasFoodItem foodItem = item as MenuCanvasFoodItem;
            //        var pageStyle = (foodItem.Page.Style.Styles["page"] as MenuStyles.PageStyle);
            //        nextMenuCanvasItemY += foodItem.Style.BeforeSpacing+ pageStyle.LineSpacing;
            //        foodItem.XPos = XPos;
            //        foodItem.YPos = nextMenuCanvasItemY;
            //        foodItem.Width = Width;
            //        foodItem.MaxHeight = MaxHeight - (foodItem.YPos - YPos);
            //        foodItem.RenderMenuCanvasItems();

            //        nextMenuCanvasItemY = foodItem.YPos + foodItem.Height;
            //    }
            //}
            //Height = nextMenuCanvasItemY - YPos;
        }
        /// <MetaDataID>{d356e403-6923-4bc4-a962-d24a2eda7365}</MetaDataID>
        private void RenderSeparationLines()
        {
            var existingSeparationLines = _SeparationLines.ToList();

            var layoutStyle = (PageColumn.Page.Style.Styles["layout"] as MenuStyles.LayoutStyle);
            if (layoutStyle.LineBetweenColumns)
            {
                List<IMenuCanvasLine> separationLiness = MenuCanvasColumn.RenderSeparationLines(layoutStyle, _Columns.OfType<IMenuCanvasColumn>().ToList());
                foreach (var separationline in separationLiness)
                {
                    if (existingSeparationLines.Count > 0)
                    {
                        IMenuCanvasLine line = existingSeparationLines[0];
                        existingSeparationLines.RemoveAt(0);
                        line.X1 = separationline.X1;
                        line.X2 = separationline.X2;
                        line.Y1 = separationline.Y1;
                        line.Y2 = separationline.Y2;
                        line.Stroke = separationline.Stroke;
                        double strockThickness = layoutStyle.SeparationLineThickness;
                        line.StrokeThickness = separationline.StrokeThickness;
                        line.LineType = layoutStyle.SeparationLineType;

                    }
                    else
                    {
                        OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(separationline);
                        _SeparationLines.Add(separationline);
                    }
                }
            }


            foreach (var line in existingSeparationLines)
                _SeparationLines.Remove(line);
        }

    }
}