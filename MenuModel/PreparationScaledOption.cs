using System;
using OOAdvantech.Collections.Generic;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using System.Linq;
using System.Collections.Generic;
using OOAdvantech;
using OOAdvantech.Remoting.RestApi.Serialization;

namespace MenuModel
{
    /// <MetaDataID>{0b1833a7-4119-4b54-846a-5923a17a2797}</MetaDataID>
    [BackwardCompatibilityID("{0b1833a7-4119-4b54-846a-5923a17a2797}")]
    [Persistent()]
    public class PreparationScaledOption : MarshalByRefObject, IPreparationScaledOption
    {
      

        /// <exclude>Excluded</exclude>
        bool _AutoGenFullName;

        /// <MetaDataID>{2d56972b-08d4-4e89-90ef-cff92b0d0cc4}</MetaDataID>
        [PersistentMember(nameof(_AutoGenFullName))]
        [BackwardCompatibilityID("+13")]
        public bool AutoGenFullName
        {
            get => _AutoGenFullName;
            set
            {
                if (_AutoGenFullName != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _AutoGenFullName = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }



        /// <exclude>Excluded</exclude>
        OOAdvantech.MultilingualMember<string> _FullName = new OOAdvantech.MultilingualMember<string>();

        /// <MetaDataID>{541c04ff-c65e-4367-bc8c-db15d73da30f}</MetaDataID>
        [PersistentMember(nameof(_FullName))]
        [BackwardCompatibilityID("+11")]
        public string FullName
        {
            get
            {
                if (!_FullName.HasValue)
                {
                    if (OptionGroup != null && _AutoGenFullName)
                        return OptionGroup.Name + " " + Name;
                    else
                        return Name;
                }

                return _FullName;
            }
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

        /// <MetaDataID>{a4169a45-c020-4916-ad01-3c3c871a20da}</MetaDataID>
        [BackwardCompatibilityID("+12")]
        public Multilingual MultilingualFullName => new Multilingual(_FullName);

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{e125245d-fd5d-4e6e-9751-96f965187fa4}</MetaDataID>
        bool _IsRecipeIngredient;
        /// <MetaDataID>{b00f531e-188e-4f50-9e3c-d02029efab66}</MetaDataID>
        [PersistentMember(nameof(_IsRecipeIngredient))]
        [BackwardCompatibilityID("+10")]
        public bool IsRecipeIngredient
        {
            get => _IsRecipeIngredient;
            set
            {
                if (_IsRecipeIngredient != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _IsRecipeIngredient = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;



        /// <MetaDataID>{f1bf3fdc-3d63-48f3-846b-fb9ae2dcf5b6}</MetaDataID>
        public PreparationScaledOption()
        {

        }

        /// <MetaDataID>{b30abbb5-771c-4d2d-8ee1-058f041ebc1f}</MetaDataID>
        public PreparationScaledOption(string name)
        {
            _Name.Value = name;
        }

        /// <MetaDataID>{3a3fd8ea-96f6-48bd-94a2-221053ec5ede}</MetaDataID>
        public PreparationScaledOption(IPreparationScaledOption preparationScaledOption, IMenuItemType menuItemType)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                this._Name = new MultilingualMember<string>(preparationScaledOption.MultilingualName.Values);
                this._FullName = new MultilingualMember<string>(preparationScaledOption.MultilingualFullName.Values);
                this._AutoGenFullName = preparationScaledOption.AutoGenFullName;
                this._IsRecipeIngredient = preparationScaledOption.IsRecipeIngredient;
                //this._Owner = menuItemType;
                this._Price = preparationScaledOption.Price;
                this._Quantitative = preparationScaledOption.Quantitative;

                OOAdvantech.PersistenceLayer.ObjectStorage objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(menuItemType);
                if (preparationScaledOption.LevelType is MenuModel.JsonViewModel.ScaleType)
                {
                    if ((preparationScaledOption.LevelType as MenuModel.JsonViewModel.ScaleType).Uri.IndexOf(objectStorage.StorageMetaData.StorageIdentity) == 0)
                    {
                        _LevelType = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri<IScaleType>((preparationScaledOption.LevelType as MenuModel.JsonViewModel.ScaleType).Uri);
                        _Initial = _LevelType.Levels[preparationScaledOption.LevelType.Levels.IndexOf(preparationScaledOption.Initial)];
                    }
                }
                objectStorage.CommitTransientObjectState(this);
                stateTransition.Consistent = true;
            }

        }


        /// <MetaDataID>{ad04d56a-77a8-4f0b-b074-5d1ccc3133af}</MetaDataID>
        public Multilingual MultilingualName => new Multilingual(_Name);

        /// <exclude>Excluded</exclude>
        OOAdvantech.MultilingualMember<string> _Name = new OOAdvantech.MultilingualMember<string>();
        /// <MetaDataID>{531e20cb-27d8-4415-82ff-67414a33d736}</MetaDataID>
        [PersistentMember("_Name")]
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
                        _Name.Value = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        IScaleType _LevelType;

        /// <MetaDataID>{abfda70e-8d28-4407-a296-0230e69eab21}</MetaDataID>
        [PersistentMember("_LevelType")]
        [BackwardCompatibilityID("+2")]
        public IScaleType LevelType
        {
            get
            {
                if (_LevelType != null && Initial == null && _LevelType.Levels.Count > 0)
                    Initial = _LevelType.Levels[0];

                return _LevelType;
            }
            set
            {
                if (_LevelType != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        Initial = null;
                        _LevelType = value;

                        if (_LevelType != null && _LevelType.Levels.Count > 0)
                            Initial = _LevelType.Levels[0];
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <exclude>Excluded</exclude>
        MenuModel.ILevel _Initial;

        /// <MetaDataID>{9ef035f4-4bc5-4da3-9f4d-772dc9b1c824}</MetaDataID>
        [PersistentMember("_Initial")]
        [BackwardCompatibilityID("+1")]
        public MenuModel.ILevel Initial
        {
            get
            {
                return _Initial;
            }
            set
            {
                if (_Initial != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {

                        _Initial = value;
                        stateTransition.Consistent = true;
                    }

                    //ObjectStateChanged?.Invoke(this, nameof(Initial));
                }
            }
        }


        /// <exclude>Excluded</exclude>
        Set<ICustomizedPrice> _PricingContexts = new Set<ICustomizedPrice>();

        /// <MetaDataID>{fd49bbb1-91af-4b00-a1e2-97601e12f729}</MetaDataID>
        [PersistentMember("_PricingContexts")]
        [BackwardCompatibilityID("+5")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        public IList<MenuModel.ICustomizedPrice> PricingContexts
        {
            get
            {
                return _PricingContexts.AsReadOnly();
            }
        }



        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{ca35ac14-e34f-4841-a778-37d1852f1c6f}</MetaDataID>
        decimal _Price;

        /// <MetaDataID>{bb0325c9-7f61-4949-8b25-b3543b3f0db3}</MetaDataID>
        [PersistentMember("_Price")]
        public virtual decimal Price
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
        MenuModel.IPreparationOptionsGroup _OptionGroup;

        /// <MetaDataID>{eecae3c8-b1b5-4be6-9965-bb1e9c000179}</MetaDataID>
        [PersistentMember("_OptionGroup")]
        [BackwardCompatibilityID("+6")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        public MenuModel.IPreparationOptionsGroup OptionGroup
        {
            get
            {
                return _OptionGroup;
            }
        }

        /// <exclude>Excluded</exclude>
        MenuModel.IMenuItemType _Owner;


        /// <MetaDataID>{fd6a880d-4a8a-4776-a6b0-ff9c406799c2}</MetaDataID>
        [PersistentMember("_Owner")]
        [BackwardCompatibilityID("+7")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        public MenuModel.IMenuItemType Owner
        {
            get
            {
                return _Owner;
            }
        }


        /// <MetaDataID>{58f40ec5-d62f-4543-a476-6da52b848405}</MetaDataID>
        public IMenuItemType MenuItemType
        {
            get
            {
                if (OptionGroup != null)
                    return OptionGroup.MenuItemType;
                else
                    return Owner;
            }
        }

        /// <exclude>Excluded</exclude>
        Set<IOptionMenuItemSpecific> _MenuItemsOptionSpecific = new Set<IOptionMenuItemSpecific>();


        /// <MetaDataID>{c1f78a78-9843-495c-8f43-d05a093e4056}</MetaDataID>
        [PersistentMember(nameof(_MenuItemsOptionSpecific))]
        [BackwardCompatibilityID("+8")]
        public IList<IOptionMenuItemSpecific> MenuItemsOptionSpecific
        {
            get
            {
                return _MenuItemsOptionSpecific.AsReadOnly();
            }
        }

        /// <exclude>Excluded</exclude>
        bool? _Quantitative;

        /// <MetaDataID>{c8db0158-5bf4-4bfc-976c-40894d7e9bc7}</MetaDataID>
        [PersistentMember(nameof(_Quantitative))]
        [BackwardCompatibilityID("+9")]
        public bool Quantitative
        {
            get
            {
                if (_Quantitative == null)
                    _Quantitative = LevelType.ZeroLevelScaleType;

                return _Quantitative.Value;
            }

            set
            {
                if (_Quantitative != value)
                {

                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Quantitative = value;
                        stateTransition.Consistent = true;
                    }

                }
            }
        }

      



        /// <MetaDataID>{9ee65ddc-efa8-440f-a2d5-1207e091705c}</MetaDataID>
        public decimal GetPrice(IPricingContext pricicingContext)
        {
            if (pricicingContext != null)
            {
                ICustomizedPrice customazedPrice = (from ancustomazedPrice in _PricingContexts
                                                    where ancustomazedPrice.PricingContext == pricicingContext
                                                    select ancustomazedPrice).FirstOrDefault();
                if (customazedPrice != null)
                    return customazedPrice.Price;
            }

            return Price;
        }

        /// <MetaDataID>{ec231c6b-4cb5-4c9a-9386-d6bdfd7849ef}</MetaDataID>
        public void SetPrice(IPricingContext pricicingContext, decimal price)
        {
            ICustomizedPrice customazedPrice = (from ancustomazedPrice in _PricingContexts
                                                where ancustomazedPrice.PricingContext == pricicingContext
                                                select ancustomazedPrice).FirstOrDefault();

            if (customazedPrice != null)
                customazedPrice.Price = price;
            else
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    customazedPrice = new CustomizedPrice(pricicingContext, this);
                    OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(customazedPrice);
                    customazedPrice.Price = price;
                    _PricingContexts.Add(customazedPrice);
                    stateTransition.Consistent = true;
                }

            }
            //Price = price;
        }

        /// <MetaDataID>{47116b57-4e26-4628-b03f-038a866bf828}</MetaDataID>
        public void RemoveCustomazedPrice(ICustomizedPrice customazedPrice)
        {
            //using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            //{
            //    _PricingContexts.Remove(customazedPrice);
            //    stateTransition.Consistent = true;
            //}
        }

        /// <MetaDataID>{4c5bf749-4bb3-415e-b698-9ad136271225}</MetaDataID>
        public IOptionMenuItemSpecific GetMenuItemSpecific(IMenuItem menuItem)
        {
            if (menuItem == null)
                return null;
            return (from menuItemSpecific in MenuItemsOptionSpecific
                    where menuItemSpecific.MenuItemOptionSpecific == menuItem
                    select menuItemSpecific).FirstOrDefault();
        }

        /// <MetaDataID>{6b4fa5b2-ad0a-43fe-8fe0-a4c0de8ba383}</MetaDataID>
        public ILevel GetInitialFor(IMenuItem menuItem)
        {
            if (menuItem == null)
                return Initial;
            IOptionMenuItemSpecific optionMenuItemSpecific = GetMenuItemSpecific(menuItem);
            if (optionMenuItemSpecific != null && optionMenuItemSpecific.MenuItemOptionSpecific == menuItem && optionMenuItemSpecific.InitialLevel != null)
                return optionMenuItemSpecific.InitialLevel;
            return Initial;
        }

        /// <MetaDataID>{79ded298-8e83-47ab-96d0-0ffb983fef40}</MetaDataID>
        public void SetInitialFor(IMenuItem menuItem, ILevel initialLevel)
        {
            if (menuItem != null)
            {
                IOptionMenuItemSpecific optionMenuItemSpecific = GetMenuItemSpecific(menuItem);
                if (optionMenuItemSpecific == null)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        optionMenuItemSpecific = new OptionMenuItemSpecific(menuItem, this);
                        OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(optionMenuItemSpecific);
                        _MenuItemsOptionSpecific.Add(optionMenuItemSpecific);
                        stateTransition.Consistent = true;
                    }
                }
                optionMenuItemSpecific.InitialLevel = initialLevel;
            }
            else
                Initial = initialLevel;
        }

        /// <MetaDataID>{8bdbf291-53af-4eb9-b017-9025287fe627}</MetaDataID>
        public void RemoveOptionSpecificFor(IMenuItem menuItem)
        {
            IOptionMenuItemSpecific optionMenuItemSpecific = GetMenuItemSpecific(menuItem);
            if (optionMenuItemSpecific != null)
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _MenuItemsOptionSpecific.Remove(optionMenuItemSpecific);
                    OOAdvantech.PersistenceLayer.ObjectStorage.DeleteObject(optionMenuItemSpecific);
                    stateTransition.Consistent = true;
                }
            }
        }

        /// <MetaDataID>{51fdd790-7cd5-47ec-acff-83edcc70cff8}</MetaDataID>
        public bool IsHiddenFor(IMenuItem menuItem)
        {
            if (menuItem == null)
                return false;

            IOptionMenuItemSpecific optionMenuItemSpecific = GetMenuItemSpecific(menuItem);
            if (optionMenuItemSpecific != null && optionMenuItemSpecific.MenuItemOptionSpecific == menuItem)
                return optionMenuItemSpecific.Hide;

            return false;
        }



        /// <MetaDataID>{494ff5b0-4290-4047-bb51-86b9f7c4062f}</MetaDataID>
        public void SetHiddenFor(IMenuItem menuItem, bool value)
        {
            if (menuItem != null)
            {
                IOptionMenuItemSpecific optionMenuItemSpecific = GetMenuItemSpecific(menuItem);
                if (optionMenuItemSpecific == null && value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        optionMenuItemSpecific = new OptionMenuItemSpecific(menuItem, this);
                        OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(optionMenuItemSpecific);
                        _MenuItemsOptionSpecific.Add(optionMenuItemSpecific);
                        stateTransition.Consistent = true;
                    }
                }
                if (optionMenuItemSpecific != null)
                    optionMenuItemSpecific.Hide = value;

                if (optionMenuItemSpecific != null && !optionMenuItemSpecific.Hide && optionMenuItemSpecific.InitialLevel == null)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _MenuItemsOptionSpecific.Remove(optionMenuItemSpecific);
                        OOAdvantech.PersistenceLayer.ObjectStorage.DeleteObject(optionMenuItemSpecific);
                        stateTransition.Consistent = true;
                    }
                }

            }
        }


        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<ITag> _PreparationTags = new OOAdvantech.Collections.Generic.Set<ITag>();

        /// <MetaDataID>{bb132a8f-c130-4efe-8b5d-2d5e7176c80c}</MetaDataID>
        [PersistentMember(nameof(_PreparationTags))]
        [BackwardCompatibilityID("+15")]
        public System.Collections.Generic.List<MenuModel.ITag> PreparationTags => _PreparationTags.ToThreadSafeList();


        /// <MetaDataID>{fcd7f32e-028f-488f-a7d4-231d967ca139}</MetaDataID>
        public void RemovePreparationTag(ITag tag)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _PreparationTags.Remove(tag);
                stateTransition.Consistent = true;
            }
        }


      
        /// <MetaDataID>{38a33b37-5cc4-4f58-b495-221d4009f2ce}</MetaDataID>
        public ITag NewPreparationTag()
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                var tag = new Tag();
                tag.Name = "new Tag";
                OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(tag);
                _PreparationTags.Add(tag);
                stateTransition.Consistent = true;
                return tag;
            }

        }

    }
}