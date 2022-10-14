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

        [HttpInVisible]
        IFoodServiceSession MainSession { get; }


        ServicePointState ServicePointState { get; }

        void SendMealInvitationMessage(InvitationChannel channel, string endPoint);

        [GenerateEventConsumerProxy]
        event SharedItemChangedHandle SharedItemChanged;

        /// <MetaDataID>{ddc0de45-90e3-4ad6-ad3b-ba01880aef0a}</MetaDataID>
        void SuggestMenuItem(Messmate messmate, string menuItemUri);


        #region  Meal invitation
        /// <MetaDataID>{8fabd5f0-b381-439c-a726-932a70dcdf4d}</MetaDataID>
        void MealInvitation(Messmate messmate);


        Task<Contact> PickContact();

        /// <MetaDataID>{009b6efd-a074-450e-b2d7-58755a212bb3}</MetaDataID>
        Task<bool> AcceptInvitation(Messmate messmate, string messageID);

        /// <MetaDataID>{149a02ff-8930-49cb-ae78-fa52cbea5b39}</MetaDataID>
        void DenyInvitation(Messmate messmate, string messageID);

        /// <MetaDataID>{a7914fd9-b836-4504-ab6a-407c4803f4f6}</MetaDataID>
        void CancelMealInvitation(Messmate messmate);
        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        /// <MetaDataID>{9a168c2f-de48-47d1-ab4c-d46d48292364}</MetaDataID>
        string GetMealInvitationQRCode(string color);


        /// <MetaDataID>{9307ddb6-eccf-42f0-920a-3f022bd6c958}</MetaDataID>
        MenuData MenuData { get; }


        /// <MetaDataID>{0d8183bc-41b0-4601-a291-16c18ad31b08}</MetaDataID>
        IList<Messmate> GetCandidateMessmates();

        /// <MetaDataID>{1956fcb0-8a1a-4df8-ac01-057fac87a883}</MetaDataID>
        IList<Messmate> GetMessmates();


        /// <MetaDataID>{a2590c45-f6aa-404e-9547-897155c1ed10}</MetaDataID>
        void RefreshMessmates();



        /// <MetaDataID>{89a16f62-175a-41b9-bb83-31c88100e6b8}</MetaDataID>
        Task<bool> SendItemsForPreparation();


   
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



        /// <MetaDataID>{d7ff4093-88ba-4478-93a2-fe911236cb04}</MetaDataID>
        void EndOfMenuItemProposal(Messmate messmate, string messageID);


        /// <MetaDataID>{f7534611-0be9-4b2b-93f7-9cb0ff64d602}</MetaDataID>
        IList<FlavourBusinessManager.RoomService.ItemPreparation> PreparationItems { get; }

    }
}
