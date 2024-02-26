using FlavourBusinessFacade.PriceList;
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
                ItemsPrices.Add(itemsPriceInfo);
                stateTransition.Consistent = true;
            }

        }

        /// <MetaDataID>{68e9146a-561a-49c4-935c-c83d784aa782}</MetaDataID>
        public void RemoveItemsPriceInfos(IItemsPriceInfo itemsPriceInfo)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                ItemsPrices.Remove(itemsPriceInfo);
                stateTransition.Consistent = true;
            }
        }
    }
}