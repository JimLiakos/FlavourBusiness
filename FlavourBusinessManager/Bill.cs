using FinanceFacade;
using OOAdvantech.MetaDataRepository;
using System;
using System.Collections.Generic;

namespace FlavourBusinessManager.EndUsers
{
    /// <MetaDataID>{0a80e52c-fc44-4bc3-8e84-970570d12727}</MetaDataID>
    public class Bill :MarshalByRefObject, FlavourBusinessFacade.RoomService.IBill,OOAdvantech.Remoting.IExtMarshalByRefObject
    {
        public Bill(List<IPayment> payments, List<IItem> canceledItems)
        {
            Payments=payments;
            CanceledItems=canceledItems;
        }
        [CachingDataOnClientSide]
        public List<IPayment> Payments { get; }

        public List<IItem> CanceledItems { get; }
    }

}