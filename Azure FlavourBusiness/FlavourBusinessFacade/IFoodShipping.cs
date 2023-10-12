using FlavourBusinessFacade.EndUsers;
using OOAdvantech.MetaDataRepository;

namespace FlavourBusinessFacade.Shipping
{
    /// <MetaDataID>{fb36f7f0-3762-4db7-9e05-40e87ba43631}</MetaDataID>
    [GenerateFacadeProxy]
    public interface IFoodShipping : RoomService.IServingBatch
    {
        IPlace Place { get;  }

        string ClientFullName { get;  }

        string PhoneNumber { get;  }

        string DeliveryRemark { get; }

        string NotesForClient { get; }


    }
}