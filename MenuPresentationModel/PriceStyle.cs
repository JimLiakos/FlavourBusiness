using System;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;

namespace MenuPresentationModel.MenuStyles
{
    /// <MetaDataID>{3821da7a-7021-4bfd-9544-207a183abbea}</MetaDataID>
    [BackwardCompatibilityID("{3821da7a-7021-4bfd-9544-207a183abbea}")]
    [Persistent()]
    public class PriceStyle : IPriceStyle
    {


        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;



        /// <exclude>Excluded</exclude>
        bool _DisplayCurrencySymbol;

        /// <MetaDataID>{97707ffe-e54f-4892-bb84-bc97274a083c}</MetaDataID>
        [PersistentMember(nameof(_DisplayCurrencySymbol))]
        [BackwardCompatibilityID("+1")]
        public bool DisplayCurrencySymbol
        {
            get
            {
                return _DisplayCurrencySymbol;
            }

            set
            {

                if (_DisplayCurrencySymbol != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _DisplayCurrencySymbol = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        bool _DotsMatchNameColor;
        /// <MetaDataID>{1db520f7-315e-48ec-8e2e-b32416143b8a}</MetaDataID>
        [PersistentMember(nameof(_DotsMatchNameColor))]
        [BackwardCompatibilityID("+2")]
        public bool DotsMatchNameColor
        {
            get
            {
                return _DotsMatchNameColor;
            }

            set
            {
                if (_DotsMatchNameColor != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _DotsMatchNameColor = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        PriceLayout _Layout;
        /// <MetaDataID>{10c8cc40-b7c9-44a3-b131-88a48772086f}</MetaDataID>
        [PersistentMember(nameof(_Layout))]
        [BackwardCompatibilityID("+3")]
        public PriceLayout Layout
        {
            get
            {
                return _Layout;
            }

            set
            {

                if (_Layout != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Layout = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        string _Name;
        /// <MetaDataID>{eabbde66-ca29-43b0-8cc0-2fec09d42bb5}</MetaDataID>
        [PersistentMember(nameof(_Name))]
        [BackwardCompatibilityID("+4")]
        public string Name
        {
            get
            {
                return _Name;
            }

            set
            {
                if (_Name != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Name = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        string _PriceLeader;
        /// <MetaDataID>{2c8e20bf-6ee1-4601-a144-b9b8494711a0}</MetaDataID>
        [PersistentMember(nameof(_PriceLeader))]
        [BackwardCompatibilityID("+5")]
        public string PriceLeader
        {
            get
            {
                return _PriceLeader;
            }

            set
            {
                if (_PriceLeader != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _PriceLeader = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
    }
}