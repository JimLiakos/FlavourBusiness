using OOAdvantech.MetaDataRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlavourBusinessFacade.HumanResources
{

    /// <MetaDataID>{9D668B50-B0D6-40E1-A910-FDFB37380135}</MetaDataID>
    [BackwardCompatibilityID("{9D668B50-B0D6-40E1-A910-FDFB37380135}")]
    public interface ITranslationActivity : IActivity
    {
        /// <MetaDataID>{3486590f-53ad-4a25-81db-473dbb14b8cc}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        [CachingDataOnClientSide]
        TranslationType TranslationType { get; set; }



        /// <MetaDataID>{7950ea43-9dad-44bf-af6a-749a9670a17f}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        [CachingDataOnClientSide]
        string TranslationIdentity { get; set; }
    }

    /// <MetaDataID>{279feca7-596a-4f26-99af-4d6d301a4b96}</MetaDataID>
    public enum TranslationType
    {
        Application = 1,
        Menu = 2

    }
}
