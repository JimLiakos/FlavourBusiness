using System;
using System.Collections.Generic;
using System.Linq;
using FlavourBusinessFacade.EndUsers;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;

namespace FlavourBusinessManager.EndUsers
{
    /// <MetaDataID>{c502c6c0-09cc-4a1f-925f-be44fa7644ea}</MetaDataID>
    [BackwardCompatibilityID("{c502c6c0-09cc-4a1f-925f-be44fa7644ea}")]
    [Persistent()]
    public class FoodServiceClient : MarshalByRefObject, OOAdvantech.Remoting.IExtMarshalByRefObject, IFoodServiceClient
    {
        /// <MetaDataID>{e74570a3-f55a-4203-9ec7-d9ba5d25cb17}</MetaDataID>
        [PersistentMember()]
        [BackwardCompatibilityID("+9")]
        private string DeliveryPlacesJson;

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


        /// <exclude>Excluded</exclude>
        List<Place> _DeliveryPlaces = new List<Place>();

        /// <MetaDataID>{972042e9-c407-4532-bdf2-2db29f6e4e11}</MetaDataID>
        public List<IPlace> DeliveryPlaces
        {
            get
            {
                lock (this)
                    return _DeliveryPlaces.OfType<IPlace>().ToList();
            }
        }

        /// <exclude>Excluded</exclude>
        string _FriendlyName;

        /// <MetaDataID>{db910cf4-9afe-4c40-b4e5-e4c1e266c131}</MetaDataID>
        [PersistentMember(nameof(_FriendlyName))]
        [BackwardCompatibilityID("+10")]
        public string FriendlyName
        {
            get=> _FriendlyName;
            set
            {
                if (_FriendlyName != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _FriendlyName = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <MetaDataID>{7e30445f-1aeb-4ff7-91b2-b83606628ad5}</MetaDataID>
        public void RemoveDeliveryPlace(IPlace place)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                lock (this)
                    _DeliveryPlaces.Remove(place as Place);

                stateTransition.Consistent = true;
            }
        }

        /// <MetaDataID>{c6c177e9-725f-4173-8520-ecd11726358b}</MetaDataID>
        public void AddDeliveryPlace(IPlace place)
        {
            if (place == null)
                return;
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                var existingPlace = _DeliveryPlaces.Where(x => x.PlaceID == place.PlaceID).FirstOrDefault();
                if (existingPlace != null)
                    UpdateDeliveryPlace(place);
                else
                {
                    lock (this)
                        _DeliveryPlaces.Add(place as Place);
                }
                stateTransition.Consistent = true;
            }
        }
        /// <MetaDataID>{6af560ea-96da-40f1-9158-65753afed15c}</MetaDataID>
        [BeforeCommitObjectStateInStorageCall]
        void BeforeCommitObjectState()
        {
            lock (this)
                DeliveryPlacesJson = OOAdvantech.Json.JsonConvert.SerializeObject(_DeliveryPlaces);

        }
        /// <MetaDataID>{0524d783-69ed-4402-8f54-8123c7718d24}</MetaDataID>
        [ObjectActivationCall]
        public void ObjectActivation()
        {
            if (DeliveryPlacesJson != null)
                _DeliveryPlaces = OOAdvantech.Json.JsonConvert.DeserializeObject<List<Place>>(DeliveryPlacesJson);
            else
                _DeliveryPlaces = new List<Place>();

        }

        /// <MetaDataID>{27e725df-0785-4f1d-bd67-59df24ba43ee}</MetaDataID>
        public void UpdateDeliveryPlace(IPlace place)
        {
            Place existingPlace = null;
            lock (this)
                existingPlace = _DeliveryPlaces.Where(x => x.PlaceID == place.PlaceID).FirstOrDefault();
            if (existingPlace != null)
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    existingPlace.Area = place.Area;
                    existingPlace.CityTown = place.CityTown;
                    existingPlace.Country = place.Country;
                    existingPlace.Description = place.Description;
                    existingPlace.Location = place.Location;
                    existingPlace.PlaceID = place.PlaceID;
                    existingPlace.PostalCode = place.PostalCode;
                    existingPlace.StateProvinceRegion = place.StateProvinceRegion;
                    existingPlace.Street = place.Street;
                    existingPlace.StreetNumber = place.StreetNumber;

                    stateTransition.Consistent = true;
                }

            }


        }
    }
}