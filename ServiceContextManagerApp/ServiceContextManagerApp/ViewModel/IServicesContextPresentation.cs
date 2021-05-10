using OOAdvantech.MetaDataRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceContextManagerApp
{
    /// <MetaDataID>{ac7629ae-b6fb-4a24-9f62-f94f27f4b9d7}</MetaDataID>
    [HttpVisible]
    public interface IServicesContextPresentation
    {
        string ServicesContextName { get; set; }

        List<ISupervisorPresentation> Supervisors { get; }

        List<IWaiterPresentation> Waiters { get; }

        bool RemoveSupervisor(ISupervisorPresentation supervisorPresentation);

        void MakeSupervisorActive(ISupervisorPresentation supervisorPresentation);

        [GenerateEventConsumerProxy]
        event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;


        NewSupervisorCode GetNewWaiterQRCode( string color);


    }
}
