using System;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;

namespace MenuPresentationModel
{
    /// <MetaDataID>{6c41c9a5-a600-4eb1-88bf-118addc97946}</MetaDataID>
    public class Style : IStyle
    {

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
        public Alignment Alignment
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
        IFontData _Font;
        /// <MetaDataID>{9e509821-e5cf-4939-bb4d-15d69d097b5b}</MetaDataID>
        [PersistentMember(nameof(_Font))]
        [BackwardCompatibilityID("+4")]
        public MenuPresentationModel.IFontData Font
        {
            get
            {
                return _Font;
            }
            set
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _Font = value; 
                    stateTransition.Consistent = true;
                }

            }
        }

      
    }
}