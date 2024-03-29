using System;
using System.Linq;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Transactions;

namespace TakeAwayApp
{

    /// <MetaDataID>{D58FFE37-B0EF-4A84-B482-35D984D2B43E}</MetaDataID>

    [BackwardCompatibilityID("{D58FFE37-B0EF-4A84-B482-35D984D2B43E}")]
    [Persistent()]
    public class ApplicationSettings : DontWaitApp.ApplicationSettings
    {


        /// <exclude>Excluded</exclude>
        string _CommunicationCredentialKey;


        /// <MetaDataID>{21b6eea8-9f5e-4c87-a0cc-76e293ac20d9}</MetaDataID>

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



        /// <exclude>Excluded</exclude>
        string _TakeAwayStationTitle;


        /// <MetaDataID>{4b583826-fc80-4c00-acc3-7e5cdd0f2c36}</MetaDataID>
        [PersistentMember(nameof(_TakeAwayStationTitle))]
        [BackwardCompatibilityID("+2")]
        public string TakeAwayStationTitle
        {
            get => _TakeAwayStationTitle;

            set
            {
                if (_TakeAwayStationTitle != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _TakeAwayStationTitle = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }





        /// <MetaDataID>{ba4a2ec6-cafc-4042-8a38-cbec16bd17e5}</MetaDataID>
        public static string ExtraStoragePath { get; internal set; }


        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <exclude>Excluded</exclude>
        static ObjectStorage _AppSettingsStorage;


        /// <MetaDataID>{f7626e2a-7093-42fa-bdb7-2f5aef189a1e}</MetaDataID>
        public static ObjectStorage AppSettingsStorage
        {
            get
            {
                if (_AppSettingsStorage == null)
                {
#if DeviceDotNet
                    string storageLocation = @"\PreparationStationAppSettings.xml";
#else
                    string appDataPath = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Microneme";
                    if (!System.IO.Directory.Exists(appDataPath))
                        System.IO.Directory.CreateDirectory(appDataPath);
                    appDataPath += "\\DontWaitTakeAway";
                    if (!string.IsNullOrEmpty(ExtraStoragePath))
                        appDataPath += "\\" + ExtraStoragePath;

                    if (!System.IO.Directory.Exists(appDataPath))
                        System.IO.Directory.CreateDirectory(appDataPath);
                    string storageLocation = appDataPath + "\\PreparationStationAppSettings.xml";
#endif

                    try
                    {
                        _AppSettingsStorage = OOAdvantech.PersistenceLayer.ObjectStorage.OpenStorage("PreparationStationAppSettings", storageLocation, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                    }
                    catch (StorageException error)
                    {

                        if (error.Reason == StorageException.ExceptionReason.StorageDoesnotExist)
                        {
                            _AppSettingsStorage = OOAdvantech.PersistenceLayer.ObjectStorage.NewStorage("PreparationStationAppSettings",
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





        /// <MetaDataID>{cf33574b-2964-47d3-9d1e-d9f5d778fa8d}</MetaDataID>
        public new static ApplicationSettings Current
        {
            get
            {
                if (!(_Current is ApplicationSettings))
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
                return _Current as ApplicationSettings;
            }
        }

    }
}