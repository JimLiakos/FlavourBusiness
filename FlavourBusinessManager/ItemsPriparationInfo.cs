using System;
using FlavourBusinessFacade.ServicesContextResources;
using MenuModel;
using Newtonsoft.Json;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;

namespace FlavourBusinessManager.ServicesContextResources
{
    /// <MetaDataID>{8b58d11d-9cc5-4d15-9235-f6b337d2eaa5}</MetaDataID>
    [BackwardCompatibilityID("{8b58d11d-9cc5-4d15-9235-f6b337d2eaa5}")]
    [Persistent()]
    public class ItemsPreparationInfo : MarshalByRefObject, OOAdvantech.Remoting.IExtMarshalByRefObject, IItemsPreparationInfo
    {

        /// <MetaDataID>{75f7d359-4764-4ea6-af54-181b6f2ebeaf}</MetaDataID>
        protected ItemsPreparationInfo()
        {
        }


        /// <MetaDataID>{ceb67265-17de-4ac2-a22f-c9814cfa38b2}</MetaDataID>
        public ItemsPreparationInfo(IItemsCategory itemsCategory)
        {
            _ItemsInfoObjectUri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(itemsCategory).GetPersistentObjectUri(itemsCategory);

            _Description = itemsCategory.Name;
        }

        /// <MetaDataID>{783a888e-ce32-429f-97b0-d2ea043bc308}</MetaDataID>
        public ItemsPreparationInfo(IMenuItem menuItem)
        {
            _ItemsInfoObjectUri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(menuItem).GetPersistentObjectUri(menuItem);

            _Description = menuItem.Name;
        }


        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <exclude>Excluded</exclude>
        string _Description;

        /// <MetaDataID>{49b221fc-f97e-4a30-a609-a5713b0ebff2}</MetaDataID>
        [PersistentMember(nameof(_Description))]
        [BackwardCompatibilityID("+2")]
        [CachingDataOnClientSide]
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
        [JsonIgnore]
        IClassified _MenuModelObject;

        /// <MetaDataID>{b0673367-393f-48cc-a4b6-8fb6856d64d8}</MetaDataID>
        [JsonIgnore]
        public IClassified MenuModelObject
        {
            get
            {
                if (_MenuModelObject == null)
                    _MenuModelObject = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(ItemsInfoObjectUri) as IClassified;

                return _MenuModelObject;
            }
        }

        /// <exclude>Excluded</exclude>
        string _ItemsInfoObjectUri;

        /// <MetaDataID>{99a0d85d-e212-4c51-a384-8379a0d4ba59}</MetaDataID>
        [PersistentMember(nameof(_ItemsInfoObjectUri))]
        [BackwardCompatibilityID("+1")]
        [CachingDataOnClientSide]
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

        ///// <exclude>Excluded</exclude>
        //bool _Exclude;

        ///// <MetaDataID>{2f3185f1-2870-4d22-a60f-07581e5a503b}</MetaDataID>
        //[PersistentMember(nameof(_Exclude))]
        //[BackwardCompatibilityID("+3")]
        //[CachingDataOnClientSide]
        //public bool Exclude
        //{
        //    get => _Exclude;
        //    internal set
        //    {
        //        if (_Exclude != value)
        //        {
        //            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
        //            {
        //                _Exclude = value;
        //                stateTransition.Consistent = true;
        //            }
        //        }
        //    }
        //}

        /// <exclude>Excluded</exclude>
        double? _PreparationTimeSpanInMin = 1;

        /// <MetaDataID>{e02a922b-99e0-470a-807e-41039b2e963d}</MetaDataID>
        [PersistentMember(nameof(_PreparationTimeSpanInMin))]
        [BackwardCompatibilityID("+4")]
        [CachingDataOnClientSide]
        public double? PreparationTimeSpanInMin
        {
            get => _PreparationTimeSpanInMin;
            set
            {
                if (_PreparationTimeSpanInMin != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _PreparationTimeSpanInMin = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        int _LearningCurveCount;
        /// <MetaDataID>{a518b44c-b08a-4d65-b804-cacd8f57c351}</MetaDataID>
        [PersistentMember(nameof(_LearningCurveCount))]
        [BackwardCompatibilityID("+5")]
        public int LearningCurveCount
        {
            get => _LearningCurveCount;
            set
            {
                if (_LearningCurveCount != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _LearningCurveCount = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        ItemsPreparationInfoType _ItemsPreparationInfoType;

        /// <MetaDataID>{8428d0f2-eb93-487b-ac39-7e74e2877423}</MetaDataID>
        [PersistentMember(nameof(_ItemsPreparationInfoType))]
        [CachingDataOnClientSide]
        [BackwardCompatibilityID("+6")]
        public ItemsPreparationInfoType ItemsPreparationInfoType
        {
            get => _ItemsPreparationInfoType;
            set
            {
                if (_ItemsPreparationInfoType != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ItemsPreparationInfoType = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        bool? _IsCooked;
        /// <MetaDataID>{745b3dd4-59b8-409e-82e4-0dd1ed0e0ab4}</MetaDataID>
        [PersistentMember(nameof(_IsCooked))]
        [BackwardCompatibilityID("+7")]
        public bool? IsCooked
        {
            get => _IsCooked;
            set
            {

                if (_IsCooked != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _IsCooked = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }
    }
}



