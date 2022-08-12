
using System.Data.HashFunction.CRC;

using OOAdvantech.Remoting.RestApi;
using System.Linq;
using System.Collections.Generic;
using System;
using FlavourBusinessFacade;
using FlavourBusinessFacade.HumanResources;
using Azure;

namespace FlavourBusinessManager
{
    /// <MetaDataID>{2d073bb7-66e1-4428-995a-434e081a6708}</MetaDataID>
    /// <summary>Defines a authentication system account which refered to server roles in flavour business system roles  </summary>
    public class AuthUserRef : Azure.Data.Tables.ITableEntity
    {

        public class Role
        {
            public string TypeFullName { get; set; }
            public string ObjectUri { get; set; }
            public string ComputingContextID { get; set; }


            /// <exclude>Excluded</exclude>
            object _RoleObject;

            [OOAdvantech.Json.JsonIgnore]
            public object RoleObject
            {
                get
                {
                    if (!string.IsNullOrWhiteSpace(this.ComputingContextID) && this.ComputingContextID != ComputationalResources.IsolatedComputingContext.CurrentContextID)
                    {
                        if (_RoleObject == null && !string.IsNullOrWhiteSpace(ObjectUri))
                        {
                            string channelUri = string.Format("{0}({1})", RemotingServices.ServerPublicUrl, this.ComputingContextID);
                            _RoleObject = RemotingServices.GetPersistentObject(channelUri, ObjectUri);
                        }

                    }
                    else if (_RoleObject == null && !string.IsNullOrWhiteSpace(ObjectUri))
                        _RoleObject = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(ObjectUri);

                    return _RoleObject;
                }
                set
                {
                    if (System.Runtime.Remoting.RemotingServices.IsTransparentProxy(value))
                    {
                        string publicChannelUri = null;
                        string internalchannelUri = null;
                        var proxy = System.Runtime.Remoting.RemotingServices.GetRealProxy(value as MarshalByRefObject) as OOAdvantech.Remoting.RestApi.Proxy;
                        ObjectUri = (proxy as OOAdvantech.Remoting.IProxy).ObjectUri.PersistentUri;
                        ObjRef.GetChannelUriParts(proxy.ChannelUri, out publicChannelUri, out internalchannelUri);
                        ComputingContextID = internalchannelUri;
                        TypeFullName = proxy.TypeFullName;

                    }
                    else
                    {
                        ObjectUri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(value).GetPersistentObjectUri(value);
                        TypeFullName = value.GetType().FullName;
                        ComputingContextID = ComputationalResources.IsolatedComputingContext.CurrentContextID;
                    }

                    _RoleObject = value;
                }
            }

            public static string GetObjectUri(object roleObject)
            {
                if (System.Runtime.Remoting.RemotingServices.IsTransparentProxy(roleObject))
                {
                    var proxy = System.Runtime.Remoting.RemotingServices.GetRealProxy(roleObject as MarshalByRefObject) as OOAdvantech.Remoting.RestApi.Proxy;
                    return (proxy as OOAdvantech.Remoting.IProxy).ObjectUri.PersistentUri;
                }
                else
                {
                    return OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(roleObject).GetPersistentObjectUri(roleObject);
                }
            }
        }


        ///// <exclude>Excluded</exclude>
        //static CloudTable _AuthUserRefCloudTable;
        ///// <MetaDataID>{8aa0e63d-2722-4cb9-b67a-c0f0c813a854}</MetaDataID>
        //static CloudTable AuthUserRefCloudTable
        //{
        //    get
        //    {
        //        if (_AuthUserRefCloudTable == null)
        //        {
        //            CloudStorageAccount account = FlavourBusinessManagerApp.CloudTableStorageAccount;

        //            //if ((string.IsNullOrWhiteSpace(FlavourBusinessManagerApp.FlavourBusinessStoragesAccountName) || FlavourBusinessManagerApp.FlavourBusinessStoragesAccountName == "devstoreaccount1"))
        //            //    account = CloudStorageAccount.DevelopmentStorageAccount;
        //            //else
        //            //    account = new CloudStorageAccount(new StorageCredentials(FlavourBusinessManagerApp.FlavourBusinessStoragesAccountName, FlavourBusinessManagerApp.FlavourBusinessStoragesAccountkey), true);

        //            CloudTableClient tableClient = account.CreateCloudTableClient();
        //            CloudTable table = tableClient.GetTableReference("AuthUserRefCloudTable");
        //            if (!table.Exists())
        //                table.CreateIfNotExists();
        //            _AuthUserRefCloudTable = table;
        //        }
        //        return _AuthUserRefCloudTable;
        //    }
        //}


        /// <exclude>Excluded</exclude>
        static Azure.Data.Tables.TableClient _AuthUserRefCloudTable_a;

        static Azure.Data.Tables.TableClient AuthUserRefCloudTable_a
        {
            get
            {
                if (_AuthUserRefCloudTable_a == null)
                {

                    var tablesAccount = FlavourBusinessManagerApp.TablesAccount;


                    var table_a = tablesAccount.GetTableClient("AuthUserRefCloudTable");

                    Azure.Pageable<Azure.Data.Tables.Models.TableItem> queryTableResults = tablesAccount.Query(String.Format("TableName eq '{0}'", "AuthUserRefCloudTable"));
                    bool authUserRefCloudTable_Exist = queryTableResults.Count() > 0;




                    if (!authUserRefCloudTable_Exist)
                        table_a.CreateIfNotExists();


                    _AuthUserRefCloudTable_a = table_a;
                }
                return _AuthUserRefCloudTable_a;
            }
        }

        /// <MetaDataID>{c51d844b-ef6c-414e-a48f-a0ad7c7223f1}</MetaDataID>
        public static ICRCConfig CRCConfig
        {
            get
            {
                return System.Data.HashFunction.CRC.CRCConfig.CRC4_ITU;
            }
        }

        /// <MetaDataID>{45bd7f73-5188-4dc2-a0cf-b6f5dd868ab8}</MetaDataID>
        public string Email { get; set; }

        /// <MetaDataID>{0dca16c1-59f0-4423-bf0a-2b3f39f1dfb5}</MetaDataID>
        public string Address { get; set; }

        /// <MetaDataID>{8c4fee8d-e6fa-4636-a58f-3f9ab0692625}</MetaDataID>
        public string PhoneNumber { get; set; }

        /// <MetaDataID>{c52d6f17-a7cf-499f-8026-2592a2df41ec}</MetaDataID>
        public string PhotoUrl { get; set; }

        /// <MetaDataID>{1e4fd107-a4eb-46eb-971c-f65c33dd7f24}</MetaDataID>
        public string GetIdentity()
        {
            return RowKey;
        }

        /// <MetaDataID>{1c062a9d-74b0-491b-b1af-0f8b6dec0376}</MetaDataID>
        public string FullName { get; set; }

        /// <MetaDataID>{ef69c2e7-b418-46bd-8be6-400cea6a46d4}</MetaDataID>
        static object AuthUserRefsLock = new object();
        /// <MetaDataID>{559cb54f-0a36-4108-99f3-b6dcc83e430d}</MetaDataID>
        public static AuthUserRef GetAuthUserRef(AuthUser authUser, bool create)
        {
            lock (AuthUserRefsLock)
            {
                if (authUser == null)
                    return null;

                string partitionKey = CRCFactory.Instance.Create(CRCConfig).ComputeHash(System.Text.Encoding.UTF8.GetBytes(authUser.User_ID)).AsHexString();

                //AuthUserRef authUserRef = (from authUserRefEntry in AuthUserRefCloudTable.CreateQuery<AuthUserRef>()
                //                           where authUserRefEntry.PartitionKey == partitionKey && authUserRefEntry.RowKey == authUser.User_ID
                //                           select authUserRefEntry).FirstOrDefault();
                AuthUserRef authUserRef = (from authUserRefEntry in AuthUserRefCloudTable_a.Query<AuthUserRef>()
                                           where authUserRefEntry.PartitionKey == partitionKey && authUserRefEntry.RowKey == authUser.User_ID
                                           select authUserRefEntry).FirstOrDefault();


                if (authUserRef == null && create)
                {
                    authUserRef = new AuthUserRef() { PartitionKey = partitionKey, RowKey = authUser.User_ID, Email = authUser.Email, PhotoUrl = authUser.Picture, RolesJson = OOAdvantech.Json.JsonConvert.SerializeObject(new List<string>()) };
                    //TableOperation insertOperation = TableOperation.Insert(authUserRef);
                    //var result = AuthUserRefCloudTable.Execute(insertOperation);
                    var result = AuthUserRefCloudTable_a.AddEntity(authUserRef);
                }
                else
                {
                    if (authUserRef != null)
                    {
                        if (authUser.Email != authUserRef.Email || authUser.Picture != authUserRef.PhotoUrl)
                        {
                            authUserRef.Email = authUser.Email;
                            authUserRef.PhotoUrl = authUser.Picture;
                            //var result = AuthUserRefCloudTable.Execute(replaceOperation);
                            var result = AuthUserRefCloudTable_a.UpdateEntity(authUserRef, Azure.ETag.All, Azure.Data.Tables.TableUpdateMode.Replace);
                        }
                    }
                }
                return authUserRef;
            }
        }


        public static AuthUserRef GetCallContextAuthUserRef(bool create)
        {
            AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
            lock (AuthUserRefsLock)
            {
                if (authUser == null)
                    return null;

                string partitionKey = CRCFactory.Instance.Create(CRCConfig).ComputeHash(System.Text.Encoding.UTF8.GetBytes(authUser.User_ID)).AsHexString();


                AuthUserRef authUserRef = (from authUserRefEntry in AuthUserRefCloudTable_a.Query<AuthUserRef>()
                                           where authUserRefEntry.PartitionKey == partitionKey && authUserRefEntry.RowKey == authUser.User_ID
                                           select authUserRefEntry).FirstOrDefault();

                if (authUserRef == null && create)
                {
                    authUserRef = new AuthUserRef() { PartitionKey = partitionKey, RowKey = authUser.User_ID, Email = authUser.Email, PhotoUrl = authUser.Picture, RolesJson = OOAdvantech.Json.JsonConvert.SerializeObject(new List<string>()) };
                    //TableOperation insertOperation = TableOperation.Insert(authUserRef);
                    //var result = AuthUserRefCloudTable.Execute(insertOperation);
                    var result = AuthUserRefCloudTable_a.AddEntity(authUserRef);
                }
                else
                {
                    if (authUserRef != null)
                    {
                        if (authUser.Email != authUserRef.Email || authUser.Picture != authUserRef.PhotoUrl)
                        {
                            authUserRef.Email = authUser.Email;
                            authUserRef.PhotoUrl = authUser.Picture;
                            //TableOperation replaceOperation = TableOperation.Replace(authUserRef);
                            //var result = AuthUserRefCloudTable.Execute(replaceOperation);
                            var result = AuthUserRefCloudTable_a.UpdateEntity(authUserRef, Azure.ETag.All, Azure.Data.Tables.TableUpdateMode.Replace);
                        }
                    }
                }
                return authUserRef;
            }
        }

        /// <MetaDataID>{83d4c8f0-7004-4de0-a736-5330e5deddac}</MetaDataID>
        public static AuthUserRef GetAuthUserRef(string user_ID)
        {
            lock (AuthUserRefsLock)
            {
                if (user_ID == null)
                    return null;

                string partitionKey = CRCFactory.Instance.Create(CRCConfig).ComputeHash(System.Text.Encoding.UTF8.GetBytes(user_ID)).AsHexString();

                AuthUserRef authUserRef = (from authUserRefEntry in AuthUserRefCloudTable_a.Query<AuthUserRef>()
                                           where authUserRefEntry.PartitionKey == partitionKey && authUserRefEntry.RowKey == user_ID
                                           select authUserRefEntry).FirstOrDefault();

                return authUserRef;
            }
        }


        /// <MetaDataID>{d5a9745d-a286-4ad7-8a22-352aa5d88d31}</MetaDataID>
        public static AuthUserRef GetAuthUserRefByUserName(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                return null;
            lock (AuthUserRefsLock)
            {


                AuthUserRef authUserRef = (from authUserRefEntry in AuthUserRefCloudTable_a.Query<AuthUserRef>()
                                           where authUserRefEntry.Email == userName
                                           select authUserRefEntry).FirstOrDefault();
                return authUserRef;
            }
        }

        /// <MetaDataID>{8bccb578-89f5-410c-b59c-70fe2169f980}</MetaDataID>
        internal static AuthUserRef GetAuthUserRefForRole(string user_ID)
        {
            string partitionKey = CRCFactory.Instance.Create(CRCConfig).ComputeHash(System.Text.Encoding.UTF8.GetBytes(user_ID)).AsHexString();

            AuthUserRef authUserRef = (from authUserRefEntry in AuthUserRefCloudTable_a.Query<AuthUserRef>()
                                       where authUserRefEntry.PartitionKey == partitionKey && authUserRefEntry.RowKey == user_ID
                                       select authUserRefEntry).FirstOrDefault();
            return authUserRef;
        }

        /// <MetaDataID>{73de9d3d-cffe-49cd-a33b-e10fbb1f6748}</MetaDataID>
        public string RolesJson { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }

        /// <MetaDataID>{1e0a7d36-5beb-430c-a0d3-f066edc7f716}</MetaDataID>
        object rolesLock = new object();
        /// <MetaDataID>{628a5046-bca8-4cf9-9b31-eb750401b15c}</MetaDataID>
        System.Collections.Generic.List<Role> _Roles;
        /// <MetaDataID>{9e8460db-142c-4236-babe-9593961ba8b5}</MetaDataID>
        public List<Role> GetRoles()
        {
            lock (rolesLock)
            {
                if (_Roles == null)
                    _Roles = OOAdvantech.Json.JsonConvert.DeserializeObject<List<Role>>(RolesJson);
                return _Roles.ToList();
            }
        }

        /// <MetaDataID>{33a47219-9bca-48bb-9cf3-d918c577ecb8}</MetaDataID>
        public void AddRole(object roleObject)
        {
            lock (rolesLock)
            {
                if (_Roles == null)
                    GetRoles();
                if (_Roles.Where(x => x.RoleObject == roleObject).FirstOrDefault() == null)
                {
                    Role role = new Role();
                    role.RoleObject = roleObject;
                    _Roles.Add(role);
                    int tries = 5;
                    while (tries >= 0)
                    {
                        try
                        {
                            AuthUserRef authUserRef = (from authUserRefEntry in AuthUserRefCloudTable_a.Query<AuthUserRef>()
                                                       where authUserRefEntry.PartitionKey == PartitionKey && authUserRefEntry.RowKey == RowKey
                                                       select authUserRefEntry).FirstOrDefault();

                            var rolesDictionary = _Roles.ToDictionary(x => x.ObjectUri);

                            foreach (var storageRole in OOAdvantech.Json.JsonConvert.DeserializeObject<List<Role>>(authUserRef.RolesJson))
                            {
                                if (!rolesDictionary.ContainsKey(storageRole.ObjectUri))
                                    rolesDictionary.Add(storageRole.ObjectUri, storageRole);
                            }
                            RolesJson = OOAdvantech.Json.JsonConvert.SerializeObject(rolesDictionary.Values);
                            //TableOperation updateOperation = TableOperation.Replace(this);
                            //var result = AuthUserRefCloudTable.Execute(updateOperation);
                            var result = AuthUserRefCloudTable_a.UpdateEntity(this, Azure.ETag.All, Azure.Data.Tables.TableUpdateMode.Replace);
                            _Roles = rolesDictionary.Values.ToList();
                            break;
                        }
                        catch (System.Exception error)
                        {
                        }
                        tries--;
                    }
                }
            }
        }


        /// <MetaDataID>{87aabbda-23f8-46fe-8333-b612e2e2e5ee}</MetaDataID>
        public void RemoveRole(object roleObject)
        {
            lock (rolesLock)
            {
                if (_Roles == null)
                    GetRoles();
                if (_Roles.Where(x => x.RoleObject == roleObject).FirstOrDefault() != null)
                {
                    var role = _Roles.Where(x => x.RoleObject == roleObject).FirstOrDefault();
                    _Roles.Remove(role);

                    int tries = 5;
                    while (tries >= 0)
                    {
                        try
                        {
                            AuthUserRef authUserRef = (from authUserRefEntry in AuthUserRefCloudTable_a.Query<AuthUserRef>()
                                                       where authUserRefEntry.PartitionKey == PartitionKey && authUserRefEntry.RowKey == RowKey
                                                       select authUserRefEntry).FirstOrDefault();

                            var rolesDictionary = _Roles.ToDictionary(x => x.ObjectUri);

                            foreach (var storageRole in OOAdvantech.Json.JsonConvert.DeserializeObject<List<Role>>(authUserRef.RolesJson))
                            {
                                if (!rolesDictionary.ContainsKey(storageRole.ObjectUri))
                                    rolesDictionary.Add(storageRole.ObjectUri, storageRole);
                            }
                            rolesDictionary.Remove(role.ObjectUri);
                            RolesJson = OOAdvantech.Json.JsonConvert.SerializeObject(rolesDictionary.Values);
                            //TableOperation updateOperation = TableOperation.Replace(this);
                            //var result = AuthUserRefCloudTable.Execute(updateOperation);
                            var result = AuthUserRefCloudTable_a.UpdateEntity(this, Azure.ETag.All, Azure.Data.Tables.TableUpdateMode.Replace);
                            _Roles = rolesDictionary.Values.ToList();
                            break;
                        }
                        catch (System.Exception error)
                        {
                        }
                        tries--;
                    }
                }

            }
        }
        /// <MetaDataID>{c7f85c32-4099-4068-8075-1b77fc372583}</MetaDataID>
        internal T GetRoleObject<T>() where T : class
        {
            lock (rolesLock)
            {
                GetRoles();
                Role role = _Roles.Where(x => x.TypeFullName == typeof(T).FullName).FirstOrDefault();
                if (role != null)
                    return role.RoleObject as T;
                else
                    return default(T);
            }
        }
        /// <summary>
        /// Get role object which belong to the current computing context
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        internal T GetContextRoleObject<T>() where T : class
        {
            lock (rolesLock)
            {
                GetRoles();
                Role role = _Roles.Where(x => x.TypeFullName == typeof(T).FullName && x.ComputingContextID == ComputationalResources.IsolatedComputingContext.CurrentContextID && x.RoleObject != null).FirstOrDefault();

                if (role != null)
                    return role.RoleObject as T;
                else
                    return default(T);
            }
        }

        /// <MetaDataID>{313295d7-0029-44af-8cfb-13c59b664e3d}</MetaDataID>
        internal bool HasRole(object roleObject)
        {
            lock (rolesLock)
            {
                GetRoles();
                var objectUri = Role.GetObjectUri(roleObject);
                return _Roles.Where(x => x.ObjectUri == objectUri).FirstOrDefault() != null;
            }

        }

        /// <MetaDataID>{99b2f5eb-477c-4661-999c-9deba6788230}</MetaDataID>
        internal void Save()
        {
            //TableOperation updateOperation = TableOperation.Replace(this);
            //var result = AuthUserRefCloudTable.Execute(updateOperation);
            var result = AuthUserRefCloudTable_a.UpdateEntity(this, Azure.ETag.All, Azure.Data.Tables.TableUpdateMode.Replace);
        }
    }
}