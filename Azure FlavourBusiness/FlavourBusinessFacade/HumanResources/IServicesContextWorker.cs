using OOAdvantech.MetaDataRepository;
using System;

namespace FlavourBusinessFacade.HumanResources
{
    /// <MetaDataID>{55339f08-4525-4c82-bd59-ac39342563f0}</MetaDataID>
    [BackwardCompatibilityID("{55339f08-4525-4c82-bd59-ac39342563f0}")]
    [GenerateFacadeProxy]
    public interface IServicesContextWorker : IParty
    {
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
        IShiftWork ActiveShiftWork { get; }

        /// <MetaDataID>{8ded2820-79d2-4ae2-8508-15d1f9d7458b}</MetaDataID>
        void ChangeSiftWork(IShiftWork shiftWork, DateTime startedAt, double timespanInHours);
    }
}