using DontWaitApp;
using FlavourBusinessFacade;
using FlavourBusinessFacade.EndUsers;
using FlavourBusinessFacade.ServicesContextResources;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;

namespace TakeAwayApp.ViewModel
{
    /// <MetaDataID>{dbe98ca9-afef-4dc4-a186-7cf87d8bba53}</MetaDataID>
    [HttpVisible]
    public interface IHomeDeliverySession
    {
        /// <MetaDataID>{569a0690-d425-4782-8ee1-3d38ac8f08c4}</MetaDataID>
        [Association("", Roles.RoleA, "6cbfc791-e915-4a7c-bd89-d4b978989b51")]
        [RoleAMultiplicityRange(1, 1)]
        [CachingDataOnClientSide]
        IFoodServicesClientSessionViewModel FoodServiceClientSession { get; }


        bool UncommittedChanges { get; }

        /// <MetaDataID>{156d964a-37e9-4d03-b7b5-d6fc1480fae6}</MetaDataID>
        string CallerPhone { get; set; }


        /// <MetaDataID>{266da272-f342-404e-932a-98aeeae0d47a}</MetaDataID>
        FoodServiceClientVM SessionClient { get; set; }

        /// <MetaDataID>{44d60b2f-6e6e-4f80-8383-ee4c69f51569}</MetaDataID>
        System.Collections.Generic.List<HomeDeliveryServicePointAbbreviation> GetNeighborhoodFoodServers(Coordinate location);

        /// <MetaDataID>{c49c5078-a64d-40ff-a64e-f4297fd931be}</MetaDataID>
        CallerCenterSessionState State { get; set; }

        /// <MetaDataID>{8bac2794-adff-4303-964a-eeca2f51e094}</MetaDataID>
        System.Collections.Generic.List<HomeDeliveryServicePointAbbreviation> HomeDeliveryServicePoints { get; }

        /// <MetaDataID>{41af5d0a-7a99-4bac-bae3-196c72cb7134}</MetaDataID>
        HomeDeliveryServicePointAbbreviation HomeDeliveryServicePoint { get; set; }

        IPlace DeliveryPlace { get; set; }



        bool OrderCommit();





    }


}

/// <MetaDataID>{98fa632e-6780-417c-aef4-973c6bd73f13}</MetaDataID>
public enum CallerCenterSessionState
{
    PendingCall,
    OrderTaking,
    Committed
}


