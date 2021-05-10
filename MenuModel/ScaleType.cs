using System;
using System.Collections.Generic;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;

namespace MenuModel
{
    /// <MetaDataID>{983fc6b5-5807-4cbd-b1bc-5a712cefea63}</MetaDataID>
    [BackwardCompatibilityID("{983fc6b5-5807-4cbd-b1bc-5a712cefea63}")]
    [Persistent()]
    public class ScaleType  : MarshalByRefObject, IScaleType, OOAdvantech.PersistenceLayer.IObjectStateEventsConsumer
    {

        /// <exclude>Excluded</exclude>
        bool _ZeroLevelScaleType;

        /// <MetaDataID>{c90124c5-1dd7-4ee9-8fdf-a21bc4e81167}</MetaDataID>
        /// <summary>When is true the first level defines the absence of option.</summary>
        [PersistentMember(nameof(_ZeroLevelScaleType))]
        [BackwardCompatibilityID("+3")]
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

        public event OOAdvantech.ObjectChangeStateHandle ObjectStateChanged;

        /// <MetaDataID>{fbb858d6-4b29-43e9-b1f0-21f89f14ac65}</MetaDataID>
        public ScaleType()
        {

        }

        /// <MetaDataID>{bb2749a8-7cd5-4753-94ea-d04b274ba344}</MetaDataID>
        public ScaleType(string name)
        {
            _Name.Value = name;
        }


        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<ILevel> _Levels=new OOAdvantech.Collections.Generic.Set<ILevel>();

        /// <MetaDataID>{37cedd97-e658-4757-b123-77f84ced4d7c}</MetaDataID>
        [PersistentMember("_Levels")]
        [BackwardCompatibilityID("+1")]
        public IList<MenuModel.ILevel> Levels
        {
            get
            {
                return _Levels.AsReadOnly();
            }
        }
        /// <exclude>Excluded</exclude>
        OOAdvantech.MultilingualMember<string> _Name = new OOAdvantech.MultilingualMember<string>();

        /// <MetaDataID>{bef0110d-4c9a-4cca-8b87-c7d85ff8ad5c}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        [PersistentMember("_Name")]
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                if (_Name != null)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Name.Value = value; 
                        stateTransition.Consistent = true;
                    }
                    ObjectStateChanged?.Invoke(this, nameof(Name));
                }
            }
        }

        /// <MetaDataID>{e6f187cd-46a8-49b9-b91a-31af924e452f}</MetaDataID>
        public void MoveLevel(MenuModel.ILevel level, int newpos)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _Levels.Remove(level);
                _Levels.Insert(newpos, level); 
                stateTransition.Consistent = true;
            }

        }

        /// <MetaDataID>{93e9bfa7-df7d-4557-8758-827cb4846fbd}</MetaDataID>
        public void AddLevel(ILevel level)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _Levels.Add(level); 
                stateTransition.Consistent = true;
            }
        }

        /// <MetaDataID>{c0496475-1a6c-4d12-8fa9-4937dbee056a}</MetaDataID>
        public void RemoveLevel(ILevel level)
        {
            if (_Levels.Count > 2)
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _Levels.Remove(level);
                    stateTransition.Consistent = true;
                }
            }
            else
                throw new System.Exception("ScaleType levels must be at least two");
        }
     

        /// <MetaDataID>{f97aea66-d598-47ad-ad24-43ff839b93d1}</MetaDataID>
        public void InsertLevel(int index, ILevel level)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _Levels.Insert(index, level);
                stateTransition.Consistent = true;
            }

        }

        /// <MetaDataID>{63de5e61-4c75-48ff-8a12-2e475368282d}</MetaDataID>
        public void OnCommitObjectState()
        {
            if (_Levels.Count == 0)
            {
                OOAdvantech.PersistenceLayer.ObjectStorage objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this);
                Level level = new Level("Unchecked");
                level.UncheckOption = true;
                objectStorage.CommitTransientObjectState(level);
                _Levels.Add(level);
                level = new Level("Checked");
                level.UncheckOption = false;
                objectStorage.CommitTransientObjectState(level);
                _Levels.Add(level);
            }
        }

        /// <MetaDataID>{ea3913ae-489e-45ea-b829-2223ebb080bc}</MetaDataID>
        public void BeforeCommitObjectState()
        {
        }

        /// <MetaDataID>{11fdf5ad-6171-43eb-a832-0784ad6c5004}</MetaDataID>
        public void OnActivate()
        {

        }

        /// <MetaDataID>{13668ec8-8709-4925-9dde-c571fc74a870}</MetaDataID>
        public void OnDeleting()
        {

        }

        /// <MetaDataID>{b7465873-2926-40ab-a07b-526452695ff0}</MetaDataID>
        public void LinkedObjectAdded(object linkedObject, AssociationEnd associationEnd)
        {

        }

        /// <MetaDataID>{83f838b6-c1a0-450b-b4eb-d2b9d64c7fde}</MetaDataID>
        public void LinkedObjectRemoved(object linkedObject, AssociationEnd associationEnd)
        {

        }
    }
}