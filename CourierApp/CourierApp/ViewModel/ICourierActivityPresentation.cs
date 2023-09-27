using FlavourBusinessFacade;
using OOAdvantech.MetaDataRepository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CourierApp.ViewModel
{
    /// <MetaDataID>{99fe8217-2732-4d1a-b074-c88a95272563}</MetaDataID>
    [HttpVisible]
    public interface ICourierActivityPresentation
    {


        Task<bool> AssignCourier();

        Task<UserData> AssignDevice();

        bool InActiveShiftWork { get; }


        System.DateTime ActiveShiftWorkStartedAt { get; }


        System.DateTime ActiveShiftWorkEndsAt { get; }

        void SiftWorkStart(DateTime startedAt, double timespanInHours);

        void ExtendSiftWorkStart(double timespanInHours);

    }
}
