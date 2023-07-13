using FlavourBusinessFacade.HumanResources;
using OOAdvantech.MetaDataRepository;
using ServiceContextManagerApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceContextManagerApp
{

    /// <MetaDataID>{3fa3a62c-423a-4b4f-aa48-6ee62af2c1ee}</MetaDataID>
    [HttpVisible]
    public interface ITakeawayCashierPresentation: IWorkerPresentation
    {

        /// <MetaDataID>{f45b37b5-78d2-47a3-9e2a-1418b826be85}</MetaDataID>
        string FullName { get; set; }

        /// <MetaDataID>{275c85c5-47e5-4c13-b988-51f62980e28e}</MetaDataID>
        string UserName { get; set; }

        /// <MetaDataID>{dc5adb97-c19c-4cc7-90ec-fa8d32b1fc7d}</MetaDataID>
        string Email { get; set; }

        /// <MetaDataID>{ea0e4a79-b624-4e65-91a4-d7fc5cdba215}</MetaDataID>
        string PhotoUrl { get; set; }

        /// <MetaDataID>{a2077296-2564-4140-823f-5ba4d7a817f7}</MetaDataID>
        string TakeawayCashierIdentity { get; }


    }
}
