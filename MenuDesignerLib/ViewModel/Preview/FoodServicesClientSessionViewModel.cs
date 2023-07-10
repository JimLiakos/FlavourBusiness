using DontWaitApp;
using FinanceFacade;
using FlavourBusinessFacade.EndUsers;
using FlavourBusinessFacade.RoomService;
using FlavourBusinessFacade.ServicesContextResources;
using FlavourBusinessManager.RoomService;
using OOAdvantech;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuDesigner.ViewModel.Preview
{
    /// <MetaDataID>{7a078ffa-ba00-43b7-89f7-73f9f625ee67}</MetaDataID>
    class FoodServicesClientSessionViewModel : MarshalByRefObject, IFoodServicesClientSessionViewModel, OOAdvantech.Remoting.IExtMarshalByRefObject
    {
        /// <MetaDataID>{cda04e9a-c2cc-407a-b290-e3841a086a25}</MetaDataID>
        public IFoodServiceSession MainSession => null;

        /// <MetaDataID>{f0125bcd-3096-40f0-b22e-42680d45f3d8}</MetaDataID>
        public ServicePointState ServicePointState => ServicePointState.Conversation;

        /// <MetaDataID>{cdffa8d8-3788-4688-bfeb-4dcdf0b283f5}</MetaDataID>
        public MenuData MenuData { get; set; }

        /// <MetaDataID>{f12b11f7-8597-4551-9f68-702e88cd1fd5}</MetaDataID>
        public IList<ItemPreparation> PreparationItems => OrderItems;

        /// <MetaDataID>{90c42001-6830-415d-a9b9-a5166f8dd0af}</MetaDataID>
        public List<ItemPreparation> OrderItems => new List<ItemPreparation>();

        /// <MetaDataID>{ebe5991a-53e9-4c15-93cb-a9323a65699f}</MetaDataID>
        public IPlace DeliveryPlace { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime? ServiceTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public PayOptions? PayOption { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string DeliveryComment { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public List<TipOption> TipOptions => throw new NotImplementedException();
        public List<CurrencyOption> CurrencyOptions { get; }

        public IFoodServiceClientSession FoodServicesClientSession => throw new NotImplementedException();

        public event SharedItemChangedHandle SharedItemChanged;
        public event MenuItemProposalHandle MenuItemProposal;
        public event MessmatesWaitForYouToDecideHandle MessmatesWaitForYouToDecide;
        public event ObjectChangeStateHandle ObjectChangeState;

        /// <MetaDataID>{2416f0ce-58b1-45b0-a9cf-b73bdbd4e428}</MetaDataID>
        public void AddItem(ItemPreparation item)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{c66c2fa5-5d6c-4853-9b5b-8d1507908ba0}</MetaDataID>
        public void AddSharingItem(ItemPreparation item)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{1c9e34c1-e698-49c5-8e09-bb7ff540befb}</MetaDataID>
        public ChangeDeliveryPlaceResponse CanChangeDeliveryPlace(IPlace newDeliveryPlace)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{08cb638b-bf94-4be4-a8da-c9eec5d50469}</MetaDataID>
        public void EndOfMenuItemProposal(Messmate messmate, string messageID)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{ca0380ef-f535-49f8-9186-b19449b6d6b1}</MetaDataID>
        public Task<IBill> GetBill()
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{725f7147-e8bf-4395-8f09-44bc316b9c39}</MetaDataID>
        public IList<Messmate> GetCandidateMessmates()
        {
            return new List<Messmate>();
        }

        /// <MetaDataID>{c643936a-8fdb-4418-acf1-d273994afccb}</MetaDataID>
        public IList<Messmate> GetMessmates()
        {
            return new List<Messmate>();
        }

        /// <MetaDataID>{1faa0f90-08ee-422b-a65c-4a5699a34c63}</MetaDataID>
        public Task<IPayment> GetPayment()
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{bdb20e62-ceda-4610-9297-e2b02d34c586}</MetaDataID>
        public void ItemChanged(ItemPreparation item)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{b756e7f4-a514-43fa-ba6c-68c7fddf9130}</MetaDataID>
        public IPayment Pay()
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{e68e6b53-8dee-4137-bcc1-130ad008acba}</MetaDataID>
        public void Pay(IPayment payment, decimal tipAmount)
        {
            throw new NotImplementedException();
        }

        public Task Pay(IPayment payment, PaymentMethod paymentMethod, decimal tipAmount)
        {
            throw new NotImplementedException();
        }

        public Task<bool> PayAndCommit(IPayment payment, decimal tipAmount)
        {
            throw new NotImplementedException();
        }

        public Task<bool> PayAndCommit(IPayment payment, PaymentMethod paymentMethod, decimal tipAmount)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{d0cf9d3a-1ae0-4ed3-83c7-a754ff12bbec}</MetaDataID>
        public void RefreshMessmates()
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{0fef240f-ea73-4f88-85f6-15ec124f641c}</MetaDataID>
        public void RemoveItem(ItemPreparation item)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{ce8b759a-d2f8-41e2-b818-ddfea2f26c05}</MetaDataID>
        public Task<bool> SendItemsForPreparation()
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{23623b10-ea81-4149-a8ed-fb0e2d5ffd4a}</MetaDataID>
        public void SuggestMenuItem(Messmate messmate, string menuItemUri)
        {
            throw new NotImplementedException();
        }

    }
}
