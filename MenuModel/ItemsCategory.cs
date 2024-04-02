using System;
using System.Collections.Generic;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using System.Linq;

namespace MenuModel
{
    /// <MetaDataID>{33f10f18-9a4a-4c33-b27b-922989c95508}</MetaDataID>
    [BackwardCompatibilityID("{33f10f18-9a4a-4c33-b27b-922989c95508}")]
    [Persistent()]
    public class ItemsCategory : IItemsCategory
    {
        /// <MetaDataID>{20f3fd98-b465-41e9-9be5-e5d764c2e999}</MetaDataID>
        protected ItemsCategory()
        {

        }



        /// <MetaDataID>{6ee29d36-091f-4ad5-9dfd-2bda666aaf48}</MetaDataID>
        public ItemsCategory(string name)
        {
            _Name = name;
        }
        /// <MetaDataID>{b8b09c98-0478-4d4e-9c44-83681cdc7ee2}</MetaDataID>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <exclude>Excluded</exclude>
        //OOAdvantech.Member<MenuModel.IClass> _Class = new OOAdvantech.Member<MenuModel.IClass>();

        OOAdvantech.Member<IClass> _Class = new OOAdvantech.Member<IClass>();
        /// <MetaDataID>{bfd499aa-91e1-4e78-bd42-783c15adf0e0}</MetaDataID>
        [PersistentMember("_Class")]
        [BackwardCompatibilityID("+1")]
        [AssociationEndBehavior(PersistencyFlag.ReferentialIntegrity)]
        public IClass Class
        {
            set
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _Class.Value = value;//.Value = value; 
                    stateTransition.Consistent = true;
                }
            }
            get
            {
                return _Class.Value;
            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<IClassified> _ClassifiedItems = new OOAdvantech.Collections.Generic.Set<IClassified>();

        /// <MetaDataID>{ce14aeb6-477e-41ae-ab1b-f0bb00903fea}</MetaDataID>
        [PersistentMember(nameof(_ClassifiedItems))]
        [TransactionalMember(LockOptions.Shared,nameof(_ClassifiedItems))]
        [BackwardCompatibilityID("+2")]

        public IList<MenuModel.IClassified> ClassifiedItems
        {
            get
            {

                return _ClassifiedItems.ToThreadSafeList();
            }
        }

        /// <exclude>Excluded</exclude>
        string _Name;

        /// <MetaDataID>{c2f32017-3394-427d-ad1a-a4fdc2821df9}</MetaDataID>
        [BackwardCompatibilityID("+3")]
        [PersistentMember("_Name")]
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _Name = value;
                    stateTransition.Consistent = true;
                }
            }
        }

        /// <MetaDataID>{2204acb6-0184-431d-bd10-3792f72843a4}</MetaDataID>
        public List<IMenuItemType> MenuItemTypes
        {
            get
            {
                return (from menuItemType in ClassifiedItems.OfType<IMenuItemType>()
                        select menuItemType).ToList();
            }
        }

        /// <MetaDataID>{bd62a99f-12d6-4487-8be7-bd5ab7036c6c}</MetaDataID>
        public IItemsCategory Parent => Class as IItemsCategory;

        /// <MetaDataID>{8d634d5a-1bbc-4581-a506-85ff54a0d3e5}</MetaDataID>
        public IList<IItemsCategory> SubCategories => ClassifiedItems.OfType<IItemsCategory>().ToList();

        /// <MetaDataID>{a890dc6b-356c-49ef-89e4-bcfdaa7479f0}</MetaDataID>
        public IList<IMenuItem> MenuItems => ClassifiedItems.OfType<IMenuItem>().ToList();

        /// <MetaDataID>{0ddc5d9b-a08a-47de-81f0-f37f5c5d9d62}</MetaDataID>
        public void AddClassifiedItem(IClassified classifiedItem)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this,nameof(ClassifiedItems)))
            {
                _ClassifiedItems.Add(classifiedItem);
                stateTransition.Consistent = true;
            }
        }

        /// <MetaDataID>{4eec6ee4-a387-474b-9da5-1034b579652d}</MetaDataID>
        public void InsertClassifiedItem(int index, IClassified classifiedItem)
        {
            _ClassifiedItems.Insert(index, classifiedItem);

        }

        /// <MetaDataID>{9511b9bc-79a3-440e-9822-8ec043fb3cdf}</MetaDataID>
        public void RemoveClassifiedItem(IClassified classifiedItem)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _ClassifiedItems.Remove(classifiedItem);
                stateTransition.Consistent = true;
            }
            int tt = _ClassifiedItems.Count;

        }

        /// <MetaDataID>{4ce761ed-ac4d-4999-95a9-b82652660339}</MetaDataID>
        public IItemsCategory NewSubCategory(string newItemsCategoryName)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                var newItemsCategory = new ItemsCategory(newItemsCategoryName);
                OOAdvantech.PersistenceLayer.ObjectStorage objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this);
                if (objectStorage != null)
                    objectStorage.CommitTransientObjectState(newItemsCategory);
                AddClassifiedItem(newItemsCategory);
                stateTransition.Consistent = true;
                return newItemsCategory;
            }
        }

        /// <MetaDataID>{8bcad8fe-3fd5-488e-99a8-017912f55ba0}</MetaDataID>
        public bool CanDeleteSubCategory(IItemsCategory itemsCategory)
        {
            return _ClassifiedItems.CanDeletePermanently(itemsCategory);
        }
    }
}