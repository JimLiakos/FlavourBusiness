namespace MenuPresentationModel
{
    /// <summary>
    /// Defines the the types of content which can host a menu page
    /// </summary>
    /// <MetaDataID>{af3f7b9c-62f5-45df-9c7f-25b025000626}</MetaDataID>
    public static class PageContentType
    {
        /// <summary>
        /// Menu heading can be a title of food items category or a title of  food items group or menu labaling
        /// </summary>
        /// <MetaDataID>{17c6a88c-e210-40da-b878-95a63e28d5da}</MetaDataID>
        public const string MenuHeading = "21E107E7-4A4E-4DB1-A201-76CBC24FA730";


        /// <summary>
        /// Food item content contains alls info to describe the food item.
        /// </summary>
        /// <MetaDataID>{b5a65e4a-7fc8-4f2e-9c5f-e83b146fc7e0}</MetaDataID>
        public const string FoodItem = "FEEEE045-E487-4F75-894E-BD12F08A1807";

        /// <summary>
        /// Food items group content contains a collection of food items with common document format.
        /// </summary>
        /// <MetaDataID>{6c3becf6-0413-4718-9174-47b72796748b}</MetaDataID>
        public const string FoodItemsGroup = "09462658-B646-4F78-89D9-BFF18D726035";


        /// <summary>
        /// Items group content contains a collection general items.
        /// </summary>
        /// <MetaDataID>{53a7851d-1aa4-4941-9bc5-d182c8a44f23}</MetaDataID>
        public const string PresentationItemsGroup = "A966057E-D68C-41CD-9327-866437FBA845";



    }
}