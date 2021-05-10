using System;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;

namespace MenuModel
{
    /// <MetaDataID>{0ef7eb5d-f096-4a8f-bad0-79d3f768e38a}</MetaDataID>
    [BackwardCompatibilityID("{0ef7eb5d-f096-4a8f-bad0-79d3f768e38a}")]
    [Persistent()]
    public class OptionMenuItemSpecific : IOptionMenuItemSpecific
    {

        /// <MetaDataID>{6dea3b70-846a-43da-962d-c624e1b10d62}</MetaDataID>
        protected OptionMenuItemSpecific()
        {
        }

        /// <MetaDataID>{8112e92c-ed46-4455-afbe-9493ee1174bc}</MetaDataID>
        public OptionMenuItemSpecific(IMenuItem menuItem, IPreparationScaledOption option)
        {
            _Option = option;
            _MenuItemOptionSpecific = menuItem;
        }
        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;


        /// <exclude>Excluded</exclude>
        IPreparationScaledOption _Option;
        /// <MetaDataID>{153f171d-0019-4433-940a-0515228149f8}</MetaDataID>
        [AssociationClassRole(Roles.RoleB, nameof(_Option))]
        [BackwardCompatibilityID("")]
        public MenuModel.IPreparationScaledOption Option
        {
            set
            {

            }
            get
            {
                return _Option;
            }
        }



        /// <exclude>Excluded</exclude>
        IMenuItem _MenuItemOptionSpecific;
        /// <MetaDataID>{5f8dcedb-d5f0-42f5-93cf-b0b5b7e586ef}</MetaDataID>
        [AssociationClassRole(Roles.RoleA,nameof(_MenuItemOptionSpecific))]
        [BackwardCompatibilityID("+3")]
        public IMenuItem MenuItemOptionSpecific
        {
            get
            {
                return _MenuItemOptionSpecific;
            }
        }

        /// <exclude>Excluded</exclude>
        ILevel _InitialLevel;


        /// <MetaDataID>{02050237-ae4c-4238-ba63-e888bfbe0e32}</MetaDataID>
        [PersistentMember("_InitialLevel")]
        [BackwardCompatibilityID("+1")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        public MenuModel.ILevel InitialLevel
        {
            get
            {
                return _InitialLevel;
            }

            set
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _InitialLevel = value;
                    stateTransition.Consistent = true;
                }

            }
        }
        /// <exclude>Excluded</exclude>
        bool _Hide;
        /// <MetaDataID>{f70c39fa-404c-4a5a-bce7-8846ce453e83}</MetaDataID>
        [PersistentMember(nameof(_Hide))]
        [BackwardCompatibilityID("+4")]
        public bool Hide
        {
            get
            {
                return _Hide;
            }
            set
            {
                if (_Hide != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Hide = value;
                        if (_Hide)
                            _InitialLevel = null;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
    }
}