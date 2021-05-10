using OOAdvantech.MetaDataRepository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContextManagerApp
{

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

    [HttpVisible]
    public class NewSupervisorCode
    {
        public string QRCode { get; set; }
        public string Code { get; set; }
    }


}
