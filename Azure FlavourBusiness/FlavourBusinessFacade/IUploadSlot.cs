using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlavourBusinessFacade
{
    /// <MetaDataID>{c3b22da1-140a-4994-bf0a-d091fd2af813}</MetaDataID>
    [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("{c3b22da1-140a-4994-bf0a-d091fd2af813}")]
    [OOAdvantech.MetaDataRepository.HttpVisible]
    public interface IUploadSlot
    {
        string CreateFileUploadSession(int length, string contentType);
        bool UploadFile(string fileUploadIdentity, string dataChunk);
        void EndOfUploadFile(string fileUploadIdentity);
    }

    /// <MetaDataID>{86ca9713-ad55-4680-9306-56f4dffd24a0}</MetaDataID>
    [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("{86ca9713-ad55-4680-9306-56f4dffd24a0}")]
    [OOAdvantech.MetaDataRepository.HttpVisible]
    public interface IUploadService
    {
        /// <MetaDataID>{3a545216-8803-4775-843c-6b2c5ca94e81}</MetaDataID>
        IUploadSlot GetUploadSlotFor(OrganizationStorageRef storageRef);
    }

}
