using FlavourBusinessFacade.HumanResources;
using OOAdvantech.Collections.Generic;
using System;
using System.Linq;
namespace FlavourBusinessFacade
{
    /// <MetaDataID>{68a3f5dc-c3f5-4634-a876-a90b6a82bc37}</MetaDataID>
    public class UserData:IUser
    {

        public string UserName { get; set; }


        public string Password { get; set; }

        /// <MetaDataID>{9e96b5f9-98c1-4c91-b2e1-9c5036007374}</MetaDataID>
        public string PhoneNumber { get; set; }

        /// <MetaDataID>{7287339e-9342-4aa4-b566-e763a901e2b4}</MetaDataID>
        public string Identity { get; set; }


        /// <MetaDataID>{c7261650-0847-4085-b0a4-1ea33707ad14}</MetaDataID>
        public string FullName { get; set; }


        /// <MetaDataID>{6acf5261-52d1-4ea7-aa7d-51d34a54a337}</MetaDataID>
        public string Email { get; set; }


        /// <MetaDataID>{5d727a5e-98eb-4c46-9963-3a95ac6eef35}</MetaDataID>
        public System.Collections.Generic.List<UserRole> Roles { get; set; }
        /// <MetaDataID>{c373e288-7016-4e14-8b04-0f114096a204}</MetaDataID>
        public string PhotoUrl { get; set; }


        public string Trademark { get; set; }

        public string Address { get; set; }

        public string OAuthUserIdentity => Identity;

        //public string OAuthUserIdentity { get; set; }

        object rolesLock = new object();

        public IUser GetRoleObject(RoleType roleType)
        {
            lock (rolesLock)
            {
                if(Roles==null)
                    return default(IUser);
                return Roles.Where(x => x.RoleType == roleType).Select(x=>x.User).FirstOrDefault();
            }
        }



     

        public struct UserRole
        {
            public IUser User { get;  set; }
            public RoleType RoleType { get; set; }

            public static RoleType GetRoleType(string typeFullName)
            {
                if ("FlavourBusinessManager.HumanResources.ServiceContextSupervisor" == typeFullName)
                    return RoleType.ServiceContextSupervisor;
                if ("FlavourBusinessManager.Organization" == typeFullName)
                    return RoleType.Organization;
                if ("FlavourBusinessManager.HumanResources.Waiter" == typeFullName)
                    return RoleType.Waiter;

                if ("FlavourBusinessManager.HumanResources.ServiceContextSupervisor" == typeFullName)
                    return RoleType.ServiceContextSupervisor;
                if ("FlavourBusinessManager.Organization" == typeFullName)
                    return RoleType.Organization;
                if ("FlavourBusinessManager.HumanResources.MenuMaker" == typeFullName)
                    return RoleType.MenuMaker;
                if ("FlavourBusinessManager.EndUsers.FoodServiceClient" == typeFullName)
                    return RoleType.EndUser;
                if ("FlavourBusinessManager.HumanResources.TakeawayCashier" == typeFullName)
                    return RoleType.TakeAwayCashier;

                

                return RoleType.Unknown;
            }


        }







    }

    public enum RoleType
    {
        Unknown = 0,
        Organization = 0b000001,
        ServiceContextSupervisor = 0b000010,
        Waiter = 0b000100,
        EndUser = 0b001000,
        MenuMaker = 0b010000,
        TakeAwayCashier = 0b100000

    }
}