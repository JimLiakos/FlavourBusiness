using FlavourBusinessFacade.EndUsers;
using OOAdvantech.Authentication;
using System.Collections.Generic;

namespace FlavourBusinessFacade
{
    /// <MetaDataID>{d3252bec-25e6-4459-85d9-d176c9aa25c1}</MetaDataID>
    [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("{d3252bec-25e6-4459-85d9-d176c9aa25c1}")]
    [OOAdvantech.MetaDataRepository.HttpVisible]
    [OOAdvantech.MetaDataRepository.GenerateFacadeProxy]
    public interface IAuthFlavourBusiness
    {
        /// <MetaDataID>{abc46398-4664-452f-b75b-26620640c201}</MetaDataID>
        IFoodServiceClient SignUpEndUser(EndUserData endUser);

        bool IsUsernameInUse(string username, SignInProvider signInProvider);

#if DEBUG
        /// <MetaDataID>{a63bbf17-8e18-475e-aaf9-7fae77753b15}</MetaDataID>
        UserData GetUser(string userName);

        void SendVerificationEmail(string emailAddress);

        void SignUpUserWithEmailAndPassword(string email, string password, UserData userData, string verificationCode);
        /// <MetaDataID>{2a98d270-5673-4bdd-8055-2bb327aae030}</MetaDataID>
        IUser SignIn(RoleType roleType);

        /// <MetaDataID>{7821eb13-4e78-41e6-9c75-a418f8db10bb}</MetaDataID>
        IUser SignUp(UserData userData, RoleType roleType);

        /// <MetaDataID>{ac99ba38-5fe1-4ce7-822e-dd38904b32b4}</MetaDataID>
        void UpdateUserProfile(UserData userData, RoleType roleType);
#endif


        /// <MetaDataID>{bfa334fa-25de-4c63-ba62-c674cdba65f6}</MetaDataID>
        IOrganization SignUpWorker(WorkerData organizationData);

        /// <MetaDataID>{8aeb8cf3-4a39-48df-a68a-8d8797a2d2f4}</MetaDataID>
        IOrganization SignUpFounder(OrganizationData organizationData);

        /// <MetaDataID>{5af6bfb5-fa1d-40ad-9ddd-03f19123856f}</MetaDataID>
        IOrganization SignInFounder();


        /// <MetaDataID>{da820633-2290-4609-9b9b-de0f00193f58}</MetaDataID>
        UserData SignIn();

#if DEBUG
        /// <MetaDataID>{c7fbafbc-99aa-462d-9f37-921e0abfb09c}</MetaDataID>
        UserData SignInUser(string userID);
#endif

        /// <MetaDataID>{31e46cb8-f1b4-4b4a-9d87-758884a18a42}</MetaDataID>
        UserData SignUp(UserData userData);

        /// <MetaDataID>{37c5c4ad-adba-4316-85e3-98480b585e86}</MetaDataID>
        EndUsers.IFoodServiceClient SignInEndUser();


        /// <MetaDataID>{67cece1c-8138-4d9c-be89-22d4ddb00ae3}</MetaDataID>
        HumanResources.IServiceContextSupervisor SignInServiceContextSupervisor();

        /// <MetaDataID>{5cc3b4eb-df38-484a-8992-9440b7eae0b9}</MetaDataID>
        IFoodServiceClient SIMCardSignInEndUser(string simCardIdentity);

        /// <MetaDataID>{a41d6ec3-96b6-4383-aec9-dcd4b3f88bea}</MetaDataID>
        void UpdateFounderUserProfile(OrganizationData organizationData);




        /// <MetaDataID>{3ee61f46-f60e-47a1-a9f5-33ef4e7d2f52}</MetaDataID>
        void UpdateEndUserProfile(EndUserData endUserDataData);

        /// <MetaDataID>{8427508a-0738-4f55-b42b-1938a56817d7}</MetaDataID>
        //string GetMessage(string name, int age, IOrganization pok, out string message);
        string GetMessage(string name, int age, IOrganization pok);
        IList<UserData> GetNativeUsers(string serviceContextIdentity, RoleType roleType);
    }
}