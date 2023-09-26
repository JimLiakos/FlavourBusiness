using System;
using FlavourBusinessFacade;
using FlavourBusinessFacade.EndUsers;
using FlavourBusinessFacade.HumanResources;
using OOAdvantech.Authentication;


namespace FlavourBusinessFacade
{
    /// <MetaDataID>{f2053e50-ea7c-4343-9750-fcecc9dceaf1}</MetaDataID>


    public class OrganizationsManager : OOAdvantech.Remoting.MonoStateClass, IAuthFlavourBusiness
    {

        public OrganizationsManager()
        {
        }

        /// <MetaDataID>{1efea50f-aede-4866-b70d-f39d8c2e09b8}</MetaDataID>
        public static IAuthFlavourBusiness AuthFlavourBusiness;

        /// <MetaDataID>{545e7fa2-95c7-4f2b-830e-d24e0081c378}</MetaDataID>
        public IOrganization SignInFounder()
        {
            return AuthFlavourBusiness.SignInFounder();
        }

        public IFoodServiceClient SignInEndUser()
        {
            return AuthFlavourBusiness.SignInEndUser();
        }


        /// <MetaDataID>{9c9852ae-d46a-42d1-a60f-5743c7527bdd}</MetaDataID>
        public IFoodServiceClient SignUpEndUser(EndUserData organizationData)
        {
            return AuthFlavourBusiness.SignUpEndUser(organizationData);
        }

        /// <MetaDataID>{bafa448f-c9ad-4707-9b70-ff0146666a1d}</MetaDataID>
        public IOrganization SignUpFounder(OrganizationData organizationData)
        {
            return AuthFlavourBusiness.SignUpFounder(organizationData);
        }

        /// <MetaDataID>{f81b4689-5893-4728-a746-4480b238815d}</MetaDataID>
        public IOrganization SignUpWorker(WorkerData organizationData)
        {
            return AuthFlavourBusiness.SignUpWorker(organizationData);
        }

        public void UpdateFounderUserProfile(OrganizationData organizationData)
        {
            AuthFlavourBusiness.UpdateFounderUserProfile(organizationData);
        }

        public void UpdateEndUserProfile(EndUserData endUserDataData)
        {
            AuthFlavourBusiness.UpdateEndUserProfile(endUserDataData);
        }

        public System.Collections.Generic.IList<UserData> GetNativeUsers(string serviceContextIdentity, RoleType roleType)
        {
            return AuthFlavourBusiness.GetNativeUsers(serviceContextIdentity, RoleType.Courier);
        }

        public string GetMessage(string name, int age, IOrganization pok)
        {
            return AuthFlavourBusiness.GetMessage(name, age, pok);
        }


        public IFoodServiceClient SIMCardSignInEndUser(string simCardIdentity)
        {
            return AuthFlavourBusiness.SIMCardSignInEndUser(simCardIdentity);
        }

        public IServiceContextSupervisor SignInServiceContextSupervisor()
        {
            return AuthFlavourBusiness.SignInServiceContextSupervisor();
        }

        public UserData SignIn()
        {
            return AuthFlavourBusiness.SignIn();
        }

        public UserData SignUp(UserData userData)
        {
            return AuthFlavourBusiness.SignUp(userData);
        }
#if DEBUG
        public UserData SignInUser(string userID)
        {
            return AuthFlavourBusiness.SignInUser(userID);
        }

        public UserData GetUser(string userName)
        {
            return AuthFlavourBusiness.GetUser(userName);
        }

        public IUser SignIn(RoleType roleType)
        {
            return AuthFlavourBusiness.SignIn(roleType);
        }

        public IUser SignUp(UserData userData, RoleType roleType)
        {
            return AuthFlavourBusiness.SignUp(userData, roleType);
        }

        public void SendVerificationEmail(string emailAddress)
        {
            AuthFlavourBusiness.SendVerificationEmail(emailAddress);
        }

        public void SignUpUserWithEmailAndPassword(string email, string password, UserData userData, string verificationCode)
        {
            AuthFlavourBusiness.SignUpUserWithEmailAndPassword(email, password, userData, verificationCode);
        }

        public void UpdateUserProfile(UserData userData, RoleType roleType)
        {
            AuthFlavourBusiness.UpdateUserProfile(userData, roleType);
        }

        public bool IsUsernameInUse(string username, SignInProvider signInProvider)
        {
            return AuthFlavourBusiness.IsUsernameInUse(username, signInProvider);
        }

        public UserData SignInNativeUser(string serviceContextIdentity, string userName, string password)
        {
            return AuthFlavourBusiness.SignInNativeUser(serviceContextIdentity, userName, password);
        }
#endif
    }


    /// <MetaDataID>{aaed326f-6cad-42e7-b168-3cec677b3ea7}</MetaDataID>
    [OOAdvantech.MetaDataRepository.HttpVisible]
    public class OrganizationData
    {
        /// <MetaDataID>{93e5c95f-f826-4e81-959f-16173ed5778b}</MetaDataID>
        public string Email { get; set; }
        /// <MetaDataID>{b2454a42-10ca-4b94-87df-0953c00052db}</MetaDataID>
        public string FullName { get; set; }
        /// <MetaDataID>{5cb66168-4ae5-4752-a829-d8af1d544004}</MetaDataID>
        public string Trademark { get; set; }

        /// <MetaDataID>{476d0645-6f75-4dfe-a974-a7abbdd6579d}</MetaDataID>
        public string Address { get; set; }

        /// <MetaDataID>{17337f97-fd6f-4f18-8692-8fd2460107b7}</MetaDataID>
        public string PhoneNumber { get; set; }


    }
}