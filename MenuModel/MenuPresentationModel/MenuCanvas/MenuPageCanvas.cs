using System;
using System.Collections.Generic;
using MenuPresentationModel.MenuStyles;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;

namespace MenuPresentationModel.MenuCanvas
{
    /// <MetaDataID>{9bd65116-ca24-4ab8-b4b4-b92d13416f39}</MetaDataID>
    [BackwardCompatibilityID("{9bd65116-ca24-4ab8-b4b4-b92d13416f39}")]
    [Persistent()]
    public class MenuPageCanvas : IMenuPageCanvas
    {
        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;


        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<IMenuCanvasColumn> _Columns = new OOAdvantech.Collections.Generic.Set<IMenuCanvasColumn>();

        /// <MetaDataID>{c75bca93-afad-41ec-9725-4f09bfff2a5d}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        [ImplementationMember(nameof(_Columns))]
        public System.Collections.Generic.IList<MenuPresentationModel.MenuCanvas.IMenuCanvasColumn> Columns
        {
            get
            {
                return _Columns.AsReadOnly();
            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<IMenuCanvasItem> _MenuCanvasItems = new OOAdvantech.Collections.Generic.Set<IMenuCanvasItem>();

        /// <MetaDataID>{c5b5fcce-af72-4bd7-995d-fc3709d3d3dd}</MetaDataID>
        [PersistentMember(nameof(_MenuCanvasItems))]
        [BackwardCompatibilityID("+2")]
        public System.Collections.Generic.IList<MenuPresentationModel.MenuCanvas.IMenuCanvasItem> MenuCanvasItems
        {
            get
            {
                return _MenuCanvasItems.AsReadOnly();
            }
        }

        /// <exclude>Excluded</exclude>
        int _NumberofColumns;
        /// <MetaDataID>{ef01528c-f2c4-4a1c-90aa-d5026858203f}</MetaDataID>
        [PersistentMember(nameof(_NumberofColumns))]
        [BackwardCompatibilityID("+3")]
        public int NumberofColumns
        {
            get
            {
                return _NumberofColumns;
            }

            set
            {
                if (_NumberofColumns != value)
                {

                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _NumberofColumns = value;
                        stateTransition.Consistent = true;
                    }

                }
            }
        }

        IStyleSheet _Style;
        public IStyleSheet Style
        {
            get
            {
                return _Style;
            }

            set
            {
                _Style = value;
            }
        }

        /// <MetaDataID>{db14bf4b-4732-4e24-86d8-0690aa1fb892}</MetaDataID>
        public void AddMenuItem(IMenuCanvasItem manuCanvasitem)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _MenuCanvasItems.Add(manuCanvasitem);
                stateTransition.Consistent = true;
            }

        }

        /// <MetaDataID>{62fed454-5350-4877-97e8-6df61ef9fbc7}</MetaDataID>
        public void Delete(IMenuCanvasItem manuCanvasitem)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _MenuCanvasItems.Remove(manuCanvasitem);
                stateTransition.Consistent = true;
            }
        }

        /// <MetaDataID>{1b863489-fece-4947-9a93-903b8e1d7f99}</MetaDataID>
        public void InsertMenuItem(int pos, IMenuCanvasItem manuCanvasitem)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _MenuCanvasItems.Insert(pos, manuCanvasitem);
                stateTransition.Consistent = true;
            }
        }

        /// <MetaDataID>{5eefc7c6-f51b-434f-ba07-7490d3c536cd}</MetaDataID>
        public void InsertMenuItemAfter(IMenuCanvasItem manuCanvasitem, IMenuCanvasItem newManuCanvasitem)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                int pos = _MenuCanvasItems.IndexOf(manuCanvasitem);
                _MenuCanvasItems.Insert(pos, newManuCanvasitem);
                stateTransition.Consistent = true;
            }
        }

        /// <MetaDataID>{8d131ee2-3237-491b-bac6-514112dc30f1}</MetaDataID>
        public void MoveMenuItem(MenuPresentationModel.MenuCanvas.IMenuCanvasItem manuCanvasItem, int newpos)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _MenuCanvasItems.Remove(manuCanvasItem);
                _MenuCanvasItems.Insert(newpos, manuCanvasItem);
                stateTransition.Consistent = true;
            }
        }
    }
}