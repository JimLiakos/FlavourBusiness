using OOAdvantech.MetaDataRepository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContextManagerApp
{

    /// <MetaDataID>{bf7e3f02-1669-499b-97d4-b9c001d76fa4}</MetaDataID>
    [HttpVisible]
    interface IManagerPresentation
    {
        bool IsOrganizationManager { get; }

        bool IsServiceContextSupervisor { get; }

        NewSupervisorCode GetNewSupervisorQRCode(IServicesContextPresentation servicesContext, string color);



        string FlavoursServiceContextDescription { get; }

        List<IServicesContextPresentation> ServicesContexts { get; }


        Task<bool> AssignSupervisor();


    }

    /// <MetaDataID>{8ec8c78a-0c9b-4abb-9340-5f1186361844}</MetaDataID>
    [HttpVisible]
    public class NewSupervisorCode
    {
        public string QRCode { get; set; }
        public string Code { get; set; }
    }


}
