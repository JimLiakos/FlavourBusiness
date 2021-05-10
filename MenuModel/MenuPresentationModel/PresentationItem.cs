using System;
using System.Collections.Generic;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Transactions;

namespace MenuPresentationModel
{
    /// <MetaDataID>{aeec2831-c6d2-419c-b910-961dc5e6f902}</MetaDataID>
    [BackwardCompatibilityID("{aeec2831-c6d2-419c-b910-961dc5e6f902}")]
    [Persistent()]
    public class PresentationItem  : MarshalByRefObject, IObjectStateEventsConsumer
    {

        ///// <exclude>Excluded</exclude>
        ///// <MetaDataID>{f0731828-faf1-451d-afdd-0d158478f62e}</MetaDataID>
        //bool _IsGroup;
        ///// <MetaDataID>{1555cc71-4a22-4f93-82cf-80b6fbe32d9a}</MetaDataID>
        //[PersistentMember("_IsGroup")]
        //[BackwardCompatibilityID("+9")]
        //public bool IsGroup
        //{
        //    get
        //    {
        //        return _IsGroup;
        //    }

        //    set
        //    {
        //        if (_IsGroup != value)
        //        {
        //            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
        //            {
        //                _IsGroup = value;
        //                stateTransition.Consistent = true;
        //            }
        //        }
        //    }
        //}

        ///// <exclude>Excluded</exclude>
        //Guid _ParentID;
        ///// <MetaDataID>{3f30c2e0-e2a0-4cd2-a421-b782669d2d9c}</MetaDataID>
        //[PersistentMember("_ParentID")]
        //[BackwardCompatibilityID("+8")]
        //public Guid ParentID
        //{
        //    get
        //    {
        //        return _ParentID;
        //    }

        //    set
        //    {
        //        if (_ParentID != value)
        //        {
        //            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
        //            {
        //                _ParentID = value;
        //                stateTransition.Consistent = true;
        //            }
        //        }
        //    }
        //}


        /// <exclude>Excluded</exclude>
        Guid _ID;
        /// <MetaDataID>{95a918b7-c466-4676-9a15-294ff80fb68c}</MetaDataID>
        [PersistentMember("_ID")]
        [BackwardCompatibilityID("+7")]
        public Guid ID
        {
            get
            {
                return _ID;
            }

            set
            {
                if (_ID != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ID = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{3c9ad117-0156-4782-99f6-994ceede0a00}</MetaDataID>
        string _PageContentType;

        /// <MetaDataID>{10ab3974-8c6d-4fee-94ca-2793dc3976ee}</MetaDataID>
        [PersistentMember("_PageContentType")]
        [BackwardCompatibilityID("+6")]
        public string PageContentType
        {
            set
            {
                if(_PageContentType!=value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _PageContentType = value; 
                        stateTransition.Consistent = true;
                    }
                }
            }
            get
            {
                return _PageContentType;
            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;


        ///// <exclude>Excluded</exclude>
        ///// <MetaDataID>{4965e946-9ca3-4b90-ba70-2527b86af605}</MetaDataID>
        //int _ZIndex;
        ///// <MetaDataID>{55ecd2b2-0ab3-4d9f-8a7f-6f84982fcc08}</MetaDataID>
        //[PersistentMember("_ZIndex")]
        //[BackwardCompatibilityID("+5")]
        //public int ZIndex
        //{
        //    get
        //    {
        //        return _ZIndex;
        //    }
        //    set
        //    {

        //        using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
        //        {
        //            _ZIndex = value; 
        //            stateTransition.Consistent = true;
        //        }

        //    }
        //}

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{878a6658-385e-4954-871e-97eda52ad3bb}</MetaDataID>
        double _Height;
        /// <MetaDataID>{2acf8402-dcbc-408e-aff7-c03a103f095b}</MetaDataID>
        [PersistentMember("_Height")]
        [BackwardCompatibilityID("+4")]
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

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{d6eb2dec-cf12-4c8d-9fc9-a8d7903c5c0b}</MetaDataID>
        double _Width;
        /// <MetaDataID>{c411153f-cb48-4130-bd07-33955e24f531}</MetaDataID>
        [PersistentMember("_Width")]
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
        /// <MetaDataID>{b33d7338-c467-4ce9-8dff-96183fa21b4a}</MetaDataID>
        double _Left;
        /// <MetaDataID>{86df4305-8332-4625-bb04-01a215d8e029}</MetaDataID>
        [PersistentMember("_Left")]
        [BackwardCompatibilityID("+2")]
        public double Left
        {
            get
            {
                return _Left;
            }

            set
            {
                if (_Left != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Left = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

    

        /// <MetaDataID>{65be278e-32c2-4b88-8eb0-a8d40d94e33b}</MetaDataID>
        public PresentationItem()
        {

        }


        public PresentationItem(PresentationItem copy)        {

            _Top = copy._Top;
            _Left = copy._Left;
            _Height = copy.Height;
            _Width = copy._Width;
            _PageContentType = copy._PageContentType;
            _ID = Guid.NewGuid();

        }

    /// <exclude>Excluded</exclude>
        /// <MetaDataID>{5654ddf1-fbe1-4709-bd71-5be491d0daa5}</MetaDataID>
        double _Top;
  
        /// <MetaDataID>{f5193c63-103f-48ca-a150-4f50744351bb}</MetaDataID>
        [PersistentMember("_Top")]
        [BackwardCompatibilityID("+1")]
        public double Top
        {
            get
            {
                return _Top;
            }
            set
            {
                if (_Top != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Top = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        public bool IsSelected { get; set; }

        public virtual PresentationItem Copy(Dictionary<object,object> copiedObjects)
        {
            PresentationItem copy = new PresentationItem(this);
            copiedObjects[this] = copy;
            return copy;
        }

        public virtual void OnCommitObjectState()
        {
        }
        public virtual void OnActivate()
        {
        }

        public virtual void OnDeleting()
        {
        }

        public void LinkedObjectAdded(object linkedObject, AssociationEnd associationEnd)
        {
            
        }

        public void LinkedObjectRemoved(object linkedObject, AssociationEnd associationEnd)
        {
            
        }
    }
}