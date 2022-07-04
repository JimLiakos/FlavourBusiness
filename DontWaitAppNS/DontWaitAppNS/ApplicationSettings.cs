using System;
using OOAdvantech;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Transactions;
using System.Linq;
using FlavourBusinessManager.RoomService;

namespace DontWaitApp
{
    /// <MetaDataID>{d2560ed7-6c28-4e1d-8f49-98a4ef93784a}</MetaDataID>
    [BackwardCompatibilityID("{d2560ed7-6c28-4e1d-8f49-98a4ef93784a}")]
    [Persistent()]
    public class ApplicationSettings
    {

        /// <exclude>Excluded</exclude>
        string _WaiterObjectRef;

        /// <MetaDataID>{d6490bd8-ec25-4cd1-add6-420dd6e1ce3b}</MetaDataID>
        [PersistentMember(nameof(_WaiterObjectRef))]
        [BackwardCompatibilityID("+15")]
        public string WaiterObjectRef
        {
            get => _WaiterObjectRef;
            set
            {

                if (_WaiterObjectRef != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _WaiterObjectRef = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        MenuData _LastServicePoinMenuData;

        /// <MetaDataID>{c0bab971-0230-4294-b1c0-4e4123e4c4f2}</MetaDataID>
        [PersistentMember(nameof(_LastServicePoinMenuData))]
        [BackwardCompatibilityID("+9")]
        public MenuData LastServicePoinMenuData
        {
            get
            {
                return _LastServicePoinMenuData;
            }
            set
            {
                if (_LastServicePoinMenuData != value)
                {
                    if (_LastServicePoinMenuData != null)
                    {
                        //OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this)
                        foreach (ItemPreparation itemPreparation in _LastServicePoinMenuData.OrderItems)
                        {
                            if (value.OrderItems!=null&&!value.OrderItems.Contains(itemPreparation))
                                ObjectStorage.DeleteObject(itemPreparation);
                        }

                    }
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _LastServicePoinMenuData = value;

                        var objectStarage = ObjectStorage.GetStorageOfObject(this);
                        foreach (ItemPreparation itemPreparation in _LastServicePoinMenuData.OrderItems)
                            objectStarage.CommitTransientObjectState(itemPreparation);

                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        string _Path;

        /// <MetaDataID>{0c8c81da-2e77-48f6-b927-49fc8802fd2f}</MetaDataID>
        [PersistentMember(nameof(_Path))]
        [BackwardCompatibilityID("+8")]
        public string Path
        {
            get => _Path;
            set
            {

                if (_Path != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Path = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }




        /// <exclude>Excluded</exclude>
        string _LastClientSessionID;

        /// <MetaDataID>{1706785f-72bf-4b66-9307-8f8c310ccc3f}</MetaDataID>
        [PersistentMember(nameof(_LastClientSessionID))]
        [BackwardCompatibilityID("+14")]
        public string LastClientSessionID
        {
            get
            {
                return _LastClientSessionID;
            }
            set
            {
                if (_LastClientSessionID != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _LastClientSessionID = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }



        /// <exclude>Excluded</exclude>
        string _SignInProvider;
        /// <MetaDataID>{dda41587-5296-4cce-85d4-a0c9567fee7b}</MetaDataID>
        [PersistentMember(nameof(_SignInProvider))]
        [BackwardCompatibilityID("+13")]
        public string SignInProvider
        {
            get
            {
                return _SignInProvider;
            }
            set
            {
                if (_LastClientSessionID != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _SignInProvider = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        ObjectStateManagerLink StateManagerLink;


        /// <exclude>Excluded</exclude>
        bool _IsForegroundServiceStarted;

        /// <MetaDataID>{4551532f-2ffb-4b58-b731-7c0703f82e83}</MetaDataID>
        [BackwardCompatibilityID("")]
        public bool IsForegroundServiceStarted
        {
            get => _IsForegroundServiceStarted;
            set
            {
                if (_IsForegroundServiceStarted != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _IsForegroundServiceStarted = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <exclude>Excluded</exclude>
        static ObjectStorage _AppSettingsStorage;
        /// <MetaDataID>{da878c84-7957-48fd-a7ac-fef703351615}</MetaDataID>
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
                    appDataPath += "\\DontWaitWater";
                    if (!string.IsNullOrEmpty(ExtraStoragePath))
                        appDataPath += "\\" + ExtraStoragePath;

                    if (!System.IO.Directory.Exists(appDataPath))
                        System.IO.Directory.CreateDirectory(appDataPath);
                    string storageLocation = appDataPath + "\\DontWaitAppSettings.xml";
#endif

                    try
                    {
                        _AppSettingsStorage = OOAdvantech.PersistenceLayer.ObjectStorage.OpenStorage("DontWaitAppSettings", storageLocation, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                    }
                    catch (StorageException error)
                    {

                        if (error.Reason == StorageException.ExceptionReason.StorageDoesnotExist)
                        {
                            _AppSettingsStorage = OOAdvantech.PersistenceLayer.ObjectStorage.NewStorage("DontWaitAppSettings",
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
        /// <MetaDataID>{82643791-08c8-4d09-a3db-c5e8de5e5061}</MetaDataID>
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
                           // _Current.FriendlyName = "FriendlyName 1";
                            AppSettingsStorage.CommitTransientObjectState(_Current);
                        }
                        else
                        {
                            //_Current.FriendlyName = "FriendlyName 2";
                        }
                        stateTransition.Consistent = true;
                    }
                }
                return _Current;
            }
        }


        /// <exclude>Excluded</exclude>
        string _SignInUserName;
        /// <MetaDataID>{1aa6eeca-93e3-44f9-9f42-de763b037039}</MetaDataID>
        [PersistentMember(nameof(_SignInUserName))]
        [BackwardCompatibilityID("+10")]
        public string SignInUserName
        {
            get
            {
                return _SignInUserName;
            }
            set
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _SignInUserName = value;
                    stateTransition.Consistent = true;
                }
            }
        }
        /// <exclude>Excluded</exclude>
        string _SignInUserIdentity;
        /// <MetaDataID>{4a51c48b-5fec-4309-9fbe-6b418094b292}</MetaDataID>
        [PersistentMember(nameof(_SignInUserIdentity))]
        [BackwardCompatibilityID("+11")]
        public string SignInUserIdentity
        {
            get
            {
                return _SignInUserIdentity;
            }
            set
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _SignInUserIdentity = value;
                    stateTransition.Consistent = true;
                }
            }
        }







        /// <exclude>Excluded</exclude>
        string _FriendlyName;

        /// <MetaDataID>{d527686e-187a-4e4f-9128-7cc0d2275d63}</MetaDataID>
        [PersistentMember(nameof(_FriendlyName))]
        [BackwardCompatibilityID("+12")]
        public string FriendlyName
        {
            get
            {
                //return _FriendlyName;
                return null;
            }
            set
            {

                //if (_FriendlyName != value)
                //{
                //    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                //    {
                //        _FriendlyName = value;
                //        stateTransition.Consistent = true;
                //    }
                //}
            }
        }

        /// <MetaDataID>{a20dfd91-81e2-4fab-9ddd-293b5a60b460}</MetaDataID>
        public static string ExtraStoragePath { get; internal set; }
    }
}