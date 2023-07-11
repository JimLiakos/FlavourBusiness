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

        /// <MetaDataID>{0d85a95f-541f-4f8d-8547-0edf585187c2}</MetaDataID>
        public IList<IServiceContextSupervisor> Supervisors { get; set; }

        public IList<IShiftWork> ActiveShiftWorks { get; set; }
        
    }
}