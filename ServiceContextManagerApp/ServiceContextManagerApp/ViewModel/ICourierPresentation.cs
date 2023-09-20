using OOAdvantech.MetaDataRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceContextManagerApp
{
    /// <MetaDataID>{736132b0-eb8f-406f-bbbe-dcbff02b2479}</MetaDataID>
    [HttpVisible]
    public interface ICourierPresentation : ViewModel.IWorkerPresentation
    {
        /// <MetaDataID>{8f62c7fc-02f7-4d11-a06c-3299f7bffc46}</MetaDataID>
        string FullName { get; set; }

        /// <MetaDataID>{c393118f-810d-45e5-a0f3-f792f29f45dc}</MetaDataID>
        string UserName { get; set; }

        /// <MetaDataID>{68073750-eb8f-4145-8be7-5c08c7517074}</MetaDataID>
        string Email { get; set; }

        /// <MetaDataID>{5b045399-60bd-4e6e-9a83-17f0fef39e1e}</MetaDataID>
        string PhotoUrl { get; set; }

        /// <MetaDataID>{01b0f834-95ce-4615-a300-e90b2e17daf1}</MetaDataID>
        string CourierIdentity { get; }

    }
}
