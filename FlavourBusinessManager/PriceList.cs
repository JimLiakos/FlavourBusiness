using FlavourBusinessFacade.PriceList;
using FlavourBusinessFacade.ServicesContextResources;
using FlavourBusinessManager.ServicesContextResources;
using MenuModel;
using OOAdvantech;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using System.Collections.Generic;

namespace FlavourBusinessManager.PriceList
{
    /// <MetaDataID>{089749b1-083b-4348-b598-6fd512b853a5}</MetaDataID>
    [BackwardCompatibilityID("{089749b1-083b-4348-b598-6fd512b853a5}")]
    [Persistent()]
    public class PriceList : IPriceList
    {
        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;
        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<IItemsPriceInfo> _ItemsPrices = new OOAdvantech.Collections.Generic.Set<IItemsPriceInfo>();
        /// <MetaDataID>{5bc8adba-d38c-4992-98a6-6a3ce5881759}</MetaDataID>
        [PersistentMember(nameof(_ItemsPrices))]
        [AssociationEndBehavior(PersistencyFlag.CascadeDelete | PersistencyFlag.OnConstruction)]
        [BackwardCompatibilityID("+1")]
        public System.Collections.Generic.List<FlavourBusinessFacade.PriceList.IItemsPriceInfo> ItemsPrices => _ItemsPrices.ToThreadSafeList();

        /// <exclude>Excluded</exclude>
        string _Description;

        /// <MetaDataID>{c99d37ac-f822-4c25-b668-f7bf7cf9f527}</MetaDataID>
        [PersistentMember(nameof(_Description))]
        [BackwardCompatibilityID("+2")]
        public string Description
        {
            get => _Description; set
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

        /// <MetaDataID>{0131fa91-7249-4e84-b030-a40b49940389}</MetaDataID>
        public void AddItemsPriceInfos(IItemsPriceInfo itemsPriceInfo)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _ItemsPrices.Add(itemsPriceInfo);
                stateTransition.Consistent = true;
            }

        }

        /// <MetaDataID>{68e9146a-561a-49c4-935c-c83d784aa782}</MetaDataID>
        public void RemoveItemsPriceInfos(IItemsPriceInfo itemsPriceInfo)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _ItemsPrices.Remove(itemsPriceInfo);
                stateTransition.Consistent = true;
            }
        }

        public event ObjectChangeStateHandle ObjectChangeState;

        /// <MetaDataID>{2cfb3a5d-e099-48a6-917a-848ac5fae692}</MetaDataID>
        public void RemoveItemsPriceInfos(List<IItemsPriceInfo> itemsPriceInfos)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                foreach (var itemsPreparationInfo in itemsPriceInfos)
                    this._ItemsPrices.Remove(itemsPreparationInfo);
                stateTransition.Consistent = true;
            }
            ObjectChangeState?.Invoke(this, null);

        }

        /// <MetaDataID>{cbbb4db7-0a70-4867-a942-e58184352c3a}</MetaDataID>
        public IItemsPriceInfo NewPriceInfo(string itemsInfoObjectUri, ItemsPriceInfoType itemsPriceInfoType)
        {
            try
            {
                var obj = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfoObjectUri);

                if (obj is MenuModel.ItemsCategory)
                {
                    ItemsPriceInfo itemsPiceInfo = new ItemsPriceInfo(obj as MenuModel.ItemsCategory);

                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(itemsPiceInfo);
                        this._ItemsPrices.Add(itemsPiceInfo);
                        itemsPiceInfo.ItemsPriceInfoType = itemsPriceInfoType;

                        stateTransition.Consistent = true;
                    }

                    return itemsPiceInfo;
                }

                if (obj is MenuModel.IMenuItem)
                {
                    ItemsPriceInfo itemsPiceInfo = new ItemsPriceInfo(obj as MenuModel.IMenuItem);

                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(itemsPiceInfo);
                        this._ItemsPrices.Add(itemsPiceInfo);
                        itemsPiceInfo.ItemsPriceInfoType = itemsPriceInfoType;
                        stateTransition.Consistent = true;
                    }

                    return itemsPiceInfo;

                }
            }
            finally
            {
                ObjectChangeState?.Invoke(this, null);
            }

            return null;

        }
    }
}