using System.Collections.Generic;

using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using System.Linq;
using FlavourBusinessFacade.RoomService;
using MenuModel;
using FlavourBusinessFacade.EndUsers;
using System;
using OOAdvantech.Json;
using FlavourBusinessFacade.ServicesContextResources;
using MenuModel.JsonViewModel;
using OOAdvantech;
using System.Globalization;




#if !FlavourBusinessDevice
using FlavourBusinessManager.ServicesContextResources;
using FlavourBusinessManager.EndUsers;
using FlavourBusinessManager.ServicePointRunTime;
#endif

namespace FlavourBusinessManager.RoomService
{
    /// <MetaDataID>{4f7dfec6-51d2-4207-a807-b8451a94f289}</MetaDataID>
    [Persistent()]
    [BackwardCompatibilityID("{4f7dfec6-51d2-4207-a807-b8451a94f289}")]
    public class ItemPreparation : IItemPreparation
    {
        /// <exclude>Excluded</exclude>
        MultilingualMember<string> _FontUri = new MultilingualMember<string>();

        /// <MetaDataID>{531f7d0d-3d0d-4b9b-a200-8aebbd742268}</MetaDataID>
        [PersistentMember(nameof(_FontUri))]
        [BackwardCompatibilityID("+37")]
        public string FontUri
        {
            get => _FontUri;
            set
            {
                if (_FontUri != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _FontUri.Value = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <MetaDataID>{e235da04-dc4e-4b53-bf79-4fa86fa72d5a}</MetaDataID>
        [BackwardCompatibilityID("+36")]
        public Multilingual MultilingualDescription
        {
            get => new Multilingual(_Description);
            set
            {
                _Description = new MultilingualMember<string>(value);
            }
        }

        /// <exclude>Excluded</exclude>
        MultilingualMember<string> _Description = new MultilingualMember<string>();

        /// <MetaDataID>{ce02a853-1dbd-4d99-8d86-5a4330a06e0c}</MetaDataID>
        [PersistentMember(nameof(_Description))]
        [BackwardCompatibilityID("+35")]
        [JsonIgnore]
        public string Description
        {
            get => _Description.Value;
            set
            {
                if (_Description != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Description.Value = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }



        /// <exclude>Excluded</exclude>
        int _PreparatioOrder;

        /// <MetaDataID>{804344b6-8080-4796-8775-a1eff2cbbc11}</MetaDataID>
        [PersistentMember(nameof(_PreparatioOrder))]
        [TransactionalMember(LockOptions.Shared, nameof(_PreparatioOrder))]
        [BackwardCompatibilityID("+33")]
        public int PreparatioOrder
        {
            get => _PreparatioOrder;
            set
            {
                if (_PreparatioOrder != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this, nameof(PreparatioOrder)))
                    {
                        _PreparatioOrder = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        DateTime? _CookingStartsAt;
        /// <MetaDataID>{cc4d9fdb-0a34-4ae1-bb89-5c28a48bbcdf}</MetaDataID>
        [PersistentMember(nameof(_CookingStartsAt))]
        [BackwardCompatibilityID("+29")]
        public System.DateTime? CookingStartsAt
        {
            get => _CookingStartsAt;
            set
            {
                if (_CookingStartsAt != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _CookingStartsAt = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }




        /// <MetaDataID>{4b0c7c73-d9b6-408c-b50a-3dc74bff17f9}</MetaDataID>
        [BackwardCompatibilityID("+30")]
        public double PreparationTimeSpanInMin { get; set; } = -1;


        /// <MetaDataID>{1315d6ad-a380-49fd-946f-a1274fcd0f6c}</MetaDataID>
        [BackwardCompatibilityID("+31")]
        public double CookingTimeSpanInMin { get; set; } = -1;


        /// <exclude>Excluded</exclude> 
        DateTime? _PreparationStartsAt;

        /// <MetaDataID>{ff683fa2-0ccd-42c5-af39-aa072d8fee05}</MetaDataID>
        [PersistentMember(nameof(_PreparationStartsAt))]
        [BackwardCompatibilityID("+20")]
        public DateTime? PreparationStartsAt
        {
            get => _PreparationStartsAt;
            set
            {
                if (_PreparationStartsAt != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _PreparationStartsAt = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }





        /// <exclude>Excluded</exclude>
        DateTime? _PreparedAtForecast;

        /// <MetaDataID>{005db153-7c66-451b-aae7-165d02d8a42c}</MetaDataID>
        [PersistentMember(nameof(_PreparedAtForecast))]
        [BackwardCompatibilityID("+21")]
        public DateTime? PreparedAtForecast
        {
            get => _PreparedAtForecast;
            set
            {
                if (_PreparedAtForecast != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _PreparedAtForecast = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }



        /// <exclude>Excluded</exclude>
        string _Comments;

        /// <MetaDataID>{166ef5d3-57c2-4b7e-8aa5-8ca9c5c2d4be}</MetaDataID>
        [PersistentMember(nameof(_Comments))]
        [BackwardCompatibilityID("+19")]
        public string Comments
        {
            get => _Comments;
            set
            {
                if (_Comments != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Comments = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        string _SelectedMealCourseTypeUri;

        /// <MetaDataID>{46568c98-10d1-4979-9ea1-9736b02f692c}</MetaDataID>
        [PersistentMember(nameof(_SelectedMealCourseTypeUri))]
        [BackwardCompatibilityID("+17")]
        public string SelectedMealCourseTypeUri
        {
            get => _SelectedMealCourseTypeUri;
            set
            {

                if (_SelectedMealCourseTypeUri != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _SelectedMealCourseTypeUri = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <exclude>Excluded</exclude>
        bool _CustomItemEnabled;

        /// <MetaDataID>{fe1db4d8-6f4a-4cd7-8b2c-2cfd3e64cf06}</MetaDataID>
        [PersistentMember(nameof(_CustomItemEnabled))]
        [BackwardCompatibilityID("+15")]
        public bool CustomItemEnabled
        {
            get => _CustomItemEnabled;
            set
            {
                if (_CustomItemEnabled != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _CustomItemEnabled = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }


        /// <exclude>Excluded</exclude>
        DateTime _StateTimestamp;

        /// <MetaDataID>{7331358c-9d50-4cbc-b085-3758e9d37c40}</MetaDataID>
        [PersistentMember(nameof(_StateTimestamp))]
        [BackwardCompatibilityID("+14")]
        public DateTime StateTimestamp
        {
            get => _StateTimestamp;
            set
            {

                if (_StateTimestamp != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _StateTimestamp = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <exclude>Excluded</exclude>
        ItemPreparationState _State;


        /// <MetaDataID>{5eb3c518-d532-4dc6-bec1-86202b25ea90}</MetaDataID>
        [PersistentMember(nameof(_State))]
        [BackwardCompatibilityID("+12")]
        public ItemPreparationState State
        {
            get => _State;
            set
            {
                if (_State != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        var previousState = _State;
                        _State = value;

                        if (previousState == ItemPreparationState.PreparationDelay)
                        {

                        }
                        if (value == ItemPreparationState.Serving)
                        {

                        }

                        if (_State == ItemPreparationState.InPreparation)
                            _PreparationStartsAt = DateTime.UtcNow;
                        if (_State.IsInPreviousState(ItemPreparationState.InPreparation))
                        {
                            _PreparationStartsAt = null;
                            PreparationTimeSpanInMin = -1;
                        }


                        if (_State == ItemPreparationState.IsRoasting)
                            _CookingStartsAt = DateTime.UtcNow;

                        if (_State.IsInPreviousState(ItemPreparationState.IsRoasting))
                            _CookingStartsAt = null;


                        if (_State.IsIntheSameOrFollowingState(ItemPreparationState.IsPrepared) && previousState.IsInPreviousState(ItemPreparationState.IsPrepared) && PreparationStartsAt != null)
                            PreparationTimeSpanInMin = (DateTime.UtcNow - PreparationStartsAt.Value).TotalMinutes;

                        if (IsCooked && _State.IsIntheSameOrFollowingState(ItemPreparationState.IsPrepared) && previousState.IsInPreviousState(ItemPreparationState.IsPrepared) && CookingStartsAt != null)
                            CookingTimeSpanInMin = (DateTime.UtcNow - CookingStartsAt.Value).TotalMinutes;


                        stateTransition.Consistent = true;
                    }




                    ObjectChangeState?.Invoke(this, nameof(State));





#if !FlavourBusinessDevice
                    //if (ClientSession is EndUsers.FoodServiceClientSession)
                    //{
                    //    foreach (var preparationStation in (ClientSession as EndUsers.FoodServiceClientSession).ServicesContextRunTime.PreparationStationRunTimes.Values.OfType<PreparationStationRuntime>())
                    //        preparationStation.OnPreparationItemChangeState(this);
                    //}
#endif

                }
            }
        }


        /// <MetaDataID>{52d16497-7a11-47e5-9020-8517e3d26493}</MetaDataID>
        public bool IsInFollowingState(ItemPreparationState state)
        {
            //following 
            return ((int)this.State) > ((int)state);
        }
        /// <MetaDataID>{b1802d54-39e2-45cc-b67e-4c8740ae9445}</MetaDataID>
        public bool IsIntheSameOrFollowingState(ItemPreparationState state)
        {
            //following 
            return ((int)this.State) >= ((int)state);
        }

        /// <MetaDataID>{3e7af055-3c4e-4dc6-a201-64a5c6579372}</MetaDataID>
        public bool IsInPreviousState(ItemPreparationState state)
        {
            //previous
            return ((int)this.State) < ((int)state);
        }
        /// <MetaDataID>{3476fc80-8567-4993-b024-5772e5ace32e}</MetaDataID>
        public bool IsInTheSameOrPreviousState(ItemPreparationState state)
        {
            //previous
            return ((int)this.State) <= ((int)state);
        }


        public event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;

        /// <MetaDataID>{b1d5d07f-80ed-4028-9238-bed638d7dd1f}</MetaDataID>
        public ItemPreparation()
        {
        }
        /// <MetaDataID>{a417b157-24b6-4aa1-a041-3f23c3e0f5f9}</MetaDataID>
        public ItemPreparation(string uid, string menuItemUri, string name)
        {
            _MenuItemUri = menuItemUri;
            this.uid = uid;
            _Name = name;
        }

#if !FlavourBusinessDevice
        /// <MetaDataID>{229422b2-6920-4823-9991-2f54133084c1}</MetaDataID>
        public ItemPreparation(IMenuItem menuItem)
        {

            var foodItem = new MenuFoodItem(menuItem, new Dictionary<object, object>());
            _MenuItemUri = foodItem.Uri;
            //this.uid = Guid.NewGuid().ToString("N");
            _uid = Guid.NewGuid().ToString("N");
            _Name = foodItem.Name;
            this._MenuItem = foodItem;
            _Quantity = 1;
        }
#endif

        /// <MetaDataID>{a6cc3ed1-24e7-4416-a62a-b11f0b9bd67a}</MetaDataID>
        [BeforeCommitObjectStateInStorageCall]
        void BeforeCommitObjectState()
        {
            OOAdvantech.PersistenceLayer.ObjectStorage myStorage = null;
            foreach (var optionChange in this._OptionsChanges)
            {
                if (myStorage == null)
                    myStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this);
                myStorage.CommitTransientObjectState(optionChange);
            }
        }




        /// <exclude>Excluded</exclude> 
        bool _IsShared;

        /// <MetaDataID>{d54450f2-2144-4274-876d-414338da1d30}</MetaDataID>
        [PersistentMember(nameof(_IsShared))]
        [BackwardCompatibilityID("+10")]
        public bool IsShared
        {
            get => _IsShared;
            set
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _IsShared = value;
                    stateTransition.Consistent = true;
                }
            }
        }

        /// <exclude>Excluded</exclude>
        string _SessionID;
        /// <MetaDataID>{6ab2c8e5-2293-416c-914a-ac9cefc63b7b}</MetaDataID>
        public string SessionID
        {
            get
            {
                if (_SessionID == null && ClientSession != null)
                    _SessionID = ClientSession.SessionID;

                return _SessionID;
            }
            set
            {
                if (ClientSession == null)
                    _SessionID = value;
            }
        }


        /// <exclude>Excluded</exclude>
        int? _NumberOfShares;
        /// <MetaDataID>{804375b2-47fe-4846-9c45-ce0ff2c7b239}</MetaDataID>
        [PersistentMember(nameof(_NumberOfShares))]
        [TransactionalMember(LockOptions.Shared, nameof(_NumberOfShares))]
        [BackwardCompatibilityID("+32")]
        public int NumberOfShares
        {
            get
            {
                if (ClientSession != null)
                {
                    if (_NumberOfShares == null)
                    {
                        using (ObjectStateTransition stateTransition = new ObjectStateTransition(this, nameof(NumberOfShares)))
                        {
                            _NumberOfShares = _SharedWithClients.Count + 1;
                            stateTransition.Consistent = true;
                        }
                    }
                }

                if (_NumberOfShares == null)
                    return 1;
                return _NumberOfShares.Value;
            }
            set
            {
                if (ClientSession == null)
                    _NumberOfShares = value;
            }
        }

        /// <MetaDataID>{a3dfa875-e9b1-4fae-9656-c09968bd587f}</MetaDataID>
        [ObjectsLinkCall]
        void ObjectsLink(object linkedObject, AssociationEnd associationEnd, bool added)
        {
            if (associationEnd.Name == nameof(SharedWithClients))
            {
                _NumberOfShares = null;
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _NumberOfShares = NumberOfShares;
                    stateTransition.Consistent = true;
                }


            }
        }

        /// <exclude>Excluded</exclude>
        [JsonIgnore]
        IMenuItem _MenuItem;



        //#if !FlavourBusinessDevice

        //        /// <MetaDataID>{85d453b1-7f6e-4097-bd7b-151d6cbe28f9}</MetaDataID>
        //        public IMenuItem MenuItem { get => _MenuItem; }
        //#else
        /// <MetaDataID>{17b276e1-5002-494a-8b82-13c417ef80af}</MetaDataID>
        [JsonIgnore]
        /// <MetaDataID>{85d453b1-7f6e-4097-bd7b-151d6cbe28f9}</MetaDataID>
        public IMenuItem MenuItem { get => _MenuItem; }
        //#endif

#if !FlavourBusinessDevice
        /// <MetaDataID>{da441ccb-5f5b-4ee2-8c2b-1ce95b5a5b19}</MetaDataID>
        public IMenuItem LoadMenuItem()
        {
            if (_MenuItem == null && !string.IsNullOrWhiteSpace(_MenuItemUri))
            {
                _MenuItem = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(_MenuItemUri) as IMenuItem;

                var optionsDictionary = (from itemType in _MenuItem.Types.OfType<MenuItemType>()
                                         from option in itemType.GetAllScaledOptions()
                                         select option).ToDictionary(x => x.Uri);

                foreach (var optionChange in this.OptionsChanges.OfType<OptionChange>())
                {
                    PreparationScaledOption option = null;
                    if (optionsDictionary.TryGetValue(optionChange.OptionUri, out option))
                    {
                        optionChange.Option = option;
                        optionChange.itemSpecificOption = _MenuItem.OptionsMenuItemSpecifics.Where(x => x.Option == option).FirstOrDefault();
                    }
                }
            }
            return _MenuItem;
        }

        /// <MetaDataID>{bb55947a-360a-4242-a5e8-3bf3264f8059}</MetaDataID>
        //        internal void UpdateMultiligaulFields()
        //        {
        //            if (_MenuItem == null)
        //                LoadMenuItem();

        //            if (_MenuItem != null)
        //            {
        //                var optionsDictionary = (from itemType in _MenuItem.Types.OfType<MenuItemType>()
        //                                         from option in itemType.GetAllScaledOptions()
        //                                         select option).ToDictionary(x => x.Uri);

        //                Description=FullName;

        //                foreach (var optionChange in this.OptionsChanges.OfType<OptionChange>())
        //                {
        //                    if (optionChange.Option==null)
        //                    {
        //                        PreparationScaledOption option = null;
        //                        if (optionsDictionary.TryGetValue(optionChange.OptionUri, out option))
        //                        {
        //                            optionChange.Option = option;
        //                            optionChange.itemSpecificOption = _MenuItem.OptionsMenuItemSpecifics.Where(x => x.Option == option).FirstOrDefault();
        //                        }
        //                    }
        //                    if (optionChange.Option!=null)
        //                    {
        //                        bool checkUncheck = (optionChange.Option.LevelType is FixedScaleType)&&(optionChange.Option.LevelType as FixedScaleType).UniqueIdentifier==FixedScaleTypes.CheckUncheck;

        //                        int levelIndex = optionChange.Option.LevelType.Levels.IndexOf(optionChange.NewLevel);
        //                        if (levelIndex<2)
        //                            optionChange.Description=optionChange.Option.FullName;
        //                        else
        //                            optionChange.Description=optionChange.NewLevel.Name+" "+optionChange.Option.FullName;

        //                    }
        //                }
        //            }
        //            //_MenuItem.OptionsMenuItemSpecifics
        //        }


#endif



        /// <MetaDataID>{08cd685b-2f14-4812-ab5f-af170874b642}</MetaDataID>
        public IMenuItem LoadMenuItem(Dictionary<string, MenuFoodItem> menuItems)
        {
            if (_MenuItem == null && !string.IsNullOrWhiteSpace(_MenuItemUri))
            {
                _MenuItem = menuItems[_MenuItemUri] as IMenuItem;
                var optionsDictionary = (from options in (_MenuItem as MenuFoodItem).ItemOptions
                                         from option in options.Options
                                         select option).ToDictionary(x => x.Uri);


                foreach (var optionChange in OptionsChanges.OfType<OptionChange>())
                {
                    Option option = null;
                    if (optionsDictionary.TryGetValue(optionChange.OptionUri, out option))
                    {
                        var itemSpecific = new MenuModel.JsonViewModel.OptionMenuItemSpecific();
                        itemSpecific.Option = option;
                        itemSpecific.InitialLevel = option.Initial;
                        optionChange.itemSpecificOption = itemSpecific;


                        //optionChange.itemSpecificOption.Option.LevelType.ZeroLevelScaleType

                    }
                    //optionChange.OptionUri
                }


            }

            return _MenuItem;
        }
        /// <MetaDataID>{b4056bc7-a1f1-485f-adc1-22386bdc8e34}</MetaDataID>
        [CommitObjectStateInStorageCall]
        void CommitObjectState()
        {

        }
        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<IOptionChange> _OptionsChanges = new OOAdvantech.Collections.Generic.Set<IOptionChange>();


        /// <MetaDataID>{a5ba7a5a-dfd3-4253-ab1e-fb2bd88f7dfa}</MetaDataID>
        [PersistentMember(nameof(_OptionsChanges))]
        [BackwardCompatibilityID("+8")]
        public System.Collections.Generic.IList<FlavourBusinessFacade.RoomService.IOptionChange> OptionsChanges
        {
            set
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    if (value != null)
                    {
                        foreach (var optionChange in _OptionsChanges.ToList())
                        {
                            if (!value.Contains(optionChange))
                                _OptionsChanges.Remove(optionChange);
                        }
                        foreach (var optionChange in value.ToList())
                        {
                            if (!_OptionsChanges.Contains(optionChange))
                                _OptionsChanges.Add(optionChange);
                        }
                    }
                    else
                    {
                        _OptionsChanges.Clear();
                    }
                    stateTransition.Consistent = true;
                }


            }
            get
            {
                return _OptionsChanges.ToThreadSafeList();
            }
        }

#if !FlavourBusinessDevice
        /// <MetaDataID>{a12bbfc9-246b-43ba-8f61-d61f589d0806}</MetaDataID>
        public void EnsurePresentationFor(CultureInfo culture)
        {
            if (this.ClientSession != null)
            {


                using (OOAdvantech.CultureContext context = new OOAdvantech.CultureContext(culture, true))
                {
                    MenuPresentationModel.MenuCanvas.IMenuCanvasFoodItem menuCanvasFoodItem = (this.ClientSession as FoodServiceClientSession).GraphicMenu.GetMenuCanvasFoodItem(MenuItemUri);
                    string fontUri = menuCanvasFoodItem.Font.Uri;

                    this.FontUri = fontUri;

                    if (_MenuItem == null)
                        LoadMenuItem();

                    if (_MenuItem != null)
                    {
                        var optionsDictionary = (from itemType in _MenuItem.Types.OfType<MenuItemType>()
                                                 from option in itemType.GetAllScaledOptions()
                                                 select option).ToDictionary(x => x.Uri);

                        Description = FullName;

                        foreach (var optionChange in this.OptionsChanges.OfType<OptionChange>())
                        {
                            if (optionChange.Option == null)
                            {
                                PreparationScaledOption option = null;
                                if (optionsDictionary.TryGetValue(optionChange.OptionUri, out option))
                                {
                                    optionChange.Option = option;
                                    optionChange.itemSpecificOption = _MenuItem.OptionsMenuItemSpecifics.Where(x => x.Option == option).FirstOrDefault();
                                }
                            }
                            if (optionChange.Option != null)
                            {
                                bool checkUncheck = (optionChange.Option.LevelType is FixedScaleType) && (optionChange.Option.LevelType as FixedScaleType).UniqueIdentifier == FixedScaleTypes.CheckUncheck;

                                int levelIndex = optionChange.Option.LevelType.Levels.IndexOf(optionChange.NewLevel);
                                if (levelIndex < 2)
                                    optionChange.Description = optionChange.Option.FullName;
                                else
                                    optionChange.Description = optionChange.NewLevel.Name + " " + optionChange.Option.FullName;

                            }
                        }
                    }
                }

            }
        }
#endif

        /// <exclude>Excluded</exclude>
        string _MenuItemUri;
        /// <MetaDataID>{4f4593df-d159-431b-a73b-72fbe3e2de6f}</MetaDataID>
        [PersistentMember(nameof(_MenuItemUri))]
        [BackwardCompatibilityID("+1")]
        public string MenuItemUri
        {
            get
            {
                return _MenuItemUri;
            }
            set
            {

                if (_MenuItemUri != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _MenuItemUri = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }


        /// <exclude>Excluded</exclude>
        string _uid;

        /// <MetaDataID>{8323bddc-f359-407c-80f5-10ed0c701705}</MetaDataID>
        [PersistentMember(nameof(_uid))]
        [BackwardCompatibilityID("+2")]
        public string uid
        {
            get
            {
                return _uid;
            }
            set
            {

                if (_uid != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _uid = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        double _Quantity;
        /// <MetaDataID>{db39c1aa-f271-42cd-aed2-c21372af95e7}</MetaDataID>
        [PersistentMember(nameof(_Quantity))]
        [BackwardCompatibilityID("+3")]
        public double Quantity
        {
            get
            {
                return _Quantity;
            }
            set
            {

                if (_Quantity != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Quantity = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }


        /// <exclude>Excluded</exclude>
        string _ISOCurrencySymbol;

        /// <MetaDataID>{987e8ebc-f208-4831-8720-826a9a74ecaa}</MetaDataID>
        [PersistentMember(nameof(_ISOCurrencySymbol))]
        [BackwardCompatibilityID("+4")]
        public string ISOCurrencySymbol
        {
            get
            {
                return _ISOCurrencySymbol;
            }
            set
            {

                if (_ISOCurrencySymbol != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ISOCurrencySymbol = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        ///// <exclude>Excluded</exclude>
        //List<OptionChange> _OptionsChanges;
        ///// <MetaDataID>{e6c28994-e47b-4188-9ba8-00b144a8102b}</MetaDataID>
        //public List<OptionChange> OptionsChanges
        //{
        //    get
        //    {

        //        return _OptionsChanges;
        //    }
        //    set
        //    {
        //        _OptionsChanges = value;
        //    }
        //}

        /// <exclude>Excluded</exclude>
        double _Price;
        /// <MetaDataID>{984b3805-b650-480b-8698-69c5b6a8dee9}</MetaDataID>
        [PersistentMember(nameof(_Price))]
        [BackwardCompatibilityID("+5")]
        public double Price
        {
            get
            {
                return _Price;
            }
            set
            {

                if (_Price != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Price = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        double _ModifiedItemPrice;

        /// <MetaDataID>{ac3d655d-d6f6-41c4-8b14-26e707ae5a76}</MetaDataID>
        [PersistentMember(nameof(_ModifiedItemPrice))]
        [BackwardCompatibilityID("+34")]
        public double ModifiedItemPrice
        {
            get => _ModifiedItemPrice;
            set
            {
                if (_ModifiedItemPrice != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ModifiedItemPrice = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }



        //PaidAmount


        /// <exclude>Excluded</exclude>
        [JsonIgnore]
        string _Name;
        /// <MetaDataID>{13715b98-1b1f-488c-aaa3-d333aae9d4f5}</MetaDataID>
        [PersistentMember(nameof(_Name))]
        [BackwardCompatibilityID("+6")]
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

        /// <MetaDataID>{ecaa0fc6-d491-4b06-a904-c07f2222d96e}</MetaDataID>
        public string FullName
        {
            get
            {
                string name = Name;
                if (_MenuItem != null)
                {
                    name = _MenuItem.FullName;
                    if (string.IsNullOrWhiteSpace(name))
                        name = _MenuItem.Name;
                }
                // x.Option is MenuModel.IPricingContext => ItemSelectorOption

                var optionChange = this.OptionsChanges.OfType<OptionChange>().Where(x => x.PriceDif == 1 && x.Option is MenuModel.IPricingContext).FirstOrDefault();
                if (optionChange != null)
                    return name + " " + optionChange.OptionName;
                else
                    return name;
            }
        }

        /// <exclude>Excluded</exclude>
        string _MenuItemPriceUri;
        /// <MetaDataID>{e71cec68-a8e9-4a45-8559-943ce3e62003}</MetaDataID>
        [PersistentMember(nameof(_MenuItemPriceUri))]
        [BackwardCompatibilityID("+7")]
        public string MenuItemPriceUri
        {
            get
            {
                return _MenuItemPriceUri;
            }
            set
            {

                if (_MenuItemPriceUri != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _MenuItemPriceUri = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.Member<IFoodServiceClientSession> _ClientSession = new OOAdvantech.Member<IFoodServiceClientSession>();

        /// <MetaDataID>{837efcf4-421f-42a2-be0b-a2d6c911a92a}</MetaDataID>
        [PersistentMember(nameof(_ClientSession))]
        //[AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [BackwardCompatibilityID("+9")]
        [CachingOnlyReferenceOnClientSide]
        public FlavourBusinessFacade.EndUsers.IFoodServiceClientSession ClientSession => _ClientSession.Value;


        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<IFoodServiceClientSession> _SharedWithClients = new OOAdvantech.Collections.Generic.Set<IFoodServiceClientSession>();

        /// <MetaDataID>{343765f9-a38a-40e6-be7c-776bad666f11}</MetaDataID>
        [JsonIgnore]
        [PersistentMember(nameof(_SharedWithClients))]
        [BackwardCompatibilityID("+11")]
        public IList<IFoodServiceClientSession> SharedWithClients => _SharedWithClients.AsReadOnly();



        /// <MetaDataID>{2350e3c0-38b2-4f02-8294-77bbff22380c}</MetaDataID>
        public bool Update(ItemPreparation item)
        {
            if (_MenuItemUri != item._MenuItemUri)
                throw new Exception("_MenuItem can't be change");

            bool changed;
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                changed = (_Price != item.Price ||
                        _ModifiedItemPrice != item.ModifiedItemPrice ||
                        _Quantity != item.Quantity ||
                        _IsShared != item.IsShared ||
                        _NumberOfShares != item.NumberOfShares ||
                        _SelectedMealCourseTypeUri != item.SelectedMealCourseTypeUri);

                _Price = item.Price;
                _ModifiedItemPrice = item.ModifiedItemPrice;
                _Quantity = item.Quantity;
                _IsShared = item.IsShared;
                _NumberOfShares = item.NumberOfShares;
                _CustomItemEnabled = item.CustomItemEnabled;
                _SelectedMealCourseTypeUri = item.SelectedMealCourseTypeUri;

                if (item._State == ItemPreparationState.Serving)
                {


                }

                _State = item._State;
                if (_StateTimestamp != item.StateTimestamp)
                {
                    _StateTimestamp = item.StateTimestamp;
                    changed = true;
                }


                List<OptionChange> removedOptions = new List<OptionChange>(_OptionsChanges.OfType<OptionChange>());

                foreach (var optionChange in item.OptionsChanges.OfType<OptionChange>())
                {
                    var existingOpionChange = (from optChange in removedOptions where optChange.OptionUri == (optionChange as OptionChange).OptionUri select optChange).FirstOrDefault();
                    if (existingOpionChange != null)
                    {
                        changed = changed | existingOpionChange.Update(optionChange as OptionChange);

                        removedOptions.Remove(existingOpionChange);
                    }
                    else
                    {
                        OptionChange theOptionChange = new OptionChange(optionChange.OptionUri, this);
                        theOptionChange.Update(optionChange);
                        _OptionsChanges.Add(theOptionChange);
                        changed = true;
                    }
                }


                foreach (var optionChange in removedOptions)
                {
                    _OptionsChanges.Remove(optionChange);
                    changed = true;
                }
                if (_MenuItemPriceUri != item.MenuItemPriceUri)
                    changed = true;
                _MenuItemPriceUri = item.MenuItemPriceUri;
                ObjectChangeState?.Invoke(this, null);
                stateTransition.Consistent = true;
            }
            return changed;
        }

        ///// <MetaDataID>{cb983f29-dd38-491e-9e85-3431eb5c4c1a}</MetaDataID>
        //public void AddPayment(FinanceFacade.IPayment payment)
        //{
        //    throw new NotImplementedException();
        //}

        ///// <MetaDataID>{324aa334-173e-42fd-ae27-0cced1fa6fa0}</MetaDataID>
        //public void RemovePayment(FinanceFacade.IPayment payment)
        //{
        //    throw new NotImplementedException();
        //}

        /// <MetaDataID>{74e7c1c6-6831-4122-89fe-dc4336cee82f}</MetaDataID>
        public List<string> SharedInSessions
        {
            get
            {
                List<string> shareInSessions = new List<string>();
                if (ClientSession == null && NumberOfShares == 0)
                    return shareInSessions;

                if (ClientSession != null && NumberOfShares == 1)
                {
                    shareInSessions.Add(ClientSession.SessionID);
                    return shareInSessions;
                }

                shareInSessions.AddRange(SharedWithClients.Select(x => x.SessionID).ToList());
                if (ClientSession != null)
                    shareInSessions.Add(ClientSession.SessionID);
                return shareInSessions;
            }
        }


        /// <exclude>Excluded</exclude>
        IMealCourse _MealCourse;

        /// <MetaDataID>{3733d472-f996-4129-a609-2571974cec56}</MetaDataID>
        [JsonIgnore]
        [PersistentMember(nameof(_MealCourse))]
        //[AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [BackwardCompatibilityID("+16")]
        public FlavourBusinessFacade.RoomService.IMealCourse MealCourse
        {
            get => _MealCourse;
            set
            {
                if (_MealCourse != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _MealCourse = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        IPreparationStation _PreparationStation;

        /// <MetaDataID>{f8b1250c-e750-4254-80d6-b3dba382d14d}</MetaDataID>
        [JsonIgnore]
        [PersistentMember(nameof(_PreparationStation))]
        [BackwardCompatibilityID("+18")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        public IPreparationStation PreparationStation
        {
            get => _PreparationStation;
            set
            {

                if (_PreparationStation != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _PreparationStation = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }
#if !FlavourBusinessDevice
        /// <MetaDataID>{87e4df68-e4a0-40eb-ba7c-2923a9167991}</MetaDataID>
        public IPreparationStation ActivePreparationStation
        {
            get
            {

                if (PreparationStation?.MainStation != null && !(PreparationStation as PreparationStation).IsActive)
                    return PreparationStation?.MainStation;
                else
                    return PreparationStation;

            }
        }
#else
        public IPreparationStation ActivePreparationStation => null;

#endif

        /// <MetaDataID>{f6245894-8a95-4b52-a9ee-892602fb6ead}</MetaDataID>
        public int AppearanceOrder { get; set; }



        /// <exclude>Excluded</exclude>
        string _CodeCard;


        /// <MetaDataID>{b45fbcc9-e058-4102-b596-0a3e617f669f}</MetaDataID>
        [PersistentMember(nameof(_CodeCard))]
        [BackwardCompatibilityID("+22")]
        public string CodeCard
        {
            get => _CodeCard;
            set
            {
                if (_CodeCard != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _CodeCard = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <MetaDataID>{175b996d-0c6e-4786-b7f0-fcdaae06bf96}</MetaDataID>
        public string TransferTracking { get; set; }

        /// <MetaDataID>{3393c0e9-7404-4097-be16-5d9e678a9bdb}</MetaDataID>
        public bool IsCooked { get; set; }



        /// <exclude>Excluded</exclude>
        bool _InEditState;

        /// <MetaDataID>{460750e3-47e8-4234-977a-4a2a8c539680}</MetaDataID>
        [PersistentMember(nameof(_InEditState))]
        [BackwardCompatibilityID("+38")]
        public bool InEditState
        {
            get => _InEditState; 
            set
            {
                if (_InEditState != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _InEditState = value;
                        stateTransition.Consistent = true;
                    }
                }

                Transaction.RunOnTransactionCompleted(() =>
                {
                    ObjectChangeState?.Invoke(this, nameof(InEditState));
                });


            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.Member<IServingBatch> _ServedInTheBatch = new OOAdvantech.Member<IServingBatch>();

        /// <MetaDataID>{ab76fae1-98fe-4f45-9973-c9dde93e547d}</MetaDataID>
        [PersistentMember(nameof(_ServedInTheBatch))]
        [BackwardCompatibilityID("+23")]
        [JsonIgnore]
        public IServingBatch ServedInTheBatch => _ServedInTheBatch.Value;


        /// <exclude>Excluded</exclude>
        string _TransactionUri;


        /// <MetaDataID>{5b71d7bd-61ea-41b5-9955-2b19d47b3320}</MetaDataID>
        [PersistentMember(nameof(_TransactionUri))]
        [BackwardCompatibilityID("+24")]
        [JsonIgnore]
        public string TransactionUri
        {
            get => _TransactionUri;

            internal set
            {

                if (_TransactionUri != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _TransactionUri = value;
                        stateTransition.Consistent = true;
                    }
                }
                ItemPreparationState mss;

            }
        }

        /// <exclude>Excluded</exclude>
        Dictionary<string, decimal> _PaidAmounts;
        /// <MetaDataID>{ca57372d-75b2-4b8a-9533-7baf1a7bf209}</MetaDataID>
        public Dictionary<string, decimal> PaidAmounts
        {
            get
            {
#if !FlavourBusinessDevice
                if (ClientSession != null)
                {
                    _PaidAmounts = new Dictionary<string, decimal>();
                    List<IFoodServiceClientSession> foodServiceClientSessions = SharedWithClients.ToList();
                    foodServiceClientSessions.Add(ClientSession);
                    var foodServiceClientSessionss = foodServiceClientSessions.Distinct().ToList();

                    foreach (var foodServiceClientSession in foodServiceClientSessions.OfType<FoodServiceClientSession>())
                    {
                        decimal paidAmount = Bill.GetPaidAmount(foodServiceClientSession, this);
                        _PaidAmounts[foodServiceClientSession.SessionID] = paidAmount;
                    }
                }
#endif
                return _PaidAmounts;
            }
            set
            {
                _PaidAmounts = value;
            }
        }

#if !FlavourBusinessDevice
        /// <MetaDataID>{7e458fbc-06a1-4e0e-97e7-ed4ff8d59e11}</MetaDataID>
        [JsonIgnore]
        public bool IsPaid
        {
            get
            {
                return Bill.IsPaid(this);
            }
        }
#endif
        ///// <MetaDataID>{8aade568-ea01-4550-9906-4857220f756b}</MetaDataID>
        //public List<FinanceFacade.IPayment> Payments => new List<FinanceFacade.IPayment>();




        ///// <MetaDataID>{fdd3e18e-43b8-4c77-80b1-d7a17c1a9c8b}</MetaDataID>
        //public string TimeStamp;
    }




}