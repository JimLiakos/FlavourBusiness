using OOAdvantech.MetaDataRepository;

namespace FlavourBusinessFacade.RoomService
{
    /// <MetaDataID>{e829df38-0b19-40d1-9211-9f95fb470174}</MetaDataID>
    [BackwardCompatibilityID("{e829df38-0b19-40d1-9211-9f95fb470174}")]
    public interface IOptionChange
    {
        /// <MetaDataID>{954c40fa-2992-4bf9-bf08-31fe883a8f83}</MetaDataID>
        [Association("ItemPreparationOptionsChange", Roles.RoleB, "8e564693-11c5-445c-8cb3-1f9505a2f2d2")]
        [RoleBMultiplicityRange(1, 1)]
        IItemPreparation ItemPreparation { get; set; }
    }
}