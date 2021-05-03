using FlavourBusinessFacade.HumanResources;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlavourBusinessManager.HumanResources
{
    /// <MetaDataID>{9070121c-6931-42b3-aa32-34f2de7e2ed6}</MetaDataID>
    [BackwardCompatibilityID("{9070121c-6931-42b3-aa32-34f2de7e2ed6}")]
    [Persistent()]
    public class Translator :MarshalByRefObject,  OOAdvantech.Remoting.IExtMarshalByRefObject, FlavourBusinessFacade.IUser, ITranslator
    {
      

        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<IAccountability> _Responsibilities = new OOAdvantech.Collections.Generic.Set<IAccountability>();

        /// <MetaDataID>{94c35ea1-a6b5-41d7-b2ed-6262d10985bc}</MetaDataID>
        [PersistentMember(nameof(_Responsibilities))]
        [BackwardCompatibilityID("+10")]
       
        public List<IAccountability> Responsibilities => _Responsibilities.ToThreadSafeList();




        /// <exclude>Excluded</exclude>
        string _PhotoUrl;
        /// <MetaDataID>{ebe177b8-4b8e-4420-bc94-a479e8984a9d}</MetaDataID>
        [PersistentMember(nameof(_PhotoUrl))]
        [BackwardCompatibilityID("+8")]
        public string PhotoUrl
        {
            get => _PhotoUrl;
            set
            {

                if (_PhotoUrl != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _PhotoUrl = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        string _SignUpUserIdentity;

        /// <MetaDataID>{53da5ab7-d83b-43df-b84d-728a67f2a6f9}</MetaDataID>
        [PersistentMember(nameof(_SignUpUserIdentity))]
        [BackwardCompatibilityID("+7")]
        public string SignUpUserIdentity
        {
            get => _SignUpUserIdentity;
            set
            {

                if (_SignUpUserIdentity != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _SignUpUserIdentity = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        string _Name;
        /// <MetaDataID>{2989d348-e300-4e02-91bf-0cab63610141}</MetaDataID>
        [PersistentMember(nameof(_Name))]
        [BackwardCompatibilityID("+6")]
        public string Name
        {
            get => _Name;
            set
            {

                if (_Name != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Name = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;



        /// <exclude>Excluded</exclude>
        string _FullName;

        /// <MetaDataID>{d7191045-389d-40cc-a860-4efa4d60de5f}</MetaDataID>
        [PersistentMember(nameof(_FullName))]
        [BackwardCompatibilityID("+1")]
        public string FullName
        {
            get => _FullName;
            set {

                if (_FullName != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _FullName = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <exclude>Excluded</exclude>
        string _Email;

        /// <MetaDataID>{6eee5c16-c8f0-4c66-b232-f8dee0c91de2}</MetaDataID>
        [PersistentMember(nameof(_Email))]
        [BackwardCompatibilityID("+2")]
        public string Email
        {
            get => _Email;
            set 
            {
                if (_Email != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Email = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <exclude>Excluded</exclude>
        string _PhoneNumber;

        /// <MetaDataID>{c46b6ab3-a9e6-4b58-8d16-b902d444a75a}</MetaDataID>
        [PersistentMember(nameof(_PhoneNumber))]
        [BackwardCompatibilityID("+3")]
        public string PhoneNumber
        {
            get => _PhoneNumber;
            set
            {
                if (_PhoneNumber != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _PhoneNumber = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <exclude>Excluded</exclude>
        string _UserName;
        /// <MetaDataID>{dd79b80f-e677-4662-a497-392d6e6433c9}</MetaDataID>
        [PersistentMember(nameof(_UserName))]
        [BackwardCompatibilityID("+4")]
        public string UserName
        {
            get => _UserName;
            set
            {
                if (_UserName != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _UserName = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <exclude>Excluded</exclude>
        string _Identity;

        /// <MetaDataID>{ac669162-3b51-4d99-97a6-558ccd6031e8}</MetaDataID>
        [PersistentMember(nameof(_Identity))]
        [BackwardCompatibilityID("+5")]
        public string Identity
        {
            get
            {
                if (_Identity == null)
                {

                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Identity = Guid.NewGuid().ToString("N");
                        stateTransition.Consistent = true;
                    }
                }
                return _Identity;
            }
        }
        /// <MetaDataID>{ec178358-9bf6-4568-bd1f-6747629e4669}</MetaDataID>
        public void RemoveTranslationActivity(ITranslationActivity activity)
        {
            //using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            //{
            //    _Activities.Remove(activity);
            //    stateTransition.Consistent = true;
            //}
        }


        /// <exclude>Excluded</exclude>
        System.Collections.Generic.List<FlavourBusinessFacade.UserData.UserRole> _Roles;
        /// <MetaDataID>{a5f72e68-67e0-4261-b275-d81fc66c55b1}</MetaDataID>
        public System.Collections.Generic.List<FlavourBusinessFacade.UserData.UserRole> Roles
        {
            get
            {
                if (_Roles == null)
                {
                    FlavourBusinessFacade.UserData.UserRole role = new FlavourBusinessFacade.UserData.UserRole() { User = this, RoleType = FlavourBusinessFacade.UserData.UserRole.GetRoleType(GetType().FullName) };
                    _Roles = new List<FlavourBusinessFacade.UserData.UserRole>() { role };
                }
                return _Roles;
            }
        }

        /// <MetaDataID>{c6a669f4-5d61-4b21-826b-fdab7021f2e1}</MetaDataID>
        [BackwardCompatibilityID("+11")]
        public List<IAccountability> Commissions => new List<IAccountability>();



        /// <MetaDataID>{8cea5fd0-3c19-48fc-a63c-2517795944a0}</MetaDataID>
        public IActivity NewTranslationActivity(string subjectDescription, TranslationType selectedTranslationType, string subjcectIdentity)
        {

            //TranslationActivity translationActivity = Activities.OfType<TranslationActivity>().Where(x => x.TranslationIdentity == subjcectIdentity).FirstOrDefault();

            //using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            //{
            //    if (translationActivity == null)
            //    {
            //        translationActivity = new TranslationActivity();
            //        OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(translationActivity);
            //        _Activities.Add(translationActivity);
            //    }
            //    translationActivity.Name = subjectDescription;
            //    translationActivity.TranslationType = selectedTranslationType;
            //    translationActivity.TranslationIdentity = subjcectIdentity;

            //    stateTransition.Consistent = true;
            //}


            //return translationActivity;

            return null;
        }
    }
}