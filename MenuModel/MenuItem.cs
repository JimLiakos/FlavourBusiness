using System;
using System.Collections.Generic;
using OOAdvantech.Collections.Generic;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using System.Linq;
using FinanceFacade;
using OOAdvantech;
using OOAdvantech.Remoting.RestApi.Serialization;

namespace MenuModel
{
    /// <MetaDataID>{5c1aac35-2956-4ead-a424-435a5781c314}</MetaDataID>
    [BackwardCompatibilityID("{5c1aac35-2956-4ead-a424-435a5781c314}")]
    [Persistent()]
    public class MenuItem : FinanceFacade.ITaxable, IClassified, IMenuItem, OOAdvantech.PersistenceLayer.IObjectStateEventsConsumer
    {

        
        public bool SelectorAlwaysInDescriptionOverriden
        {
            get
            {
                return _SelectorAlwaysInDescription.HasValue;
            }
        }

        /// <exclude>Excluded</exclude>
        bool? _SelectorAlwaysInDescription;
        /// <MetaDataID>{1b741074-ac97-4e93-8ebb-d7f804602fc3}</MetaDataID>
        [PersistentMember(nameof(_SelectorAlwaysInDescription))]
        [BackwardCompatibilityID("+20")]
        public bool SelectorAlwaysInDescription
        {

            get
            {
                ItemSelectorOption ItemSelector = Prices.OfType<MenuItemPrice>().Where(x => x.ItemSelector != null&& x.ItemSelector.OptionGroup is ItemSelectorOptionsGroup).Select(x => x.ItemSelector).FirstOrDefault();
                if (ItemSelector != null && !_SelectorAlwaysInDescription.HasValue)
                    return (ItemSelector.OptionGroup as ItemSelectorOptionsGroup).AlwaysInDescription;
                if (!_SelectorAlwaysInDescription.HasValue)
                    return default(bool);

                return _SelectorAlwaysInDescription.Value;
            }

            set
            {
                if (_SelectorAlwaysInDescription != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _SelectorAlwaysInDescription = value;
                        stateTransition.Consistent = true;
                    }
                    //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectorAlwaysInDescription)));
                }
            }

        }


      


        /// <exclude>Excluded</exclude>
        bool _Stepper;

        /// <MetaDataID>{25cd9ff8-01b0-463e-8df2-0009906bb4bf}</MetaDataID>
        [PersistentMember(nameof(_Stepper))]
        [BackwardCompatibilityID("+18")]
        public bool Stepper
        {
            get => _Stepper;
            set
            {
                if (_Stepper != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Stepper = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        MultilingualMember<string> _PromptForCustom = new MultilingualMember<string>();


        /// <MetaDataID>{8d61343d-dea2-46c1-8186-01a92ef721d6}</MetaDataID>
        [PersistentMember(nameof(_PromptForCustom))]
        [BackwardCompatibilityID("+17")]
        public string PromptForCustom
        {
            get => _PromptForCustom.Value;
            set
            {

                if (_PromptForCustom.Value != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _PromptForCustom.Value = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        MultilingualMember<string> _PromptForDefault = new MultilingualMember<string>();

        /// <MetaDataID>{b1476b7d-533a-48c4-8e11-63efaca841a2}</MetaDataID>
        [PersistentMember(nameof(_PromptForDefault))]
        [BackwardCompatibilityID("+16")]
        public string PromptForDefault
        {
            get => _PromptForDefault;
            set
            {
                if (_PromptForDefault.Value != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _PromptForDefault.Value = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        bool _AllowCustom;

        /// <MetaDataID>{6600bdd8-f866-46a6-857b-f113dca77ef8}</MetaDataID>
        [PersistentMember(nameof(_AllowCustom))]
        [BackwardCompatibilityID("+15")]
        public bool AllowCustom
        {
            get => _AllowCustom;
            set
            {
                if (_AllowCustom != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _AllowCustom = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        public event ObjectChangeStateHandle ObjectChangeState;
        /// <exclude>Excluded</exclude>
        OOAdvantech.MultilingualMember<string> _FullName = new MultilingualMember<string>();

        /// <MetaDataID>{f952788d-b9ea-453a-be0f-3718680cbb86}</MetaDataID>
        [PersistentMember(nameof(_FullName))]
        [BackwardCompatibilityID("+14")]
        public string FullName
        {
            get => _FullName;
            set
            {

                if (_FullName != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        if (string.IsNullOrWhiteSpace(value))
                            _FullName.ClearValue();
                        else
                            _FullName.Value = value;

                        stateTransition.Consistent = true;
                    }
                }
            }
        }





        /// <exclude>Excluded</exclude>
        OOAdvantech.MultilingualMember<string> _ExtrasDescription = new OOAdvantech.MultilingualMember<string>();


        /// <MetaDataID>{e4763b81-229a-4355-9e8f-c17f478f0286}</MetaDataID>
        [PersistentMember(nameof(_ExtrasDescription))]
        [BackwardCompatibilityID("+12")]
        public string ExtrasDescription
        {
            get
            {
                return _ExtrasDescription;
            }
            set
            {
                if (_ExtrasDescription != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ExtrasDescription.Value = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.MultilingualMember<string> _Description = new OOAdvantech.MultilingualMember<string>();

        /// <MetaDataID>{afd37e91-852a-4b35-9374-49542bda5131}</MetaDataID>
        [PersistentMember(nameof(_Description))]
        [BackwardCompatibilityID("+11")]
        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                if (_Description != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {

                        _Description.Value = value;
                        if (string.IsNullOrWhiteSpace(value))
                            _Description.ClearValue();

                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        Set<IOptionMenuItemSpecific> _OptionsMenuItemSpecifics = new Set<IOptionMenuItemSpecific>();

        /// <MetaDataID>{9504c26b-fcbf-48da-a90b-a513b58e38cb}</MetaDataID>
        [PersistentMember(nameof(_OptionsMenuItemSpecifics))]
        [AssociationEndBehavior(PersistencyFlag.CascadeDelete)]
        [BackwardCompatibilityID("+10")]
        public IList<IOptionMenuItemSpecific> OptionsMenuItemSpecifics
        {
            get
            {
                return _OptionsMenuItemSpecifics.AsReadOnly();
            }
        }


        /// <MetaDataID>{ba74fea1-26ab-4a48-b988-0e7d2f7bb49c}</MetaDataID>
        public void RemoveMenuItemPrice(IMenuItemPrice price)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _Prices.Remove(price);
                stateTransition.Consistent = true;
            }
        }

        /// <MetaDataID>{78ec4a3b-6cc9-41b8-8675-459d98c105ad}</MetaDataID>
        public void AddMenuItemPrice(IMenuItemPrice price)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {

                _Prices.Add(price);
                stateTransition.Consistent = true;
            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <MetaDataID>{169fc92b-6f27-48ae-9156-c00dca6ff47f}</MetaDataID>
        public MenuItem()
        {

        }
        /// <exclude>Excluded</exclude>
        Set<MenuModel.IMenuItemType> _Types = new Set<IMenuItemType>();

        /// <MetaDataID>{fbf8d3d9-14d8-4d34-8259-a2fb77d09487}</MetaDataID>
        [PersistentMember("_Types")]
        [BackwardCompatibilityID("+8")]
        public IList<IMenuItemType> Types
        {
            get
            {

                return _Types.AsReadOnly();
            }
        }




        /// <exclude>Excluded</exclude>
        OOAdvantech.MultilingualMember<string> _Name = new OOAdvantech.MultilingualMember<string>();


        /// <MetaDataID>{931f9780-8036-4998-92f4-caac492abea8}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        [PersistentMember(nameof(_Name))]
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                if (_Name.Value != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Name.Value = value;
                        if (string.IsNullOrWhiteSpace(value))
                            _Name.ClearValue();

                        if (DedicatedType != null)
                            DedicatedType.Name = value;

                        stateTransition.Consistent = true;
                    }
                    ObjectChangeState?.Invoke(this, nameof(Name));
                }
            }
        }

        /// <MetaDataID>{f86c0164-cc44-4497-9a6e-2e531aa0e97c}</MetaDataID>
        public Multilingual MultilingualName => new Multilingual(_Name);

        /// <MetaDataID>{2454c63b-19d9-4a91-8f8a-7a0d60434839}</MetaDataID>
        public Multilingual MultilingualFullName => new Multilingual(_FullName);

        /// <MetaDataID>{e8df7cea-35ad-4ad8-adc4-966a5cbdb33b}</MetaDataID>
        public Multilingual MultilingualPromptForCustom => new Multilingual(_PromptForCustom);

        /// <MetaDataID>{0ec002d9-3fb4-401f-bd51-884f6b9f43bb}</MetaDataID>
        public Multilingual MultilingualPromptForDefault => new Multilingual(_PromptForDefault);


        /// <MetaDataID>{a66e34ee-b54d-4be0-9b8b-2794da007c92}</MetaDataID>
        public Multilingual MultilingualDescription => new Multilingual(_Description);


        /// <MetaDataID>{860c08a3-498d-4db4-ae20-dd7ac897361c}</MetaDataID>
        public Multilingual MultilingualExtrasDescription => new Multilingual(_ExtrasDescription);




        ///// <exclude>Excluded</exclude>
        //decimal _Price;
        ///// <MetaDataID>{583ba988-3966-4394-afb0-e96d2a5f82eb}</MetaDataID>
        //[BackwardCompatibilityID("+2")]
        //[PersistentMember("_Price")]
        //public decimal Price
        //{
        //    set
        //    {
        //        if (_Price != value)
        //        {
        //            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
        //            {
        //                _Price = value;
        //                stateTransition.Consistent = true;
        //            }
        //        }
        //    }
        //    get
        //    {
        //        return _Price;
        //    }
        //}

        /// <exclude>Excluded</exclude>
        OOAdvantech.Member<MenuModel.IClass> _Class = new OOAdvantech.Member<MenuModel.IClass>();
        /// <MetaDataID>{e63bf395-f95b-445c-b03e-bb0b7ffec49b}</MetaDataID>
        [AssociationEndBehavior(PersistencyFlag.ReferentialIntegrity)]

        [PersistentMember(nameof(_Class))]
        [BackwardCompatibilityID("+5")]
        public MenuModel.IClass Class
        {
            get
            {
                return _Class.Value;
            }
            set
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _Class.Value = value;
                    stateTransition.Consistent = true;
                }
            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.Member<MenuModel.IMenuItemType> _DedicatedType = new OOAdvantech.Member<IMenuItemType>();

        /// <MetaDataID>{229addbe-f4e0-4876-97fb-656278b75af8}</MetaDataID>
        [BackwardCompatibilityID("+6")]
        [PersistentMember(nameof(_DedicatedType))]
        public MenuModel.IMenuItemType DedicatedType
        {
            get
            {
                return _DedicatedType.Value;
            }

            set
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _DedicatedType.Value = value;
                    stateTransition.Consistent = true;
                }
            }
        }

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{a30d55fa-c164-42a6-9630-37ad9de9c896}</MetaDataID>
        Set<IMenuItemPrice> _Prices = new Set<IMenuItemPrice>();

        /// <MetaDataID>{ae8f4cb7-40e5-4b06-a1b6-f06faa44dbb7}</MetaDataID>
        [PersistentMember(nameof(_Prices))]
        [BackwardCompatibilityID("+9")]
        [AssociationEndBehavior(PersistencyFlag.CascadeDelete)]
        public IList<IMenuItemPrice> Prices
        {
            get
            {
                return _Prices.ToThreadSafeList();
            }
        }

        /// <MetaDataID>{b7c34717-cfff-4037-95aa-054127d842e7}</MetaDataID>
        public MenuModel.IMenuItemPrice MenuItemPrice
        {
            get
            {
                var menuItemPrice = (from aMenuItemPrice in Prices.OfType<MenuItemPrice>()
                                     where aMenuItemPrice.ItemSelector != null && !aMenuItemPrice.ItemSelector.GetInitialFor(this).UncheckOption
                                     select aMenuItemPrice).FirstOrDefault();

                if (menuItemPrice == null)
                    menuItemPrice = (from aMenuItemPrice in Prices.OfType<MenuItemPrice>()
                                     where aMenuItemPrice.ItemSelector == null
                                     select aMenuItemPrice).FirstOrDefault();

                if (menuItemPrice == null)
                {

                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        menuItemPrice = new MenuItemPrice();
                        menuItemPrice.Name = "MenuItemDefault";
                        if (OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this) != null)
                            OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(menuItemPrice);
                        AddMenuItemPrice(menuItemPrice);
                        menuItemPrice.Price = 0;

                        stateTransition.Consistent = true;
                    }
                }
                return menuItemPrice;
            }
        }

        /// <exclude>Excluded</exclude>
        IMenu _Menu;
        /// <MetaDataID>{d879a7b4-78a0-4a2e-af9d-e24d1d94935a}</MetaDataID>
        public IMenu Menu
        {
            get
            {
                if (_Menu == null)
                {

                    var objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this);
                    if (objectStorage == null)
                        return null;
                    OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(objectStorage);
                    IClass rootClass = Class;

                    while (rootClass is IClassified && (rootClass as IClassified).Class != null)
                        rootClass = (rootClass as IClassified).Class;



                    _Menu = (from menu in storage.GetObjectCollection<IMenu>()
                             where menu.RootCategory == rootClass
                             select menu).FirstOrDefault();
                }
                return _Menu;
            }
        }


        /// <exclude>Excluded</exclude>
        Member<ITaxableType> _TaxableType = new Member<ITaxableType>();

        /// <MetaDataID>{ed3807ec-bea1-49ef-a315-032befbcabc0}</MetaDataID>
        [PersistentMember(nameof(_TaxableType))]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [BackwardCompatibilityID("+13")]
        public FinanceFacade.ITaxableType TaxableType
        {
            get
            {
                return _TaxableType.Value;
            }

            set
            {
                if (_TaxableType != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _TaxableType.Value = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <MetaDataID>{01790b89-7392-450d-aaa9-d50c9ee5391c}</MetaDataID>
        public IItemsCategory Category => Class as IItemsCategory;



        /// <exclude>Excluded</exclude>
        Set<IPartofMeal> _PartofMeals;


        /// <MetaDataID>{2b029fda-1ffb-4086-a9a5-2e1a6319e4fb}</MetaDataID>
        [PersistentMember(nameof(_PartofMeals))]
        [BackwardCompatibilityID("+19")]
        public IList<IPartofMeal> PartofMeals
        {
            get
            {
                if(_PartofMeals==null)
                {

                }
                return _PartofMeals.ToThreadSafeList();
            }

        }

        




        ///// <MetaDataID>{9504c26b-fcbf-48da-a90b-a513b58e38cb}</MetaDataID>
        //public IList<IPreparationScaledOption> OptionsMenuItemSpecifics
        //{
        //    get
        //    {
        //        throw new NotImplementedException();
        //    }
        //}


        //public decimal GetPriceFor(IPricedSubject pricedSubject)
        //{
        //    pricedSubject.GetPrice
        //    return pricedSubject.GetPrice(this);

        //    //ICustomazedPrice customazedPrice=(from anCustomazedPrice in PricedSubjects
        //    //                                  where anCustomazedPrice.PricedSubject== pricedSubject
        //    //                                  select anCustomazedPrice).f

        //}

        /// <MetaDataID>{dd8a9ccd-3ceb-4986-bf33-05425819ab2a}</MetaDataID>
        public void AddType(IMenuItemType type)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _Types.Add(type);
                stateTransition.Consistent = true;
            }
        }

        /// <MetaDataID>{ed31ef4c-1d33-4d28-97a7-80f2bf684863}</MetaDataID>
        public void RemoveType(IMenuItemType type)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                foreach (MenuItemPrice menuItemPrice in Prices)
                {
                    if (menuItemPrice.ItemSelector != null && menuItemPrice.ItemSelector.MenuItemType == type)
                        RemoveMenuItemPrice(menuItemPrice);
                }

                foreach (IOptionMenuItemSpecific opionSpecific in OptionsMenuItemSpecifics)
                {
                    if (opionSpecific.Option != null && opionSpecific.Option.MenuItemType == type)
                        RemoveOptionsMenuItemSpecific(opionSpecific);
                }

                _Types.Remove(type);
                stateTransition.Consistent = true;
            }
        }

        /// <MetaDataID>{318a061a-73b3-48b3-8f48-16382e2eee2b}</MetaDataID>
        private void RemoveOptionsMenuItemSpecific(IOptionMenuItemSpecific opionSpecific)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _OptionsMenuItemSpecifics.Remove(opionSpecific);
                stateTransition.Consistent = true;
            }
        }

        /// <MetaDataID>{d7fa8c30-054c-4a0d-8157-3db63026be01}</MetaDataID>
        public void OnCommitObjectState()
        {
            if (_DedicatedType.Value == null)
            {
                _DedicatedType.Value = new MenuItemType(Properties.Resources.ItemDedicatedTypeDefaultName);
                OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(DedicatedType);
            }
        }

        /// <MetaDataID>{5d5d710e-cd9b-466a-a91b-5c26365caed8}</MetaDataID>
        public void OnActivate()
        {
            if (_DedicatedType.Value == null)
            {
                _DedicatedType.Value = new MenuItemType(Properties.Resources.ItemDedicatedTypeDefaultName);
                OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(DedicatedType);
            }
        }

        /// <MetaDataID>{041b9f05-39f5-4d3d-934d-6ffdb41ca572}</MetaDataID>
        public void BeforeCommitObjectState()
        {
        }

        /// <MetaDataID>{68546e78-9f52-42c1-8c34-c187ab483f51}</MetaDataID>
        public void OnDeleting()
        {

        }

        /// <MetaDataID>{b8e48928-982f-473f-b29c-0309d2230d7d}</MetaDataID>
        public void LinkedObjectAdded(object linkedObject, AssociationEnd associationEnd)
        {

        }

        /// <MetaDataID>{f55a61e3-26a7-424f-beae-cdfee6246957}</MetaDataID>
        public void LinkedObjectRemoved(object linkedObject, AssociationEnd associationEnd)
        {

        }



        /// <MetaDataID>{4c9db58f-0090-409c-b949-1e4e0ecb4cda}</MetaDataID>
        public IPartofMeal AddMealType(IMealType mealType, IMealCourseType mealCourseType)
        {
            var partofMeal = this.PartofMeals.Where(x => x.MealType == mealType).FirstOrDefault();
            if (partofMeal != null)
                partofMeal.MealCourseType = mealCourseType;
            else
            {
                partofMeal = new PartofMeal(this, mealType);
                partofMeal.MealCourseType = mealCourseType;

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(partofMeal);
                    this._PartofMeals.Add(partofMeal);
                    stateTransition.Consistent = true;
                }
            }
            return partofMeal;
        }


        /// <MetaDataID>{d1d02287-a906-47ab-b81e-2232c27b3100}</MetaDataID>
        public void RemoveMealType(IPartofMeal partofMeal)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                this._PartofMeals.Remove(partofMeal);
                stateTransition.Consistent = true;
            }

        }
    }
}
