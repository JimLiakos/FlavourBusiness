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
#if !FlavourBusinessDevice

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
        DateTime? _PreparedAt;

        /// <MetaDataID>{ff683fa2-0ccd-42c5-af39-aa072d8fee05}</MetaDataID>
        [PersistentMember(nameof(_PreparedAt))]
        [BackwardCompatibilityID("+20")]
        public DateTime? PreparedAt
        {
            get => _PreparedAt;
            set
            {
                if (_PreparedAt != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _PreparedAt = value;
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
                        _State = value;
                        stateTransition.Consistent = true;
                    }
                    ObjectChangeState?.Invoke(this, nameof(State));
#if !FlavourBusinessDevice
                    //if (ClientSession is EndUsers.FoodServiceClientSession)
                    //{
                    //    foreach (var preparationStation in (ClientSession as EndUsers.FoodServiceClientSession).ServicesContextRunTime.PreparationStationRuntimes.Values.OfType<PreparationStationRuntime>())
                    //        preparationStation.OnPreparationItemChangeState(this);
                    //}
#endif


                }
            }
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
            var myStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this);
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                foreach (var optionChange in this._OptionsChanges)
                    myStorage.CommitTransientObjectState(optionChange);

                stateTransition.Consistent = true;
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
        public int NumberOfShares
        {
            get
            {
                if (ClientSession != null)
                    _NumberOfShares = _SharedWithClients.Count + 1;

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

        /// <MetaDataID>{da441ccb-5f5b-4ee2-8c2b-1ce95b5a5b19}</MetaDataID>
        public IMenuItem LoadMenuItem()
        {
            if (_MenuItem == null && !string.IsNullOrWhiteSpace(_MenuItemUri))
                _MenuItem = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(_MenuItemUri) as IMenuItem;

            return _MenuItem;
        }

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
                return _OptionsChanges.AsReadOnly();
            }
        }


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
            ;
            bool changed;
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                changed = (_Price != item.Price ||
_Quantity != item.Quantity ||
_IsShared != item.IsShared ||
_NumberOfShares != item.NumberOfShares ||
_SelectedMealCourseTypeUri != item.SelectedMealCourseTypeUri);

                _Price = item.Price;
                _Quantity = item.Quantity;
                _IsShared = item.IsShared;
                _NumberOfShares = item.NumberOfShares;
                _CustomItemEnabled = item.CustomItemEnabled;
                _SelectedMealCourseTypeUri = item.SelectedMealCourseTypeUri;

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

        /// <MetaDataID>{74e7c1c6-6831-4122-89fe-dc4336cee82f}</MetaDataID>
        public List<string> SharedInSessions
        {
            get
            {
                var shareInSessions = SharedWithClients.Select(x => x.SessionID).ToList();
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

        public bool IsCooked { get; set; }

        ///// <MetaDataID>{fdd3e18e-43b8-4c77-80b1-d7a17c1a9c8b}</MetaDataID>
        //public string Timestamp;
    }


}