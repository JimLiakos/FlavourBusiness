using System;

using OOAdvantech.Collections.Generic;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;

namespace MenuModel
{
    /// <MetaDataID>{ffd97b11-b13b-4b1f-acb3-08566035ecfb}</MetaDataID>
    [BackwardCompatibilityID("{ffd97b11-b13b-4b1f-acb3-08566035ecfb}")]
    [Persistent()]
    public class ItemSelectorOptionsGroup : PreparationOptionsGroup
    {
        /// <MetaDataID>{cd722275-fd49-4065-842d-9ad8e5ffbca7}</MetaDataID>
        public ItemSelectorOptionsGroup()
        {
            base.SelectionType = SelectionType.SingleSelection | SelectionType.AtLeastOneSelected;
        }


        /// <MetaDataID>{be01801f-aae3-4894-a1e6-cd3d4e1ff927}</MetaDataID>
        public ItemSelectorOptionsGroup(string name) : this()
        {
            _Name.Value = name;
        }

        /// <MetaDataID>{c9c8fc1c-a89f-42dc-acc6-f87e4e8aba7f}</MetaDataID>
        public override void AddPreparationOption(IPreparationScaledOption preparationOption)
        {
            throw new NotSupportedException("AddPreparationOption not supported. Use NewPreparationOption istead");
        }


    
        /// <MetaDataID>{e15445d2-7fb9-45be-b41b-98ab4c039d7b}</MetaDataID>
        public override IPreparationScaledOption NewPreparationOption()
        {
            IPreparationScaledOption option = new ItemSelectorOption();
            base.AddPreparationOption(option);


            OOAdvantech.PersistenceLayer.ObjectStorage objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this);
            if (objectStorage != null)
                objectStorage.CommitTransientObjectState(option);


            return option;

            // return base.NewPreparationOption();
        }

        /// <MetaDataID>{258fe6b2-3186-4c7f-a0b9-64eced1952ca}</MetaDataID>
        public override SelectionType SelectionType
        {
            get
            {
                return base.SelectionType;
            }

            set
            {
                base.SelectionType = value;
                base.SelectionType = SelectionType.SingleSelection | SelectionType.AtLeastOneSelected;
            }
        }

    }
}