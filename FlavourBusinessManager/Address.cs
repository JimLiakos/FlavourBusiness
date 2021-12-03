using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;

namespace FlavourBusinessManager.ServicesContextResources
{
    /// <MetaDataID>{fb18ba5d-a7ab-4ffc-9574-9404b7bdc512}</MetaDataID>
    [BackwardCompatibilityID("{fb18ba5d-a7ab-4ffc-9574-9404b7bdc512}")]
    [Persistent()]
    public class Address : FinanceFacade.IAddress
    {
        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink=new OOAdvantech.ObjectStateManagerLink();

        /// <exclude>Excluded</exclude>
        double _Longitude;
        /// <MetaDataID>{de8df6bc-e884-41e6-8b25-125f4b3f28fb}</MetaDataID>
        [PersistentMember(nameof(_Longitude))]
        [BackwardCompatibilityID("+12")]
        public double Longitude
        {
            get => _Longitude;
            set
            {
                if (_Longitude != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Longitude = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        double _Latitude;
        /// <MetaDataID>{e572ba54-e469-427c-aede-0e6188b62d16}</MetaDataID>
        [PersistentMember(nameof(_Latitude))]
        [BackwardCompatibilityID("+11")]
        public double Latitude
        {
            get => _Latitude;
            set
            {
                if (_Longitude != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Longitude = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }
        /// <exclude>Excluded</exclude>
        string _CountryCode;

        /// <MetaDataID>{6052b3f3-ad73-4eee-ba04-2765fb0bedc6}</MetaDataID>
        [PersistentMember(nameof(_CountryCode))]
        [BackwardCompatibilityID("+10")]
        public string CountryCode
        {
            get => _CountryCode;
            set
            {
                if (_CountryCode != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _CountryCode = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }
        /// <exclude>Excluded</exclude>
        string _Country;

        /// <MetaDataID>{e57988c8-bc4e-4a8b-b61c-234eaae86d81}</MetaDataID>
        [PersistentMember(nameof(_Country))]
        [BackwardCompatibilityID("+9")]
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
        string _PostalCode;

        /// <MetaDataID>{8fbd44c6-a357-4c39-971f-a3072a9d5dba}</MetaDataID>
        [PersistentMember(nameof(_PostalCode))]
        [BackwardCompatibilityID("+8")]
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
        string _Area;

        /// <MetaDataID>{8246248a-31db-4623-832e-f350c9834af3}</MetaDataID>
        [PersistentMember(nameof(_Area))]
        [BackwardCompatibilityID("+7")]
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
        string _Sub_Area;

        /// <MetaDataID>{4afa04df-4e1a-49fd-83ee-7b65acd1511c}</MetaDataID>
        [PersistentMember(nameof(_Sub_Area))]
        [BackwardCompatibilityID("+6")]
        public string Sub_Area
        {
            get => _Sub_Area;
            set
            {
                if (_Sub_Area != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Sub_Area = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        string _City;

        /// <MetaDataID>{999f79c6-54a7-4eb3-9a76-aebc04cfe27a}</MetaDataID>
        [PersistentMember(nameof(_City))]
        [BackwardCompatibilityID("+5")]
        public string City
        {
            get => _City;
            set
            {
                if (_City != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _City = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        string _Locality;

        /// <MetaDataID>{58b872cb-97ac-42dd-922b-97f6916fa295}</MetaDataID>
        [PersistentMember(nameof(_Locality))]
        [BackwardCompatibilityID("+4")]
        public string Locality
        {
            get => _Locality;
            set
            {
                if (_Locality != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Locality = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        string _StreetNumber;

        /// <MetaDataID>{ce817913-2067-477f-ba07-21049a473143}</MetaDataID>
        [PersistentMember(nameof(_StreetNumber))]
        [BackwardCompatibilityID("+3")]
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
        string _StreetName;

        /// <MetaDataID>{b3de0f94-460d-4f8a-87c2-5decb383043f}</MetaDataID>
        [PersistentMember(nameof(_StreetName))]
        [BackwardCompatibilityID("+2")]
        public string StreetName
        {
            get => _StreetName;
            set
            {
                if (_StreetName != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _StreetName = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        string _LocationName;


        /// <MetaDataID>{58973987-811f-4e07-9581-f88e3402ef3a}</MetaDataID>
        [PersistentMember(nameof(_LocationName))]
        [BackwardCompatibilityID("+1")]
        public string LocationName
        {
            get => _LocationName;
            set
            {
                if (_LocationName != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _LocationName = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        public string FormattedAddress
        {
            get
            {
                return string.Format("{0} {1}, {3} {6}, {7}", StreetName, StreetNumber, Locality, City, Sub_Area, Area, PostalCode, Country);
            }
        }
    }
}