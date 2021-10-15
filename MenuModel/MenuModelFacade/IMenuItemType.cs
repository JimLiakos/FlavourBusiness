using System.Collections.Generic;
using OOAdvantech.MetaDataRepository;
namespace MenuModel
{

    public delegate void PreparationOptionAddedHandle(IMenuItemType menuType, IPreparationOption preparationOption);
    public delegate void PreparationOptionRemovedHandle(IMenuItemType menuType, IPreparationOption preparationOption);
    /// <MetaDataID>{e2352da1-a028-4f5c-980b-0403e38510c5}</MetaDataID>
    public interface IMenuItemType
    {
        [Association("ItemTypeTags", Roles.RoleA, "9d978529-a864-4ddb-a62f-c825741b81a5")]
        [RoleAMultiplicityRange(1)]
        [RoleBMultiplicityRange(0, 1)]
        List<ITag> PreparationTags { get; }


        /// <MetaDataID>{b8612e4e-dce3-4855-8e2d-a3b689f6f55d}</MetaDataID>
        ITag NewPreparationTag( );

        /// <MetaDataID>{c6076c60-d729-4c5d-8dd8-0d3f76d3a75b}</MetaDataID>
        void RemovePreparationTag(ITag tag);


        /// <MetaDataID>{46fd7d70-aaf8-4ef8-a073-36b5e55e9c0e}</MetaDataID>
        void MovePreparationOption(IPreparationOption preparationOption, int newpos);

        [Association("ItemDedicatedType", Roles.RoleB, "75c3099a-9485-47db-8755-e64d984a7297")]
        IMenuItem Owner { get; }

        [AssociationEndBehavior(PersistencyFlag.ReferentialIntegrity | PersistencyFlag.CascadeDelete)]
        [Association("ItemTypePreparation", Roles.RoleA, true, "102fcd8d-cc58-42cd-8dcc-4bd51cd424e9"), OOAdvantech.MetaDataRepository.RoleAMultiplicityRange(1)]
        IList<MenuModel.IPreparationOption> Options
        { 
            get;
        }
        /// <MetaDataID>{f8617cc8-9df2-44ba-8be2-65228da3d7fd}</MetaDataID>
        string Name
        {
            get;
            set;
        }
        /// <MetaDataID>{8dcebe1e-202b-4aa9-ad54-d178516eb79f}</MetaDataID>
        List<IPricingContext> PricingContexts { get; }

        /// <MetaDataID>{fbb0a37f-f0f2-4b2c-a09e-07c1040680e7}</MetaDataID>
        void AddPreparationOption(IPreparationOption option);


        /// <MetaDataID>{34e49292-915d-45d0-b03f-4f9d48da4f2c}</MetaDataID>
        void RemovePreparationOption(IPreparationOption option);

        event PreparationOptionAddedHandle PreparationOptionAdded;

        event PreparationOptionRemovedHandle PreparationOptionRemoved;


    }
}
