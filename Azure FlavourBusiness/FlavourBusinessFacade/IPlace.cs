namespace FlavourBusinessFacade.EndUsers
{
    /// <MetaDataID>{8de7583e-64c8-4cec-a817-3f03a96fd677}</MetaDataID>
    public interface IPlace
    {
        /// <MetaDataID>{695c4de1-97fc-4c26-8db5-7037b80e9b17}</MetaDataID>
        string PlaceID { get; set; }

        /// <MetaDataID>{0ca1bd1e-7dc8-49d8-ab73-c8e8dfaf77b8}</MetaDataID>
        Coordinate Location { get; set; }

        /// <MetaDataID>{e329489b-6d39-431d-ba24-44d2aea135f0}</MetaDataID>
        string StateProvinceRegion { get; set; }


        /// <MetaDataID>{e40b58b3-dd3d-4265-ab14-9b358c2902e8}</MetaDataID>
        string CityTown { get; set; }

        /// <MetaDataID>{5b5edd76-9c82-44f5-b51f-cb89f113ed8b}</MetaDataID>
        string Country { get; set; }

        /// <MetaDataID>{6817d072-0d68-4eb4-866b-59b07aa5cdad}</MetaDataID>
        string PostalCode { get; set; }

        /// <MetaDataID>{beedde2f-85b4-4965-8455-c123ecb768c3}</MetaDataID>
        string Area { get; set; }

        /// <MetaDataID>{df2c909b-ee2e-4f40-9c8b-9b99dc4eec06}</MetaDataID>
        string Street { get; set; }

        /// <MetaDataID>{3513645e-7564-4bd4-a951-ab05dbcd587d}</MetaDataID>
        string StreetNumber { get; set; }

        /// <MetaDataID>{8268d4c6-9154-4fa5-a4c1-7fe537350d9c}</MetaDataID>
        string Description { get; set; }


        /// <MetaDataID>{a4cd01ce-e822-4255-b43b-cc14bd7a344e}</MetaDataID>
        bool Default { get; set; }
        double RouteDistanceInMeters { get; set; }
        double RouteDurationInMinutes { get; set; }
        Coordinate RouteOrigin { get; set; }


        /// <MetaDataID>{a2975c8d-e456-41aa-b753-dd1432594866}</MetaDataID>
        string GetExtensionProperty(string name);
        /// <MetaDataID>{b2086dcb-57ec-46cd-8301-43a18b6c1567}</MetaDataID>
        void SetExtensionProperty(string name, string value);

        /// <MetaDataID>{32ca9a22-417a-423e-93d3-8c2d87c76a9b}</MetaDataID>
        void RemovetExtensionProperty(string name);
    }


}