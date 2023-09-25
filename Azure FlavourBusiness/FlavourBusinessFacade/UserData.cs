using FlavourBusinessFacade.HumanResources;
using OOAdvantech.Collections.Generic;
using System;
using System.Linq;
namespace FlavourBusinessFacade
{
    /// <MetaDataID>{68a3f5dc-c3f5-4634-a876-a90b6a82bc37}</MetaDataID>
    public class UserData:IUser
    {

        /// <MetaDataID>{528fe583-6574-47f2-aa4b-c84174f2d76d}</MetaDataID>
        public string UserName { get; set; }


        /// <MetaDataID>{fd4f5d74-36ce-4a65-9e62-a15b1dfa6bc8}</MetaDataID>
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


        /// <MetaDataID>{f58f1ca4-0790-4dc9-b672-cb2732c0f33f}</MetaDataID>
        public string Trademark { get; set; }

        /// <MetaDataID>{8e9fcb10-61f8-4805-814a-a8e3a44d246b}</MetaDataID>
        public string Address { get; set; }

        /// <MetaDataID>{2b82ed7b-31d9-4e8a-81a5-74de6c4f236e}</MetaDataID>
        public string OAuthUserIdentity => Identity;

        //public string OAuthUserIdentity { get; set; }

        /// <MetaDataID>{908446b2-0dac-4b19-9208-c07b89fcd96c}</MetaDataID>
        object rolesLock = new object();

        /// <MetaDataID>{89b791d2-f9a9-4c77-b295-3dc06dc15fd1}</MetaDataID>
        public IUser GetRoleObject(RoleType roleType)
        {
            lock (rolesLock)
            {
                if (Roles==null)
                    return default(IUser);
                return Roles.Where(x => x.RoleType == roleType).Select(x => x.User).FirstOrDefault();
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
                if ("FlavourBusinessManager.HumanResources.Courier" == typeFullName)
                    return RoleType.Courier;




                return RoleType.Unknown;
            }


        }







    }

    /// <MetaDataID>{cb914bfc-3a7a-4c5e-8c4b-97a03a9cdcf5}</MetaDataID>
    public enum RoleType
    {
        Unknown = 0,
        Organization = 0b0000001,
        ServiceContextSupervisor = 0b0000010,
        Waiter = 0b0000100,
        EndUser = 0b0001000,
        MenuMaker = 0b0010000,
        TakeAwayCashier = 0b0100000,
        Courier = 0b1000000

    }
}