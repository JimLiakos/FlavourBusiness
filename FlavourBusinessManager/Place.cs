using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;

namespace FlavourBusinessManager.EndUsers
{
    /// <MetaDataID>{affca228-cac8-42fa-be5b-7da4d5f457bb}</MetaDataID>
    [BackwardCompatibilityID("{affca228-cac8-42fa-be5b-7da4d5f457bb}")]
    [Persistent()]
    public class Place : FlavourBusinessFacade.EndUsers.IPlace
    {
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
    }
}