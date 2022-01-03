using FinanceFacade;
using FlavourBusinessFacade.RoomService;
using System.Collections.Generic;

namespace FlavourBusinessFacade.ServicesContextResources
{

    public delegate void OpenTransactionsHandle(ICashiersStationRuntime sender, string deviceUpdateEtag);
    public delegate void CashierStationDeviceValidatingHandle(ICashiersStationRuntime cashiersStationRuntime,string token);
    /// <MetaDataID>{194e978c-62f2-4388-bf41-f2f9fe59cad4}</MetaDataID>
    public interface ICashiersStationRuntime
    {

        event OpenTransactionsHandle OpenTransactions;

        //event CashierStationDeviceValidatingHandle CashierStationDeviceValidating;

        /// <MetaDataID>{a7218012-da49-4b19-ac73-a5f9d7911cfe}</MetaDataID>
        List<FinanceFacade.ITransaction> GetOpenTransactions(string deviceUpdateEtag);
        void TransactionCommited(ITransaction transaction);

        // void Validate(string token);

    }



 
}