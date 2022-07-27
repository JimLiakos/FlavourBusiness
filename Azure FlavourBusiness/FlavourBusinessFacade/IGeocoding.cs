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
        void SavePlace(IPlace place);
        void RemovePlace(IPlace place);

        void SetDefaultPlace(IPlace deliveryPlace);

        List<IPlace> Places { get; }

        Task<Coordinate?> GetCurrentLocation();

     

    }
}
