using FirebaseAdmin.Auth;
using OOAdvantech.Authentication;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using System;

namespace FlavourBusinessManager
{
    /// <MetaDataID>{ceb3c975-18f5-4a16-b67d-90ce4f29185e}</MetaDataID>
    [BackwardCompatibilityID("{ceb3c975-18f5-4a16-b67d-90ce4f29185e}")]
    [Persistent()]
    public class NativeAuthUser : FlavourBusinessFacade.INativeAuthUser
    {
        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;


        /// <MetaDataID>{968d050a-111c-4d82-82d6-ca25b56f4687}</MetaDataID>
        public NativeAuthUser()
        {

        }
        /// <MetaDataID>{37ef69d9-5bcf-4e04-92cf-e51a6eb4da50}</MetaDataID>
        public NativeAuthUser(string userName, string password, string userFullName)
        {
            UserName=userName;
            Password=password;
            UserFullName=userFullName;
        }

        /// <exclude>Excluded</exclude>
        string _UserName;

        /// <MetaDataID>{1af5a0fe-8c69-4f14-84a8-fa1c7f21e96d}</MetaDataID>
        [PersistentMember(nameof(_UserName))]
        [BackwardCompatibilityID("+1")]
        public string UserName
        {
            get => _UserName; set
            {
                if (_UserName!=value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _UserName=value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }



        /// <exclude>Excluded</exclude>
        string _Password;


        /// <MetaDataID>{413b971b-6a23-418d-8217-1d362375a2ee}</MetaDataID>
        [PersistentMember(nameof(_Password))]
        [BackwardCompatibilityID("+2")]
        public string Password
        {
            get => _Password; set
            {
                if (_Password!=value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Password=value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        string _UserFullName;


        /// <MetaDataID>{b6f2b05a-dd34-4231-a44a-13426263c386}</MetaDataID>
        [PersistentMember(nameof(_UserFullName))]
        [BackwardCompatibilityID("+3")]
        public string UserFullName
        {
            get => _UserName; set
            {
                if (_UserName!=value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _UserName=value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <exclude>Excluded</exclude>
        string _FireBaseUserName;


        /// <MetaDataID>{7d0f4f4c-d86a-4cc9-960b-657997cc7bef}</MetaDataID>
        [PersistentMember(nameof(_FireBaseUserName))]
        [BackwardCompatibilityID("+4")]
        public string FireBaseUserName
        {
            get => _FireBaseUserName; set
            {
                if (_FireBaseUserName!=value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _FireBaseUserName=value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <exclude>Excluded</exclude>
        string _FireBasePasword;



        /// <MetaDataID>{8131869e-e4a8-401d-8e47-713f1e2523dd}</MetaDataID>
        [PersistentMember(nameof(_FireBasePasword))]
        [BackwardCompatibilityID("+5")]
        public string FireBasePasword
        {
            get => _FireBasePasword; set
            {
                if (_FireBasePasword!=value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _FireBasePasword=value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <MetaDataID>{eb0a6728-600a-450a-9fe6-e8dfbc80e0af}</MetaDataID>
        public void CreateFirebaseEmailUserCredential()
        {

            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                var ticks = new DateTime(2022, 1, 1).Ticks;
                var uniqueId = (DateTime.Now.Ticks - ticks).ToString("x");


                FireBaseUserName = ServicePointRunTime.ServicesContextRunTime.Current.FlavoursServicesContext.ContextStorageName+"_"+uniqueId+"@fakeuser.com";

                FireBasePasword=uniqueId;
                stateTransition.Consistent = true;
            }

        }

        //void sdsd()
        //{

        //    FirebaseAuthentication.Configuration.FirebaseConfiguration firebaseConfiguration = new FirebaseAuthentication.Configuration.FirebaseConfiguration();


        //    FirebaseAuth.DefaultInstance.pr






        //}



    }
}