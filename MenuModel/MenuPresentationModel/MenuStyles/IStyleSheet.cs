using OOAdvantech.MetaDataRepository;

namespace MenuPresentationModel.MenuStyles
{
    /// <MetaDataID>{c2758212-571b-47ad-96d4-3cd7628f166e}</MetaDataID>
    public interface IStyleSheet
    {
        [Association("StyleSheetStyles", Roles.RoleA, "0d9cbce1-a323-4c91-a53a-5b0fc2a8218e")]
        System.Collections.Generic.IDictionary<string,IStyleRule> Styles { get;  }

        /// <MetaDataID>{dcc871f2-0d04-48d1-ae75-9df2a3dde22e}</MetaDataID>
        string Name { get; set; }


        /// <MetaDataID>{8f5589d2-e5e5-4d04-a7ad-aea3f34e1b4b}</MetaDataID>
        void AddStyle(IStyleRule style);

        /// <MetaDataID>{dd939515-2955-4e8a-ab4a-6545e6ed87c6}</MetaDataID>
        void RemoveStyle(IStyleRule style);

        /// <MetaDataID>{2669f024-43dc-40a2-8c94-f7ed37ad2277}</MetaDataID>
        string StyleSheetIdentity { get; set; }
    }
}