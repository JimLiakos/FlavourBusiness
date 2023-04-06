using OOAdvantech;
using OOAdvantech.MetaDataRepository;

namespace FlavourBusinessFacade.RoomService
{
    /// <MetaDataID>{e829df38-0b19-40d1-9211-9f95fb470174}</MetaDataID>
    [BackwardCompatibilityID("{e829df38-0b19-40d1-9211-9f95fb470174}")]
    public interface IOptionChange
    {
        /// <MetaDataID>{dde5aaa3-d498-4db2-a4a2-85349c97dc98}</MetaDataID>
        [BackwardCompatibilityID("+6")]
        bool NotifiedUnCheckedOption { get; set; }

        /// <MetaDataID>{9c88e934-f93d-4e85-ac88-a786c5231269}</MetaDataID>
        [BackwardCompatibilityID("+3")]
        string Description { get; set; }

        /// <MetaDataID>{783d5688-e77b-4b50-bca0-9f4d14e247cc}</MetaDataID>
        [BackwardCompatibilityID("+4")]
        Multilingual MultilingualDescription { get; }


        /// <MetaDataID>{954c40fa-2992-4bf9-bf08-31fe883a8f83}</MetaDataID>
        [Association("ItemPreparationOptionsChange", Roles.RoleB, "8e564693-11c5-445c-8cb3-1f9505a2f2d2")]
        [RoleBMultiplicityRange(1, 1)]
        IItemPreparation ItemPreparation { get; set; }

#if DeviceDotNet
        /// <MetaDataID>{5f676077-232b-45d5-a808-056e01ff65dc}</MetaDataID>
        [OOAdvantech.Json.JsonIgnore]
        [BackwardCompatibilityID("+1")]
        MenuModel.IOptionMenuItemSpecific itemSpecificOption { get; }
#endif

        /// <MetaDataID>{375dc922-7f3b-4eac-a466-2fcbec194efe}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        bool Without { get; }

        /// <MetaDataID>{1f1fec9c-c64b-4a19-9e34-db619db8db66}</MetaDataID>
        [BackwardCompatibilityID("+5")]
        bool CheckedOption { get; set; }


    }
}