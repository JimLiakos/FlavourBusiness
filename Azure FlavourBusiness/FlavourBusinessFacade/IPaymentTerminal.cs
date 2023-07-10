using OOAdvantech;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Remoting;
using System;

namespace FlavourBusinessFacade.ServicesContextResources
{
    /// <MetaDataID>{58d3cf0b-b25b-491a-aea9-9c8105bfa5e9}</MetaDataID>
    [BackwardCompatibilityID("{58d3cf0b-b25b-491a-aea9-9c8105bfa5e9}")]
    public interface IPaymentTerminal
    {
        /// <MetaDataID>{86a88de9-c388-47de-87c8-71af0a6a8273}</MetaDataID>
        [BackwardCompatibilityID("+3")]
        string Description { get; set; }

        /// <MetaDataID>{8793dde9-ded9-4857-a26e-9ed33851e246}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        string ServicesContextIdentity { get; set; }

        /// <MetaDataID>{9f04f16f-a61c-4e01-a96a-ce41c1e23cc8}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        string PaymentTerminalIdentity { get; set; }

        [RemoteEventPublish(InvokeType.Async)]
        event ObjectChangeStateHandle ObjectChangeState;

        
    }
}