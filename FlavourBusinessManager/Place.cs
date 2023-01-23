using FlavourBusinessFacade.EndUsers;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using System;
using System.Collections.Generic;
using System.IO;

namespace FlavourBusinessManager.EndUsers
{
    /// <MetaDataID>{affca228-cac8-42fa-be5b-7da4d5f457bb}</MetaDataID>
    [BackwardCompatibilityID("{affca228-cac8-42fa-be5b-7da4d5f457bb}")]
    [Persistent()]
    public class Place : FlavourBusinessFacade.EndUsers.IPlace
    {
      
        /// <MetaDataID>{3e6ca39a-d174-4656-85e5-19abdc70c408}</MetaDataID>
        public static bool AreSame(IPlace a, IPlace b)
        {
            if (a == null && b == null) return true;
            if (a != null && b == null) return false;
            if (a == null && b != null) return false;

            if (a.Location.Longitude != 0 && a.Location.Longitude == b.Location.Longitude && a.Location.Latitude == b.Location.Latitude)
                return true;

            if (a.Location.Longitude != 0 && (a.Location.Longitude != b.Location.Longitude || a.Location.Latitude != b.Location.Latitude))
                return false;

            if ((a.PostalCode != b.PostalCode) ||
                (a.PostalCode != b.PostalCode) ||
                (a.Street != b.Street) ||
                (a.StreetNumber != b.StreetNumber) ||
                (a.Area != b.Area) ||
                (a.CityTown != b.CityTown))
            {
                return false;
            }
            return true;
        }
        /// <MetaDataID>{1012419e-b619-497b-8a29-b6d2e6da2940}</MetaDataID>
        public Place() { }

        /// <MetaDataID>{08bc67e4-cd1e-484f-a046-68b9727d79df}</MetaDataID>
        static internal Place GetPlace(FlavourBusinessManager.EndUsers.PlaceData placeData)
        {
            if (placeData.Street == null && placeData.Area == null && placeData.Location.Latitude == 0 && placeData.Location.Longitude == 0)
                return null;

            Place place = new Place(placeData.PlaceID, placeData.Location, placeData.Country,
                                               placeData.StateProvinceRegion, placeData.CityTown,
                                               placeData.Area, placeData.PostalCode, placeData.Street,
                                               placeData.StreetNumber, placeData.Description,placeData.ExtensionProperties);
            return place;
        }

        /// <MetaDataID>{c0e14b79-a5f2-4e77-bc8d-9c641f0d0966}</MetaDataID>
        [OOAdvantech.Json.JsonProperty]
        internal Dictionary<string, string> ExtensionProperties { get; set; } = new Dictionary<string, string>();

        /// <MetaDataID>{e4b41e8d-fb26-4eb4-bf23-ffd85847fbf1}</MetaDataID>
        public string GetExtensionProperty(string name)
        {
            ExtensionProperties.TryGetValue(name, out var value);
            return value;
        }

        /// <MetaDataID>{544a21d2-2f1c-45ed-9d84-ccba3e9a4084}</MetaDataID>
        public void SetExtensionProperty(string name, string value)
        {
            if (value == null)
                RemovetExtensionProperty(name);
            else
                ExtensionProperties[name] = value;
        }

        /// <MetaDataID>{b069a2b8-d20e-4f8f-b670-778298cc61c4}</MetaDataID>
        public void RemovetExtensionProperty(string name)
        {
            ExtensionProperties.Remove(name);
        }


        /// <MetaDataID>{965e770a-aa83-44a5-939c-91fc0ef241c7}</MetaDataID>
        [PersistentMember()]
        [BackwardCompatibilityID("+12")]
        private string ExtensionPropertiesJson;


        /// <MetaDataID>{a49f7465-3395-43a9-8b4e-2f3191fb004e}</MetaDataID>
        [ObjectActivationCall]
        public void ObjectActivation()
        {
            ExtensionProperties=OOAdvantech.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(ExtensionPropertiesJson);
        }
        /// <MetaDataID>{ba7a9880-bfc8-4f5f-85b9-ffbe7faa8d55}</MetaDataID>
        [BeforeCommitObjectStateInStorageCall]
        void BeforeCommitObjectState()
        {
            ExtensionPropertiesJson=OOAdvantech.Json.JsonConvert.SerializeObject(ExtensionProperties);
        }

        public void Update(IPlace value)
        {


            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                if (_PlaceID == value.PlaceID)
                {
                    _Location = value.Location;
                    _Country = value.Country;
                    _StateProvinceRegion = value.StateProvinceRegion;
                    _CityTown = value.CityTown;
                    _Area = value.Area;
                    _PostalCode = value.PostalCode;
                    _Street = value.Street;
                    _StreetNumber = value.StreetNumber;
                    _Description = value.Description;
                    ExtensionProperties= (value as Place).ExtensionProperties;
                }

                stateTransition.Consistent = true;
            }


        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;
        /// <exclude>Excluded</exclude> 
        string _CityTown;

        /// <MetaDataID>{7eb014e0-7a36-4c25-893a-412b359f6f15}</MetaDataID>
        [PersistentMember(nameof(_CityTown))]
        [BackwardCompatibilityID("+10")]
        public string CityTown
        {
            get => _CityTown;
            set
            {
                if (_CityTown != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _CityTown = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude> 
        string _StateProvinceRegion;
        /// <MetaDataID>{b4d5f3d4-5d69-47c1-946a-1ac148028d1c}</MetaDataID>
        [PersistentMember(nameof(_StateProvinceRegion))]
        [BackwardCompatibilityID("+9")]
        public string StateProvinceRegion
        {
            get => _StateProvinceRegion;
            set
            {
                if (_StateProvinceRegion != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _StateProvinceRegion = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }
        /// <exclude>Excluded</exclude> 
        string _Description;
        /// <MetaDataID>{f3172f6f-396c-4e7b-a697-9bd74fbde7be}</MetaDataID>
        [PersistentMember(nameof(_Description))]
        [BackwardCompatibilityID("+8")]
        public string Description
        {
            get => _Description;
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
        string _StreetNumber;
        /// <MetaDataID>{e7d750d4-0b65-4edb-b198-27c164054820}</MetaDataID>
        [PersistentMember(nameof(_StreetNumber))]
        [BackwardCompatibilityID("+7")]
        public string StreetNumber
        {
            get => _StreetNumber;
            set
            {
                if (_StreetNumber != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _StreetNumber = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude> 
        string _Street;

        /// <MetaDataID>{22c6a6d2-ad36-4f3e-b78b-04e17c227beb}</MetaDataID>
        [PersistentMember(nameof(_Street))]
        [BackwardCompatibilityID("+6")]
        public string Street
        {
            get => _Street;
            set
            {
                if (_Street != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Street = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude> 
        string _Area;
        /// <MetaDataID>{1ec77077-0630-4275-918e-fc097c49ea13}</MetaDataID>
        [PersistentMember(nameof(_Area))]
        [BackwardCompatibilityID("+5")]
        public string Area
        {
            get => _Area;
            set
            {
                if (_Area != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Area = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude> 
        string _PostalCode;
        /// <MetaDataID>{c9133f08-2ce2-423e-b717-4337fd0d29f8}</MetaDataID>
        [PersistentMember(nameof(_PostalCode))]
        [BackwardCompatibilityID("+4")]
        public string PostalCode
        {
            get => _PostalCode;
            set
            {
                if (_PostalCode != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _PostalCode = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude> 
        string _Country;
        /// <MetaDataID>{a7e70d5f-0a18-47c2-b0ec-497843d1f5ab}</MetaDataID>
        [PersistentMember(nameof(_Country))]
        [BackwardCompatibilityID("+3")]
        public string Country
        {
            get => _Country;
            set
            {
                if (_Country != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Country = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude> 
        FlavourBusinessFacade.EndUsers.Coordinate _Location;
        /// <MetaDataID>{b9588635-cef0-48b6-b6b9-d12a5b4c011a}</MetaDataID>
        [PersistentMember(nameof(_Location))]
        [BackwardCompatibilityID("+2")]
        public FlavourBusinessFacade.EndUsers.Coordinate Location
        {
            get => _Location;
            set
            {
                if (_Location != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Location = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude> 
        string _PlaceID;
        /// <MetaDataID>{9407928d-6c94-4570-844d-cdd6b53534be}</MetaDataID>
        [PersistentMember(nameof(_PlaceID))]
        [BackwardCompatibilityID("+1")]
        public string PlaceID
        {
            get => _PlaceID;
            set
            {
                if (_PlaceID != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _PlaceID = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }


        /// <exclude>Excluded</exclude>
        bool _Default;

        /// <MetaDataID>{69840cba-a26b-43f7-ada5-18fc9bfce6ca}</MetaDataID>
        public Place(string placeID, Coordinate location, string country, string stateProvinceRegion, string cityTown, string area, string postalCode, string street, string streetNumber, string description, Dictionary<string, string> extensionProperties)
        {
            _PlaceID = placeID;
            _Location = location;
            _Country = country;
            _StateProvinceRegion = stateProvinceRegion;
            _CityTown = cityTown;
            _Area = area;
            _PostalCode = postalCode;
            _Street = street;
            _StreetNumber = streetNumber;
            _Description = description;
            ExtensionProperties= extensionProperties;
        }

        /// <MetaDataID>{6948881c-c426-41a6-8769-a08b2359b329}</MetaDataID>
        [PersistentMember(nameof(_Default))]
        [BackwardCompatibilityID("+11")]
        public bool Default
        {
            get => _Default;
            set
            {
                if (_Default != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Default = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
    }
}