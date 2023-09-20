using FlavourBusinessFacade.HumanResources;
using OOAdvantech.MetaDataRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceContextManagerApp.ViewModel
{


    /// <MetaDataID>{a9aa4854-d5eb-4461-a364-3fa681119098}</MetaDataID>
    [HttpVisible]
    public interface IWorkerPresentation
    {
        /// <MetaDataID>{4b895f3e-b839-4856-b4bb-03051993e3f5}</MetaDataID>
        bool Suspended { get; }


        /// <MetaDataID>{2652e066-9ddb-4208-b17e-e022da8f8a44}</MetaDataID>
        bool InActiveShiftWork { get; }

        /// <MetaDataID>{9c5f96ba-2d4e-4b67-8a85-fe332b1c03e9}</MetaDataID>
        System.DateTime ActiveShiftWorkStartedAt { get; }

        /// <MetaDataID>{33346f2a-8c78-48eb-b817-1a8f965f7f6e}</MetaDataID>
        void ChangeSiftWork(DateTime startedAt, double timespanInHours);

        /// <MetaDataID>{919a16ad-04ef-45f3-a5fd-bdf4497653b5}</MetaDataID>
        System.DateTime ActiveShiftWorkEndsAt { get; }

        /// <MetaDataID>{e46d2635-cad8-4916-8029-32c9a2fed539}</MetaDataID>
        List<IServingShiftWork> GetSifts(DateTime startDate, DateTime endDate);

        /// <MetaDataID>{fd03d41b-d879-4d4b-965e-b5c3a84aac2f}</MetaDataID>
        List<IServingShiftWork> GetLastThreeSifts();

        bool NativeUser { get; set; }

        string WorkerIdentity { get; }
    }
}
