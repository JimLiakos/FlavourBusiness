using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MenuModel;
using OOAdvantech;
using OOAdvantech.Json;

namespace MenuModel.JsonViewModel
{
    /// <MetaDataID>{dcf114f9-d0ef-4301-bec1-273571f309bd}</MetaDataID>
    public class MenuFoodItem : TypedObject, IMenuItem
    {
        /// <MetaDataID>{7ba12cf1-2fc2-4462-9549-e79e0cabac4b}</MetaDataID>
        public MenuFoodItem()
        {

        }

        /// <MetaDataID>{7d896d84-3ce1-4ee7-ab0c-c19232d93533}</MetaDataID>
        public string Uri { get; set; }

#if MenuModel
        /// <MetaDataID>{8a3bf850-f5d0-4b1b-8d5b-c1e50b74e2d3}</MetaDataID>
        public MenuFoodItem(MenuModel.IMenuItem menuItem, Dictionary<object, object> mappedObject)
        {
            Uri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(menuItem).GetPersistentObjectUri(menuItem);
            _Name = new Multilingual(menuItem.MultilingualName);
            _FullName = new Multilingual(menuItem.MultilingualFullName);
            _PromptForDefault = new Multilingual(menuItem.MultilingualPromptForDefault);
            _PromptForCustom = new Multilingual(menuItem.MultilingualPromptForCustom);
            AllowCustom = menuItem.AllowCustom;
            Stepper = menuItem.Stepper;
            Description = menuItem.Description;
            ExtrasDescription = menuItem.ExtrasDescription;


            ISOCurrencySymbol = RegionInfo.CurrentRegion.ISOCurrencySymbol;
            CurrencySymbol = RegionInfo.CurrentRegion.CurrencySymbol;
            Quantity = 1;
            Prices = new List<IMenuItemPrice>();

            mappedObject[menuItem] = this;
            foreach (var menuItemPrice in menuItem.Prices)
                Prices.Add(JsonViewModel.MenuItemPrice.GetMenuItemPrice(menuItemPrice, mappedObject));
            if (menuItem.MenuItemPrice != null)
                MenuItemPrice = mappedObject[menuItem.MenuItemPrice] as IMenuItemPrice;

            ItemOptions = (from menuItemType in menuItem.Types
                           from optionGroup in menuItemType.Options.OfType<MenuModel.IPreparationOptionsGroup>()
                           select new JsonViewModel.OptionGroup().Init(optionGroup, mappedObject)).ToList();

            var unGroupedScaledOptions = (from menuItemType in menuItem.Types
                                          from option in menuItemType.Options.OfType<MenuModel.IPreparationScaledOption>()
                                          select option).ToList();


            if (unGroupedScaledOptions.Count > 0)
                ItemOptions.Add(new JsonViewModel.OptionGroup().Init(unGroupedScaledOptions, mappedObject));


            foreach (var optionSpecific in menuItem.OptionsMenuItemSpecifics)
            {
                if (OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(optionSpecific) != null)
                {
                    OptionMenuItemSpecific optionMenuItemSpecific = new OptionMenuItemSpecific() { Hide = optionSpecific.Hide, InitialLevelIndex = optionSpecific.Option.LevelType.Levels.IndexOf(optionSpecific.InitialLevel), Option = JsonViewModel.Option.GetOption(optionSpecific.Option, mappedObject), MenuItemOptionSpecific = this };
                    optionMenuItemSpecific.Uri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(optionSpecific).GetPersistentObjectUri(optionSpecific);
                    _OptionsMenuItemSpecifics.Add(optionMenuItemSpecific);
                }
            }

            PartofMeals = (from partOfMeal in menuItem.PartofMeals select new PartofMeal(partOfMeal, mappedObject)).OfType<IPartofMeal>().ToList();

        }
#endif


        /// <exclude>Excluded</exclude>
        Multilingual _Name = new Multilingual();

        /// <MetaDataID>{a2dd9055-c206-498b-aebf-4ccf0cd19f21}</MetaDataID>
        [JsonIgnore]
        public string Name
        {
            get
            {
                return _Name.GetValue<string>();
            }
            set
            {
                _Name.SetValue<string>(value);
            }
        }

        /// <exclude>Excluded</exclude>
        Multilingual _FullName = new Multilingual();

        /// <MetaDataID>{0ce5cb2f-42ec-4a14-8983-f7d4d4144227}</MetaDataID>
        [JsonIgnore]
        public string FullName
        {
            get
            {
                return _FullName.GetValue<string>();
            }
            set
            {
                _FullName.SetValue<string>(value);
            }
        }
        /// <MetaDataID>{0117300c-8e0f-4d14-9274-0136d56d1083}</MetaDataID>
        public Multilingual MultilingualName
        {
            get
            {
                return new Multilingual(_Name);
            }
            set
            {
                _Name = value;
            }
        }
        /// <MetaDataID>{dc1df6b0-5184-431d-8ceb-47eb8ac23653}</MetaDataID>
        
        public Multilingual MultilingualFullName
        {
            get
            {
                return new Multilingual(_FullName);
            }
            set
            {
                _FullName = value;
            }
        }

        /// <exclude>Excluded</exclude>
        Multilingual _Description = new Multilingual();
        /// <MetaDataID>{6daa2373-a0b4-4cb4-84b3-1782055dcba0}</MetaDataID>
        [JsonIgnore]
        public string Description { get => _Description.GetValue<string>(); set => _Description.SetValue<string>(value); }

        /// <exclude>Excluded</exclude>
        Multilingual _ExtrasDescription = new Multilingual();

        /// <MetaDataID>{43483056-af33-4c5f-8a04-cb25dffde308}</MetaDataID>
        [JsonIgnore]
        public string ExtrasDescription { get => _ExtrasDescription.GetValue<string>(); set => _ExtrasDescription.SetValue<string>(value); }




        /// <MetaDataID>{5c85c373-a932-4543-9466-96a817d0a23a}</MetaDataID>
        public List<OptionGroup> ItemOptions { set; get; }

        /// <MetaDataID>{7b7e0fa3-d03b-4a63-aae4-218a0d0699a8}</MetaDataID>
        public IList<IMenuItemPrice> Prices { set; get; }
        /// <MetaDataID>{2ed1e1fe-f97f-47cd-8b0d-7c254f2fb503}</MetaDataID>
        public string ISOCurrencySymbol { set; get; }
        /// <MetaDataID>{7f623d9c-e566-46dc-a580-2f046de92f68}</MetaDataID>
        public string CurrencySymbol { set; get; }

        /// <MetaDataID>{807947b0-8e63-4861-a842-5ec7ea1b71ca}</MetaDataID>
        public decimal Quantity { set; get; }


        /// <MetaDataID>{84da19f0-d010-4abf-ab11-9474d1f4c723}</MetaDataID>
        public IMenuItemPrice MenuItemPrice { get; set; }


        /// <MetaDataID>{b1c788d0-fcc8-4712-a322-a3823b356c59}</MetaDataID>
        List<IOptionMenuItemSpecific> _OptionsMenuItemSpecifics = new List<IOptionMenuItemSpecific>();
        /// <MetaDataID>{14cf338a-79fd-48e2-8e8f-cd23a035dca8}</MetaDataID>
        public IList<IOptionMenuItemSpecific> OptionsMenuItemSpecifics
        {
            get
            {
                return _OptionsMenuItemSpecifics;
            }
            set
            {
                if (value == null)
                    _OptionsMenuItemSpecifics = null;
                else
                    _OptionsMenuItemSpecifics = value.ToList();
            }
        }



        /// <MetaDataID>{0489f198-f0eb-44fb-a9b5-7177ead9dc76}</MetaDataID>
        public IMenuItemType DedicatedType { get; set; }


        /// <MetaDataID>{6c0f007c-3f05-4cb0-89d1-6f2d18676392}</MetaDataID>
        public IList<IMenuItemType> Types { get; set; }


        /// <exclude>Excluded</exclude>
        Multilingual _PromptForCustom = new Multilingual();
        [JsonIgnore]
        public string PromptForCustom { get => _PromptForCustom.GetValue<string>(); set => _PromptForCustom.SetValue<string>(value); }

        /// <exclude>Excluded</exclude>
        Multilingual _PromptForDefault = new Multilingual();
        [JsonIgnore]
        public string PromptForDefault { get => _PromptForDefault.GetValue<string>(); set => _PromptForDefault.SetValue<string>(value); }
        public bool AllowCustom { get; set; }

        public Multilingual MultilingualPromptForCustom { get { return new Multilingual(_PromptForCustom); } set { _PromptForCustom = value; } }

        public Multilingual MultilingualPromptForDefault { get { return new Multilingual(_PromptForDefault); } set { _PromptForDefault = value; } }

        public Multilingual MultilingualDescription { get { return new Multilingual(_Description); } set { _Description = value; } }

        public Multilingual MultilingualExtrasDescription { get { return new Multilingual(_ExtrasDescription); } set { _ExtrasDescription = value; } }

        public bool Stepper { get; set; }

        public IList<IPartofMeal> PartofMeals { get; set; }



#if MenuPresentationModel
        public IMenu Menu
        {
            get
            {
                return null;
            }
        }
#endif

        /// <MetaDataID>{9ff3ce56-abb8-4764-b348-521e50355731}</MetaDataID>
        public void AddMenuItemPrice(IMenuItemPrice price)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{be5f91af-7c0f-49bc-b61d-7d26ac78f3b6}</MetaDataID>
        public void RemoveMenuItemPrice(IMenuItemPrice price)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{50857a1b-a156-41ac-b1f2-e7cf40ad50a6}</MetaDataID>
        public void AddType(IMenuItemType type)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{d27bbd26-964b-4618-9e20-9ad22e08dd7d}</MetaDataID>
        public void RemoveType(IMenuItemType type)
        {
            throw new NotImplementedException();
        }

        public void RemoveMealType(IPartofMeal partofMeal)
        {
            throw new NotImplementedException();
        }

        public IPartofMeal AddMealType(IMealType mealType, IMealCourseType mealCourseType)
        {
            throw new NotImplementedException();
        }
    }

    /// <MetaDataID>{f47f8b44-6b40-4ed7-bfea-998bb404d21e}</MetaDataID>
    public class MenuItemPrice : TypedObject, MenuModel.IPricingContext, MenuModel.IMenuItemPrice


    {
        /// <MetaDataID>{225a0d59-fdef-4023-829e-733038de9ca5}</MetaDataID>
        public decimal Price { set; get; }
        /// <MetaDataID>{405a31b0-b653-4f33-98ea-9b7844e7ffe6}</MetaDataID>
        public string Uri { get; private set; }

        /// <MetaDataID>{4c475919-9029-42e1-b0bc-3b633d02dc60}</MetaDataID>
        public ItemSelectorOption ItemSelector { set; get; }

        /// <MetaDataID>{496dc0ea-fde6-4837-a1bb-4f1b7f7d5f83}</MetaDataID>
        public string Name
        {
            get; set;
        }

        /// <MetaDataID>{c3eb981a-ed52-4426-879b-a5cf634c2752}</MetaDataID>
        public IList<ICustomizedPrice> PricedSubjects
        {
            get; set;
        }
        /// <MetaDataID>{d510254b-4a39-417f-af6d-b04fa368397c}</MetaDataID>
        public IMenuItem MenuItem { get; set; }

        /// <MetaDataID>{b740c339-d5f9-4434-b64e-a2d3ce5ab6d6}</MetaDataID>
        public bool IsDefaultPrice { get; set; }


        /// <MetaDataID>{3218842f-3e3d-4168-b54a-7c19406f4b9d}</MetaDataID>
        public IList<ICustomizedPrice> PricingContexts
        {
            get
            {
                return new List<ICustomizedPrice>();
            }
        }

#if MenuModel
        /// <MetaDataID>{891372fe-b019-4b94-bf40-9a4c8c76a8ae}</MetaDataID>
        public static MenuItemPrice GetMenuItemPrice(MenuModel.IMenuItemPrice orgMenutemPrice, Dictionary<object, object> mappedObject)
        {
            if (mappedObject.ContainsKey(orgMenutemPrice))
                return mappedObject[orgMenutemPrice] as MenuItemPrice;
            else
            {
                MenuFoodItem menuItem = mappedObject[orgMenutemPrice.MenuItem] as MenuFoodItem;
                MenuItemPrice menutemPrice = new MenuItemPrice();

                menutemPrice.IsDefaultPrice = orgMenutemPrice.IsDefaultPrice;
                menutemPrice.PricedSubjects = new List<ICustomizedPrice>();
                menutemPrice.MenuItem = menuItem;
                menutemPrice.Price = orgMenutemPrice.Price;
                menutemPrice.Uri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(orgMenutemPrice).GetPersistentObjectUri(orgMenutemPrice);


                mappedObject[orgMenutemPrice] = menutemPrice;
                if (orgMenutemPrice is MenuModel.MenuItemPrice && (orgMenutemPrice as MenuModel.MenuItemPrice).ItemSelector != null)
                    menutemPrice.ItemSelector = ItemSelectorOption.GetItemSelectorOption((orgMenutemPrice as MenuModel.MenuItemPrice).ItemSelector, mappedObject);
                return menutemPrice;
            }
        }
#endif

        /// <MetaDataID>{1a988709-6772-4d56-835d-c168eb03af2f}</MetaDataID>
        public void RemoveCustomazedPrice(ICustomizedPrice customazedPrice)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{405e6c21-fce5-4fcd-82a3-fc9731190f12}</MetaDataID>
        public ICustomizedPrice GetCustomazedPrice(IPricedSubject pricedSubject)
        {
            CustomizedPrice customazedPrice = (from aCustomazedPrice in PricedSubjects.OfType<CustomizedPrice>()
                                               where aCustomazedPrice.PricedSubject == pricedSubject
                                               select aCustomazedPrice).FirstOrDefault();
            if (customazedPrice == null)
            {
                customazedPrice = new CustomizedPrice();
                //option.Uri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(orgOption).GetPersistentObjectUri(orgOption);


                customazedPrice.PricedSubject = pricedSubject;
                customazedPrice.PricingContext = this;

                PricedSubjects.Add(customazedPrice);
                pricedSubject.PricingContexts.Add(customazedPrice);
            }
            return customazedPrice;
        }

        /// <MetaDataID>{ae3debb3-6ca7-4c9a-94cf-43f4bee29857}</MetaDataID>
        public decimal GetPrice(IPricingContext pricicingContext)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{52ed8594-aeb1-40ae-9554-a5e0d4d12b7e}</MetaDataID>
        public void SetPrice(IPricingContext pricicingContext, decimal price)
        {
            throw new NotImplementedException();
        }
    }

}
