using ESD_DTool.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashierStationDevice
{
    /// <MetaDataID>{6a723107-e61c-4464-9a63-0fbc71c78020}</MetaDataID>
    public class RBSDocSigner:IDocumentSignDevice
    {
        ESDPROT RBSESD = new ESDPROT();

        public string UnlockKey { get; private set; }

        public bool IsOnline => throw new NotImplementedException();

        Devices CurDEV = new Devices();

        public event EventHandler<EventArgs> DeviceStatusChanged;

        public  void Connect(string ethernetIP)
        {
            string unlockKey = RBSESD.ReadUnlockKey();
            CurDEV.GGPSKey = "";
            CurDEV.EthernetIP = ethernetIP;
            CurDEV.ProxyIP = "";
            CurDEV.SerialNO = "***********";
            CurDEV.ComNo = 1;// Convert.ToByte(SerialPort.Text.Substring(3, 1));
            CurDEV.ActivationCode = unlockKey;

            

        }

        public SignatureData SignDocument(string document)
        {
            throw new NotImplementedException();
        }

        public string PrepareEpsilonLine(EpsilonLineData epsilonLineData)
        {
            throw new NotImplementedException();
        }

        public List<string> CheckStatusForError()
        {
            throw new NotImplementedException();
        }
    }
}
