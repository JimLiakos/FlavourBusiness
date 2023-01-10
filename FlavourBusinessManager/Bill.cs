using FinanceFacade;
using OOAdvantech.MetaDataRepository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlavourBusinessManager.EndUsers
{
    /// <MetaDataID>{0a80e52c-fc44-4bc3-8e84-970570d12727}</MetaDataID>
    public class Bill : MarshalByRefObject, FlavourBusinessFacade.RoomService.IBill, OOAdvantech.Remoting.IExtMarshalByRefObject
    {
        public Bill(List<IPayment> payments)
        {
            Payments=payments;
            
        }
        [CachingDataOnClientSide]
        public List<IPayment> Payments { get; }

        public FinanceFacade.IPayment OpenPayment
        {
            get
            {
                return Payments.Where(x => x.State!=PaymentState.Completed).FirstOrDefault();
            }
        }
    }

}