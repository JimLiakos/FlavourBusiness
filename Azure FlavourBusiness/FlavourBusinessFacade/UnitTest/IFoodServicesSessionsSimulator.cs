using FlavourBusinessFacade.RoomService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlavourBusinessFacade.UnitTest
{
    public interface IFoodServicesSessionsSimulator
    {
        void StartClientSideSimulation();

        event NewSimulateSessionHandler NewSimulateSession;
    } 

    public delegate void NewSimulateSessionHandler(List<IItemPreparation> sessionItems);


    
}
