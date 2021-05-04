namespace FlavourBusinessFacade.ServicesContextResources
{
    /// <MetaDataID>{bb238676-1e53-4347-aaae-ee1aa8be3f81}</MetaDataID>
    public interface IHallLayout
    {


        string HallLayoutUri { get; }


        /// <MetaDataID>{db896785-7576-4611-a6af-69453d3dfb46}</MetaDataID>
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+4")]
        string ServicesContextIdentity { get; set; }


        string FontsLink { get; set; }

        /// <MetaDataID>{06978fca-9b6c-48ae-842b-f34e8af46781}</MetaDataID>
        /// <summary>
        /// Gets or sets the width of the shape. 
        /// </summary>
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+3")]
        double Width { get; set; }


        /// <MetaDataID>{7d5b1210-530c-4512-bee1-18bf569a58cc}</MetaDataID>
        /// <summary>
        /// Gets or sets the height of the shape. 
        /// </summary>
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+2")]
        double Height { get; set; }

        /// <MetaDataID>{c5d32dd2-8d74-4e73-b0bc-4a4f0f8d7334}</MetaDataID>
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+1")]
        string Name { get; set; }

     
    }
}