using OOAdvantech;
using OOAdvantech.MetaDataRepository;

namespace MenuPresentationModel.MenuStyles
{
    /// <MetaDataID>{40411631-07a8-4734-a0f3-3c5cf9ad5530}</MetaDataID>
    public interface IStyleRule
    {
        [Association("StyleSheetStyles", Roles.RoleB, "0d9cbce1-a323-4c91-a53a-5b0fc2a8218e")]
        [RoleBMultiplicityRange(1, 1)]
        IStyleSheet StyleSheet { get; }

        /// <MetaDataID>{021b8bc9-98b6-4f1b-b10f-98f6a9c28dfa}</MetaDataID>
        string Name { get; set; }
        /// <MetaDataID>{6f883663-8c57-4ff2-899f-a44e6824f2f0}</MetaDataID>
        IStyleRule GetDerivedStyle();
        /// <MetaDataID>{f2b2a2cd-dc22-47f9-97f7-192ea6030a67}</MetaDataID>
        void ChangeOrgStyle(IStyleRule style);

        event ObjectChangeStateHandle ObjectChangeState;
    }
}