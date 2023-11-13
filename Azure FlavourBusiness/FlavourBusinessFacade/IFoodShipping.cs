using FlavourBusinessFacade.EndUsers;
using OOAdvantech.MetaDataRepository;

namespace FlavourBusinessFacade.Shipping
{
    /// <MetaDataID>{fb36f7f0-3762-4db7-9e05-40e87ba43631}</MetaDataID>
    [GenerateFacadeProxy]
    public interface IFoodShipping : RoomService.IServingBatch
    {
        /// <MetaDataID>{1b75fcb5-7f27-419b-9fc8-17261cfac7c4}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        string Identity { get; }

        /// <MetaDataID>{e43268d5-265d-47da-9084-1bd95f4bb369}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        IPlace Place { get; }
        string ClientFullName { get; }
        [BackwardCompatibilityID("+3")]
        string PhoneNumber { get; }
        [BackwardCompatibilityID("+4")]
        string DeliveryRemark { get; }
        [BackwardCompatibilityID("+5")]
        string NotesForClient { get; }


    }
}