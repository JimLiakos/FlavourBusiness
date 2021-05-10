using OOAdvantech.MetaDataRepository;

namespace MenuPresentationModel
{
    /// <MetaDataID>{c2758212-571b-47ad-96d4-3cd7628f166e}</MetaDataID>
    public interface IStyleSheet
    {
        [Association("StyleSheetStyles", Roles.RoleA, "0d9cbce1-a323-4c91-a53a-5b0fc2a8218e")]
        System.Collections.Generic.IList<IStyle> Styles { get;  }

        /// <MetaDataID>{dcc871f2-0d04-48d1-ae75-9df2a3dde22e}</MetaDataID>
        string Name { get; set; }


        void AddStyle(IStyle style);

        /// <MetaDataID>{dd939515-2955-4e8a-ab4a-6545e6ed87c6}</MetaDataID>
        void RemoveStyle(IStyle style);
    }
}