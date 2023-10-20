using FlavourBusinessFacade.EndUsers;
using OOAdvantech.MetaDataRepository;

namespace FlavourBusinessFacade.Shipping
{
    /// <MetaDataID>{fb36f7f0-3762-4db7-9e05-40e87ba43631}</MetaDataID>
    [GenerateFacadeProxy]
    public interface IFoodShipping : RoomService.IServingBatch
    {
        /// <MetaDataID>{e43268d5-265d-47da-9084-1bd95f4bb369}</MetaDataID>
        IPlace Place { get; }

        /// <MetaDataID>{2d25c09e-e7b4-4de0-a8ab-46b69ca2a01b}</MetaDataID>
        string ClientFullName { get; }

        /// <MetaDataID>{b9c002a5-1dbe-476d-8b2c-301a55b4e3ed}</MetaDataID>
        string PhoneNumber { get; }

        /// <MetaDataID>{c655db5e-ee48-4daa-80e2-85bc26a2e7ee}</MetaDataID>
        string DeliveryRemark { get; }

        /// <MetaDataID>{9cd91377-c2b0-42f3-9c6d-71bfee28a962}</MetaDataID>
        string NotesForClient { get; }


    }
}