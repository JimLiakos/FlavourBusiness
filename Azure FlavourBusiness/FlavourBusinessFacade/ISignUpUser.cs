using OOAdvantech.MetaDataRepository;
using OOAdvantech;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using OOAdvantech.Json.Linq;

namespace FlavourBusinessFacade.ViewModel
{
    /// <MetaDataID>{96a8b867-dd68-41d0-aa1f-d8e02f00224c}</MetaDataID>
    [OOAdvantech.MetaDataRepository.HttpVisible]
    public interface IUser
    {
        /// <MetaDataID>{f9330aa3-1bad-4709-ae81-e63572f423bc}</MetaDataID>
        [OOAdvantech.MetaDataRepository.CachingDataOnClientSide]
        string FullName { get; set; }
        /// <MetaDataID>{a43d4066-ecf3-4a87-bf30-0c30c8a8e074}</MetaDataID>
        [OOAdvantech.MetaDataRepository.CachingDataOnClientSide]
        string UserName { get; set; }
        /// <MetaDataID>{10cf6d90-36b8-421b-9c35-350a2c5f0a95}</MetaDataID>
        string Email { get; set; }
        /// <MetaDataID>{0654d96c-d898-448a-bf3c-d414fe291b30}</MetaDataID>
        string Password { get; set; }
        /// <MetaDataID>{bb20cd5b-e7b6-48a2-9856-77aa454b8989}</MetaDataID>
        string ConfirmPassword { get; set; }

        /// <MetaDataID>{db24c0d2-6c10-41f0-b6d7-4c071027127b}</MetaDataID>
        string PhoneNumber { get; set; }
    }

    /// <MetaDataID>{e6b6ed2b-c1a0-4c53-9d50-4136afc3faff}</MetaDataID>
    [OOAdvantech.MetaDataRepository.HttpVisible]
    public interface ILocalization
    {
        /// <MetaDataID>{6c551519-9f15-4b0b-9608-d957db625e95}</MetaDataID>
        string Language { get; }

        /// <MetaDataID>{ed4ee429-33f1-43bf-8ea5-c92169cae9b3}</MetaDataID>
        string DefaultLanguage { get; }

        /// <MetaDataID>{45d4c218-adff-467d-8136-4ad2f83a2697}</MetaDataID>
        string GetTranslation(string langCountry);

        /// <MetaDataID>{c7d216d1-8970-4fcb-8d2f-ae71ae5ba947}</MetaDataID>
        string GetString(string langCountry, string key);

        /// <MetaDataID>{0699aa09-153a-4251-842c-e493e6a777a5}</MetaDataID>
        void SetString(string langCountry, string key, string newValue);

        /// <MetaDataID>{e1b0bcde-7694-4b3d-a5ba-e8f8d83aa55a}</MetaDataID>
        string AppIdentity { get; }

    }



    /// <MetaDataID>{6fe8b63a-548e-4d58-9763-b765030a87d4}</MetaDataID>
    [HttpVisible]
    public interface ISecureUser : FlavourBusinessFacade.ViewModel.IUser
    {

        [GenerateEventConsumerProxy]
        event ObjectChangeStateHandle ObjectChangeState;


        /// <MetaDataID>{a5bb9008-1509-44ac-961c-170a742ba163}</MetaDataID>
        [OOAdvantech.MetaDataRepository.CachingDataOnClientSide]
        string SignInProvider { get; set; }


        /// <MetaDataID>{ea0d5857-2842-403e-8b46-3d285e2dd6f0}</MetaDataID>
        [OOAdvantech.MetaDataRepository.CachingDataOnClientSide]
        string OAuthUserIdentity { get; set; }

        /// <MetaDataID>{701c9f86-e958-431c-abf2-4a66777d6255}</MetaDataID>
        void SignOut();

        /// <MetaDataID>{44652dec-effd-48bb-aae7-da0fe78b0671}</MetaDataID>
        Task<bool> SignUp();

        /// <MetaDataID>{7ba8e62a-0baa-4fe9-b58c-ed9d08381a1e}</MetaDataID>
        Task<bool> SignIn();

        /// <MetaDataID>{ab6e6eb2-5155-49f9-b80e-68b00e96b730}</MetaDataID>
        void SaveUserProfile();

        /// <MetaDataID>{7495ec7c-a77a-41b9-81c4-5e7c2f59226d}</MetaDataID>
        bool IsUsernameInUse(string username, OOAdvantech.Authentication.SignInProvider signInProvider);

        /// <MetaDataID>{763106d8-994b-489e-be2e-44130d719f3b}</MetaDataID>
        void SendVerificationEmail(string emailAddress);

        /// <MetaDataID>{9e7f3e6a-740f-4e76-a7e3-61d571ecd90a}</MetaDataID>
        void CreateUserWithEmailAndPassword(string emailVerificationCode);

        /// <MetaDataID>{4fe32dc1-cc74-4993-a441-f5e10dace55b}</MetaDataID>
        IList<UserData> GetNativeUsers();

        /// <MetaDataID>{a3ec8eff-c8ac-4898-b0a2-24c571c9082b}</MetaDataID>
        UserData SignInNativeUser(string userName, string password);



    }

    /// <MetaDataID>{b2178661-8030-4ca2-aceb-cb14553d12df}</MetaDataID>
    [OOAdvantech.MetaDataRepository.HttpVisible]
    public interface IFoodServiceClient
    {
        string PhoneNumber { get; set; }
        string FullName { get; set; }
        string EmailAddress { get; set; }

        string Identity { get; }


    }

}
