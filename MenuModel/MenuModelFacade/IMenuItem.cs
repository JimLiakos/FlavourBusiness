using System.Collections.Generic;
using OOAdvantech.MetaDataRepository;
using OOAdvantech;

namespace MenuModel
{
    /// <MetaDataID>{5b1dc307-2ce6-4339-ab21-a69b6c4a71ec}</MetaDataID>
    public interface IMenuItem 
    { 
        /// <MetaDataID>{befa463c-cd86-40cc-8e18-585b593f3b74}</MetaDataID>
        [BackwardCompatibilityID("+13")]
        string ItemInfo { get; set; }

        /// <MetaDataID>{e5b82955-f736-45f9-8ad9-488d0759a9ba}</MetaDataID>
        [BackwardCompatibilityID("+12")]
        bool SelectorAlwaysInDescription { get; set; }

        /// <MetaDataID>{c18d15b6-1506-4e96-8ee0-91658db67cd9}</MetaDataID>
        void RemoveMealType(IPartofMeal partofMeal);

        /// <MetaDataID>{7f074056-24ab-40b2-85cf-149ae599e64d}</MetaDataID>
        IPartofMeal AddMealType(MenuModel.IMealType mealType, MenuModel.IMealCourseType mealCourseType);

        [RoleAMultiplicityRange(0)]
        [AssociationClass(typeof(IPartofMeal))]
        [Association("PartofMeal", Roles.RoleA, "be244893-c036-42d9-a168-ff65d8ef16a1")]
        IList<IPartofMeal> PartofMeals { get;  }

        /// <MetaDataID>{985afb25-281d-4fcd-a81d-da14d033bf81}</MetaDataID>
        [BackwardCompatibilityID("+11")]
        bool Stepper { get; set; }

        /// <MetaDataID>{91a02cb0-e244-4885-bdeb-2cc79f7569ea}</MetaDataID>
        [BackwardCompatibilityID("+10")]
        string PromptForCustom { get; set; }

        /// <MetaDataID>{519c2e81-96fe-4f79-b3e7-640659dd7d9f}</MetaDataID>
        [BackwardCompatibilityID("+9")]
        string PromptForDefault { get; set; }

        /// <MetaDataID>{4cf6ac43-bd78-4a46-9a0e-3a3db3d939a8}</MetaDataID>
        [BackwardCompatibilityID("+8")]
        bool AllowCustom { get; set; }

        /// <MetaDataID>{32e3f727-8ffd-4f08-bebf-788bd3643274}</MetaDataID>
        [BackwardCompatibilityID("+7")]
        string FullName { get; set; }

#if MenuPresentationModel
        /// <MetaDataID>{e34e9dca-c746-487c-95a1-561393f43e20}</MetaDataID>
        IMenu Menu { get; }
#endif

        /// <MetaDataID>{fd794122-eeea-43b0-b247-08a283861151}</MetaDataID>
        [BackwardCompatibilityID("+3")]
        string Description { get; set; }

        /// <MetaDataID>{9e0b699a-f983-4fc8-afd9-65e289790908}</MetaDataID>
        [BackwardCompatibilityID("+4")]
        string ExtrasDescription { get; set; }


        /// <MetaDataID>{3e22f527-9e36-4032-8fec-1fc19ae285d8}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        IMenuItemPrice MenuItemPrice { get; }


        [RoleBMultiplicityRange(0)]
        [Association("OptionMenuItemSpecific", Roles.RoleB, "681cc9cd-c086-4449-9344-92567710b6d0")]
        [AssociationClass(typeof(IOptionMenuItemSpecific))]
        IList<IOptionMenuItemSpecific> OptionsMenuItemSpecifics { get; }



        /// <MetaDataID>{a8b2c471-8d52-4f40-ba3f-30d68e9ab6ef}</MetaDataID>
        [Association("ItemPrice", Roles.RoleA, "0964beb4-bfb2-475c-a1d7-5ba113b512ce")]
        [RoleAMultiplicityRange(1)]
        IList<IMenuItemPrice> Prices { get; }

        [AssociationEndBehavior(PersistencyFlag.CascadeDelete)]
        [Association("ItemDedicatedType", Roles.RoleA, "75c3099a-9485-47db-8755-e64d984a7297", "acf70d37-a943-4b51-a02f-e012fdac9f03")]
        IMenuItemType DedicatedType { get; set; }


        

        /// <MetaDataID>{761fd0c4-bc7d-4dc5-8a41-84b4cf03a470}</MetaDataID>
        void AddMenuItemPrice(IMenuItemPrice price);

        /// <MetaDataID>{283d0a3f-6b88-4b5c-85d0-aa49ecfe722b}</MetaDataID>
        void RemoveMenuItemPrice(IMenuItemPrice price);


        /// <MetaDataID>{a0d3abe9-a660-4ce4-bd8e-589828680f38}</MetaDataID>
        void AddType(IMenuItemType type);

        /// <MetaDataID>{dcf7f5ac-6353-4397-8e8e-f01bdc34c284}</MetaDataID>
        void RemoveType(IMenuItemType type);

        [Association("ItemTypes", Roles.RoleA, "acf70d37-a943-4b51-a02f-e012fdac9f03")]
        IList<IMenuItemType> Types { get; }

        /// <MetaDataID>{02857345-ec00-4075-89e0-0b81e5f2f22a}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        string Name
        {
            get;
            set;
        }

        /// <MetaDataID>{6616342b-566f-4c8a-8718-7df8b7bc6212}</MetaDataID>
        Multilingual MultilingualName { get; }

        /// <MetaDataID>{a543ef12-f032-499e-b578-90752cc14625}</MetaDataID>
        Multilingual MultilingualFullName { get; }


        /// <MetaDataID>{9916abd7-a7fb-4045-8a7d-f7db2d44e884}</MetaDataID>
        Multilingual MultilingualPromptForCustom { get; }

        /// <MetaDataID>{147d81c1-d772-4afe-b65a-735adaafac58}</MetaDataID>
        Multilingual MultilingualPromptForDefault { get; }

        /// <MetaDataID>{894bb958-1c87-41a9-800b-9b824a328051}</MetaDataID>
        Multilingual MultilingualDescription { get; }


        /// <MetaDataID>{beef7cf6-a213-4794-8c50-fb8974ac186e}</MetaDataID>
        Multilingual MultilingualExtrasDescription { get; }

        Multilingual MultilingualItemInfo { get;}


    }
}
