using FlavourBusinessFacade.RoomService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlavourBusinessFacade
{
    /// <MetaDataID>{8c0caa74-fb56-48fa-8392-29e628056186}</MetaDataID>
    public class ItemPreparationAbbreviation
    {

        public string uid;

        public DateTime StateTimestamp;

    }

    /// <MetaDataID>{8c10d93a-c0cd-4e9a-8a13-4441eb1e546f}</MetaDataID>
    public class ItemPreparationStateAbbreviation
    {
        /// <MetaDataID>{9dd91357-e5b1-4432-b1bf-d0ab3c942c58}</MetaDataID>
        public string uid;
        /// <MetaDataID>{2115d8ae-a6b1-4cc6-8264-d9a8925252d8}</MetaDataID>
        public DateTime StateTimeStamp;
        /// <MetaDataID>{790dabb6-743c-48eb-affb-0b415a34b414}</MetaDataID>
        public bool InEditState;
        /// <MetaDataID>{bb31f8d7-0c6d-4a26-ae87-f2cd50714aca}</MetaDataID>
        public ItemPreparationState State;
    }

    /// <MetaDataID>{e9560d4a-6ddf-4200-a06c-9a2ee2f6798d}</MetaDataID>
    public class SessionItemPreparationAbbreviation
    {
        public string SessionID;

        public string uid;

        public DateTime StateTimestamp;

        public bool IsShared;

        public double? Quantity;


    }
}
