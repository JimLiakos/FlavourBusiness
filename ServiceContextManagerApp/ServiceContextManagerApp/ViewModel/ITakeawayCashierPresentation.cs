using FlavourBusinessFacade.HumanResources;
using OOAdvantech.MetaDataRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceContextManagerApp
{

    /// <MetaDataID>{3fa3a62c-423a-4b4f-aa48-6ee62af2c1ee}</MetaDataID>
    [HttpVisible]
    public interface ITakeawayCashierPresentation
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

        /// <MetaDataID>{377607b2-f4eb-4982-959a-a05080e91710}</MetaDataID>
        bool Suspended { get; }


        /// <MetaDataID>{e1d3990f-7f85-498f-a299-234e446836c4}</MetaDataID>
        bool InActiveShiftWork { get; }

        /// <MetaDataID>{a71a1e34-f54d-4d64-adf9-c67c48fdbc15}</MetaDataID>
        System.DateTime ActiveShiftWorkStartedAt { get; }

        /// <MetaDataID>{5ec2ff22-baae-40fa-a267-1254c7bcef43}</MetaDataID>
        void ChangeSiftWork(DateTime startedAt, double timespanInHours);

        /// <MetaDataID>{f9d7db3c-494f-4e55-ad3c-50514f06a4ac}</MetaDataID>
        System.DateTime ActiveShiftWorkEndsAt { get; }

        /// <MetaDataID>{1ea21a08-d046-4b80-ab75-66a43ad543e1}</MetaDataID>
        List<IServingShiftWork> GetSifts(DateTime startDate, DateTime endDate);

        /// <MetaDataID>{940f0f62-6bfc-4e23-821b-d198aa71525e}</MetaDataID>
        List<IServingShiftWork> GetLastThreeSifts();
    }
}
