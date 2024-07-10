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
        /// <MetaDataID>{7c1fcbdf-41ea-4504-9774-bcf5bc782b2d}</MetaDataID>
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


        /// <MetaDataID>{116b8aab-99b2-4657-b361-83d5328eece1}</MetaDataID>
        List<FlavourBusinessFacade.Print.Printing> GetPendingPrintings(System.Collections.Generic.List<string> printingsOnLocalSpooler, string deviceUpdateEtag);


    }

    /// <MetaDataID>{72632049-59b3-4ba9-bcd9-43a5ec30d518}</MetaDataID>
    public class DeviceAssignKeyData
    {
        /// <MetaDataID>{f306cbeb-66bb-44ac-b21f-ff29975823a1}</MetaDataID>
        public string PrintManagerDeviceAssignFullKey;
        /// <MetaDataID>{789f909d-fed4-490f-af44-c5dc613f41d9}</MetaDataID>
        public string PrintManagerDeviceAssignShortKey;
    }



    /// <MetaDataID>{4c55c3c2-761d-4fdc-8c9e-b9d5f3011daa}</MetaDataID>
    public class Printer
    {
        /// <MetaDataID>{8f34f5c3-402e-4441-a70c-fd63d4d83771}</MetaDataID>
        public string Description { get; set; }
        /// <MetaDataID>{e4fc6cdc-7e03-4807-a8ab-92476170660f}</MetaDataID>
        public bool IsESCPOSPrinter { get; set; }
        /// <MetaDataID>{6777430d-6609-46de-8860-f559eb3d79e1}</MetaDataID>
        public string Address { get; set; }

        /// <MetaDataID>{65dad5ec-1615-4d3e-ae21-b5652e6eb53a}</MetaDataID>
        public string Identity { get; set; }

        /// <MetaDataID>{77ffa4bf-e37f-4e87-b438-003bc709b244}</MetaDataID>
        public PrinterStatus? Status { get; set; }

        public enum PrinterStatus
        {
            Online,
            OffLine

        }
    }
}
