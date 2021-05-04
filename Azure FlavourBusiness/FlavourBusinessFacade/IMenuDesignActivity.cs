namespace FlavourBusinessFacade.HumanResources
{
    /// <MetaDataID>{b23eaa01-9ca4-4f4c-93ab-76a1e54564c9}</MetaDataID>
    public interface IMenuDesignActivity : IActivity
    {
        /// <MetaDataID>{8b028c61-f789-4e6c-9e98-7d87644729d6}</MetaDataID>
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+2")]
        string DesigneSubjectIdentity { get; set; }

        /// <MetaDataID>{ce6c05e1-3fec-4139-820f-d0ae2324c007}</MetaDataID>
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+1")]
        DesignSubjectType DesignActivityType { get; set; }
    }



    /// <MetaDataID>{5dcfaf69-1161-46b0-89df-75517752eb6a}</MetaDataID>
    public enum DesignSubjectType
    {
        Stylesheet = 1,
        Menu = 2

    }
}