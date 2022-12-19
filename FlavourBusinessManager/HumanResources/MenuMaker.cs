using FlavourBusinessFacade;
using FlavourBusinessFacade.HumanResources;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using OOAdvantech.Remoting.RestApi;
using System.Security.Authentication;
using OOAdvantech.PersistenceLayer;

namespace FlavourBusinessManager.HumanResources
{
    /// <MetaDataID>{e7d6524c-f870-44cd-a1a9-b3ce0a938cf1}</MetaDataID>
    [BackwardCompatibilityID("{e7d6524c-f870-44cd-a1a9-b3ce0a938cf1}")]
    [Persistent()]
    public class MenuMaker : MarshalByRefObject, IMenuMaker, IUser//, IUploadService
    {
        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;


        /// <exclude>Excluded</exclude>
        string _OAuthUserIdentity;

        /// <MetaDataID>{ad2f85c4-fcbf-48d3-82c0-dd209474a67e}</MetaDataID>
        [PersistentMember(nameof(_OAuthUserIdentity))]
        [BackwardCompatibilityID("+9")]
        public string OAuthUserIdentity
        {
            get => _OAuthUserIdentity;
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
        System.Collections.Generic.List<FlavourBusinessFacade.UserData.UserRole> _Roles;
        /// <MetaDataID>{04f7d6c9-70be-4994-9f11-dd0e6414bfa4}</MetaDataID>
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
        OOAdvantech.Collections.Generic.Set<IAccountability> _Responsibilities = new OOAdvantech.Collections.Generic.Set<IAccountability>();

        /// <MetaDataID>{1c168d0d-1f48-4a46-8938-16e19f732967}</MetaDataID>
        [PersistentMember(nameof(_Responsibilities))]
        [BackwardCompatibilityID("+10")]
        public List<IAccountability> Responsibilities => _Responsibilities.ToThreadSafeList();

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{83595f2e-4e44-4d58-ab3e-f51e3174a7f0}</MetaDataID>
        string _Name;

        /// <MetaDataID>{a384b455-eda8-46eb-929a-2ca161c75630}</MetaDataID>
        [OOAdvantech.MetaDataRepository.PersistentMember(nameof(_Name))]
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+2")]
        public string Name
        {
            get => _Name;
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
        string _PhotoUrl;

        /// <MetaDataID>{f642a8e1-a9c4-4c35-8c90-b9c2aa4daa57}</MetaDataID>
        [OOAdvantech.MetaDataRepository.PersistentMember(nameof(_PhotoUrl))]
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+3")]
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

        /// <exclude>Excluded</exclude>
        string _FullName;


        /// <MetaDataID>{a326db70-39e2-472f-ade0-5f711ebc6761}</MetaDataID>
        [OOAdvantech.MetaDataRepository.PersistentMember(nameof(_FullName))]
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+4")]
        public string FullName
        {
            get => _FullName;
            set
            {
                if (_FullName != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _FullName = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        string _Email;

        /// <MetaDataID>{e9fbf225-92c1-4243-8cc6-a9159701c3fa}</MetaDataID>
        [OOAdvantech.MetaDataRepository.PersistentMember(nameof(_Email))]
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+5")]
        public string Email
        {
            get => _Email;
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
        string _PhoneNumber;

        /// <MetaDataID>{ba3fe4b1-26f4-4935-9872-1ecbe89174b2}</MetaDataID>
        [OOAdvantech.MetaDataRepository.PersistentMember(nameof(_PhoneNumber))]
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+6")]
        public string PhoneNumber
        {
            get => _PhoneNumber;
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


        /// <exclude>Excluded</exclude>
        string _UserName;


        /// <MetaDataID>{6538ba2d-137f-4616-8651-4ecba7b3a05e}</MetaDataID>
        [OOAdvantech.MetaDataRepository.PersistentMember(nameof(_UserName))]
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+7")]
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

        /// <exclude>Excluded</exclude>
        string _Identity;

        /// <MetaDataID>{dd8bad00-13e9-4c04-a0de-91bbefaa277f}</MetaDataID>
        protected MenuMaker()
        {

        }
        /// <MetaDataID>{8d19c86d-ba14-4f8e-bcf7-0331b1c21ca5}</MetaDataID>
        public MenuMaker(string signUpUserIdentity)
        {
            _OAuthUserIdentity = signUpUserIdentity;
            _Identity = Guid.NewGuid().ToString("N");
        }

        /// <MetaDataID>{623016e9-c977-4762-b02a-2f677233e1b3}</MetaDataID>
        [OOAdvantech.MetaDataRepository.PersistentMember(nameof(_Identity))]
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+8")]
        public string Identity
        {
            get
            {
                if (_Identity == null)
                {

                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Identity = Guid.NewGuid().ToString("N");
                        stateTransition.Consistent = true;
                    }
                }
                return _Identity;
            }
        }

        /// <MetaDataID>{fb36af09-65af-4948-821f-b438460780aa}</MetaDataID>
        [BackwardCompatibilityID("+11")]
        public List<IAccountability> Commissions => new List<IAccountability>();


        /// <MetaDataID>{a7bab3fa-b22c-4f46-94df-3bf602e05fc0}</MetaDataID>
        public IActivity NewMenuDesignActivity(IAccountability menuMakingAccountability, string subjectDescription, DesignSubjectType selectedTranslationType, string subjcectIdentity)
        {
            MenuDesignActivity menuMakingActivity = menuMakingAccountability.Activities.OfType<MenuDesignActivity>().Where(x => x.DesigneSubjectIdentity == subjcectIdentity).FirstOrDefault();

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                if (menuMakingActivity == null)
                {
                    menuMakingActivity = new MenuDesignActivity();
                    OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(menuMakingActivity);
                    menuMakingAccountability.AddActivity(menuMakingActivity);
                }
                menuMakingActivity.Name = subjectDescription;
                menuMakingActivity.DesignActivityType = selectedTranslationType;
                menuMakingActivity.DesigneSubjectIdentity = subjcectIdentity;

                stateTransition.Consistent = true;
            }


            return menuMakingActivity;
        }


        /// <MetaDataID>{7a64d377-6a1d-43f8-aa90-dcee138c2a33}</MetaDataID>
        public OrganizationStorageRef GetStorage(OrganizationStorages dataType)
        {
            AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
            if (authUser == null)
                throw new AuthenticationException();

            bool authorized = false;
            if (authUser != null)
            {
                AuthUserRef authUserRef = AuthUserRef.GetAuthUserRef(authUser, false);
                if (authUserRef.GetContextRoleObject<MenuMaker>() == this)
                    authorized= true;
            }
            if (!authorized) //authUser.User_ID != this.SignUpUserIdentity)
                throw new InvalidCredentialException("The user " + authUser.Name + " isn't recognized as menu maker owner.");

            if (dataType == OrganizationStorages.StyleSheets)
                return Organization.StyleSheets;
            else if (dataType == OrganizationStorages.HeadingAccents)
                return Organization.HeadingAccents;
            else if (dataType == OrganizationStorages.Borders)
                return Organization.Borders;
            else if (dataType == OrganizationStorages.BackgroundImages)
                return Organization.BackgroundImages;

            return null;
        }



        /// <MetaDataID>{4dbd6a1e-46f1-4cc7-996d-905f7940a424}</MetaDataID>
        public OrganizationStorageRef GetGraphicMenu(IActivity menuMakingActivity)
        {
            IResourceManager organization = menuMakingActivity.Accountability.Commissioner as IResourceManager;
            if ((menuMakingActivity as MenuDesignActivity).DesignActivityType == DesignSubjectType.Menu)
            {
                OrganizationStorageRef graphicMenu = organization.GraphicMenus.Where(x => x.StorageIdentity == (menuMakingActivity as MenuDesignActivity).DesigneSubjectIdentity).FirstOrDefault();
                graphicMenu.UploadService = organization as IUploadService;
                return graphicMenu;
            }
            return null;
        }
        /// <MetaDataID>{02ea7e15-59d7-41ed-a2f1-f96bbc5558e9}</MetaDataID>
        public OrganizationStorageRef GetGraphicMenuItems(IActivity menuMakingActivity)
        {
            IResourceManager organization = menuMakingActivity.Accountability.Commissioner as IResourceManager;
            if ((menuMakingActivity as MenuDesignActivity).DesignActivityType == DesignSubjectType.Menu)
            {
                OrganizationStorageRef graphicMenu = organization.GraphicMenus.Where(x => x.StorageIdentity == (menuMakingActivity as MenuDesignActivity).DesigneSubjectIdentity).FirstOrDefault();
                if (graphicMenu != null)
                {
                    var graphicMenuItems = organization.GetStorage(OrganizationStorages.RestaurantMenus);
                    graphicMenuItems.UploadService = organization as IUploadService;
                    return graphicMenuItems;
                }

            }
            return null;
        }


    }
}