using OOAdvantech.MetaDataRepository;
using System;
using System.Collections.Generic;

namespace FlavourBusinessFacade.HumanResources
{
    /// <MetaDataID>{55339f08-4525-4c82-bd59-ac39342563f0}</MetaDataID>
    [BackwardCompatibilityID("{55339f08-4525-4c82-bd59-ac39342563f0}")]
    [GenerateFacadeProxy]
    public interface IServicesContextWorker : IParty
    {
        /// <MetaDataID>{b166ebd0-afeb-40af-b8cb-324fffad2a0d}</MetaDataID>
        [BackwardCompatibilityID("+5")]
        string UserLanguageCode { get; set; }

        /// <MetaDataID>{adc60835-ab6b-4576-89a2-3f6a7e4aa9f9}</MetaDataID>
        [BackwardCompatibilityID("+4")]
        string WorkerAssignKey { get; set; }

        /// <MetaDataID>{337b7e3f-56a4-4115-bfc7-5a91295fba8c}</MetaDataID>
        [BackwardCompatibilityID("+3")]
        bool Suspended { get; }

        /// <MetaDataID>{f1cb514f-f976-4008-9605-7129f1b57790}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        string ServicesContextIdentity { get; set; }
        [Association("WorkerShifWork", Roles.RoleA, "484dfd7b-d501-43b7-871c-f247f3e648b1" )]
        System.Collections.Generic.IList<IShiftWork> ShiftWorks { get; }

        //IServicesContextWorker
        //[Association("WorkerShifWork", Roles.RoleA, "484dfd7b-d501-43b7-871c-f247f3e648b1")]
        //System.Collections.Generic.IList<FlavourBusinessFacade.HumanResources.IShiftWork> ShiftWorks { get; }




        /// <MetaDataID>{c6736e13-3e0f-4903-b160-e66dad6b9e1c}</MetaDataID>
        void RemoveShiftWork(IShiftWork shifWork);
        /// <MetaDataID>{a6830a28-86ee-436e-8df8-2d48e9731865}</MetaDataID>
        IShiftWork NewShiftWork(System.DateTime startedAt, double timespanInHours);
        /// <MetaDataID>{78a715ff-bcc3-42c4-9cb5-864e92de4bbf}</MetaDataID>
        IShiftWork ShiftWork { get; }

        /// <MetaDataID>{8ded2820-79d2-4ae2-8508-15d1f9d7458b}</MetaDataID>
        void ChangeSiftWork(FlavourBusinessFacade.HumanResources.IShiftWork shiftWork, System.DateTime startedAt, double timespanInHours);


        /// <MetaDataID>{e10ce69e-6209-4970-98d0-de98153fd84f}</MetaDataID>
        List<IServingShiftWork> GetSifts(DateTime startDate, DateTime endDate);


        /// <MetaDataID>{41bd3c34-4706-430b-b450-48d352418ae6}</MetaDataID>
        List<IServingShiftWork> GetLastThreeSifts();

        /// <MetaDataID>{d2bfcb05-fdc1-4ca3-9bbf-e9fb6da31696}</MetaDataID>
        [CachingDataOnClientSide]
        bool NativeUser { get; set; }

    }
}