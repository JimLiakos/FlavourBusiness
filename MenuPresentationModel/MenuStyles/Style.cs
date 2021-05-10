using System;
using OOAdvantech;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using UIBaseEx;

namespace MenuPresentationModel.MenuStyles
{
    /// <MetaDataID>{6c41c9a5-a600-4eb1-88bf-118addc97946}</MetaDataID>
    [BackwardCompatibilityID("{6c41c9a5-a600-4eb1-88bf-118addc97946}")]
    [Persistent()]
    public class Style : IStyleRule, OOAdvantech.PersistenceLayer.IObjectStateEventsConsumer
    {
        public bool IsDerivedStyle
        {
            get
            {
                    return false;
            }
        }

        /// <MetaDataID>{d5d15bbf-a7b8-4a92-8d38-7ad5cf9f13e3}</MetaDataID>
        public virtual IStyleRule GetDerivedStyle()
        {
            throw new NotImplementedException();
        }
        /// <MetaDataID>{abb9d38a-73d1-4e10-8a0d-d4844f1e4a2b}</MetaDataID>
        public void ChangeOrgStyle(IStyleRule style)
        {
        }
        public void OnCommitObjectState()
        {
        }
        public void BeforeCommitObjectState()
        {
        }
        public void OnActivate()
        {
        }
        public void OnDeleting()
        {
        }
        public void LinkedObjectAdded(object linkedObject, AssociationEnd associationEnd)
        {
        }
        public void LinkedObjectRemoved(object linkedObject, AssociationEnd associationEnd)
        {
        }
        /// <exclude>Excluded</exclude>
        IStyleSheet _StyleSheet;
        /// <MetaDataID>{a30d84c9-ec68-4113-b814-9caae4fe537a}</MetaDataID>
        [PersistentMember("_StyleSheet")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [BackwardCompatibilityID("+5")]
        [OOAdvantech.Json.JsonIgnore]
        public MenuPresentationModel.MenuStyles.IStyleSheet StyleSheet
        {
            get
            {
                return _StyleSheet;
            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <exclude>Excluded</exclude>
        double _AfterSpacing;
        /// <MetaDataID>{406b7757-6819-4ccc-8e93-5df3dba966f8}</MetaDataID>
        [PersistentMember(nameof(_AfterSpacing))]
        [BackwardCompatibilityID("+1")]
        public double AfterSpacing
        {
            get
            {
                return _AfterSpacing;
            }

            set
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _AfterSpacing = value;
                    stateTransition.Consistent = true;
                }

            }
        }


        /// <exclude>Excluded</exclude>
        Alignment _Alignment;
        /// <MetaDataID>{635a9e30-130f-435e-ae73-a6f9e3858804}</MetaDataID>
        [PersistentMember(nameof(_Alignment))]
        [BackwardCompatibilityID("+2")]
        public MenuPresentationModel.MenuStyles.Alignment Alignment
        {
            get
            {
                return _Alignment;
            }

            set
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _Alignment = value;
                    stateTransition.Consistent = true;
                }

            }
        }


        /// <exclude>Excluded</exclude>
        double _BeforeSpacing;
        /// <MetaDataID>{a85673b9-3406-44e4-b891-c88a5f4f9901}</MetaDataID>
        [PersistentMember(nameof(_BeforeSpacing))]
        [BackwardCompatibilityID("+3")]
        public double BeforeSpacing
        {
            get
            {
                return _BeforeSpacing;
            }

            set
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _BeforeSpacing = value;
                    stateTransition.Consistent = true;
                }

            }
        }


        /// <exclude>Excluded</exclude>
        FontData _Font;

        public event ObjectChangeStateHandle ObjectChangeState;

        /// <MetaDataID>{9e509821-e5cf-4939-bb4d-15d69d097b5b}</MetaDataID>
        [PersistentMember(nameof(_Font))]
        [BackwardCompatibilityID("+4")]
        public FontData Font
        {
            get
            {
                return _Font;
            }
            set
            {
                if (_Font != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Font = value;
                        stateTransition.Consistent = true;
                    }
                    ObjectChangeState?.Invoke(this, nameof(Font));
                }
                

            }
        }

        /// <MetaDataID>{1efa6bc0-2cc9-44cd-8aff-c04af53d99f3}</MetaDataID>
        public string Name
        {
            get
            {
                return null;
            }

            set
            {
                throw new NotImplementedException();
            }

        }
    }
}