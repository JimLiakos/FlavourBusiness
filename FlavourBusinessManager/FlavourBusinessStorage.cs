using FlavourBusinessFacade;
using Microsoft.Azure.Storage;
using OOAdvantech.Collections.Generic;
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

        public string BlobName
        {
            get
            {
                string blobName = Url;
                int index = blobName.LastIndexOf("/");
                if (index != -1)
                    blobName = blobName.Substring(index + 1);

                if (blobName.LastIndexOf('.') != -1)
                    return blobName.Substring(0, blobName.LastIndexOf('.'));
                else
                    return blobName;

            }
        }



        /// <MetaDataID>{170c2448-eef2-411a-a67e-c4d1e102b472}</MetaDataID>
        [ObjectActivationCall]
        public void ObjectActivation()
        {
            if (!string.IsNullOrWhiteSpace(PropertiesValuesJson))
                _PropertiesValues = OOAdvantech.Json.JsonConvert.DeserializeObject<System.Collections.Generic.Dictionary<string, string>>(PropertiesValuesJson);

        }
        /// <MetaDataID>{bc68a14c-7ad5-4858-a22b-f4caed9ea9f8}</MetaDataID>
        [BeforeCommitObjectStateInStorageCall]
        public void BeforeCommitObjectState()
        {
            PropertiesValuesJson = OOAdvantech.Json.JsonConvert.SerializeObject(_PropertiesValues);
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

        /// <MetaDataID>{f0bdb2b7-ac88-4baa-a9ea-74a59843a52c}</MetaDataID>
        [PersistentMember]
        [BackwardCompatibilityID("+7")]
        public string PropertiesValuesJson;
        
        /// <exclude>Excluded</exclude>
        System.Collections.Generic.Dictionary<string, string> _PropertiesValues = new System.Collections.Generic.Dictionary<string, string>();
        /// <MetaDataID>{95e0e1b9-d359-47a7-ab7b-e70139c407ae}</MetaDataID>
        public System.Collections.Generic.Dictionary<string, string> PropertiesValues { get => new System.Collections.Generic.Dictionary<string, string>(_PropertiesValues); }


        /// <MetaDataID>{6564e7c2-a81c-47cb-a413-4d4ce9fab9ed}</MetaDataID>
        public string GetPropertyValue(string name)
        {
            string value = null;
            _PropertiesValues.TryGetValue(name, out value);
            return value;
        }
        /// <MetaDataID>{efa9bfb6-4b72-441f-a6e6-2440d9cd43cb}</MetaDataID>
        public void SetPropertyValue(string name, string value)
        {
            _PropertiesValues[name] = value;
        }
        /// <MetaDataID>{a53fd071-66cc-48a5-bb48-6433e7f7e39d}</MetaDataID>
        public void RemoveProperty(string name)
        {
            _PropertiesValues.Remove(name);
        }
    }
}