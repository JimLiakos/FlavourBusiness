using FlavourBusinessFacade.EndUsers;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;

namespace FlavourBusinessManager.EndUsers
{
    /// <MetaDataID>{0202486a-b558-467c-a4ae-575d7d1e5924}</MetaDataID>
    [BackwardCompatibilityID("{0202486a-b558-467c-a4ae-575d7d1e5924}")]
    [Persistent()]
    public struct PlaceData
    {
        /// <MetaDataID>{ccedaa1c-cc2e-427a-af4e-41000f8bf19a}</MetaDataID>
        public static PlaceData GetPlaceData(IPlace place)
        {
            if (place == null)
                return new PlaceData();
            PlaceData placeData = new PlaceData(place.PlaceID, place.Location, place.Country,
                                                place.StateProvinceRegion, place.CityTown,
                                                place.Area, place.PostalCode, place.Street,
                                                place.StreetNumber, place.Description);
            return placeData;
        }

        /// <MetaDataID>{72aa5870-d68a-4e49-8b10-d4b7a54222ea}</MetaDataID>
        public static bool operator ==(PlaceData left, PlaceData right)
        {
            return left.Equals(right);
        }

        /// <MetaDataID>{fafbf187-5dee-444f-b600-78d55976938f}</MetaDataID>
        public static bool operator !=(PlaceData left, PlaceData right)
        {
            return !(left==right);
        }

        /// <exclude>Excluded</exclude>
        string _CityTown;
        /// <MetaDataID>{50c93377-b741-40bc-97b3-755cc1568cc2}</MetaDataID>
        [PersistentMember(nameof(_CityTown))]
        [BackwardCompatibilityID("+7")]
        public string CityTown
        {
            get => _CityTown;
            set
            {

                if (_CityTown!=value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _CityTown=value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        string _StateProvinceRegion;
        /// <MetaDataID>{d252edfd-c038-4551-ad7b-619a11fca2b9}</MetaDataID>
        [PersistentMember(nameof(_StateProvinceRegion))]
        [BackwardCompatibilityID("+6")]
        public string StateProvinceRegion
        {
            get => _StateProvinceRegion;
            set
            {
                if (_StateProvinceRegion!=value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _StateProvinceRegion=value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        string _Description;
        /// <MetaDataID>{abb00cd5-58fc-4d0c-a388-2f88ba91cec4}</MetaDataID>
        [PersistentMember(nameof(_Description))]
        [BackwardCompatibilityID("+5")]
        public string Description
        {
            get => _Description;
            set
            {
                if (_Description!=value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Description=value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        string _StreetNumber;
        /// <MetaDataID>{48fea14e-04b5-49c9-8e23-9c4a7bfb6c55}</MetaDataID>
        [PersistentMember(nameof(_StreetNumber))]
        [BackwardCompatibilityID("+8")]
        public string StreetNumber
        {
            get => _StreetNumber;
            set
            {

                if (_StreetNumber!=value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _StreetNumber=value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        string _Street;
        /// <MetaDataID>{3b91c84b-869f-4a36-83f3-5ce232ee49ca}</MetaDataID>
        [PersistentMember(nameof(_Street))]
        [BackwardCompatibilityID("+4")]
        public string Street
        {
            get => _Street;
            set
            {
                if (_Street!=value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Street=value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        string _Area;
        /// <MetaDataID>{e5c0a99a-371c-441f-a248-aa5929025bd9}</MetaDataID>
        [PersistentMember(nameof(_Area))]
        [BackwardCompatibilityID("+9")]
        public string Area
        {
            get => _Area;
            set
            {
                if (_Area!=value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Area=value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }


        /// <exclude>Excluded</exclude>
        string _PostalCode;
        /// <MetaDataID>{66c17d30-9585-4f2c-a0f8-4288eaae020c}</MetaDataID>
        [PersistentMember(nameof(_PostalCode))]
        [BackwardCompatibilityID("+10")]
        public string PostalCode
        {
            get => _PostalCode;
            set
            {
                if (_PostalCode!=value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _PostalCode=value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        string _Country;
        /// <MetaDataID>{25ea321a-e827-464a-8b21-cf01c1886022}</MetaDataID>
        [PersistentMember(nameof(_Country))]
        [BackwardCompatibilityID("+3")]
        public string Country
        {
            get => _Country;
            set
            {
                if (_Country!=value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Country=value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        FlavourBusinessFacade.EndUsers.Coordinate _Location;
        /// <MetaDataID>{b005a5ac-97ec-4c6d-b65b-1408130c9e30}</MetaDataID>
        [PersistentMember(nameof(_Location))]
        [BackwardCompatibilityID("+2")]
        public FlavourBusinessFacade.EndUsers.Coordinate Location
        {
            get => _Location;
            set
            {
                if (_Location!=value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Location=value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        string _PlaceID;

        /// <MetaDataID>{d1f20d85-c3a5-4c27-9750-f4ff9745bc7e}</MetaDataID>
        public PlaceData(string placeID, Coordinate location, string country, string stateProvinceRegion, string cityTown, string area, string postalCode, string street, string streetNumber, string description) : this()
        {
            _PlaceID=placeID;
            _Location=location;
            _Country=country;
            _StateProvinceRegion=stateProvinceRegion;
            _CityTown=cityTown;
            _Area=area;
            _PostalCode=postalCode;
            _Street=street;
            _StreetNumber=streetNumber;
            _Description=description;
        }

        /// <MetaDataID>{0120bade-d1d0-4ef9-8c0f-cbb12f53a1fe}</MetaDataID>
        [PersistentMember(nameof(_PlaceID))]
        [BackwardCompatibilityID("+1")]
        public string PlaceID
        {
            get => _PlaceID;
            set
            {
                if (_PlaceID!=value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _PlaceID=value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }
    }
}