using OOAdvantech.MetaDataRepository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WaiterApp.ViewModel
{
    /// <MetaDataID>{981df64d-06d2-47af-9900-792dd6492ef0}</MetaDataID>
    [HttpVisible]
    interface IWaiterPresentation
    {

        /// <MetaDataID>{315e4a19-6d46-4ae0-8e38-6bef23632d5a}</MetaDataID>
        Task<bool> AssignWaiter();

        /// <MetaDataID>{b385fbf4-0e92-4edb-9862-20a31ed527e5}</MetaDataID>
        bool IsActiveWaiter { get; }
    }
}
