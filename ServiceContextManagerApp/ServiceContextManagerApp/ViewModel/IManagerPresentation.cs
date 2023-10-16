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
        /// <MetaDataID>{73740119-002c-4961-8a60-915180086a7a}</MetaDataID>
        bool IsOrganizationManager { get; }

        /// <MetaDataID>{5482ddbf-74e3-4511-b86e-ad09e15cc91d}</MetaDataID>
        bool IsServiceContextSupervisor { get; }

        /// <MetaDataID>{73f283b4-db60-40fa-8fc6-5175295ee7be}</MetaDataID>
        NewUserCode GetNewSupervisorQRCode(IServicesContextPresentation servicesContext, string color);



        /// <MetaDataID>{10930d35-8234-4c72-8e94-6a752e759566}</MetaDataID>
        string FlavoursServiceContextDescription { get; }

        /// <MetaDataID>{298f0136-8365-4068-bdad-8ff538b16207}</MetaDataID>
        List<IServicesContextPresentation> ServicesContexts { get; }


        /// <MetaDataID>{192517c7-d832-4dac-84d7-993a2fabbd20}</MetaDataID>
        Task<bool> AssignSupervisor();




    }

    /// <MetaDataID>{8ec8c78a-0c9b-4abb-9340-5f1186361844}</MetaDataID>
    [HttpVisible]
    public class NewUserCode
    {
        public string QRCode { get; set; }
        public string Code { get; set; }
    }


}
