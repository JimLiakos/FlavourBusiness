using FlavourBusinessFacade.ServicesContextResources;
using OOAdvantech;
using OOAdvantech.MetaDataRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlavourBusinessFacade.Printing
{
    public delegate void DocumentPendingToPrintHandled(IPrintManager sender, string deviceUpdateEtag);

    /// <MetaDataID>{9A07EDC5-FAF4-48D5-9A3E-1C116B762F3C}</MetaDataID>
    [BackwardCompatibilityID("{9A07EDC5-FAF4-48D5-9A3E-1C116B762F3C}")]
    [GenerateFacadeProxy]
    public interface IPrintManager
    {
        /// <exclude>Excluded</exclude>
        event DocumentPendingToPrintHandled DocumentPendingToPrint;

        /// <MetaDataID>{acdb2b1f-e612-415f-b4ec-ee834e4f9983}</MetaDataID>
        void DocumentPrinted(string documentIdentity);


        /// <MetaDataID>{8f932ae7-22c3-4469-90c4-a3bf76ef0d10}</MetaDataID>
        DeviceAssignKeyData GetPrintManagerNewCredentialKey();
        string AssignDevice(string credentialKey);
        /// <MetaDataID>{29738060-4554-4962-8849-4ddff58a7f0c}</MetaDataID>
        void UpdatePrinterStatus(Printer printer, Printer.PrinterStatus online);


        /// <MetaDataID>{692ab65a-c0b1-4247-8e1e-df5a574c911d}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        [HttpVisible]
        List<Printer> Printers { get; }


        [HttpVisible]
        event ObjectChangeStateHandle ObjectChangeState;

        /// <MetaDataID>{a401819e-3949-4dfd-b5f4-916f2b1c6728}</MetaDataID>
        [HttpVisible]
        bool LocalPrintingServiceIsRunning { get; }


        List<FlavourBusinessFacade.Print.Printing> GetPendingPrintings(System.Collections.Generic.List<string> printingsOnLocalSpooler, string deviceUpdateEtag);


    }

    /// <MetaDataID>{72632049-59b3-4ba9-bcd9-43a5ec30d518}</MetaDataID>
    public class DeviceAssignKeyData
    {
        public string PrintManagerDeviceAssignFullKey;
        public string PrintManagerDeviceAssignShortKey;
    }



    /// <MetaDataID>{4c55c3c2-761d-4fdc-8c9e-b9d5f3011daa}</MetaDataID>
    public class Printer
    {
        public string Description { get; set; }
        public bool IsESCPOSPrinter { get; set; }
        public string Address { get; set; }

        public string Identity { get; set; }

        public PrinterStatus? Status { get; set; }

        public enum PrinterStatus
        {
            Online,
            OffLine

        }
    }
}
