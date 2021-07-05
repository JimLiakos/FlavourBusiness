using System;
using System.Linq;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Transactions;

namespace PreparationStationDevice
{
    /// <MetaDataID>{bc38e2f6-8ead-4a8a-8a19-0e5e54123264}</MetaDataID>
    [BackwardCompatibilityID("{cb26b77f-6f40-4d8e-a583-c0661f82ba96}")]
    [Persistent()]
    public class ApplicationSettings
    {
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

        

        /// <exclude>Excluded</exclude>
        string _PreparationStationTitle;

        /// <MetaDataID>{d33915e8-efd6-4a5e-9140-430c65e49b65}</MetaDataID>
        [PersistentMember(nameof(_PreparationStationTitle))]
        [BackwardCompatibilityID("+2")]
        public string PreparationStationTitle
        {
            get => _PreparationStationTitle; 
            
            set
            {
                if (_PreparationStationTitle != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _PreparationStationTitle = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <MetaDataID>{f7bdf5cc-1cb2-45ef-9a97-3e7a0db20410}</MetaDataID>
        public static string ExtraStoragePath { get; internal set; }


        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <exclude>Excluded</exclude>
        static ObjectStorage _AppSettingsStorage;

        /// <MetaDataID>{34c4d2b6-a10e-4b77-b298-1d424d5e8f2e}</MetaDataID>
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
                    appDataPath += "\\DontWaitWater";
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



        /// <exclude>Excluded</exclude>
        static ApplicationSettings _Current;

        /// <MetaDataID>{73b8024c-cf56-4592-8a79-4e83fc9cab65}</MetaDataID>
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

    }
}