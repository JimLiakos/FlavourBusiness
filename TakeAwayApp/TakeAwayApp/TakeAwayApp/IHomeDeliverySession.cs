using OOAdvantech.MetaDataRepository;

namespace TakeAwayApp
{
    /// <MetaDataID>{dbe98ca9-afef-4dc4-a186-7cf87d8bba53}</MetaDataID>
    [HttpVisible]
    public interface IHomeDeliverySession
    {
        /// <MetaDataID>{569a0690-d425-4782-8ee1-3d38ac8f08c4}</MetaDataID>
        [Association("", Roles.RoleA, "6cbfc791-e915-4a7c-bd89-d4b978989b51")]
        [RoleAMultiplicityRange(1, 1)]
        [CachingDataOnClientSide]
        DontWaitApp.IFoodServicesClientSessionViewModel FoodServiceClientSession { get; }

        /// <MetaDataID>{156d964a-37e9-4d03-b7b5-d6fc1480fae6}</MetaDataID>
        string CallerPhone { get; }

        /// <MetaDataID>{c49c5078-a64d-40ff-a64e-f4297fd931be}</MetaDataID>
        CallerCenterSessionState State { get; set; }




    }

    /// <MetaDataID>{98fa632e-6780-417c-aef4-973c6bd73f13}</MetaDataID>
    public enum CallerCenterSessionState
    {
        PendingCall,
        OrderTaking,
        Commited

    }
}