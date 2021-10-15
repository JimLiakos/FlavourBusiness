using System;
using System.Collections.Generic;
using System.ComponentModel;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using System.Linq;
namespace MenuModel
{
    /// <MetaDataID>{3981358c-b93f-4420-815f-864f964dba6c}</MetaDataID>
    [BackwardCompatibilityID("{3981358c-b93f-4420-815f-864f964dba6c}")]
    [Persistent()]
    public class MenuItemType : MarshalByRefObject, IClassified, IMenuItemType, System.ComponentModel.INotifyPropertyChanged
    {


        /// <exclude>Excluded</exclude>
        OOAdvantech.MultilingualMember<string> _Name = new OOAdvantech.MultilingualMember<string>();
        /// <MetaDataID>{9ff83696-c3ad-4c01-963b-85b285be16ff}</MetaDataID>
        [BackwardCompatibilityID("+6")]
        [PersistentMember("_Name")]
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                if (_Name != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Name.Value = value;
                        stateTransition.Consistent = true;
                    }
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                }
            }
        }







        /// <exclude>Excluded</exclude>
        OOAdvantech.Member<MenuModel.IMenuItem> _Owner = new OOAdvantech.Member<IMenuItem>();


        /// <MetaDataID>{a1570153-fbfd-469e-ad66-8837ea619374}</MetaDataID>
        [BackwardCompatibilityID("+3")]
        public MenuModel.IMenuItem Owner
        {
            get
            {
                return _Owner.Value;
            }
        }




        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;






        public event PropertyChangedEventHandler PropertyChanged;
        public event PreparationOptionAddedHandle PreparationOptionAdded;
        public event PreparationOptionRemovedHandle PreparationOptionRemoved;


        /// <MetaDataID>{45ef3a3e-2519-475c-8173-c20df2213577}</MetaDataID>
        public MenuItemType(string name)
        {
            _Name.Value = name;
        }


        /// <MetaDataID>{aa9ec515-47b9-446d-a1e4-9de68664f7b1}</MetaDataID>
        public MenuItemType()
        {

        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<MenuModel.IPreparationOption> _Options = new OOAdvantech.Collections.Generic.Set<IPreparationOption>();

        /// <MetaDataID>{4e0407ee-c23a-4e6a-8737-64886f81f041}</MetaDataID>
        [PersistentMember("_Options")]
        [BackwardCompatibilityID("+5")]
        public IList<MenuModel.IPreparationOption> Options
        {
            get
            {

                return _Options.AsReadOnly();
            }
        }


        /// <MetaDataID>{5a266cf0-ba7a-4056-90ed-1aee3db19e45}</MetaDataID>
        public void MovePreparationOption(IPreparationOption preparationOption, int newpos)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _Options.Remove(preparationOption);
                _Options.Insert(newpos, preparationOption);
                stateTransition.Consistent = true;
            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.Member<MenuModel.IClass> _Class = new OOAdvantech.Member<MenuModel.IClass>();

        /// <MetaDataID>{5ff4f561-fd32-4a65-9fc0-cc47409f3b20}</MetaDataID>
        [PersistentMember("_Class")]
        [BackwardCompatibilityID("+7")]
        public MenuModel.IClass Class
        {
            get
            {
                return _Class.Value;
            }
            set
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _Class.Value = value;
                    stateTransition.Consistent = true;
                }
            }
        }

        /// <MetaDataID>{82d1bcbf-ac32-4eb8-a38e-2e3222ff4315}</MetaDataID>
        public List<IPricingContext> PricingContexts
        {
            get
            {
                return (from itemSelectorOptionsGroup in this.Options.OfType<ItemSelectorOptionsGroup>()
                        from itemSelectorOption in itemSelectorOptionsGroup.GroupedOptions
                        select itemSelectorOption).OfType<IPricingContext>().ToList();
            }
        }
        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<ITag> _PreparationTags = new OOAdvantech.Collections.Generic.Set<ITag>();



        /// <MetaDataID>{bb132a8f-c130-4efe-8b5d-2d5e7176c80c}</MetaDataID>
        [PersistentMember(nameof(_PreparationTags))]
        [BackwardCompatibilityID("+9")]
        public System.Collections.Generic.List<MenuModel.ITag> PreparationTags => _PreparationTags.ToThreadSafeList();

        /// <MetaDataID>{38a33b37-5cc4-4f58-b495-221d4009f2ce}</MetaDataID>
        public ITag NewPreparationTag()
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                var tag = new Tag();
                tag.Name = "new Tag";
                OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(tag);
                _PreparationTags.Add(tag);
                stateTransition.Consistent = true;
                return tag;
            }

        }

        /// <MetaDataID>{6f93d5d8-debb-4d37-8299-0ac61ff737e2}</MetaDataID>
        public void RemovePreparationTag(ITag tag)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _PreparationTags.Remove(tag);
                stateTransition.Consistent = true;
            }
        }

        /// <MetaDataID>{bb7d9578-5c29-478c-81cd-8b59470b8479}</MetaDataID>
        internal void OnPreparationOptionRemoved(IPreparationOption preparationOption)
        {
            PreparationOptionRemoved?.Invoke(this, preparationOption);
        }
        /// <MetaDataID>{768215f4-5a61-42a3-9261-b0f63c11197c}</MetaDataID>
        internal void OnPreparationOptionAdded(IPreparationOption preparationOption)
        {
            PreparationOptionAdded?.Invoke(this, preparationOption);
        }
        /// <MetaDataID>{3634598f-910e-4b11-ae21-05729542f6ee}</MetaDataID>
        public void AddPreparationOption(IPreparationOption option)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _Options.Add(option);
                stateTransition.Consistent = true;
            }

        }

        /// <MetaDataID>{720ac683-71dc-4d5c-956f-7e3fc3879865}</MetaDataID>
        public void RemovePreparationOption(IPreparationOption option)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _Options.Remove(option);
                stateTransition.Consistent = true;
            }

        }

     
    }
}