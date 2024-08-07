using System;
using OOAdvantech;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Transactions;
using System.Linq;
using FlavourBusinessManager.RoomService;
using System.Threading.Tasks;
using Xamarin.Forms;
using FlavourBusinessFacade.HumanResources;
using System.Collections.Generic;

namespace DontWaitApp
{
    /// <MetaDataID>{d2560ed7-6c28-4e1d-8f49-98a4ef93784a}</MetaDataID>
    [BackwardCompatibilityID("{d2560ed7-6c28-4e1d-8f49-98a4ef93784a}")]
    [Persistent()]
    public class ApplicationSettings
    {
        /// <MetaDataID>{e700e295-1444-4fdf-bdaa-6bcd201ff59e}</MetaDataID>
        public void RemoveClientSession(FoodServicesClientSessionViewModel clientSession)
        {
            if (clientSession != null)
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    if (clientSession == DisplayedFoodServicesClientSession)
                        DisplayedFoodServicesClientSession = null;


                    _ActiveSessions.Remove(clientSession);
                    stateTransition.Consistent = true;
                }
            }

        }

        /// <MetaDataID>{5ed898fa-9013-4fd3-b897-c7bcb0dba827}</MetaDataID>
        public void AddClientSession(FoodServicesClientSessionViewModel clientSession)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _ActiveSessions.Add(clientSession);
                stateTransition.Consistent = true;
            }

        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<FoodServicesClientSessionViewModel> _ActiveSessions = new OOAdvantech.Collections.Generic.Set<FoodServicesClientSessionViewModel>();

        [RoleAMultiplicityRange(0)]
        [Association("AppFoodServicesClientSession", Roles.RoleA, "263140f9-632e-43dc-a923-9e37a5d9a348")]
        [RoleBMultiplicityRange(1, 1)]
        [PersistentMember(nameof(_ActiveSessions))]
        [AssociationEndBehavior(PersistencyFlag.ReferentialIntegrity | PersistencyFlag.CascadeDelete)]
        public System.Collections.Generic.List<FoodServicesClientSessionViewModel> ActiveSessions => _ActiveSessions.ToThreadSafeList();


        /// <exclude>Excluded</exclude>
        FlavourBusinessManager.EndUsers.FoodServiceClient _ClientAsGuest;

        [Association("AppClientAsGuest", Roles.RoleA, "96589fb5-74e3-446b-a76c-5aa0742f5f32")]
        [PersistentMember(nameof(_ClientAsGuest))]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        public FlavourBusinessManager.EndUsers.FoodServiceClient ClientAsGuest
        {
            get => _ClientAsGuest;
            set
            {

                if (_ClientAsGuest != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ClientAsGuest = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

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
        string _ServiceContextDevice;
        /// <MetaDataID>{119c7726-37c7-4763-acbc-f347fd01fd0b}</MetaDataID>
        [PersistentMember(nameof(_ServiceContextDevice))]
        [BackwardCompatibilityID("+16")]
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

        /// <MetaDataID>{ab7fda42-2d66-4593-81f5-1f0e3a95382d}</MetaDataID>
        public FoodServicesClientSessionViewModel DisplayedFoodServicesClientSession
        {
            get
            {
                if (string.IsNullOrWhiteSpace(LastClientSessionID))
                    return null;
                return ActiveSessions.Where(x => x.ClientSessionID == LastClientSessionID).FirstOrDefault();

            }
            set
            {
                if (value == null)
                    LastClientSessionID = "";
                else
                    LastClientSessionID = value.ClientSessionID;

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
                if (_LastServicePoinMenuData.ClientSessionID == null)
                {

                    return _LastServicePoinMenuData;
                }
                return _LastServicePoinMenuData;
            }
            set
            {
                if (_LastServicePoinMenuData != value)
                {
                    if (value.ClientSessionID == null)
                    {

                    }
                    if (_LastServicePoinMenuData != null)
                    {
                        //OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this)
                        foreach (ItemPreparation itemPreparation in _LastServicePoinMenuData.OrderItems)
                        {
                            if (value.OrderItems != null && !value.OrderItems.Contains(itemPreparation))
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
        private string LastClientSessionID
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



        ///// <exclude>Excluded</exclude>
        //string _SignInProvider;
        ///// <MetaDataID>{dda41587-5296-4cce-85d4-a0c9567fee7b}</MetaDataID>
        //[PersistentMember(nameof(_SignInProvider))]
        //[BackwardCompatibilityID("+13")]
        //public string SignInProvider
        //{
        //    get
        //    {
        //        return _SignInProvider;
        //    }
        //    set
        //    {
        //        if (_LastClientSessionID != value)
        //        {
        //            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
        //            {
        //                _SignInProvider = value;
        //                stateTransition.Consistent = true;
        //            }
        //        }
        //    }
        //}

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

        /// <MetaDataID>{568752b1-76f6-4a8f-8a81-273e5c24243f}</MetaDataID>
        static object AppSettingsStorageLock = new object();

        /// <exclude>Excluded</exclude>
        static ObjectStorage _AppSettingsStorage;
        /// <MetaDataID>{da878c84-7957-48fd-a7ac-fef703351615}</MetaDataID>
        public static ObjectStorage AppSettingsStorage
        {
            get
            {

                lock (AppSettingsStorageLock)
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
        }



        /// <exclude>Excluded</exclude>
        protected static ApplicationSettings _Current;

        /// <MetaDataID>{392dfc5c-a5b1-46d6-a5a1-4f456e14e755}</MetaDataID>
        public static ApplicationSettings Current
        {
            get
            {
                lock (AppSettingsStorageLock)
                {
                    if (_Current != null)
                        return _Current;
                }
                GetCurrent().Wait();
                return _Current;
            }
        }
        /// <MetaDataID>{82643791-08c8-4d09-a3db-c5e8de5e5061}</MetaDataID>
        public static Task<ApplicationSettings> GetCurrent()
        {
            lock (AppSettingsStorageLock)
            {
                if (_Current != null)
                    return Task<ApplicationSettings>.FromResult(_Current);

                if (AppSettingsTask != null)
                    return AppSettingsTask;
                AppSettingsTask = Task<ApplicationSettings>.Run(() =>
            {
                if (_Current == null)
                {

                    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
                    {

                        try
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


                        }
                        catch (Exception error)
                        {

                            throw;
                        }
                        stateTransition.Consistent = true;
                    }
                }
                return _Current;
            });
                return AppSettingsTask;
            }
        }

        /// <MetaDataID>{ee5bee48-b903-46fd-b6b8-1b9fa541e998}</MetaDataID>
        Dictionary<string, double> HallsLayoutsScales = new Dictionary<string, double>();

        /// <MetaDataID>{90ca154a-efe4-41bc-83fc-60b104ec0c1c}</MetaDataID>
        public void SetHallLayoutScale(string hallLayoutUri, bool rotated, double scale)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                string hallKey = $"{hallLayoutUri};{rotated}";
                HallsLayoutsScales[hallKey] = scale; 
                stateTransition.Consistent = true;
            }
        }

        public double? GetHallLayoutScale(string hallLayoutUri, bool rotated )
        {
            //return null;
            double scale =0;
            string hallKey = $"{hallLayoutUri};{rotated}";
            if (HallsLayoutsScales.TryGetValue(hallKey, out scale))
                return scale;
            else
                return null;
        }


        /// <MetaDataID>{e56acb3b-c09e-4e57-b928-74b2649c7b87}</MetaDataID>
        [PersistentMember]
        [BackwardCompatibilityID("+20")]
        string HallsLayoutsScalesJson;


        /// <MetaDataID>{f7d26ffa-ef57-446e-8de6-aed8cb8a76c9}</MetaDataID>
        [ObjectActivationCall]
        public void ObjectActivation()
        {
 
            if (!string.IsNullOrWhiteSpace(HallsLayoutsScalesJson))
                HallsLayoutsScales = OOAdvantech.Json.JsonConvert.DeserializeObject<Dictionary<string, double>>(HallsLayoutsScalesJson);

        }


        /// <MetaDataID>{2924ab14-e8d1-46d1-b500-b010c3ce7f1a}</MetaDataID>
        [BeforeCommitObjectStateInStorageCall]
        public void BeforeCommitObjectState()
        {
            HallsLayoutsScalesJson = OOAdvantech.Json.JsonConvert.SerializeObject(HallsLayoutsScales);
        }


        ///// <exclude>Excluded</exclude>
        //string _SignInUserName;
        ///// <MetaDataID>{1aa6eeca-93e3-44f9-9f42-de763b037039}</MetaDataID>
        //[PersistentMember(nameof(_SignInUserName))]
        //[BackwardCompatibilityID("+10")]
        //public string SignInUserName
        //{
        //    get
        //    {
        //        return _SignInUserName;
        //    }
        //    set
        //    {
        //        using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
        //        {
        //            _SignInUserName = value;
        //            stateTransition.Consistent = true;
        //        }
        //    }
        //}
        ///// <exclude>Excluded</exclude>
        //string _SignInUserIdentity;
        ///// <MetaDataID>{4a51c48b-5fec-4309-9fbe-6b418094b292}</MetaDataID>
        //[PersistentMember(nameof(_SignInUserIdentity))]
        //[BackwardCompatibilityID("+11")]
        //public string SignInUserIdentity
        //{
        //    get
        //    {
        //        return _SignInUserIdentity;
        //    }
        //    set
        //    {

        //        using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
        //        {
        //            _SignInUserIdentity = value;
        //            stateTransition.Consistent = true;
        //        }
        //    }
        //}







        /// <exclude>Excluded</exclude>
        string _FriendlyName;
        /// <MetaDataID>{94684440-5506-4756-a0b2-130a0146ad1b}</MetaDataID>
        private static Task<ApplicationSettings> AppSettingsTask;

        /// <MetaDataID>{d527686e-187a-4e4f-9128-7cc0d2275d63}</MetaDataID>
        [PersistentMember(nameof(_FriendlyName))]
        [BackwardCompatibilityID("+12")]
        string FriendlyName
        {
            get
            {
                bool tt = false;
                if (tt)
                    return null;
                else
                    return _FriendlyName;

            }
            set
            {
                if (_FriendlyName != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _FriendlyName = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <MetaDataID>{a20dfd91-81e2-4fab-9ddd-293b5a60b460}</MetaDataID>
        public static string ExtraStoragePath { get; internal set; }


        /// <exclude>Excluded</exclude>
        bool _UserDenyCameraPermission;

        /// <MetaDataID>{5a4d97ab-3430-4126-b475-521692144605}</MetaDataID>
        [PersistentMember(nameof(_UserDenyCameraPermission))]
        [BackwardCompatibilityID("+17")]
        public bool UserDenyCameraPermission
        {
            get => _UserDenyCameraPermission;
            internal set
            {
                if (_UserDenyCameraPermission != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _UserDenyCameraPermission = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        DateTime? _LastTimeWhereUserDenyRemoteNotifications;

        /// <MetaDataID>{854c1451-f130-4c81-8a68-9668266207bb}</MetaDataID>
        [PersistentMember(nameof(_LastTimeWhereUserDenyRemoteNotifications))]
        [BackwardCompatibilityID("+19")]
        public DateTime? LastTimeWhereUserDenyRemoteNotifications
        {
            get => _LastTimeWhereUserDenyRemoteNotifications;
            set
            {
                if (_LastTimeWhereUserDenyRemoteNotifications != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _LastTimeWhereUserDenyRemoteNotifications = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        bool _UserDenyRemoteNotificationsPermission;

        /// <MetaDataID>{e5de0a0d-065b-4e95-90db-8e57d4befbc2}</MetaDataID>
        [PersistentMember(nameof(_UserDenyRemoteNotificationsPermission))]
        [BackwardCompatibilityID("+18")]
        public bool UserDenyRemoteNotificationsPermission
        {
            get => _UserDenyRemoteNotificationsPermission;
            internal set
            {
                if (_UserDenyRemoteNotificationsPermission != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _UserDenyRemoteNotificationsPermission = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
    }
}