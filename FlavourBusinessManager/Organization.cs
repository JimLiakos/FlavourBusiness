using System;
using OOAdvantech.Transactions;
using OOAdvantech.MetaDataRepository;
using FlavourBusinessFacade;
using OOAdvantech.PersistenceLayer;
using System.Linq;
using System.Xml.Linq;
using OOAdvantech.Remoting.RestApi;
using System.Security.Authentication;
using System.Collections.Generic;
using MenuPresentationModel;
using FlavourBusinessToolKit;
using System.Text.RegularExpressions;
using OOAdvantech;
using ComputationalResources;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using FlavourBusinessFacade.HumanResources;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using FlavourBusinessManager.EndUsers;
using System.Resources;
using FlavourBusinessManager.Properties;
using OOAdvantech.Json;

//using FlavourBusinessToolKit;

namespace FlavourBusinessManager
{
    /// <MetaDataID>{3d5670ae-e925-4c84-801f-de7fc948144b}</MetaDataID>
    [BackwardCompatibilityID("{3d5670ae-e925-4c84-801f-de7fc948144b}")]
    [Persistent()]
    public class Organization : MarshalByRefObject, IOrganization, IResourceManager, IUploadService, OOAdvantech.Remoting.IExtMarshalByRefObject
    {

        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<IAccountability> _Commissions = new OOAdvantech.Collections.Generic.Set<IAccountability>();


        /// <MetaDataID>{3d5670ae-e925-4c84-801f-de7fc948144b}</MetaDataID>
        [PersistentMember(nameof(_Commissions))]
        [BackwardCompatibilityID("+12")]
        public List<IAccountability> Commissions => _Commissions.ToThreadSafeList();

        /// <exclude>Excluded</exclude>
        string _PhotoUrl;
        /// <MetaDataID>{5af7d794-ae95-454d-9d59-c9f14e402c23}</MetaDataID>
        [PersistentMember(nameof(_PhotoUrl))]
        [BackwardCompatibilityID("+11")]
        public string PhotoUrl
        {
            get => _PhotoUrl;
            set
            {
                if (_PhotoUrl != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _PhotoUrl = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <MetaDataID>{d875d728-f1be-4601-91f5-3e10f99c8034}</MetaDataID>
        public IFlavoursServicesContext GetFlavoursServicesContext(string servicesContextIdentity)
        {
            return ServicesContexts.Where(x => x.ServicesContextIdentity == servicesContextIdentity).FirstOrDefault();
        }

        /// <exclude>Excluded</exclude>
        System.Collections.Generic.List<FlavourBusinessFacade.UserData.UserRole> _Roles;
        /// <MetaDataID>{2402bb4d-7b6d-4774-ac7c-1d692c6515c4}</MetaDataID>
        public System.Collections.Generic.List<FlavourBusinessFacade.UserData.UserRole> Roles
        {
            get
            {
                if (_Roles == null)
                {
                    FlavourBusinessFacade.UserData.UserRole role = new FlavourBusinessFacade.UserData.UserRole() { User = this, RoleType = FlavourBusinessFacade.UserData.UserRole.GetRoleType(GetType().FullName) };
                    _Roles = new List<FlavourBusinessFacade.UserData.UserRole>() { role };
                }
                return _Roles;
            }
        }

        /// <exclude>Excluded</exclude>
        string _OAuthUserIdentity;
        /// <MetaDataID>{ad08a771-a0d3-48f1-816e-cb08cae9b93c}</MetaDataID>
        [PersistentMember(nameof(_OAuthUserIdentity))]
        [BackwardCompatibilityID("+8")]
        public string OAuthUserIdentity
        {
            get
            {
                return _OAuthUserIdentity;
            }
            set
            {

                if (_OAuthUserIdentity != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _OAuthUserIdentity = value;
                        stateTransition.Consistent = true;
                    }
                }

            }

        }


        /// <exclude>Excluded</exclude>
        string _Email;

        /// <MetaDataID>{668126f6-16fd-47bf-9469-b8277d83ebd8}</MetaDataID>
        [BackwardCompatibilityID("+7")]
        [PersistentMember(nameof(_Email))]
        public string Email
        {
            get
            {
                return _Email;
            }
            set
            {
                if (_Email != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Email = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<FlavourBusinessStorage> _Storages = new OOAdvantech.Collections.Generic.Set<FlavourBusinessStorage>();

        [Association("OrganizationStorages", OOAdvantech.MetaDataRepository.Roles.RoleA, "6e017516-e7a5-4d27-895d-b5438b1d91a9")]
        [PersistentMember(nameof(_Storages))]
        [AssociationEndBehavior(PersistencyFlag.CascadeDelete)]
        public System.Collections.Generic.IList<FlavourBusinessStorage> Storages
        {
            get
            {
                return _Storages.AsReadOnly();
            }
        }


        /// <MetaDataID>{30e89166-13ef-41cd-b3e6-af1e2f893578}</MetaDataID>
        public static IOrganization CurrentOrganization;

        /// <exclude>Excluded</exclude>
        string _Address;
        /// <MetaDataID>{ccf64a66-c19d-4d09-a4b9-c46a0403eb7f}</MetaDataID>
        [PersistentMember(nameof(_Address))]
        [BackwardCompatibilityID("+5")]
        public string Address
        {
            get
            {
                return _Address;
            }
            set
            {
                if (_Address != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Address = value;
                        stateTransition.Consistent = true;
                    }
                }
                ObjectChangeState?.Invoke(this, nameof(Address));
            }
        }





        /// <exclude>Excluded</exclude>
        string _PhoneNumber;
        /// <MetaDataID>{be64fca2-2b0d-46f6-8eab-37a1bd4a88ee}</MetaDataID>
        [PersistentMember(nameof(_PhoneNumber))]
        [BackwardCompatibilityID("+4")]
        public string PhoneNumber
        {
            get
            {
                return _PhoneNumber;
            }
            set
            {
                if (_PhoneNumber != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _PhoneNumber = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <MetaDataID>{9792048c-436e-4331-962b-a6598fa8bd1c}</MetaDataID>
        public Organization(string signUpUserIdentity)
        {

            _OAuthUserIdentity = signUpUserIdentity;
            //_Identity = identity;
            _Identity = Guid.NewGuid().ToString("N");
        }

        /// <MetaDataID>{ed0b9ab9-cfdd-475b-9863-6994dbe2e86e}</MetaDataID>
        public Organization()
        {

        }
        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <exclude>Excluded</exclude>
        string _Name;
        /// <MetaDataID>{acd1d569-95f7-4d01-a7e6-d3c111fd9e71}</MetaDataID>
        [PersistentMember(nameof(_Name))]
        [BackwardCompatibilityID("+1")]
        public string Name
        {
            get
            {
                return _Name;
            }

            set
            {
                if (_Name != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Name = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        string _Trademark;
        /// <MetaDataID>{f77371ed-d9ac-4904-849c-842512a13f77}</MetaDataID>
        [PersistentMember(nameof(_Trademark))]
        [BackwardCompatibilityID("+2")]
        public string Trademark
        {
            get
            {
                return _Trademark;
            }

            set
            {
                if (_Trademark != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Trademark = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        string _Identity;


        /// <exclude>Excluded</exclude>
        [PersistentMember(nameof(_Identity))]
        [BackwardCompatibilityID("+3")]
        public string Identity
        {
            get
            {
                if (_Identity == null)
                    _Identity = OAuthUserIdentity;
                return _Identity;
            }
        }



        /// <MetaDataID>{9b9d4cd4-b224-4f22-aea8-8d541fe5fdc5}</MetaDataID>
        public IUploadSlot GetUploadSlotFor(OrganizationStorageRef storageRef)
        {

            AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;

            if (!UploadPermissionsGranted(authUser, storageRef))
                throw new AuthenticationException();


            var fbStorage = (from storage in this.Storages
                             where storage.StorageIdentity == storageRef.StorageIdentity && storage.FlavourStorageType == storageRef.FlavourStorageType
                             select storage).FirstOrDefault();

            if (fbStorage == null)
            {
                foreach (var serviceContext in this._ServicesContexts)
                {
                    IUploadSlot uploadSlot = (serviceContext.GetRunTime() as IUploadService).GetUploadSlotFor(storageRef);
                    if (uploadSlot != null)
                        return uploadSlot;
                }

                return null;
            }
            else
            {
                string blobUrl = fbStorage.Url;
                var uploadSlot = new UploadSlot(blobUrl, FlavourBusinessManagerApp.CloudBlobStorageAccount, FlavourBusinessManagerApp.RootContainer);
                uploadSlot.FileUploaded += UploadSlot_FileUploaded;
                uploadSlot.Tag = fbStorage;
                return uploadSlot;
            }
        }

        private void UploadSlot_FileUploaded(object sender, EventArgs e)
        {
            UploadSlot uploadSlot = (UploadSlot)sender;
            (sender as UploadSlot).FileUploaded -= UploadSlot_FileUploaded;

            var fbStorage = uploadSlot.Tag as FlavourBusinessStorage;

            if (fbStorage != null && fbStorage.FlavourStorageType == OrganizationStorages.PriceList)
            {
                ObjectStorage.UpdateOperativeObjects(fbStorage.StorageIdentity);

                foreach (var serviceContext in this._ServicesContexts)
                {
                    Task.Run(() =>
                    {
                        serviceContext.GetRunTime()?.ObjectStorageUpdate(fbStorage.StorageIdentity, fbStorage.FlavourStorageType);
                    });
                }
            }

        }


        /// <MetaDataID>{63e2922e-522e-4410-ac66-d0dce99eef1e}</MetaDataID>
        private bool UploadPermissionsGranted(AuthUser authUser, OrganizationStorageRef storageRef)
        {

            foreach (var accountability in Commissions)
            {
                if (authUser != null && accountability.Responsible is HumanResources.MenuMaker && (accountability.Responsible as HumanResources.MenuMaker).OAuthUserIdentity == authUser.User_ID)
                {
                    foreach (var activity in accountability.Activities.OfType<IMenuDesignActivity>().Where(x => x.DesignActivityType == DesignSubjectType.Menu))
                    {
                        var graphicMenu = GraphicMenus.Where(x => x.StorageIdentity == activity.DesigneSubjectIdentity).FirstOrDefault();

                        if (graphicMenu != null && storageRef.StorageIdentity == graphicMenu.StorageIdentity)
                            return true;

                        if (graphicMenu != null && GetStorage(OrganizationStorages.RestaurantMenus).StorageIdentity == storageRef.StorageIdentity)
                            return true;

                    }
                }
            }
            if (authUser != null)
            {
                AuthUserRef authUserRef = AuthUserRef.GetAuthUserRef(authUser, false);
                if (authUserRef.GetContextRoleObject<Organization>() == this)
                    return true;
            }
            return false;
        }

        /// <exclude>Excluded</exclude>
        static OrganizationStorageRef _BackgroundImages;
        /// <MetaDataID>{fddac309-6ec0-430e-89b4-edb0e8139935}</MetaDataID>
        internal static OrganizationStorageRef BackgroundImages
        {
            get
            {
                if (_BackgroundImages == null)
                {
                    string blobUrl = RawStorageCloudBlob.BlobsStorageHttpAbsoluteUri + "graphicmenusresources/BackgroundImages.xml";
                    RawStorageCloudBlob rawStorageCloudBlob = new RawStorageCloudBlob(blobUrl, "BackgroundImages");
                    var objectStorage = ObjectStorage.OpenStorage("BackgroundImages", rawStorageCloudBlob, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");

                    OrganizationStorageRef backgroundImagesStorage = new OrganizationStorageRef
                    {
                        StorageIdentity = objectStorage.StorageMetaData.StorageIdentity,
                        FlavourStorageType = OrganizationStorages.BackgroundImages,
                        Name = objectStorage.StorageMetaData.StorageName,
                        StorageUrl = blobUrl
                    };


                    _BackgroundImages = backgroundImagesStorage;
                }
                return _BackgroundImages;
            }
        }



        /// <exclude>Excluded</exclude>
        static OrganizationStorageRef _Borders;
        /// <MetaDataID>{fddac309-6ec0-430e-89b4-edb0e8139935}</MetaDataID>
        internal static OrganizationStorageRef Borders
        {
            get
            {
                if (_Borders == null)
                {
                    string blobUrl = RawStorageCloudBlob.BlobsStorageHttpAbsoluteUri + "graphicmenusresources/Borders.xml";
                    RawStorageCloudBlob rawStorageCloudBlob = new RawStorageCloudBlob(blobUrl, "Borders");
                    var objectStorage = ObjectStorage.OpenStorage("Borders", rawStorageCloudBlob, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");

                    OrganizationStorageRef bordersStorage = new OrganizationStorageRef
                    {
                        StorageIdentity = objectStorage.StorageMetaData.StorageIdentity,
                        FlavourStorageType = OrganizationStorages.Borders,
                        Name = objectStorage.StorageMetaData.StorageName,
                        StorageUrl = blobUrl
                    };
                    _Borders = bordersStorage;
                }


                return _Borders;
            }
        }



        /// <exclude>Excluded</exclude>
        static OrganizationStorageRef _PageBorders;
        /// <MetaDataID>{31f50e7f-5a66-4dbb-8832-0e985a291c7c}</MetaDataID>
        static OrganizationStorageRef PageBorders
        {
            get
            {
                if (_PageBorders == null)
                {
                    string blobUrl = RawStorageCloudBlob.BlobsStorageHttpAbsoluteUri + "graphicmenusresources/BackgroundImages.xml";
                    RawStorageCloudBlob rawStorageCloudBlob = new RawStorageCloudBlob(blobUrl, "Borders");
                    var objectStorage = ObjectStorage.OpenStorage("Borders", rawStorageCloudBlob, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");

                    OrganizationStorageRef backgroundImagesStorage = new OrganizationStorageRef
                    {
                        StorageIdentity = objectStorage.StorageMetaData.StorageIdentity,
                        FlavourStorageType = OrganizationStorages.Borders,
                        Name = objectStorage.StorageMetaData.StorageName,
                        StorageUrl = blobUrl
                    };


                    _PageBorders = backgroundImagesStorage;
                }
                return _PageBorders;
            }
        }

        /// <exclude>Excluded</exclude>
        static OrganizationStorageRef _HeadingAccents;
        /// <MetaDataID>{a716324b-e921-4418-bb8b-214c890b6395}</MetaDataID>
        internal static OrganizationStorageRef HeadingAccents
        {
            get
            {
                if (_HeadingAccents == null)
                {
                    string blobUrl = RawStorageCloudBlob.BlobsStorageHttpAbsoluteUri + "graphicmenusresources/HeadingAccents.xml";
                    RawStorageCloudBlob rawStorageCloudBlob = new RawStorageCloudBlob(blobUrl, "HeadingAccents");
                    var objectStorage = ObjectStorage.OpenStorage("HeadingAccents", rawStorageCloudBlob, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");

                    OrganizationStorageRef backgroundImagesStorage = new OrganizationStorageRef
                    {
                        StorageIdentity = objectStorage.StorageMetaData.StorageIdentity,
                        FlavourStorageType = OrganizationStorages.HeadingAccents,
                        Name = objectStorage.StorageMetaData.StorageName,
                        StorageUrl = blobUrl
                    };

                    _HeadingAccents = backgroundImagesStorage;
                }
                return _HeadingAccents;
            }
        }



        /// <exclude>Excluded</exclude>
        static OrganizationStorageRef _StyleSheets;
        /// <MetaDataID>{98070975-179c-41b3-82e3-cbdd1f71f2f3}</MetaDataID>
        internal static OrganizationStorageRef StyleSheets
        {
            get
            {
                if (_StyleSheets == null)
                {
                    string blobUrl = RawStorageCloudBlob.BlobsStorageHttpAbsoluteUri + "graphicmenusresources/StyleSheets.xml";
                    RawStorageCloudBlob rawStorageCloudBlob = new RawStorageCloudBlob(blobUrl, "StyleSheets");
                    var objectStorage = ObjectStorage.OpenStorage("StyleSheets", rawStorageCloudBlob, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");

                    MenuPresentationModel.MenuStyles.StyleSheet.ObjectStorage = objectStorage;

                    OrganizationStorageRef backgroundImagesStorage = new OrganizationStorageRef
                    {
                        StorageIdentity = objectStorage.StorageMetaData.StorageIdentity,
                        FlavourStorageType = OrganizationStorages.StyleSheets,
                        Name = objectStorage.StorageMetaData.StorageName,
                        StorageUrl = blobUrl
                    };

                    _StyleSheets = backgroundImagesStorage;
                }
                return _StyleSheets;
            }
        }



        /// <MetaDataID>{c5907f0d-660b-472e-aae0-903f43628cee}</MetaDataID>
        public OrganizationStorageRef GetStorage(OrganizationStorages dataType)
        {
            AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
            if (authUser == null)
                throw new AuthenticationException();

            string userId = authUser.User_ID;
            //AuthUserRef authUserRef = AuthUserRef.GetAuthUserRef(authUser, false);


            bool authorized = false;
            if (authUser != null)
            {
                AuthUserRef authUserRef = AuthUserRef.GetAuthUserRef(authUser, false);
                if (authUserRef.GetContextRoleObject<Organization>() == this)
                    authorized = true;
            }
            if (!authorized)//(authUser.User_ID != this.SignUpUserIdentity && authUserRef.GetContextRoleObject<Organization>() != this)
                throw new InvalidCredentialException("The user " + authUser.Name + " isn't recognized as organization owner.");

            if (dataType == OrganizationStorages.StyleSheets)
                return StyleSheets;

            ObjectID objectID = null;
            string storageIdentity = null;

            var fbstorage = (from storage in this.Storages
                             where storage.FlavourStorageType == dataType
                             select storage).FirstOrDefault();
            System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();
            OOAdvantech.PersistenceLayer.StoragesClient storagesClient = null;
            string serverUrl = OOAdvantech.Remoting.RestApi.RemotingServices.ServerPublicUrl.Substring(0, OOAdvantech.Remoting.RestApi.RemotingServices.ServerPublicUrl.IndexOf("/api"));
            OrganizationStorageRef storageRef = null;
            if (fbstorage == null)
            {

                if (dataType == OrganizationStorages.HeadingAccents)
                {
                    return HeadingAccents;
                }
                else if (dataType == OrganizationStorages.Borders)
                {
                    return Borders;
                }
                else if (dataType == OrganizationStorages.BackgroundImages)
                {
                    return BackgroundImages;
                }
                else if (dataType == OrganizationStorages.OperativeRestaurantMenu)
                {
                    string blobUrl = null;
                    StorageInstanceRef.GetObjectID(this, out objectID, out storageIdentity);
                    blobUrl = "usersfolder/" + Identity + "/Operative/" + dataType.ToString() + ".xml";
                    RawStorageCloudBlob rawStorage = new RawStorageCloudBlob(new XDocument(), blobUrl);
                    ObjectStorage objectStorage = null;
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        objectStorage = ObjectStorage.NewStorage("RestaurantMenuData",
                                       rawStorage,
                                       "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");

                        fbstorage = new FlavourBusinessStorage();
                        fbstorage.StorageIdentity = objectStorage.StorageMetaData.StorageIdentity;
                        fbstorage.Name = objectStorage.StorageMetaData.StorageName;
                        fbstorage.Owner = this;
                        fbstorage.FlavourStorageType = dataType;
                        fbstorage.Url = blobUrl;
                        ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(fbstorage);
                        _Storages.Add(fbstorage);

                        stateTransition.Consistent = true;
                    }
                    var storageUrl = RawStorageCloudBlob.BlobsStorageHttpAbsoluteUri + fbstorage.Url;
                    var lastModified = RawStorageCloudBlob.GetBlobLastModified(fbstorage.Url);

                    storageRef = new OrganizationStorageRef { StorageIdentity = fbstorage.StorageIdentity, FlavourStorageType = fbstorage.FlavourStorageType, Name = fbstorage.Name, Description = fbstorage.Description, StorageUrl = storageUrl, Version = fbstorage.Version, TimeStamp = lastModified.Value.UtcDateTime };

                }
                else
                {
                    string blobUrl = null;
                    StorageInstanceRef.GetObjectID(this, out objectID, out storageIdentity);
                    blobUrl = "usersfolder/" + Identity + "/" + dataType.ToString() + ".xml";
                    RawStorageCloudBlob rawStorage = new RawStorageCloudBlob(new XDocument(), blobUrl);
                    ObjectStorage objectStorage = null;
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        objectStorage = ObjectStorage.NewStorage("RestaurantMenuData",
                                       rawStorage,
                                       "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");

                        fbstorage = new FlavourBusinessStorage();
                        fbstorage.StorageIdentity = objectStorage.StorageMetaData.StorageIdentity;
                        fbstorage.Name = objectStorage.StorageMetaData.StorageName;
                        fbstorage.Owner = this;
                        fbstorage.FlavourStorageType = dataType;
                        fbstorage.Url = blobUrl;
                        ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(fbstorage);
                        _Storages.Add(fbstorage);

                        stateTransition.Consistent = true;
                    }
                    var storageUrl = RawStorageCloudBlob.BlobsStorageHttpAbsoluteUri + fbstorage.Url;
                    var lastModified = RawStorageCloudBlob.GetBlobLastModified(fbstorage.Url);

                    storageRef = new OrganizationStorageRef { StorageIdentity = fbstorage.StorageIdentity, FlavourStorageType = fbstorage.FlavourStorageType, Name = fbstorage.Name, Description = fbstorage.Description, StorageUrl = storageUrl, Version = fbstorage.Version, TimeStamp = lastModified.Value.UtcDateTime };

                    System.Threading.Tasks.Task.Run(async () =>
                   {
                       storagesClient = new StoragesClient(httpClient);
                       storagesClient.BaseUrl = serverUrl;
                       string res = await storagesClient.PostAsync(objectStorage.StorageMetaData, true);
                   });
                }
            }
            else
            {
                var storageUrl = RawStorageCloudBlob.BlobsStorageHttpAbsoluteUri + fbstorage.Url;
                var lastModified = RawStorageCloudBlob.GetBlobLastModified(fbstorage.Url);

                storageRef = new OrganizationStorageRef { StorageIdentity = fbstorage.StorageIdentity, FlavourStorageType = fbstorage.FlavourStorageType, Name = fbstorage.Name, Description = fbstorage.Description, StorageUrl = storageUrl, Version = fbstorage.Version, TimeStamp = lastModified.Value.UtcDateTime };

            }

#if DEBUG
            if (dataType == OrganizationStorages.RestaurantMenus)
            {
                System.Threading.Tasks.Task.Run(async () =>
                {
                    try
                    {
                        storagesClient = new StoragesClient(httpClient);
                        storagesClient.BaseUrl = serverUrl;
                        StorageMetaData storageMetaData = await storagesClient.GetAsync(storageRef.StorageIdentity);
                        if (storageMetaData == null)
                        {
                            var rawStorageData = new RawStorageData(storageRef, null);

                            var objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.OpenStorage(fbstorage.Name, rawStorageData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");

                            //storageMetaData = new OOAdvantech.MetaDataRepository.StorageMetaData()
                            //{
                            //    StorageIdentity = storageRef.StorageIdentity,
                            //    StorageLocation = storageRef.StorageUrl,
                            //    StorageType = objectStorage.StorageMetaData.StorageType,
                            //    StorageName = objectStorage.StorageMetaData.StorageName,
                            //    NativeStorageID = objectStorage.StorageMetaData.NativeStorageID,
                            //    MultipleObjectContext = true

                            //};
                            string res = await storagesClient.PostAsync(objectStorage.StorageMetaData, true);

                            //string res = await storagesClient.PostAsync(storageMetaData);
                        }
                    }
                    catch (Exception error)
                    {

                    }
                });
            }
#endif



            return storageRef;
        }






        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{6345f778-30b8-44d4-b6fb-1757b667d6f0}</MetaDataID>
        OOAdvantech.Collections.Generic.Set<FlavourBusinessFacade.IFlavoursServicesContext> _ServicesContexts = new OOAdvantech.Collections.Generic.Set<IFlavoursServicesContext>();

        public event ObjectChangeStateHandle ObjectChangeState;

        /// <MetaDataID>{de0c6668-9eb9-410c-a040-835c93214f75}</MetaDataID>
        [PersistentMember(nameof(_ServicesContexts))]
        [BackwardCompatibilityID("+6")]
        public System.Collections.Generic.IList<FlavourBusinessFacade.IFlavoursServicesContext> ServicesContexts
        {
            get
            {
                return _ServicesContexts.ToThreadSafeList().ToList();
            }
        }

        /// <exclude>Excluded</exclude>
        string _UserName;
        /// <MetaDataID>{73172d54-a417-41dd-becb-4dbf87a820a9}</MetaDataID>
        [PersistentMember(nameof(_UserName))]
        [BackwardCompatibilityID("+9")]
        public string UserName
        {
            get => _UserName;
            set
            {
                if (_UserName != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _UserName = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <MetaDataID>{3c4f6c4f-4b2b-4b67-95b0-b7c05ad207e7}</MetaDataID>
        [BackwardCompatibilityID("+10")]
        public string FullName { get => Name; set => Name = value; }

        /// <MetaDataID>{b72cc301-2ea1-4f72-a084-1b864e4a4737}</MetaDataID>
        public List<IAccountability> Responsibilities => new List<IAccountability>();




        /// <MetaDataID>{9313df91-244e-4d7d-a126-bc2d779e8a34}</MetaDataID>
        public OrganizationStorageRef UpdateStorage(string name, string description, string storageIdentity)
        {
            lock (this)
            {
                AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
                if (authUser == null)
                    throw new AuthenticationException();
                bool authorized = false;
                if (authUser != null)
                {
                    AuthUserRef authUserRef = AuthUserRef.GetAuthUserRef(authUser, false);
                    if (authUserRef.GetContextRoleObject<Organization>() == this)
                        authorized = true;
                }
                if (!authorized)
                    throw new InvalidCredentialException("The user " + authUser.Name + " isn't recognized as organization owner.");


                var fbstorage = (from storage in this.Storages
                                 where storage.Name == name && storage.StorageIdentity != storageIdentity
                                 select storage).FirstOrDefault();
                if (fbstorage == null)
                {
                    fbstorage = (from storage in this.Storages
                                 where storage.StorageIdentity == storageIdentity
                                 select storage).FirstOrDefault();

                    bool storageUpdated = false;
                    if (fbstorage != null)
                    {
                        using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                        {
                            if (fbstorage.Name != name)
                            {
                                storageUpdated = true;
                                //if (fbstorage.FlavourStorageType == OrganizationStorages.GraphicMenu)
                                //    RenamePublishedGraphicMenu(fbstorage.StorageIdentity, fbstorage.Version, fbstorage.Name, name);

                                fbstorage.Name = name;

                                string blobsContainerName = "usersfolder";
                                if (!string.IsNullOrWhiteSpace(FlavourBusinessManagerApp.RootContainer))
                                    blobsContainerName = FlavourBusinessManagerApp.RootContainer;
                                var blobClient = FlavourBusinessManagerApp.CloudBlobStorageAccount.CreateCloudBlobClient();
                                var container = blobClient.GetContainerReference(blobsContainerName);


                                container.CreateIfNotExists();
                                ObjectID objectID = null;
                                string orgStorageIdentity = null;
                                StorageInstanceRef.GetObjectID(this, out objectID, out orgStorageIdentity);

                            }

                            if (fbstorage.Description != description)
                            {
                                fbstorage.Description = description;
                                storageUpdated = true;
                            }

                            stateTransition.Consistent = true;
                        }
                    }
                    var storageUrl = RawStorageCloudBlob.BlobsStorageHttpAbsoluteUri + fbstorage.Url;
                    var lastModified = RawStorageCloudBlob.GetBlobLastModified(fbstorage.Url);

                    OrganizationStorageRef storageRef = new OrganizationStorageRef { StorageIdentity = fbstorage.StorageIdentity, FlavourStorageType = fbstorage.FlavourStorageType, Name = fbstorage.Name, Description = fbstorage.Description, StorageUrl = storageUrl, TimeStamp = lastModified.Value.UtcDateTime };


                    foreach (var servicesContext in ServicesContexts)
                        servicesContext.GetRunTime().StorageMetaDataUpdated(storageRef);



                    return storageRef;

                }
                else
                    throw new System.Data.DuplicateNameException(string.Format(Properties.Resources.DublicateStorageNameMessage, name));
            }

        }


        #region Code for graphic menus

        /// <summary>
        /// Defines the Graphic menus which you can get with authorization check 
        /// </summary>
        /// <MetaDataID>{14a3544a-3c12-490a-b56d-901dfd4c3b27}</MetaDataID>
        public List<OrganizationStorageRef> GraphicMenus
        {
            get
            {
                if (!(System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") is AuthUser authUser))
                    throw new AuthenticationException();

                string userId = authUser.User_ID;
                AuthUserRef authUserRef = AuthUserRef.GetAuthUserRef(authUser, false);

                if (authUserRef.GetContextRoleObject<Organization>() == this)
                    return UnSafeGraphicMenus;

                bool authorized = false;

                //AuthUserRef authUserRef = AuthUserRef.GetAuthUserRef(authUser, false);
                if (authUserRef.GetContextRoleObject<Organization>() == this)
                    authorized = true;

                if (!authorized)//(authUser.User_ID != this.SignUpUserIdentity)
                    throw new InvalidCredentialException("The user " + authUser.Name + " isn't recognized as organization owner.");
                return UnSafeGraphicMenus;
            }
        }
        /// <summary>
        /// Defines the Graphic menus which you can get without authorization check 
        /// </summary>
        /// <MetaDataID>{8894e33a-9a7a-48ea-915a-b27c15f02fc1}</MetaDataID>
        internal List<OrganizationStorageRef> UnSafeGraphicMenus
        {
            get
            {
                List<OrganizationStorageRef> graphicMenusStorages = new List<OrganizationStorageRef>();
                var fbstorages = (from storage in this.Storages
                                  where storage.FlavourStorageType == OrganizationStorages.GraphicMenu
                                  select storage).ToList();

                string urlRoot = RawStorageCloudBlob.BlobsStorageHttpAbsoluteUri;
                foreach (var fbStorage in fbstorages)
                {
                    try
                    {
                        var storageUrl = urlRoot + fbStorage.Url;
                        var lastModified = RawStorageCloudBlob.GetBlobLastModified(fbStorage.Url);

                        OrganizationStorageRef storageRef = new OrganizationStorageRef { StorageIdentity = fbStorage.StorageIdentity, FlavourStorageType = fbStorage.FlavourStorageType, Name = fbStorage.Name, StorageUrl = storageUrl, TimeStamp = lastModified.Value.UtcDateTime, Version = fbStorage.Version };
                        graphicMenusStorages.Add(storageRef);
                    }
                    catch (Exception error)
                    {

                        throw;
                    }
                }

                return graphicMenusStorages;
            }
        }


        internal List<OrganizationStorageRef> InternalPriceLists
        {
            get
            {
                List<OrganizationStorageRef> priceListsStorages = new List<OrganizationStorageRef>();
                var fbstorages = (from storage in this.Storages
                                  where storage.FlavourStorageType == OrganizationStorages.PriceList
                                  select storage).ToList();

                string urlRoot = RawStorageCloudBlob.BlobsStorageHttpAbsoluteUri;
                foreach (var fbStorage in fbstorages)
                {
                    try
                    {
                        var storageUrl = urlRoot + fbStorage.Url;
                        var lastModified = RawStorageCloudBlob.GetBlobLastModified(fbStorage.Url);

                        OrganizationStorageRef storageRef = new OrganizationStorageRef { StorageIdentity = fbStorage.StorageIdentity, FlavourStorageType = fbStorage.FlavourStorageType, Name = fbStorage.Name, StorageUrl = storageUrl, TimeStamp = lastModified.Value.UtcDateTime, Version = fbStorage.Version };
                        priceListsStorages.Add(storageRef);
                    }
                    catch (Exception error)
                    {

                        throw;
                    }
                }

                return priceListsStorages;
            }
        }


        public List<OrganizationStorageRef> PriceLists
        {
            get
            {
                if (!(System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") is AuthUser authUser))
                    throw new AuthenticationException();

                string userId = authUser.User_ID;
                AuthUserRef authUserRef = AuthUserRef.GetAuthUserRef(authUser, false);

                if (authUserRef.GetContextRoleObject<Organization>() == this)
                    return InternalPriceLists;

                bool authorized = false;

                //AuthUserRef authUserRef = AuthUserRef.GetAuthUserRef(authUser, false);
                if (authUserRef.GetContextRoleObject<Organization>() == this)
                    authorized = true;

                if (!authorized)//(authUser.User_ID != this.SignUpUserIdentity)
                    throw new InvalidCredentialException("The user " + authUser.Name + " isn't recognized as organization owner.");
                return InternalPriceLists;
            }
        }


        /// <MetaDataID>{20b3e8a3-f9c1-4e99-8128-48178b2df525}</MetaDataID>
        public OrganizationStorageRef NewGraphicMenu(string culture)
        {


            AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
            if (authUser == null)
                throw new AuthenticationException();

            bool authorized = false;
            if (authUser != null)
            {
                AuthUserRef authUserRef = AuthUserRef.GetAuthUserRef(authUser, false);
                if (authUserRef.GetContextRoleObject<Organization>() == this)
                    authorized = true;
            }
            if (!authorized)//(authUser.User_ID != this.SignUpUserIdentity)
                throw new InvalidCredentialException("The user " + authUser.Name + " isn't recognized as organization owner.");

            string graphicMenuName = Properties.Resources.DefaultGraphicMenuName;



            var fbstorage = (from storage in this.Storages
                             where storage.Name == graphicMenuName
                             select storage).FirstOrDefault();
            int count = 1;
            while (fbstorage != null)
            {
                graphicMenuName = Properties.Resources.DefaultGraphicMenuName + count;
                fbstorage = (from storage in this.Storages
                             where storage.Name == graphicMenuName
                             select storage).FirstOrDefault();
            }


            try
            {
                ObjectID objectID = null;
                string storageIdentity = null;
                StorageInstanceRef.GetObjectID(this, out objectID, out storageIdentity);
                string blobUrl = "usersfolder/" + Identity + "/" + graphicMenuName + ".xml";
                RawStorageCloudBlob rawStorage = new RawStorageCloudBlob(new XDocument(), blobUrl);

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    var objectStorage = ObjectStorage.NewStorage("RestMenu",
                                    rawStorage,
                                    "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");

                    objectStorage.StorageMetaData.Culture = culture;

                    MenuPresentationModel.RestaurantMenu restaurantMenu = new MenuPresentationModel.RestaurantMenu();
                    restaurantMenu.Name = graphicMenuName;
                    objectStorage.CommitTransientObjectState(restaurantMenu);
                    MenuPresentationModel.MenuPage firstPage = new MenuPage();
                    objectStorage.CommitTransientObjectState(firstPage);
                    restaurantMenu.AddPage(firstPage);

                    MenuPresentationModel.MenuCanvas.FoodItemsHeading titleHeading = new MenuPresentationModel.MenuCanvas.FoodItemsHeading();
                    objectStorage.CommitTransientObjectState(titleHeading);
                    titleHeading.Description = "Gatsby's";// Gatsby's";
                    titleHeading.HeadingType = MenuPresentationModel.MenuCanvas.HeadingType.Title;
                    restaurantMenu.AddMenuItem(titleHeading);

                    fbstorage = new FlavourBusinessStorage();
                    fbstorage.StorageIdentity = objectStorage.StorageMetaData.StorageIdentity;
                    fbstorage.Name = graphicMenuName;
                    fbstorage.Owner = this;
                    fbstorage.Url = blobUrl;
                    fbstorage.FlavourStorageType = OrganizationStorages.GraphicMenu;
                    ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(fbstorage);
                    _Storages.Add(fbstorage);
                    stateTransition.Consistent = true;
                }

                var storageUrl = RawStorageCloudBlob.BlobsStorageHttpAbsoluteUri + fbstorage.Url;
                var lastModified = RawStorageCloudBlob.GetBlobLastModified(fbstorage.Url);

                OrganizationStorageRef storageRef = new OrganizationStorageRef { StorageIdentity = fbstorage.StorageIdentity, FlavourStorageType = fbstorage.FlavourStorageType, Name = fbstorage.Name, Description = fbstorage.Description, StorageUrl = storageUrl, TimeStamp = lastModified.Value.UtcDateTime };

                storageRef.UploadService = this;

                return storageRef;
            }
            catch (Exception error)
            {
                throw;
            }





        }


        /// <MetaDataID>{8029bfa7-eae6-42e5-9363-955d0c9cbab6}</MetaDataID>
        private void RenamePublishedGraphicMenu(string storageIdentity, string version, string oldName, string newName)
        {
            string oldPublishedGraphicMenuUri = null;//string.Format("{0}/Menus/{1}/{2}/{3}.json", Identity, storageIdentity, version, oldName);
            string newPublishedGraphicMenuUri = null;//string.Format("{0}/Menus/{1}/{2}/{3}.json", Identity, storageIdentity, version, newName);
            if (string.IsNullOrWhiteSpace(version))
            {
                oldPublishedGraphicMenuUri = string.Format("{0}/Menus/{1}/{3}.json", Identity, storageIdentity, version, oldName);
                newPublishedGraphicMenuUri = string.Format("{0}/Menus/{1}/{3}.json", Identity, storageIdentity, version, newName);
            }
            else
            {
                oldPublishedGraphicMenuUri = string.Format("{0}/Menus/{1}/{2}/{3}.json", Identity, storageIdentity, version, oldName);
                newPublishedGraphicMenuUri = string.Format("{0}/Menus/{1}/{2}/{3}.json", Identity, storageIdentity, version, newName);

            }
            string blobsContainerName = "usersfolder";
            if (!string.IsNullOrWhiteSpace(FlavourBusinessManagerApp.RootContainer))
            {
                blobsContainerName = FlavourBusinessManagerApp.RootContainer;
                oldPublishedGraphicMenuUri = "usersfolder/" + oldPublishedGraphicMenuUri;
                newPublishedGraphicMenuUri = "usersfolder/" + newPublishedGraphicMenuUri;
            }
            var blobClient = FlavourBusinessManagerApp.CloudBlobStorageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(blobsContainerName);


            container.CreateIfNotExists();


            var existBlob = container.GetBlockBlobReference(oldPublishedGraphicMenuUri);
            var newBlob = container.GetBlockBlobReference(newPublishedGraphicMenuUri);
            if (newBlob.Exists())
            {
                //create a new blob
                var task = newBlob.StartCopyAsync(existBlob);
                task.Wait();
                //delete the old
                existBlob.Delete();
            }
        }

        /// <MetaDataID>{e5a2b4e3-b326-4926-b805-aea57de36bf2}</MetaDataID>
        private void RemovePublishedGraphicMenu(string storageIdentity, string version, string newName)
        {
            string publishedGraphicMenuUri = string.Format("{0}/Menus/{1}/{2}/{3}.json", Identity, storageIdentity, version, newName);

            string blobsContainerName = "usersfolder";
            if (!string.IsNullOrWhiteSpace(FlavourBusinessManagerApp.RootContainer))
            {
                blobsContainerName = FlavourBusinessManagerApp.RootContainer;
                publishedGraphicMenuUri = "usersfolder/" + publishedGraphicMenuUri;
            }
            var blobClient = FlavourBusinessManagerApp.CloudBlobStorageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(blobsContainerName);



            container.CreateIfNotExists();
            var blob = container.GetBlockBlobReference(publishedGraphicMenuUri);
            blob.DeleteIfExists();
        }




        /// <MetaDataID>{ca459274-7876-461b-8069-1b29c071d2b1}</MetaDataID>
        public void RemoveGraphicMenu(string storageIdentity)
        {


            AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
            if (authUser == null)
                throw new AuthenticationException();
            bool authorized = false;
            if (authUser != null)
            {
                AuthUserRef authUserRef = AuthUserRef.GetAuthUserRef(authUser, false);
                if (authUserRef.GetContextRoleObject<Organization>() == this)
                    authorized = true;
            }
            if (!authorized)//(authUser.User_ID != this.SignUpUserIdentity)
                throw new InvalidCredentialException("The user " + authUser.Name + " isn't recognized as organization owner.");


            //Each graphic menu is splitted  in threw parts storage entry as header ,blob with data and published json file

            var fbstorage = (from storage in this.Storages
                             where storage.StorageIdentity == storageIdentity
                             select storage).FirstOrDefault();


            if (fbstorage != null)
            {

                foreach (var serviceContext in this.ServicesContexts)
                {
                    if (serviceContext.GetRunTime().IsGraphicMenuAssigned(storageIdentity))
                        throw new Exception("Remove all graphic menu assignments first.");
                }

                // removes published json
                RemovePublishedGraphicMenu(fbstorage.StorageIdentity, fbstorage.Version, fbstorage.Name);

                var graphicMenuStorageRef = UnSafeGraphicMenus.Where(x => x.StorageIdentity == storageIdentity).FirstOrDefault();


                // removes graphic menu storage data
                var blobClient = FlavourBusinessManagerApp.CloudBlobStorageAccount.CreateCloudBlobClient();
                string containerName = fbstorage.Url.Substring(0, fbstorage.Url.IndexOf("/"));
                string blobUrl = fbstorage.Url.Substring(fbstorage.Url.IndexOf("/") + 1);
                var container = blobClient.GetContainerReference(containerName);
                CloudBlockBlob blob = container.GetBlockBlobReference(blobUrl);
                blob.DeleteIfExists();


                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _Storages.Remove(fbstorage);
                    stateTransition.Consistent = true;
                }
            }
        }

        /// <MetaDataID>{a21cdfe9-2281-4461-84d2-a085398c6739}</MetaDataID>
        public void PublishMenu(OrganizationStorageRef storageRef)
        {
            AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
            if (authUser == null)
                throw new AuthenticationException();

            bool authorized = false;
            if (authUser != null)
            {
                AuthUserRef authUserRef = AuthUserRef.GetAuthUserRef(authUser, false);
                if (authUserRef.GetContextRoleObject<Organization>() == this)
                    authorized = true;
            }
            if (!authorized)//(authUser.User_ID != this.SignUpUserIdentity)
                throw new InvalidCredentialException("The user " + authUser.Name + " isn't recognized as organization owner.");

            ;
            ObjectID objectID = null;
            string orgStorageIdentity = null;
            StorageInstanceRef.GetObjectID(this, out objectID, out orgStorageIdentity);

            var organizationStorage = (from fbStorage in Storages
                                       where fbStorage.StorageIdentity == storageRef.StorageIdentity
                                       select fbStorage).FirstOrDefault();
            string version = "";
            string oldVersion = organizationStorage.Version;

            if (string.IsNullOrWhiteSpace(oldVersion))
            {
                version = "v1";

            }
            else
            {
                int v = int.Parse(oldVersion.Replace("v", ""));
                v++;
                version = "v" + v.ToString();
            }



            string orgIdentity = this.Identity;

            string versionSuffix = "";
            if (!string.IsNullOrWhiteSpace(version))
                versionSuffix = "/" + version + "/";
            else
                versionSuffix = "/";

            string serverStorageFolder = string.Format("usersfolder/{0}/Menus/{1}{2}", orgIdentity, storageRef.StorageIdentity, versionSuffix);



            string oldVersionSuffix = "";
            if (!string.IsNullOrWhiteSpace(oldVersion))
                oldVersionSuffix = "/" + oldVersion + "/";
            else
                oldVersionSuffix = "/";

            string previousVersionServerStorageFolder = string.Format("usersfolder/{0}/Menus/{1}{2}", orgIdentity, storageRef.StorageIdentity, oldVersionSuffix);


            IFileManager fileManager = new BlobFileManager(FlavourBusinessManagerApp.CloudBlobStorageAccount, FlavourBusinessManagerApp.RootContainer);
            string menuBlobName = storageRef.BlobName;

            string xmlFileName = serverStorageFolder + menuBlobName + ".xml";

            try
            {
                string previousVersionXmlFileName = previousVersionServerStorageFolder + menuBlobName + ".xml";

                Stream previousVersionXmlStream = fileManager.GetBlobStream(previousVersionXmlFileName);
                Stream xmlFileNameStream = fileManager.GetBlobStream(storageRef.StorageUrl.Replace(fileManager.RootUri + "/", ""));
                if (FileManager.StreamEquals(previousVersionXmlStream, xmlFileNameStream))
                {
                    return;
                }
            }
            catch (Exception error)
            {


            }
            RawStorageData rawStorageData = new RawStorageData(GetStorage(OrganizationStorages.RestaurantMenus), null);
            ObjectStorage restMenusData = ObjectStorage.OpenStorage("RestMenusData", rawStorageData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");

            rawStorageData = new RawStorageData(storageRef, null);

            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(OOAdvantech.PersistenceLayer.ObjectStorage.OpenStorage("RestMenu", rawStorageData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider"));


            IRestaurantMenuPublisher restaurantMenu = (from menu in storage.GetObjectCollection<IRestaurantMenuPublisher>()
                                                       select menu).FirstOrDefault();


            restaurantMenu.PublishMenu(serverStorageFolder, previousVersionServerStorageFolder, "", fileManager, storageRef);

            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.RequiresNew))
            {
                double? pageHeight = (restaurantMenu as MenuPresentationModel.MenuCanvas.IRestaurantMenu).Pages.FirstOrDefault()?.Height;
                double? pageWidth = (restaurantMenu as MenuPresentationModel.MenuCanvas.IRestaurantMenu).Pages.FirstOrDefault()?.Width;
                if (pageHeight != null)
                {
                    organizationStorage.SetPropertyValue("MenuPageHeight", pageHeight.Value.ToString(CultureInfo.GetCultureInfo(1033)));
                    organizationStorage.SetPropertyValue("MenuPageWidth", pageWidth.Value.ToString(CultureInfo.GetCultureInfo(1033)));
                }
                else
                {
                    organizationStorage.RemoveProperty("MenuPageHeight");
                    organizationStorage.RemoveProperty("MenuPageWidth");
                }

                organizationStorage.Version = version;
                stateTransition.Consistent = true;
            }

            storageRef.Version = version;


            var menuItemsStorageRef = GetStorage(OrganizationStorages.RestaurantMenus);
            var operativeMenuItemsStorageRef = GetStorage(OrganizationStorages.OperativeRestaurantMenu);

            string menuItemsBlobUrl = menuItemsStorageRef.StorageUrl;
            string operativeMenuItemsBlobUrl = operativeMenuItemsStorageRef.StorageUrl;//.Replace(RawStorageCloudBlob.CloudStorageAccount.BlobStorageUri.PrimaryUri.AbsoluteUri+"/","");
            fileManager.Copy(menuItemsBlobUrl, operativeMenuItemsBlobUrl);

            menuItemsStorageRef.UploadService = null;

            //PublishMenuRestaurantMenuData(operativeMenuItemsStorageRef);

            operativeMenuItemsStorageRef = GetStorage(OrganizationStorages.OperativeRestaurantMenu);
            operativeMenuItemsStorageRef.UploadService = null;

            foreach (var servicesContext in ServicesContexts)
            {
                servicesContext.GetRunTime().StorageMetaDataUpdated(storageRef);
                var RestaurantMenuDataSharedUri = servicesContext.GetRunTime().RestaurantMenuDataSharedUri;
                servicesContext.GetRunTime().OperativeRestaurantMenuDataUpdated(operativeMenuItemsStorageRef);
                RestaurantMenuDataSharedUri = servicesContext.GetRunTime().RestaurantMenuDataSharedUri;
            }

        }


        public OrganizationStorageRef PublishPriceList(OrganizationStorageRef priceListStorageRef)
        {
            AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
            if (authUser == null)
                throw new AuthenticationException();

            bool authorized = false;
            if (authUser != null)
            {
                AuthUserRef authUserRef = AuthUserRef.GetAuthUserRef(authUser, false);
                if (authUserRef.GetContextRoleObject<Organization>() == this)
                    authorized = true;
            }
            if (!authorized)//(authUser.User_ID != this.SignUpUserIdentity)
                throw new InvalidCredentialException("The user " + authUser.Name + " isn't recognized as organization owner.");


            ObjectID objectID = null;
            string orgStorageIdentity = null;
            StorageInstanceRef.GetObjectID(this, out objectID, out orgStorageIdentity);

            var organizationStorage = (from fbStorage in Storages
                                       where fbStorage.StorageIdentity == priceListStorageRef.StorageIdentity
                                       select fbStorage).FirstOrDefault();
            string version = "";
            string oldVersion = organizationStorage.Version;

            if (string.IsNullOrWhiteSpace(oldVersion))
            {
                version = "v1";

            }
            else
            {
                int v = int.Parse(oldVersion.Replace("v", ""));
                v++;
                version = "v" + v.ToString();
            }



            string orgIdentity = this.Identity;

            string versionSuffix = "";
            if (!string.IsNullOrWhiteSpace(version))
                versionSuffix = "/" + version + "/";
            else
                versionSuffix = "/";



            string serverStorageFolder = string.Format("usersfolder/{0}/PriceLists/{1}{2}", orgIdentity, priceListStorageRef.StorageIdentity, versionSuffix);

            string oldVersionSuffix = "";
            if (!string.IsNullOrWhiteSpace(oldVersion))
                oldVersionSuffix = "/" + oldVersion + "/";
            else
                oldVersionSuffix = "/";


            FlavourBusinessToolKit.RawStorageData rawStorageData = new FlavourBusinessToolKit.RawStorageData(priceListStorageRef, null);
            OOAdvantech.Linq.Storage priceListStorage = new OOAdvantech.Linq.Storage(OOAdvantech.PersistenceLayer.ObjectStorage.OpenStorage("PriceList", rawStorageData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider"));


            var priceList = (from m_priceList in priceListStorage.GetObjectCollection<PriceList.PriceList>()
                             select m_priceList).FirstOrDefault();


            string previousVersionServerStorageFolder = string.Format("usersfolder/{0}/PriceLists/{1}{2}", orgIdentity, priceListStorageRef.StorageIdentity, oldVersionSuffix);

            string priceListBlobName = serverStorageFolder + priceListStorageRef.StorageUrl.Substring(priceListStorageRef.StorageUrl.LastIndexOf("/") + 1).Replace(".xml", ".json");

            string previousVersionPriceListBlobName = previousVersionServerStorageFolder + priceListStorageRef.StorageUrl.Substring(priceListStorageRef.StorageUrl.LastIndexOf("/") + 1).Replace(".xml", ".json");


            if (WritePublicPriceListMenuDataIfChanged(priceList, priceListBlobName, previousVersionPriceListBlobName))
            {
                organizationStorage.Version = version;
                priceListStorageRef.Version = version;


                foreach (var servicesContext in ServicesContexts)
                    servicesContext.GetRunTime().StorageMetaDataUpdated(priceListStorageRef);
            }
            return priceListStorageRef;




        }

        private bool WritePublicPriceListMenuDataIfChanged(PriceList.PriceList priceList, string jsonFileName, string previousVersionJsonFileName)
        {
            string json = JsonConvert.SerializeObject(priceList, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Serialize, PreserveReferencesHandling = PreserveReferencesHandling.All });

            IFileManager fileManager = new BlobFileManager(FlavourBusinessManagerApp.CloudBlobStorageAccount, FlavourBusinessManagerApp.RootContainer);




            var jSettings = OOAdvantech.Remoting.RestApi.Serialization.JsonSerializerSettings.TypeRefSerializeSettings;
            string jsonEx = OOAdvantech.Json.JsonConvert.SerializeObject(priceList, jSettings);
            if (fileManager.Exist(previousVersionJsonFileName))
            {
                Stream previousVersionJsonStream = fileManager.GetBlobStream(previousVersionJsonFileName);

                byte[] buffer = new byte[previousVersionJsonStream.Length];
                previousVersionJsonStream.Position = 0;
                previousVersionJsonStream.Read(buffer, 0, (int)previousVersionJsonStream.Length);
                previousVersionJsonStream.Close();
                string oldJsonEx = System.Text.Encoding.UTF8.GetString(buffer);

                if (oldJsonEx.Length == jsonEx.Length && oldJsonEx == jsonEx)
                    return false;
            }


            MemoryStream jsonRestaurantMenuStream = new MemoryStream();
            byte[] jsonBuffer = System.Text.Encoding.UTF8.GetBytes(jsonEx);
            jsonRestaurantMenuStream.Write(jsonBuffer, 0, jsonBuffer.Length);
            jsonRestaurantMenuStream.Position = 0;

            if (fileManager != null)
                fileManager.Upload(jsonFileName, jsonRestaurantMenuStream, "application/json");

            return true;


        }




        #endregion

        #region Code for menu data publish
        ///// <MetaDataID>{e316c772-4861-4a2a-8183-e656ff7a011a}</MetaDataID>
        //private void PublishMenuRestaurantMenuData(OrganizationStorageRef restaurantMenuDataStorageRef)
        //{
        //    var organizationStorage = (from fbStorage in Storages
        //                               where fbStorage.StorageIdentity == restaurantMenuDataStorageRef.StorageIdentity
        //                               select fbStorage).FirstOrDefault();

        //    IFileManager fileManager = new BlobFileManager(RawStorageCloudBlob.CloudStorageAccount);
        //    string version = "";
        //    string oldVersion = restaurantMenuDataStorageRef.Version;
        //    if (string.IsNullOrWhiteSpace(oldVersion))
        //    {
        //        version = "v1";
        //    }
        //    else
        //    {
        //        int v = int.Parse(oldVersion.Replace("v", ""));
        //        v++;
        //        version = "v" + v.ToString();
        //    }

        //    string previousVersionServerStorageFolder = GetVersionFolder(oldVersion);

        //    string serverStorageFolder = GetVersionFolder(version);
        //    string jsonFileName = serverStorageFolder + restaurantMenuDataStorageRef.Name + ".json";
        //    WritePublicRestaurantMenuData(restaurantMenuDataStorageRef, jsonFileName);

        //    if (fileManager != null)
        //    {
        //        jsonFileName = previousVersionServerStorageFolder + restaurantMenuDataStorageRef.Name + ".json";
        //        fileManager.GetBlobInfo(jsonFileName).DeleteIfExists();
        //    }

        //    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.RequiresNew))
        //    {
        //        organizationStorage.Version = version;
        //        stateTransition.Consistent = true;
        //    }



        //}

        ///// <MetaDataID>{5643d261-95ed-4c57-a6ee-9bb4994f3b1c}</MetaDataID>
        //private static void WritePublicRestaurantMenuData(OrganizationStorageRef restaurantMenuDataStorageRef, string jsonFileName)
        //{
        //    IFileManager fileManager = new BlobFileManager(RawStorageCloudBlob.CloudStorageAccount);

        //    RawStorageData rawStorageData = new RawStorageData(restaurantMenuDataStorageRef, null);

        //    OOAdvantech.Linq.Storage restMenusData = new OOAdvantech.Linq.Storage(OOAdvantech.PersistenceLayer.ObjectStorage.OpenStorage("RestMenusData", rawStorageData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider"));
        //    Dictionary<object, object> mappedObject = new Dictionary<object, object>();
        //    List<MenuModel.IMenuItem> menuFoodItems = (from menuItem in restMenusData.GetObjectCollection<MenuModel.IMenuItem>()
        //                                               select menuItem).ToList().Select(x => new MenuModel.JsonViewModel.MenuFoodItem(x, mappedObject)).OfType<MenuModel.IMenuItem>().ToList();

        //    var jSetttings = OOAdvantech.Remoting.RestApi.Serialization.JsonSerializerSettings.TypeRefSerializeSettings;
        //    string jsonEx = OOAdvantech.Json.JsonConvert.SerializeObject(menuFoodItems, jSetttings);
        //    MemoryStream jsonRestaurantMenuStream = new MemoryStream();
        //    byte[] jsonBuffer = System.Text.Encoding.UTF8.GetBytes(jsonEx);
        //    jsonRestaurantMenuStream.Write(jsonBuffer, 0, jsonBuffer.Length);
        //    jsonRestaurantMenuStream.Position = 0;

        //    if (fileManager != null)
        //        fileManager.Upload(jsonFileName, jsonRestaurantMenuStream, "application/json");
        //}

        //private string GetVersionFolder(string version)
        //{
        //    string versionSuffix = "";
        //    if (!string.IsNullOrWhiteSpace(version))
        //        versionSuffix = "/" + version + "/";
        //    else
        //        versionSuffix = "/";

        //    string serverStorageFolder = string.Format("usersfolder/{0}{1}", this.Identity, versionSuffix);
        //    return serverStorageFolder;
        //}

        #endregion
        /// <MetaDataID>{9bfa1150-7dfa-466f-b9f1-4a66d8ad6ffd}</MetaDataID>
        public IFlavoursServicesContext NewFlavoursServicesContext()
        {

            AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
            if (authUser == null)
                throw new AuthenticationException();

            bool authorized = false;
            if (authUser != null)
            {
                AuthUserRef authUserRef = AuthUserRef.GetAuthUserRef(authUser, false);
                if (authUserRef.GetContextRoleObject<Organization>() == this)
                    authorized = true;
            }
            if (!authorized)//(authUser.User_ID != this.SignUpUserIdentity)
                throw new InvalidCredentialException("The user " + authUser.Name + " isn't recognized as organization owner.");


            var organizationMainStorage = OpenOrganizationMainStorage();
            if (organizationMainStorage == null)
                organizationMainStorage = OpenOrganizationMainStorage(true);

            ObjectStorage objectStorage = ObjectStorage.GetStorageOfObject(this);
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(ObjectStorage.GetStorageOfObject(this));

                var allStorageNames = (from servicesContext in storage.GetObjectCollection<FlavoursServicesContext>() select servicesContext.ContextStorageName).ToList();

                string contextStorageName = GetContextStorageName();

                string instanceContextStorageName = contextStorageName;
                int instanceCount = 0;
                while (allStorageNames.Contains(instanceContextStorageName))
                {
                    instanceContextStorageName = contextStorageName + instanceCount.ToString();
                    instanceCount++;
                }


                FlavoursServicesContext flavoursServiceContext = new FlavoursServicesContext();
                flavoursServiceContext.ContextStorageName = instanceContextStorageName;
                flavoursServiceContext.OrganizationStorageIdentity = organizationMainStorage.StorageMetaData.StorageName;
                flavoursServiceContext.Description = Properties.Resources.FlavoursServicePointDefaultName;
                ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(flavoursServiceContext);
                _ServicesContexts.Add(flavoursServiceContext);
                flavoursServiceContext.ServicesContextIdentity = Guid.NewGuid().ToString("N");


                IIsolatedComputingContext isolatedComputingContext = ComputingCluster.CurrentComputingCluster.NewIsolatedComputingContext(flavoursServiceContext.ServicesContextIdentity, flavoursServiceContext.Description);

                flavoursServiceContext.RunAtContext = isolatedComputingContext;
                stateTransition.Consistent = true;
                return flavoursServiceContext;
            }
        }

        /// <MetaDataID>{4e2cd8f1-ead5-4cd3-88d1-5714f5bb01c3}</MetaDataID>
        private string GetContextStorageName()
        {
            string contextStorageName = Email.ToLower();

            Regex rgx = new Regex("[^a-zA-Z0-9 -]");
            contextStorageName = rgx.Replace(contextStorageName, "");

            if (Regex.IsMatch(contextStorageName, @"^\d"))
                contextStorageName = "s" + contextStorageName;


            //TODO:Να ελεχθεί όταν δεν υπάρχει Email
            return contextStorageName;


        }

        //[ObjectActivationCall]
        //internal void OnActivated()
        //{
        //    Task.Run(() =>
        //    {
        //        var objectStorage = OpenOrganizationMainStorage(true);
        //        foreach (var servicesContext in ServicesContexts)
        //        {
        //            (servicesContext as FlavoursServicesContext).OrganizationStorageIdentity = objectStorage.StorageMetaData.StorageIdentity;
        //        }

        //    });
        //}
        static object lockObj = new object();
        public ObjectStorage OpenOrganizationMainStorage(bool create = false)
        {
            lock (lockObj)
            {
                ObjectStorage storageSession = null;
                string storageName = GetOrganizationMainStorageName();
                string storageLocation = "DevStorage";
                string storageType = "OOAdvantech.WindowsAzureTablesPersistenceRunTime.StorageProvider";

                try
                {
                    storageSession = ObjectStorage.OpenStorage(storageName,
                                                                storageLocation,
                                                                storageType, FlavourBusinessManagerApp.FlavourBusinessStoragesAccountName, FlavourBusinessManagerApp.FlavourBusinessStoragesAccountkey);


                    System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();
                    string serverUrl = OOAdvantech.Remoting.RestApi.RemotingServices.ServerPublicUrl.Substring(0, OOAdvantech.Remoting.RestApi.RemotingServices.ServerPublicUrl.IndexOf("/api"));
                    var storagesClient = new OOAdvantech.PersistenceLayer.StoragesClient(httpClient);
                    storagesClient.BaseUrl = serverUrl;
                    var task = storagesClient.GetAsync(storageSession.StorageMetaData.StorageIdentity);
                    task.Wait();
                    var storageMetaData = task.Result;
                    if (storageMetaData == null || storageMetaData.StorageIdentity == null)
                        storagesClient.PostAsync(storageSession.StorageMetaData, true);

                }
                catch (OOAdvantech.PersistenceLayer.StorageException Error)
                {
                    if (Error.Reason == OOAdvantech.PersistenceLayer.StorageException.ExceptionReason.StorageDoesnotExist)
                    {
                        if (create)
                            storageSession = ObjectStorage.NewStorage(storageName,
                                                                    storageLocation,
                                                                    storageType);
                        else
                            return null;
                    }
                    else
                        throw Error;
                    try
                    {
                        RegisterOrganizationMainStorageComponents(storageSession);

                    }
                    catch (System.Exception Errore)
                    {
                        int sdf = 0;
                    }
                }
                catch (System.Exception Error)
                {
                    int tt = 0;
                }

                return storageSession;
            }
        }

        private void RegisterOrganizationMainStorageComponents(ObjectStorage objectStorage)
        {
            List<string> types = new List<string>() { typeof(FoodServiceClient).FullName };
            objectStorage.StorageMetaData.RegisterComponent(typeof(Organization).Assembly.FullName, types);
        }

        private string GetOrganizationMainStorageName()
        {
            string contextStorageName = "jim.liakos@gmail.com";// Email.ToLower();

            Regex rgx = new Regex("[^a-zA-Z0-9 -]");
            contextStorageName = rgx.Replace(contextStorageName, "");

            if (Regex.IsMatch(contextStorageName, @"^\d"))
                contextStorageName = "s" + contextStorageName;


            //TODO:Να ελεχθεί όταν δεν υπάρχει Email
            return contextStorageName += "Main";


        }

        /// <MetaDataID>{986f2a52-ad23-4dc6-8122-cf57c5788b3a}</MetaDataID>
        public void DeleteServicesContext(IFlavoursServicesContext servicesContext)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _ServicesContexts.Remove(servicesContext);
                stateTransition.Consistent = true;
            }

        }

        /// <MetaDataID>{c9ca1322-be86-4924-b289-05b74abb9c60}</MetaDataID>
        public string NewSupervisor(string servicesContextIdentity)
        {
            AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
            if (authUser == null)
                throw new AuthenticationException();
            bool authorized = false;
            if (authUser != null)
            {
                AuthUserRef authUserRef = AuthUserRef.GetAuthUserRef(authUser, false);
                if (authUserRef.GetContextRoleObject<Organization>() == this)
                    authorized = true;
            }
            if (!authorized)//(authUser.User_ID != this.SignUpUserIdentity)
                throw new InvalidCredentialException("The user " + authUser.Name + " isn't recognized as organization owner.");

            var servicesContext = ServicesContexts.Where(x => x.ServicesContextIdentity == servicesContextIdentity).FirstOrDefault();
            if (servicesContext == null)
                throw new System.ArgumentException("Unknown service context");

            return servicesContext.GetRunTime().NewSupervisor();

        }
        /// <MetaDataID>{b8cc5b90-1b66-4e6a-b281-2c62c5f8b6aa}</MetaDataID>
        public FlavourBusinessFacade.HumanResources.ITranslator AssignTranslatorRoleToUser(UserData userData)
        {
            var objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this);
            AuthUserRef authUserRef = AuthUserRef.GetAuthUserRef(userData.Identity);

            if (authUserRef == null)
                throw new InvalidCredentialException("The user " + userData.FullName + " isn't registered.");

            var translator = authUserRef.GetRoleObject<FlavourBusinessFacade.HumanResources.ITranslator>() as HumanResources.Translator;
            if (translator == null)
            {

                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.RequiresNew))
                {
                    translator = new HumanResources.Translator();
                    translator.OAuthUserIdentity = userData.Identity;
                    translator.Name = userData.FullName;
                    translator.Email = userData.Email;
                    translator.PhotoUrl = userData.PhotoUrl;
                    objectStorage.CommitTransientObjectState(translator);
                    stateTransition.Consistent = true;
                }
                authUserRef.AddRole(translator);
            }
            return translator;
        }

        /// <MetaDataID>{ac6a59dd-0fd8-4d30-98d0-a5caf49c62b7}</MetaDataID>
        public IAccountability AssignMenuMakerRoleToUser(UserData userData)
        {
            var objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this);
            AuthUserRef authUserRef = AuthUserRef.GetAuthUserRef(userData.Identity);

            if (authUserRef == null)
                throw new InvalidCredentialException("The user " + userData.FullName + " isn't registered.");

            HumanResources.MenuMaker menuMaker = authUserRef.GetRoleObject<HumanResources.MenuMaker>() as HumanResources.MenuMaker;
            if (menuMaker != null)
            {

                OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(objectStorage);
                var menuMakingAccountability = (from accountability in storage.GetObjectCollection<IAccountability>()
                                                where accountability.Responsible == menuMaker && accountability.Responsible == this
                                                select accountability).FirstOrDefault();

                if (menuMakingAccountability == null)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        menuMakingAccountability = new HumanResources.Accountability(this, menuMaker);
                        objectStorage.CommitTransientObjectState(menuMakingAccountability);
                        menuMakingAccountability.Description = "ManuMaking";
                        (menuMakingAccountability as HumanResources.Accountability).AccountabilityType = HumanResources.AccountabilityType.MenuMaking;

                        stateTransition.Consistent = true;
                    }
                }
                return menuMakingAccountability;
            }
            return null;

        }


        /// <MetaDataID>{45fac76a-1114-4606-a374-02a94b2a1593}</MetaDataID>
        public List<ITranslator> GetTranslators(WorkerState state)
        {
            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this));
            return (from translator in storage.GetObjectCollection<HumanResources.Translator>()
                    select translator).OfType<ITranslator>().ToList();

        }

        /// <MetaDataID>{64dd95c6-b0f2-4b47-b1ac-799a4862f604}</MetaDataID>
        public List<IAccountability> GetMenuMakers(WorkerState state)
        {
            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this));
            return (from menuMakingAccountability in storage.GetObjectCollection<HumanResources.Accountability>()
                    where menuMakingAccountability.AccountabilityType == HumanResources.AccountabilityType.MenuMaking
                    select menuMakingAccountability).OfType<IAccountability>().ToList();
        }



        /// <MetaDataID>{94607e6b-bb09-49e9-8644-5f2dd5ec14bf}</MetaDataID>
        public void RemoveTranslator(ITranslator translator)
        {
            AuthUserRef authUserRef = AuthUserRef.GetAuthUserRef((translator as HumanResources.Translator).OAuthUserIdentity);
            if (authUserRef != null)
                authUserRef.RemoveRole(translator);

            ObjectStorage.DeleteObject(translator);

        }
        /// <MetaDataID>{371a2e62-a952-4241-aef5-7cd1850affb7}</MetaDataID>
        public void RemoveMenuMaker(IAccountability menuMakingAccountability)
        {

            ObjectStorage.DeleteObject(menuMakingAccountability);

        }

        public OrganizationStorageRef NewPriceList()
        {

            AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
            if (authUser == null)
                throw new AuthenticationException();

            bool authorized = false;
            if (authUser != null)
            {
                AuthUserRef authUserRef = AuthUserRef.GetAuthUserRef(authUser, false);
                if (authUserRef.GetContextRoleObject<Organization>() == this)
                    authorized = true;
            }
            if (!authorized)//(authUser.User_ID != this.SignUpUserIdentity)
                throw new InvalidCredentialException("The user " + authUser.Name + " isn't recognized as organization owner.");



            string priceListName = Resources.DefaultPriceListDescription;

            var fbstorage = (from storage in this.Storages
                             where storage.Name == priceListName
                             select storage).FirstOrDefault();
            int count = 1;
            while (fbstorage != null)
            {
                priceListName = Resources.DefaultPriceListDescription + count;
                fbstorage = (from storage in this.Storages
                             where storage.Name == priceListName
                             select storage).FirstOrDefault();
            }
            try
            {
                ObjectID objectID = null;
                string storageIdentity = null;
                StorageInstanceRef.GetObjectID(this, out objectID, out storageIdentity);
                string blobUrl = "usersfolder/" + Identity + "/" + priceListName + ".xml";
                RawStorageCloudBlob rawStorage = new RawStorageCloudBlob(new XDocument(), blobUrl);

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    var objectStorage = ObjectStorage.NewStorage("PriceList",
                                    rawStorage,
                                    "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");


                    PriceList.PriceList priceList = new PriceList.PriceList();
                    priceList.Description = priceListName;
                    objectStorage.CommitTransientObjectState(priceList);
                    PriceList.ItemsPriceInfo priceListItemsPriceInfo = new PriceList.ItemsPriceInfo();
                    priceListItemsPriceInfo.Description = "pricelist items";
                    objectStorage.CommitTransientObjectState(priceListItemsPriceInfo);
                    priceList.AddItemsPriceInfos(priceListItemsPriceInfo);

                    fbstorage = new FlavourBusinessStorage();
                    fbstorage.StorageIdentity = objectStorage.StorageMetaData.StorageIdentity;
                    fbstorage.Name = priceListName;
                    fbstorage.Owner = this;
                    fbstorage.Url = blobUrl;
                    fbstorage.FlavourStorageType = OrganizationStorages.PriceList;
                    ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(fbstorage);
                    _Storages.Add(fbstorage);
                    stateTransition.Consistent = true;
                }

                var storageUrl = RawStorageCloudBlob.BlobsStorageHttpAbsoluteUri + fbstorage.Url;
                var lastModified = RawStorageCloudBlob.GetBlobLastModified(fbstorage.Url);

                OrganizationStorageRef storageRef = new OrganizationStorageRef { StorageIdentity = fbstorage.StorageIdentity, FlavourStorageType = fbstorage.FlavourStorageType, Name = fbstorage.Name, Description = fbstorage.Description, StorageUrl = storageUrl, TimeStamp = lastModified.Value.UtcDateTime };

                storageRef.UploadService = this;

                return storageRef;
            }
            catch (Exception error)
            {
                throw;
            }
        }

        public void RemovePriceList(string storageIdentity)
        {

            AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
            if (authUser == null)
                throw new AuthenticationException();
            bool authorized = false;
            if (authUser != null)
            {
                AuthUserRef authUserRef = AuthUserRef.GetAuthUserRef(authUser, false);
                if (authUserRef.GetContextRoleObject<Organization>() == this)
                    authorized = true;
            }
            if (!authorized)//(authUser.User_ID != this.SignUpUserIdentity)
                throw new InvalidCredentialException("The user " + authUser.Name + " isn't recognized as organization owner.");


            //Each graphic menu is splitted  in threw parts storage entry as header ,blob with data and published json file

            var fbstorage = (from storage in this.Storages
                             where storage.StorageIdentity == storageIdentity
                             select storage).FirstOrDefault();


            if (fbstorage != null)
            {

                #region service context price lists
                //foreach (var serviceContext in this.ServicesContexts)
                //{
                //    if (serviceContext.GetRunTime().IsGraphicMenuAssigned(storageIdentity))
                //        throw new Exception("Remove all graphic menu assignments first.");
                //}

                //// removes published json
                //RemovePublishedGraphicMenu(fbstorage.StorageIdentity, fbstorage.Version, fbstorage.Name); 
                #endregion

                var priceListStorageRef = PriceLists.Where(x => x.StorageIdentity == storageIdentity).FirstOrDefault();


                // removes graphic menu storage data
                var blobClient = FlavourBusinessManagerApp.CloudBlobStorageAccount.CreateCloudBlobClient();
                string containerName = fbstorage.Url.Substring(0, fbstorage.Url.IndexOf("/"));
                string blobUrl = fbstorage.Url.Substring(fbstorage.Url.IndexOf("/") + 1);
                var container = blobClient.GetContainerReference(containerName);
                CloudBlockBlob blob = container.GetBlockBlobReference(blobUrl);
                blob.DeleteIfExists();


                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _Storages.Remove(fbstorage);
                    stateTransition.Consistent = true;
                }
            }
        }
    }
}



