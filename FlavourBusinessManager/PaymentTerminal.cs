using OOAdvantech;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using System;

namespace FlavourBusinessManager.ServicesContextResources
{
    /// <MetaDataID>{21612961-3fff-4664-a99d-b3619c9159c2}</MetaDataID>
    public class PaymentTerminal :MarshalByRefObject, OOAdvantech.Remoting.IExtMarshalByRefObject, FlavourBusinessFacade.ServicesContextResources.IPaymentTerminal
    {

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <exclude>Excluded</exclude>
        string _Description;

        /// <MetaDataID>{7fafc070-8dcc-4238-aa35-9d6819813a52}</MetaDataID>
        [PersistentMember(nameof(_Description))]
        [BackwardCompatibilityID("+1")]
        public string Description
        {
            get => _Description; set
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
        string _ServicesContextIdentity;

        /// <MetaDataID>{d94eefe5-a6cf-42c7-8201-362959967948}</MetaDataID>
        [PersistentMember(nameof(_ServicesContextIdentity))]
        [BackwardCompatibilityID("+2")]
        public string ServicesContextIdentity
        {
            get => _ServicesContextIdentity; set
            {
                if (_ServicesContextIdentity!=value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ServicesContextIdentity=value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <exclude>Excluded</exclude>
        string _PaymentTerminalIdentity;

        public event ObjectChangeStateHandle ObjectChangeState;

        /// <MetaDataID>{73b4a925-fd49-495e-8761-3c1ed1c4274f}</MetaDataID>
        public PaymentTerminal(string servicesContextIdentity)
        {
            ServicesContextIdentity=servicesContextIdentity;
        }
        /// <MetaDataID>{a95e5da5-c4ea-4c44-8385-42b91901b497}</MetaDataID>
        public PaymentTerminal()
        {

        }

        /// <MetaDataID>{b701b043-bbd8-4ba8-8a33-3513bc2634be}</MetaDataID>
        [PersistentMember(nameof(_PaymentTerminalIdentity))]
        [BackwardCompatibilityID("+3")]
        public string PaymentTerminalIdentity
        {
            get => _PaymentTerminalIdentity; set
            {
                if (_PaymentTerminalIdentity!=value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _PaymentTerminalIdentity=value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

    }
}