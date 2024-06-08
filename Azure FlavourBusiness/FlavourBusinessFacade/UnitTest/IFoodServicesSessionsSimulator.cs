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
        /// <MetaDataID>{f7364bd9-194f-4d82-b90f-949f84b63207}</MetaDataID>
        void StartClientSideSimulation(SessionType sessionType);

        event NewSimulateSessionHandler NewSimulateSession;
    }

    public delegate void NewSimulateSessionHandler(List<IItemPreparation> sessionItems);


    
}
