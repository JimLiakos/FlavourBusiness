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

                    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
                    {
                        OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(AppSettingsStorage);
                        _Current = (from appSetting in storage.GetObjectCollection<ApplicationSettings>() select appSetting).FirstOrDefault();
                        if (_Current == null)
                        {
                            _Current = new ApplicationSettings();
                            AppSettingsStorage.CommitTransientObjectState(_Current);
                        }
                        stateTransition.Consistent = true;
                    }
                }
                return _Current;
            }
        }
        

        /// <exclude>Excluded</exclude>
        static ObjectStorage _AppSettingsStorage;


        /// <MetaDataID>{8b048297-d686-438d-a405-1919a7ea025d}</MetaDataID>
        public static OOAdvantech.PersistenceLayer.ObjectStorage AppSettingsStorage
        {
            get
            {
                if (_AppSettingsStorage == null)
                {
#if DeviceDotNet
                    string storageLocation = @"\DontWaitAppSettings.xml";
#else
                    string appDataPath = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Microneme";
                    if (!System.IO.Directory.Exists(appDataPath))
                        System.IO.Directory.CreateDirectory(appDataPath);
                    appDataPath += "\\CashiersStation";
                    if (!string.IsNullOrEmpty(ExtraStoragePath))
                        appDataPath += "\\" + ExtraStoragePath;

                    if (!System.IO.Directory.Exists(appDataPath))
                        System.IO.Directory.CreateDirectory(appDataPath);
                    string storageLocation = appDataPath + "\\CashiersStation.xml";
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
                }
            }
        }

    }
}