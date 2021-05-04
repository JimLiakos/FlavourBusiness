namespace FlavourBusinessFacade.ComputingResources
{
    /// <MetaDataID>{6902582e-0c8f-4e43-84fb-f607cc42b483}</MetaDataID>
    [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("{6902582e-0c8f-4e43-84fb-f607cc42b483}")]
    public interface IIsolatedComputingContext
    {
        string Description { get; set; }
        string ContextID { get; set; }


        /// <MetaDataID>{94a89caf-7c22-44ab-9bd6-5afd46af5132}</MetaDataID>
        int ComputingResourceID { get; set; }
    }
}

