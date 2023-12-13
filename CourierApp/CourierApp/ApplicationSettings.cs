using OOAdvantech.MetaDataRepository;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierApp
{
    /// <MetaDataID>{ca4de730-f591-438c-86eb-e1054f058dc0}</MetaDataID>
    [BackwardCompatibilityID("{ca4de730-f591-438c-86eb-e1054f058dc0}")]
    [Persistent()]
    public class ApplicationSettings
    {
        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <exclude>Excluded</exclude>
        static ObjectStorage _AppSettingsStorage;

        /// <MetaDataID>{8692cff4-9a37-4489-83ce-840956b72bc6}</MetaDataID>
        public static string ExtraStoragePath { get; internal set; }

        /// <MetaDataID>{f7626e2a-7093-42fa-bdb7-2f5aef189a1e}</MetaDataID>
        public static OOAdvantech.PersistenceLayer.ObjectStorage AppSettingsStorage
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
                    string storageLocation = appDataPath + "\\CourierAppSettings.xml";
#endif

                    try
                    {
                        _AppSettingsStorage = OOAdvantech.PersistenceLayer.ObjectStorage.OpenStorage("CourierAppSettings", storageLocation, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                    }
                    catch (StorageException error)
                    {

                        if (error.Reason == StorageException.ExceptionReason.StorageDoesnotExist)
                        {
                            _AppSettingsStorage = OOAdvantech.PersistenceLayer.ObjectStorage.NewStorage("CourierAppSettings",
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
        protected static ApplicationSettings _Current;

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

        /// <exclude>Excluded</exclude>
        string _CourierObjectRef;

        /// <MetaDataID>{8c68f200-c9e3-4a46-8c5a-051a06dcd5b0}</MetaDataID>
        [PersistentMember(nameof(_CourierObjectRef))]
        [BackwardCompatibilityID("+1")]
        public string CourierObjectRef
        {
            get => _CourierObjectRef;
            set
            {

                if (_CourierObjectRef != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _CourierObjectRef = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }


        /// <exclude>Excluded</exclude>
        string _ServiceContextDevice;
        /// <MetaDataID>{119c7726-37c7-4763-acbc-f347fd01fd0b}</MetaDataID>
        [PersistentMember(nameof(_ServiceContextDevice))]
        [BackwardCompatibilityID("+2")]
        public string ServiceContextDevice
        {
            get => _ServiceContextDevice;
            set
            {
                if (_ServiceContextDevice != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ServiceContextDevice = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        public string HomeDeliveryServicePointIdentity { get; internal set; }
    }
}

