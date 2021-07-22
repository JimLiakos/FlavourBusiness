using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using FlavourBusinessFacade.RoomService;
using MenuModel;
using MenuModel.JsonViewModel;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;

namespace FlavourBusinessManager.RoomService
{
    /// <MetaDataID>{c3c3932c-a4a6-4371-ae16-fd90553be136}</MetaDataID>
    [BackwardCompatibilityID("{c3c3932c-a4a6-4371-ae16-fd90553be136}")]
    [Persistent()]
    public class OptionChange : IOptionChange
    {
        /// <exclude>Excluded</exclude>
        bool _Without;

        /// <MetaDataID>{7fb6d78d-8331-44c3-ba48-1c6f37a2911a}</MetaDataID>
        [PersistentMember(nameof(_Without))]
        [BackwardCompatibilityID("+9")]
        public bool Without
        {
            get => _Without;
            set
            {

                if (_Without != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Without = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        string _OptionFullName;
        /// <MetaDataID>{41c1849e-9c38-4efa-9007-129ba107505d}</MetaDataID>
        [PersistentMember(nameof(_OptionFullName))]
        [BackwardCompatibilityID("+7")]
        public string OptionFullName
        {
            get => _OptionFullName;
            set
            {
                if (_OptionFullName != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _OptionFullName = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <MetaDataID>{5300a6ae-f573-470a-9f03-863d294fcea6}</MetaDataID>
        public OptionChange()
        {

        }
        /// <MetaDataID>{d877ae36-4a7b-49a1-a3bf-76929ca97040}</MetaDataID>
        public OptionChange(string optionUri, IItemPreparation itemPreparation)
        {
            _ItemPreparation = itemPreparation;
            _OptionUri = optionUri;
        }
        /// <exclude>Excluded</exclude>
        FlavourBusinessFacade.RoomService.IItemPreparation _ItemPreparation;

        /// <MetaDataID>{de69898f-8a60-4d16-8fb2-2e9cacee526d}</MetaDataID>
        [PersistentMember(nameof(_ItemPreparation))]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [BackwardCompatibilityID("+6")]
        public IItemPreparation ItemPreparation
        {
            get
            {
              return   _ItemPreparation;
            }
            set
            {

                if (_ItemPreparation != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ItemPreparation = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;


        /// <exclude>Excluded</exclude>
        string _NewLevelUri;

        /// <MetaDataID>{590ac6c6-efef-4ebb-b0bb-4e1c2a1b17ea}</MetaDataID>
        [PersistentMember(nameof(_NewLevelUri))]
        [BackwardCompatibilityID("+1")]
        public string NewLevelUri
        {
            get => _NewLevelUri;
            set
            {

                if (_NewLevelUri != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _NewLevelUri = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <MetaDataID>{0c607f35-f658-46a2-86b5-87c81de8a7da}</MetaDataID>
        [JsonIgnore]
        public ILevel NewLevel
        {
            get
            {
                if (itemSpecificOption != null)
                    return itemSpecificOption.Option.LevelType.Levels.OfType<MenuModel.JsonViewModel.Level>().Where(x => x.Uri == NewLevelUri).First();
                else
                    return null;
            }
        }

        /// <exclude>Excluded</exclude>
        string _OptionUri;

        /// <MetaDataID>{83f9f65f-ab4d-4c25-8b61-b2526ce260bb}</MetaDataID>
        [PersistentMember(nameof(_OptionUri))]
        [BackwardCompatibilityID("+2")]
        public string OptionUri
        {
            get => _OptionUri;
            set
            {

                if (_OptionUri != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _OptionUri = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        



        /// <exclude>Excluded</exclude>
        string _OptionName;


        /// <MetaDataID>{05ddc3f5-4330-4e7c-bcf4-cf486a80d9f8}</MetaDataID>
        [PersistentMember(nameof(_OptionName))]
        [BackwardCompatibilityID("+3")]
        public string OptionName
        {
            get => _OptionName;
            set
            {

                if (_OptionName != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _OptionName = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }



        /// <exclude>Excluded</exclude>
        int _PriceDif;

        /// <MetaDataID>{a363ad07-a202-449e-a8f0-92f710eb93dd}</MetaDataID>
        [PersistentMember(nameof(_PriceDif))]
        [BackwardCompatibilityID("+4")]
        public int PriceDif
        {
            get => _PriceDif;
            set
            {

                if (_PriceDif != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _PriceDif = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }



        /// <exclude>Excluded</exclude>
        double _OptionPrice;

        /// <MetaDataID>{6bf994e7-35f4-4528-b827-159a3d5fd3cd}</MetaDataID>
        [PersistentMember(nameof(_OptionPrice))]
        [BackwardCompatibilityID("+5")]
        public double OptionPrice
        {
            get => _OptionPrice;
            set
            {

                if (_OptionPrice != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _OptionPrice = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <MetaDataID>{fdc79eae-d68d-4fbe-aff7-253d9306e198}</MetaDataID>
        [JsonIgnore]
        public IOptionMenuItemSpecific itemSpecificOption { get; internal set; }

     


        /// <MetaDataID>{e44bc19d-1563-4b43-ab0e-5e4c37707d55}</MetaDataID>
        internal bool Update(OptionChange optionChange)
        {

            bool changed = (_NewLevelUri != optionChange.NewLevelUri ||
                _OptionName != optionChange.OptionName ||
                _OptionPrice != optionChange.OptionPrice ||
                _PriceDif != optionChange.PriceDif);


            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _NewLevelUri = optionChange.NewLevelUri;
                _OptionName = optionChange.OptionName;
                _OptionPrice = optionChange.OptionPrice;
                _PriceDif = optionChange.PriceDif;
                _Without = optionChange.Without; 
                stateTransition.Consistent = true;
            }

            return changed;

        }
    }
}
