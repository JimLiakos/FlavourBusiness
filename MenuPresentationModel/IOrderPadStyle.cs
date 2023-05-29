using OOAdvantech.MetaDataRepository;

namespace MenuPresentationModel.MenuStyles
{
    /// <MetaDataID>{13ebcb47-5b4b-427b-85f7-8122296822f3}</MetaDataID>
    public interface IOrderPadStyle : IStyleRule
    {
        [Association("OrderPadBackground", Roles.RoleA, "1bc5fe45-ea6b-407e-a09a-b8593028ddb3")]
        IPageImage Background { get; set; }

        /// <MetaDataID>{b592de46-a68d-4c71-89cb-361999dbd5ba}</MetaDataID>
        UIBaseEx.Margin BackgroundMargin { get; set; }

        /// <MetaDataID>{d7464a7a-c4fe-457a-96e9-81c505914d49}</MetaDataID>
        ImageStretch BackgroundStretch { get; set; }
    }
}