using FlavourBusinessFacade.EndUsers;
using OOAdvantech.MetaDataRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlavourBusinessFacade
{
    /// <MetaDataID>{5ea0757f-8254-441d-ac27-f62d3c7c2e7e}</MetaDataID>
    [HttpVisible]
    public interface IGeocodingPlaces
    {
        /// <MetaDataID>{a449bda0-5917-4d01-bbc5-1c4cbd31e907}</MetaDataID>
        void SavePlace(IPlace place);
        /// <MetaDataID>{9e90c664-6a43-4a5f-b929-352fe754dfa1}</MetaDataID>
        void RemovePlace(IPlace place);

        /// <MetaDataID>{93bd8e9e-b459-45e2-8844-78dd08225645}</MetaDataID>
        void SetDefaultPlace(IPlace deliveryPlace);

        /// <MetaDataID>{0e59cdf5-4bec-4455-a67f-fe2adaed9ed8}</MetaDataID>
        List<IPlace> Places { get; }

        /// <MetaDataID>{1092e70e-b750-426b-8918-a6190f6c2424}</MetaDataID>
        Task<Coordinate?> GetCurrentLocation();

    }
}
