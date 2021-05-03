using System;

using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;

namespace FlavourBusinessManager.EndUsers
{
    /// <MetaDataID>{c502c6c0-09cc-4a1f-925f-be44fa7644ea}</MetaDataID>
    [BackwardCompatibilityID("{c502c6c0-09cc-4a1f-925f-be44fa7644ea}")]
    [Persistent()]
    public class FoodServiceClient : MarshalByRefObject, OOAdvantech.Remoting.IExtMarshalByRefObject, FlavourBusinessFacade.EndUsers.IFoodServiceClient
    {

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <MetaDataID>{9340b238-961b-45a0-8969-8a72855f7343}</MetaDataID>
        public FoodServiceClient(string identity)
        {
            _Identity = identity;
        }
        /// <MetaDataID>{9391b1e9-f016-4559-bc03-29894745e447}</MetaDataID>
        protected FoodServiceClient()
        {

        }


        /// <exclude>Excluded</exclude>
        string _Email;
        /// <MetaDataID>{ba5914ea-127c-40af-97d8-cd87df46fff2}</MetaDataID>
        [PersistentMember(nameof(_Email))]
        [BackwardCompatibilityID("+2")]
        public string Email
        {
            get
            {
                return _Email;
            }
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
        string _Address;
        /// <MetaDataID>{cdbcb435-54f3-49c9-b71c-9cd5ac88d844}</MetaDataID>
        [PersistentMember(nameof(_Address))]
        [BackwardCompatibilityID("+1")]
        public string Address
        {
            get
            {
                return _Address;
            }

            set
            {
                if (_Address != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Address = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        //{
        //    get
        //    {
        //        throw new NotImplementedException();
        //    }

        //    set
        //    {
        //        throw new NotImplementedException();
        //    }
        //}
        /// <exclude>Excluded</exclude>
        string _Identity;

        /// <MetaDataID>{1da2bbe4-9228-4888-a11e-18306e07dafb}</MetaDataID>
        [PersistentMember(nameof(_Identity))]
        [BackwardCompatibilityID("+3")]
        public string Identity
        {
            get
            {
                return _Address;
            }
        }

        /// <exclude>Excluded</exclude>
        string _Name;
        /// <MetaDataID>{48f89f72-084a-40c4-bda0-681ce3ce956d}</MetaDataID>
        [PersistentMember(nameof(_Name))]
        [BackwardCompatibilityID("+4")]
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
        string _PhoneNumber;

        /// <MetaDataID>{1076926b-7c1b-434d-9f86-670d4bd52979}</MetaDataID>
        [PersistentMember(nameof(_PhoneNumber))]
        [BackwardCompatibilityID("+5")]
        public string PhoneNumber
        {
            get
            {
                return _PhoneNumber;
            }
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
        FlavourBusinessFacade.EndUsers.SIMCardData _SIMCardData;

        /// <MetaDataID>{2a53d30e-a7f9-4aa8-87f3-acd7e1dc649d}</MetaDataID>
        [PersistentMember(nameof(_SIMCardData))]
        [BackwardCompatibilityID("+7")]
        public FlavourBusinessFacade.EndUsers.SIMCardData SIMCardData
        {
            get
            {
                return _SIMCardData;
            }

            set
            {

                if (_SIMCardData != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _SIMCardData = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }
    }
}