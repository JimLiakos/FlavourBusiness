using FlavourBusinessFacade.EndUsers;
using FlavourBusinessFacade.RoomService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlavourBusinessFacade.UnitTest
{
    /// <MetaDataID>{d723daec-5615-4146-a267-75a3b096092a}</MetaDataID>
    public interface IFoodServicesSessionsSimulator
    {
        void StartClientSideSimulation(SessionType sessionType);

        event NewSimulateSessionHandler NewSimulateSession;
    }

    public delegate void NewSimulateSessionHandler(List<IItemPreparation> sessionItems);


    
}
