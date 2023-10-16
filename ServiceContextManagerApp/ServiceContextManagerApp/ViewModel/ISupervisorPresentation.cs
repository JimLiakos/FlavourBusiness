using OOAdvantech;
using OOAdvantech.MetaDataRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceContextManagerApp
{
    /// <MetaDataID>{e508bdac-d089-4daa-9145-abc2fe318f38}</MetaDataID>
    [HttpVisible]
    public interface ISupervisorPresentation
    {
        /// <MetaDataID>{41a5d1a3-9a12-45fb-af72-b14bd48f1b5b}</MetaDataID>
        bool InActiveShiftWork { get; }

        /// <MetaDataID>{a7a535cf-f3b9-4328-9b03-f7a22eb97710}</MetaDataID>
        DateTime ActiveShiftWorkStartedAt { get; }

        /// <MetaDataID>{47584cc2-c05e-4e6d-8798-102dd8e6fa3d}</MetaDataID>
        DateTime ActiveShiftWorkEndsAt { get; }

        /// <MetaDataID>{1ec5fce4-b4b9-476a-b7ee-14b0db6c99ca}</MetaDataID>
        void ExtendShiftWorkStart(double timespanInHours);

        /// <MetaDataID>{0153ab81-9272-4fe9-9fca-c958c90da31b}</MetaDataID>
        void ShiftWorkStart(DateTime startedAt, double timespanInHours);

        /// <MetaDataID>{f9330aa3-1bad-4709-ae81-e63572f423bc}</MetaDataID>
        string FullName { get; set; }
        /// <MetaDataID>{a43d4066-ecf3-4a87-bf30-0c30c8a8e074}</MetaDataID>
        string UserName { get; set; }
        /// <MetaDataID>{10cf6d90-36b8-421b-9c35-350a2c5f0a95}</MetaDataID>
        string Email { get; set; }

        /// <MetaDataID>{a6ab320b-225a-40d5-a44f-f582b5216a08}</MetaDataID>
        string PhotoUrl { get; set; }

        /// <MetaDataID>{5087ce21-b5e1-4769-a51e-326204119bf0}</MetaDataID>
        string SupervisorIdentity { get; }

        /// <MetaDataID>{5a621115-a14d-4bcc-92ce-4c65cdf9e14f}</MetaDataID>
        bool Suspended { get; }

        [GenerateEventConsumerProxy]
        event ObjectChangeStateHandle ObjectChangeState;

    }
}
