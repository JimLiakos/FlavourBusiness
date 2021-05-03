using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace FloorLayoutDesigner
{
    /// <MetaDataID>{bcda5df8-64bd-489d-b75d-f86386625fba}</MetaDataID>
    internal class SelectionService
    {


        public event EventHandler SelectionsChanged;
        /// <MetaDataID>{833d9c4d-a31f-4eeb-8dd7-8339d145058d}</MetaDataID>
        private DesignerCanvas designerCanvas;

        /// <MetaDataID>{bd1cacb8-a3b6-4dc7-a9b1-cf6a9e4a6cb4}</MetaDataID>
        private List<ISelectable> currentSelection;
        /// <MetaDataID>{b75a44ab-3a04-40c9-bcef-a30f5d98a302}</MetaDataID>
        internal List<ISelectable> CurrentSelection
        {
            get
            {
                if (currentSelection == null)
                    currentSelection = new List<ISelectable>();

                return currentSelection;
            }
        }

        /// <MetaDataID>{f55901cb-779d-4a56-8a6f-799b0f4b738e}</MetaDataID>
        public SelectionService(DesignerCanvas canvas)
        {
            this.designerCanvas = canvas;
        }

        /// <MetaDataID>{d384b637-51ba-416a-8fc0-1fe36ea48ab6}</MetaDataID>
        internal void SelectItem(ISelectable item)
        {
            this.ClearSelection();
            this.AddToSelection(item);
        }

        /// <MetaDataID>{a6979671-d9e7-44f9-9077-cdc0596c80da}</MetaDataID>
        internal void AddToSelection(ISelectable item)
        {
            
            if (item is IGroupable)
            {
                List<IGroupable> groupItems = GetGroupMembers(item as IGroupable);

                foreach (ISelectable groupItem in groupItems)
                {
                    groupItem.IsSelected = true;
                    CurrentSelection.Add(groupItem);
                }

            }
            else
            {
                item.IsSelected = true;
                CurrentSelection.Add(item);
            }

            SelectionsChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <MetaDataID>{e972c072-93f9-4c65-9164-5dfbcddb4cf4}</MetaDataID>
        internal void RemoveFromSelection(ISelectable item)
        {

            if (item is IGroupable)
            {
                List<IGroupable> groupItems = GetGroupMembers(item as IGroupable);

                foreach (ISelectable groupItem in groupItems)
                {
                    groupItem.IsSelected = false;
                    CurrentSelection.Remove(groupItem);
                }
            }
            else
            {
                item.IsSelected = false;
                CurrentSelection.Remove(item);
            }

       


            SelectionsChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <MetaDataID>{b8f9c361-65fb-4217-9b08-670afac09738}</MetaDataID>
        internal void ClearSelection()
        {
            CurrentSelection.ForEach(item => item.IsSelected = false);
            CurrentSelection.Clear();
            SelectionsChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <MetaDataID>{7ebf91a4-7cab-4b34-aa7b-43713c9b076a}</MetaDataID>
        internal void SelectAll()
        {
            ClearSelection();
            CurrentSelection.AddRange(designerCanvas.Children.OfType<ISelectable>());
            CurrentSelection.ForEach(item => item.IsSelected = true);
            SelectionsChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <MetaDataID>{f2b450b2-7f4d-4b51-84ac-20dc4a0e694f}</MetaDataID>
        internal List<IGroupable> GetGroupMembers(IGroupable item)
        {
            IEnumerable<IGroupable> list =WPFUIElementObjectBind.ObjectContext.FindChilds<DesignerItem>(designerCanvas).OfType<IGroupable>();//designerCanvas.Children.OfType<IGroupable>()
            IGroupable rootItem = GetRoot(list, item);
            return GetGroupMembers(list, rootItem);
        }

        /// <MetaDataID>{826b74b6-d2ec-4eb6-b86c-5bec700cb140}</MetaDataID>
        internal IGroupable GetGroupRoot(IGroupable item)
        {
            IEnumerable<IGroupable> list = WPFUIElementObjectBind.ObjectContext.FindChilds<DesignerItem>(designerCanvas).OfType<IGroupable>(); //designerCanvas.Children.OfType<IGroupable>();
            return GetRoot(list, item);
        }

        /// <MetaDataID>{1e54415c-2f0e-44a1-9b06-69e2f7b666b6}</MetaDataID>
        private IGroupable GetRoot(IEnumerable<IGroupable> list, IGroupable node)
        {
            if (node == null || node.ParentID == Guid.Empty)
            {
                return node;
            }
            else
            {
                foreach (IGroupable item in list)
                {
                    if (item.ID == node.ParentID)
                    {
                        return GetRoot(list, item);
                    }
                }
                return null;
            }
        }

        /// <MetaDataID>{512d29df-fa7a-4fb4-bc49-726249e6995c}</MetaDataID>
        private List<IGroupable> GetGroupMembers(IEnumerable<IGroupable> list, IGroupable parent)
        {
            List<IGroupable> groupMembers = new List<IGroupable>();
            groupMembers.Add(parent);

            var children = list.Where(node => node.ParentID == parent.ID);

            foreach (IGroupable child in children)
            {
                groupMembers.AddRange(GetGroupMembers(list, child));
            }

            return groupMembers;
        }
    }
}
