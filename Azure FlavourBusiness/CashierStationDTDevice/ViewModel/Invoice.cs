using FinanceFacade;
using FlavourBusinessFacade.ServicesContextResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashierStationDevice.ViewModel
{
    /// <MetaDataID>{e72a2600-cde3-4d57-aadf-e930781fcf2e}</MetaDataID>
    public class Invoice
    {
        public Invoice(ITransaction transaction,IFisicalParty issuer)
        {
            Transaction = transaction;
            Items = transaction?.Items.ToList();
            Issuer = issuer;
            Name = "Microneme";
        }

        public string Name { get; set; }

        public IFisicalParty Issuer { get; set; }

        public List<IItem> Items { get; }
        public ITransaction Transaction { get; }
    }
}
