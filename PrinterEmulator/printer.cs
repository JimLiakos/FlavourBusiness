using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrinterEmulator
{
    public class Printer
    {


        //00011011
        //00010011

        //0001000 //Off-line

        public string Description => Port.ToString();

        public int Port;

        public byte[] Status=new byte[4] { 0b00010011, 0,0,0 };


        public bool online
        {
            get
            {
                return (Status[0] & (byte)PrinterStatus.Offline)==0;
            }
            set
            {
                if(value)
                {
                    //online
                    byte Offline = (byte)PrinterStatus.Offline;
                    Status[0] &= (byte)~Offline;
                }
                else
                {
                    //offline
                    Status[0] |= (byte)PrinterStatus.Offline;
                }
            }
        }




    }
}

public enum PrinterStatus
{
    Offline = 0b0001000
}
