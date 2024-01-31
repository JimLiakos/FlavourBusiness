using UIBaseEx;

namespace MenuPresentationModel.MenuStyles
{
    /// <MetaDataID>{780644c5-e5c3-443a-9a81-1295ee916c7e}</MetaDataID>
    public interface IMenuItemInfoStyle : IStyleRule
    {
        /// <MetaDataID>{bb08f458-22ec-4611-84bc-c0e84ced6815}</MetaDataID>
        FontData HeadingFont { get; set; }

        /// <MetaDataID>{57cb3e2b-a09c-4cc8-b06b-1f57743dec99}</MetaDataID>
        FontData ParagraphFont { get; set; }
    }
}