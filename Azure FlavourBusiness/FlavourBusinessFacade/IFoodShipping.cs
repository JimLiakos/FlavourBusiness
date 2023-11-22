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
        /// <MetaDataID>{b67cdb6e-ae8b-454e-bfaf-5093d0643bd7}</MetaDataID>
        string ClientFullName { get; }
        /// <MetaDataID>{f8fcc4e5-e6a2-4d60-8516-f89bb658af23}</MetaDataID>
        [BackwardCompatibilityID("+3")]
        string PhoneNumber { get; }
        /// <MetaDataID>{7c1f54e2-57a7-42a7-a76c-ec6d61e0d0e2}</MetaDataID>
        [BackwardCompatibilityID("+4")]
        string DeliveryRemark { get; }
        /// <MetaDataID>{1df58717-0e1e-4e3f-9965-7f61403a0cd8}</MetaDataID>
        [BackwardCompatibilityID("+5")]
        string NotesForClient { get; }


    }
}