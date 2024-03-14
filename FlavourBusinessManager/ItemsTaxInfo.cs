using FlavourBusinessFacade.PriceList;
using MenuModel;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;

namespace FlavourBusinessManager.PriceList
{
    /// <MetaDataID>{bde00c44-4e59-40c7-8f7e-ea006e3ed7da}</MetaDataID>
    [BackwardCompatibilityID("{bde00c44-4e59-40c7-8f7e-ea006e3ed7da}")]
    [Persistent()]
    public class ItemsTaxInfo : FlavourBusinessFacade.PriceList.IItemsTaxInfo
    {

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;


        /// <exclude>Excluded</exclude>
        string _Description;

        /// <MetaDataID>{2b71126c-f4f4-4e31-983d-dc611c8aa9c5}</MetaDataID>
        [PersistentMember(nameof(_Description))]
        [BackwardCompatibilityID("+3")]
        public string Description
        {
            get => _Description;
            set
            {

                if (_Description!=value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Description=value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }


        /// <exclude>Excluded</exclude>
        string _ItemsInfoObjectUri;

        /// <MetaDataID>{fb583e4d-ce38-4cf4-b15c-9803cfe6b8bd}</MetaDataID>
        [PersistentMember(nameof(_ItemsInfoObjectUri))]
        [BackwardCompatibilityID("+1")]
        public string ItemsInfoObjectUri
        {
            get => _ItemsInfoObjectUri;
            set
            {
                if (_ItemsInfoObjectUri!=value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ItemsInfoObjectUri=value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }



        /// <exclude>Excluded</exclude>
        ItemsPriceInfoType _ItemsPriceInfoType;


        /// <MetaDataID>{534de307-5b6a-46ec-a79b-f106a905d5a3}</MetaDataID>
        [PersistentMember(nameof(_ItemsPriceInfoType))]
        [BackwardCompatibilityID("+2")]
        public FlavourBusinessFacade.PriceList.ItemsPriceInfoType ItemsPriceInfoType { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        /// <MetaDataID>{5e632c89-43de-4020-a085-dbaee7a13a7c}</MetaDataID>
        public MenuModel.IClassified MenuModelObject => throw new System.NotImplementedException();




    }
}