using FlavourBusinessFacade.HumanResources;
using OOAdvantech.MetaDataRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceContextManagerApp
{
    /// <MetaDataID>{a83f46df-66d6-4c24-96ce-ce17f03b4056}</MetaDataID>
    [HttpVisible]
    public interface IWaiterPresentation
    {
        /// <MetaDataID>{f9330aa3-1bad-4709-ae81-e63572f423bc}</MetaDataID>
        string FullName { get; set; }
        /// <MetaDataID>{a43d4066-ecf3-4a87-bf30-0c30c8a8e074}</MetaDataID>
        string UserName { get; set; }
        /// <MetaDataID>{10cf6d90-36b8-421b-9c35-350a2c5f0a95}</MetaDataID>
        string Email { get; set; }

        string PhotoUrl { get; set; }

        string WaiterIdentity { get; }

        bool Suspended { get; }


        bool InActiveShiftWork { get; }

        System.DateTime ActiveShiftWorkStartedAt { get; }

        void ChangeSiftWork(DateTime startedAt, double timespanInHours);

        System.DateTime ActiveShiftWorkEndsAt { get; }

        List<IServingShiftWork> GetSifts(DateTime startDate, DateTime endDate);

        List<IServingShiftWork> GetLastThreeSifts();


    }
}
