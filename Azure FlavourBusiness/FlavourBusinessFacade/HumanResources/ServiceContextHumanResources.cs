using FlavourBusinessFacade.ServicesContextResources;
using System.Collections.Generic;

namespace FlavourBusinessFacade.HumanResources
{
    /// <MetaDataID>{5a28c4c8-2d9d-4fe7-ae93-aa4eb87d4b5d}</MetaDataID>
    public class ServiceContextHumanResources
    {
        /// <MetaDataID>{4f5502c2-abdf-4257-b850-57ecb031435c}</MetaDataID>
        public IList<IWaiter> Waiters { get; set; }

        public IList<ITakeawayCashier> TakeawayCashiers { get; set; }

        /// <MetaDataID>{4a365d15-672b-47c1-8667-3c20890cdc15}</MetaDataID>
        public IList<ICourier> Couriers { get; set; }

        /// <MetaDataID>{0d85a95f-541f-4f8d-8547-0edf585187c2}</MetaDataID>
        public IList<IServiceContextSupervisor> Supervisors { get; set; }

        /// <MetaDataID>{f099a90d-9d76-43c9-b736-51e8b86d1594}</MetaDataID>
        public IList<IShiftWork> ActiveShiftWorks { get; set; }

    }
}