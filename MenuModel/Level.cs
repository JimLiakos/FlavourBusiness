using System;
using System.Collections.Generic;
using MenuModel.JsonViewModel;
using OOAdvantech;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;

namespace MenuModel
{
    /// <MetaDataID>{51666fd7-3e9a-47ef-8138-52753b6ad25f}</MetaDataID>
    [BackwardCompatibilityID("{51666fd7-3e9a-47ef-8138-52753b6ad25f}")]
    [Persistent()]
    public class Level : ILevel
    {


        public string Uri
        {
            get
            {
                return OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).GetPersistentObjectUri(this);
            }
        }


        /// <exclude>Excluded</exclude>
        double? _PriceFactor;

        /// <MetaDataID>{ab2c1e51-3213-4c83-9fcf-271fe5d58dac}</MetaDataID>
        [PersistentMember(nameof(_PriceFactor))]
        [BackwardCompatibilityID("+4")]
        public double PriceFactor
        {
            get
            {
                if (_PriceFactor.HasValue)
                    return _PriceFactor.Value;
                if (DeclaringType != null)
                    return DeclaringType.Levels.IndexOf(this);
                else
                    return 0;
            }
                
            set
            {
                if (_PriceFactor != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _PriceFactor = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;



        /// <MetaDataID>{0c0b2183-05a7-4795-87da-ea794413003e}</MetaDataID>
        public Level()
        {

        }
        /// <MetaDataID>{07413a7c-a0b5-41d2-b88b-4965fbfd57b7}</MetaDataID>
        public Level(string name)
        {

            _Name.Value = name;
        }
        /// <exclude>Excluded</exclude>
        OOAdvantech.MultilingualMember<string> _Name = new OOAdvantech.MultilingualMember<string>();

        /// <MetaDataID>{ace749f1-1658-4e4c-8df2-0c42c1d6688d}</MetaDataID>
        [PersistentMember("_Name")]
        public string Name
        {
            get
            {
                return _Name;
            }

            set
            {
                if (_Name != value)//&& !(DeclaringType is MenuModel.FixedScaleType))
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Name.Value = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        bool _UncheckOption;

        /// <MetaDataID>{7968f8ba-000b-41db-bf81-c6b710d42313}</MetaDataID>
        [PersistentMember("_UncheckOption")]
        public bool UncheckOption
        {
            get
            {
                return _UncheckOption;
            }
            set
            {
                if (_UncheckOption != value && !(DeclaringType is MenuModel.FixedScaleType))
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _UncheckOption = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <exclude>Excluded</exclude>
        IScaleType _DeclaringType;

        /// <MetaDataID>{8c2f284b-9d19-453e-b4ea-a91aec0645d1}</MetaDataID>
        [PersistentMember("_DeclaringType")]
        [BackwardCompatibilityID("+3")]
        [AssociationEndBehavior(PersistencyFlag.CascadeDelete)]
        public IScaleType DeclaringType
        {
            get
            {
                return _DeclaringType;
            }
        }

        /// <MetaDataID>{fd18472d-20fa-4cb6-9fd3-6e55b2386d6f}</MetaDataID>
        public Multilingual MultilingualName => new Multilingual(_Name);
    }
}
