using System.Collections.Generic;
using OOAdvantech.MetaDataRepository;
namespace MenuModel
{
    /// <MetaDataID>{5b1dc307-2ce6-4339-ab21-a69b6c4a71ec}</MetaDataID>
    public interface IMenuItem
    {

        #if MenuPresentationModel
        /// <MetaDataID>{e34e9dca-c746-487c-95a1-561393f43e20}</MetaDataID>
        IMenu Menu { get; }
        #endif

        /// <MetaDataID>{fd794122-eeea-43b0-b247-08a283861151}</MetaDataID>
        string Description { get; set; }

        /// <MetaDataID>{9e0b699a-f983-4fc8-afd9-65e289790908}</MetaDataID>
        string ExtrasDescription { get; set; }


        /// <MetaDataID>{3e22f527-9e36-4032-8fec-1fc19ae285d8}</MetaDataID>
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
        string Name
        {
            get;
            set;
        }
    }
}
