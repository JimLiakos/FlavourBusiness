using System;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;

namespace FlavourBusinessManager.ServicesContextResources
{
    /// <MetaDataID>{82bb8c43-f1bb-4e36-8c91-d9e217bfbdd1}</MetaDataID>
    [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("{82bb8c43-f1bb-4e36-8c91-d9e217bfbdd1}")]
    [OOAdvantech.MetaDataRepository.Persistent()]
    public class CallerIDLine : MarshalByRefObject, OOAdvantech.Remoting.IExtMarshalByRefObject, FlavourBusinessFacade.ServicesContextResources.ICallerIDLine
    {


        /// <MetaDataID>{4388fdbb-070e-4757-8280-21342024ab91}</MetaDataID>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;


        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{50f1363e-3a27-4ed7-a044-de16743e33ee}</MetaDataID>
        string _LineDescription;

        /// <MetaDataID>{a91c47fb-9b42-4b46-970c-c7b1773d54ec}</MetaDataID>
        [PersistentMember(nameof(_LineDescription))]
        [BackwardCompatibilityID("+1")]
        public string LineDescription
        {
            get
            {
                return _LineDescription;
            }

            set
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _LineDescription = value;
                    stateTransition.Consistent = true;
                }

            }
        }
    }
}