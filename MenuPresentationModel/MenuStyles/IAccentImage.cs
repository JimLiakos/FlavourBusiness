namespace MenuPresentationModel.MenuStyles
{
    /// <MetaDataID>{f9edc63e-0a0f-4fdb-b940-68fce5987a25}</MetaDataID>
    public interface IImage
    {
        /// <MetaDataID>{64720c90-c67d-48bc-bffe-7474edb3662f}</MetaDataID>
        Resource Image { get; set; }

        /// <MetaDataID>{2472808f-f6f4-4823-9726-57501c9b2cca}</MetaDataID>
        double Height { get; set; }

        /// <MetaDataID>{06d1ed8d-5374-4fff-a9f8-935c78166a26}</MetaDataID>
        double Width { get; set; }

        /// <MetaDataID>{3b59aff7-9e6a-46d4-ab82-6b616c60bccd}</MetaDataID>
        string Uri { get; }


    }
}