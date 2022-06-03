using FlavourBusinessFacade.ServicesContextResources;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlavourBusinessManager.ServicesContextResources
{
    /// <MetaDataID>{0653c6c7-137b-4a4a-b017-76c6fe24213c}</MetaDataID>
    [BackwardCompatibilityID("{0653c6c7-137b-4a4a-b017-76c6fe24213c}")]
    [Persistent()]
    public class ItemPreparationTimeSpan
    {
        /// <exclude>Excluded</exclude>
        double _PreparationForecastTimespan;
        /// <MetaDataID>{335d7e52-b8fb-4f7c-9510-90e124553c59}</MetaDataID>
        [PersistentMember(nameof(_PreparationForecastTimespan))]
        [BackwardCompatibilityID("+7")]
        public double PreparationForecastTimespan
        {
            get => _PreparationForecastTimespan;
            set
            {
                if (_PreparationForecastTimespan != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _PreparationForecastTimespan = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <MetaDataID>{bf2ea810-b48e-4a5b-9a48-73417851fc2d}</MetaDataID>
        public ItemPreparationTimeSpan()
        {
        }


        [Association("ItemPreparationStatistics", Roles.RoleB, "7038c775-433e-4e6d-8400-ed23de8f8eb6")]
        [RoleBMultiplicityRange(1, 1)]
        [PersistentMember()]
        
        public PreparationStation PreparationStation;

        /// <exclude>Excluded</exclude>
        string _ItemsInfoObjectUri;

        /// <MetaDataID>{93e932fb-0a25-4e8e-8dfd-31f8b4ecf9f8}</MetaDataID>
        [PersistentMember(nameof(_ItemsInfoObjectUri))]
        [BackwardCompatibilityID("+5")]
        public string ItemsInfoObjectUri
        {
            get => _ItemsInfoObjectUri;
            set
            {
                if (_ItemsInfoObjectUri != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ItemsInfoObjectUri = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <exclude>Excluded</exclude>
        DateTime _StartsAt;
        /// <MetaDataID>{e93c355f-07a8-4a91-be02-acf3e6b0a147}</MetaDataID>
        [PersistentMember(nameof(_StartsAt))]
        [BackwardCompatibilityID("+4")]
        public DateTime StartsAt
        {
            get => _StartsAt;
            set
            {
                if (_StartsAt != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _StartsAt = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <exclude>Excluded</exclude>
        DateTime _EndsAt;
        /// <MetaDataID>{00068a01-afa8-48bc-950c-feaa3d275a80}</MetaDataID>
        [PersistentMember(nameof(_EndsAt))]
        [BackwardCompatibilityID("+3")]
        public DateTime EndsAt
        {
            get => _EndsAt;
            set
            {
                if (_EndsAt != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _EndsAt = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <exclude>Excluded</exclude>
        double _DurationDif;
        /// <MetaDataID>{4360f9ec-d675-4931-809d-c955fd6263f7}</MetaDataID>
        [PersistentMember(nameof(_DurationDif))]
        [BackwardCompatibilityID("+2")]
        public double DurationDif
        {
            get => _DurationDif;
            set
            {
                if (_DurationDif != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _DurationDif = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <MetaDataID>{82313792-9e0b-4f23-a4b5-c7820353fd79}</MetaDataID>
        public double DefaultTimeSpanInMin { get; set; }
        /// <MetaDataID>{fca50c81-2cf7-4a81-8ba4-a69217657789}</MetaDataID>
        public double DurationDifPerc { get; set; }

        /// <exclude>Excluded</exclude>
        double _InformationValue;
        /// <MetaDataID>{3a9e0d5c-579c-4c86-b39a-d0ca08c24d98}</MetaDataID>
        [PersistentMember(nameof(_InformationValue))]
        [BackwardCompatibilityID("+1")]
        public double InformationValue
        {
            get => _InformationValue;
            set
            {
                if (_InformationValue != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _InformationValue = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }



        /// <MetaDataID>{8fa56c55-de83-4ffb-81e0-a0325d43baae}</MetaDataID>
        public double OrgDurationDif { get; set; }
        /// <MetaDataID>{61be2be3-0e9e-49bd-9ab6-bbb5db9e1c85}</MetaDataID>
        public double OrgDurationDifPerc { get; set; }


        /// <exclude>Excluded</exclude>
        IItemsPreparationInfo _ItemsPreparationInfo;
        /// <MetaDataID>{57b0ad83-bc9c-4845-be95-8eb825e2d2e2}</MetaDataID>
        public IItemsPreparationInfo ItemsPreparationInfo
        {
            get => _ItemsPreparationInfo;
            internal set
            {
                _ItemsPreparationInfo = value;
                ItemsInfoObjectUri = value?.ItemsInfoObjectUri;
            }
        }
    
    }

    /// <MetaDataID>{895959fd-af89-4d5e-ad0f-d592223cceaa}</MetaDataID>
    public enum TimeSpanType
    {
        /// <summary>
        /// Record the preparation timespan
        /// </summary>
        Preparation = 1,
        /// <summary>
        /// Records the timespan between the forecast time where the item will be prepared
        /// and the time where item was prepared
        /// </summary>
        PreparationForecast
    }
}

