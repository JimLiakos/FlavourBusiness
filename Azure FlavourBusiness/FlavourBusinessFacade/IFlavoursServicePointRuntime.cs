namespace FlavourBusinessFacade
{
    /// <MetaDataID>{ab91ca77-88ab-4ee4-835b-c651d21efde9}</MetaDataID>
    public interface IFlavoursServicePointRuntime
    {
        /// <MetaDataID>{2e3ac379-09ee-4a82-be2f-721452356c5b}</MetaDataID>
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+1")]
        string Description { get; set; }

        void AssignGraphicMenu(OrganizationStorageRef graphicMenuStorageRef);

        /// <MetaDataID>{4e027323-bb1f-4b08-bfbb-cf810db20c32}</MetaDataID>
        void RemoveGraphicMenu(OrganizationStorageRef graphicMenuStorageRef);


    }
}