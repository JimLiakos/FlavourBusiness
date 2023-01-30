using FlavourBusinessFacade.EndUsers;
using FlavourBusinessFacade.ServicesContextResources;
using OOAdvantech.MetaDataRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DontWaitApp
{

    /// <MetaDataID>{54e86af5-3fd6-4b5e-94ef-ca06f9bbfe0d}</MetaDataID>
    [HttpVisible]
    public interface IFoodServicesClientSessionViewModel
    {
        [Association("FoofServiceSessionDeliverPlace", Roles.RoleA, "fe596f30-a4c4-44cb-91c6-4f0b7200fd8f")]
        FlavourBusinessFacade.EndUsers.IPlace DeliveryPlace { get; set; }


        /// <MetaDataID>{ad945fbe-f52f-4c07-9b70-bfa13139db91}</MetaDataID>
        string DeliveryComment { get; set; }


        /// <MetaDataID>{f2674898-5add-445e-800d-162ffb6a5087}</MetaDataID>
        DateTime? ServiceTime { get; set; }

        /// <MetaDataID>{29923a1e-c1ff-434e-b5c9-0c90778e9554}</MetaDataID>
        ChangeDeliveryPlaceResponse CanChangeDeliveryPlace(FlavourBusinessFacade.EndUsers.IPlace newDeliveryPlace);
        [RoleAMultiplicityRange(0)]
        [AssociationEndBehavior(PersistencyFlag.CascadeDelete)]
        [Association("ClientSesionOrderItems", Roles.RoleA, "d5c354c5-f20e-409b-b824-6f0d350186b3")]
        List<FlavourBusinessManager.RoomService.ItemPreparation> OrderItems { get; }

        /// <MetaDataID>{ddc0de45-90e3-4ad6-ad3b-ba01880aef0a}</MetaDataID>
        void SuggestMenuItem(Messmate messmate, string menuItemUri);

        /// <MetaDataID>{d7ff4093-88ba-4478-93a2-fe911236cb04}</MetaDataID>
        void EndOfMenuItemProposal(Messmate messmate, string messageID);

        /// <MetaDataID>{89a16f62-175a-41b9-bb83-31c88100e6b8}</MetaDataID>
        Task<bool> SendItemsForPreparation();

        /// <MetaDataID>{a2590c45-f6aa-404e-9547-897155c1ed10}</MetaDataID>
        void RefreshMessmates();


        /// <MetaDataID>{3cbedb95-504f-48f5-bfdf-597798842253}</MetaDataID>
        Task<FlavourBusinessFacade.RoomService.IBill> GetBill();

        /// <MetaDataID>{ff44b605-d9c9-4796-9b41-5bd22ed8e965}</MetaDataID>
        Task Pay(FinanceFacade.IPayment payment, decimal tipAmount);


        /// <MetaDataID>{1956fcb0-8a1a-4df8-ac01-057fac87a883}</MetaDataID>
        IList<Messmate> GetMessmates();

        

  


        /// <MetaDataID>{c3c9d4ab-41b4-41a5-a0cb-6d2d6cba58e5}</MetaDataID>
        [HttpInVisible]
        IFoodServiceSession MainSession { get; }


        /// <MetaDataID>{e48a2bb1-481f-4417-bef0-b2e7b9401b45}</MetaDataID>
        ServicePointState ServicePointState { get; }

        [GenerateEventConsumerProxy]
        event SharedItemChangedHandle SharedItemChanged;


        /// <MetaDataID>{9307ddb6-eccf-42f0-920a-3f022bd6c958}</MetaDataID>
        MenuData MenuData { get; }


        /// <MetaDataID>{0d8183bc-41b0-4601-a291-16c18ad31b08}</MetaDataID>
        IList<Messmate> GetCandidateMessmates();


   
        [GenerateEventConsumerProxy]
        event MenuItemProposalHandle MenuItemProposal;

        [GenerateEventConsumerProxy]
        event MessmatesWaitForYouToDecideHandle MessmatesWaitForYouToDecide;


        [GenerateEventConsumerProxy]
        event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;

        /// <MetaDataID>{397f6f36-da35-4f39-9e7e-0512e87b8b79}</MetaDataID>
        void AddItem(FlavourBusinessManager.RoomService.ItemPreparation item);

        /// <MetaDataID>{bac19771-bd08-445b-9ed1-601e0dfa67d1}</MetaDataID>
        void RemoveItem(FlavourBusinessManager.RoomService.ItemPreparation item);

        /// <MetaDataID>{69fba72b-056c-43d5-a999-2ba977e87a7c}</MetaDataID>
        void ItemChanged(FlavourBusinessManager.RoomService.ItemPreparation item);

        /// <MetaDataID>{2e3da50d-bf09-43b3-ae13-4feaf667b2f2}</MetaDataID>
        void AddSharingItem(FlavourBusinessManager.RoomService.ItemPreparation item);


        /// <MetaDataID>{f7534611-0be9-4b2b-93f7-9cb0ff64d602}</MetaDataID>
        IList<FlavourBusinessManager.RoomService.ItemPreparation> PreparationItems { get; }


        /// <MetaDataID>{5c8b7177-5047-4ba2-a21e-20644280f845}</MetaDataID>
        PayOptions? PayOption { get; set; }

        List<TipOption> TipOptions { get; } 

    }

    /// <MetaDataID>{0cb06dce-d187-4c17-9caa-ad584c8854f1}</MetaDataID>
    public enum PayOptions
    {
        PayOnCheckout = 1,
        PayOnDelivery = 2
    }

    public class TipOption
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public string ISOCurrencySymbol { get; set; }
    }
}
