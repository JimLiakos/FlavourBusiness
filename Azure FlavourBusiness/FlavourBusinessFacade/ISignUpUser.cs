using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlavourBusinessFacade.ViewModel
{
    /// <MetaDataID>{96a8b867-dd68-41d0-aa1f-d8e02f00224c}</MetaDataID>
    [OOAdvantech.MetaDataRepository.HttpVisible]
    public interface IUser
    {
        /// <MetaDataID>{f9330aa3-1bad-4709-ae81-e63572f423bc}</MetaDataID>
        [OOAdvantech.MetaDataRepository.CachingDataOnClientSide]
        string FullName { get; set; }
        /// <MetaDataID>{a43d4066-ecf3-4a87-bf30-0c30c8a8e074}</MetaDataID>
         [OOAdvantech.MetaDataRepository.CachingDataOnClientSide]
        string UserName { get; set; }
        /// <MetaDataID>{10cf6d90-36b8-421b-9c35-350a2c5f0a95}</MetaDataID>
        string Email { get; set; }
        /// <MetaDataID>{0654d96c-d898-448a-bf3c-d414fe291b30}</MetaDataID>
        string Password { get; set; }
        /// <MetaDataID>{bb20cd5b-e7b6-48a2-9856-77aa454b8989}</MetaDataID>
        string ConfirmPassword { get; set; }
    }

    /// <MetaDataID>{e6b6ed2b-c1a0-4c53-9d50-4136afc3faff}</MetaDataID>
    [OOAdvantech.MetaDataRepository.HttpVisible]
    public interface ILocalization
    {
        /// <MetaDataID>{6c551519-9f15-4b0b-9608-d957db625e95}</MetaDataID>
        string Language { get; }

        string DefaultLanguage { get; }

        /// <MetaDataID>{45d4c218-adff-467d-8136-4ad2f83a2697}</MetaDataID>
        string GetTranslation(string langCountry);

        /// <MetaDataID>{c7d216d1-8970-4fcb-8d2f-ae71ae5ba947}</MetaDataID>
        string GetString(string langCountry, string key);

        /// <MetaDataID>{0699aa09-153a-4251-842c-e493e6a777a5}</MetaDataID>
        void SetString(string langCountry, string key, string newValue);

        /// <MetaDataID>{e1b0bcde-7694-4b3d-a5ba-e8f8d83aa55a}</MetaDataID>
        string AppIdentity { get; }

    }

}
