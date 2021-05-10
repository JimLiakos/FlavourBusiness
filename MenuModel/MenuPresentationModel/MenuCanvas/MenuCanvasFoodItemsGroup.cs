using System;
using System.Collections.Generic;
using System.Windows;
using OOAdvantech.Transactions;

namespace MenuPresentationModel.MenuCanvas
{
    /// <MetaDataID>{5b385e5a-ffb8-4ec4-b7d7-314d58f23e32}</MetaDataID>
    [Transactional]
    public class MenuCanvasFoodItemsGroup : MarshalByRefObject, IMenuCanvasFoodItemsGroup
    {
        /// <MetaDataID>{ede94873-d2e6-4e37-8413-239c1fea20c0}</MetaDataID>
        public double Height { get; set; }

        /// <MetaDataID>{8362c7c4-5ab3-4ea3-87bf-669bbc719e42}</MetaDataID>
        public MenuCanvasFoodItemsGroup(MenuCanvasColumn menuCanvasColumn)
        {
            _Column = menuCanvasColumn;
        }
        /// <MetaDataID>{b347de53-4f50-44d1-b16e-4c9c73fe7a43}</MetaDataID>
        protected MenuCanvasFoodItemsGroup()
        {

        }
        /// <exclude>Excluded</exclude>
        IMenuCanvasColumn _Column;
        /// <MetaDataID>{6795327b-8830-47de-9d85-6093a650f1fc}</MetaDataID>
        public IMenuCanvasColumn Column
        {
            get
            {
                return _Column;
            }
        }


        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<IGroupedItem> _GroupedItems = new OOAdvantech.Collections.Generic.Set<IGroupedItem>();

        /// <MetaDataID>{dbd25e1d-133a-442c-9c47-8ba13f77cbd5}</MetaDataID>
        [OOAdvantech.MetaDataRepository.ImplementationMember(nameof(_GroupedItems))]
        public IList<IGroupedItem> GroupedItems
        {
            get
            {
                return _GroupedItems.AsReadOnly();
            }
        }

        /// <MetaDataID>{68cde684-beab-4b44-a9f7-5f2a79864863}</MetaDataID>
        public IMenuCanvasHeading ItemsGroupHeading
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <MetaDataID>{92293a64-904d-463c-aee2-b8664b3dfcc7}</MetaDataID>
        public double MaxHeight { get; set; }

        /// <MetaDataID>{d78c2426-158c-4db6-8ff7-ee6e1085bc07}</MetaDataID>
        public double Width { get; set; }

        /// <MetaDataID>{70b9dc96-f53c-4bf5-8f44-97f2990dac08}</MetaDataID>
        public double YPos { get; set; }

        /// <MetaDataID>{7b193984-2c91-4dc7-887c-bb89a157c3b1}</MetaDataID>
        public double XPos { get; set; }

        /// <MetaDataID>{d390e137-8a73-4b74-b4fc-5c490d84d030}</MetaDataID>
        public void AddGroupedItem(IGroupedItem item)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
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
        internal void RenderMenuCanvasItems()
        {
            

            double nextMenuCanvasItemY = YPos;//+= menuItemStyle.BeforeSpacing;
            foreach (var item in _GroupedItems)
            {
                if (item is MenuCanvasFoodItem)
                {
                    MenuCanvasFoodItem foodItem = item as MenuCanvasFoodItem;
                    var pageStyle = (foodItem.Page.Style.Styles["page"] as MenuStyles.PageStyle);
                    nextMenuCanvasItemY += foodItem.Style.BeforeSpacing+ pageStyle.LineSpacing;
                    foodItem.XPos = XPos;
                    foodItem.YPos = nextMenuCanvasItemY;
                    foodItem.Width = Width;
                    foodItem.MaxHeight = MaxHeight - (foodItem.YPos - YPos);
                    foodItem.RenderMenuCanvasItems();

                    nextMenuCanvasItemY = foodItem.YPos + foodItem.Height;
                }
            }
            Height = nextMenuCanvasItemY - YPos;
        }

    }
}