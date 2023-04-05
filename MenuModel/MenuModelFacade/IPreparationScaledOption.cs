using System.Collections.Generic;
using OOAdvantech.MetaDataRepository;
using OOAdvantech;

namespace MenuModel
{
    /// <MetaDataID>{b39bddb7-8b3f-447d-a8ba-6130d048cbf0}</MetaDataID>
    public interface IPreparationScaledOption : IPricedSubject, IPreparationOption
    {
        /// <MetaDataID>{6e119471-363f-4e9e-aed1-de0506416ee7}</MetaDataID>
        void RemovePreparationTag(ITag tag);
        /// <MetaDataID>{23739421-571c-415b-8a0c-ca7702eb0975}</MetaDataID>
        MenuModel.ITag NewPreparationTag();

        [Association("OtionTag", Roles.RoleA, "c46ab72d-2868-4607-b1b1-96d7bc916509")]
        [RoleBMultiplicityRange(0, 1)]
        List<ITag> PreparationTags { get; }


        /// <MetaDataID>{29efc8b3-7f6c-4747-9f54-ba3919433c30}</MetaDataID>
        [BackwardCompatibilityID("+6")]
        bool AutoGenFullName { get; set; }

        /// <MetaDataID>{e20c5be0-5e51-4fdf-a859-932d55a58b75}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        string FullName { get; set; }

        /// <MetaDataID>{089e7a6d-a78d-4769-9ba4-130ac3d42121}</MetaDataID>
        /// <summary>Defines scaling as quantitative.
        /// When quantitative attribute is true the first scaling level defines the option absence</summary>
        [BackwardCompatibilityID("+3")]
        bool Quantitative { get; set; }

        /// <summary>
        /// This option participate in recipe as ingredient when has value true.
        /// Otherwise is false 'Well-cooked steak'
        /// </summary>
        /// <MetaDataID>{e9cb48bc-7431-4a81-8a75-e9da736cf337}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        bool IsRecipeIngredient { get; set; }

        /// <MetaDataID>{e9d5c225-22f1-4e13-9862-f6fb9b186206}</MetaDataID>
        void RemoveOptionSpecificFor(IMenuItem menuItem);

        /// <MetaDataID>{2857ac05-cb51-4de0-9adc-d273893eec61}</MetaDataID>
        [BackwardCompatibilityID("+4")]
        Multilingual MultilingualName { get; }

        /// <MetaDataID>{9c848aaf-8415-4c53-a386-9ea74364ae24}</MetaDataID>
        [BackwardCompatibilityID("+5")]
        Multilingual MultilingualFullName { get; }



        [RoleBMultiplicityRange(0)]
        [RoleAMultiplicityRange(0)]
        [Association("OptionMenuItemSpecific", Roles.RoleA, "681cc9cd-c086-4449-9344-92567710b6d0")]
        [AssociationClass(typeof(IOptionMenuItemSpecific))]
        IList<IOptionMenuItemSpecific> MenuItemsOptionSpecific { get; }



        [Association("OptionsGroup", Roles.RoleB, "40a09710-0820-4640-9989-655ea652dc97")]
        IPreparationOptionsGroup OptionGroup { get; }
#if MenuPresentationModel
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


        /// <MetaDataID>{c5297bf5-7093-4e9a-8c52-7b6119f6124f}</MetaDataID>
        string Uri { get; }

    }
}
