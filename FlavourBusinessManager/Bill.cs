using FinanceFacade;
using System.Collections.Generic;

namespace FlavourBusinessManager.EndUsers
{
    /// <MetaDataID>{0a80e52c-fc44-4bc3-8e84-970570d12727}</MetaDataID>
    public class Bill : FlavourBusinessFacade.RoomService.IBill
    {
        public Bill(List<IPayment> payments)
        {
            Payments=payments;
        }
        public List<IPayment> Payments { get; }
    }

}