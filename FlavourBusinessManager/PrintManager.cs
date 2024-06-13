using FlavourBusinessFacade;
using FlavourBusinessFacade.Printing;
using FlavourBusinessFacade.ServicesContextResources;
using FlavourBusinessManager.ServicesContextResources;
using OOAdvantech;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlavourBusinessManager.Printing
{
    /// <MetaDataID>{84baa848-645b-436f-98ae-296868ee1b72}</MetaDataID>
    [BackwardCompatibilityID("{84baa848-645b-436f-98ae-296868ee1b72}")]
    [Persistent()]
    public class PrintManager : OOAdvantech.Remoting.ExtMarshalByRefObject, IPrintManager
    {
        /// <MetaDataID>{66782715-3b03-40ff-9441-009dc836d4e0}</MetaDataID>
        private string PrintManagerNewCredentialKey;

        /// <exclude>Excluded</exclude>
        string _PrintManagerCredentialKey;

        /// <MetaDataID>{9528e28d-e9b5-4eeb-920e-825f6ede312d}</MetaDataID>
        [PersistentMember(nameof(_PrintManagerCredentialKey))]
        [BackwardCompatibilityID("+7")]
        public string PrintManagerCredentialKey
        {
            get => _PrintManagerCredentialKey;
            set
            {
                if (_PrintManagerCredentialKey != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _PrintManagerCredentialKey = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        System.Collections.Generic.List<Printer> _Printers = new System.Collections.Generic.List<Printer>();

        public System.Collections.Generic.List<Printer> Printers
        {
            get
            {
                System.Collections.Generic.List<string> printerIDs = null;
                lock (_Printers)
                    printerIDs = _Printers.Select(x => x.Identity).ToList();

                var preparationStations = ServicePointRunTime.ServicesContextRunTime.Current.PreparationStations.SelectMany(x => x.SubStations).Union(ServicePointRunTime.ServicesContextRunTime.Current.PreparationStations).Where(x => printerIDs != null && !printerIDs.Contains(x.ShortIdentity)).ToList();

                lock (_Printers)
                {
                    _Printers.AddRange(preparationStations.Where(x => !string.IsNullOrWhiteSpace(x.Printer)).Select(x => new Printer() { Address = x.Printer, Identity = x.ShortIdentity, Description = x.Description }).ToList());
                    return _Printers.ToList();
                }

            }
        }
        public void UpdatePrinterStatus(Printer printer, Printer.PrinterStatus status)
        {
            lock(_Printers)
            {
                var thePrinter=_Printers.Where(x => x.Identity == printer.Identity).FirstOrDefault();
                if(thePrinter.Status!=status)
                {
                    thePrinter.Status = status;
                    ObjectChangeState?.Invoke(this, nameof(Printers));

                }

            }
        }



        public string AssignDevice(string credentialKey)
        {
            if (!string.IsNullOrWhiteSpace(PrintManagerNewCredentialKey) && credentialKey == PrintManagerNewCredentialKey)
            {

                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {
                    GetPrintManagerNewCredentialKey();//destroy credentialKe key because produce new

                    var ticks = new DateTime(2022, 1, 1).Ticks;
                    var uniqueId = (DateTime.Now.Ticks - ticks).ToString("x");

                    PrintManagerCredentialKey = ServicePointRunTime.ServicesContextRunTime.Current.ServicesContextIdentity + ";" + uniqueId;

                    Transaction.RunOnTransactionCompleted( () => {

                        ObjectChangeState?.Invoke(this, nameof(AssignDevice));
                    });

                    stateTransition.Consistent = true;
                }


                return PrintManagerCredentialKey;
            }
            return null;
        }

        /// <MetaDataID>{6d6c08ed-4542-4c48-a148-8618f5d2777e}</MetaDataID>
        public DeviceAssignKeyData GetPrintManagerNewCredentialKey()
        {



            var ticks = new DateTime(2022, 1, 1).Ticks;
            var uniqueId = (DateTime.Now.Ticks - ticks).ToString("x");

            PrintManagerNewCredentialKey = ServicePointRunTime.ServicesContextRunTime.Current.ServicesContextIdentity + ";" + uniqueId;

            string uniqueIdsUrl = string.Format("http://{0}:8090/api/ExpiringUniqueId/{1}/60", FlavourBusinessFacade.ComputingResources.EndPoint.Server, PrintManagerNewCredentialKey);
            string shortIdentity = null;
            using (System.Net.WebClient wc = new System.Net.WebClient())
            {
                var json = wc.DownloadString(uniqueIdsUrl);

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    shortIdentity = OOAdvantech.Json.JsonConvert.DeserializeObject<string>(json);
                    stateTransition.Consistent = true;
                }
            }

            return new DeviceAssignKeyData() { PrintManagerDeviceAssignFullKey = PrintManagerNewCredentialKey, PrintManagerDeviceAssignShortKey = shortIdentity };
        }



        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <MetaDataID>{7b3d0978-6e98-41ff-897b-5a4ebd62f484}</MetaDataID>
        public PrintManager()
        {

        }

        internal void Init()
        {
            var preparationStations = ServicePointRunTime.ServicesContextRunTime.Current.PreparationStations.Union(ServicePointRunTime.ServicesContextRunTime.Current.PreparationStations.SelectMany(x => x.SubStations)).OfType<PreparationStation>().ToList();
            var preparationStationItemsPreparationContextSnapshots = (from snapshot in new OOAdvantech.Linq.Storage(ObjectStorage.GetStorageOfObject(ServicePointRunTime.ServicesContextRunTime.Current)).GetObjectCollection<ItemsPreparationContextSnapshots>()
                                                                      select snapshot).ToList().GroupBy(x => x.PreparationStationIdentity).ToList();



            foreach (var itemsPreparationContextSnapshots in preparationStationItemsPreparationContextSnapshots)
            {

                var preparationStation = preparationStations.Where(x => x.PreparationStationIdentity == itemsPreparationContextSnapshots.Key).FirstOrDefault();
                if (preparationStation != null)
                {
                    preparationStation.PrintManager = new PreparationStationPrintManager(itemsPreparationContextSnapshots.ToList());
                    preparationStation.OnPreparationItemsChangeState();
                    preparationStations.Remove(preparationStation);
                }

            }

            foreach (var preparationStation in preparationStations)
                preparationStation.PrintManager = new PreparationStationPrintManager(preparationStation);




            Task.Run(() =>
            {

                while (true)
                {
                    lock (DeviceUpdateEtagLock)
                    {


                        long numberOfTicks = 0;
                        if (long.TryParse(DeviceUpdateEtag, out numberOfTicks))
                        {

                            DateTime myDate = new DateTime(numberOfTicks);
                            if ((DateTime.Now - myDate).TotalSeconds > 3)
                            {
                                if (RaiseEventTimeStamp == null || (DateTime.UtcNow - RaiseEventTimeStamp.Value).TotalSeconds > 10)
                                {
                                    _PrintingPending?.Invoke(this, DeviceUpdateEtag);
                                    RaiseEventTimeStamp = DateTime.UtcNow;
                                }
                            }
                        }
                    }

                    System.Threading.Thread.Sleep(500);
                }


            });
        }


        /// <exclude>Excluded</exclude>
        public event DocumentPendingToPrintHandled _PrintingPending;


        public event ObjectChangeStateHandle ObjectChangeState;


        /// <summary>
        /// Used from devices to update its state.
        /// The number of subscribers are the number of attached devices
        /// </summary>
        public event DocumentPendingToPrintHandled DocumentPendingToPrint
        {
            add
            {
                var objectChangeState = !(_PrintingPending?.GetInvocationList().Length > 0);
                
                _PrintingPending += value;

                if(objectChangeState)
                    ObjectChangeState?.Invoke(this, nameof(LocalPrintingServiceIsRunning));
            }
            remove
            {
                var objectChangeState = (_PrintingPending?.GetInvocationList().Length == 1);

                _PrintingPending -= value;


                if (objectChangeState&& _PrintingPending==null)
                    ObjectChangeState?.Invoke(this, nameof(LocalPrintingServiceIsRunning));

            }
        }


        /// <MetaDataID>{b1084d11-d945-4ebc-9b8c-27c8f6f231f3}</MetaDataID>
        public void OnNewPrinting()
        {
            Transaction.RunOnTransactionCompleted(() =>
            {

                lock (DeviceUpdateEtagLock)
                {
                    if (string.IsNullOrWhiteSpace(DeviceUpdateEtag))
                        DeviceUpdateEtag = System.DateTime.Now.Ticks.ToString();
                }
            });
        }


        public List<prin GetPendingPrinings(System.Collections.Generic.List<string> printingsOnLocalSpooler, string deviceUpdateEtag)
        {

            ObjectActivated.Task.Wait();

            if (deviceUpdateEtag == DeviceUpdateEtag)
            {
                lock (DeviceUpdateEtagLock)
                {
                    DeviceUpdateEtag = null;
                    RaiseEventTimeStamp = null;
                }
            }
        }


            /// <MetaDataID>{d9a91342-0adb-4c3b-b5ff-84bb02ffce55}</MetaDataID>
            public void DocumentPrinted(string documentIdentity)
        {

        }

        /// <MetaDataID>{56bfae80-7a37-4702-85f7-12dd170d7990}</MetaDataID>
        internal void Print(ItemsPreparationContextSnapshots snapshot)
        {
            PreparationStation preparationStation = GetPreparationStation(snapshot);
            preparationStation.Printer = "";


        }

        /// <MetaDataID>{c382d8ad-6fe4-439c-9f9e-47860db6d641}</MetaDataID>
        private PreparationStation GetPreparationStation(ItemsPreparationContextSnapshots snapshot)
        {
            return ServicePointRunTime.ServicesContextRunTime.Current.PreparationStations.SelectMany(x => x.SubStations).Union(ServicePointRunTime.ServicesContextRunTime.Current.PreparationStations).Where(x => x.PreparationStationIdentity == snapshot.PreparationStationIdentity).FirstOrDefault() as PreparationStation;
        }



        /// <MetaDataID>{c2cf9f82-dce4-470a-b1e3-f326c6ceabdc}</MetaDataID>
        object DeviceUpdateEtagLock = new object();
        /// <exclude>Excluded</exclude>
        string _DeviceUpdateEtag;

        /// <summary>
        /// Device update mechanism operates asynchronously
        /// When the state of preparation station change the change marked as timestamp
        /// The device update mechanism raise event after 3 seconds.
        /// The device catch the event end gets the changes for timestamp (DeviceUpdateEtag) 
        /// the PreparationStationRuntime clear DeviceUpdateEtag 
        /// </summary>
        /// <MetaDataID>{cccb4c57-81e3-4bcd-adb5-608f4ceac8f9}</MetaDataID>

        public string DeviceUpdateEtag
        {
            get
            {
                lock (DeviceUpdateEtagLock)
                    return _DeviceUpdateEtag;
            }
            set
            {
                lock (DeviceUpdateEtagLock)
                {

                    if (value != null && string.IsNullOrWhiteSpace(value))
                    {

                    }

                    if (_DeviceUpdateEtag != value)
                    {
                        using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                        {
                            _DeviceUpdateEtag = value;
                            stateTransition.Consistent = true;
                        }
                    }
                }
            }
        }



        /// <MetaDataID>{15c65eed-3a80-4186-8d1d-7f90f7bd218b}</MetaDataID>
        public DateTime? RaiseEventTimeStamp { get; private set; }

        public bool LocalPrintingServiceIsRunning => _PrintingPending?.GetInvocationList().Length>0;
    }
}