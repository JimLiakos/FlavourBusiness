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

    public class ItemPreparationStateAbbreviation
    {
        public string uid;
        public DateTime StateTimeStamp;
        public bool InEditState;
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
