using FlavourBusinessFacade;
using FlavourBusinessFacade.EndUsers;
using FlavourBusinessFacade.ServicesContextResources;
using OOAdvantech;
using OOAdvantech.Json;
using OOAdvantech.MetaDataRepository;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DontWaitApp
{

    /// <MetaDataID>{54e86af5-3fd6-4b5e-94ef-ca06f9bbfe0d}</MetaDataID>
    [HttpVisible]
    public interface IFoodServicesClientSessionViewModel
    {
        [Association("FoofServiceSessionDeliverPlace", Roles.RoleA, "fe596f30-a4c4-44cb-91c6-4f0b7200fd8f")]
        IPlace DeliveryPlace { get; set; }

        /// <summary>
        /// This method change the meal participation from implicit to explicit
        /// </summary>
        /// <param name="clientSessionID">
        /// Defines the id of clientSession that participate in meal implicitly 
        /// </param>
        /// <returns>
        /// return true if clientSession changed from implicit participation to explicit  
        /// </returns>
        Task<bool> MakeExplicitMealParticipation(string clientSessionID);

        /// <MetaDataID>{ad945fbe-f52f-4c07-9b70-bfa13139db91}</MetaDataID>
        string DeliveryComment { get; set; }

        /// <MetaDataID>{751c779c-3b56-4669-a8e0-a04471502a28}</MetaDataID>
        string OrderComment { get; set; }

        /// <MetaDataID>{48783de4-ce28-425e-8e36-e5eeec6daa2e}</MetaDataID>
        string NotesForClient { get; set; }
 
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


        void MessageHasBeenRead(string messageID);


        void MessageHasBeenDisplayed(string messageID);

        /// <MetaDataID>{64fe935d-ffbf-4f58-a474-4bee412b24f2}</MetaDataID>
        Task<bool> CommitNewSessionType(SessionType sessionType);

        /// <MetaDataID>{89a16f62-175a-41b9-bb83-31c88100e6b8}</MetaDataID>
        Task<FoodServiceSessionCommitResponse> SendItemsForPreparation();

        /// <MetaDataID>{a2590c45-f6aa-404e-9547-897155c1ed10}</MetaDataID>
        Task RefreshMessmates();


        /// <MetaDataID>{3cbedb95-504f-48f5-bfdf-597798842253}</MetaDataID>
        Task<FlavourBusinessFacade.RoomService.IBill> GetBill();


        /// <MetaDataID>{bf8c65cf-4109-4211-ad29-49793d160c56}</MetaDataID>
        Task<FlavourBusinessFacade.RoomService.IBill> GetBill(List<SessionItemPreparationAbbreviation> itemPreparations);

        /// <MetaDataID>{ff44b605-d9c9-4796-9b41-5bd22ed8e965}</MetaDataID>
        Task Pay(FinanceFacade.IPayment payment, FinanceFacade.PaymentMethod paymentMethod, decimal tipAmount);
        //Task<bool> PayAndCommit(FinanceFacade.IPayment payment, PaymentMethod paymentMethod, decimal tipAmount);



        /// <MetaDataID>{1956fcb0-8a1a-4df8-ac01-057fac87a883}</MetaDataID>
        IList<Messmate> GetMessmates();


        /// <MetaDataID>{745f2c27-cb6b-48ef-8cc9-42910eb00099}</MetaDataID>
        IFoodServiceClientSession FoodServicesClientSession
        {
            get;
        }
        /// <MetaDataID>{6ae7852f-d4c2-482d-b210-dab0aead75bd}</MetaDataID>
        [HttpInVisible]
        string MainSessionID { get; }


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


        bool ImplicitMealParticipation { get; }


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

        /// <MetaDataID>{f93159e7-a54e-471c-b8c9-daaced2165b5}</MetaDataID>
        List<TipOption> TipOptions { get; }
        /// <MetaDataID>{b59dac49-e6fa-47bc-984f-c4b15863c3a9}</MetaDataID>
        List<CurrencyOption> CurrencyOptions { get; }




    }

    /// <MetaDataID>{0cb06dce-d187-4c17-9caa-ad584c8854f1}</MetaDataID>
    public enum PayOptions
    {
        PayOnCheckout = 1,
        PayOnDelivery = 2
    }

    /// <MetaDataID>{20ba1c67-f576-49d2-ad1d-5913e5d893f7}</MetaDataID>
    public class TipOption
    {
        public string Name { get; set; }
        /// <MetaDataID>{ebb03da1-e20f-4166-ad1a-4d089b353917}</MetaDataID>
        public decimal Amount { get; set; }
        /// <MetaDataID>{98b37b23-b0f0-4a6f-a723-f3f7d8fcda4e}</MetaDataID>
        public string ISOCurrencySymbol { get; set; }
    }

    /// <MetaDataID>{37764e57-f95e-4ea7-aa8b-dd3f670500ea}</MetaDataID>
    public class CurrencyOption
    {
        /// <MetaDataID>{eba6dc45-1d79-498b-831d-d907dc6de026}</MetaDataID>
        public string Name { get; set; }
        /// <MetaDataID>{01d51604-ab8f-4d24-95d6-f660495f972f}</MetaDataID>
        public decimal Amount { get; set; }
        /// <MetaDataID>{5c1bde0a-262f-46b4-bd1e-817a4ce23619}</MetaDataID>
        public string ISOCurrencySymbol { get; set; }
    }



    /// <MetaDataID>{e321a477-b121-4cec-b915-138e8e28d2ca}</MetaDataID>
    public enum FoodServiceSessionCommitResponse
    {
        SessionCommitted = 0,
        SessionChangesCommitted = 1,
        SessionNewItemsCommitted = 2


    }

}
