namespace ComputationalResources
{


    /// <MetaDataID>{6902582e-0c8f-4e43-84fb-f607cc42b483}</MetaDataID>
    [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("{6902582e-0c8f-4e43-84fb-f607cc42b483}")]
    public interface IIsolatedComputingContext
    {
        /// <MetaDataID>{de9e7bff-b140-4d8d-844e-fcad5124f152}</MetaDataID>
        string Description { get; set; }
        /// <MetaDataID>{f53a329a-5422-46bd-bb73-c4979cf00410}</MetaDataID>
        string ContextID { get; set; }


        /// <MetaDataID>{94a89caf-7c22-44ab-9bd6-5afd46af5132}</MetaDataID>
        int ComputingResourceID { get; set; }

        /// <MetaDataID>{9b31c84a-129c-4ab1-a4ed-b132c403ed14}</MetaDataID>
        bool FixedComputingResource { get; set; }

        ResourceAllocationState ResourceAllocationState { get; set; }
    }



    /// <MetaDataID>{b30c18c6-1b24-4ff4-b0bd-38b0a83ee406}</MetaDataID>
    public enum ResourceAllocationState
    {
        OperationState,
        ReAssignState
    }
}

