using FlavourBusinessFacade.HumanResources;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using System;

namespace FlavourBusinessManager.HumanResources
{
    /// <MetaDataID>{efe4e65a-7af9-40a6-baff-c02b437d57c1}</MetaDataID>
    [BackwardCompatibilityID("{efe4e65a-7af9-40a6-baff-c02b437d57c1}")]
    [Persistent()]
    public class MenuDesignActivity : OOAdvantech.Remoting.ExtMarshalByRefObject, FlavourBusinessFacade.HumanResources.IMenuDesignActivity
    {
        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <exclude>Excluded</exclude>
        string _Name;

        /// <MetaDataID>{cc1864e1-6234-4f66-a4db-c2a7c05e122a}</MetaDataID>
        [PersistentMember(nameof(_Name))]
        [BackwardCompatibilityID("+1")]
        public string Name
        {
            get => _Name;
            set
            {

                if (_Name != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Name = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        string _DesigneSubjectIdentity;

        /// <MetaDataID>{1065f243-4bc5-4c7f-bbf2-7141ebac589c}</MetaDataID>
        [PersistentMember(nameof(_DesigneSubjectIdentity))]
        [BackwardCompatibilityID("+2")]
        public string DesigneSubjectIdentity
        {
            get => _DesigneSubjectIdentity;
            set
            {

                if (_DesigneSubjectIdentity != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _DesigneSubjectIdentity = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }


        /// <exclude>Excluded</exclude>
        DesignSubjectType _DesignActivityType;

        /// <MetaDataID>{f68c904b-7e13-4d0f-a009-37adce537317}</MetaDataID>
        [PersistentMember(nameof(_DesignActivityType))]
        [BackwardCompatibilityID("+3")]
        public FlavourBusinessFacade.HumanResources.DesignSubjectType DesignActivityType
        {
            get => _DesignActivityType;
            set
            {
                if (_DesignActivityType != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _DesignActivityType = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        
        /// <exclude>Excluded</exclude>
        OOAdvantech.Member<IAccountability> _Accountability = new OOAdvantech.Member<IAccountability>();
        /// <MetaDataID>{bc50c044-15ed-4a5d-bb1f-74d34fd64ada}</MetaDataID>
        [PersistentMember(nameof(_Accountability))]
        [BackwardCompatibilityID("+4")]
        public FlavourBusinessFacade.HumanResources.IAccountability Accountability => _Accountability.Value;
    }
}