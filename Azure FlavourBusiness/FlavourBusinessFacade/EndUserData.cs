
using FlavourBusinessFacade.EndUsers;
using System.Collections.Generic;

namespace FlavourBusinessFacade
{
    /// <MetaDataID>{582829b9-924b-4b2a-98db-473a8d02059d}</MetaDataID>
    public class EndUserData
    {
        /// <MetaDataID>{40cc8ecc-5e5d-4a96-ab8e-72bb2272247f}</MetaDataID>
        public string Name { get; set; }

        /// <MetaDataID>{0f90aefc-6e41-439f-a991-a5099191f8ae}</MetaDataID>
        public string Email { get; set; }

        /// <MetaDataID>{d9c629b1-c67d-482f-be5e-5e058b019aa1}</MetaDataID>
        public string Identity { get; set; }

        /// <MetaDataID>{260c4cc0-8487-40cb-8ab6-7f5de83aeaaf}</MetaDataID>
        public EndUsers.SIMCardData SIMCard { get; set; }
        public List<IPlace> DeliveryPlaces { get; set; }
        public string FriendlyName { get; set; }
    }
}