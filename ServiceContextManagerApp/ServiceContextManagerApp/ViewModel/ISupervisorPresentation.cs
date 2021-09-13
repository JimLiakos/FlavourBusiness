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
        /// <MetaDataID>{f9330aa3-1bad-4709-ae81-e63572f423bc}</MetaDataID>
        string FullName { get; set; }
        /// <MetaDataID>{a43d4066-ecf3-4a87-bf30-0c30c8a8e074}</MetaDataID>
        string UserName { get; set; }
        /// <MetaDataID>{10cf6d90-36b8-421b-9c35-350a2c5f0a95}</MetaDataID>
        string Email { get; set; }

        string PhotoUrl { get; set; }

        string SupervisorIdentity { get; }

        bool Suspended { get; }

    }
}
