using System.Collections.Generic;
using OOAdvantech.MetaDataRepository;
namespace MenuModel
{
    /// <MetaDataID>{b39bddb7-8b3f-447d-a8ba-6130d048cbf0}</MetaDataID>
    public interface IPreparationScaledOption : IPricedSubject, IPreparationOption
    {
        /// <summary>Defines scaling as quantitative.
        /// When quantitative attribute is true the first scalling level defines the option absence</summary>
        /// <MetaDataID>{eefdcfc2-525b-4a56-856c-5580996df7b6}</MetaDataID>
        bool Quantitative { get; set; }



        [RoleBMultiplicityRange()]
        [RoleAMultiplicityRange(0)]
        [Association("OptionMenuItemSpecific", Roles.RoleA, "681cc9cd-c086-4449-9344-92567710b6d0")]
        [AssociationClass(typeof(IOptionMenuItemSpecific))]
        IList<IOptionMenuItemSpecific> MenuItemsOptionSpecific { get; }


#if MenuPresentationModel
        [Association("OptionsGroup", Roles.RoleB, "40a09710-0820-4640-9989-655ea652dc97")]
        IPreparationOptionsGroup OptionGroup { get; }
#endif

        /// <MetaDataID>{a3ad1833-2558-4ff7-ac4e-91789058c010}</MetaDataID>
        [Association("OptionLevels", Roles.RoleA, "9ad86ca3-f76e-40e1-96bc-615c78999e5e")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction | PersistencyFlag.ReferentialIntegrity)]
        MenuModel.IScaleType LevelType
        {
            set;
            get;
        }

        /// <MetaDataID>{bd97e300-45fb-4d39-abee-b3dcf4db65bc}</MetaDataID>
        [Association("OptionInitialLevel", Roles.RoleA, "54ef027c-dd0c-4592-a946-f4482de5d1d9")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction | PersistencyFlag.ReferentialIntegrity)]
        ILevel Initial
        {
            set;
            get;
        }


        /// <MetaDataID>{d065ce53-c7dd-44b9-b49f-f21ba8399579}</MetaDataID>
        IOptionMenuItemSpecific GetMenuItemSpecific(IMenuItem menuItem);

        /// <MetaDataID>{c100a237-fcf3-49f2-9896-6b3122512165}</MetaDataID>
        ILevel GetInitialFor(IMenuItem menuItem);

        /// <MetaDataID>{54d1d914-2453-48f8-8f53-b645f1268ec8}</MetaDataID>
        void SetInitialFor(IMenuItem menuItem, ILevel initialLevel);


        /// <MetaDataID>{be5e5eac-f5ed-40b3-ae03-68e3918afeff}</MetaDataID>
        bool IsHiddenFor(IMenuItem menuItem);


        /// <MetaDataID>{f9f74c1a-7b65-48bf-931c-358341a44041}</MetaDataID>
        void SetHiddenFor(IMenuItem menuItem, bool value);

    }
}
