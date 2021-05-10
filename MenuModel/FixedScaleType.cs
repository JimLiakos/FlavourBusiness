using System;

using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using OOAdvantech.Linq;
using System.Linq;
using System.Collections.Generic;

namespace MenuModel
{
    /// <MetaDataID>{1bec0ffa-4059-4a4d-9f01-9bfbaf94fc18}</MetaDataID>
    [BackwardCompatibilityID("{1bec0ffa-4059-4a4d-9f01-9bfbaf94fc18}")]
    [Persistent()]
    public class FixedScaleType : IScaleType
    {

        /// <exclude>Excluded</exclude>
        bool _ZeroLevelScaleType;

        /// <MetaDataID>{14459806-f20d-44ac-b3f2-c1213ac6dc22}</MetaDataID>
        /// <summary>When is true the first level defines the absence of option.</summary>
        [PersistentMember(nameof(_ZeroLevelScaleType))]
        [BackwardCompatibilityID("+4")]
        public bool ZeroLevelScaleType
        {
            get => _ZeroLevelScaleType;
            set
            {
                if (_ZeroLevelScaleType != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ZeroLevelScaleType = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <MetaDataID>{cb66f988-d868-4543-8bbc-10a40d67a52b}</MetaDataID>
        public static void UpdateStorage(OOAdvantech.PersistenceLayer.ObjectStorage objectStorage)
        {
            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(objectStorage);

            var checkUncheckScaleType = (from scaleType in storage.GetObjectCollection<MenuModel.FixedScaleType>()
                                         where scaleType.UniqueIdentifier == FixedScaleTypes.CheckUncheck
                                         select scaleType).FirstOrDefault();
            if (checkUncheckScaleType == null)
            {

                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {
                    
                    List<ILevel> levels = new List<ILevel>();
                    Level level = new Level("Unchecked");
                    level.PriceFactor = 0;
                    level.UncheckOption = true;
                    objectStorage.CommitTransientObjectState(level);
                    levels.Add(level);
                    level = new Level("Checked");
                    level.UncheckOption = false;
                    objectStorage.CommitTransientObjectState(level);
                    levels.Add(level);
                    checkUncheckScaleType = new FixedScaleType(FixedScaleTypes.CheckUncheck, "Check Unchecked", levels);
                    checkUncheckScaleType.ZeroLevelScaleType = true;

                    objectStorage.CommitTransientObjectState(checkUncheckScaleType);

                    stateTransition.Consistent = true;
                }


            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <MetaDataID>{6ea00660-1098-4ee1-b11f-c8093d89ed3e}</MetaDataID>
        private FixedScaleType()
        {

        }
        /// <MetaDataID>{408e7646-33bf-423d-8bdc-d0e6e67f5776}</MetaDataID>
        internal FixedScaleType(string uniqueIdentifier, string name, List<ILevel> levels)
        {
            if (levels==null||levels.Count < 2)
                throw new System.Exception("ScaleType levels must be at least two");

            _UniqueIdentifier = uniqueIdentifier;
            _Name.Value = name;
            foreach (var level in levels)
                _Levels.AddRange(levels);
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<ILevel> _Levels = new OOAdvantech.Collections.Generic.Set<ILevel>();
        /// <MetaDataID>{3cd691c4-896c-4c51-a50c-301a12ba3659}</MetaDataID>
        [PersistentMember("_Levels")]
        [BackwardCompatibilityID("+2")]
        public IList<MenuModel.ILevel> Levels
        {
            get
            {
                return _Levels.AsReadOnly();
            }
        }


        /// <exclude>Excluded</exclude>
        OOAdvantech.MultilingualMember<string> _Name = new OOAdvantech.MultilingualMember<string>();

        /// <MetaDataID>{392c24de-0415-4367-9a01-f1eec75dd059}</MetaDataID>
        [PersistentMember("_Name")]
        [BackwardCompatibilityID("+3")]
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                //throw new NotImplementedException("You can't change name of fixed scale type.");
                if (_Name != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Name.Value= value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <exclude>Excluded</exclude>
        string _UniqueIdentifier;
        /// <MetaDataID>{820fd9cb-b7ed-407c-9432-cd47db20dfe4}</MetaDataID>
        [PersistentMember("_UniqueIdentifier")]
        [BackwardCompatibilityID("+1")]
        public string UniqueIdentifier
        {
            get
            {
                return _UniqueIdentifier;
            }

            set
            {
                if (_UniqueIdentifier != value)
                {

                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _UniqueIdentifier = value;
                        stateTransition.Consistent = true;
                    }

                }
            }
        }

        /// <MetaDataID>{9fca6ef1-9da8-439d-89ea-d4f6a3db865b}</MetaDataID>
        public void AddLevel(ILevel level)
        {
            throw new NotImplementedException("You can't change levels of fixed scale type.");
            
        }

        /// <MetaDataID>{a36cde50-d0d8-4113-a47b-fe0f3a6010a9}</MetaDataID>
        public void InsertLevel(int index, MenuModel.ILevel level)
        {
            throw new NotImplementedException("You can't change levels of fixed scale type.");
        }

        /// <MetaDataID>{b0c50f1b-0157-41e7-86f1-4bd980e51132}</MetaDataID>
        public void MoveLevel(ILevel level, int newpos)
        {
            throw new NotImplementedException("You can't change levels of fixed scale type.");
        }

        /// <MetaDataID>{aad961c8-8453-4d79-9a30-da185bf803e9}</MetaDataID>
        public void RemoveLevel(ILevel level)
        {
            throw new NotImplementedException("You can't change levels of fixed scale type.");
        }
    }
}

