using System;
using System.Collections.Generic;
using FlavourBusinessFacade.RoomService;
using FlavourBusinessFacade.ServicesContextResources;
using OOAdvantech.MetaDataRepository;

namespace FlavourBusinessFacade.EndUsers
{
    /// <MetaDataID>{0705a4fa-af2e-4a58-b666-e1bba0e07f2a}</MetaDataID>
    [BackwardCompatibilityID("{0705a4fa-af2e-4a58-b666-e1bba0e07f2a}")]
    [GenerateFacadeProxy]
    public interface IFoodServiceClientSession: IMessageConsumer
    {

        /// <MetaDataID>{4a6cfe26-2795-4345-85e5-f518c066e86c}</MetaDataID>
        Dictionary<string, ItemPreparationState> Prepare(List<IItemPreparation> itemPreparations);



        
        /// <MetaDataID>{a8310e74-53f0-4f82-9400-63f5377dacae}</MetaDataID>
        [BackwardCompatibilityID("+9")]
        bool WaiterSession { get; }
 
        [Association("WaterServiceSession", Roles.RoleB, "0c49af08-a143-4f46-8e69-6f3b0f44870b")]
        HumanResources.IWaiter Waiter { get; }



        /// <MetaDataID>{6bb4647d-2291-403c-9330-cae0a44d9015}</MetaDataID>
        [BackwardCompatibilityID("+8")]
        DateTime ModificationTime { get; set; }

        /// <MetaDataID>{3ee751a4-b32a-442d-a159-67ce755c127e}</MetaDataID>
        void RemoveSharingItem(IItemPreparation item);

        /// <MetaDataID>{2699b4f5-a3ce-442e-8c61-19c2a32d5d67}</MetaDataID>
        void AddSharingItem(IItemPreparation item);

        [Association("ClientsSharedItems", Roles.RoleA, "d3f8b87e-0f43-4373-939e-950dd9db19b2")]
        [RoleBMultiplicityRange(1)]
        [IgnoreErrorCheck]
        IList<IItemPreparation> SharedItems { get; }


        [Association("ClientFlavourItems", Roles.RoleA, true, "913f7a7e-cd2a-4833-a0dc-dde7987eefff")]
        [RoleBMultiplicityRange(1, 1)]
        IList<IItemPreparation> FlavourItems { get; }


        /// <MetaDataID>{0c36ad38-061e-4225-96cb-09a9d5bcb738}</MetaDataID>
        void RemoveItem(IItemPreparation item);



        /// <MetaDataID>{9bc00ae4-359d-4dab-8e0d-62f715d04e0f}</MetaDataID>
        void ItemChanged(IItemPreparation item);

        /// <MetaDataID>{1558c42a-ea8c-44e5-8c15-83563c6468dc}</MetaDataID>
        void AddItem(IItemPreparation item);

        

        event ItemStateChangedHandle ItemStateChanged;

        event ItemsStateChangedHandle ItemsStateChanged;

        event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;


 

        /// <MetaDataID>{fd43be80-69fc-44d3-8624-a95ed5810848}</MetaDataID>
        void DeviceResume();

        /// <MetaDataID>{22c876e5-da5a-4f59-b643-9a7eae34f311}</MetaDataID>
        void DeviceSleep();


        /// <MetaDataID>{cec35a23-7ff3-494a-b1fc-b6019c6af2e4}</MetaDataID>
        void MealInvitation(IFoodServiceClientSession messmateClientSesion);

        /// <MetaDataID>{6da62499-9a91-4ea4-a15c-2be74f43f356}</MetaDataID>
        void CancelMealInvitation(IFoodServiceClientSession messmateClientSesion);

        /// <MetaDataID>{ebd6610e-df86-4892-bcd9-65b8e681d7b2}</MetaDataID>
        void AcceptMealInvitation(string clientSessionToken, FlavourBusinessFacade.EndUsers.IFoodServiceClientSession messmateClientSesion);



        /// <MetaDataID>{5545b538-62a4-4a96-a27e-68a080061669}</MetaDataID>
        void MealInvitationDenied(IFoodServiceClientSession foodServiceClientSession);

        /// <summary>
        /// Gets food service client sessions of persons which seat on same table
        /// </summary>
        /// <returns>
        /// a collection with Food Service Client Sessions
        /// </returns>
        /// <MetaDataID>{ee3f560b-1f48-4a05-8d1a-aba78af17972}</MetaDataID>
        IList<IFoodServiceClientSession> GetPeopleNearMe();

        /// <MetaDataID>{3013a723-966e-4084-8e5c-955f32fd9079}</MetaDataID>
        IList<IFoodServiceClientSession> GetMealParticipants();


        /// <MetaDataID>{f13e6867-f3c7-4751-a42b-c73d78941650}</MetaDataID>
        IList<IFoodServiceClientSession> GetServicePointParticipants();


        [Association("ServicePointClientSesions", Roles.RoleA, "fc1aed39-ae8c-4c0b-8aff-3a8bf91c7ee7")]
        [RoleAMultiplicityRange(1, 1)]
        IServicePoint ServicePoint { get; set; }


        [Association("SessionClient", Roles.RoleA, "adc4931c-7835-4dac-bed0-0ec8d8278847")]
        EndUsers.IFoodServiceClient Client { get; set; }

        /// <MetaDataID>{b949486a-408d-4c4b-875d-0f40b8ad374d}</MetaDataID>
        void RaiseItemStateChanged(string uid, string itemOwningSession, string itemChangeSession, bool isShared, List<string> shareInSessions);


        /// <MetaDataID>{3fd7e6b9-b785-4397-8826-8907fcaec907}</MetaDataID>
        void RaiseItemsStateChanged(Dictionary<string, ItemPreparationState> newItemsState);

        /// <MetaDataID>{8426fcc3-a1c9-435c-87d4-e2f77d8e5566}</MetaDataID>
        [Association("FoodServiceSession", Roles.RoleB, "93808acd-1c78-45da-8c44-dd7666ae0128")]
        [OOAdvantech.MetaDataRepository.RoleBMultiplicityRange(1, 1)]
        IFoodServiceSession MainSession { get; }


        ///// <MetaDataID>{00bd07cb-55bb-4fe5-b45e-e5f3f4e82235}</MetaDataID>
        //[BackwardCompatibilityID("+1")]
        //string ClientDeviceID { get; set; }

        /// <MetaDataID>{aee33f7e-f102-41f4-9553-4477d1d1b445}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        [CachingDataOnClientSide]
        string ClientName { get; set; }

        /// <MetaDataID>{cc81edc5-40c9-4a8c-9dc4-96592933f789}</MetaDataID>
        [BackwardCompatibilityID("+3")]
        DateTime SessionStarts { get; set; }

        /// <MetaDataID>{2865c1ea-feb4-470d-b0bd-7801c6c77421}</MetaDataID>
        [BackwardCompatibilityID("+4")]
        DateTime SessionEnds { get; set; }

        /// <MetaDataID>{af9582e3-8d8d-4025-a10c-2b3e445507e8}</MetaDataID>
        [BackwardCompatibilityID("+5")]
        [CachingDataOnClientSide]
        DateTime DateTimeOfLastRequest { get; set; }


        /// <MetaDataID>{fa5df8bc-0c95-444c-bb59-11d59b3ca7b0}</MetaDataID>
        OrganizationStorageRef Menu { get; }


        /// <MetaDataID>{5a821c89-cc92-4bc5-84f7-6287613044bf}</MetaDataID>
        /// <summary>This token is the identity of device for push notification mechanism</summary>
        [BackwardCompatibilityID("+6")]
        string DeviceFirebaseToken { get; set; }


        /// <MetaDataID>{030c7218-d755-4978-87e4-4745a42804d0}</MetaDataID>
        [BackwardCompatibilityID("+7")]
        string SessionID { get; }

        /// <MetaDataID>{712f8ab3-cc86-4767-aa6b-583d92067219}</MetaDataID>
        IItemPreparation GetItem(string itemUid);

        /// <MetaDataID>{b8ad8261-1dc2-4065-aaf9-3648bfa5d9c0}</MetaDataID>
        Dictionary<string, ItemPreparationState> Commit(List<IItemPreparation> itemPreparations);

        /// <MetaDataID>{5e5aa9fa-5b59-43f6-9e62-0852f60c6c4b}</MetaDataID>
        void MenuItemProposal(IFoodServiceClientSession messmateClientSesion, string menuItemUri);
    }

    /// <MetaDataID>{6162db9b-39b6-4d27-aea8-3f4070476c2d}</MetaDataID>
    public enum ClientMessages
    {
        PartOfMealRequest = 0,
        YouMustDecide = 1,
        MenuItemProposal = 2,
        ShareItemHasChange = 3,
        LaytheTable=4,
    }

  
    public delegate void ItemStateChangedHandle(string itemUid, string itemOwningSession, bool isShared, List<string> shareInSessions);

    public delegate void ItemsStateChangedHandle(Dictionary<string, ItemPreparationState> newItemsState);


    /// <MetaDataID>{559c8633-1178-4ce7-aa69-c71b0e6d2798}</MetaDataID>
    public struct ClientSessionData
    {
        /// <MetaDataID>{10c60dd6-a207-4c50-8adf-5e8aa3c65424}</MetaDataID>
        public string ServicePointIdentity;
        /// <MetaDataID>{fc539f91-4ce9-4117-bb97-f4b033fb976f}</MetaDataID>
        public string Token;
        /// <MetaDataID>{575e68f6-b3ad-4ae7-9ca0-63d3f81d26ad}</MetaDataID>
        public EndUsers.IFoodServiceClientSession FoodServiceClientSession;

        /// <MetaDataID>{86cd02d5-d2d1-4fc4-b4d0-becbe44500c0}</MetaDataID>
        public string ServicesContextLogo;

        /// <MetaDataID>{61d96939-76a9-4eac-9a22-1dd3c521ce07}</MetaDataID>
        public string ServicesPointName;
    }

}