using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MenuModel;
using OOAdvantech;
using OOAdvantech.Json;

namespace MenuModel.JsonViewModel
{
    /// <MetaDataID>{2cdd4709-a7f8-4189-82f1-8e5b23765be1}</MetaDataID>
    public class Option : TypedObject, MenuModel.IPricedSubject, IPreparationScaledOption
    {
        /// <MetaDataID>{faf6ccab-5bd4-4bfe-9399-d39b5a1ff2c0}</MetaDataID>
        public bool Quantitative { get; set; }
        /// <MetaDataID>{85a2935f-71d5-4e7a-ae31-e6307c322e1c}</MetaDataID>
        public string Uri { get; set; }



        /// <MetaDataID>{648b2db2-d585-42a3-b63e-0d8fc2fe28a0}</MetaDataID>
        public Option()
        {

        }
        /// <MetaDataID>{9c83e4fd-902e-4f94-b3b5-21d3e31f1193}</MetaDataID>
        public Option(IPreparationScaledOption orgOption)
        {
            PricingContexts = new List<ICustomizedPrice>();
            Price = orgOption.Price;
            _Name = new Multilingual(orgOption.MultilingualName);
            _FullName = new Multilingual(orgOption.MultilingualFullName);
            Quantitative = orgOption.Quantitative;
            Uri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(orgOption).GetPersistentObjectUri(orgOption);
            InitialLevelIndex = orgOption.LevelType.Levels.IndexOf(orgOption.Initial);// (orgOption.GetInitialFor(menuItem));
            IsRecipeIngredient = orgOption.IsRecipeIngredient;
            AutoGenFullName = orgOption.AutoGenFullName;
        }


        /// <exclude>Excluded</exclude>
        protected Multilingual _FullName = new Multilingual();

        public string FullName
        {
            get
            {
                if (this.AutoGenFullName)
                    return this.OptionGroup.Name + " " + this.Name;


                if (this.MultilingualFullName.GetValue<string>() != null)
                    return this.MultilingualFullName.GetValue<string>();

                return this.Name;


            }
            set
            {
                _FullName.SetValue<string>(value);
            }
        }




        /// <exclude>Excluded</exclude>
        protected Multilingual _Name = new Multilingual();

        /// <MetaDataID>{06636afc-de3b-4373-866c-79c2530762a7}</MetaDataID>
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


        /// <MetaDataID>{1490d197-9796-4f82-90fc-1b02a04b454d}</MetaDataID>
        public ScaleType LevelType { get; set; }
        /// <MetaDataID>{8e88e3b7-0e36-4d84-9452-5a3010a5554d}</MetaDataID>
        public int InitialLevelIndex { get; set; }

        /// <MetaDataID>{5285b47a-cd80-4fa3-9d96-c9663b1659c1}</MetaDataID>
        public decimal Price { get; set; }

        /// <MetaDataID>{1343ffa4-0a3f-4c7e-94be-692da1ed858d}</MetaDataID>
        public IList<ICustomizedPrice> PricingContexts
        {
            get; set;
        }

        /// <MetaDataID>{eed72e16-f833-466e-a0e1-eb1e3a4c7325}</MetaDataID>
        public IList<IOptionMenuItemSpecific> MenuItemsOptionSpecific { get; set; }


        /// <MetaDataID>{451b6d71-80ba-4807-8b6b-d7d187de69cd}</MetaDataID>
        public IPreparationOptionsGroup OptionGroup { get; set; }



        /// <MetaDataID>{0016f43d-4e55-4f35-91e5-60cab26e4864}</MetaDataID>
        IScaleType IPreparationScaledOption.LevelType { get => this.LevelType; set => this.LevelType = value as ScaleType; }


        /// <MetaDataID>{65eedb29-3847-489a-84d0-af08d65bd652}</MetaDataID>
        public ILevel Initial
        {
            get
            {
                return LevelType.Levels[InitialLevelIndex];
            }
            set
            {
            }
        }


        /// <MetaDataID>{c01963e7-a5cc-4190-ad2a-aa3a21934dda}</MetaDataID>
        public IMenuItemType Owner { get; set; }

        /// <MetaDataID>{f0a2d654-90a6-4ebf-bc58-24e04c16f86a}</MetaDataID>
        public IMenuItemType MenuItemType { get; set; }

        /// <MetaDataID>{651a67ee-b6d4-42fb-b895-17310c10916e}</MetaDataID>
        public Multilingual MultilingualName { get => new Multilingual(_Name); set { } }

        public Multilingual MultilingualFullName { get => new Multilingual(_FullName); set { _FullName = value; } }

        /// <MetaDataID>{93085f95-6a95-4064-aaaf-95037afa97d2}</MetaDataID>
        public bool IsRecipeIngredient { get; set; }
        public bool AutoGenFullName { get; set; }


#if MenuModel

        /// <MetaDataID>{95ceda69-6d7b-4335-9623-e3b58ecd6b46}</MetaDataID>
        internal static Option GetOption(MenuModel.IPreparationScaledOption orgOption, Dictionary<object, object> mappedObject)//, IMenuItem menuItem)
        {
            if (mappedObject.ContainsKey(orgOption))
            {
                //if (orgOption is MenuModel.ItemSelectorOption)
                //{
                //    JsonViewModel.ItemSelectorOption itemSelectorOption = mappedObject[orgOption] as JsonViewModel.ItemSelectorOption;
                //    foreach (var orgMenuItemPriceEntry in (orgOption as MenuModel.ItemSelectorOption).MenuItemPrices)
                //    {
                //        JsonViewModel.MenuItemPrice menuItemPrice = null;
                //        if (mappedObject.ContainsKey(orgMenuItemPriceEntry.Key) && mappedObject.ContainsKey(orgMenuItemPriceEntry.Value))
                //        {
                //            menuItemPrice = mappedObject[orgMenuItemPriceEntry.Value] as JsonViewModel.MenuItemPrice;
                //            itemSelectorOption.MenuItemPrices.Add(menuItemPrice);
                //        }
                //    }
                //    itemSelectorOption.InitialLevelIndex = orgOption.LevelType.Levels.IndexOf(orgOption.Initial);// (orgOption.GetInitialFor(menuItem));
                //    return itemSelectorOption;
                //}
                //else if (orgOption is MenuModel.IPreparationScaledOption)
                //{
                //    JsonViewModel.Option option = mappedObject[orgOption] as Option;
                //    option.InitialLevelIndex = orgOption.LevelType.Levels.IndexOf(orgOption.Initial);// orgOption.GetInitialFor(menuItem));
                //    foreach (var customPrice in orgOption.PricingContexts)
                //    {

                //    }
                //    return option;
                //}
                //else
                return mappedObject[orgOption] as Option;
            }
            else
            {
                Option option = new Option(orgOption);
                mappedObject[orgOption] = option;

                option.LevelType = ScaleType.GetScaleTypeFor(orgOption.LevelType, mappedObject);


                foreach (var customPrice in orgOption.PricingContexts)
                {
                    if (mappedObject.ContainsKey(customPrice.PricingContext))
                    {
                        IPricingContext jsonPricingContext = mappedObject[customPrice.PricingContext] as IPricingContext;
                        IPricedSubject jsonPricedSubject = option;
                        ICustomizedPrice customazedPrice = jsonPricingContext.GetCustomazedPrice(jsonPricedSubject);
                        customazedPrice.Price = customPrice.Price;
                    }
                    else
                    {

                    }
                }

                if (orgOption is MenuModel.ItemSelectorOption)
                {
                    ItemSelectorOption itemSelectorOption = mappedObject[orgOption] as JsonViewModel.ItemSelectorOption;
                    foreach (var orgMenuItemPriceEntry in (orgOption as MenuModel.ItemSelectorOption).MenuItemPrices)
                    {
                        JsonViewModel.MenuItemPrice menuItemPrice = null;
                        if (mappedObject.ContainsKey(orgMenuItemPriceEntry.Key) && mappedObject.ContainsKey(orgMenuItemPriceEntry.Value))
                        {
                            menuItemPrice = mappedObject[orgMenuItemPriceEntry.Value] as JsonViewModel.MenuItemPrice;
                            itemSelectorOption.MenuItemPrices.Add(menuItemPrice);
                        }
                    }
                }


                return option;
            }
        }

#endif
        /// <MetaDataID>{c90a6cc7-9444-4397-bb48-6edb777e6a4e}</MetaDataID>
        public decimal GetPrice(IPricingContext pricicingContext)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{572a252a-3086-413a-9780-c188d79e561e}</MetaDataID>
        public void SetPrice(IPricingContext pricicingContext, decimal price)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{0a5747c2-0826-4bf2-99ab-350aefca58ab}</MetaDataID>
        public void RemoveCustomazedPrice(ICustomizedPrice customazedPrice)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{4f31586f-46c9-4cc0-9295-bf46a5efa3e2}</MetaDataID>
        public IOptionMenuItemSpecific GetMenuItemSpecific(IMenuItem menuItem)
        {
            return null;
        }

        /// <MetaDataID>{e1fba8ba-8592-4e58-8382-e3f9b6b2c5e0}</MetaDataID>
        public ILevel GetInitialFor(IMenuItem menuItem)
        {
            if (menuItem != null)
            {
                var optionsMenuItemSpecific = menuItem.OptionsMenuItemSpecifics.Where(x => x.Option == this).FirstOrDefault();
                if (optionsMenuItemSpecific != null)
                    return optionsMenuItemSpecific.InitialLevel;
                return Initial;
            }
            return Initial;
        }

        /// <MetaDataID>{3d564627-92cb-462f-b506-6cf382838c23}</MetaDataID>
        public void SetInitialFor(IMenuItem menuItem, ILevel initialLevel)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{85705750-b740-4aae-8e46-dcc99ced470d}</MetaDataID>
        public bool IsHiddenFor(IMenuItem menuItem)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{fa8afcbb-8b98-4686-9d6e-b2ecbe71ebc9}</MetaDataID>
        public void SetHiddenFor(IMenuItem menuItem, bool value)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{ff3cb718-369b-401c-a158-b0ec7fd28ee4}</MetaDataID>
        public void RemoveOptionSpecificFor(IMenuItem menuItem)
        {
            throw new NotImplementedException();
        }
    }


    /// <MetaDataID>{a84587dd-3413-410a-af29-73797fda7dd7}</MetaDataID>
    public class ScaleType : IScaleType
    {
        /// <MetaDataID>{276bfbdf-2b7d-4384-8e94-93a601b3a0db}</MetaDataID>
        public string Name { get; set; }

        /// <MetaDataID>{b20efda3-97e6-4d63-a9c3-e5910754738b}</MetaDataID>
        static Dictionary<MenuModel.IScaleType, JsonViewModel.ScaleType> jsonScaleTypeDictionary = new Dictionary<MenuModel.IScaleType, ScaleType>();

        /// <MetaDataID>{ec4241d6-ef9e-4b4b-99aa-49e6abf4ce1e}</MetaDataID>
        public ScaleType()
        {
            Levels = new List<Level>();
        }


        [JsonConstructor]
        public ScaleType(string name, string uri, List<Level> levels, bool zeroLevelScaleType)
        {
            Name = name;
            Uri = uri;
            Levels = levels;
            ZeroLevelScaleType = zeroLevelScaleType;
            foreach (var level in Levels)
                level.DeclaringType = this;
        }
        /// <MetaDataID>{5ffef0da-8b59-4475-a47e-4c318f07a7e9}</MetaDataID>
        public ScaleType(MenuModel.IScaleType scaleType)
        {
            Levels = (from level in scaleType.Levels select new Level(level)).ToList();
            Uri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(scaleType).GetPersistentObjectUri(scaleType);
            ZeroLevelScaleType = scaleType.ZeroLevelScaleType;
        }
        /// <MetaDataID>{35980699-9af5-4f30-be75-c3626799c741}</MetaDataID>
        public List<Level> Levels { get; set; }// = new List<Level>();
        /// <MetaDataID>{c1b82140-b937-4ce1-b040-6e738581a045}</MetaDataID>
        public string Uri { get; set; }
        public bool ZeroLevelScaleType { get; set; }

        IList<ILevel> IScaleType.Levels => this.Levels.OfType<ILevel>().ToList();

        /// <MetaDataID>{dbb658eb-e0a2-4eb3-bc64-d4194db6d8c4}</MetaDataID>
        internal static ScaleType GetScaleTypeFor(MenuModel.IScaleType scaleType)
        {
            ScaleType jsonScaleType = null;
            if (!jsonScaleTypeDictionary.TryGetValue(scaleType, out jsonScaleType))
            {
                jsonScaleType = new ScaleType(scaleType);
                jsonScaleTypeDictionary[scaleType] = jsonScaleType;
            }
            return jsonScaleType;
        }

        /// <MetaDataID>{0c5f5dfd-a4fd-4f82-ad00-7b76f7471750}</MetaDataID>
        internal static ScaleType GetScaleTypeFor(MenuModel.IScaleType scaleType, Dictionary<object, object> mappedObject)
        {
            ScaleType jsonScaleType = null;

            if (!mappedObject.ContainsKey(scaleType))
            {
                jsonScaleType = new ScaleType(scaleType);
                mappedObject[scaleType] = jsonScaleType;
                return jsonScaleType;
            }
            else
                return mappedObject[scaleType] as ScaleType;

        }

        public void InsertLevel(int index, ILevel level)
        {
            throw new NotImplementedException();
        }

        public void MoveLevel(ILevel level, int newpos)
        {
            throw new NotImplementedException();
        }

        public void AddLevel(ILevel level)
        {
            throw new NotImplementedException();
        }

        public void RemoveLevel(ILevel level)
        {
            throw new NotImplementedException();
        }
    }

    /// <MetaDataID>{07988325-98d9-4392-9135-01a0aee1af00}</MetaDataID>
    public class Level : ILevel
    {

        public Level(ILevel level)
        {
            _Name = level.MultilingualName;
            UncheckOption = level.UncheckOption;
            PriceFactor = level.PriceFactor;
            Uri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(level).GetPersistentObjectUri(level);

        }
        public Level()
        {
        }
        public double PriceFactor { get; set; }
        public Multilingual MultilingualName { get => new Multilingual(_Name); set { } }


        /// <exclude>Excluded</exclude>
        protected Multilingual _Name = new Multilingual();

        /// <MetaDataID>{37620c73-4bf5-4280-a40d-67fb7ef64614}</MetaDataID>
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

        /// <MetaDataID>{57632ed0-748c-4e96-b3f8-0f4d5e3cb607}</MetaDataID>
        public bool UncheckOption { get; set; }
        /// <MetaDataID>{c96e920d-19bf-4397-9485-ca425fea1488}</MetaDataID>
        public string Uri { get; set; }

        public IScaleType DeclaringType { get; set; }
    }
}
