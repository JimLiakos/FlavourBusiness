using System;
using System.Collections.Generic;
using System.Linq;
using FlavourBusinessFacade.ServicesContextResources;
using MenuModel;
using OOAdvantech;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;

namespace FlavourBusinessManager.ServicesContextResources
{
    /// <MetaDataID>{8b58d11d-9cc5-4d15-9235-f6b337d2eaa5}</MetaDataID>
    [BackwardCompatibilityID("{8b58d11d-9cc5-4d15-9235-f6b337d2eaa5}")]
    [Persistent()]
    public class ItemsPreparationInfo : MarshalByRefObject, OOAdvantech.Remoting.IExtMarshalByRefObject, IItemsPreparationInfo
    {
        /// <MetaDataID>{ccdd24fd-e5c4-419d-8628-0c3c793d22bd}</MetaDataID>
        [PersistentMember()]
        [BackwardCompatibilityID("+27")]
        private string TagsJson;

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
        [OOAdvantech.Json.JsonIgnore]
        IClassified _MenuModelObject;

        /// <MetaDataID>{b0673367-393f-48cc-a4b6-8fb6856d64d8}</MetaDataID>
        [OOAdvantech.Json.JsonIgnore]
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
        public event ObjectChangeStateHandle ObjectChangeState;

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
        double? _PreparationTimeSpanInMin ;

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
        double? _CookingTimeSpanInMin;

        /// <MetaDataID>{9e4ec1b0-9ea5-41e0-b1c2-472b320fddda}</MetaDataID>
        [PersistentMember(nameof(_CookingTimeSpanInMin))]
        [BackwardCompatibilityID("+8")]
        [CachingDataOnClientSide]
        public double? CookingTimeSpanInMin
        {
            get => _CookingTimeSpanInMin;
            set
            {
                if (_CookingTimeSpanInMin != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _CookingTimeSpanInMin = value;
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


        /// <MetaDataID>{23efbf59-a1cb-4792-bc84-f62c5db8d0a5}</MetaDataID>
        public List<ITag> Copy(List<ITag> tags)
        {
            if (_PreparationTags != null && _PreparationTags.Count > 0)
                throw new Exception("You can copy tags, only in items preparation info, with no tags.");

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _PreparationTags = new List<ITag>();
                foreach (Tag copyTag in tags)
                {
                    Tag tag = new Tag(copyTag);
                    _PreparationTags.Add(tag);
                }
                stateTransition.Consistent = true;
            }

            return _PreparationTags.ToList();
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


                        if ((_ItemsPreparationInfoType & ItemsPreparationInfoType.Include)== ItemsPreparationInfoType.Include && _PreparationTimeSpanInMin == null)
                            _PreparationTimeSpanInMin =1;

                        stateTransition.Consistent = true;
                    }
                    ObjectChangeState?.Invoke(this, null);
                }
            }
        }

        /// <exclude>Excluded</exclude>
        bool? _IsCooked;
        /// <MetaDataID>{745b3dd4-59b8-409e-82e4-0dd1ed0e0ab4}</MetaDataID>
        [PersistentMember(nameof(_IsCooked))]
        [BackwardCompatibilityID("+7")]
        [CachingDataOnClientSide]
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

        /// <MetaDataID>{a6cc3ed1-24e7-4416-a62a-b11f0b9bd67a}</MetaDataID>
        [BeforeCommitObjectStateInStorageCall]
        internal void OnBeforeCommitObjectState()
        {
            lock (PreparationTagLock)
            {
                if (_PreparationTags == null)
                    TagsJson = null;
                else
                    TagsJson = OOAdvantech.Json.JsonConvert.SerializeObject(_PreparationTags);
            }
        }
        /// <MetaDataID>{cf7a1312-160c-40e5-9477-7b7c7d7a0e24}</MetaDataID>
        [ObjectActivationCall]
        internal void OnActivated()
        {
            lock (PreparationTagLock)
            {
                if (!string.IsNullOrWhiteSpace(TagsJson))
                    _PreparationTags = OOAdvantech.Json.JsonConvert.DeserializeObject<List<MenuModel.Tag>>(TagsJson).OfType<ITag>().ToList();
                else
                    _PreparationTags = null;
            }
            //Json.JsonConvert.DeserializeObject 
        }

        /// <MetaDataID>{9ab3cf41-fa8f-4265-a8f2-08f46ad6ccf7}</MetaDataID>
        public ITag NewPrepatationTag()
        {
            lock (PreparationTagLock)
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    var tag = new Tag();
                    tag.Name = "new Tag";
                    if (_PreparationTags == null)
                        _PreparationTags = new List<ITag>();
                    _PreparationTags.Add(tag);

                    stateTransition.Consistent = true;
                    return tag;
                }

            }
        }


        /// <MetaDataID>{85f39c09-2a51-47e0-9234-99ada490d340}</MetaDataID>
        public void ClearTags()
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _PreparationTags = null;
                stateTransition.Consistent = true;
            }

        }
        /// <MetaDataID>{27308509-7e60-4ffe-bd83-bec40c4df809}</MetaDataID>
        object PreparationTagLock = new object();
        /// <MetaDataID>{5002bf1b-391a-4ce5-a9e4-11d1fe0e9671}</MetaDataID>
        public void RemovePreparationTag(ITag tag)
        {

            lock (PreparationTagLock)
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    var preparationTag = _PreparationTags.OfType<Tag>().Where(x => x.Uid == (tag as Tag).Uid).FirstOrDefault();
                    if (preparationTag != null)
                        _PreparationTags.Remove(preparationTag);
                    if (_PreparationTags == null && _PreparationTags.Count == 0)
                        _PreparationTags = null;


                    stateTransition.Consistent = true;
                }

            }


        }

        /// <MetaDataID>{d971121c-a734-4e03-88aa-8bf86068ae8e}</MetaDataID>
        public void UpdateTag(ITag tag)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                var existingTag = _PreparationTags.Where(x => x.Uid == tag.Uid).FirstOrDefault();
                if (existingTag != null)
                {
                    var index = _PreparationTags.IndexOf(existingTag);
                    _PreparationTags.RemoveAt(index);
                    _PreparationTags.Insert(index, tag);
                }
                stateTransition.Consistent = true;
            }


        }


        /// <MetaDataID>{848fe936-d592-4479-a878-b8f779222438}</MetaDataID>
        List<ITag> _PreparationTags = null;
        /// <MetaDataID>{4427b61d-87f1-4cc8-a4d9-c45201e2f726}</MetaDataID>
        public List<ITag> PreparationTags
        {
            get
            {
                lock (PreparationTagLock)
                {
                    if (_PreparationTags == null)
                        return null;
                    return _PreparationTags.ToList();
                }
            }
        }
    }
}



