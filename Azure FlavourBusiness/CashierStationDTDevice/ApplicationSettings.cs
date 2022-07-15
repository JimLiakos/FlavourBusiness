using FlavourBusinessFacade.ServicesContextResources;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Transactions;
using System;
using System.Linq;
namespace CashierStationDevice
{
    /// <MetaDataID>{62e67aab-3926-4c64-8211-d2de6beaef00}</MetaDataID>
    [BackwardCompatibilityID("{62e67aab-3926-4c64-8211-d2de6beaef00}")]
    [Persistent()]
    public class ApplicationSettings
    {
        /// <exclude>Excluded</exclude>
        string _DocumentSignerOutputFolder;

        /// <MetaDataID>{3ada20d5-62e4-453d-8613-581ae62bdf66}</MetaDataID>
        [OOAdvantech.MetaDataRepository.PersistentMember(nameof(_DocumentSignerOutputFolder))]
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+4")]
        public string DocumentSignerOutputFolder
        {
            get => _DocumentSignerOutputFolder;
            set
            {

                if (_DocumentSignerOutputFolder != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _DocumentSignerOutputFolder = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }
    

        /// <exclude>Excluded</exclude>
        string _DocumentSignerType;

        /// <MetaDataID>{6a383767-4d19-4f63-9518-35065aa02f56}</MetaDataID>
        [PersistentMember(nameof(_DocumentSignerType))]
        [BackwardCompatibilityID("+2")]
        public string DocumentSignerType
        {
            get => _DocumentSignerType;
            set
            {

                if (_DocumentSignerType != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _DocumentSignerType = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        string _DocumentSignerDeviceIPAddress;

        /// <MetaDataID>{82718c75-cb1d-4409-a2ce-4b848d5221fa}</MetaDataID>
        [PersistentMember(nameof(_DocumentSignerDeviceIPAddress))]
        [BackwardCompatibilityID("+3")]
        public string DocumentSignerDeviceIPAddress
        {
            get => _DocumentSignerDeviceIPAddress;
            set
            {
                if (_DocumentSignerDeviceIPAddress != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _DocumentSignerDeviceIPAddress = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <MetaDataID>{d033859f-8c04-47cd-8f7a-69b6af5ceab0}</MetaDataID>
        public ICashiersStationRuntime CashiersStation;

        public event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;
        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<Model.TransactionPrinter> _TransactionsPrinters = new OOAdvantech.Collections.Generic.Set<Model.TransactionPrinter>();


        /// <MetaDataID>{f428c1f7-594f-4899-be0d-5c297d0a1237}</MetaDataID>
        [Association("CashierStationPrinters", Roles.RoleA, "1c778140-1ab8-42d2-a1bd-5055514aabca")]
        [RoleAMultiplicityRange(1)]
        [PersistentMember(nameof(_TransactionsPrinters))]
        public System.Collections.Generic.List<Model.TransactionPrinter> TransactionsPrinters => _TransactionsPrinters.ToThreadSafeList();


        /// <MetaDataID>{2a7fda1f-840d-4f30-b889-14c73618352e}</MetaDataID>
        public void AddTransactionPrinter(Model.TransactionPrinter transactionPrinter)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _TransactionsPrinters.Add(transactionPrinter);
                stateTransition.Consistent = true;
            }
        }

        /// <MetaDataID>{aefefb04-6e80-48e4-810e-a4359cb0361f}</MetaDataID>
        public void RemoveTransactionPrinter(Model.TransactionPrinter transactionPrinter)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _TransactionsPrinters.Remove(transactionPrinter);
                stateTransition.Consistent = true;
            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <MetaDataID>{eb4339d5-8185-46f1-8875-ecf5a37524e9}</MetaDataID>
        public static string ExtraStoragePath { get; set; }


        /// <exclude>Excluded</exclude>
        static ApplicationSettings _Current;

        /// <MetaDataID>{dbe7df69-804a-465c-988f-398cb27aee1f}</MetaDataID>
        public static ApplicationSettings Current
        {
            get
            {
                
                if (_Current == null)
                {

                    using (SystemStateTransition suppressStateTransition = new SystemStateTransition(TransactionOption.Suppress))
                    {
                        OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(AppSettingsStorage);
                        _Current = (from appSetting in storage.GetObjectCollection<ApplicationSettings>() select appSetting).FirstOrDefault();
                        using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                        {
                            if (_Current == null)
                            {
                                _Current = new ApplicationSettings();
                                AppSettingsStorage.CommitTransientObjectState(_Current);
                            }
                            if (_Current.TransactionsPrinters.Where(x => x.IsDefault).FirstOrDefault() == null)
                            {
                                Model.TransactionPrinter transactionPrinter = new Model.TransactionPrinter();
                                AppSettingsStorage.CommitTransientObjectState(transactionPrinter);
                                transactionPrinter.Description = "Default";
                                transactionPrinter.IsDefault = true;
                                _Current.AddTransactionPrinter(transactionPrinter);

                            }
                            stateTransition.Consistent = true;
                        }
                    }
                }
                return _Current;
            }
        }

        /// <MetaDataID>{67eea655-69f7-47cc-bb8e-c531f1f8ff4f}</MetaDataID>
        static public string AppDataPath
        {
            get
            {
                string appDataPath = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Microneme";
                if (!System.IO.Directory.Exists(appDataPath))
                    System.IO.Directory.CreateDirectory(appDataPath);
                appDataPath += "\\CashiersStation";
                if (!string.IsNullOrEmpty(ExtraStoragePath))
                    appDataPath += "\\" + ExtraStoragePath;



                if (!System.IO.Directory.Exists(appDataPath))
                    System.IO.Directory.CreateDirectory(appDataPath);
                return appDataPath;
            }
        }
        /// <exclude>Excluded</exclude>
        static ObjectStorage _AppSettingsStorage;


        /// <MetaDataID>{8b048297-d686-438d-a405-1919a7ea025d}</MetaDataID>
        public static ObjectStorage AppSettingsStorage
        {
            set
            {

            }
            get
            {
                if (_AppSettingsStorage == null)
                {
#if DeviceDotNet
                    string storageLocation = @"\DontWaitAppSettings.xml";
#else

                    string storageLocation = AppDataPath + "\\CashiersStation.xml";
#endif

                    try
                    {
                        _AppSettingsStorage = OOAdvantech.PersistenceLayer.ObjectStorage.OpenStorage("CashiersStation", storageLocation, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                    }
                    catch (StorageException error)
                    {

                        if (error.Reason == StorageException.ExceptionReason.StorageDoesnotExist)
                        {
                            _AppSettingsStorage = OOAdvantech.PersistenceLayer.ObjectStorage.NewStorage("CashiersStation",
                                                                    storageLocation,
                                                                    "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                        }
                        else
                            throw error;
                    }
                    catch (Exception error)
                    {
                    }
                }
                return _AppSettingsStorage;
            }
        }

        /// <MetaDataID>{1fe845a2-2aae-4f58-99cc-304d37e9311e}</MetaDataID>
        public CompanyHeader CompanyHeader;

        /// <exclude>Excluded</exclude>
        string _CommunicationCredentialKey;

        /// <MetaDataID>{706f0d10-8c30-473a-b1a1-958081b7637b}</MetaDataID>
        [PersistentMember(nameof(_CommunicationCredentialKey))]
        [BackwardCompatibilityID("+1")]
        public string CommunicationCredentialKey
        {
            get => _CommunicationCredentialKey;
            set
            {
                if (_CommunicationCredentialKey != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _CommunicationCredentialKey = value;
                        stateTransition.Consistent = true;
                    }
                    ObjectChangeState?.Invoke(this, nameof(CommunicationCredentialKey));

                }
            }
        }

    }
}