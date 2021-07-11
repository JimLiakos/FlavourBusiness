using FlavourBusinessFacade;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using System;

namespace FlavourBusinessManager
{
    /// <MetaDataID>{6f3f34a0-317e-410b-809d-1e884c34926c}</MetaDataID>
    [BackwardCompatibilityID("{6f3f34a0-317e-410b-809d-1e884c34926c}")]
    [Persistent()]
    public class FlavourBusinessStorage : MarshalByRefObject, OOAdvantech.Remoting.IExtMarshalByRefObject
    {

        /// <exclude>Excluded</exclude>
        string _Version;
        /// <MetaDataID>{5d951fa1-7e4c-4eba-9710-a02d021619d5}</MetaDataID>
        [PersistentMember(nameof(_Version))]
        [BackwardCompatibilityID("+6")]
        public string Version
        {
            get
            {
                return _Version;
            }
            set
            {
                if (_Version != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Version = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }



        /// <exclude>Excluded</exclude>
        string _Description;
        /// <MetaDataID>{0ae0b1ed-c48f-4ac0-93b7-f8966d38b1c6}</MetaDataID>
        [PersistentMember(nameof(_Description))]
        [BackwardCompatibilityID("+5")]
        public string Description
        {
            get
            {
                return _Description;
            }

            set
            {

                if (_Description != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Description = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <exclude>Excluded</exclude>
        Organization _Owner;
        [PersistentMember(nameof(_Owner))]
        [Association("OrganizationStorages", Roles.RoleB, "6e017516-e7a5-4d27-895d-b5438b1d91a9")]
        [RoleBMultiplicityRange(1, 1)]
        public Organization Owner
        {
            get
            {
                return _Owner;
            }
            set
            {


                if (_Owner != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Owner = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <exclude>Excluded</exclude>
        string _Url;
        /// <MetaDataID>{aed90b06-dd49-4146-a1aa-bebddd1cbd59}</MetaDataID>
        [PersistentMember(nameof(_Url))]
        [BackwardCompatibilityID("+4")]
        public string Url
        {
            get
            {
                return _Url;
            }
            set
            {
                if (_Url != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Url = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        string _StorageIdentity;
        /// <MetaDataID>{1f1cf564-a93c-4cd9-a946-1d9d5a0e3119}</MetaDataID>
        [PersistentMember(nameof(_StorageIdentity))]
        [BackwardCompatibilityID("+1")]
        public string StorageIdentity
        {
            get
            {
                return _StorageIdentity;
            }
            set
            {

                if (_StorageIdentity != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _StorageIdentity = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }


        /// <exclude>Excluded</exclude>
        string _Name;
        /// <MetaDataID>{f727f53b-de97-452d-90e8-067d92022b2f}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        [PersistentMember(nameof(_Name))]
        public string Name
        {
            get
            {
                return _Name;
            }
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
        OrganizationStorages _FlavourStorageType;
        /// <MetaDataID>{d9a5ad1c-8e7d-4426-90d3-7064631604b5}</MetaDataID>
        [PersistentMember(nameof(_FlavourStorageType))]
        [BackwardCompatibilityID("+3")]
        public OrganizationStorages FlavourStorageType
        {
            get
            {
                return _FlavourStorageType;
            }
            set
            {
                if (_FlavourStorageType != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _FlavourStorageType = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
    }
}