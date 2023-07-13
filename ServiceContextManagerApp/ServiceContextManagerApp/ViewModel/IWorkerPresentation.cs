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
        bool Suspended { get; }


        bool InActiveShiftWork { get; }

        System.DateTime ActiveShiftWorkStartedAt { get; }

        void ChangeSiftWork(DateTime startedAt, double timespanInHours);

        System.DateTime ActiveShiftWorkEndsAt { get; }

        List<IServingShiftWork> GetSifts(DateTime startDate, DateTime endDate);

        List<IServingShiftWork> GetLastThreeSifts();
    }
}
