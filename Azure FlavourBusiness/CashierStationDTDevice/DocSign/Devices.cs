using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESD_DTool.Helper
{
    /// <MetaDataID>{b250e16d-a117-401b-9c1f-0eeaf33b6b45}</MetaDataID>
    public class Devices
    {
        public string SerialNO { get; set; }
        public long TotalBytes { get; set; }
        public bool IsEthernet { get; set; }
        public bool IsProxy { get; set; }
        public string ActivationCode { get; set; }

        public string GGPSKey { get; set; }
        public string EthernetIP { get; set; }
        public string ProxyIP { get; set; }
        public byte ComNo { get; set; }
        public bool IsActive { get; set; }
        public string LastConnectedClient { get; set; }
        public bool InProgress { get; set; }
        public String BackupData { get; set; }
        public String TempSerno { get; set; }
        public long TotalSigned { get; set; }
        public bool AfterSigndoc { get; set; }
        /*
        public bool IsEthernet
        {
            get
            {
                return _IsEthernet;
            }
            set
            {
                _IsEthernet = value;
            }
        }

        private byte _ComNo;
        public byte ComNo
        {
            
            get
            {
                return _ComNo;
            }
            set
            {
                _ComNo = value;
            }
        }

      
        public string EthernetIP
        {
            get
            {
                return _EthernetIP;
            }
            set
            {
                _EthernetIP = value;
            }
        }

        
        public string IsActive
        {
            get
            {
                return _IsActive;
            }
            set
            {
                _IsActive = value;
            }
        }
        */

    }
}
