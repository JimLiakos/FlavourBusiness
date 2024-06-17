using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using System;

namespace FlavourBusinessFacade.Print
{
    /// <MetaDataID>{79056cc0-f965-412f-85a5-b470518ff3bf}</MetaDataID>
    [BackwardCompatibilityID("{79056cc0-f965-412f-85a5-b470518ff3bf}")]
    [Persistent()]
    public class Printing
    {


        /// <exclude>Excluded</exclude>
        DateTime _DateTime;

        /// <MetaDataID>{afb03366-56ff-4891-85e6-a2fbd21444fd}</MetaDataID>
        /// <summary>
        /// Defines the date time where printing created
        /// </summary>
        [PersistentMember(nameof(_DateTime))]
        [BackwardCompatibilityID("+4")]
        public DateTime DateTime
        {
            get => _DateTime;
            set
            {
                if (_DateTime != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _DateTime = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        byte[] _RawData;

        /// <MetaDataID>{7feab22e-c370-44f5-979c-52dba64047b2}</MetaDataID>
        /// <summary>
        /// Defines printing as raw data used for esc/pos printers
        /// </summary>
        public byte[] RawData
        {
            get => _RawData;
            set
            {
                if (_RawData != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _RawData = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <exclude>Excluded</exclude>
        PrinterType _PrinterType;

        /// <summary>
        /// Defines the printer type esc pos or graphic
        /// </summary>
        // <MetaDataID>{268c807b-875f-49d2-9154-cf0f853dc9b4}</MetaDataID>
        [PersistentMember(nameof(_PrinterType))]
        [BackwardCompatibilityID("+2")]
        public PrinterType PrinterType
        {
            get => _PrinterType; set
            {
                if (_PrinterType != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _PrinterType = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }



        /// <exclude>Excluded</exclude>
        string _Printer;

        /// <summary>
        /// Actual is as string where the local system ca identify the printer 
        /// For instance "ipaddress,port" for ESC/POS printer
        /// </summary>
        /// <MetaDataID>{c6c57a1b-60c8-4d32-8124-ccbc2b55bc5e}</MetaDataID>
        [PersistentMember(nameof(_Printer))]
        [BackwardCompatibilityID("+1")]
        public string Printer
        {
            get => _Printer;
            set
            {
                if (_Printer != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Printer = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }




        /// <exclude>Excluded</exclude>
        string _ID;

        /// <summary>
        /// Defines the Identity of printing
        /// </summary>
        /// <MetaDataID>{02e08b25-5b96-485c-bda8-e822d8947554}</MetaDataID>
        [PersistentMember(nameof(_ID))]
        [BackwardCompatibilityID("+5")]
        public string ID
        {
            get => _ID;
            set
            {
                if (_ID != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ID = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }



        /// <exclude>Excluded</exclude>
        DateTime _ValidUntil;


        /// <summary>
        /// Collect as garbage after this time
        /// </summary>
        /// <MetaDataID>{0ccce1db-634a-4157-955f-fcb93971a37f}</MetaDataID>
        [PersistentMember(nameof(_ValidUntil))]
        [BackwardCompatibilityID("+6")]
        public DateTime ValidUntil
        {
            get => _ValidUntil; set
            {
                if (_ValidUntil != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ValidUntil = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        public string PrinterID { get; set; }
    }

    /// <MetaDataID>{4abd478a-f782-4063-b0a9-62ca03328c2c}</MetaDataID>
    public enum PrinterType
    {
        Graphic,
        ESCPOS
    }
}