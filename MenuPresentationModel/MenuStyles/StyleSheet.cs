using System;
using System.Collections.Generic;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Transactions;
using System.Linq;
using OOAdvantech;

namespace MenuPresentationModel.MenuStyles
{
    /// <MetaDataID>{a7553c39-0fe6-4802-bd6b-564b0e912294}</MetaDataID>
    [BackwardCompatibilityID("{a7553c39-0fe6-4802-bd6b-564b0e912294}")]
    [Persistent()]
    public class StyleSheet : MarshalByRefObject, IStyleSheet, IObjectStateEventsConsumer
    {

        public event ObjectChangeStateHandle ObjectChangeState;

        /// <exclude>Excluded</exclude>
        string _StyleSheetIdentity;

        /// <MetaDataID>{c1ed16b5-f8f2-487b-898b-0c95091ba7e4}</MetaDataID>
        [PersistentMember("_StyleSheetIdentity")]
        [BackwardCompatibilityID("+4")]
        public string StyleSheetIdentity
        {
            get
            {
                return _StyleSheetIdentity;
            }

            set
            {

                if (_StyleSheetIdentity != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _StyleSheetIdentity = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }


        /// <exclude>Excluded</exclude>
        string _OrgStyleSheetIdentity;

        /// <MetaDataID>{a5e04137-133b-468b-9685-c60eaa49550b}</MetaDataID>
        [PersistentMember("_OrgStyleSheetIdentity")]
        [BackwardCompatibilityID("+3")]
        protected string OrgStyleSheetIdentity
        {
            get
            {
                return _OrgStyleSheetIdentity;
            }
            set
            {

                if (_OrgStyleSheetIdentity != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _OrgStyleSheetIdentity = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <MetaDataID>{bfedd247-1095-4a7b-812b-233772963925}</MetaDataID>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <exclude>Excluded</exclude>
        string _Name;
        /// <MetaDataID>{b76a4c1b-510e-448c-b689-99d6e3aec8e8}</MetaDataID>
        [PersistentMember(nameof(_Name))]
        [BackwardCompatibilityID("+1")]
        public string Name
        {
            get
            {
                if (_OrgStyleSheet != null && _Name == null)
                    return _OrgStyleSheet.Name;
                return _Name;
            }
            set
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _Name = value;
                    stateTransition.Consistent = true;
                }

            }
        }

        /// <MetaDataID>{e950a440-ed19-4aa8-bd74-9495a6c4a0f2}</MetaDataID>
        public StyleSheet CreateDerivedStyleSheet()
        {
            return new StyleSheet(this);
        }

        /// <MetaDataID>{4c68890f-67ae-4636-a779-56d0bf366fed}</MetaDataID>
        public StyleSheet()
        {
            _StyleSheetIdentity = Guid.NewGuid().ToString();
        }
        /// <exclude>Excluded</exclude>
        StyleSheet _OrgStyleSheet;

        /// <MetaDataID>{57fe8e8c-3165-49b4-b54b-9855c6a1ce3b}</MetaDataID>
        public StyleSheet OrgStyleSheet
        {
            get
            {
                return _OrgStyleSheet;
            }
        }



        /// <MetaDataID>{85b70213-095e-41ec-a8fd-dc7ab3073eab}</MetaDataID>
        public StyleSheet(StyleSheet orgStyleSheet)
        {
            _StyleSheetIdentity = Guid.NewGuid().ToString();
            _OrgStyleSheet = orgStyleSheet;

            _OrgStyleSheetIdentity = _OrgStyleSheet.StyleSheetIdentity;

            var clonedStyles = (from orgStyle in orgStyleSheet._Styles
                                select orgStyle.GetDerivedStyle());
            foreach (var style in clonedStyles)
                _Styles.Add(style);
        }

        /// <MetaDataID>{ee1fe5c9-fa28-4988-b1c9-e2d8551f9d4d}</MetaDataID>
        public void ChangeOrgStyle(StyleSheet orgStyleSheet)
        {
            _OrgStyleSheet = orgStyleSheet;

            OrgStyleSheetIdentity = _OrgStyleSheet.StyleSheetIdentity;

            foreach (var style in orgStyleSheet._Styles)
                Styles[style.Name].ChangeOrgStyle(style);

        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<IStyleRule> _Styles = new OOAdvantech.Collections.Generic.Set<IStyleRule>();
        /// <MetaDataID>{5f2a1d95-1061-4928-b208-b776d0b2d035}</MetaDataID>

        [IgnoreErrorCheck()]
        [PersistentMember(nameof(_Styles))]
        [BackwardCompatibilityID("+2")]
        public System.Collections.Generic.IDictionary<string, IStyleRule> Styles
        {

            get
            {
                return _Styles.AsReadOnly().ToDictionary(style => style.Name);
            }
        }

        /// <MetaDataID>{b2513d92-2170-41d3-bc06-9a12683a12e7}</MetaDataID>
        public void AddStyle(IStyleRule style)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _Styles.Add(style);
                stateTransition.Consistent = true;
            }
        }

        /// <MetaDataID>{b4e71fd3-dd62-4bbd-894e-ea958d5d9350}</MetaDataID>
        public void RemoveStyle(IStyleRule style)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _Styles.Remove(style);
                stateTransition.Consistent = true;
            }
        }


        /// <exclude>Excluded</exclude> 
        static ObjectStorage _ObjectStorage;

        /// <MetaDataID>{b7c4970f-f223-4d42-b9fe-92bf1d3cc304}</MetaDataID>
        public static IStyleSheet GetStyleSheet(string name)
        {
            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(ObjectStorage);
            //_Menus = (from menu in storage.GetObjectCollection<MenuModel.IMenu>() select new MenuViewModel(menu, this) ).ToDictionary(menuViewModel=>menuViewModel.Menu);
            return (from styleSheet in storage.GetObjectCollection<IStyleSheet>()
                    where styleSheet.Name == name
                    select styleSheet).FirstOrDefault();
            //_Menus = (from menu in storageMenus select new MenuViewModel(menu, this)).ToDictionary(menuViewModel => menuViewModel.Menu);
            return null;
        }

        /// <exclude>Excluded</exclude>
        public static ObjectStorage ObjectStorage
        {
            get
            {

                if (_ObjectStorage == null)
                {


                    string appDataPath = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Microneme";
                    if (!System.IO.Directory.Exists(appDataPath))
                        System.IO.Directory.CreateDirectory(appDataPath);
                    appDataPath += "\\DontWaitWater";
                    if (!System.IO.Directory.Exists(appDataPath))
                        System.IO.Directory.CreateDirectory(appDataPath);
                    string storageLocation = appDataPath + "\\StyleSheets.xml";

                    try
                    {
                        _ObjectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.OpenStorage("StyleSheets", storageLocation, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                    }
                    catch (StorageException error)
                    {

                        if (error.Reason == StorageException.ExceptionReason.StorageDoesnotExist)
                        {
                            _ObjectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.NewStorage("StyleSheets",
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
                return _ObjectStorage;
            }
            set
            {
                _ObjectStorage = value;
            }
        }

        /// <MetaDataID>{37d012d8-3f41-4f65-95f4-9929b47ab0f5}</MetaDataID>
        public static IStyleSheet NewStyleSheet(string styleName)
        {


            StyleSheet styleSheet = new StyleSheet();
            styleSheet.Name = styleName;
            ObjectStorage.CommitTransientObjectState(styleSheet);

            return styleSheet;
        }

        /// <MetaDataID>{a9b48d81-e645-430e-8fa6-1dbdc2e4b62f}</MetaDataID>
        public void OnCommitObjectState()
        {
            var objectStorage = ObjectStorage.GetStorageOfObject(this);
            foreach (var style in _Styles)
                objectStorage.CommitTransientObjectState(style);
        }
        public void BeforeCommitObjectState()
        {
        }
        /// <MetaDataID>{a82eaab5-97ad-42da-ae41-aae0787a07f5}</MetaDataID>
        public void OnActivate()
        {
            if (!string.IsNullOrWhiteSpace(OrgStyleSheetIdentity))
            {
                _OrgStyleSheet = (from styleSheet in MenuPresentationModel.MenuStyles.StyleSheet.StyleSheets
                                  where styleSheet.StyleSheetIdentity == OrgStyleSheetIdentity
                                  select styleSheet).FirstOrDefault() as StyleSheet;

                foreach (var style in _OrgStyleSheet._Styles)
                    Styles[style.Name].ChangeOrgStyle(style);
            }
            else
            {

                var styles = Styles;
                if (!styles.ContainsKey("order-pad"))
                {
                    OrderPadStyle orderPadStyle = new OrderPadStyle();
                    orderPadStyle.Name="order-pad";
                    orderPadStyle.Background=styles[]
                }

            }

            foreach (var style in _Styles)
                style.ObjectChangeState += Style_ObjectChangeState;

        }

        private void Style_ObjectChangeState(object _object, string member)
        {


            ObjectChangeState?.Invoke(_object, null);
        }

        /// <MetaDataID>{0906f7dc-eb60-4051-be69-1504f4bb0d7b}</MetaDataID>
        public void OnDeleting()
        {

        }

        /// <MetaDataID>{f8fa4d63-93de-4ae7-8a40-189a13d5836d}</MetaDataID>
        public void LinkedObjectAdded(object linkedObject, AssociationEnd associationEnd)
        {

        }

        /// <MetaDataID>{532716f9-9fc0-4dc0-9cf6-3ab8d25fc0c4}</MetaDataID>
        public void LinkedObjectRemoved(object linkedObject, AssociationEnd associationEnd)
        {

        }

        /// <MetaDataID>{bd2ad6b2-f465-4032-b8bb-da3c2ea1c57c}</MetaDataID>
        public static List<MenuPresentationModel.MenuStyles.IStyleSheet> StyleSheets
        {
            get
            {
                OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(ObjectStorage);

                return (from styleSheet in storage.GetObjectCollection<IStyleSheet>()
                        select styleSheet).ToList();
            }

        }

        public static List<StyleSheetRef> StyleSheetRefs
        {
            get
            {
                OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(ObjectStorage);

                return (from styleSheet in storage.GetObjectCollection<IStyleSheet>()
                        select styleSheet).ToList().Select(x => new StyleSheetRef() { Name = x.Name, Uri= ObjectStorage.GetPersistentObjectUri(x) }).ToList();
            }
        }
    }

    /// <MetaDataID>{a2301dd3-a78f-4ffa-8d81-39651277f23a}</MetaDataID>
    public class StyleSheetRef
    {
        public string Name;
        public string Uri;

    }
    //MenuPresentationModel.MenuStyles.StyleSheet.StyleSheets.Select(x=>x.Name).ToArray()
}